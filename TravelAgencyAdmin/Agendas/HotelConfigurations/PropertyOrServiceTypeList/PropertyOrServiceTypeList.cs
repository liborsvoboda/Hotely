using System;

namespace TravelAgencyAdmin.Classes
{

    public partial class PropertyOrServiceTypeList
    {
        public int Id { get; set; } = 0;
        public string SystemName { get; set; } = null;
        public int PropertyOrServiceUnitTypeId { get; set; }
        public bool IsSearchRequired { get; set; }
        public bool IsService { get; set; }
        public bool IsBit { get; set; }
        public bool IsValue { get; set; }
        public bool IsRangeValue { get; set; }
        public bool IsRangeTime { get; set; }
        public bool IsValueRangeAllowed { get; set; }
        public bool SearchDefaultBit { get; set; }
        public string SearchDefaultValue { get; set; } = null;
        public int? SearchDefaultMin { get; set; } = null;
        public int? SearchDefaultMax { get; set; } = null;
        public bool IsFeeInfoRequired { get; set; }
        public bool IsFeeRangeAllowed { get; set; }
        public int UserId { get; set; } = App.UserData.Authentification.Id;
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// Translation Part
        /// </summary>
        public string Translation { get; set; }
        public string PropertyOrServiceUnitType { get; set; }
        
    }

}
