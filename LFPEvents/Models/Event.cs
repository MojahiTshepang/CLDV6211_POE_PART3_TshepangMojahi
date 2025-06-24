using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LFPEvents.Models
{
    [Table("Events")] // Changed from "Event" to match actual table name
    public partial class Event
    {
        public Event()
        {
            Bookings = new HashSet<Booking>();
        }

        public int EventID { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        public int VenueID { get; set; }

        [ForeignKey("VenueID")]
        public virtual Venue Venue { get; set; }

        public int? EventTypeID { get; set; }

        [ForeignKey("EventTypeID")]
        public virtual EventType EventType { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
