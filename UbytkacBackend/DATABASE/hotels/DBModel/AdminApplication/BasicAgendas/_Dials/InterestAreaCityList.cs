using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("InterestAreaCityList")]
    [Index("Iacid", "CityId", Name = "IX_InterestAreaCityList", IsUnique = true)]
    public partial class InterestAreaCityList
    {
        [Key]
        public int Id { get; set; }
        [Column("IACId")]
        public int Iacid { get; set; }
        public int CityId { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("CityId")]
        [InverseProperty("InterestAreaCityLists")]
        public virtual CityList City { get; set; } = null!;
        [ForeignKey("Iacid")]
        [InverseProperty("InterestAreaCityLists")]
        public virtual InterestAreaList Iac { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("InterestAreaCityLists")]
        public virtual SolutionUserList User { get; set; } = null!;
    }
}
