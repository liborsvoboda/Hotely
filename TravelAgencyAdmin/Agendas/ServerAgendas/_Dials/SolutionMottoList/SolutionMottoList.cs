using System;

namespace EasyITSystemCenter.Classes {

    public partial class SolutionMottoList {
        public int Id { get; set; } = 0;
        public string SystemName { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public string Translation { get; set; }
    }
}