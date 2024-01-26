using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("MessageTypeList")]
    [Index("Name", Name = "IX_MessageTypeList", IsUnique = true)]
    public partial class MessageTypeList
    {
        public MessageTypeList()
        {
            MessageModuleLists = new HashSet<MessageModuleList>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        [Column(TypeName = "text")]
        public string Variables { get; set; }
        public bool AnswerAllowed { get; set; }
        public bool IsSystemOnly { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("MessageTypeLists")]
        public virtual UserList User { get; set; }
        [InverseProperty("MessageType")]
        public virtual ICollection<MessageModuleList> MessageModuleLists { get; set; }
    }
}
