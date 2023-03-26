using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelAgencyBackEnd.Models.ViewModels
{
    public class ReviewModel
    {
        public int HotelId { get; set; }
        public int ReservationId { get; set; }
        public int GuestId { get; set; }
        public float Rating { get; set; }
        public string Description { get; set; }
    }
}
