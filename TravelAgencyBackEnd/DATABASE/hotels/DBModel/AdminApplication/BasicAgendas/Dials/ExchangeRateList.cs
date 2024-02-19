using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("ExchangeRateList")]
    public partial class ExchangeRateList
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
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("CurrencyId")]
        [InverseProperty("ExchangeRateLists")]
        public virtual CurrencyList Currency { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("ExchangeRateLists")]
        public virtual UserList User { get; set; }
    }
}
