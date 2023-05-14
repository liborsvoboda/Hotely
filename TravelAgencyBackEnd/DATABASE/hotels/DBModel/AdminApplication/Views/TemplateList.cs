namespace TravelAgencyBackEnd.DBModel {

    [Table("TemplateList")]
    [Index("Name", Name = "IX_TemplateList", IsUnique = true)]
    public partial class TemplateList {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public bool Default { get; set; }
        public int UserId { get; set; }

        [Required]
        public bool? Active { get; set; }

        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("TemplateLists")]
        public virtual UserList User { get; set; }
    }
}