namespace eventPublisher.domain.models
{
    public class Subscription
    {
        private int _eventId;
        private long _applicationId;
        private string _callbackUrl;
        private string _failedCommandCallbackUrl;

        public Subscription(int eventId, long applicationId, string callbackUrl, string failedCommandCallbackUrl)
        {
            _eventId = eventId;
            _applicationId = applicationId;
            _callbackUrl = callbackUrl;
            _failedCommandCallbackUrl = failedCommandCallbackUrl;
        }
    }
}