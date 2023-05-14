using System;

namespace TravelAgencyAdmin.Classes {

    public partial class CountryList {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public string CountryTranslation { get; set; }
    }
}