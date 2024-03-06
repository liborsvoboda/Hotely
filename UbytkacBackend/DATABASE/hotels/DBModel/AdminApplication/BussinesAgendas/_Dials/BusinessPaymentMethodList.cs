using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("BusinessPaymentMethodList")]
    [Index("Name", Name = "IX_PaymentMethodList", IsUnique = true)]
    public partial class BusinessPaymentMethodList
    {
        public BusinessPaymentMethodList()
        {
            BusinessIncomingInvoiceLists = new HashSet<BusinessIncomingInvoiceList>();
            BusinessOutgoingInvoiceLists = new HashSet<BusinessOutgoingInvoiceList>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        public bool Default { get; set; }
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        public bool AutoGenerateReceipt { get; set; }
        public bool AllowGenerateReceipt { get; set; }
        public int UserId { get; set; }
        [Required]
        public bool? Active { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("BusinessPaymentMethodLists")]
        public virtual SolutionUserList User { get; set; } = null!;
        [InverseProperty("PaymentMethod")]
        public virtual ICollection<BusinessIncomingInvoiceList> BusinessIncomingInvoiceLists { get; set; }
        [InverseProperty("PaymentMethod")]
        public virtual ICollection<BusinessOutgoingInvoiceList> BusinessOutgoingInvoiceLists { get; set; }
    }
}
