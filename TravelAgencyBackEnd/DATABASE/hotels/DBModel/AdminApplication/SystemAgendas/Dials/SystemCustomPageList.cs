using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SystemCustomPageList")]
    [Index("PageName", Name = "IX_SystemCustomPageList", IsUnique = true)]
    public partial class SystemCustomPageList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        [Unicode(false)]
        public string PageName { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        public int UserId { get; set; }
        [Required]
        public bool? Active { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("SystemCustomPageLists")]
        public virtual UserList User { get; set; }
    }
}
