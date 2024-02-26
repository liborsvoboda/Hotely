using System;

namespace UbytkacAdmin.Classes {

    public partial class SolutionOperationList {
        public int Id { get; set; } = 0;
        public string InheritedTypeName { get; set; } = null;
        public string Name { get; set; } = null;
        public string InputData { get; set; } = null;
        public string Description { get; set; }
        public string InheritedResultTypeName { get; set; } = null;
        public int UserId { get; set; }
        public bool Active { get; set; }
        public DateTime TimeStamp { get; set; }

        public string TypeNameTranslation { get; set; } = null;
        public string ResultTypeTranslation { get; set; } = null;
    }
}