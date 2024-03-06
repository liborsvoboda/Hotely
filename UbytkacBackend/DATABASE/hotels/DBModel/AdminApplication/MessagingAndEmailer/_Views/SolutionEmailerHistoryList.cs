using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SolutionEmailerHistoryList")]
    public partial class SolutionEmailerHistoryList
    {
        [Key]
        public int Id { get; set; }
        [StringLength(1024)]
        [Unicode(false)]
        public string Recipient { get; set; } = null!;
        [StringLength(1024)]
        [Unicode(false)]
        public string Subject { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? Email { get; set; }
        [StringLength(2048)]
        [Unicode(false)]
        public string Status { get; set; } = null!;
        public DateTime TimeStamp { get; set; }
    }
}
