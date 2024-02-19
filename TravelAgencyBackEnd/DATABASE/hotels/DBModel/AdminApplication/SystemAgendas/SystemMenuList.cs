using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SystemMenuList")]
    [Index("FormPageName", Name = "IX_GlobalMenuList", IsUnique = true)]
    public partial class SystemMenuList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string MenuType { get; set; }
        public int GroupId { get; set; }
        [Required]
        [StringLength(250)]
        [Unicode(false)]
        public string FormPageName { get; set; }
        [Required]
        [StringLength(1024)]
        [Unicode(false)]
        public string AccessRole { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        public int UserId { get; set; }
        public bool NotShowInMenu { get; set; }
        [Required]
        public bool? Active { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("GroupId")]
        [InverseProperty("SystemMenuLists")]
        public virtual SystemGroupMenuList Group { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("SystemMenuLists")]
        public virtual UserList User { get; set; }
    }
}
