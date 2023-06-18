using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TravelAgencyBackEnd.DBModel
{
    [Table("PropertyOrServiceTypeList")]
    [Index("SystemName", Name = "IX_PropertyOrServiceList", IsUnique = true)]
    public partial class PropertyOrServiceTypeList
    {
        public PropertyOrServiceTypeList()
        {
            HotelPropertyAndServiceLists = new HashSet<HotelPropertyAndServiceList>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string SystemName { get; set; }
        public int? PropertyGroupId { get; set; }
        public int PropertyOrServiceUnitTypeId { get; set; }
        public bool IsSearchRequired { get; set; }
        public bool IsService { get; set; }
        public bool IsBit { get; set; }
        public bool IsValue { get; set; }
        public bool IsRangeValue { get; set; }
        public bool IsRangeTime { get; set; }
        public bool IsValueRangeAllowed { get; set; }
        public bool? SearchDefaultBit { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string SearchDefaultValue { get; set; }
        public int? SearchDefaultMin { get; set; }
        public int? SearchDefaultMax { get; set; }
        [Column("isFeeInfoRequired")]
        public bool IsFeeInfoRequired { get; set; }
        [Column("isFeeRangeAllowed")]
        public bool IsFeeRangeAllowed { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("PropertyGroupId")]
        [InverseProperty("PropertyOrServiceTypeLists")]
        public virtual PropertyGroupList PropertyGroup { get; set; }
        [ForeignKey("PropertyOrServiceUnitTypeId")]
        [InverseProperty("PropertyOrServiceTypeLists")]
        public virtual PropertyOrServiceUnitList PropertyOrServiceUnitType { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("PropertyOrServiceTypeLists")]
        public virtual UserList User { get; set; }
        [InverseProperty("PropertyOrService")]
        public virtual ICollection<HotelPropertyAndServiceList> HotelPropertyAndServiceLists { get; set; }
    }
}
