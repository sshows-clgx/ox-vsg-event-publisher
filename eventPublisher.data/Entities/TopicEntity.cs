using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace eventPublisher.data.entities
{
    public class TopicEntity
    {
        [Key]
        public int TopicId { get; set; }
        public string Name { get; set; }
        public DateTime InsertedUtc { get; set; }

		public virtual ICollection<ApplicationEventEntity> ApplicationEvents { get; set; }
    }
}