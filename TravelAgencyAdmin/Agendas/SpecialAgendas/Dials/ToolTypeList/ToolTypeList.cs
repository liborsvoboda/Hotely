using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;

namespace UbytkacAdmin.Classes {

    public partial class ToolTypeList {
        public int Id { get; set; } = 0;
        public int Sequence { get; set; }
        public string Name { get; set; } = null;
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }
    }

   


}