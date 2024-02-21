using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("BasicAttachmentList")]
    [Index("ParentId", "ParentType", Name = "IX_AttachmentList")]
    [Index("ParentId", "FileName", Name = "UX_AttachmentList", IsUnique = true)]
    public partial class BasicAttachmentList
    {
        [Key]
        public int Id { get; set; }
        public int ParentId { get; set; }
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string ParentType { get; set; }
        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string FileName { get; set; }
        [Required]
        public byte[] Attachment { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("BasicAttachmentLists")]
        public virtual SolutionUserList User { get; set; }
    }
}
