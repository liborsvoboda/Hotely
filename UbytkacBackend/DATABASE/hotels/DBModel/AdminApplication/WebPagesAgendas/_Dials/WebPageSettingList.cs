using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("WebPageSettingList")]
    [Index("Key", Name = "IX_WebPageSettingList", IsUnique = true)]
    public partial class WebPageSettingList
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Key { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? Value { get; set; }
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("WebPageSettingLists")]
        public virtual SolutionUserList User { get; set; } = null!;
    }
}
