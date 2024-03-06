using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SolutionOperationList")]
    [Index("Name", Name = "IX_SolutionOperationList", IsUnique = true)]
    public partial class SolutionOperationList
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string InheritedTypeName { get; set; } = null!;
        [StringLength(255)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column(TypeName = "text")]
        public string InputData { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string InheritedResultTypeName { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        public int UserId { get; set; }
        [Required]
        public bool? Active { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("SolutionOperationLists")]
        public virtual SolutionUserList User { get; set; } = null!;
    }
}
