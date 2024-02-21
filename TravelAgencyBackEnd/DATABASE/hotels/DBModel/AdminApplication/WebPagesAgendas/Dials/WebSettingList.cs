using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("WebSettingList")]
    [Index("Key", Name = "IX_WebSettingList", IsUnique = true)]
    public partial class WebSettingList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Key { get; set; }
        [Column(TypeName = "text")]
        public string Value { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("WebSettingLists")]
        public virtual SolutionUserList User { get; set; }
    }
}
