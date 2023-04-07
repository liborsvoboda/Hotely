using System;

namespace TravelAgencyAdmin.Classes
{

    public partial class CityList
    {
        public int Id { get; set; } = 0;
        public int CountryId { get; set; }
        public string City { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public string Translation { get; set; }
    }

}
