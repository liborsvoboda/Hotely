using System;

namespace UbytkacAdmin.Classes {

    public partial class AdminLoginHistoryList {
        public int Id { get; set; } = 0;
        public string IpAddress { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime Timestamp { get; set; }
    }
}