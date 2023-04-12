using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class HotelRoomList
    {
        public HotelRoomList()
        {
            HotelReservedRoomLists = new HashSet<HotelReservedRoomList>();
        }

        public int Id { get; set; }
        public int HotelId { get; set; }
        public int RoomTypeId { get; set; }
        public string Name { get; set; }
        public string DescriptionCz { get; set; }
        public string DescriptionEn { get; set; }
        public double Price { get; set; }
        public int MaxCapacity { get; set; }
        public bool ExtraBed { get; set; }
        public int RoomsCount { get; set; }
        public bool ApproveRequest { get; set; }
        public bool Approved { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual HotelList Hotel { get; set; }
        public virtual HotelRoomTypeList RoomType { get; set; }
        public virtual UserList User { get; set; }
        public virtual ICollection<HotelReservedRoomList> HotelReservedRoomLists { get; set; }
    }
}
