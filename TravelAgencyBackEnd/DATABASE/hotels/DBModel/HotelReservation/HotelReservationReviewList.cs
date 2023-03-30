using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class HotelReservationReviewList
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public int ReservationId { get; set; }
        public int GuestId { get; set; }
        public double Rating { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual GuestList Guest { get; set; }
        public virtual HotelList Hotel { get; set; }
        public virtual HotelReservationList Reservation { get; set; }
    }
}
