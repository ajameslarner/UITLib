using System;

namespace UITestingFramework
{
    /// <summary>
    /// Custom exception for missing properties.
    /// </summary>
    public class MissingPropertyException : Exception
    {
        /// <summary>
        /// Missing property exception constructor.
        /// </summary>
        public MissingPropertyException() { }
        /// <summary>
        /// Missing property exception constructor.
        /// </summary>
        public MissingPropertyException(string message) : base(message) { }
        /// <summary>
        /// Missing property exception constructor.
        /// </summary>
        public MissingPropertyException(string message, Exception inner) : base(message, inner) { }
    }
}
