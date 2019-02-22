using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eventPublisher.data.entities
{
    public class SubscriptionEntity
    {
        public int EventId { get; set; }
        public long ApplicationId { get; set; }
        public string CallbackUrl { get; set; }
        public DateTime InsertedUtc { get; set; }

        [ForeignKey("EventId")]
        public virtual ApplicationEventEntity ApplicationEventNav { get; set; }

        [ForeignKey("ApplicationId")]
        public virtual ApplicationEntity ApplicationNav { get; set; }
    }
}