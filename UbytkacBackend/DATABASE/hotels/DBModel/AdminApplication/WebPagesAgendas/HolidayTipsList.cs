using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("HolidayTipsList")]
    [Index("Name", Name = "IX_HolidayTipsList", IsUnique = true)]
    public partial class HolidayTipsList
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        public int Sequence { get; set; }
        [Column(TypeName = "text")]
        public string DescriptionCz { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? DescriptionEn { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("HolidayTipsLists")]
        public virtual SolutionUserList User { get; set; } = null!;
    }
}
