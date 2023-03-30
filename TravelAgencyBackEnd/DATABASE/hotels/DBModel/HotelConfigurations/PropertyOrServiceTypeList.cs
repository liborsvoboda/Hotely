using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class PropertyOrServiceTypeList
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public int PropertyOrServiceUnitTypeId { get; set; }
        public bool IsSearchRequired { get; set; }
        public bool IsService { get; set; }
        public bool IsBit { get; set; }
        public bool IsValue { get; set; }
        public bool IsRangeValue { get; set; }
        public bool IsRangeTime { get; set; }
        public bool IsValueRangeAllowed { get; set; }
        public bool? SearchDefaultBit { get; set; }
        public string SearchDefaultValue { get; set; }
        public int? SearchDefaultMin { get; set; }
        public int? SearchDefaultMax { get; set; }
        public bool IsFeeInfoRequired { get; set; }
        public bool IsFeeRangeAllowed { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual PropertyOrServiceUnitList PropertyOrServiceUnitType { get; set; }
        public virtual UserList User { get; set; }
        public virtual HotelPropertyAndServiceList HotelPropertyAndServiceList { get; set; }
    }
}
