namespace UbytkacAdmin.Api {

    /// <summary>
    /// ALL standard View AND Form API Call must end with "List" - These will automatic added for
    /// reports Definitions
    /// </summary>
    public enum ApiUrls {
        AccessRoleList,
        AdminLoginHistoryList,
        AddressList,
        Authentication,
        BackendCheck,
        BranchList,
        CityList,
        CountryAreaList,
        CountryAreaCityList,
        CountryList,
        CreditPackageList,
        CurrencyList,
        Calendar,
        DocumentAdviceList,
        DocumentTypeList,
        EmailTemplateList,
        ExchangeRateList,
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
        IgnoredExceptionList,
        InterestAreaList,
        InterestAreaCityList,
        LanguageList,
        MottoList,
        OftenQuestionList,
        ParameterList,
        PrivacyPolicyList,
        PropertyGroupList,
        PropertyOrServiceTypeList,
        PropertyOrServiceUnitList,
        RegistrationInfoList,
        ReportList,
        ReportQueueList,
        TermsList,
        SystemLanguageList,
        UbytkacInfoList,
        UserList,
        UserRoleList,
        WebSettingList,

        SystemFailList,
        TemplateClassList
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