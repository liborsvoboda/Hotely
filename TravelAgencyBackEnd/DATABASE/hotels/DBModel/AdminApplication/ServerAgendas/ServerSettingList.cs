using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("ServerSettingList")]
    [Index("Key", Name = "IX_ServerSettingList", IsUnique = true)]
    public partial class ServerSettingList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string InheritedGroupName { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Type { get; set; }
        [Required]
        [StringLength(150)]
        public string Key { get; set; }
        [Required]
        [StringLength(150)]
        public string Value { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [StringLength(1024)]
        [Unicode(false)]
        public string Link { get; set; }
        public int UserId { get; set; }
        [Required]
        public bool? Active { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("ServerSettingLists")]
        public virtual UserList User { get; set; }
    }
}
