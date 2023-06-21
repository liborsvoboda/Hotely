namespace TravelAgencyAdmin.Api {

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
        CountryList,
        CurrencyList,
        Calendar,
        DocumentAdviceList,
        DocumentTypeList,
        ExchangeRateList,
        GuestList,
        GuestLoginHistoryList,
        HolidayTipsList,
        HotelActionTypeList,
        HotelApprovalList,
        HotelImagesList,
        HotelList,
        HotelPropertyAndServiceList,
        HotelRoomList,
        HotelRoomTypeList,
        IgnoredExceptionList,
        InterestAreaList,
        InterestAreaCityList,
        LanguageList,
        MottoList,
        OftenQuestionList,
        ParameterList,
        PropertyGroupList,
        PropertyOrServiceTypeList,
        PropertyOrServiceUnitList,
        RegistrationInfoList,
        ReportList,
        ReportQueueList,
        UbytkacInfoList,
        UserList,
        UserRoleList,

        SystemFailList,
        TemplateClassList
    }

    /// <summary>
    /// Definition of Result API calls for Insert / Update / Delete
    /// </summary>
    public class DBResultMessage {
        public int insertedId { get; set; } = 0;
        public string status { get; set; }
        public int recordCount { get; set; } = -1;
        public string ErrorMessage { get; set; }
    }
}