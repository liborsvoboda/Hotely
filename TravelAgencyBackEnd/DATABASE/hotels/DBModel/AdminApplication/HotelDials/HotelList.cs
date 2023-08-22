using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("HotelList")]
    [Index("Name", "CityId", "UserId", Name = "IX_Hotels", IsUnique = true)]
    public partial class HotelList
    {
        public HotelList()
        {
            HotelAccommodationActionLists = new HashSet<HotelAccommodationActionList>();
            HotelImagesLists = new HashSet<HotelImagesList>();
            HotelPropertyAndServiceLists = new HashSet<HotelPropertyAndServiceList>();
            HotelReservationDetailLists = new HashSet<HotelReservationDetailList>();
            HotelReservationLists = new HashSet<HotelReservationList>();
            HotelReservationReviewLists = new HashSet<HotelReservationReviewList>();
            HotelReservedRoomLists = new HashSet<HotelReservedRoomList>();
            HotelRoomLists = new HashSet<HotelRoomList>();
        }

        [Key]
        public int Id { get; set; }
        public int CountryId { get; set; }
        public int CityId { get; set; }
        [Required]
        [StringLength(250)]
        [Unicode(false)]
        public string Name { get; set; }
        [Required]
        [Unicode(false)]
        public string DescriptionCz { get; set; }
        [Unicode(false)]
        public string DescriptionEn { get; set; }
        public int DefaultCurrencyId { get; set; }
        public bool ApproveRequest { get; set; }
        public bool Approved { get; set; }
        [Required]
        public bool Advertised { get; set; }
        public int TotalCapacity { get; set; }
        [Column(TypeName = "decimal(2, 2)")]
        public decimal AverageRating { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public bool Top { get; set; }
        public DateTime? TopDate { get; set; }
        public int TopShown { get; set; }
        public DateTime? LastTopShown { get; set; }
        public int Shown { get; set; }

        [ForeignKey("CityId")]
        [InverseProperty("HotelLists")]
        public virtual CityList City { get; set; }
        [ForeignKey("CountryId")]
        [InverseProperty("HotelLists")]
        public virtual CountryList Country { get; set; }
        [ForeignKey("DefaultCurrencyId")]
        [InverseProperty("HotelLists")]
        public virtual CurrencyList DefaultCurrency { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("HotelLists")]
        public virtual UserList User { get; set; }
        [InverseProperty("Hotel")]
        public virtual ICollection<HotelAccommodationActionList> HotelAccommodationActionLists { get; set; }
        [InverseProperty("Hotel")]
        public virtual ICollection<HotelImagesList> HotelImagesLists { get; set; }
        [InverseProperty("Hotel")]
        public virtual ICollection<HotelPropertyAndServiceList> HotelPropertyAndServiceLists { get; set; }
        [InverseProperty("Hotel")]
        public virtual ICollection<HotelReservationDetailList> HotelReservationDetailLists { get; set; }
        [InverseProperty("Hotel")]
        public virtual ICollection<HotelReservationList> HotelReservationLists { get; set; }
        [InverseProperty("Hotel")]
        public virtual ICollection<HotelReservationReviewList> HotelReservationReviewLists { get; set; }
        [InverseProperty("Hotel")]
        public virtual ICollection<HotelReservedRoomList> HotelReservedRoomLists { get; set; }
        [InverseProperty("Hotel")]
        public virtual ICollection<HotelRoomList> HotelRoomLists { get; set; }
    }
}
