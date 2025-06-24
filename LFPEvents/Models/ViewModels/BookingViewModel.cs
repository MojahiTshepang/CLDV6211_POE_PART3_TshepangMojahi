using System;

namespace LFPEvents.Models.ViewModels
{
    public class BookingViewModel
    {
        public int BookingID { get; set; }
        public string CustomerName { get; set; }
        public string EventTitle { get; set; }
        public string VenueName { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
