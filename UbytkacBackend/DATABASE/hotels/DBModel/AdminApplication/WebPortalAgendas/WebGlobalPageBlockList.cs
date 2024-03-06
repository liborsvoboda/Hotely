using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("WebGlobalPageBlockList")]
    [Index("Name", "PagePartType", Name = "IX_WebGlobalBodyBlockList", IsUnique = true)]
    public partial class WebGlobalPageBlockList
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string PagePartType { get; set; } = null!;
        public int Sequence { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        public bool RewriteLowerLevel { get; set; }
        [Column(TypeName = "text")]
        public string? GuestHtmlContent { get; set; }
        [Column(TypeName = "text")]
        public string? UserHtmlContent { get; set; }
        [Column(TypeName = "text")]
        public string? AdminHtmlContent { get; set; }
        [Column(TypeName = "text")]
        public string? ProviderHtmlContent { get; set; }
        public bool Active { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("WebGlobalPageBlockLists")]
        public virtual SolutionUserList User { get; set; } = null!;
    }
}
