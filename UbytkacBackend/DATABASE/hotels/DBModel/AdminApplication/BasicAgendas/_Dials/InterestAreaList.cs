using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("InterestAreaList")]
    [Index("SystemName", Name = "IX_InterestAreaList", IsUnique = true)]
    public partial class InterestAreaList
    {
        public InterestAreaList()
        {
            InterestAreaCityLists = new HashSet<InterestAreaCityList>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string SystemName { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("InterestAreaLists")]
        public virtual SolutionUserList User { get; set; } = null!;
        [InverseProperty("Iac")]
        public virtual ICollection<InterestAreaCityList> InterestAreaCityLists { get; set; }
    }
}
