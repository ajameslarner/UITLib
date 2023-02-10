using Microsoft.VisualBasic.FileIO;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace UITestingFramework.Utilities
{
    public static class Equality
    {
        public static bool Be<T>(this A<T> x, T y)
        {
            return x.Value.Equals(y);
        }
        public static bool NotBe<T>(this A<T> x, T y)
        {
            return !x.Value.Equals(y);
        }
        public static bool Equal<T>(this A<T> x, T y)
        {
            return x.Value.Equals(y);
        }
        public static bool NotEqual<T>(this A<T> x, T y)
        {
            return !x.Value.Equals(y);
        }
    }
}