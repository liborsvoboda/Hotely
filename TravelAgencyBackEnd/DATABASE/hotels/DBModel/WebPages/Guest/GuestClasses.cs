using TravelAgencyBackEnd.CoreClasses;
using TravelAgencyBackEnd.DBModel;

namespace TravelAgencyBackEnd.WebPages
{

    public class GuestLoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public int UserId { get; set; }
    }


    public class GuestLoginResponse
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        public string Token { get; set; }

    }

    public class GuestRegistration {
        public GuestList User { get; set; }
        public string Language { get; set; }
    }
}
