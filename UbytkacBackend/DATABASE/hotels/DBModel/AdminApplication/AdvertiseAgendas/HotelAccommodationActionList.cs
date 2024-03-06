using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("HotelAccommodationActionList")]
    [Index("HotelId", "HotelActionTypeId", "StartDate", "EndDate", Name = "IX_HotelAccomodationActionList", IsUnique = true)]
    public partial class HotelAccommodationActionList
    {
        public HotelAccommodationActionList()
        {
            HotelReservationLists = new HashSet<HotelReservationList>();
        }

        [Key]
        public int Id { get; set; }
        public int HotelId { get; set; }
        public int HotelActionTypeId { get; set; }
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }
        public int DaysCount { get; set; }
        public double Price { get; set; }
        [Column(TypeName = "text")]
        public string DescriptionCz { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? DescriptionEn { get; set; }
        public bool Top { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("HotelId")]
        [InverseProperty("HotelAccommodationActionLists")]
        public virtual HotelList Hotel { get; set; } = null!;
        [ForeignKey("HotelActionTypeId")]
        [InverseProperty("HotelAccommodationActionLists")]
        public virtual HotelActionTypeList HotelActionType { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("HotelAccommodationActionLists")]
        public virtual SolutionUserList User { get; set; } = null!;
        [InverseProperty("HotelAccommodationAction")]
        public virtual ICollection<HotelReservationList> HotelReservationLists { get; set; }
    }
}
