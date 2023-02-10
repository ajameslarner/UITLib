using Microsoft.VisualBasic.FileIO;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace UITestingFramework.Utilities
{
    public static class Nullity
    {
        public static bool BeNull<T>(this A<T> a)
        {
            return a.Value == null;
        }
        public static bool NotBeNull<T>(this A<T> a)
        {
            return a.Value != null;
        }
    }
}