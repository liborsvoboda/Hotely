using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        public int Children { get; set; }
        public int ExtraBed { get; set; }
        public string Message { get; set; }
        public bool GuestSender { get; set; }
        public bool Shown { get; set; }
        public int OwnerId { get; set; }
        public string AccessRole { get; set; }
        public DateTime Timestamp { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public virtual GuestList Guest { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual HotelList Hotel { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual UserList Owner { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual HotelReservationList Reservation { get; set; }
    }
}
