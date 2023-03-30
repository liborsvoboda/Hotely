using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class HotelPropertyAndServiceList
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public int PropertyOrServiceId { get; set; }
        public string Value { get; set; }
        public string Valuerange { get; set; }
        public double? Fee { get; set; }
        public string FeeRange { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual PropertyOrServiceTypeList IdNavigation { get; set; }
    }
}
