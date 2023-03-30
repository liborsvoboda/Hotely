using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class HotelRoomTypeList
    {
        public HotelRoomTypeList()
        {
            HotelReservedRoomLists = new HashSet<HotelReservedRoomList>();
            HotelRoomLists = new HashSet<HotelRoomList>();
        }

        public int Id { get; set; }
        public string SystemName { get; set; }
        public string DescriptionCz { get; set; }
        public string DescriptionEn { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual ICollection<HotelReservedRoomList> HotelReservedRoomLists { get; set; }
        public virtual ICollection<HotelRoomList> HotelRoomLists { get; set; }
    }
}
