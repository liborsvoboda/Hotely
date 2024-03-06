using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("WebCoreFileList")]
    [Index("FileName", "SpecificationType", Name = "IX_WebCoreFileList", IsUnique = true)]
    public partial class WebCoreFileList
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string SpecificationType { get; set; } = null!;
        public int Sequence { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string MetroPath { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string FileName { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        public bool RewriteLowerLevel { get; set; }
        [Column(TypeName = "text")]
        public string? GuestFileContent { get; set; }
        [Column(TypeName = "text")]
        public string? UserFileContent { get; set; }
        [Column(TypeName = "text")]
        public string? AdminFileContent { get; set; }
        [Column(TypeName = "text")]
        public string? ProviderContent { get; set; }
        public bool IsUniquePath { get; set; }
        public bool AutoUpdateOnSave { get; set; }
        public bool Active { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("WebCoreFileLists")]
        public virtual SolutionUserList User { get; set; } = null!;
    }
}
