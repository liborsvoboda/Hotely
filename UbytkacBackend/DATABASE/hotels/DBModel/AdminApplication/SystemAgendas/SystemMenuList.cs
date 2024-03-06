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
        [StringLength(50)]
        [Unicode(false)]
        public string MenuType { get; set; } = null!;
        public int GroupId { get; set; }
        [StringLength(250)]
        [Unicode(false)]
        public string FormPageName { get; set; } = null!;
        [StringLength(1024)]
        [Unicode(false)]
        public string AccessRole { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        public int UserId { get; set; }
        public bool NotShowInMenu { get; set; }
        [Required]
        public bool? Active { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("GroupId")]
        [InverseProperty("SystemMenuLists")]
        public virtual SystemGroupMenuList Group { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("SystemMenuLists")]
        public virtual SolutionUserList User { get; set; } = null!;
    }
}
