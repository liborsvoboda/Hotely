using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SystemParameterList")]
    [Index("SystemName", "UserId", Name = "IX_ParameterList", IsUnique = true)]
    public partial class SystemParameterList
    {
        [Key]
        public int Id { get; set; }
        public int? UserId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string SystemName { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string Type { get; set; } = null!;
        [Column(TypeName = "text")]
        public string Value { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("SystemParameterLists")]
        public virtual SolutionUserList? User { get; set; }
    }
}
