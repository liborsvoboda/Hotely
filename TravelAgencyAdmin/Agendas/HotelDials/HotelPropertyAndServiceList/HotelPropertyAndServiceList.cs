using System;

namespace TravelAgencyAdmin.Classes {

    public partial class HotelPropertyAndServiceList {
        public int Id { get; set; } = 0;
        public int HotelId { get; set; }
        public int PropertyOrServiceId { get; set; }
        public bool IsAvailable { get; set; }
        public double? Value { get; set; } = null;
        public double? ValueRangeMin { get; set; } = null;
        public double? ValueRangeMax { get; set; } = null;
        public bool Fee { get; set; }
        public double? FeeValue { get; set; }
        public double? FeeRangeMin { get; set; } = null;
        public double? FeeRangeMax { get; set; } = null;
        public bool ApproveRequest { get; set; }
        public bool Approved { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public string Accommodation { get; set; }
        public string PropertyOrService { get; set; }
        public bool IsSearchRequired { get; set; }
        public bool IsService { get; set; }
        public string PropertyUnit { get; set; }
    }
}