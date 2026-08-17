using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SimpleCommon.Utility
{
  public class HashUtility
  {
    /// <summary>
    /// The original password hash: a single unsalted SHA-512 rendered as dash separated hex.
    ///
    /// Still needed because deployed databases are full of passwords in this format and operators
    /// must keep signing in without being asked to change anything. <see cref="PasswordHasher"/>
    /// verifies these and quietly replaces them on the next successful sign-in. Do not use this for
    /// new passwords.
    /// </summary>
    public static string GetEncryptedPass(string password)
    {
      using (SHA512 hashCreator = SHA512.Create())
      {
        byte[] hash = hashCreator.ComputeHash(Encoding.UTF8.GetBytes(password ?? string.Empty));
        return BitConverter.ToString(hash);
      }
    }
  }
}
