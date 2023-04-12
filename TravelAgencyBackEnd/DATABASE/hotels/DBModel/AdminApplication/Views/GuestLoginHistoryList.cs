using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class GuestLoginHistoryList
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public int GuestId { get; set; }
        public string Email { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
