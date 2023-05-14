using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TravelAgencyBackEnd.DBModel
{
    [Keyless]
    public partial class HotelApprovalList
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        [Required]
        [StringLength(250)]
        [Unicode(false)]
        public string Name { get; set; }
        [StringLength(4096)]
        [Unicode(false)]
        public string DescriptionCz { get; set; }
        [StringLength(4096)]
        [Unicode(false)]
        public string DescriptionEn { get; set; }
        public int DefaultCurrencyId { get; set; }
        public bool ApproveRequest { get; set; }
        public bool Approved { get; set; }
        public bool Advertised { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public int TotalCapacity { get; set; }
    }
}
