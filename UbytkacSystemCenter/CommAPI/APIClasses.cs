namespace EasyITSystemCenter.Api {

    /// <summary>
    /// ALL standard View AND Form API Call must end with "List" - These will automatic added for
    /// reports Definitions TODO From Table?
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

        /*Production Guides APIs*/
        ProdGuidGroupList,
        ProdGuidOperationList,
        ProdGuidPartList,
        ProdGuidPersonList,
        ProdGuidWorkList,

        /*Documentation Server Administration APIs*/
        DocSrvDocumentationCodeLibraryList,
        DocSrvDocumentationList,
        DocSrvDocumentationGroupList,
        DocSrvDocTemplateList,

        /*License Server Administration APIs*/
        LicSrvLicenseActivationFailList,
        LicSrvLicenseAlgorithmList,
        LicSrvUsedLicenseList,

        /*Server Administartion Setting APIs*/
        ServerApiSecurityList,
        ServerCorsDefAllowedOriginList,
        ServerHealthCheckTaskList,
        ServerLiveDataMonitorList,
        ServerModuleAndServiceList,
        ServerSettingList,
        ServerStaticOrMvcDefPathList,
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
        SolutionTaskList,
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

        /*WebPortal APIs*/
        WebBannedIpAddressList,
        WebCodeLibraryList,
        WebCoreFileList,
        WebDeveloperNewsList,
        WebDocumentationCodeLibraryList,
        WebDocumentationList,
        WebGlobalPageBlockList,
        WebGroupMenuList,
        WebMenuList,
        WebSettingList,
        WebVisitIpList,

        /*WebHosting Apis*/
        WebConfiguratorList,

        /*Server APIs*/
        ServerApi,
        StoredProceduresList,

        /*Servers*/
        CoreServerRestart,
        FtpServerStart,
        FtpServerStop,

        //AddOns
        CityList,
        CodeLibraryList,
        CountryAreaList,
        CountryAreaCityList,
        CountryList,
        CreditPackageList,

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
        WebPageSettingList,


        /*Template Classes*/
        TemplateClassList,
        TemplateClassListWithParrent
    }

    /// <summary>
    /// Global API Definition of Result API calls for All Calling of Insert / Update / Delete
    /// </summary>
    public class DBResultMessage {
        public int InsertedId { get; set; } = 0;
        public string Status { get; set; }
        public int RecordCount { get; set; } = -1;
        public string ErrorMessage { get; set; }
    }
}