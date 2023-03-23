using System;

namespace TravelAgencyAdmin.Api
{
    /// <summary>
    /// ALL standard View AND Form API Call must end with "List" - These will automatic added for reports Definitions
    /// </summary>
    public enum ApiUrls
    {
        TravelAgencyAdminAttachmentList,
        TravelAgencyAdminAddressList,
        Authentication,
        BackendCheck,
        TravelAgencyAdminBranchList,
        TravelAgencyAdminCurrencyList,
        TravelAgencyAdminDocumentAdviceList,
        TravelAgencyAdminExchangeRateList,
        TravelAgencyAdminCalendar,
        TravelAgencyAdminItemList,
        TravelAgencyAdminLoginHistoryList,
        TravelAgencyAdminMottoList,
        TravelAgencyAdminNotesList,
        TravelAgencyAdminOfferList,
        TravelAgencyAdminOfferItemList,
        TravelAgencyAdminParameterList,
        TravelAgencyAdminReportList,
        TravelAgencyAdminReportQueueList,
        TravelAgencyAdminServerSetting,
        TravelAgencyAdminUnitList,
        TravelAgencyAdminUserList,
        TravelAgencyAdminUserRoleList,
        TravelAgencyAdminVatList,
        TravelAgencyAdminVisitorList,

        TravelAgencyAdminTemplateClassList
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



