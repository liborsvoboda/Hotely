using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("GuestList")]
    [Index("Email", Name = "IX_GuestList")]
    public partial class GuestList
    {
        public GuestList()
        {
            GuestAdvertiserNoteLists = new HashSet<GuestAdvertiserNoteList>();
            GuestFavoriteLists = new HashSet<GuestFavoriteList>();
            GuestSettingLists = new HashSet<GuestSettingList>();
            HotelReservationDetailLists = new HashSet<HotelReservationDetailList>();
            HotelReservationLists = new HashSet<HotelReservationList>();
            HotelReservationReviewLists = new HashSet<HotelReservationReviewList>();
            SolutionMessageModuleLists = new HashSet<SolutionMessageModuleList>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string Email { get; set; }
        [Required]
        [StringLength(1024)]
        [Unicode(false)]
        public string Password { get; set; }
        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string LastName { get; set; }
        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string Street { get; set; }
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string ZipCode { get; set; }
        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string City { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Country { get; set; }
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string Phone { get; set; }
        [Required]
        public bool Active { get; set; }
        public int? UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("GuestLists")]
        public virtual SolutionUserList User { get; set; }
        [InverseProperty("Guest")]
        public virtual ICollection<GuestAdvertiserNoteList> GuestAdvertiserNoteLists { get; set; }
        [InverseProperty("Guest")]
        public virtual ICollection<GuestFavoriteList> GuestFavoriteLists { get; set; }
        [InverseProperty("Guest")]
        public virtual ICollection<GuestSettingList> GuestSettingLists { get; set; }
        [InverseProperty("Guest")]
        public virtual ICollection<HotelReservationDetailList> HotelReservationDetailLists { get; set; }
        [InverseProperty("Guest")]
        public virtual ICollection<HotelReservationList> HotelReservationLists { get; set; }
        [InverseProperty("Guest")]
        public virtual ICollection<HotelReservationReviewList> HotelReservationReviewLists { get; set; }
        [InverseProperty("Guest")]
        public virtual ICollection<SolutionMessageModuleList> SolutionMessageModuleLists { get; set; }
    }
}
