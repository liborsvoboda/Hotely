using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("BusinessOutgoingOrderList")]
    [Index("DocumentNumber", Name = "IX_OutgoingOrderList", IsUnique = true)]
    [Index("Supplier", Name = "IX_OutgoingOrderList_Supplier")]
    public partial class BusinessOutgoingOrderList
    {
        public BusinessOutgoingOrderList()
        {
            BusinessOutgoingOrderSupportLists = new HashSet<BusinessOutgoingOrderSupportList>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string DocumentNumber { get; set; } = null!;
        [StringLength(512)]
        [Unicode(false)]
        public string Customer { get; set; } = null!;
        [StringLength(512)]
        [Unicode(false)]
        public string Supplier { get; set; } = null!;
        public bool Storned { get; set; }
        public int TotalCurrencyId { get; set; }
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        [Column(TypeName = "numeric(10, 2)")]
        public decimal TotalPriceWithVat { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("TotalCurrencyId")]
        [InverseProperty("BusinessOutgoingOrderLists")]
        public virtual BasicCurrencyList TotalCurrency { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("BusinessOutgoingOrderLists")]
        public virtual SolutionUserList User { get; set; } = null!;
        public virtual ICollection<BusinessOutgoingOrderSupportList> BusinessOutgoingOrderSupportLists { get; set; }
    }
}
