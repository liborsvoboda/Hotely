using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class CityList
    {
        public CityList()
        {
            HotelLists = new HashSet<HotelList>();
        }

        public int Id { get; set; }
        public int CountryId { get; set; }
        public string City { get; set; }
        public int UserId { get; set; }
        public string AccessRole { get; set; }
        public DateTime Timestamp { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public virtual CountryList Country { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual UserList User { get; set; }
        public virtual ICollection<HotelList> HotelLists { get; set; }
    }
}
