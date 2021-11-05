using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace project.api.Core.CustomExceptions
{
    public class JobException : Exception
    {
        public JobException()
        {
        }

        public JobException(string message) : base(message)
        {
        }

        public JobException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected JobException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
