using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("WebMenuList")]
    [Index("Name", "GroupId", Name = "IX_WebMenuList", IsUnique = true)]
    public partial class WebMenuList
    {
        [Key]
        public int Id { get; set; }
        public int GroupId { get; set; }
        public int Sequence { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [StringLength(100)]
        [Unicode(false)]
        public string? MenuClass { get; set; }
        [StringLength(2096)]
        [Unicode(false)]
        public string? Description { get; set; }
        [Column(TypeName = "text")]
        public string HtmlContent { get; set; } = null!;
        public bool UserMenu { get; set; }
        public bool AdminMenu { get; set; }
        [Column("UserIPAddress")]
        [StringLength(50)]
        [Unicode(false)]
        public string? UserIpaddress { get; set; }
        public int UserId { get; set; }
        public bool Active { get; set; }
        public bool Default { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("GroupId")]
        [InverseProperty("WebMenuLists")]
        public virtual WebGroupMenuList Group { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("WebMenuLists")]
        public virtual SolutionUserList User { get; set; } = null!;
    }
}
