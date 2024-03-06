using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("ServerLiveDataMonitorList")]
    [Index("RootPath", Name = "IX_ServerLiveDataMonitorList", IsUnique = true)]
    public partial class ServerLiveDataMonitorList
    {
        [Key]
        public int Id { get; set; }
        [StringLength(1024)]
        [Unicode(false)]
        public string RootPath { get; set; } = null!;
        [StringLength(1024)]
        [Unicode(false)]
        public string FileExtensions { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        public int UserId { get; set; }
        [Required]
        public bool Active { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("ServerLiveDataMonitorLists")]
        public virtual SolutionUserList User { get; set; } = null!;
    }
}
