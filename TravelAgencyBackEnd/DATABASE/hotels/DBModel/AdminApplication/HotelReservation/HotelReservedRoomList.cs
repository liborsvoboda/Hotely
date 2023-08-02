using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TravelAgencyBackEnd.DBModel
{
    [Table("HotelReservedRoomList")]
    [Index("HotelRoomId", "ReservationId", Name = "IX_HotelReservedRoomList", IsUnique = true)]
    public partial class HotelReservedRoomList
    {
        [Key]
        public int Id { get; set; }
        public int HotelId { get; set; }
        public int HotelRoomId { get; set; }
        public int ReservationId { get; set; }
        public int RoomTypeId { get; set; }
        public int StatusId { get; set; }
        public int Count { get; set; }
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("HotelId")]
        [InverseProperty("HotelReservedRoomLists")]
        public virtual HotelList Hotel { get; set; }
        [ForeignKey("HotelRoomId")]
        [InverseProperty("HotelReservedRoomLists")]
        public virtual HotelRoomList HotelRoom { get; set; }
        [ForeignKey("ReservationId")]
        [InverseProperty("HotelReservedRoomLists")]
        public virtual HotelReservationList Reservation { get; set; }
        [ForeignKey("RoomTypeId")]
        [InverseProperty("HotelReservedRoomLists")]
        public virtual HotelRoomTypeList RoomType { get; set; }
        [ForeignKey("StatusId")]
        [InverseProperty("HotelReservedRoomLists")]
        public virtual HotelReservationStatusList Status { get; set; }
    }
}
