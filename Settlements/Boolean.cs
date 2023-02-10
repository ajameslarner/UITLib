using Microsoft.VisualBasic.FileIO;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace UITestingFramework.Utilities
{
    public static class Boolean
    {
        public static bool BeFalse<T>(this A<T> r) where T : IEquatable<bool>
        {
            return r.Value.Equals(false);
        }
        public static bool BeTrue<T>(this A<T> r) where T : IEquatable<bool>
        {
            return r.Value.Equals(true);
        }
    }
}