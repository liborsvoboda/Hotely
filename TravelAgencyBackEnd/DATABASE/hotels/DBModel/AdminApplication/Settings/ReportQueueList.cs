namespace TravelAgencyBackEnd.DBModel {

    [Table("ReportQueueList")]
    [Index("SystemName", Name = "IX_ReportQueue", IsUnique = true)]
    [Index("TableName", Name = "IX_ReportQueueList")]
    [Index("TableName", "Sequence", Name = "IX_ReportQueueList_1", IsUnique = true)]
    public partial class ReportQueueList {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string SystemName { get; set; }

        public int Sequence { get; set; }

        [Required]
        public string Sql { get; set; }

        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string TableName { get; set; }

        public string Filter { get; set; }

        [StringLength(50)]
        [Unicode(false)]
        public string Search { get; set; }

        public string SearchColumnList { get; set; }
        public bool SearchFilterIgnore { get; set; }
        public int? RecId { get; set; }
        public bool RecIdFilterIgnore { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("ReportQueueLists")]
        public virtual UserList User { get; set; }
    }
}