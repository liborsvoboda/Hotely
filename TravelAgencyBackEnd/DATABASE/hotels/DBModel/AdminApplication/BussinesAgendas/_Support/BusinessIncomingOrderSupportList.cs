using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("BusinessIncomingOrderSupportList")]
    [Index("DocumentNumber", Name = "IX_IncomingOrderItemList")]
    public partial class BusinessIncomingOrderSupportList
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

        public virtual BusinessIncomingOrderList DocumentNumberNavigation { get; set; }
        public virtual BasicUnitList UnitNavigation { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("BusinessIncomingOrderSupportLists")]
        public virtual SolutionUserList User { get; set; }
    }
}
