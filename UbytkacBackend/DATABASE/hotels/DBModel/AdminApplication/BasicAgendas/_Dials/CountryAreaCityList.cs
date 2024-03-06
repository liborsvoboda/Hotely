using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("CountryAreaCityList")]
    public partial class CountryAreaCityList
    {
        [Key]
        public int Id { get; set; }
        [Column("ICACId")]
        public int Icacid { get; set; }
        public int CityId { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("CityId")]
        [InverseProperty("CountryAreaCityLists")]
        public virtual CityList City { get; set; } = null!;
        [ForeignKey("Icacid")]
        [InverseProperty("CountryAreaCityLists")]
        public virtual CountryAreaList Icac { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("CountryAreaCityLists")]
        public virtual SolutionUserList User { get; set; } = null!;
    }
}
