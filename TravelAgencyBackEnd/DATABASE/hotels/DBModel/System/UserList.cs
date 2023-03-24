using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class UserList
    {
        public UserList()
        {
            AddressListOwners = new HashSet<AddressList>();
            AddressListUsers = new HashSet<AddressList>();
            BranchListOwners = new HashSet<BranchList>();
            BranchListUsers = new HashSet<BranchList>();
            Calendars = new HashSet<Calendar>();
            CityLists = new HashSet<CityList>();
            CountryLists = new HashSet<CountryList>();
            CurrencyLists = new HashSet<CurrencyList>();
            DocumentAdviceListOwners = new HashSet<DocumentAdviceList>();
            DocumentAdviceListUsers = new HashSet<DocumentAdviceList>();
            DocumentTypeLists = new HashSet<DocumentTypeList>();
            HotelAccommodationActionListOwners = new HashSet<HotelAccommodationActionList>();
            HotelAccommodationActionListUsers = new HashSet<HotelAccommodationActionList>();
            HotelActionTypeLists = new HashSet<HotelActionTypeList>();
            HotelListOwners = new HashSet<HotelList>();
            HotelListUsers = new HashSet<HotelList>();
            HotelReservationDetailLists = new HashSet<HotelReservationDetailList>();
            HotelReservationReviewLists = new HashSet<HotelReservationReviewList>();
            HotelReservationStatusLists = new HashSet<HotelReservationStatusList>();
            HotelReservedRoomLists = new HashSet<HotelReservedRoomList>();
            HotelRoomLists = new HashSet<HotelRoomList>();
            LanguageLists = new HashSet<LanguageList>();
            ParameterLists = new HashSet<ParameterList>();
            ReportLists = new HashSet<ReportList>();
            ReportQueueLists = new HashSet<ReportQueueList>();
            TemplateLists = new HashSet<TemplateList>();
            UserRoleLists = new HashSet<UserRoleList>();
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
        public string AccessRole { get; set; }
        public DateTime Timestamp { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public virtual UserRoleList Role { get; set; }
        public virtual ICollection<AddressList> AddressListOwners { get; set; }
        public virtual ICollection<AddressList> AddressListUsers { get; set; }
        public virtual ICollection<BranchList> BranchListOwners { get; set; }
        public virtual ICollection<BranchList> BranchListUsers { get; set; }
        public virtual ICollection<Calendar> Calendars { get; set; }
        public virtual ICollection<CityList> CityLists { get; set; }
        public virtual ICollection<CountryList> CountryLists { get; set; }
        public virtual ICollection<CurrencyList> CurrencyLists { get; set; }
        public virtual ICollection<DocumentAdviceList> DocumentAdviceListOwners { get; set; }
        public virtual ICollection<DocumentAdviceList> DocumentAdviceListUsers { get; set; }
        public virtual ICollection<DocumentTypeList> DocumentTypeLists { get; set; }
        public virtual ICollection<HotelAccommodationActionList> HotelAccommodationActionListOwners { get; set; }
        public virtual ICollection<HotelAccommodationActionList> HotelAccommodationActionListUsers { get; set; }
        public virtual ICollection<HotelActionTypeList> HotelActionTypeLists { get; set; }
        public virtual ICollection<HotelList> HotelListOwners { get; set; }
        public virtual ICollection<HotelList> HotelListUsers { get; set; }
        public virtual ICollection<HotelReservationDetailList> HotelReservationDetailLists { get; set; }
        public virtual ICollection<HotelReservationReviewList> HotelReservationReviewLists { get; set; }
        public virtual ICollection<HotelReservationStatusList> HotelReservationStatusLists { get; set; }
        public virtual ICollection<HotelReservedRoomList> HotelReservedRoomLists { get; set; }
        public virtual ICollection<HotelRoomList> HotelRoomLists { get; set; }
        public virtual ICollection<LanguageList> LanguageLists { get; set; }
        public virtual ICollection<ParameterList> ParameterLists { get; set; }
        public virtual ICollection<ReportList> ReportLists { get; set; }
        public virtual ICollection<ReportQueueList> ReportQueueLists { get; set; }
        public virtual ICollection<TemplateList> TemplateLists { get; set; }
        public virtual ICollection<UserRoleList> UserRoleLists { get; set; }
    }
}
