using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class HotelReservationStatusList
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual UserList User { get; set; }
    }
}
