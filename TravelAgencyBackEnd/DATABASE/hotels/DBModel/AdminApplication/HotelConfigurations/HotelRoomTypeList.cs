namespace TravelAgencyBackEnd.DBModel {

    [Table("HotelRoomTypeList")]
    [Index("SystemName", Name = "IX_RoomTypeList", IsUnique = true)]
    public partial class HotelRoomTypeList {

        public HotelRoomTypeList() {
            HotelReservedRoomLists = new HashSet<HotelReservedRoomList>();
            HotelRoomLists = new HashSet<HotelRoomList>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string SystemName { get; set; }

        [StringLength(2048)]
        [Unicode(false)]
        public string DescriptionCz { get; set; }

        [StringLength(2048)]
        [Unicode(false)]
        public string DescriptionEn { get; set; }

        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [InverseProperty("RoomType")]
        public virtual ICollection<HotelReservedRoomList> HotelReservedRoomLists { get; set; }

        [InverseProperty("RoomType")]
        public virtual ICollection<HotelRoomList> HotelRoomLists { get; set; }
    }
}