using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class CountryList
    {
        public CountryList()
        {
            CityLists = new HashSet<CityList>();
            HotelLists = new HashSet<HotelList>();
        }

        public int Id { get; set; }
        public string SystemName { get; set; }
        public int UserId { get; set; }
        public string AccessRole { get; set; }
        public DateTime Timestamp { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public virtual UserList User { get; set; }
        public virtual ICollection<CityList> CityLists { get; set; }
        public virtual ICollection<HotelList> HotelLists { get; set; }
    }
}
