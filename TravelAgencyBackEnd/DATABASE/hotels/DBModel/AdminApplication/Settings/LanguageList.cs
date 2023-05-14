using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TravelAgencyBackEnd.DBModel
{
    [Table("LanguageList")]
    [Index("SystemName", Name = "IX_LanguageList", IsUnique = true)]
    public partial class LanguageList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string SystemName { get; set; }
        [StringLength(250)]
        [Unicode(false)]
        public string DescriptionCz { get; set; }
        [StringLength(250)]
        [Unicode(false)]
        public string DescriptionEn { get; set; }
        public int? UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("LanguageLists")]
        public virtual UserList User { get; set; }
    }
}
