using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("BusinessMaturityList")]
    [Index("Name", Name = "IX_MaturityList", IsUnique = true)]
    public partial class BusinessMaturityList
    {
        public BusinessMaturityList()
        {
            BusinessIncomingInvoiceLists = new HashSet<BusinessIncomingInvoiceList>();
            BusinessOutgoingInvoiceLists = new HashSet<BusinessOutgoingInvoiceList>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        public int Value { get; set; }
        public bool Default { get; set; }
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        public int UserId { get; set; }
        [Required]
        public bool? Active { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("BusinessMaturityLists")]
        public virtual SolutionUserList User { get; set; } = null!;
        [InverseProperty("Maturity")]
        public virtual ICollection<BusinessIncomingInvoiceList> BusinessIncomingInvoiceLists { get; set; }
        [InverseProperty("Maturity")]
        public virtual ICollection<BusinessOutgoingInvoiceList> BusinessOutgoingInvoiceLists { get; set; }
    }
}
