using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("PropertyOrServiceUnitList")]
    [Index("SystemName", Name = "IX_PropertyOrServiceTypeList", IsUnique = true)]
    public partial class PropertyOrServiceUnitList
    {
        public PropertyOrServiceUnitList()
        {
            PropertyOrServiceTypeLists = new HashSet<PropertyOrServiceTypeList>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(10)]
        [Unicode(false)]
        public string SystemName { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("PropertyOrServiceUnitLists")]
        public virtual SolutionUserList User { get; set; }
        [InverseProperty("PropertyOrServiceUnitType")]
        public virtual ICollection<PropertyOrServiceTypeList> PropertyOrServiceTypeLists { get; set; }
    }
}
