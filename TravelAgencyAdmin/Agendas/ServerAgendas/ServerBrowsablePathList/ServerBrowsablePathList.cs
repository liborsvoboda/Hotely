using System;

namespace EasyITSystemCenter.Classes {

    public partial class ServerBrowsablePathList {
        public int Id { get; set; } = 0;
        public string SystemName { get; set; } = null;
        public string WebRootPath { get; set; } = null;
        public string AliasPath { get; set; } = null;
        public int UserId { get; set; }
        public bool Active { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}