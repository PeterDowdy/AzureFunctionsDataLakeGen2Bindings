using System;

namespace WebJobs.Extensions.DataLakeGen2.Exceptions
{
    public class AdlOperationTimedOutException : Exception
    {
        public AdlOperationTimedOutException() : base() { }
        public AdlOperationTimedOutException(string message) : base(message) { }
        public AdlOperationTimedOutException(string message, Exception innerException) : base(message, innerException) { }
    }
}
