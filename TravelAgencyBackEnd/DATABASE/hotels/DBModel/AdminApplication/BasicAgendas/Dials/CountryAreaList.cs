using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("CountryAreaList")]
    [Index("SystemName", Name = "IX_CountryAreaList", IsUnique = true)]
    public partial class CountryAreaList
    {
        public CountryAreaList()
        {
            CountryAreaCityLists = new HashSet<CountryAreaCityList>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string SystemName { get; set; }
        public int CountryId { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("CountryId")]
        [InverseProperty("CountryAreaLists")]
        public virtual CountryList Country { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("CountryAreaLists")]
        public virtual UserList User { get; set; }
        [InverseProperty("Icac")]
        public virtual ICollection<CountryAreaCityList> CountryAreaCityLists { get; set; }
    }
}
