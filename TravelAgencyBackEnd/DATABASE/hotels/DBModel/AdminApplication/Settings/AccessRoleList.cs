namespace TravelAgencyBackEnd.DBModel {

    [Table("AccessRoleList")]
    [Index("TableName", Name = "IX_AccessRuleList", IsUnique = true)]
    public partial class AccessRoleList {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string TableName { get; set; }

        [Required]
        [StringLength(1024)]
        [Unicode(false)]
        public string AccessRole { get; set; }

        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("AccessRoleLists")]
        public virtual UserList User { get; set; }
    }
}