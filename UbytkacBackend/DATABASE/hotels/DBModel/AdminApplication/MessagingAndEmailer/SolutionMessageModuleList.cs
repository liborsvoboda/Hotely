using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SolutionMessageModuleList")]
    [Index("Subject", Name = "IX_SolutionMessageModuleList", IsUnique = true)]
    public partial class SolutionMessageModuleList
    {
        public SolutionMessageModuleList()
        {
            InverseMessageParent = new HashSet<SolutionMessageModuleList>();
        }

        [Key]
        public int Id { get; set; }
        public int Level { get; set; }
        public int? MessageParentId { get; set; }
        public int MessageTypeId { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string Subject { get; set; } = null!;
        [Column(TypeName = "text")]
        public string HtmlMessage { get; set; } = null!;
        public bool Shown { get; set; }
        public bool Archived { get; set; }
        public bool IsSystemMessage { get; set; }
        public bool Published { get; set; }
        public int? GuestId { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("GuestId")]
        [InverseProperty("SolutionMessageModuleLists")]
        public virtual GuestList? Guest { get; set; }
        [ForeignKey("MessageParentId")]
        [InverseProperty("InverseMessageParent")]
        public virtual SolutionMessageModuleList? MessageParent { get; set; }
        [ForeignKey("MessageTypeId")]
        [InverseProperty("SolutionMessageModuleLists")]
        public virtual SolutionMessageTypeList MessageType { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("SolutionMessageModuleLists")]
        public virtual SolutionUserList User { get; set; } = null!;
        [InverseProperty("MessageParent")]
        public virtual ICollection<SolutionMessageModuleList> InverseMessageParent { get; set; }
    }
}
