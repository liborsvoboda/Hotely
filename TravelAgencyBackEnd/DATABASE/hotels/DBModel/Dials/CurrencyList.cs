using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class CurrencyList
    {
        public CurrencyList()
        {
            ExchangeRateLists = new HashSet<ExchangeRateList>();
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
        public DateTime TimeStamp { get; set; }

        public virtual UserList User { get; set; }
        public virtual ICollection<ExchangeRateList> ExchangeRateLists { get; set; }
        public virtual ICollection<HotelList> HotelLists { get; set; }
    }
}
