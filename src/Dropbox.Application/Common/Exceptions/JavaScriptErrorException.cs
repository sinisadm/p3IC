using System;
using System.Runtime.Serialization;

namespace Dropbox.Application.Common.Exceptions
{
    [Serializable]
    public class JavaScriptErrorException : Exception
    {
        public JavaScriptErrorException()
        {
        }

        public JavaScriptErrorException(string message) : base(message)
        {
        }

        public JavaScriptErrorException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected JavaScriptErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
