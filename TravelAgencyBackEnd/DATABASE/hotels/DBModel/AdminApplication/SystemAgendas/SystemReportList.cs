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
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string PageName { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string SystemName { get; set; }
        public bool JoinedId { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string ReportPath { get; set; }
        [Required]
        [StringLength(100)]
        [Unicode(false)]
        public string MimeType { get; set; }
        [Required]
        public byte[] File { get; set; }
        public int UserId { get; set; }
        [Required]
        public bool? Active { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Default { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("SystemReportLists")]
        public virtual SolutionUserList User { get; set; }
    }
}
