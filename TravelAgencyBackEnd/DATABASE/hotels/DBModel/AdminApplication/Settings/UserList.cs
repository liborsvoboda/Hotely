using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class UserList
    {
        public UserList()
        {
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
            HotelLists = new HashSet<HotelList>();
            HotelPropertyAndServiceLists = new HashSet<HotelPropertyAndServiceList>();
            HotelReservationStatusLists = new HashSet<HotelReservationStatusList>();
            HotelRoomLists = new HashSet<HotelRoomList>();
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

        public int Id { get; set; }
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Description { get; set; }
        public byte[] Photo { get; set; }
        public string PhotoPath { get; set; }
        public bool? Active { get; set; }
        public string ApiToken { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual UserRoleList Role { get; set; }
        public virtual ICollection<AccessRoleList> AccessRoleLists { get; set; }
        public virtual ICollection<AddressList> AddressLists { get; set; }
        public virtual ICollection<BranchList> BranchLists { get; set; }
        public virtual ICollection<Calendar> Calendars { get; set; }
        public virtual ICollection<CityList> CityLists { get; set; }
        public virtual ICollection<CountryList> CountryLists { get; set; }
        public virtual ICollection<CurrencyList> CurrencyLists { get; set; }
        public virtual ICollection<DocumentAdviceList> DocumentAdviceLists { get; set; }
        public virtual ICollection<DocumentTypeList> DocumentTypeLists { get; set; }
        public virtual ICollection<ExchangeRateList> ExchangeRateLists { get; set; }
        public virtual ICollection<HotelAccommodationActionList> HotelAccommodationActionLists { get; set; }
        public virtual ICollection<HotelActionTypeList> HotelActionTypeLists { get; set; }
        public virtual ICollection<HotelList> HotelLists { get; set; }
        public virtual ICollection<HotelPropertyAndServiceList> HotelPropertyAndServiceLists { get; set; }
        public virtual ICollection<HotelReservationStatusList> HotelReservationStatusLists { get; set; }
        public virtual ICollection<HotelRoomList> HotelRoomLists { get; set; }
        public virtual ICollection<LanguageList> LanguageLists { get; set; }
        public virtual ICollection<MottoList> MottoLists { get; set; }
        public virtual ICollection<ParameterList> ParameterLists { get; set; }
        public virtual ICollection<PropertyOrServiceTypeList> PropertyOrServiceTypeLists { get; set; }
        public virtual ICollection<PropertyOrServiceUnitList> PropertyOrServiceUnitLists { get; set; }
        public virtual ICollection<ReportList> ReportLists { get; set; }
        public virtual ICollection<ReportQueueList> ReportQueueLists { get; set; }
        public virtual ICollection<SystemFailList> SystemFailLists { get; set; }
        public virtual ICollection<TemplateList> TemplateLists { get; set; }
    }
}
