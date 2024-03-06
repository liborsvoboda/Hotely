using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("CountryList")]
    [Index("SystemName", Name = "IX_Country", IsUnique = true)]
    public partial class CountryList
    {
        public CountryList()
        {
            CityLists = new HashSet<CityList>();
            CountryAreaLists = new HashSet<CountryAreaList>();
            HotelLists = new HashSet<HotelList>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string SystemName { get; set; } = null!;
        [StringLength(10)]
        [Unicode(false)]
        public string IsoCode { get; set; } = null!;
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("CountryLists")]
        public virtual SolutionUserList User { get; set; } = null!;
        [InverseProperty("Country")]
        public virtual ICollection<CityList> CityLists { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<CountryAreaList> CountryAreaLists { get; set; }
        [InverseProperty("Country")]
        public virtual ICollection<HotelList> HotelLists { get; set; }
    }
}
