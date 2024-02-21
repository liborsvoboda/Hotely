using System;

namespace UbytkacAdmin.Classes {

    public partial class HotelRoomTypeList {
        public int Id { get; set; } = 0;
        public string SystemName { get; set; }
        public string DescriptionCz { get; set; }
        public string DescriptionEn { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public string Translation { get; set; } = null;
    }
}