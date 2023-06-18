using System;

namespace TravelAgencyAdmin.Classes {

    public partial class HotelList {
        public int Id { get; set; } = 0;
        public int? CountryId { get; set; } = null;
        public int? CityId { get; set; } = null;
        public string Name { get; set; }
        public string DescriptionCz { get; set; }
        public string DescriptionEn { get; set; }
        public int? DefaultCurrencyId { get; set; } = null;
        public bool ApproveRequest { get; set; }
        public bool Approved { get; set; }
        public bool Advertised { get; set; }
        public int TotalCapacity { get; set; }
        public decimal AverageRating { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public string CountryTranslation { get; set; }
        public string CityTranslation { get; set; }
        public string Currency { get; set; }

        public CityList City { get; set; }
    }
}