namespace eventPublisher.domain.models
{
    public class Subscription
    {
        private int _eventId;
        private long _applicationId;
        private string _applicationName;
        private string _topicName;
        private string _callbackUrl;
        private string _failedCommandCallbackUrl;

        public Subscription(int eventId, long applicationId, string applicationName, string topicName, string callbackUrl, string failedCommandCallbackUrl)
        {
            _eventId = eventId;
            _applicationId = applicationId;
            _applicationName = applicationName;
            _topicName = topicName;
            _callbackUrl = callbackUrl;
            _failedCommandCallbackUrl = failedCommandCallbackUrl;
        }

        public string CallbackUrl => _callbackUrl;
        public string FailedCommandCallbackUrl => _failedCommandCallbackUrl;
        public long ApplicationId => _applicationId;
        public string ApplicationName => _applicationName;
        public string TopicName => _topicName;
    }
}