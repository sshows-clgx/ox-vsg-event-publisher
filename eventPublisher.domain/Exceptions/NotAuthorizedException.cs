using System;

namespace eventPublisher.domain.exceptions
{
    public class NotAuthorizedException : Exception
	{
		public NotAuthorizedException()
        {
        }

        public NotAuthorizedException(string message)
            : base(message)
        {
        }

        public NotAuthorizedException(string message, Exception inner)
            : base(message, inner)
        {
        }
	}
}