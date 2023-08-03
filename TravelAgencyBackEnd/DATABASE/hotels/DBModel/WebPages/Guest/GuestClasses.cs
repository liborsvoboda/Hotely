namespace TravelAgencyBackEnd.WebPages {

    public class GuestLoginRequest {
        public string Email { get; set; }
        public string Password { get; set; }

        public int UserId { get; set; }
    }

    public class GuestLoginResponse {
        public int Id { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string Token { get; set; }
    }

    public class GuestRegistration {
        public GuestList User { get; set; }
        public string Language { get; set; }
    }

    public class BookingRequest {
        public BookingDetail Booking { get; set; }
        public string Language { get; set; }
    }

    public class BookingDetail {
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public int CurrencyId { get; set; }
        public string Currency { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int AdultInput { get; set; }
        public int ChildrenInput { get; set; }
        public string Message { get; set; }
        public double TotalPrice { get; set; }
        public BookingUser User { get; set; }
        public List<BookingRoomList> Rooms { get; set; }
    }

    public class BookingUser {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
    }

    public class BookingRoomList {
        public int Id { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public int Booked { get; set; }
    }

    public class BookingCancel {
        public int ReservationId { get; set; }
        public string Message { get; set; }
        public string Language { get; set; }
    }
}