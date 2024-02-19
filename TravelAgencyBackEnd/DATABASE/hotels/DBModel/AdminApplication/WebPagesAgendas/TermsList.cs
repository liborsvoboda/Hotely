using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("TermsList")]
    [Index("Name", Name = "IX_TermsList", IsUnique = true)]
    public partial class TermsList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        public int Sequence { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string DescriptionCz { get; set; }
        [Column(TypeName = "text")]
        public string DescriptionEn { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("TermsLists")]
        public virtual UserList User { get; set; }
    }
}
