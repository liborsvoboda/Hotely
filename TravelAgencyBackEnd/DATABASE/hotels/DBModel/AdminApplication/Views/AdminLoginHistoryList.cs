namespace TravelAgencyBackEnd.DBModel {

    [Table("AdminLoginHistoryList")]
    public partial class AdminLoginHistoryList {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string IpAddress { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string UserName { get; set; }

        public DateTime Timestamp { get; set; }
    }
}