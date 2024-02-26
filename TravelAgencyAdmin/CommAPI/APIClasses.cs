namespace UbytkacAdmin.Api {

    /// <summary>
    /// ALL standard View AND Form API Call must end with "List" - These will automatic added for
    /// reports Definitions
    /// </summary>
    public enum ApiUrls {
        /*Authentication & Check APIs*/
        Authentication,
        BackendCheck,

        /*Basic/Shared APIs*/
        BasicAttachmentList,
        BasicCalendarList,
        BasicCurrencyList,
        BasicImageGalleryList,
        BasicItemList,
        BasicUnitList,
        BasicVatList,

        /*Business Module APIs*/
        BusinessAddressList,
        BusinessBranchList,
        BusinessCreditNoteList,
        BusinessCreditNoteSupportList,
        BusinessExchangeRateList,
        BusinessIncomingInvoiceList,
        BusinessIncomingInvoiceSupportList,
        BusinessIncomingOrderList,
        BusinessIncomingOrderSupportList,
        BusinessMaturityList,
        BusinessNotesList,
        BusinessOfferList,
        BusinessOfferSupportList,
        BusinessOutgoingInvoiceList,
        BusinessOutgoingInvoiceSupportList,
        BusinessOutgoingOrderList,
        BusinessOutgoingOrderSupportList,
        BusinessPaymentMethodList,
        BusinessPaymentStatusList,
        BusinessReceiptList,
        BusinessReceiptSupportList,
        BusinessWarehouseList,

        /*Server Administartion Setting APIs*/
        ServerBrowsablePathList,
        ServerCorsDefAllowedOriginList,
        ServerHealthCheckTaskList,
        ServerLiveDataMonitorList,
        ServerModuleAndServiceList,
        ServerSettingList,
        ServerToolPanelDefinitionList,
        ServerToolTypeList,
        SolutionEmailerHistoryList,
        SolutionEmailTemplateList,
        SolutionLanguageList,
        SolutionOperationList,
        SolutionMessageModuleList,
        SolutionMessageTypeList,
        SolutionMixedEnumList,
        SolutionMottoList,
        SolutionSchedulerList,
        SolutionSchedulerProcessList,
        SolutionUserList,
        SolutionUserRoleList,
        SolutionStaticFileList,
        SolutionWebsiteList,

        /*System Administartion Setting APIs*/
        SystemCustomPageList,
        SystemDocumentAdviceList,
        SystemDocumentTypeList,
        SystemGroupMenuList,
        SystemIgnoredExceptionList,
        SystemLoginHistoryList,
        SystemMenuList,
        SystemModuleList,
        SystemParameterList,
        SystemReportQueueList,
        SystemReportList,
        SolutionFailList,
        SystemTranslationList,
        SystemSvgIconList,

        /*Server Special APIs*/
        ServerEmailer,

        /*Servers*/
        CoreServerRestart,
        FtpServerStart,
        FtpServerStop,

        /*StoredProceduresList Library*/
        StoredProceduresList,
        SystemOperations,

        CityList,
        CodeLibraryList,
        CountryAreaList,
        CountryAreaCityList,
        CountryList,
        CreditPackageList,

        DocSrvDocTemplateList,
        DocumentationCodeLibraryList,
        DocumentationGroupList,
        DocumentationList,

        GuestFavoriteList,
        GuestList,
        GuestLoginHistoryList,
        GuestSettingList,
        HolidayTipsList,
        HotelActionTypeList,
        HotelApprovalList,
        HotelImagesList,
        HotelList,
        HotelPropertyAndServiceList,
        HotelReservationList,
        HotelReservationReviewApprovalList,
        HotelReservationReviewList,
        HotelReservationStatusList,
        HotelReservedRoomList,
        HotelRoomList,
        HotelRoomTypeList,
        InterestAreaList,
        InterestAreaCityList,

        OftenQuestionList,
        PrivacyPolicyList,
        PropertyGroupList,
        PropertyOrServiceTypeList,
        PropertyOrServiceUnitList,
        RegistrationInfoList,
        TermsList,
        UbytkacInfoList,
        WebMottoList,
        WebSettingList,

        TemplateClassList,
        TemplateClassListWithParrent
    }

    /// <summary>
    /// Definition of Result API calls for Insert / Update / Delete
    /// </summary>
    public class DBResultMessage {
        public int InsertedId { get; set; } = 0;
        public string Status { get; set; }
        public int RecordCount { get; set; } = -1;
        public string ErrorMessage { get; set; }
    }
}