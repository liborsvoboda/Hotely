using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("ServerCorsDefAllowedOriginList")]
    [Index("AllowedDomain", Name = "IX_ServerCorsDefAllowedOriginList", IsUnique = true)]
    public partial class ServerCorsDefAllowedOriginList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string AllowedDomain { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        public int UserId { get; set; }
        [Required]
        public bool Active { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("ServerCorsDefAllowedOriginLists")]
        public virtual SolutionUserList User { get; set; }
    }
}
