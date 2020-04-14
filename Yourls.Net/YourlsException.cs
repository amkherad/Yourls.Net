using System;
using System.Runtime.Serialization;

namespace Yourls.Net
{
    public class YourlsException : Exception
    {
        public YourlsException()
        {
        }

        protected YourlsException(
            SerializationInfo info,
            StreamingContext context
        ) : base(info, context)
        {
        }

        public YourlsException(
            string message
        ) : base(message)
        {
        }

        public YourlsException(
            string message,
            Exception innerException
        ) : base(message, innerException)
        {
        }
    }
}