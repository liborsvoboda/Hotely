using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class CurrencyList
    {
        public CurrencyList()
        {
            HotelLists = new HashSet<HotelList>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public decimal ExchangeRate { get; set; }
        public bool Fixed { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public bool Active { get; set; }
        public bool Default { get; set; }
        public string AccessRole { get; set; }
        public DateTime TimeStamp { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public virtual UserList User { get; set; }
        public virtual ICollection<HotelList> HotelLists { get; set; }
    }
}
