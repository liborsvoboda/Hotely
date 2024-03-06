using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("ServerHealthCheckTaskList")]
    [Index("TaskName", Name = "IX_HealthCheckTaskList", IsUnique = true)]
    public partial class ServerHealthCheckTaskList
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string TaskName { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string Type { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string? ServerDrive { get; set; }
        [StringLength(1024)]
        [Unicode(false)]
        public string? FolderPath { get; set; }
        [StringLength(1024)]
        [Unicode(false)]
        public string? DbSqlConnection { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string? IpAddress { get; set; }
        [StringLength(1024)]
        [Unicode(false)]
        public string? ServerUrlPath { get; set; }
        [StringLength(1024)]
        [Unicode(false)]
        public string? UrlPath { get; set; }
        [Column("SizeMB")]
        public int? SizeMb { get; set; }
        public int? Port { get; set; }
        public int UserId { get; set; }
        [Required]
        public bool Active { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("ServerHealthCheckTaskLists")]
        public virtual SolutionUserList User { get; set; } = null!;
    }
}
