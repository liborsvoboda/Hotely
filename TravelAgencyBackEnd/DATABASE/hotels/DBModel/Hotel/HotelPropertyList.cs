using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class HotelPropertyList
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public bool Required { get; set; }
        public string Type { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual UserList User { get; set; }
    }
}
