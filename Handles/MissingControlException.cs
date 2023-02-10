using System;

namespace UITestingFramework
{
    /// <summary>
    /// Custom exception for missing controls.
    /// </summary>
    public class MissingControlException : Exception
    {
        /// <summary>
        /// Missing control exception constructor.
        /// </summary>
        public MissingControlException() { }
        /// <summary>
        /// Missing control exception constructor.
        /// </summary>
        public MissingControlException(string message) : base(message) { }
        /// <summary>
        /// Missing control exception constructor.
        /// </summary>
        public MissingControlException(string message, Exception inner) : base(message, inner) { }
    }
}
