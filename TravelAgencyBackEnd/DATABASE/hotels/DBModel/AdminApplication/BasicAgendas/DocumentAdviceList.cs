using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("DocumentAdviceList")]
    [Index("BranchId", "DocumentTypeId", "StartDate", "UserId", Name = "IX_DocumentAdviceList", IsUnique = true)]
    public partial class DocumentAdviceList
    {
        [Key]
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int DocumentTypeId { get; set; }
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
        public DateTime TimeStamp { get; set; }

        [ForeignKey("BranchId")]
        [InverseProperty("DocumentAdviceLists")]
        public virtual BranchList Branch { get; set; }
        [ForeignKey("DocumentTypeId")]
        [InverseProperty("DocumentAdviceLists")]
        public virtual DocumentTypeList DocumentType { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("DocumentAdviceLists")]
        public virtual UserList User { get; set; }
    }
}
