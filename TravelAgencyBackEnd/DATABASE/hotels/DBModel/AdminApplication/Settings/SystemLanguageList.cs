using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SystemLanguageList")]
    [Index("SystemName", Name = "IX_SystemLanguageList", IsUnique = true)]
    public partial class SystemLanguageList
    {
        public SystemLanguageList()
        {
            EmailTemplateLists = new HashSet<EmailTemplateList>();
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
        [InverseProperty("SystemLanguageLists")]
        public virtual UserList User { get; set; }
        [InverseProperty("SystemLanguage")]
        public virtual ICollection<EmailTemplateList> EmailTemplateLists { get; set; }
    }
}
