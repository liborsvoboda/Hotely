using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("HotelReservationStatusList")]
    [Index("SystemName", Name = "IX_ReservationStatusList", IsUnique = true)]
    public partial class HotelReservationStatusList
    {
        public HotelReservationStatusList()
        {
            HotelReservationDetailLists = new HashSet<HotelReservationDetailList>();
            HotelReservationLists = new HashSet<HotelReservationList>();
            HotelReservedRoomLists = new HashSet<HotelReservedRoomList>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string SystemName { get; set; }
        public int Sequence { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("HotelReservationStatusLists")]
        public virtual UserList User { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<HotelReservationDetailList> HotelReservationDetailLists { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<HotelReservationList> HotelReservationLists { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<HotelReservedRoomList> HotelReservedRoomLists { get; set; }
    }
}
