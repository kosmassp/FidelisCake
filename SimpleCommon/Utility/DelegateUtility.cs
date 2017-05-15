using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleCommon.Utility
{
  public class DelegateUtility
  {
    public delegate void VoidHandler();
    public delegate void OneValueHandler<T>(T value);
    public delegate void TwoValueHandler<T1, T2>(T1 value1, T2 value2);
    public delegate void ThreeValueHandler<T1, T2, T3>(T1 value1, T2 value2, T3 value3);
    public delegate void FourValueHandler<T1, T2, T3, T4>(T1 value1, T2 value2, T3 value3, T4 value4);
    public delegate void OneValueArrayHandler<T>(T[] value);
    public delegate void TwoValueArrayHandler<T1, T2>(T1[] value1, T2[] value2);
    public delegate void ThreeValueArrayHandler<T1, T2, T3>(T1[] value1, T2[] value2, T3[] value3);
    public delegate void FourValueArrayHandler<T1, T2, T3, T4>(T1[] value1, T2[] value2, T3[] value3, T4[] value4);

  }
}
