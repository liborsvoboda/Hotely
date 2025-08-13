using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("CreditPackageList")]
    [Index("SystemName", Name = "IX_CreditPackageList", IsUnique = true)]
    public partial class CreditPackageList
    {
        [Key]
        public int Id { get; set; }
        public int Sequence { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string SystemName { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        public int CreditCount { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal CreditPrice { get; set; }
        public int CurrencyId { get; set; }
        public int UserId { get; set; }
        [Required]
        public bool Active { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("CurrencyId")]
        [InverseProperty("CreditPackageLists")]
        public virtual BasicCurrencyList Currency { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("CreditPackageLists")]
        public virtual SolutionUserList User { get; set; } = null!;
    }
}
