using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("DocumentationList")]
    [Index("Name", "DocumentationGroupId", "AutoVersion", "TimeStamp", Name = "IX_DocumentationList", IsUnique = true)]
    public partial class DocumentationList
    {
        [Key]
        public int Id { get; set; }
        public int DocumentationGroupId { get; set; }
        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string Name { get; set; }
        public int Sequence { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string MdContent { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string HtmlContent { get; set; }
        public int UserId { get; set; }
        [Required]
        public bool Active { get; set; }
        public int AutoVersion { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("DocumentationGroupId")]
        [InverseProperty("DocumentationLists")]
        public virtual DocumentationGroupList DocumentationGroup { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("DocumentationLists")]
        public virtual SolutionUserList User { get; set; }
    }
}
