using System;

namespace UbytkacAdmin.Classes {

    public partial class GuestList {
        public int Id { get; set; } = 0;
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public int UserIdAccount { get; set; }
        public bool Active { get; set; }
        public DateTime Timestamp { get; set; }
    }
}