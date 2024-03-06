using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SystemReportQueueList")]
    [Index("Name", Name = "IX_ReportQueue", IsUnique = true)]
    [Index("TableName", Name = "IX_ReportQueueList")]
    [Index("TableName", "Sequence", Name = "IX_ReportQueueList_1", IsUnique = true)]
    public partial class SystemReportQueueList
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        public int Sequence { get; set; }
        public string Sql { get; set; } = null!;
        [StringLength(150)]
        [Unicode(false)]
        public string TableName { get; set; } = null!;
        public string? Filter { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? Search { get; set; }
        public string? SearchColumnList { get; set; }
        public bool SearchFilterIgnore { get; set; }
        public int? RecId { get; set; }
        public bool RecIdFilterIgnore { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
