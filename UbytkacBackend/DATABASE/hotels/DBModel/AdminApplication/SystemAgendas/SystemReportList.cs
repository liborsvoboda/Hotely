using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SystemReportList")]
    public partial class SystemReportList
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string PageName { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string SystemName { get; set; } = null!;
        public bool JoinedId { get; set; }
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string? ReportPath { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string MimeType { get; set; } = null!;
        public byte[] File { get; set; } = null!;
        public int UserId { get; set; }
        [Required]
        public bool? Active { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Default { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("SystemReportLists")]
        public virtual SolutionUserList User { get; set; } = null!;
    }
}
