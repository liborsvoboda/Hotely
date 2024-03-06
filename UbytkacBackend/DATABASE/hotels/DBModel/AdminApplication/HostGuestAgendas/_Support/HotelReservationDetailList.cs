using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("HotelReservationDetailList")]
    [Index("HotelId", Name = "IX_ReservationsDetailList")]
    [Index("HotelId", "GuestId", Name = "IX_ReservationsDetailList_1")]
    [Index("GuestId", Name = "IX_ReservationsDetailList_2")]
    public partial class HotelReservationDetailList
    {
        [Key]
        public int Id { get; set; }
        public int GuestId { get; set; }
        public int HotelId { get; set; }
        public int ReservationId { get; set; }
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
        [StringLength(2048)]
        [Unicode(false)]
        public string? Message { get; set; }
        public bool GuestSender { get; set; }
        public bool Shown { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("CurrencyId")]
        [InverseProperty("HotelReservationDetailLists")]
        public virtual BasicCurrencyList Currency { get; set; } = null!;
        [ForeignKey("GuestId")]
        [InverseProperty("HotelReservationDetailLists")]
        public virtual GuestList Guest { get; set; } = null!;
        [ForeignKey("HotelId")]
        [InverseProperty("HotelReservationDetailLists")]
        public virtual HotelList Hotel { get; set; } = null!;
        [ForeignKey("ReservationId")]
        [InverseProperty("HotelReservationDetailLists")]
        public virtual HotelReservationList Reservation { get; set; } = null!;
        [ForeignKey("StatusId")]
        [InverseProperty("HotelReservationDetailLists")]
        public virtual HotelReservationStatusList Status { get; set; } = null!;
    }
}
