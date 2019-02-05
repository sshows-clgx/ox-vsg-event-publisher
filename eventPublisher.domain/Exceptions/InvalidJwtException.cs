using System;

namespace eventPublisher.domain.exceptions
{
    public class InvalidJwtException : Exception
	{
		public InvalidJwtException()
        {
        }

        public InvalidJwtException(string message)
            : base(message)
        {
        }

        public InvalidJwtException(string message, Exception inner)
            : base(message, inner)
        {
        }
	}
}