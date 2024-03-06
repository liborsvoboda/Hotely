using System;

namespace EasyITSystemCenter.Classes {

    public partial class HotelRoomList {
        public int Id { get; set; } = 0;
        public int? HotelId { get; set; } = null;
        public int? RoomTypeId { get; set; } = null;
        public string Name { get; set; }
        public string DescriptionCz { get; set; }
        public string DescriptionEn { get; set; }
        public string ImageName { get; set; }
        public byte[] Image { get; set; }
        public double Price { get; set; }
        public int MaxCapacity { get; set; }
        public bool ExtraBed { get; set; }
        public int RoomsCount { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public string Accommodation { get; set; }
        public string RoomType { get; set; }
    }
}