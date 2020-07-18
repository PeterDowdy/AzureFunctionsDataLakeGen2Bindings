using System;

namespace WebJobs.Extensions.DataLakeGen2.Exceptions
{
    public class AdlUnexpectedException : Exception
    {
        public AdlUnexpectedException() : base() { }
        public AdlUnexpectedException(string message) : base(message) { }
        public AdlUnexpectedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
