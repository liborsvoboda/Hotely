namespace TravelAgencyBackEnd.DBModel {

    [Table("CurrencyList")]
    [Index("Name", Name = "IX_CurrencyList", IsUnique = true)]
    public partial class CurrencyList {

        public CurrencyList() {
            ExchangeRateLists = new HashSet<ExchangeRateList>();
            HotelLists = new HashSet<HotelList>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(5)]
        [Unicode(false)]
        public string Name { get; set; }

        [Column(TypeName = "numeric(10, 2)")]
        public decimal ExchangeRate { get; set; }

        public bool Fixed { get; set; }

        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        public int UserId { get; set; }
        public bool Active { get; set; }
        public bool Default { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("CurrencyLists")]
        public virtual UserList User { get; set; }

        [InverseProperty("Currency")]
        public virtual ICollection<ExchangeRateList> ExchangeRateLists { get; set; }

        [InverseProperty("DefaultCurrency")]
        public virtual ICollection<HotelList> HotelLists { get; set; }
    }
}