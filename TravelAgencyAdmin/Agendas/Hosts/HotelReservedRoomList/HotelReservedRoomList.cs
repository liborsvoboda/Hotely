using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;

namespace UbytkacAdmin.Classes {

    public partial class HotelReservedRoomList {

        public int Id { get; set; } = 0;
        public int HotelId { get; set; }
        public int HotelRoomId { get; set; }
        public int ReservationId { get; set; }
        public int RoomTypeId { get; set; }
        public int StatusId { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool ExtraBed { get; set; }
        public DateTime Timestamp { get; set; }

        public string HotelName { get; set; }
        public string GuestName { get; set; }
        public string StatusName { get; set; }
        public string ReservationNumber { get; set; }
    }
}