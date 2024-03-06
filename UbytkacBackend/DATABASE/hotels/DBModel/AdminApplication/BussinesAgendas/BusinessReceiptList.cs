using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("BusinessReceiptList")]
    [Index("DocumentNumber", Name = "IX_ReceiptList", IsUnique = true)]
    public partial class BusinessReceiptList
    {
        public BusinessReceiptList()
        {
            BusinessOutgoingInvoiceLists = new HashSet<BusinessOutgoingInvoiceList>();
            BusinessReceiptSupportLists = new HashSet<BusinessReceiptSupportList>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string DocumentNumber { get; set; } = null!;
        [StringLength(20)]
        [Unicode(false)]
        public string? InvoiceNumber { get; set; }
        [StringLength(512)]
        [Unicode(false)]
        public string Supplier { get; set; } = null!;
        [StringLength(512)]
        [Unicode(false)]
        public string Customer { get; set; } = null!;
        public DateTime IssueDate { get; set; }
        public bool Storned { get; set; }
        public int TotalCurrencyId { get; set; }
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        [Column(TypeName = "numeric(10, 2)")]
        public decimal TotalPriceWithVat { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        public virtual BusinessOutgoingInvoiceList? InvoiceNumberNavigation { get; set; }
        [ForeignKey("TotalCurrencyId")]
        [InverseProperty("BusinessReceiptLists")]
        public virtual BasicCurrencyList TotalCurrency { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("BusinessReceiptLists")]
        public virtual SolutionUserList User { get; set; } = null!;
        [InverseProperty("Receipt")]
        public virtual ICollection<BusinessOutgoingInvoiceList> BusinessOutgoingInvoiceLists { get; set; }
        public virtual ICollection<BusinessReceiptSupportList> BusinessReceiptSupportLists { get; set; }
    }
}
