using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("BusinessIncomingOrderList")]
    [Index("DocumentNumber", Name = "IX_IncomingOrderList", IsUnique = true)]
    [Index("Supplier", Name = "IX_IncomingOrderList_Supplier")]
    public partial class BusinessIncomingOrderList
    {
        public BusinessIncomingOrderList()
        {
            BusinessIncomingOrderSupportLists = new HashSet<BusinessIncomingOrderSupportList>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string DocumentNumber { get; set; }
        [Required]
        [StringLength(512)]
        [Unicode(false)]
        public string Supplier { get; set; }
        [Required]
        [StringLength(512)]
        [Unicode(false)]
        public string Customer { get; set; }
        public bool Storned { get; set; }
        public int TotalCurrencyId { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string CustomerOrderNumber { get; set; }
        [Column(TypeName = "numeric(10, 2)")]
        public decimal TotalPriceWithVat { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("TotalCurrencyId")]
        [InverseProperty("BusinessIncomingOrderLists")]
        public virtual BasicCurrencyList TotalCurrency { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("BusinessIncomingOrderLists")]
        public virtual SolutionUserList User { get; set; }
        public virtual ICollection<BusinessIncomingOrderSupportList> BusinessIncomingOrderSupportLists { get; set; }
    }
}
