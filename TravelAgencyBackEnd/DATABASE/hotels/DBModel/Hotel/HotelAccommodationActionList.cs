using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class HotelAccommodationActionList
    {
        public HotelAccommodationActionList()
        {
            HotelReservationLists = new HashSet<HotelReservationList>();
        }

        public int Id { get; set; }
        public int HotelId { get; set; }
        public int HotelActionTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DaysCount { get; set; }
        public double Price { get; set; }
        public string DescriptionCz { get; set; }
        public string DescriptionEn { get; set; }
        public bool Top { get; set; }
        public int UserId { get; set; }
        public int OwnerId { get; set; }
        public string AccessRole { get; set; }
        public DateTime Timestamp { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public virtual HotelList Hotel { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual HotelActionTypeList HotelActionType { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual UserList Owner { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual UserList User { get; set; }
        public virtual ICollection<HotelReservationList> HotelReservationLists { get; set; }
    }
}
