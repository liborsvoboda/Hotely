using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("BusinessExchangeRateList")]
    public partial class BusinessExchangeRateList
    {
        [Key]
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Value { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ValidFrom { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ValidTo { get; set; }
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        public int UserId { get; set; }
        [Required]
        public bool? Active { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("CurrencyId")]
        [InverseProperty("BusinessExchangeRateLists")]
        public virtual BasicCurrencyList Currency { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("BusinessExchangeRateLists")]
        public virtual SolutionUserList User { get; set; } = null!;
    }
}
