using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SolutionEmailTemplateList")]
    [Index("SystemLanguageId", "TemplateName", Name = "IX_EmailTemplateList", IsUnique = true)]
    public partial class SolutionEmailTemplateList
    {
        [Key]
        public int Id { get; set; }
        public int SystemLanguageId { get; set; }
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
        public string Subject { get; set; }
        [Column(TypeName = "text")]
        public string Email { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("SystemLanguageId")]
        [InverseProperty("SolutionEmailTemplateLists")]
        public virtual SolutionLanguageList SystemLanguage { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("SolutionEmailTemplateLists")]
        public virtual SolutionUserList User { get; set; }
    }
}
