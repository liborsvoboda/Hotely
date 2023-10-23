namespace UbytkacBackend.WebPages {

    public class WebSettingList1 {
        public List<WebSetting> Settings { get; set; }
    }

    public class WebSetting {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class AddReview {
        public int HotelId { get; set; }
        public int ReservationId { get; set; }
        public int Rating { get; set; }
        public string Message { get; set; }
        public string Language { get; set; }
    }
}