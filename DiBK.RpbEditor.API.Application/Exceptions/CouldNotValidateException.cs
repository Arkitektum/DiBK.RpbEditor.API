using System;

namespace DiBK.RpbEditor.Application.Exceptions
{
    public class CouldNotValidateException : Exception
    {
        public CouldNotValidateException()
        {
        }

        public CouldNotValidateException(string message) : base(message)
        {
        }
    }
}
