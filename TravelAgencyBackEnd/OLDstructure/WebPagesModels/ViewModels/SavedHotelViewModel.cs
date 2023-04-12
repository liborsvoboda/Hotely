using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelAgencyBackEnd.Models.ViewModels
{
    public class SavedHotelViewModel
    {
        public string HotelName { get; set; }
        public string HotelDescription { get; set; }
        public string HotelImg { get; set; }
        public int HotelId { get; set; }
    }
}
