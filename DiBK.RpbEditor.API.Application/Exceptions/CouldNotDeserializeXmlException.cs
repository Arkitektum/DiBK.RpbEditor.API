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
    }
}
