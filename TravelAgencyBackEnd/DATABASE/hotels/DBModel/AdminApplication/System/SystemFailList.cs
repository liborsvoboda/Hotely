using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SystemFailList")]
    public partial class SystemFailList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Message { get; set; }
        public int? UserId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string UserName { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("SystemFailLists")]
        public virtual UserList User { get; set; }
    }
}
