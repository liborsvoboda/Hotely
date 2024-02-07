using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("EmailerHistoryList")]
    public partial class EmailerHistoryList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(1024)]
        [Unicode(false)]
        public string Recipient { get; set; }
        [Required]
        [StringLength(1024)]
        [Unicode(false)]
        public string Subject { get; set; }
        [Column(TypeName = "text")]
        public string Email { get; set; }
        [Required]
        [StringLength(2048)]
        [Unicode(false)]
        public string Status { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
