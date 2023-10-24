using System;
using System.ComponentModel.DataAnnotations;

namespace UbytkacAdmin.Classes {

    public partial class HotelReservationReviewList {
        public int Id { get; set; } = 0;
        public int HotelId { get; set; }
        public int ReservationId { get; set; }
        public int GuestId { get; set; }
        public double Rating { get; set; }
        public string Description { get; set; }
        public string Answer { get; set; }
        public bool? Approved { get; set; }
        public bool AdvertiserShown { get; set; }
        public DateTime Timestamp { get; set; }


        public string HotelName { get; set; }
        public string ReservationNumber { get; set; }
        public string GuestName { get; set; }
    }
}