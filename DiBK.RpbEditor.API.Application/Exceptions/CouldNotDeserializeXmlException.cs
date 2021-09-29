using System;

namespace DiBK.RpbEditor.Application.Exceptions
{
    public class CouldNotDeserializeXmlException : Exception
    {
        public CouldNotDeserializeXmlException()
        {
        }

        public CouldNotDeserializeXmlException(string message) : base(message)
        {
        }

        public CouldNotDeserializeXmlException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
