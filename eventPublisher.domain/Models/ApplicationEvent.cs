namespace eventPublisher.domain.models
{
    public class ApplicationEvent
    {
        private long _applicationId;
        private string _applicationName;
        private int _eventId;
        private string _topicName;

        public ApplicationEvent(long applicationId, string applicationName, int eventId, string topicName) {
            _applicationId = applicationId;
            _applicationName = applicationName;
            _eventId = eventId;
            _topicName = topicName;
        }

        public long ApplicationId => _applicationId;
        
        public int EventId => _eventId;

        public string ApplicationName => _applicationName;

        public string TopicName => _topicName;
    }
}