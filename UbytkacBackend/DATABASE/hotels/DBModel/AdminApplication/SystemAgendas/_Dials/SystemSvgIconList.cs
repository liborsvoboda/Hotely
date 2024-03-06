using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SystemSvgIconList")]
    [Index("Name", Name = "IX_SvgIconList", IsUnique = true)]
    public partial class SystemSvgIconList
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [StringLength(4096)]
        [Unicode(false)]
        public string SvgIconPath { get; set; } = null!;
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("SystemSvgIconLists")]
        public virtual SolutionUserList User { get; set; } = null!;
    }
}
