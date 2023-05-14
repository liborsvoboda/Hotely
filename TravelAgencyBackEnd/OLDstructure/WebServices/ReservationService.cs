namespace TravelAgencyBackEnd.Services {

    public class ReservationService {
        private readonly hotelsContext _db;

        public ReservationService() {
            _db = new hotelsContext();
        }

        public IEnumerable<HotelReservationList> FindReservation(int id) {
            return _db.HotelReservationLists.Where(x => x.GuestId == id).AsEnumerable();
        }

        public IEnumerable<HotelReservationDetailList> FindReservationDetails(int id) {
            return _db.HotelReservationDetailLists.Where(x => x.ReservationId == id).AsEnumerable();
        }
    }
}