using TravelAgencyBackEnd.Services;

namespace TravelAgencyBackEnd.Models.ViewModels {

    public class AvailableHotelViewModel {
        public HotelRoomsViewModel RoomList { get; set; }
        public HotelList Hotel { get; set; }

        public AvailableHotelViewModel() {
        }

        public AvailableHotelViewModel(HotelRoomsViewModel list, HotelList hotel) {
            RoomList = list;
            Hotel = hotel;
        }
    }
}