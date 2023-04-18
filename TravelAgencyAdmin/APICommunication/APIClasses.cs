using System;

namespace TravelAgencyAdmin.Api
{
    /// <summary>
    /// ALL standard View AND Form API Call must end with "List" - These will automatic added for reports Definitions
    /// </summary>
    public enum ApiUrls
    {
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
        HotelActionTypeList,
        HotelApprovalList,
        HotelImagesList,
        HotelList,
        HotelPropertyAndServiceList,
        HotelRoomList,
        HotelRoomTypeList,
        IgnoredExceptionList,
        LanguageList,
        MottoList,
        ParameterList,
        PropertyOrServiceTypeList,
        PropertyOrServiceUnitList,
        ReportList,
        ReportQueueList,
        UserList,
        UserRoleList,

        SystemFailList,
        TemplateClassList
    }


    /// <summary>
    /// Definition of Result API calls for Insert / Update / Delete
    /// </summary>
    public class DBResultMessage
    {
        public int insertedId { get; set; } = 0;
        public string status { get; set; }
        public int recordCount { get; set; } = -1;
        public string ErrorMessage { get; set; }
    }
}



