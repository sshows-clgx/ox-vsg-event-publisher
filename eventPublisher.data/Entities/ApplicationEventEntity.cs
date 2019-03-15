using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eventPublisher.data.entities
{
    public class ApplicationEventEntity
    {
        [Key]
        public int EventId { get; set; }
        public string Name { get; set; }
        public int TopicId { get; set; }
        public long ApplicationId { get; set; }
        public string PublisherCallbackUrl { get; set; }
        public DateTime InsertedUtc { get; set; }

        [ForeignKey("ApplicationId")]
        public virtual ApplicationEntity ApplicationNav { get; set; }
        [ForeignKey("TopicId")]
        public virtual TopicEntity TopicNav { get; set; }
    }
}