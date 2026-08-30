using System;
using System.Collections.Generic;
using System.Globalization;

namespace InventoryAndSales.Business
{
  /// <summary>
  /// A report's rows worked out into something that can be laid out: what each column holds, how
  /// each value should read, and what the columns add up to.
  ///
  /// The reports arrive as dictionaries of already-stringified database values, so an amount reaches
  /// here as "100000.0000" and a count as "3". Presenting either of those raw is why a generated
  /// report used to be hard to read. Rather than teach every query about its own formatting, the
  /// shape is worked out once, here, from the values themselves.
  /// </summary>
  public class ReportTable
  {
    /// <summary>Money keeps two decimals; a count has none. Both are grouped with thousands.</summary>
    private const string MoneyFormat = "#,##0.00";
    private const string CountFormat = "#,##0";

    /// <summary>
    /// The shapes <c>CustomQuery</c> writes a DateTime in. Matching one is what makes a column a date
    /// rather than free text.
    /// </summary>
    private static readonly string[] DateFormats = { "dd MMM yyyy", "dd MMM yyyy HH:mm:ss" };

    /// <summary>
    /// Headings that mark a per-unit or averaged figure. Adding these up produces a number that
    /// means nothing, so they are formatted like money but left out of the totals row.
    /// </summary>
    private static readonly string[] NotAdditive = { "Satuan", "Rata-rata" };

    private ReportTable(string[] headers, List<string[]> rows, ReportColumnKind[] columnKinds,
                        string[] totals, bool hasTotals)
    {
      Headers = headers;
      Rows = rows;
      ColumnKinds = columnKinds;
      Totals = totals;
      HasTotals = hasTotals;
    }

    public string[] Headers { get; private set; }

    /// <summary>One entry per row, in <see cref="Headers"/> order, formatted for display.</summary>
    public List<string[]> Rows { get; private set; }

    /// <summary>What each column holds, so a renderer can align and wrap it sensibly.</summary>
    public ReportColumnKind[] ColumnKinds { get; private set; }

    /// <summary>Per column: the formatted column total, or empty where a total means nothing.</summary>
    public string[] Totals { get; private set; }

    /// <summary>False when no column could be totalled, in which case there is no footer to draw.</summary>
    public bool HasTotals { get; private set; }

    public int RowCount
    {
      get { return Rows.Count; }
    }

    public int ColumnCount
    {
      get { return Headers.Length; }
    }

    /// <summary>
    /// Reads a report result set. An empty result gives an empty table rather than null, so callers
    /// that only want to say "no data" do not have to check twice.
    /// </summary>
    public static ReportTable From(List<Dictionary<string, string>> reportRows)
    {
      if (reportRows == null || reportRows.Count == 0)
        return new ReportTable(new string[0], new List<string[]>(), new ReportColumnKind[0], new string[0], false);

      string[] headers = new List<string>(reportRows[0].Keys).ToArray();
      int columns = headers.Length;

      // Read by heading rather than by position: a row that somehow lacks a column then loses that
      // one cell instead of shifting every cell after it into the wrong column.
      List<string[]> rawRows = new List<string[]>(reportRows.Count);
      foreach (Dictionary<string, string> reportRow in reportRows)
      {
        string[] values = new string[columns];
        for (int i = 0; i < columns; i++)
        {
          string value;
          values[i] = reportRow.TryGetValue(headers[i], out value) ? (value ?? string.Empty) : string.Empty;
        }
        rawRows.Add(values);
      }

      ReportColumnKind[] kinds = new ReportColumnKind[columns];
      bool[] fractional = new bool[columns];
      for (int i = 0; i < columns; i++)
        kinds[i] = ClassifyColumn(rawRows, i, out fractional[i]);

      string[] totals = new string[columns];
      bool hasTotals = false;
      for (int i = 0; i < columns; i++)
      {
        totals[i] = string.Empty;
        if (kinds[i] != ReportColumnKind.Number || !IsAdditive(headers[i]))
          continue;

        decimal sum = 0;
        foreach (string[] row in rawRows)
        {
          decimal value;
          if (TryParse(row[i], out value))
            sum += value;
        }
        totals[i] = Format(sum, fractional[i]);
        hasTotals = true;
      }

      List<string[]> rows = new List<string[]>(rawRows.Count);
      foreach (string[] rawRow in rawRows)
      {
        string[] row = new string[columns];
        for (int i = 0; i < columns; i++)
        {
          row[i] = kinds[i] == ReportColumnKind.Number
                     ? FormatCell(rawRow[i], fractional[i])
                     : rawRow[i];
        }
        rows.Add(row);
      }

      return new ReportTable(headers, rows, kinds, totals, hasTotals);
    }

    /// <summary>
    /// A column takes a kind only when every value in it agrees, so a column of invoice numbers is
    /// never mistaken for an amount and a stray note never turns a date column into free text by
    /// halves.
    /// </summary>
    /// <param name="fractional">
    /// True when a value carried a decimal point, which is how an amount is told apart from a count:
    /// the database hands back "100000.0000" for money and "3" for a quantity.
    /// </param>
    private static ReportColumnKind ClassifyColumn(List<string[]> rows, int column, out bool fractional)
    {
      fractional = false;
      bool numeric = true;
      bool date = true;
      bool sawValue = false;

      foreach (string[] row in rows)
      {
        string raw = row[column];
        if (string.IsNullOrEmpty(raw))
          continue;
        sawValue = true;

        decimal parsed;
        if (numeric && TryParse(raw, out parsed))
        {
          if (raw.IndexOf('.') >= 0)
            fractional = true;
        }
        else
        {
          numeric = false;
        }

        if (date && !IsDate(raw))
          date = false;

        if (!numeric && !date)
          break;
      }

      if (!sawValue)
        return ReportColumnKind.Text;
      if (numeric)
        return ReportColumnKind.Number;
      if (date)
        return ReportColumnKind.Date;

      fractional = false;
      return ReportColumnKind.Text;
    }

    private static bool IsDate(string raw)
    {
      DateTime parsed;
      return DateTime.TryParseExact(raw, DateFormats, CultureInfo.InvariantCulture,
                                    DateTimeStyles.None, out parsed);
    }

    private static bool IsAdditive(string header)
    {
      foreach (string marker in NotAdditive)
      {
        if ((header ?? string.Empty).IndexOf(marker, StringComparison.OrdinalIgnoreCase) >= 0)
          return false;
      }
      return true;
    }

    private static string FormatCell(string raw, bool fractional)
    {
      decimal value;
      if (string.IsNullOrEmpty(raw) || !TryParse(raw, out value))
        return raw ?? string.Empty;
      return Format(value, fractional);
    }

    // Invariant both ways: the values are produced by the database layer, never typed by an
    // operator, and the application pins en-US anyway. Nothing here may depend on the machine.
    private static bool TryParse(string raw, out decimal value)
    {
      return decimal.TryParse(raw, NumberStyles.Float | NumberStyles.AllowThousands,
                              CultureInfo.InvariantCulture, out value);
    }

    private static string Format(decimal value, bool fractional)
    {
      return value.ToString(fractional ? MoneyFormat : CountFormat, CultureInfo.InvariantCulture);
    }
  }
}
