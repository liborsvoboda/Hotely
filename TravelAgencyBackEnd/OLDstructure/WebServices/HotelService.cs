namespace TravelAgencyBackEnd.Services {

    public class HotelService {
        private readonly hotelsContext _db;

        public HotelService() {
            _db = new hotelsContext();
        }

        public int GetMaxCapacityAvailableForHotel(int id, DateTime date1, DateTime date2) {
            var roomsList = GetAvailableRooms(id, date1, date2);
            var maxPeople = 0;

            if (GetRoomByHotelId_Type(id, "Single") != null)
            {
                var maxsingle = roomsList.SingleRooms * GetRoomByHotelId_Type(id, "Single").MaxCapacity;
                maxPeople += maxsingle;
            }

            if (GetRoomByHotelId_Type(id, "Double") != null)
            {
                var maxdouble = roomsList.DoubleRooms * GetRoomByHotelId_Type(id, "Double").MaxCapacity;
                maxPeople += maxdouble;
            }

            if (GetRoomByHotelId_Type(id, "Family") != null)
            {
                var maxfamily = roomsList.FamilyRooms * GetRoomByHotelId_Type(id, "Family").MaxCapacity;
                maxPeople += maxfamily;
            }

            return maxPeople;
        }

        public HotelRoomsViewModel GetAvailableRooms(int id, DateTime date1, DateTime date2) {
            var reservations = _db.HotelReservationLists.Where(r => r.HotelId == id && r.StartDate >= date1 && r.EndDate <= date2).Include(r => r.HotelReservedRoomLists).ThenInclude(r => r.HotelRoom);

            var availableRooms = new Dictionary<string, int>();

            availableRooms.Add("Single", 0);
            availableRooms.Add("Double", 0);
            availableRooms.Add("Family", 0);

            foreach (var reservation in reservations)
            {
                foreach (var reservedRoom in reservation.HotelReservedRoomLists)
                {
                    if (reservedRoom.BookedRoomsRequest > 0)
                    { // Get amount of bookings for each room type
                        //availableRooms[reservedRoom.HotelRoom.RoomType] += reservedRoom.BookedRooms;
                    }
                }
            }

            var roomInfo = _db.HotelRoomLists.Where(h => h.HotelId == id).ToList();

            foreach (var key in availableRooms.Keys)
            {
                for (int i = 0; i < roomInfo.Count; i++)
                {
                    if (roomInfo[i].RoomType.SystemName == key)
                    { // Get availability of the rooms
                        availableRooms[key] = roomInfo[i].Hotel.HotelRoomLists.Count() - availableRooms[key];
                    }
                }
            }

            HotelRoomsViewModel vm = new() { SingleRooms = availableRooms["Single"], DoubleRooms = availableRooms["Double"], FamilyRooms = availableRooms["Family"] };

            return vm;
        }

        public IEnumerable<HotelReservationReviewList> GetReviews(int id) {
            return _db.HotelReservationReviewLists.Where(r => r.Hotel.Id == id).Include(c => c.Guest).AsEnumerable();
        }

        public HotelList GetById(int id) {
            return _db.HotelLists.Include(r => r.HotelRoomLists).SingleOrDefault(h => h.Id == id);
        }

        public HotelRoomList GetRoomByRoomId(int id) {
            return _db.HotelRoomLists.SingleOrDefault(r => r.Id == id);
        }

        public HotelRoomList GetRoomByHotelId_Type(int id, string type) {
            return _db.HotelRoomLists.SingleOrDefault(r => r.HotelId == id && r.RoomType.SystemName == type);
        }

        public IEnumerable<HotelList> GetAllHotels() {
            var result = _db.HotelLists.Include(n => n.Country).Include(n => n.City).Include(r => r.HotelRoomLists).AsEnumerable();
            return result;
        }

        public IEnumerable<HotelList> GetHotelsByRandom() {
            Random rand = new Random();
            int num1 = rand.Next(0, 5);
            int num2 = rand.Next(0, 5);
            int num3 = rand.Next(0, 5);

            return _db.HotelLists.Where(x => x.Id == num1);
        }

        /*
        public double GetAccomodationFee(int hotelId, string type)
        {
            var result = _db.Accomodations.SingleOrDefault(h => h.HotelId == hotelId && h.Type.ToLower() == type.ToLower());
            if (result != null)
            {
                return result.Price;
            }
            return 0;
        }
        */
        //public List<SavedHotel> GetSavedHotels(int id)
        //{
        //    return _db.SavedHotels.Where(u => u.GuestId == id).Include(h => h.Hotel).ToList();
        //}
    }
}