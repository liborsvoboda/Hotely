using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SolutionUserList")]
    [Index("UserName", Name = "IX_UserList", IsUnique = true)]
    public partial class SolutionUserList
    {
        public SolutionUserList()
        {
            BasicAttachmentLists = new HashSet<BasicAttachmentList>();
            BasicCalendarLists = new HashSet<BasicCalendarList>();
            BasicCurrencyLists = new HashSet<BasicCurrencyList>();
            BasicImageGalleryLists = new HashSet<BasicImageGalleryList>();
            BasicItemLists = new HashSet<BasicItemList>();
            BasicUnitLists = new HashSet<BasicUnitList>();
            BasicVatLists = new HashSet<BasicVatList>();
            BusinessAddressLists = new HashSet<BusinessAddressList>();
            BusinessBranchLists = new HashSet<BusinessBranchList>();
            BusinessCreditNoteLists = new HashSet<BusinessCreditNoteList>();
            BusinessCreditNoteSupportLists = new HashSet<BusinessCreditNoteSupportList>();
            BusinessExchangeRateLists = new HashSet<BusinessExchangeRateList>();
            BusinessIncomingInvoiceLists = new HashSet<BusinessIncomingInvoiceList>();
            BusinessIncomingInvoiceSupportLists = new HashSet<BusinessIncomingInvoiceSupportList>();
            BusinessIncomingOrderLists = new HashSet<BusinessIncomingOrderList>();
            BusinessIncomingOrderSupportLists = new HashSet<BusinessIncomingOrderSupportList>();
            BusinessMaturityLists = new HashSet<BusinessMaturityList>();
            BusinessNotesLists = new HashSet<BusinessNotesList>();
            BusinessOfferLists = new HashSet<BusinessOfferList>();
            BusinessOfferSupportLists = new HashSet<BusinessOfferSupportList>();
            BusinessOutgoingInvoiceLists = new HashSet<BusinessOutgoingInvoiceList>();
            BusinessOutgoingInvoiceSupportLists = new HashSet<BusinessOutgoingInvoiceSupportList>();
            BusinessOutgoingOrderLists = new HashSet<BusinessOutgoingOrderList>();
            BusinessOutgoingOrderSupportLists = new HashSet<BusinessOutgoingOrderSupportList>();
            BusinessPaymentMethodLists = new HashSet<BusinessPaymentMethodList>();
            BusinessPaymentStatusLists = new HashSet<BusinessPaymentStatusList>();
            BusinessReceiptLists = new HashSet<BusinessReceiptList>();
            BusinessReceiptSupportLists = new HashSet<BusinessReceiptSupportList>();
            BusinessWarehouseLists = new HashSet<BusinessWarehouseList>();
            CityLists = new HashSet<CityList>();
            CodeLibraryLists = new HashSet<CodeLibraryList>();
            CountryAreaCityLists = new HashSet<CountryAreaCityList>();
            CountryAreaLists = new HashSet<CountryAreaList>();
            CountryLists = new HashSet<CountryList>();
            CreditPackageLists = new HashSet<CreditPackageList>();
            DocSrvDocTemplateLists = new HashSet<DocSrvDocTemplateList>();
            DocumentationCodeLibraryLists = new HashSet<DocumentationCodeLibraryList>();
            DocumentationGroupLists = new HashSet<DocumentationGroupList>();
            DocumentationLists = new HashSet<DocumentationList>();
            GuestLists = new HashSet<GuestList>();
            HolidayTipsLists = new HashSet<HolidayTipsList>();
            HotelAccommodationActionLists = new HashSet<HotelAccommodationActionList>();
            HotelActionTypeLists = new HashSet<HotelActionTypeList>();
            HotelImagesLists = new HashSet<HotelImagesList>();
            HotelLists = new HashSet<HotelList>();
            HotelPropertyAndServiceLists = new HashSet<HotelPropertyAndServiceList>();
            HotelReservationStatusLists = new HashSet<HotelReservationStatusList>();
            HotelRoomLists = new HashSet<HotelRoomList>();
            InterestAreaCityLists = new HashSet<InterestAreaCityList>();
            InterestAreaLists = new HashSet<InterestAreaList>();
            OftenQuestionLists = new HashSet<OftenQuestionList>();
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
            ServerToolPanelDefinitionLists = new HashSet<ServerToolPanelDefinitionList>();
            ServerToolTypeLists = new HashSet<ServerToolTypeList>();
            SolutionEmailTemplateLists = new HashSet<SolutionEmailTemplateList>();
            SolutionFailLists = new HashSet<SolutionFailList>();
            SolutionLanguageLists = new HashSet<SolutionLanguageList>();
            SolutionMessageModuleLists = new HashSet<SolutionMessageModuleList>();
            SolutionMessageTypeLists = new HashSet<SolutionMessageTypeList>();
            SolutionMixedEnumLists = new HashSet<SolutionMixedEnumList>();
            SolutionMottoLists = new HashSet<SolutionMottoList>();
            SolutionOperationLists = new HashSet<SolutionOperationList>();
            SolutionSchedulerLists = new HashSet<SolutionSchedulerList>();
            SystemCustomPageLists = new HashSet<SystemCustomPageList>();
            SystemDocumentAdviceLists = new HashSet<SystemDocumentAdviceList>();
            SystemDocumentTypeLists = new HashSet<SystemDocumentTypeList>();
            SystemGroupMenuLists = new HashSet<SystemGroupMenuList>();
            SystemIgnoredExceptionLists = new HashSet<SystemIgnoredExceptionList>();
            SystemMenuLists = new HashSet<SystemMenuList>();
            SystemModuleLists = new HashSet<SystemModuleList>();
            SystemParameterLists = new HashSet<SystemParameterList>();
            SystemReportLists = new HashSet<SystemReportList>();
            SystemSvgIconLists = new HashSet<SystemSvgIconList>();
            SystemTranslationLists = new HashSet<SystemTranslationList>();
            TemplateLists = new HashSet<TemplateList>();
            TermsLists = new HashSet<TermsList>();
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
        [InverseProperty("SolutionUserLists")]
        public virtual SolutionUserRoleList Role { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BasicAttachmentList> BasicAttachmentLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BasicCalendarList> BasicCalendarLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BasicCurrencyList> BasicCurrencyLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BasicImageGalleryList> BasicImageGalleryLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BasicItemList> BasicItemLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BasicUnitList> BasicUnitLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BasicVatList> BasicVatLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessAddressList> BusinessAddressLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessBranchList> BusinessBranchLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessCreditNoteList> BusinessCreditNoteLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessCreditNoteSupportList> BusinessCreditNoteSupportLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessExchangeRateList> BusinessExchangeRateLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessIncomingInvoiceList> BusinessIncomingInvoiceLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessIncomingInvoiceSupportList> BusinessIncomingInvoiceSupportLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessIncomingOrderList> BusinessIncomingOrderLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessIncomingOrderSupportList> BusinessIncomingOrderSupportLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessMaturityList> BusinessMaturityLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessNotesList> BusinessNotesLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessOfferList> BusinessOfferLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessOfferSupportList> BusinessOfferSupportLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessOutgoingInvoiceList> BusinessOutgoingInvoiceLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessOutgoingInvoiceSupportList> BusinessOutgoingInvoiceSupportLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessOutgoingOrderList> BusinessOutgoingOrderLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessOutgoingOrderSupportList> BusinessOutgoingOrderSupportLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessPaymentMethodList> BusinessPaymentMethodLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessPaymentStatusList> BusinessPaymentStatusLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessReceiptList> BusinessReceiptLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessReceiptSupportList> BusinessReceiptSupportLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BusinessWarehouseList> BusinessWarehouseLists { get; set; }
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
        public virtual ICollection<DocSrvDocTemplateList> DocSrvDocTemplateLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<DocumentationCodeLibraryList> DocumentationCodeLibraryLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<DocumentationGroupList> DocumentationGroupLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<DocumentationList> DocumentationLists { get; set; }
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
        public virtual ICollection<InterestAreaCityList> InterestAreaCityLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<InterestAreaList> InterestAreaLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<OftenQuestionList> OftenQuestionLists { get; set; }
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
        public virtual ICollection<ServerToolPanelDefinitionList> ServerToolPanelDefinitionLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ServerToolTypeList> ServerToolTypeLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SolutionEmailTemplateList> SolutionEmailTemplateLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SolutionFailList> SolutionFailLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SolutionLanguageList> SolutionLanguageLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SolutionMessageModuleList> SolutionMessageModuleLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SolutionMessageTypeList> SolutionMessageTypeLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SolutionMixedEnumList> SolutionMixedEnumLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SolutionMottoList> SolutionMottoLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SolutionOperationList> SolutionOperationLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SolutionSchedulerList> SolutionSchedulerLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SystemCustomPageList> SystemCustomPageLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SystemDocumentAdviceList> SystemDocumentAdviceLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SystemDocumentTypeList> SystemDocumentTypeLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SystemGroupMenuList> SystemGroupMenuLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SystemIgnoredExceptionList> SystemIgnoredExceptionLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SystemMenuList> SystemMenuLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SystemModuleList> SystemModuleLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SystemParameterList> SystemParameterLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SystemReportList> SystemReportLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SystemSvgIconList> SystemSvgIconLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<SystemTranslationList> SystemTranslationLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<TemplateList> TemplateLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<TermsList> TermsLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<UbytkacInfoList> UbytkacInfoLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<WebMottoList> WebMottoLists { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<WebSettingList> WebSettingLists { get; set; }
    }
}
