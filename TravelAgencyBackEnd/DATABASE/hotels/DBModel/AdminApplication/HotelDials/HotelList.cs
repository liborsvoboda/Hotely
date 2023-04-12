using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class HotelList
    {
        public HotelList()
        {
            HotelAccommodationActionLists = new HashSet<HotelAccommodationActionList>();
            HotelPropertyAndServiceLists = new HashSet<HotelPropertyAndServiceList>();
            HotelReservationDetailLists = new HashSet<HotelReservationDetailList>();
            HotelReservationLists = new HashSet<HotelReservationList>();
            HotelReservationReviewLists = new HashSet<HotelReservationReviewList>();
            HotelRoomLists = new HashSet<HotelRoomList>();
        }

        public int Id { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public string Name { get; set; }
        public string DescriptionCz { get; set; }
        public string DescriptionEn { get; set; }
        public int DefaultCurrencyId { get; set; }
        public bool ApproveRequest { get; set; }
        public bool Approved { get; set; }
        public bool? Advertised { get; set; }
        public int TotalCapacity { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual CityList City { get; set; }
        public virtual CountryList Country { get; set; }
        public virtual CurrencyList DefaultCurrency { get; set; }
        public virtual UserList User { get; set; }
        public virtual ICollection<HotelAccommodationActionList> HotelAccommodationActionLists { get; set; }
        public virtual ICollection<HotelPropertyAndServiceList> HotelPropertyAndServiceLists { get; set; }
        public virtual ICollection<HotelReservationDetailList> HotelReservationDetailLists { get; set; }
        public virtual ICollection<HotelReservationList> HotelReservationLists { get; set; }
        public virtual ICollection<HotelReservationReviewList> HotelReservationReviewLists { get; set; }
        public virtual ICollection<HotelRoomList> HotelRoomLists { get; set; }
    }
}
