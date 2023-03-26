using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class GuestList
    {
        public GuestList()
        {
            HotelReservationDetailLists = new HashSet<HotelReservationDetailList>();
            HotelReservationLists = new HashSet<HotelReservationList>();
            HotelReservationReviewLists = new HashSet<HotelReservationReviewList>();
        }

        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual ICollection<HotelReservationDetailList> HotelReservationDetailLists { get; set; }
        public virtual ICollection<HotelReservationList> HotelReservationLists { get; set; }
        public virtual ICollection<HotelReservationReviewList> HotelReservationReviewLists { get; set; }
    }
}
