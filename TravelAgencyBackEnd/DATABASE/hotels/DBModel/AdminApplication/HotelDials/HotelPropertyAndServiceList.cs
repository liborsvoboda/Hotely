using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("HotelPropertyAndServiceList")]
    [Index("HotelId", "PropertyOrServiceId", Name = "IX_HotelPropertyAndServiceList", IsUnique = true)]
    [Index("HotelId", Name = "IX_HotelPropertyAndServiceListHotel")]
    public partial class HotelPropertyAndServiceList
    {
        [Key]
        public int Id { get; set; }
        public int HotelId { get; set; }
        public int PropertyOrServiceId { get; set; }
        public bool IsAvailable { get; set; }
        public double? Value { get; set; }
        public double? ValueRangeMin { get; set; }
        public double? ValueRangeMax { get; set; }
        public bool Fee { get; set; }
        public double? FeeValue { get; set; }
        public double? FeeRangeMin { get; set; }
        public double? FeeRangeMax { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("HotelId")]
        [InverseProperty("HotelPropertyAndServiceLists")]
        public virtual HotelList Hotel { get; set; }
        [ForeignKey("PropertyOrServiceId")]
        [InverseProperty("HotelPropertyAndServiceLists")]
        public virtual PropertyOrServiceTypeList PropertyOrService { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("HotelPropertyAndServiceLists")]
        public virtual UserList User { get; set; }
    }
}
