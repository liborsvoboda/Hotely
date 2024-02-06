using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("MessageModuleList")]
    [Index("Subject", Name = "IX_MessageModuleList", IsUnique = true)]
    [Index("MessageTypeId", Name = "IX_MessageModuleList_1")]
    [Index("Shown", Name = "IX_MessageModuleList_2")]
    public partial class MessageModuleList
    {
        public MessageModuleList()
        {
            InverseMessageParent = new HashSet<MessageModuleList>();
        }

        [Key]
        public int Id { get; set; }
        public int? MessageParentId { get; set; }
        public int MessageTypeId { get; set; }
        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string Subject { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string HtmlMessage { get; set; }
        public bool Shown { get; set; }
        public bool Archived { get; set; }
        public bool IsSystemMessage { get; set; }
        public bool Published { get; set; }
        public int? GuestId { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("GuestId")]
        [InverseProperty("MessageModuleLists")]
        public virtual GuestList Guest { get; set; }
        [ForeignKey("MessageParentId")]
        [InverseProperty("InverseMessageParent")]
        public virtual MessageModuleList MessageParent { get; set; }
        [ForeignKey("MessageTypeId")]
        [InverseProperty("MessageModuleLists")]
        public virtual MessageTypeList MessageType { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("MessageModuleLists")]
        public virtual UserList User { get; set; }
        [InverseProperty("MessageParent")]
        public virtual ICollection<MessageModuleList> InverseMessageParent { get; set; }
    }
}
