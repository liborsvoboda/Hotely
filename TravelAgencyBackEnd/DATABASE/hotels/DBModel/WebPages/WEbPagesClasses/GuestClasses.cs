using UbytkacBackend.DBModel;

namespace UbytkacBackend.WebPages {

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
        public string? UserId { get; set; }

        public string Token { get; set; }
    }

    public class GuestRegistration {
        public GuestWebReg User { get; set; }
        public string Language { get; set; }
    }

    public class GuestWebReg {
        public int Id { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public bool Active { get; set; }
        public int? UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public SolutionUserList User { get; set; }

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
        public bool Extrabed { get; set; }
    }

    public class BookingCancel {
        public int ReservationId { get; set; }
        public string Message { get; set; }
        public string Language { get; set; }
    }

    public class UpdateBookingData {
        public UpdateBooking Booking { get; set; }
        public string Language { get; set; }
    }

    public class UpdateBooking {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string Message { get; set; }

        public int Id { get; set; }
        public double TotalPrice { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int HotelId { get; set; }
        public int CurrencyId { get; set; }
        public int StatusId { get; set; }
        public int? HotelAccommodationActionId { get; set; }
    }

    public class WebGuestSettingList {
        public List<WebGuestSetting> Settings { get; set; }
        public string Language { get; set; }
    }

    public class WebGuestSetting {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class WebGuestComment {
        public string Title { get; set; }
        public string Note { get; set; }
        public int HotelId { get; set; }
        public string Language { get; set; }
    }

    public class AddGuestReview {
        public int HotelId { get; set; }
        public int ReservationId { get; set; }
        public int Rating { get; set; }
        public string Message { get; set; }
        public string Language { get; set; }
    }

    public class UnavailableRooms {
        public int HotelId { get; set; }
        public List<int> RoomsId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Language { get; set; }
    }
}