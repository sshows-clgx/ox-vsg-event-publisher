using System.Collections.Generic;

namespace eventPublisher.domain.dataTransferObjects
{
    public class ConfigurationDto
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public long ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public int TopicId { get; set; }
        public string TopicName { get; set; }
        public string PublisherCallbackUrl { get; set; }
        public IEnumerable<SubscriptionDto> Subscribers { get; set; }
    }
}