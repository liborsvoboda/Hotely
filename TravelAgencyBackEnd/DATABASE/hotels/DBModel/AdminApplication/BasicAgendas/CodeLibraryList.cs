using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("CodeLibraryList")]
    [Index("Name", Name = "IX_CodeLibraryList", IsUnique = true)]
    public partial class CodeLibraryList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        [StringLength(2096)]
        [Unicode(false)]
        public string Description { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string HtmlContent { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("CodeLibraryLists")]
        public virtual UserList User { get; set; }
    }
}
