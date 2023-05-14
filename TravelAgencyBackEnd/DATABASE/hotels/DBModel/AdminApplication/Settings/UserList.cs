namespace TravelAgencyBackEnd.DBModel {

    [Table("UserList")]
    [Index("UserName", Name = "IX_UserList", IsUnique = true)]
    public partial class UserList {

        public UserList() {
            AccessRoleLists = new HashSet<AccessRoleList>();
            AddressLists = new HashSet<AddressList>();
            BranchLists = new HashSet<BranchList>();
            Calendars = new HashSet<Calendar>();
            CityLists = new HashSet<CityList>();
            CountryLists = new HashSet<CountryList>();
            CurrencyLists = new HashSet<CurrencyList>();
            DocumentAdviceLists = new HashSet<DocumentAdviceList>();
            DocumentTypeLists = new HashSet<DocumentTypeList>();
            ExchangeRateLists = new HashSet<ExchangeRateList>();
            HotelAccommodationActionLists = new HashSet<HotelAccommodationActionList>();
            HotelActionTypeLists = new HashSet<HotelActionTypeList>();
            HotelImagesLists = new HashSet<HotelImagesList>();
            HotelLists = new HashSet<HotelList>();
            HotelPropertyAndServiceLists = new HashSet<HotelPropertyAndServiceList>();
            HotelReservationStatusLists = new HashSet<HotelReservationStatusList>();
            HotelRoomLists = new HashSet<HotelRoomList>();
            IgnoredExceptionLists = new HashSet<IgnoredExceptionList>();
            LanguageLists = new HashSet<LanguageList>();
            MottoLists = new HashSet<MottoList>();
            ParameterLists = new HashSet<ParameterList>();
            PropertyOrServiceTypeLists = new HashSet<PropertyOrServiceTypeList>();
            PropertyOrServiceUnitLists = new HashSet<PropertyOrServiceUnitList>();
            ReportLists = new HashSet<ReportList>();
            ReportQueueLists = new HashSet<ReportQueueList>();
            SystemFailLists = new HashSet<SystemFailList>();
            TemplateLists = new HashSet<TemplateList>();
        }

        [Key]
        public int Id { get; set; }

        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string UserName { get; set; }

        [Required]
        [StringLength(2048)]
        [Unicode(false)]
        public string Password { get; set; }

        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string Name { get; set; }

        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string SurName { get; set; }

        [Column(TypeName = "text")]
        public string Description { get; set; }

        public byte[] Photo { get; set; }

        [StringLength(500)]
        [Unicode(false)]
        public string PhotoPath { get; set; }

        [Required]
        public bool? Active { get; set; }

        [StringLength(4096)]
        [Unicode(false)]
        public string ApiToken { get; set; }

        public DateTime Timestamp { get; set; }

        [ForeignKey("RoleId")]
        [InverseProperty("UserLists")]
        public virtual UserRoleList Role { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<AccessRoleList> AccessRoleLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<AddressList> AddressLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<BranchList> BranchLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<Calendar> Calendars { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<CityList> CityLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<CountryList> CountryLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<CurrencyList> CurrencyLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<DocumentAdviceList> DocumentAdviceLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<DocumentTypeList> DocumentTypeLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<ExchangeRateList> ExchangeRateLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<HotelAccommodationActionList> HotelAccommodationActionLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<HotelActionTypeList> HotelActionTypeLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<HotelImagesList> HotelImagesLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<HotelList> HotelLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<HotelPropertyAndServiceList> HotelPropertyAndServiceLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<HotelReservationStatusList> HotelReservationStatusLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<HotelRoomList> HotelRoomLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<IgnoredExceptionList> IgnoredExceptionLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<LanguageList> LanguageLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<MottoList> MottoLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<ParameterList> ParameterLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<PropertyOrServiceTypeList> PropertyOrServiceTypeLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<PropertyOrServiceUnitList> PropertyOrServiceUnitLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<ReportList> ReportLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<ReportQueueList> ReportQueueLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<SystemFailList> SystemFailLists { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<TemplateList> TemplateLists { get; set; }
    }
}