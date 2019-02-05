namespace eventPublisher.domain.models
{
    public class ApplicationEvent
    {
        private long _applicationId;
        private int _eventId;

        public ApplicationEvent(long applicationId, int eventId) {
            _applicationId = applicationId;
            _eventId = eventId;
        }
    }
}