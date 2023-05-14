namespace TravelAgencyBackEnd.WebPages {

    public class WebPageRootSearchData {
        public List<WebPageRootSearch> HotelList { get; set; } = new List<WebPageRootSearch>();
    }

    public class WebPageRootSearch {
        public HotelList Hotel { get; set; } = new HotelList();
        public HotelRoomList? RoomList { get; set; } = null;
    }
}