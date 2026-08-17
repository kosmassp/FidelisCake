namespace InventoryAndSales.Business
{
  /// <summary>
  /// What a report column holds. Worked out from the values by <see cref="ReportTable"/>, and used
  /// by whatever renders the report to decide how the column should read - amounts right aligned,
  /// dates kept on one line, free text left to wrap.
  ///
  /// The kinds stop at what the presentation actually needs to distinguish; this is not a type
  /// system for report data.
  /// </summary>
  public enum ReportColumnKind
  {
    Text,
    Number,
    Date
  }
}
