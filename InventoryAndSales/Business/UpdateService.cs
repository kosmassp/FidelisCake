using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace InventoryAndSales.Business
{
  /// <summary>
  /// Finds out whether a newer release exists and gets it ready to install.
  ///
  /// This half only ever writes outside the installation folder — it downloads into a working folder
  /// under the user's local application data and unpacks it there. Nothing in the install folder is
  /// touched until <see cref="InventoryAndSales.Utility.UpdateInstaller"/> runs, in a second process,
  /// after this one has exited.
  ///
  /// Everything here is best effort. A shop with no internet, a mistyped link, a Drive file that was
  /// never shared — none of that may cost the shop its till, so failures are logged and reported as
  /// "no update", never thrown at the operator mid-sale.
  /// </summary>
  public class UpdateService
  {
    private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    /// <summary>The application's own executable, and what a staged release must therefore contain.</summary>
    public const string ApplicationExecutable = "InventoryAndSales.exe";

    /// <summary>Long enough for a slow shop connection, short enough not to hang a startup check.</summary>
    private const int RequestTimeoutMs = 30000;

    /// <summary>A release archive far larger than this is a wrong link, not a release.</summary>
    private const long MaxArchiveBytes = 200L * 1024 * 1024;

    private readonly SettingsService _settings;

    public UpdateService(SettingsService settings)
    {
      _settings = settings;
    }

    /// <summary>The version this installation is running.</summary>
    public static Version CurrentVersion
    {
      get
      {
        Version version = Assembly.GetExecutingAssembly().GetName().Version;
        return version ?? new Version(0, 0, 0, 0);
      }
    }

    /// <summary>Where the manifest lives. Empty switches the whole feature off.</summary>
    public string GetManifestUrl()
    {
      return (_settings.GetString(SettingKeys.UpdateManifestUrl, string.Empty) ?? string.Empty).Trim();
    }

    public void SetManifestUrl(string url)
    {
      _settings.SetString(SettingKeys.UpdateManifestUrl, (url ?? string.Empty).Trim());
    }

    public bool IsConfigured
    {
      get { return GetManifestUrl().Length > 0; }
    }

    /// <summary>
    /// Reads the manifest. Returns null when it is not configured, cannot be reached, or does not
    /// parse — all of which mean the same thing to the caller: carry on as you are.
    /// </summary>
    public UpdateManifest FetchManifest()
    {
      string url = GetManifestUrl();
      if (url.Length == 0)
      {
        _log.Debug("Update check skipped: no manifest URL configured.");
        return null;
      }

      try
      {
        string direct = UpdateManifest.ToDirectDownloadUrl(url);
        _log.InfoFormat("Checking for updates at {0}", direct);

        string content;
        using (WebClient client = CreateClient())
          content = client.DownloadString(direct);

        UpdateManifest manifest = UpdateManifest.Parse(content);
        if (manifest.Version == null)
        {
          _log.WarnFormat("Update manifest at {0} has no readable Version line.", direct);
          return null;
        }

        _log.InfoFormat("Update manifest reports version {0} (running {1}).", manifest.Version, CurrentVersion);
        return manifest;
      }
      catch (Exception e)
      {
        _log.Warn(string.Format("Could not read the update manifest at '{0}'.", url), e);
        return null;
      }
    }

    public static bool IsNewer(UpdateManifest manifest)
    {
      return manifest != null && manifest.Version != null && manifest.Version > CurrentVersion;
    }

    /// <summary>
    /// Downloads and unpacks the release, leaving it ready for the installer.
    /// </summary>
    /// <returns>
    /// The folder holding the new files, or empty with <paramref name="problem"/> set. An Indonesian
    /// message, because it is shown to whoever is standing at the till.
    /// </returns>
    public string StageUpdate(UpdateManifest manifest, out string problem)
    {
      problem = string.Empty;
      if (manifest == null || !manifest.CanInstall)
      {
        problem = "Berkas pembaruan belum tersedia. Silahkan unduh manual dari folder pembaruan.";
        return string.Empty;
      }

      string workingDirectory = Path.Combine(WorkingRoot, manifest.Version.ToString());
      try
      {
        // Started from empty every time: a half-finished download from a previous attempt must not
        // be mistaken for a complete release.
        if (Directory.Exists(workingDirectory))
          Directory.Delete(workingDirectory, true);
        Directory.CreateDirectory(workingDirectory);

        string archive = Path.Combine(workingDirectory, "update.zip");
        _log.InfoFormat("Downloading update {0} from {1}", manifest.Version, manifest.FileUrl);
        using (WebClient client = CreateClient())
          client.DownloadFile(manifest.FileUrl, archive);

        FileInfo downloaded = new FileInfo(archive);
        if (!downloaded.Exists || downloaded.Length == 0)
        {
          problem = "Berkas pembaruan gagal diunduh.";
          return string.Empty;
        }
        if (downloaded.Length > MaxArchiveBytes)
        {
          problem = "Berkas pembaruan terlalu besar. Periksa kembali tautan pada berkas versi.";
          return string.Empty;
        }

        // Verified before anything is unpacked: bytes that do not match the manifest are not a
        // release and must not even be opened.
        if (!ChecksumAccepted(manifest, archive, out problem))
          return string.Empty;

        // A Drive link that was never shared publicly answers with a sign-in page instead of the
        // file. It downloads perfectly happily as HTML, so the archive is what proves it is real.
        string staging = Path.Combine(workingDirectory, "staging");
        Directory.CreateDirectory(staging);
        try
        {
          ZipFile.ExtractToDirectory(archive, staging);
        }
        catch (Exception e)
        {
          _log.Error("The downloaded update is not a readable archive.", e);
          problem = "Berkas pembaruan tidak dapat dibuka. Pastikan tautan mengarah ke berkas ZIP " +
                    "dan berkas tersebut dapat diakses publik.";
          return string.Empty;
        }

        string payload = FindPayloadRoot(staging);
        if (payload.Length == 0)
        {
          problem = "Berkas pembaruan tidak berisi aplikasi. Pastikan ZIP berisi " + ApplicationExecutable + ".";
          return string.Empty;
        }

        _log.InfoFormat("Update {0} staged at {1}", manifest.Version, payload);
        return payload;
      }
      catch (Exception e)
      {
        _log.Error(string.Format("Could not prepare update {0}.", manifest.Version), e);
        problem = "Pembaruan gagal disiapkan. Periksa koneksi internet lalu coba lagi.";
        return string.Empty;
      }
    }

    /// <summary>
    /// True when the archive is what the manifest promised. A manifest without a Sha256 line skips
    /// the check — older releases never published one and must keep installing. A manifest with one
    /// is a commitment: any mismatch, including a mistyped line, refuses the install, because "the
    /// bytes are not what the release said" has no safe reading.
    /// </summary>
    private static bool ChecksumAccepted(UpdateManifest manifest, string archive, out string problem)
    {
      problem = string.Empty;
      string expected = (manifest.Sha256 ?? string.Empty).Trim().ToLowerInvariant();
      if (expected.Length == 0)
        return true;

      string actual = ComputeSha256(archive);
      if (string.Equals(expected, actual, StringComparison.Ordinal))
        return true;

      _log.ErrorFormat("Update archive rejected: manifest says SHA-256 {0} but the download is {1}.",
                       expected, actual);
      problem = "Berkas pembaruan tidak cocok dengan tanda SHA-256 pada berkas versi, sehingga " +
                "tidak dipasang. Coba lagi; bila terus gagal, periksa berkas versi.";
      return false;
    }

    private static string ComputeSha256(string path)
    {
      using (SHA256 sha = SHA256.Create())
      using (FileStream stream = File.OpenRead(path))
      {
        byte[] hash = sha.ComputeHash(stream);
        StringBuilder hex = new StringBuilder(hash.Length * 2);
        foreach (byte b in hash)
          hex.Append(b.ToString("x2", CultureInfo.InvariantCulture));
        return hex.ToString();
      }
    }

    /// <summary>
    /// Where the application actually is, which is what the installer overwrites.
    /// </summary>
    public static string InstallDirectory
    {
      get { return AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar); }
    }

    /// <summary>
    /// Downloads and unpacking happen here, outside the installation, so a failed attempt leaves the
    /// working copy untouched and needs no permissions the till does not already have.
    /// </summary>
    public static string WorkingRoot
    {
      get
      {
        return Path.Combine(
          Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
          "FidelisCake", "Update");
      }
    }

    /// <summary>
    /// The folder inside the archive that actually holds the application. Release archives are
    /// zipped both ways — files at the root, or wrapped in a single folder — so both are accepted
    /// rather than made into a packaging rule somebody has to remember every release.
    /// </summary>
    private static string FindPayloadRoot(string staging)
    {
      if (File.Exists(Path.Combine(staging, ApplicationExecutable)))
        return staging;

      foreach (string child in Directory.GetDirectories(staging))
      {
        if (File.Exists(Path.Combine(child, ApplicationExecutable)))
          return child;
      }
      return string.Empty;
    }

    private static WebClient CreateClient()
    {
      EnableModernTls();
      TimedWebClient client = new TimedWebClient(RequestTimeoutMs);
      // Drive refuses some requests that do not look like a browser.
      client.Headers.Add(HttpRequestHeader.UserAgent, "FidelisCake/" + CurrentVersion);
      client.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
      return client;
    }

    /// <summary>
    /// .NET Framework 4.6 negotiates whatever the machine's defaults allow, which on an older shop
    /// PC can still be TLS 1.0. Google refuses that outright, so the check would fail on exactly the
    /// machines that most need updating. Added rather than assigned, so nothing already enabled is
    /// switched off.
    /// </summary>
    private static void EnableModernTls()
    {
      try
      {
        ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
      }
      catch (NotSupportedException e)
      {
        _log.Warn("This machine does not support TLS 1.2; the update check may fail.", e);
      }
    }

    /// <summary>A WebClient that gives up rather than hanging a startup check for two minutes.</summary>
    private class TimedWebClient : WebClient
    {
      private readonly int _timeoutMs;

      public TimedWebClient(int timeoutMs)
      {
        _timeoutMs = timeoutMs;
      }

      protected override WebRequest GetWebRequest(Uri address)
      {
        WebRequest request = base.GetWebRequest(address);
        if (request != null)
          request.Timeout = _timeoutMs;
        HttpWebRequest http = request as HttpWebRequest;
        if (http != null)
        {
          http.ReadWriteTimeout = _timeoutMs;
          // Drive answers a sharing link with a redirect to the file itself.
          http.AllowAutoRedirect = true;
        }
        return request;
      }
    }
  }
}
