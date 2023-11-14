using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SvgIconList")]
    [Index("Name", Name = "IX_SvgIconList", IsUnique = true)]
    public partial class SvgIconList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        [Required]
        [StringLength(4096)]
        [Unicode(false)]
        public string SvgIconPath { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("SvgIconLists")]
        public virtual UserList User { get; set; }
    }
}
