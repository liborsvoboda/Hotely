using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("BusinessIncomingInvoiceList")]
    [Index("DocumentNumber", Name = "IX_IncomingInvoiceList", IsUnique = true)]
    public partial class BusinessIncomingInvoiceList
    {
        public BusinessIncomingInvoiceList()
        {
            BusinessIncomingInvoiceSupportLists = new HashSet<BusinessIncomingInvoiceSupportList>();
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
        public DateTime IssueDate { get; set; }
        public DateTime TaxDate { get; set; }
        public DateTime MaturityDate { get; set; }
        public int PaymentMethodId { get; set; }
        public int MaturityId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string OrderNumber { get; set; }
        public bool Storned { get; set; }
        public int PaymentStatusId { get; set; }
        public int TotalCurrencyId { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [Column(TypeName = "numeric(10, 2)")]
        public decimal TotalPriceWithVat { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("MaturityId")]
        [InverseProperty("BusinessIncomingInvoiceLists")]
        public virtual BusinessMaturityList Maturity { get; set; }
        [ForeignKey("PaymentMethodId")]
        [InverseProperty("BusinessIncomingInvoiceLists")]
        public virtual BusinessPaymentMethodList PaymentMethod { get; set; }
        [ForeignKey("PaymentStatusId")]
        [InverseProperty("BusinessIncomingInvoiceLists")]
        public virtual BusinessPaymentStatusList PaymentStatus { get; set; }
        [ForeignKey("TotalCurrencyId")]
        [InverseProperty("BusinessIncomingInvoiceLists")]
        public virtual BasicCurrencyList TotalCurrency { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("BusinessIncomingInvoiceLists")]
        public virtual SolutionUserList User { get; set; }
        public virtual ICollection<BusinessIncomingInvoiceSupportList> BusinessIncomingInvoiceSupportLists { get; set; }
    }
}
