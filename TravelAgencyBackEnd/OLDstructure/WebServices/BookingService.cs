namespace TravelAgencyBackEnd.Services {

    public class BookingService {
        private readonly hotelsContext _db;
        private readonly HotelService _hs;

        public BookingService() {
            _db = new hotelsContext();
            _hs = new HotelService();
        }

        public int latestId;
        public string latestType;

        public HotelReservationList GetBookingById(int id) {
            var result = _db.HotelReservationLists.Include(r => r.Guest).Include(h => h.Hotel).ThenInclude(r => r.HotelRoomLists).SingleOrDefault(r => r.Id == id);
            return result;
        }

        public HotelReservedRoomList GetReservedRoom(int id) {
            var result = _db.HotelReservedRoomLists.FirstOrDefault(x => x.ReservationId == id);
            return result;
        }

        public HotelReservationDetailList GetReservationsDetail(int id) {
            var res = _db.HotelReservationDetailLists.FirstOrDefault(r => r.ReservationId == id);
            return res;
        }

        public IEnumerable<HotelReservedRoomList> GetReservedRooms(int id) {
            return _db.HotelReservedRoomLists.Where(x => x.ReservationId == id).Include(r => r.HotelRoom).AsEnumerable();
        }

        public IEnumerable<HotelReservationList> GetAllBookingByGuestId(int id) {
            var result = _db.HotelReservationLists.Where(b => b.GuestId == id).AsEnumerable();
            return result;
        }

        public int CancelBooking(int bookingId) {
            var booking = GetBookingById(bookingId);
            var reservedRooms = GetReservedRooms(bookingId);

            foreach (var r in reservedRooms)
            {
                r.BookedRoomsRequest = 0;
                _db.HotelReservedRoomLists.Update(r);
            }

            booking.Status = "Cancelled";
            _db.HotelReservationLists.Update(booking);

            return _db.SaveChanges();
        }

        public int MakeBooking(SearchViewModel model) {
            var newReservation = new HotelReservationList()
            {
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                HotelId = model.HotelId,
                GuestId = model.GuestId,
                FullName = model.customerDetails.FirstName + " " + model.customerDetails.LastName,
                Email = model.customerDetails.Email,
                Phone = model.customerDetails.PhoneNumber,
                Street = model.customerDetails.Street,
                City = model.customerDetails.City,
                Zipcode = model.customerDetails.ZipCode,
                Country = model.customerDetails.Country,
                Status = "Confirmed"
            };

            _db.HotelReservationLists.Add(newReservation);
            _db.SaveChanges();
            latestId = newReservation.Id;

            //var Acc = new Accomodation();

            //latestType = Acc.Type = model.Type;

            var newResDetails = new HotelReservationDetailList()
            {
                Adult = model.Adults,
                // Children = model.Children, ExtraBed = model.ExtraBed,
                Message = model.CustomerMessage,
                ReservationId = latestId,

                //Type = latestType,
            };

            _db.HotelReservationDetailLists.Add(newResDetails);
            _db.SaveChanges();

            foreach (var reservedRooms in model.ReservedRooms)
            {
                var newReservedRooms = new HotelReservedRoomList()
                {
                    ReservationId = latestId,
                    HotelRoomId = reservedRooms.RoomId,
                    BookedRoomsRequest = reservedRooms.BookedRooms,
                };

                _db.HotelReservedRoomLists.Add(newReservedRooms);
                _db.SaveChanges();
            }

            var Cost = CalculateCost(newReservation, model.ReservedRooms, newResDetails);
            return latestId;
        }

        public double CalculateCost(HotelReservationList reservation, List<ReservedRooms> reservedRooms, HotelReservationDetailList reservationsDetail) {
            double totalprice = 0;
            //bool extrabed = (bool)reservationsDetail.ExtraBed;
            List<double> costPerNightForEachRoom = new();
            foreach (var room in reservedRooms)
            {
                costPerNightForEachRoom.Add(_db.HotelRoomLists.FirstOrDefault(x => x.Id == room.RoomId).Price);
            }
            DateTime d1 = reservation.StartDate;
            DateTime d2 = reservation.EndDate;
            TimeSpan t = d2 - d1;
            //var accomodationPrice = _hs.GetAccomodationFee(reservation.HotelId, reservationsDetail.Type);
            int days = (int)t.Days;
            int rooms = reservedRooms.Sum(b => b.BookedRooms);

            var CostPerNightAndRoom = costPerNightForEachRoom.Sum() * (days * rooms);

            totalprice += CostPerNightAndRoom;

            //totalprice += accomodationPrice;

            /* if (extrabed == true)
             {
                 var hotel = _hs.GetById(reservation.HotelId);
                 totalprice += (double)hotel.ExtraBedFee;
             }
            */
            using (var db = new hotelsContext())
            {
                var result = db.HotelReservationLists.SingleOrDefault(b => b.Id == latestId);
                if (result != null)
                {
                    result.TotalPrice = totalprice;
                    db.SaveChanges();
                }
            }
            return totalprice;
        }

        public void UpdateReservation(CustomerDetailsModel model, int id) {
            var reservation = _db.HotelReservationLists.SingleOrDefault(r => r.Id == id);

            if (reservation != null)
            {
                reservation.FullName = model.FirstName + " " + model.LastName;
                reservation.Email = model.Email;
                reservation.Phone = model.PhoneNumber;
                reservation.Street = model.Street;
                reservation.City = model.City;
                reservation.Zipcode = model.ZipCode;
                reservation.Country = model.Country;

                _db.SaveChanges();
            }
        }
    }
}