using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("PropertyGroupList")]
    [Index("SystemName", Name = "IX_PropertyGroupList", IsUnique = true)]
    public partial class PropertyGroupList
    {
        public PropertyGroupList()
        {
            PropertyOrServiceTypeLists = new HashSet<PropertyOrServiceTypeList>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string SystemName { get; set; }
        public int Sequence { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("PropertyGroupLists")]
        public virtual SolutionUserList User { get; set; }
        [InverseProperty("PropertyGroup")]
        public virtual ICollection<PropertyOrServiceTypeList> PropertyOrServiceTypeLists { get; set; }
    }
}
