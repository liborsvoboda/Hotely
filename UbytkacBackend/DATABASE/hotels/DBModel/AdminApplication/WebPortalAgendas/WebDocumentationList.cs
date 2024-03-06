using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("WebDocumentationList")]
    [Index("Name", "AutoVersion", "TimeStamp", Name = "IX_WebDocumentationList", IsUnique = true)]
    public partial class WebDocumentationList
    {
        [Key]
        public int Id { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        public int Sequence { get; set; }
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        [Column(TypeName = "text")]
        public string MdContent { get; set; } = null!;
        [Column(TypeName = "text")]
        public string HtmlContent { get; set; } = null!;
        public int UserId { get; set; }
        [Required]
        public bool? Active { get; set; }
        public int AutoVersion { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("WebDocumentationLists")]
        public virtual SolutionUserList User { get; set; } = null!;
    }
}
