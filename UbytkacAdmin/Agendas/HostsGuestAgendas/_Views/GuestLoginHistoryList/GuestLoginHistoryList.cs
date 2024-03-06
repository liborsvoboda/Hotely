using System;

namespace EasyITSystemCenter.Classes {

    public partial class GuestLoginHistoryList {
        public int Id { get; set; } = 0;
        public string IpAddress { get; set; }
        public int GuestId { get; set; }
        public string Email { get; set; }
        public DateTime Timestamp { get; set; }
    }
}