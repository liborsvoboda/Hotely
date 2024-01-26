using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;

namespace UbytkacAdmin.Classes {

    public partial class MessageModuleList {
        public int Id { get; set; } = 0;
        public int? MesssageParentId { get; set; }
        public int MessageTypeId { get; set; }
        public string Subject { get; set; }
        public string HtmlMessage { get; set; }
        public bool Shown { get; set; }
        public bool Archived { get; set; }
        public bool IsSystemMessage { get; set; } = true;
        public bool Published { get; set; } = false;
        public int GuestId { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        public string ParrentMessage { get; set; }
        public string ParentMessageSubject { get; set; }
        public string MessageTypeTranslation { get; set; }
    }

}