using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("BasicUnitList")]
    [Index("Name", Name = "IX_UnitList", IsUnique = true)]
    public partial class BasicUnitList
    {
        public BasicUnitList()
        {
            BasicItemLists = new HashSet<BasicItemList>();
            BusinessCreditNoteSupportLists = new HashSet<BusinessCreditNoteSupportList>();
            BusinessIncomingInvoiceSupportLists = new HashSet<BusinessIncomingInvoiceSupportList>();
            BusinessIncomingOrderSupportLists = new HashSet<BusinessIncomingOrderSupportList>();
            BusinessOutgoingInvoiceSupportLists = new HashSet<BusinessOutgoingInvoiceSupportList>();
            BusinessOutgoingOrderSupportLists = new HashSet<BusinessOutgoingOrderSupportList>();
            BusinessReceiptSupportLists = new HashSet<BusinessReceiptSupportList>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(10)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        public int UserId { get; set; }
        [Required]
        public bool? Active { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool Default { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("BasicUnitLists")]
        public virtual SolutionUserList User { get; set; } = null!;
        public virtual ICollection<BasicItemList> BasicItemLists { get; set; }
        public virtual ICollection<BusinessCreditNoteSupportList> BusinessCreditNoteSupportLists { get; set; }
        public virtual ICollection<BusinessIncomingInvoiceSupportList> BusinessIncomingInvoiceSupportLists { get; set; }
        public virtual ICollection<BusinessIncomingOrderSupportList> BusinessIncomingOrderSupportLists { get; set; }
        public virtual ICollection<BusinessOutgoingInvoiceSupportList> BusinessOutgoingInvoiceSupportLists { get; set; }
        public virtual ICollection<BusinessOutgoingOrderSupportList> BusinessOutgoingOrderSupportLists { get; set; }
        public virtual ICollection<BusinessReceiptSupportList> BusinessReceiptSupportLists { get; set; }
    }
}
