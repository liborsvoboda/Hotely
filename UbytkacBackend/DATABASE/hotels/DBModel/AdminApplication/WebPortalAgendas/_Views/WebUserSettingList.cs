using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("WebUserSettingList")]
    [Index("UserId", "Key", Name = "IX_WebUserSettingList", IsUnique = true)]
    public partial class WebUserSettingList
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Key { get; set; } = null!;
        [StringLength(250)]
        public string Value { get; set; } = null!;
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("WebUserSettingLists")]
        public virtual SolutionUserList User { get; set; } = null!;
    }
}
