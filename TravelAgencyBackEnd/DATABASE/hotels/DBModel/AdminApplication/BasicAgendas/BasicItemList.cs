using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("BasicItemList")]
    [Index("PartNumber", Name = "IX_ItemList", IsUnique = true)]
    public partial class BasicItemList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string PartNumber { get; set; }
        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string Name { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [Required]
        [StringLength(10)]
        [Unicode(false)]
        public string Unit { get; set; }
        [Column(TypeName = "numeric(10, 2)")]
        public decimal Price { get; set; }
        public int VatId { get; set; }
        public int CurrencyId { get; set; }
        public int UserId { get; set; }
        [Required]
        public bool? Active { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("CurrencyId")]
        [InverseProperty("BasicItemLists")]
        public virtual BasicCurrencyList Currency { get; set; }
        public virtual BasicUnitList UnitNavigation { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("BasicItemLists")]
        public virtual SolutionUserList User { get; set; }
        [ForeignKey("VatId")]
        [InverseProperty("BasicItemLists")]
        public virtual BasicVatList Vat { get; set; }
    }
}
