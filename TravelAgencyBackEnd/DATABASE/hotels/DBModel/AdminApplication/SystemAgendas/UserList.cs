using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("UserList")]
    [Index("UserName", Name = "IX_UserList", IsUnique = true)]
    public partial class UserList
    {
        public UserList()
        {
            AddressLists = new HashSet<AddressList>();
            BranchLists = new HashSet<BranchList>();
            Calendars = new HashSet<Calendar>();
            CityLists = new HashSet<CityList>();
            CodeLibraryLists = new HashSet<CodeLibraryList>();
            CountryAreaCityLists = new HashSet<CountryAreaCityList>();
            CountryAreaLists = new HashSet<CountryAreaList>();
            CountryLists = new HashSet<CountryList>();
            CreditPackageLists = new HashSet<CreditPackageList>();
            CurrencyLists = new HashSet<CurrencyList>();
            DocSrvDocTemplateLists = new HashSet<DocSrvDocTemplateList>();
            DocumentAdviceLists = new HashSet<DocumentAdviceList>();
            DocumentTypeLists = new HashSet<DocumentTypeList>();
            DocumentationCodeLibraryLists = new HashSet<DocumentationCodeLibraryList>();
            DocumentationGroupLists = new HashSet<DocumentationGroupList>();
            DocumentationLists = new HashSet<DocumentationList>();
            EmailTemplateLists = new HashSet<EmailTemplateList>();
            ExchangeRateLists = new HashSet<ExchangeRateList>();
            GuestLists = new HashSet<GuestList>();
            HolidayTipsLists = new HashSet<HolidayTipsList>();
            HotelAccommodationActionLists = new HashSet<HotelAccommodationActionList>();
            HotelActionTypeLists = new HashSet<HotelActionTypeList>();
            HotelImagesLists = new HashSet<HotelImagesList>();
            HotelLists = new HashSet<HotelList>();
            HotelPropertyAndServiceLists = new HashSet<HotelPropertyAndServiceList>();
            HotelReservationStatusLists = new HashSet<HotelReservationStatusList>();
            HotelRoomLists = new HashSet<HotelRoomList>();
            IgnoredExceptionLists = new HashSet<IgnoredExceptionList>();
            InterestAreaCityLists = new HashSet<InterestAreaCityList>();
            InterestAreaLists = new HashSet<InterestAreaList>();
            MessageModuleLists = new HashSet<MessageModuleList>();
            MessageTypeLists = new HashSet<MessageTypeList>();
            MottoLists = new HashSet<MottoList>();
            OftenQuestionLists = new HashSet<OftenQuestionList>();
            ParameterLists = new HashSet<ParameterList>();
            PrivacyPolicyLists = new HashSet<PrivacyPolicyList>();
            PropertyGroupLists = new HashSet<PropertyGroupList>();
            PropertyOrServiceTypeLists = new HashSet<PropertyOrServiceTypeList>();
            PropertyOrServiceUnitLists = new HashSet<PropertyOrServiceUnitList>();
            RegistrationInfoLists = new HashSet<RegistrationInfoList>();
            ServerBrowsablePathLists = new HashSet<ServerBrowsablePathList>();
            ServerCorsDefAllowedOriginLists = new HashSet<ServerCorsDefAllowedOriginList>();
            ServerHealthCheckTaskLists = new HashSet<ServerHealthCheckTaskList>();
            ServerLiveDataMonitorLists = new HashSet<ServerLiveDataMonitorList>();
            ServerModuleAndServiceLists = new HashSet<ServerModuleAndServiceList>();
            ServerSettingLists = new HashSet<ServerSettingList>();
            SolutionMixedEnumLists = new HashSet<SolutionMixedEnumList>();
            SolutionOperationLists = new HashSet<SolutionOperationList>();
            SolutionSchedulerLists = new HashSet<SolutionSchedulerList>();
            SvgIconLists = new HashSet<SvgIconList>();
            SystemCustomPageLists = new HashSet<SystemCustomPageList>();
            SystemFailLists = new HashSet<SystemFailList>();
            SystemGroupMenuLists = new HashSet<SystemGroupMenuList>();
            SystemLanguageLists = new HashSet<SystemLanguageList>();
            SystemMenuLists = new HashSet<SystemMenuList>();
            SystemModuleLists = new HashSet<SystemModuleList>();
            SystemReportLists = new HashSet<SystemReportList>();
            SystemTranslationLists = new HashSet<SystemTranslationList>();
            TemplateLists = new HashSet<TemplateList>();
            TermsLists = new HashSet<TermsList>();
            ToolPanelDefinitionLists = new HashSet<ToolPanelDefinitionList>();
            ToolTypeLists = new HashSet<ToolTypeList>();
            UbytkacInfoLists = new HashSet<UbytkacInfoList>();
            WebMottoLists = new HashSet<WebMottoList>();
            WebSettingLists = new HashSet<WebSettingList>();
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
        public bool Active { get; set; }
        [StringLength(4096)]
        [Unicode(false)]
        public string ApiToken { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("RoleId")]
        [InverseProperty("UserLists")]
        public virtual UserRoleList Role { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<AddressList> AddressLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BranchList> BranchLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Calendar> Calendars { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<CityList> CityLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<CodeLibraryList> CodeLibraryLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<CountryAreaCityList> CountryAreaCityLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<CountryAreaList> CountryAreaLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<CountryList> CountryLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<CreditPackageList> CreditPackageLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<CurrencyList> CurrencyLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<DocSrvDocTemplateList> DocSrvDocTemplateLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<DocumentAdviceList> DocumentAdviceLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<DocumentTypeList> DocumentTypeLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<DocumentationCodeLibraryList> DocumentationCodeLibraryLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<DocumentationGroupList> DocumentationGroupLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<DocumentationList> DocumentationLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<EmailTemplateList> EmailTemplateLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ExchangeRateList> ExchangeRateLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<GuestList> GuestLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<HolidayTipsList> HolidayTipsLists { get; set; }
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
        public virtual ICollection<InterestAreaCityList> InterestAreaCityLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<InterestAreaList> InterestAreaLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<MessageModuleList> MessageModuleLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<MessageTypeList> MessageTypeLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<MottoList> MottoLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<OftenQuestionList> OftenQuestionLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ParameterList> ParameterLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<PrivacyPolicyList> PrivacyPolicyLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<PropertyGroupList> PropertyGroupLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<PropertyOrServiceTypeList> PropertyOrServiceTypeLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<PropertyOrServiceUnitList> PropertyOrServiceUnitLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<RegistrationInfoList> RegistrationInfoLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ServerBrowsablePathList> ServerBrowsablePathLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ServerCorsDefAllowedOriginList> ServerCorsDefAllowedOriginLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ServerHealthCheckTaskList> ServerHealthCheckTaskLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ServerLiveDataMonitorList> ServerLiveDataMonitorLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ServerModuleAndServiceList> ServerModuleAndServiceLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ServerSettingList> ServerSettingLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SolutionMixedEnumList> SolutionMixedEnumLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SolutionOperationList> SolutionOperationLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SolutionSchedulerList> SolutionSchedulerLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SvgIconList> SvgIconLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SystemCustomPageList> SystemCustomPageLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SystemFailList> SystemFailLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SystemGroupMenuList> SystemGroupMenuLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SystemLanguageList> SystemLanguageLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SystemMenuList> SystemMenuLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SystemModuleList> SystemModuleLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SystemReportList> SystemReportLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SystemTranslationList> SystemTranslationLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<TemplateList> TemplateLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<TermsList> TermsLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ToolPanelDefinitionList> ToolPanelDefinitionLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ToolTypeList> ToolTypeLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<UbytkacInfoList> UbytkacInfoLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<WebMottoList> WebMottoLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<WebSettingList> WebSettingLists { get; set; }
    }
}
