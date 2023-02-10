using Microsoft.VisualBasic.FileIO;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace UITestingFramework.Utilities
{
    public static class Collection
    {
        public static bool Contain<T>(this A<T> a, T b) where T : ICollection<T>
        {
            foreach (var i in a.Value)
                if (Equals(i, b))
                    return true;

            return false;
        }
        public static bool NotContain<T>(this A<T> a, T b) where T : ICollection<T>
        {
            foreach (var i in a.Value)
                if (Equals(i, b))
                    return false;

            return true;
        }
        public static bool NotBeEmpty<T>(this A<T> a) where T : ICollection<T>
        {
            return a.Value.Count > 0;
        }
        public static bool BeEmpty<T>(this A<T> a) where T : ICollection<T>
        {
            return a.Value.Count == 0;
        }
    }
}