using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;

namespace EasyITSystemCenter.Classes {

    public partial class HotelReservationList {
        public int Id { get; set; } = 0;
        public string ReservationNumber { get; set; }
        public int HotelId { get; set; }
        public int GuestId { get; set; }
        public int StatusId { get; set; }
        public int CurrencyId { get; set; }
        public int? HotelAccommodationActionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TotalPrice { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime Timestamp { get; set; }

        public string HotelName { get; set; }
        public string GuestName { get; set; }
        public string StatusName { get; set; }
        public string CurrencyName { get; set; }
    }
}