using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("WebGroupMenuList")]
    [Index("Name", Name = "IX_WebGroupMenuList", IsUnique = true)]
    public partial class WebGroupMenuList
    {
        public WebGroupMenuList()
        {
            WebMenuLists = new HashSet<WebMenuList>();
        }

        [Key]
        public int Id { get; set; }
        public int Sequence { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? Onclick { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        public int UserId { get; set; }
        public bool Active { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("WebGroupMenuLists")]
        public virtual SolutionUserList User { get; set; } = null!;
        [InverseProperty("Group")]
        public virtual ICollection<WebMenuList> WebMenuLists { get; set; }
    }
}
