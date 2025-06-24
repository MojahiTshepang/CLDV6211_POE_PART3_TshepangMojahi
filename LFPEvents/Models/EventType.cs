using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // 👈 Required for [Table]

namespace LFPEvents.Models
{
    [Table("EventTypes")] // 👈 This forces EF to map to the correct table name in SQL
    public class EventType
    {
        public int EventTypeID { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Venue> Venues { get; set; }
    }
}
