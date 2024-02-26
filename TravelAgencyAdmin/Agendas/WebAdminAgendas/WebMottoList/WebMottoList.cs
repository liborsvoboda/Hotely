using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;

namespace UbytkacAdmin.Classes {

    public partial class WebMottoList {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public string HtmlContent { get; set; }
        public int UserId { get; set; }
        public bool Active { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}