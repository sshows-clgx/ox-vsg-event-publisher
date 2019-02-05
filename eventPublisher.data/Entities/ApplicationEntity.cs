using System;
using System.ComponentModel.DataAnnotations;

namespace eventPublisher.data.entities
{
    public class ApplicationEntity
    {
        [Key]
        public long ApplicationId { get; set; }
        public string Name { get; set; }
        public DateTime InsertedUtc { get; set; }
    }
}