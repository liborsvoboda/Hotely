using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SystemGroupMenuList")]
    [Index("SystemName", Name = "IX_SystemGroupMenuList", IsUnique = true)]
    public partial class SystemGroupMenuList
    {
        public SystemGroupMenuList()
        {
            SystemMenuLists = new HashSet<SystemMenuList>();
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
        [Required]
        public bool? Active { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("SystemGroupMenuLists")]
        public virtual UserList User { get; set; }
        [InverseProperty("Group")]
        public virtual ICollection<SystemMenuList> SystemMenuLists { get; set; }
    }
}
