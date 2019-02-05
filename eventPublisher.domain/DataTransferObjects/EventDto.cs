using eventPublisher.domain.enums;

namespace eventPublisher.domain.dataTransferObjects
{
    public class EventDto
    {
        public Event Event { get; set; }
        public string Data { get; set; }
    }
}