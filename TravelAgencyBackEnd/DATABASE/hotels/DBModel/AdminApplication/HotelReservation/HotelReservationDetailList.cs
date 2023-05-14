namespace TravelAgencyBackEnd.DBModel {

    [Table("HotelReservationDetailList")]
    [Index("HotelId", Name = "IX_ReservationsDetailList")]
    [Index("HotelId", "GuestId", Name = "IX_ReservationsDetailList_1")]
    [Index("GuestId", Name = "IX_ReservationsDetailList_2")]
    public partial class HotelReservationDetailList {

        [Key]
        public int Id { get; set; }

        public int GuestId { get; set; }
        public int HotelId { get; set; }
        public int ReservationId { get; set; }
        public int? HotelAccommodationActionId { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        public double TotalPrice { get; set; }
        public int Adult { get; set; }
        public int? Children { get; set; }
        public int Rooms { get; set; }
        public bool ExtraBed { get; set; }

        [Required]
        [StringLength(2048)]
        [Unicode(false)]
        public string Message { get; set; }

        public bool GuestSender { get; set; }
        public bool Shown { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("GuestId")]
        [InverseProperty("HotelReservationDetailLists")]
        public virtual GuestList Guest { get; set; }

        [ForeignKey("HotelId")]
        [InverseProperty("HotelReservationDetailLists")]
        public virtual HotelList Hotel { get; set; }

        [ForeignKey("ReservationId")]
        [InverseProperty("HotelReservationDetailLists")]
        public virtual HotelReservationList Reservation { get; set; }
    }
}