using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SolutionLanguageList")]
    [Index("SystemName", Name = "IX_ServerLanguageList", IsUnique = true)]
    public partial class SolutionLanguageList
    {
        public SolutionLanguageList()
        {
            SolutionEmailTemplateLists = new HashSet<SolutionEmailTemplateList>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string SystemName { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("SolutionLanguageLists")]
        public virtual SolutionUserList User { get; set; }
        [InverseProperty("SystemLanguage")]
        public virtual ICollection<SolutionEmailTemplateList> SolutionEmailTemplateLists { get; set; }
    }
}
