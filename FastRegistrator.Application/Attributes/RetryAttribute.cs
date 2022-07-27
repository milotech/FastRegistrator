using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastRegistrator.ApplicationCore.Attributes
{
    public class RetryAttribute: Attribute
    {
        public int MaxRetries { get; }

        public RetryAttribute(int maxRetriesCount)
        {
            MaxRetries = maxRetriesCount;
        }
    }
}
