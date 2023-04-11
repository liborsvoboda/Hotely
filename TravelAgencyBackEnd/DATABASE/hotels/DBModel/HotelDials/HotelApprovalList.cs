using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class HotelApprovalList
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public string Name { get; set; }
        public string DescriptionCz { get; set; }
        public string DescriptionEn { get; set; }
        public int DefaultCurrencyId { get; set; }
        public bool ApproveRequest { get; set; }
        public bool Approved { get; set; }
        public bool Advertised { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
