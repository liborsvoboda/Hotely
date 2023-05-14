namespace TravelAgencyBackEnd.DBModel {

    [Table("CityList")]
    [Index("City", Name = "IX_CityList", IsUnique = true)]
    public partial class CityList {

        public CityList() {
            HotelLists = new HashSet<HotelList>();
        }

        [Key]
        public int Id { get; set; }

        public int CountryId { get; set; }

        [Required]
        [StringLength(250)]
        [Unicode(false)]
        public string City { get; set; }

        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("CountryId")]
        [InverseProperty("CityLists")]
        public virtual CountryList Country { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("CityLists")]
        public virtual UserList User { get; set; }

        [InverseProperty("City")]
        public virtual ICollection<HotelList> HotelLists { get; set; }
    }
}