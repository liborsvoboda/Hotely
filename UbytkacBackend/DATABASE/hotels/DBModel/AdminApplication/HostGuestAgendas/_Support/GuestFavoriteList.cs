using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("GuestFavoriteList")]
    [Index("HotelId", "GuestId", Name = "IX_GuestFavoriteList", IsUnique = true)]
    public partial class GuestFavoriteList
    {
        [Key]
        public int Id { get; set; }
        public int HotelId { get; set; }
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        public int GuestId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("GuestId")]
        [InverseProperty("GuestFavoriteLists")]
        public virtual GuestList Guest { get; set; } = null!;
    }
}
