using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SystemTranslationList")]
    [Index("SystemName", Name = "IX_SystemTranslationList", IsUnique = true)]
    public partial class SystemTranslationList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string SystemName { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string DescriptionCz { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string DescriptionEn { get; set; }
        public int? UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("SystemTranslationLists")]
        public virtual SolutionUserList User { get; set; }
    }
}
