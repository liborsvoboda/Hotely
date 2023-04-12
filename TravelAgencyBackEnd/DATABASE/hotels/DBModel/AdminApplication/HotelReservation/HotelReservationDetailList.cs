using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class HotelReservationDetailList
    {
        public int Id { get; set; }
        public int GuestId { get; set; }
        public int HotelId { get; set; }
        public int ReservationId { get; set; }
        public int? HotelAccommodationActionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TotalPrice { get; set; }
        public int Adult { get; set; }
        public int? Children { get; set; }
        public int Rooms { get; set; }
        public bool ExtraBed { get; set; }
        public string Message { get; set; }
        public bool GuestSender { get; set; }
        public bool Shown { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual GuestList Guest { get; set; }
        public virtual HotelList Hotel { get; set; }
        public virtual HotelReservationList Reservation { get; set; }
    }
}
