using System;

namespace UbytkacAdmin.Classes {

    public partial class CountryAreaList {
        public int Id { get; set; } = 0;
        public string SystemName { get; set; }
        public int CountryId { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        public string AreaTranslation { get; set; }
        public string CountryTranslation { get; set; }
    }

    public partial class CountryAreaCityList {
        public int Id { get; set; } = 0;
        public int Icacid { get; set; }
        public int CityId { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public string CityTranslation { get; set; }
    }
}