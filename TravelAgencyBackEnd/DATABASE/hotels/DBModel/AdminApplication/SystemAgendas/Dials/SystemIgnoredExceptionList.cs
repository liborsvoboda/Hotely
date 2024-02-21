using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SystemIgnoredExceptionList")]
    [Index("ErrorNumber", Name = "IX_IgnoredExceptionList", IsUnique = true)]
    public partial class SystemIgnoredExceptionList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string ErrorNumber { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [Required]
        public bool? Active { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("SystemIgnoredExceptionLists")]
        public virtual SolutionUserList User { get; set; }
    }
}
