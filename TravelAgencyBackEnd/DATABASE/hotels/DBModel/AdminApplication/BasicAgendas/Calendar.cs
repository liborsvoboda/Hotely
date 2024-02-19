using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("Calendar")]
    public partial class Calendar
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        [StringLength(1024)]
        [Unicode(false)]
        public string Notes { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("Calendars")]
        public virtual UserList User { get; set; }
    }
}
