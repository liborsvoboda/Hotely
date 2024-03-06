using System;

namespace EasyITSystemCenter.Classes {

    public partial class SolutionUserRoleList {
        public int Id { get; set; } = 0;
        public string SystemName { get; set; } = null;
        public int MinimalAccessValue { get; set; }
        public string Description { get; set; } = null;
        public DateTime? Timestamp { get; set; } = null;

        public string Translation { get; set; } = null;
    }
}