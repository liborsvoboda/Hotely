namespace TravelAgencyBackEnd.DBModel {

    [Table("HotelReservationStatusList")]
    [Index("SystemName", Name = "IX_ReservationStatusList", IsUnique = true)]
    public partial class HotelReservationStatusList {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string SystemName { get; set; }

        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("HotelReservationStatusLists")]
        public virtual UserList User { get; set; }
    }
}