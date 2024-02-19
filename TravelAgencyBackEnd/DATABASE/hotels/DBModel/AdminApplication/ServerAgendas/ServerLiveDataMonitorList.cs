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
        [Required]
        [StringLength(1024)]
        [Unicode(false)]
        public string RootPath { get; set; }
        [Required]
        [StringLength(1024)]
        [Unicode(false)]
        public string FileExtensions { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        public int UserId { get; set; }
        [Required]
        public bool Active { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("ServerLiveDataMonitorLists")]
        public virtual UserList User { get; set; }
    }
}
