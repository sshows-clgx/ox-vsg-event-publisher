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
        [ForeignKey("Application")]
        public int ApplicationId { get; set; }
        public virtual ApplicationEntity Application { get; }
        public DateTime InsertedUtc { get; set; }

    }
}