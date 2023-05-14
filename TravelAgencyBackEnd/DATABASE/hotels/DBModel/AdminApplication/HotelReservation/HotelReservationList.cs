namespace TravelAgencyBackEnd.DBModel {

    [Table("HotelReservationList")]
    public partial class HotelReservationList {

        public HotelReservationList() {
            HotelReservationDetailLists = new HashSet<HotelReservationDetailList>();
            HotelReservedRoomLists = new HashSet<HotelReservedRoomList>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string ReservationNumber { get; set; }

        public int HotelId { get; set; }
        public int GuestId { get; set; }
        public int HotelAccommodationActionId { get; set; }

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
        [StringLength(255)]
        [Unicode(false)]
        public string FullName { get; set; }

        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string Street { get; set; }

        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string Zipcode { get; set; }

        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Country { get; set; }

        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string Phone { get; set; }

        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Status { get; set; }

        public DateTime Timestamp { get; set; }

        [Column(TypeName = "text")]
        public string SearchedText { get; set; }

        [ForeignKey("GuestId")]
        [InverseProperty("HotelReservationLists")]
        public virtual GuestList Guest { get; set; }

        [ForeignKey("HotelId")]
        [InverseProperty("HotelReservationLists")]
        public virtual HotelList Hotel { get; set; }

        [ForeignKey("HotelAccommodationActionId")]
        [InverseProperty("HotelReservationLists")]
        public virtual HotelAccommodationActionList HotelAccommodationAction { get; set; }

        [InverseProperty("Reservation")]
        public virtual HotelReservationReviewList HotelReservationReviewList { get; set; }

        [InverseProperty("Reservation")]
        public virtual ICollection<HotelReservationDetailList> HotelReservationDetailLists { get; set; }

        [InverseProperty("Reservation")]
        public virtual ICollection<HotelReservedRoomList> HotelReservedRoomLists { get; set; }
    }
}