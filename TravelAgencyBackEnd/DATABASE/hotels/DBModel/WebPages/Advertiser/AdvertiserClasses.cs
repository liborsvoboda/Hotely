namespace UbytkacBackend.WebPages {

    public class AvertiserHotel {
        public int? HotelRecId { get; set; }
        public string HotelName { get; set; }
        public int CurrencyId { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public string Description { get; set; }
    }

    public class AvertiserImages {
        public int HotelRecId { get; set; }
        public List<AvertiserImage> Images { get; set; }
    }

    public class AvertiserImage {
        public bool IsPrimary { get; set; }
        public string FileName { get; set; }
        public string Attachment { get; set; }
    }

    public class AvertiserRooms {
        public int HotelRecId { get; set; }
        public List<AvertiserRoom> Rooms { get; set; }
    }

    public class AvertiserRoom {
        public string RoomName { get; set; }
        public int RoomTypeId { get; set; }
        public double Price { get; set; }
        public bool ExtraBed { get; set; }
        public int MaxCapacity { get; set; }
        public int RoomsCount { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string Attachment { get; set; }
    }

    public class AvertiserProperties {
        public int HotelRecId { get; set; }
        public List<AvertiserProperty> Properties { get; set; }
    }

    public class AvertiserProperty {
        public int id { get; set; }
        public string name { get; set; }
        public bool isAvailable { get; set; }
        public double? value { get; set; }
        public double? valueRangeMin { get; set; }
        public double? valueRangeMax { get; set; }
        public bool fee { get; set; }
        public double? feeValue { get; set; }
        public double? feeRangeMin { get; set; }
        public double? feeRangeMax { get; set; }
    }

    public class Translation {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public string TranslationName { get; set; }
    }

    public class AdvertiserBookingData {
        public int ReservationId { get; set; }
        public int StatusId { get; set; }
        public string DetailMessage { get; set; }
        public string Language { get; set; }
    }

}