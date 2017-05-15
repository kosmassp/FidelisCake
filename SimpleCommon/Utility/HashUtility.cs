using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SimpleCommon.Utility
{
  public class HashUtility
  {
    public static string GetEncryptedPass(string password)
    {
      SHA512 hashCreator = SHA512.Create();
      hashCreator.ComputeHash(Encoding.UTF8.GetBytes(password));
      return BitConverter.ToString(hashCreator.Hash);
    }

  }
}
