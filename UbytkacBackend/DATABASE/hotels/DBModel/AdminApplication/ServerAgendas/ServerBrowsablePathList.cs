using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("ServerBrowsablePathList")]
    [Index("SystemName", Name = "IX_ServerBrowsablePathList", IsUnique = true)]
    public partial class ServerBrowsablePathList
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string SystemName { get; set; } = null!;
        [StringLength(2048)]
        [Unicode(false)]
        public string WebRootPath { get; set; } = null!;
        [StringLength(255)]
        [Unicode(false)]
        public string? AliasPath { get; set; }
        public int UserId { get; set; }
        public bool Active { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("ServerBrowsablePathLists")]
        public virtual SolutionUserList User { get; set; } = null!;
    }
}
