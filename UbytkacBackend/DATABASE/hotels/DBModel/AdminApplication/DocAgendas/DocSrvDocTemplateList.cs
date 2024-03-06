using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("DocSrvDocTemplateList")]
    [Index("Name", Name = "IX_DocSrvDocTemplateList", IsUnique = true)]
    public partial class DocSrvDocTemplateList
    {
        [Key]
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int Sequence { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        [Unicode(false)]
        public string? Template { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("GroupId")]
        [InverseProperty("DocSrvDocTemplateLists")]
        public virtual DocumentationGroupList Group { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("DocSrvDocTemplateLists")]
        public virtual SolutionUserList User { get; set; } = null!;
    }
}
