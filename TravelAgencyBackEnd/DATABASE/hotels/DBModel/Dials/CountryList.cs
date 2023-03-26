using System;
using System.Collections.Generic;

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
        public DateTime Timestamp { get; set; }

        public virtual UserList User { get; set; }
        public virtual ICollection<CityList> CityLists { get; set; }
        public virtual ICollection<HotelList> HotelLists { get; set; }
    }
}
