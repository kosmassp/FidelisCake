using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCommon.Model
{
  public interface ISearchable
  {
    string ToSearchableString();
    string[] ToDisplayKeys();
    string[] ToDisplayValues();
  }
}
