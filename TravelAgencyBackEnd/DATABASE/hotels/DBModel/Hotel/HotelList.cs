using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class HotelList
    {
        public HotelList()
        {
            HotelAccommodationActionLists = new HashSet<HotelAccommodationActionList>();
            HotelReservationDetailLists = new HashSet<HotelReservationDetailList>();
            HotelReservationLists = new HashSet<HotelReservationList>();
            HotelReservationReviewLists = new HashSet<HotelReservationReviewList>();
            HotelRoomLists = new HashSet<HotelRoomList>();
        }

        public int Id { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Pool { get; set; }
        public bool NightEntertainment { get; set; }
        public bool ChildClub { get; set; }
        public bool Restaurant { get; set; }
        public double AverageRating { get; set; }
        public double BeachDistance { get; set; }
        public double CentrumDistance { get; set; }
        public bool ExtraBedSupported { get; set; }
        public int DefaultCurrencyId { get; set; }
        public int UserId { get; set; }
        public int OwnerId { get; set; }
        public string AccessRole { get; set; }
        public DateTime Timestamp { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public virtual CityList City { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual CountryList Country { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual CurrencyList DefaultCurrency { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual UserList Owner { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual UserList User { get; set; }
        public virtual ICollection<HotelAccommodationActionList> HotelAccommodationActionLists { get; set; }
        public virtual ICollection<HotelReservationDetailList> HotelReservationDetailLists { get; set; }
        public virtual ICollection<HotelReservationList> HotelReservationLists { get; set; }
        public virtual ICollection<HotelReservationReviewList> HotelReservationReviewLists { get; set; }
        public virtual ICollection<HotelRoomList> HotelRoomLists { get; set; }
    }
}
