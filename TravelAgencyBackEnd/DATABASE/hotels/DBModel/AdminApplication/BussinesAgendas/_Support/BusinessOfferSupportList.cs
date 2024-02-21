using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("BusinessOfferSupportList")]
    [Index("DocumentNumber", Name = "IX_OfferItemList")]
    public partial class BusinessOfferSupportList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string DocumentNumber { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string PartNumber { get; set; }
        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string Name { get; set; }
        [Required]
        [StringLength(10)]
        [Unicode(false)]
        public string Unit { get; set; }
        [Column(TypeName = "numeric(10, 2)")]
        public decimal PcsPrice { get; set; }
        [Column(TypeName = "numeric(10, 2)")]
        public decimal Count { get; set; }
        [Column(TypeName = "numeric(10, 2)")]
        public decimal TotalPrice { get; set; }
        [Column(TypeName = "numeric(10, 2)")]
        public decimal Vat { get; set; }
        [Column(TypeName = "numeric(10, 2)")]
        public decimal TotalPriceWithVat { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        public virtual BusinessOfferList DocumentNumberNavigation { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("BusinessOfferSupportLists")]
        public virtual SolutionUserList User { get; set; }
    }
}
