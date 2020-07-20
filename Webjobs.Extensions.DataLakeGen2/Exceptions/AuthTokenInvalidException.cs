using System;
using System.Collections.Generic;
using System.Text;

namespace WebJobs.Extensions.DataLakeGen2.Exceptions
{
    public class AuthTokenInvalidException : Exception
    {
        public AuthTokenInvalidException() : base() { }
        public AuthTokenInvalidException(string message) : base(message) { }
        public AuthTokenInvalidException(string message, Exception innerException) : base(message, innerException) { }
    }
}
