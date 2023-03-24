using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

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
        public int OwnerId { get; set; }
        public string AccessRule { get; set; }
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
