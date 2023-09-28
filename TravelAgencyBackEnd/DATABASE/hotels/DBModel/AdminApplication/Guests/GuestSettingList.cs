using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("GuestSettingList")]
    [Index("GuestId", "Key", Name = "IX_GuestSettingList", IsUnique = true)]
    public partial class GuestSettingList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Key { get; set; }
        [StringLength(250)]
        [Unicode(false)]
        public string Value { get; set; }
        public int GuestId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("GuestId")]
        [InverseProperty("GuestSettingLists")]
        public virtual GuestList Guest { get; set; }
    }
}
