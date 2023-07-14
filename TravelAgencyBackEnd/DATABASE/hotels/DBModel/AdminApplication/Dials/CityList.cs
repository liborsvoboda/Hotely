using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TravelAgencyBackEnd.DBModel
{
    [Table("CityList")]
    [Index("CountryId", "City", Name = "IX_CityList", IsUnique = true)]
    public partial class CityList
    {
        public CityList()
        {
            CountryAreaCityLists = new HashSet<CountryAreaCityList>();
            HotelLists = new HashSet<HotelList>();
            InterestAreaCityLists = new HashSet<InterestAreaCityList>();
        }

        [Key]
        public int Id { get; set; }
        public int CountryId { get; set; }
        [Required]
        [StringLength(250)]
        [Unicode(false)]
        public string City { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("CountryId")]
        [InverseProperty("CityLists")]
        public virtual CountryList Country { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("CityLists")]
        public virtual UserList User { get; set; }
        [InverseProperty("City")]
        public virtual ICollection<CountryAreaCityList> CountryAreaCityLists { get; set; }
        [InverseProperty("City")]
        public virtual ICollection<HotelList> HotelLists { get; set; }
        [InverseProperty("City")]
        public virtual ICollection<InterestAreaCityList> InterestAreaCityLists { get; set; }
    }
}
