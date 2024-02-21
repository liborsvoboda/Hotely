using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SystemDocumentAdviceList")]
    [Index("BranchId", "DocumentType", Name = "IX_DocumentAdviceList")]
    public partial class SystemDocumentAdviceList
    {
        [Key]
        public int Id { get; set; }
        public int BranchId { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string DocumentType { get; set; }
        [Required]
        [StringLength(10)]
        [Unicode(false)]
        public string Prefix { get; set; }
        [Required]
        [StringLength(10)]
        [Unicode(false)]
        public string Number { get; set; }
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }
        public int UserId { get; set; }
        [Required]
        public bool? Active { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("BranchId")]
        [InverseProperty("SystemDocumentAdviceLists")]
        public virtual BusinessBranchList Branch { get; set; }
        public virtual SystemDocumentTypeList DocumentTypeNavigation { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("SystemDocumentAdviceLists")]
        public virtual SolutionUserList User { get; set; }
    }
}
