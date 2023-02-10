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
    public class A<T>
    {
        internal readonly T Value;

        public A(T val)
        {
            Value = val;
        }
    }
}