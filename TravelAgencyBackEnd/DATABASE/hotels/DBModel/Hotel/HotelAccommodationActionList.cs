using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class HotelAccommodationActionList
    {
        public HotelAccommodationActionList()
        {
            HotelReservationLists = new HashSet<HotelReservationList>();
        }

        public int Id { get; set; }
        public int HotelId { get; set; }
        public int HotelActionTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int DaysCount { get; set; }
        public double Price { get; set; }
        public string DescriptionCz { get; set; }
        public string DescriptionEn { get; set; }
        public bool Top { get; set; }
        public int UserId { get; set; }
        public int OwnerId { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual HotelList Hotel { get; set; }
        public virtual HotelActionTypeList HotelActionType { get; set; }
        public virtual UserList User { get; set; }
        public virtual ICollection<HotelReservationList> HotelReservationLists { get; set; }
    }
}
