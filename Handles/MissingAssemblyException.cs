using System;

namespace UITestingFramework.Core
{
    internal class MissingAssemblyException : Exception
    {
        public MissingAssemblyException()
        {
        }

        public MissingAssemblyException(string message) : base(message)
        {
        }

        public MissingAssemblyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}