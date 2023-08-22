using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("GuestLoginHistoryList")]
    public partial class GuestLoginHistoryList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string IpAddress { get; set; }
        public int GuestId { get; set; }
        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string Email { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
