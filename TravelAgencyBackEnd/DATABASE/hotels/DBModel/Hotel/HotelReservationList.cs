using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class HotelReservationList
    {
        public HotelReservationList()
        {
            HotelReservationDetailLists = new HashSet<HotelReservationDetailList>();
            HotelReservedRoomLists = new HashSet<HotelReservedRoomList>();
        }

        public int Id { get; set; }
        public string ReservationNumber { get; set; }
        public int HotelId { get; set; }
        public int GuestId { get; set; }
        public int HotelAccommodationActionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TotalPrice { get; set; }
        public int Adult { get; set; }
        public int? Children { get; set; }
        public int Rooms { get; set; }
        public bool ExtraBed { get; set; }
        public string FullName { get; set; }
        public string Street { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public int OwnerId { get; set; }
        public DateTime Timestamp { get; set; }
        public string SearchedText { get; set; }

        public virtual GuestList Guest { get; set; }
        public virtual HotelList Hotel { get; set; }
        public virtual HotelAccommodationActionList HotelAccommodationAction { get; set; }
        public virtual HotelReservationReviewList HotelReservationReviewList { get; set; }
        public virtual ICollection<HotelReservationDetailList> HotelReservationDetailLists { get; set; }
        public virtual ICollection<HotelReservedRoomList> HotelReservedRoomLists { get; set; }
    }
}
