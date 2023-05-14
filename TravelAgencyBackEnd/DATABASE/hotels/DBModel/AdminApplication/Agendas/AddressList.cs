namespace TravelAgencyBackEnd.DBModel {

    [Table("AddressList")]
    [Index("CompanyName", "UserId", Name = "IX_AddressList", IsUnique = true)]
    [Index("Email", "UserId", Name = "IX_AddressList_1")]
    public partial class AddressList {

        [Key]
        public int Id { get; set; }

        [StringLength(20)]
        [Unicode(false)]
        public string AddressType { get; set; }

        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string CompanyName { get; set; }

        [StringLength(150)]
        [Unicode(false)]
        public string ContactName { get; set; }

        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string Street { get; set; }

        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string City { get; set; }

        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string PostCode { get; set; }

        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string Phone { get; set; }

        [StringLength(150)]
        [Unicode(false)]
        public string Email { get; set; }

        [StringLength(150)]
        [Unicode(false)]
        public string BankAccount { get; set; }

        [StringLength(20)]
        [Unicode(false)]
        public string Ico { get; set; }

        [StringLength(20)]
        [Unicode(false)]
        public string Dic { get; set; }

        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("AddressLists")]
        public virtual UserList User { get; set; }
    }
}