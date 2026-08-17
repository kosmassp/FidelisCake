using System;
using System.Security.Cryptography;
using System.Text;

namespace SimpleCommon.Utility
{
  /// <summary>
  /// Password hashing with a per-user salt, plus verification of the unsalted SHA-512 hashes older
  /// databases already contain.
  ///
  /// Upgrade path: <see cref="Verify"/> accepts either format, and <see cref="NeedsUpgrade"/> tells
  /// the caller when a stored hash is still in the old format so it can be replaced silently after a
  /// successful sign-in. Nobody has to change their password, and no installation needs a data
  /// migration before it can run this build.
  ///
  /// Format written by <see cref="Hash"/>:  PBKDF2$&lt;iterations&gt;$&lt;salt&gt;$&lt;hash&gt;
  /// with salt and hash Base64 encoded - 90 characters or so, well inside M_USERS.Password
  /// varchar(256).
  /// </summary>
  public static class PasswordHasher
  {
    private const string Prefix = "PBKDF2";
    private const char Separator = '$';
    private const int SaltBytes = 16;
    private const int HashBytes = 32;

    /// <summary>
    /// Cost factor, chosen for the hardware this actually runs on.
    ///
    /// Measured at roughly 175 ms on a modern desktop, so expect a few hundred milliseconds on the
    /// older machines in the shops. That is charged on every sign-in and on every supervisor
    /// approval, which happen often enough that a second-long delay would be felt. Public guidance
    /// suggests far higher counts, but those target internet-facing services; here the database is
    /// local to a single till, and the move from one unsalted SHA-512 round to a salted, iterated
    /// hash is already the change that matters.
    ///
    /// Raising this later is safe: the value is stored inside each hash, so passwords created at the
    /// old cost keep verifying, and each one is rewritten at the new cost the next time its owner
    /// signs in.
    /// </summary>
    private const int DefaultIterations = 25000;

    public static string Hash(string password)
    {
      byte[] salt = new byte[SaltBytes];
      using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
      {
        rng.GetBytes(salt);
      }
      byte[] hash = Derive(password, salt, DefaultIterations, HashBytes);
      return string.Join(Separator.ToString(),
        Prefix,
        DefaultIterations.ToString(System.Globalization.CultureInfo.InvariantCulture),
        Convert.ToBase64String(salt),
        Convert.ToBase64String(hash));
    }

    /// <summary>
    /// True when the stored hash was produced by an older build and should be replaced once the
    /// password has been confirmed.
    /// </summary>
    public static bool NeedsUpgrade(string storedHash)
    {
      return !IsNewFormat(storedHash);
    }

    /// <summary>
    /// Checks a plain password against a stored hash in either format. Returns false rather than
    /// throwing for malformed or empty input.
    /// </summary>
    public static bool Verify(string password, string storedHash)
    {
      if (string.IsNullOrEmpty(storedHash))
        return false;

      if (!IsNewFormat(storedHash))
        return FixedTimeEquals(
          Encoding.UTF8.GetBytes(HashUtility.GetEncryptedPass(password)),
          Encoding.UTF8.GetBytes(storedHash));

      string[] parts = storedHash.Split(Separator);
      if (parts.Length != 4)
        return false;

      int iterations;
      if (!int.TryParse(parts[1], System.Globalization.NumberStyles.Integer,
            System.Globalization.CultureInfo.InvariantCulture, out iterations) || iterations <= 0)
        return false;

      byte[] salt;
      byte[] expected;
      try
      {
        salt = Convert.FromBase64String(parts[2]);
        expected = Convert.FromBase64String(parts[3]);
      }
      catch (FormatException)
      {
        return false;
      }
      if (salt.Length == 0 || expected.Length == 0)
        return false;

      byte[] actual = Derive(password, salt, iterations, expected.Length);
      return FixedTimeEquals(actual, expected);
    }

    private static bool IsNewFormat(string storedHash)
    {
      return storedHash != null
             && storedHash.StartsWith(Prefix + Separator, StringComparison.Ordinal);
    }

    private static byte[] Derive(string password, byte[] salt, int iterations, int length)
    {
      // .NET Framework 4.6 only offers the PBKDF2-HMAC-SHA1 constructor. That is still salted and
      // iterated, which is what the old single-round unsalted SHA-512 was missing.
      using (Rfc2898DeriveBytes derive = new Rfc2898DeriveBytes(password ?? string.Empty, salt, iterations))
      {
        return derive.GetBytes(length);
      }
    }

    /// <summary>Comparison whose duration does not depend on where the first difference is.</summary>
    private static bool FixedTimeEquals(byte[] left, byte[] right)
    {
      if (left == null || right == null || left.Length != right.Length)
        return false;
      int difference = 0;
      for (int i = 0; i < left.Length; i++)
        difference |= left[i] ^ right[i];
      return difference == 0;
    }
  }
}
