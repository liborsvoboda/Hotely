using System;

namespace EasyITSystemCenter.Classes {

    public partial class SystemCustomPageList {
        public int Id { get; set; } = 0;
        public string PageName { get; set; } = null;
        public string Description { get; set; }
        public int UserId { get; set; }
        public bool Active { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}