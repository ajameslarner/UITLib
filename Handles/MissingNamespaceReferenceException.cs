using System;

namespace UITestingFramework
{
    /// <summary>
    /// Custom exception for missing class namespace reference; see SequenceTest.Using()
    /// </summary>
    public class MissingNamespaceReferenceException : Exception
    {
        /// <summary>
        /// Missing control exception constructor.
        /// </summary>
        public MissingNamespaceReferenceException() { }
        /// <summary>
        /// Missing control exception constructor.
        /// </summary>
        public MissingNamespaceReferenceException(string message) : base(message) { }
        /// <summary>
        /// Missing control exception constructor.
        /// </summary>
        public MissingNamespaceReferenceException(string message, Exception inner) : base(message, inner) { }
    }
}
