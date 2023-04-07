using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class HotelPropertyAndServiceList
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public int PropertyOrServiceId { get; set; }
        public bool IsAvailable { get; set; }
        public bool? ValueBit { get; set; }
        public double? Value { get; set; }
        public double? ValueRangeMin { get; set; }
        public double? ValueRangeMax { get; set; }
        public bool Fee { get; set; }
        public double? FeeValue { get; set; }
        public double? FeeRangeMin { get; set; }
        public double? FeeRangeMax { get; set; }
        public bool Approved { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual PropertyOrServiceTypeList IdNavigation { get; set; }
        public virtual UserList User { get; set; }
    }
}
