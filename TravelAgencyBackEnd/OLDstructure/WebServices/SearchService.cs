namespace TravelAgencyBackEnd.Services {

    public class SearchService {
        private readonly hotelsContext _db;
        private readonly HotelService _hs;

        public SearchService() {
            _db = new hotelsContext();
            _hs = new HotelService();
        }

        public IEnumerable<HotelList> GetHotelByName(string input) {
            return _db.HotelLists.Where(n => n.Name.Contains(input)).Include(n => n.Country).Include(n => n.City).Include(r => r.HotelRoomLists).AsEnumerable();
        }

        public IEnumerable<HotelList> GetHotelByCity(string input) {
            var result = _db.HotelLists.Where(n => n.City.City.Contains(input)).Include(n => n.Country).Include(n => n.City).Include(r => r.HotelRoomLists).AsEnumerable();
            return result;
        }

        public IEnumerable<HotelList> GetHotelByCountry(string input) {
            var result = _db.HotelLists.Where(n => n.Country.SystemName.Contains(input)).Include(n => n.Country).Include(n => n.City).Include(r => r.HotelRoomLists).AsEnumerable();
            return result;
        }

        //includes hotel name, city name and country name
        public IEnumerable<HotelList> GetAllHotelByInput(string input) {
            var result = GetHotelByCity(input).ToHashSet();
            var hotelCountry = GetHotelByCountry(input);

            foreach (var h in hotelCountry)
            {
                result.Add(h);
            }

            var hotelName = GetHotelByName(input);
            foreach (var item in hotelName)
            {
                result.Add(item);
            }

            return result.AsEnumerable();
        }

        //working: searchstring + dates + rooms + people
        public IEnumerable<AvailableHotelViewModel> GetAvailableHotels(DateTime startDate, DateTime endDate, int rooms, int people, string input = null) {
            IEnumerable<HotelList> hotels;
            if (input == null || input == "")
            {
                hotels = _hs.GetAllHotels();
            }
            else
            {
                hotels = GetAllHotelByInput(input);
            }

            HashSet<AvailableHotelViewModel> hotelList = new HashSet<AvailableHotelViewModel>();
            SearchAvailableRoomsDependingOnPeople(startDate, endDate, rooms, people, hotels, hotelList);

            return hotelList;
        }

        //public IEnumerable<string> GetSearchAutoComplete()
        //{
        //    List<string> list = new List<string>();

        // var countries = _db.CountryLists; foreach (var country in countries) {
        // list.Add(country.SystemName); }

        // var cities = _db.CityLists; foreach (var city in cities) { list.Add(city.City); }

        // var hotels = _db.HotelLists; foreach (var hotel in hotels) { list.Add(hotel.Name); }

        //    return list.AsEnumerable();
        //}

        //method to get available rooms and checks if hotel has capacity for selected people
        private void SearchAvailableRoomsDependingOnPeople(DateTime startDate, DateTime endDate, int rooms, int people, IEnumerable<HotelList> hotelsByInput, HashSet<AvailableHotelViewModel> hotelList) {
            foreach (var h in hotelsByInput)
            {
                var hotelrooms = _hs.GetAvailableRooms(h.Id, startDate, endDate);
                int availableRooms = hotelrooms.SingleRooms + hotelrooms.DoubleRooms + hotelrooms.FamilyRooms;

                if (_hs.GetMaxCapacityAvailableForHotel(h.Id, startDate, endDate) >= people)
                {
                    if (availableRooms >= rooms)
                    {
                        hotelList.Add(new AvailableHotelViewModel(hotelrooms, h));
                    }
                }
            }
        }
    }
}