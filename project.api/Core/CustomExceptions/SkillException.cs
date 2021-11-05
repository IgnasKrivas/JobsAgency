using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace project.api.Core.CustomExceptions
{
    public class SkillException : Exception
    {
        public SkillException()
        {
        }

        public SkillException(string message) : base(message)
        {
        }

        public SkillException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SkillException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
