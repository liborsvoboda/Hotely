using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("HotelReservationList")]
    public partial class HotelReservationList
    {
        public HotelReservationList()
        {
            HotelReservationDetailLists = new HashSet<HotelReservationDetailList>();
            HotelReservedRoomLists = new HashSet<HotelReservedRoomList>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string ReservationNumber { get; set; } = null!;
        public int HotelId { get; set; }
        public int GuestId { get; set; }
        public int StatusId { get; set; }
        public int CurrencyId { get; set; }
        public int? HotelAccommodationActionId { get; set; }
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }
        public double TotalPrice { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string FirstName { get; set; } = null!;
        [StringLength(255)]
        [Unicode(false)]
        public string LastName { get; set; } = null!;
        [StringLength(255)]
        [Unicode(false)]
        public string Street { get; set; } = null!;
        [StringLength(20)]
        [Unicode(false)]
        public string Zipcode { get; set; } = null!;
        [StringLength(150)]
        [Unicode(false)]
        public string City { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string Country { get; set; } = null!;
        [StringLength(20)]
        [Unicode(false)]
        public string Phone { get; set; } = null!;
        [StringLength(255)]
        [Unicode(false)]
        public string Email { get; set; } = null!;
        public DateTime Timestamp { get; set; }

        [ForeignKey("CurrencyId")]
        [InverseProperty("HotelReservationLists")]
        public virtual BasicCurrencyList Currency { get; set; } = null!;
        [ForeignKey("GuestId")]
        [InverseProperty("HotelReservationLists")]
        public virtual GuestList Guest { get; set; } = null!;
        [ForeignKey("HotelId")]
        [InverseProperty("HotelReservationLists")]
        public virtual HotelList Hotel { get; set; } = null!;
        [ForeignKey("HotelAccommodationActionId")]
        [InverseProperty("HotelReservationLists")]
        public virtual HotelAccommodationActionList? HotelAccommodationAction { get; set; }
        [ForeignKey("StatusId")]
        [InverseProperty("HotelReservationLists")]
        public virtual HotelReservationStatusList Status { get; set; } = null!;
        [InverseProperty("Reservation")]
        public virtual HotelReservationReviewList? HotelReservationReviewList { get; set; }
        [InverseProperty("Reservation")]
        public virtual ICollection<HotelReservationDetailList> HotelReservationDetailLists { get; set; }
        [InverseProperty("Reservation")]
        public virtual ICollection<HotelReservedRoomList> HotelReservedRoomLists { get; set; }
    }
}
