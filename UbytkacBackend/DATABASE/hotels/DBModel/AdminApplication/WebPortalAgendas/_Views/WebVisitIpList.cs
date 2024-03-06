using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("WebVisitIpList")]
    [Index("WebHostIp", "TimeStamp", Name = "IX_WebVisitIpList", IsUnique = true)]
    public partial class WebVisitIpList
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string WebHostIp { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        [Column(TypeName = "text")]
        public string? WhoIsInformations { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
