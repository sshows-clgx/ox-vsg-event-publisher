using System;
using System.ComponentModel.DataAnnotations;

namespace eventPublisher.data.entities
{
    public class ApplicationEntity
    {
        [Key]
        public int ApplicationId { get; set; }
        public string Name { get; set; }
        public DateTime InsertedUtc { get; set; }
    }
}