using System;

namespace UITestingFramework
{
    public class UITestFailedException : Exception
    {
        public UITestFailedException() { }
        public UITestFailedException(string message) : base(message) { }
        public UITestFailedException(string message, Exception inner) : base(message, inner) { }
    }
}
