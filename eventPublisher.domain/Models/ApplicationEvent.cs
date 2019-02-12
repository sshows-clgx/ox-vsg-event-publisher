namespace eventPublisher.domain.models
{
    public class ApplicationEvent
    {
        private long _applicationId;
        private int _eventId;
        private string _topicName;

        public ApplicationEvent(long applicationId, int eventId, string topicName) {
            _applicationId = applicationId;
            _eventId = eventId;
            _topicName = topicName;
        }

        public string TopicName => _topicName;
    }
}