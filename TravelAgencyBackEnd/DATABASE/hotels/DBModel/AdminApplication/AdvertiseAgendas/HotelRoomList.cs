using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("HotelRoomList")]
    [Index("HotelId", Name = "IX_HotelRoomList")]
    public partial class HotelRoomList
    {
        public HotelRoomList()
        {
            HotelReservedRoomLists = new HashSet<HotelReservedRoomList>();
        }

        [Key]
        public int Id { get; set; }
        public int HotelId { get; set; }
        public int RoomTypeId { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string DescriptionCz { get; set; }
        [Column(TypeName = "text")]
        public string DescriptionEn { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string ImageName { get; set; }
        public byte[] Image { get; set; }
        public double Price { get; set; }
        public int MaxCapacity { get; set; }
        public bool ExtraBed { get; set; }
        public int RoomsCount { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("HotelId")]
        [InverseProperty("HotelRoomLists")]
        public virtual HotelList Hotel { get; set; }
        [ForeignKey("RoomTypeId")]
        [InverseProperty("HotelRoomLists")]
        public virtual HotelRoomTypeList RoomType { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("HotelRoomLists")]
        public virtual UserList User { get; set; }
        [InverseProperty("HotelRoom")]
        public virtual ICollection<HotelReservedRoomList> HotelReservedRoomLists { get; set; }
    }
}
