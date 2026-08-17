using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace InventoryAndSales.Business
{
  /// <summary>
  /// The little text file in the cloud that says what the current release is.
  ///
  /// Deliberately the simplest thing that can be edited by hand once per release — one
  /// <c>Key: value</c> per line, unknown keys ignored, blank lines and <c>#</c> comments allowed:
  ///
  /// <code>
  /// Version: 1.0.1.4
  /// Drive:   https://drive.google.com/drive/folders/1MUM...
  /// File:    https://drive.google.com/file/d/FILEID/view
  /// Notes:   Perbaikan laporan
  /// </code>
  ///
  /// <c>Drive</c> is the folder a person opens. <c>File</c> is the release archive the application
  /// downloads; without it there is nothing to install unattended and the update becomes a
  /// notification. Both accept an ordinary Google Drive sharing link — turning one into something a
  /// program can actually fetch is <see cref="ToDirectDownloadUrl"/>'s job.
  /// </summary>
  public class UpdateManifest
  {
    private UpdateManifest(Version version, string driveUrl, string fileUrl, string notes)
    {
      Version = version;
      DriveUrl = driveUrl;
      FileUrl = fileUrl;
      Notes = notes;
    }

    /// <summary>The released version, or null when the file did not say.</summary>
    public Version Version { get; private set; }

    /// <summary>Folder link for a person to open. May be empty.</summary>
    public string DriveUrl { get; private set; }

    /// <summary>Direct link to the release archive. Empty when the release cannot be installed unattended.</summary>
    public string FileUrl { get; private set; }

    public string Notes { get; private set; }

    /// <summary>True when there is an archive to download and install.</summary>
    public bool CanInstall
    {
      get { return Version != null && !string.IsNullOrEmpty(FileUrl); }
    }

    /// <summary>
    /// Reads the manifest. Never throws: a malformed file means "no update known", which is how a
    /// mistyped release must behave — the till keeps running on what it has.
    /// </summary>
    public static UpdateManifest Parse(string content)
    {
      Version version = null;
      string drive = string.Empty;
      string file = string.Empty;
      string notes = string.Empty;

      foreach (string rawLine in (content ?? string.Empty).Split('\n'))
      {
        string line = rawLine.Trim();
        if (line.Length == 0 || line.StartsWith("#", StringComparison.Ordinal))
          continue;

        int separator = line.IndexOf(':');
        if (separator <= 0)
          continue;

        string key = line.Substring(0, separator).Trim();
        string value = line.Substring(separator + 1).Trim();

        if (Is(key, "Version"))
        {
          Version parsed;
          if (Version.TryParse(value, out parsed))
            version = parsed;
        }
        else if (Is(key, "Drive"))
          drive = value;
        else if (Is(key, "File"))
          file = value;
        else if (Is(key, "Notes"))
          notes = value;
      }

      return new UpdateManifest(version, drive, ToDirectDownloadUrl(file), notes);
    }

    private static bool Is(string key, string name)
    {
      return string.Equals(key, name, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Turns a Google Drive sharing link into one that returns the file itself.
    ///
    /// A link copied out of Drive points at a viewer page, so fetching it returns HTML rather than
    /// the release. Drive will serve the bytes from <c>uc?export=download&amp;id=…</c>, so the file
    /// id is lifted out of whichever link shape was pasted in. A folder link has no file id and is
    /// left alone — a folder cannot be downloaded this way, and the caller treats that as "tell the
    /// operator where to look" rather than pretending it can install.
    ///
    /// Anything that is not a Drive link is returned untouched, so a plain web server, a share or
    /// any other host works without special handling.
    /// </summary>
    public static string ToDirectDownloadUrl(string url)
    {
      if (string.IsNullOrEmpty(url))
        return string.Empty;

      string trimmed = url.Trim();
      if (trimmed.IndexOf("drive.google.com", StringComparison.OrdinalIgnoreCase) < 0)
        return trimmed;

      if (trimmed.IndexOf("/folders/", StringComparison.OrdinalIgnoreCase) >= 0)
        return trimmed;

      // Already a direct link.
      if (trimmed.IndexOf("uc?", StringComparison.OrdinalIgnoreCase) >= 0)
        return trimmed;

      string id = FileId(trimmed);
      if (string.IsNullOrEmpty(id))
        return trimmed;

      return string.Format(CultureInfo.InvariantCulture,
                           "https://drive.google.com/uc?export=download&id={0}", id);
    }

    private static string FileId(string url)
    {
      Match path = Regex.Match(url, @"/file/d/([A-Za-z0-9_\-]+)");
      if (path.Success)
        return path.Groups[1].Value;

      Match query = Regex.Match(url, @"[?&]id=([A-Za-z0-9_\-]+)");
      return query.Success ? query.Groups[1].Value : string.Empty;
    }
  }
}
