using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("GuestAdvertiserNoteList")]
    [Index("GuestId", "Title", Name = "IX_GuestAdvertiserNoteList", IsUnique = true)]
    public partial class GuestAdvertiserNoteList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Title { get; set; }
        [StringLength(4096)]
        [Unicode(false)]
        public string Note { get; set; }
        public bool Solved { get; set; }
        public int HotelId { get; set; }
        public int GuestId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("GuestId")]
        [InverseProperty("GuestAdvertiserNoteLists")]
        public virtual GuestList Guest { get; set; }
        [ForeignKey("HotelId")]
        [InverseProperty("GuestAdvertiserNoteLists")]
        public virtual HotelList Hotel { get; set; }
    }
}
