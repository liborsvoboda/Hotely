using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace TravelAgencyAdmin.Classes {

    public partial class InterestAreaList {
        public int Id { get; set; } = 0;
        public string SystemName { get; set; }
        public string Description { get; set; } = null;
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public string AreaTranslation { get; set; }
    }

    public partial class InterestAreaCityList {
        public int Id { get; set; } = 0;
        public int Iacid { get; set; }
        public int CityId { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public string CityTranslation { get; set; }
    }
}