using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("EmailTemplateList")]
    [Index("TemplateName", Name = "IX_EmailTemplateList", IsUnique = true)]
    public partial class EmailTemplateList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string TemplateName { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Variables { get; set; }
        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string SubjectCz { get; set; }
        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string SubjectEn { get; set; }
        [Column(TypeName = "text")]
        public string EmailCz { get; set; }
        [Column(TypeName = "text")]
        public string EmailEn { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("EmailTemplateLists")]
        public virtual UserList User { get; set; }
    }
}
