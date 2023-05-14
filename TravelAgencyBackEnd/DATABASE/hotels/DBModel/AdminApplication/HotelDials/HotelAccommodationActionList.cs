namespace TravelAgencyBackEnd.DBModel {

    [Table("HotelAccommodationActionList")]
    [Index("HotelId", "HotelActionTypeId", "StartDate", "EndDate", Name = "IX_HotelAccomodationActionList", IsUnique = true)]
    public partial class HotelAccommodationActionList {

        public HotelAccommodationActionList() {
            HotelReservationLists = new HashSet<HotelReservationList>();
        }

        [Key]
        public int Id { get; set; }

        public int HotelId { get; set; }
        public int HotelActionTypeId { get; set; }

        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        public int DaysCount { get; set; }
        public double Price { get; set; }

        [Required]
        [StringLength(2048)]
        [Unicode(false)]
        public string DescriptionCz { get; set; }

        [Required]
        [StringLength(2048)]
        [Unicode(false)]
        public string DescriptionEn { get; set; }

        public bool Top { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("HotelId")]
        [InverseProperty("HotelAccommodationActionLists")]
        public virtual HotelList Hotel { get; set; }

        [ForeignKey("HotelActionTypeId")]
        [InverseProperty("HotelAccommodationActionLists")]
        public virtual HotelActionTypeList HotelActionType { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("HotelAccommodationActionLists")]
        public virtual UserList User { get; set; }

        [InverseProperty("HotelAccommodationAction")]
        public virtual ICollection<HotelReservationList> HotelReservationLists { get; set; }
    }
}