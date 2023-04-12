using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class AdminLoginHistoryList
    {
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
