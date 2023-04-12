using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class HotelReservedRoomList
    {
        public int Id { get; set; }
        public int HotelRoomId { get; set; }
        public int ReservationId { get; set; }
        public int RoomTypeId { get; set; }
        public int Adult { get; set; }
        public int? Children { get; set; }
        public int BookedRoomsRequest { get; set; }
        public bool ExtraBed { get; set; }
        public int StatusId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual HotelRoomList HotelRoom { get; set; }
        public virtual HotelReservationList Reservation { get; set; }
        public virtual HotelRoomTypeList RoomType { get; set; }
    }
}
