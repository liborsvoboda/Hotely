using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;

namespace UbytkacAdmin.Classes {

    public partial class SystemLanguageList {
        public int Id { get; set; } = 0;
        public string SystemName { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        public string Translation { get; set; }
    }
}