using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("HotelImagesList")]
    [Index("HotelId", Name = "IX_HotelImagesList")]
    [Index("HotelId", "FileName", Name = "UX_HotelImagesList", IsUnique = true)]
    public partial class HotelImagesList
    {
        [Key]
        public int Id { get; set; }
        public int HotelId { get; set; }
        public bool IsPrimary { get; set; }
        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string FileName { get; set; }
        [Required]
        public byte[] Attachment { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("HotelId")]
        [InverseProperty("HotelImagesLists")]
        public virtual HotelList Hotel { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("HotelImagesLists")]
        public virtual SolutionUserList User { get; set; }
    }
}
