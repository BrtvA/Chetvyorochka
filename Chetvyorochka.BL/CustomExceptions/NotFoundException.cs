using System.Runtime.Serialization;

namespace Chetvyorochka.BL.CustomExceptions
{
    public class NotFoundException: ApplicationException
    {
        public NotFoundException() { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, Exception inner) : base(message, inner) { }

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
