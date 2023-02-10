using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UITestingFramework.Utilities
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class UITestMethod : Attribute
    {
        public object[] Data { get; private set; }
        public UITestMethod()
        {
            Data = new object[0];
        }
        public UITestMethod(object data)
        {
            Data = new object[1] { data };
        }
    }
}