namespace TravelAgencyBackEnd.DBModel {

    [Table("ParameterList")]
    [Index("SystemName", "UserId", Name = "IX_ParameterList", IsUnique = true)]
    public partial class ParameterList {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string SystemName { get; set; }

        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string Type { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Value { get; set; }

        [Column(TypeName = "ntext")]
        public string Description { get; set; }

        public int? UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("ParameterLists")]
        public virtual UserList User { get; set; }
    }
}