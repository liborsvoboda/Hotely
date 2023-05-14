namespace TravelAgencyBackEnd.DBModel {

    [Table("HotelReservationReviewList")]
    [Index("HotelId", Name = "IX_ReviewList")]
    [Index("ReservationId", Name = "IX_ReviewList_1", IsUnique = true)]
    public partial class HotelReservationReviewList {

        [Key]
        public int Id { get; set; }

        public int HotelId { get; set; }
        public int ReservationId { get; set; }
        public int GuestId { get; set; }
        public double Rating { get; set; }

        [Required]
        [StringLength(2048)]
        [Unicode(false)]
        public string Description { get; set; }

        public DateTime Timestamp { get; set; }

        [ForeignKey("GuestId")]
        [InverseProperty("HotelReservationReviewLists")]
        public virtual GuestList Guest { get; set; }

        [ForeignKey("HotelId")]
        [InverseProperty("HotelReservationReviewLists")]
        public virtual HotelList Hotel { get; set; }

        [ForeignKey("ReservationId")]
        [InverseProperty("HotelReservationReviewList")]
        public virtual HotelReservationList Reservation { get; set; }
    }
}