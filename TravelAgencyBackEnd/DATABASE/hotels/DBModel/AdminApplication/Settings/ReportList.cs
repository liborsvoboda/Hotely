namespace TravelAgencyBackEnd.DBModel {

    [Table("ReportList")]
    [Index("PageName", "SystemName", Name = "IX_ReportList", IsUnique = true)]
    public partial class ReportList {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string PageName { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string SystemName { get; set; }

        public bool JoinedId { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        [StringLength(500)]
        [Unicode(false)]
        public string ReportPath { get; set; }

        [Required]
        public byte[] File { get; set; }

        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("ReportLists")]
        public virtual UserList User { get; set; }
    }
}