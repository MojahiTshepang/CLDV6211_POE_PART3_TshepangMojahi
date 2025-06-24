namespace LFPEvents.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Bookings")]
    public partial class Booking
    {
        public int BookingID { get; set; }

        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; }

        [Required]
        public int EventID { get; set; }

        [Required]
        [DataType(DataType.Date)] // enables calendar picker in the form
        [Column(TypeName = "date")]
        public DateTime BookingDate { get; set; }

        public virtual Event Event { get; set; }
    }
}
