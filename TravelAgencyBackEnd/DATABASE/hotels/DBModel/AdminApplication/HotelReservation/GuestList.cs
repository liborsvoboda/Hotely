using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TravelAgencyBackEnd.DBModel
{
    [Table("GuestList")]
    [Index("Email", Name = "IX_GuestList")]
    public partial class GuestList
    {
        public GuestList()
        {
            HotelReservationDetailLists = new HashSet<HotelReservationDetailList>();
            HotelReservationLists = new HashSet<HotelReservationList>();
            HotelReservationReviewLists = new HashSet<HotelReservationReviewList>();
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
        public DateTime Timestamp { get; set; }

        [InverseProperty("Guest")]
        public virtual ICollection<HotelReservationDetailList> HotelReservationDetailLists { get; set; }
        [InverseProperty("Guest")]
        public virtual ICollection<HotelReservationList> HotelReservationLists { get; set; }
        [InverseProperty("Guest")]
        public virtual ICollection<HotelReservationReviewList> HotelReservationReviewLists { get; set; }
    }
}
