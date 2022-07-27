using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.ApplicationCore.Exceptions
{
    public class RetryRequiredException : Exception
    {
        public RetryRequiredException(string message) 
            : base(message) 
        { }

        public RetryRequiredException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
