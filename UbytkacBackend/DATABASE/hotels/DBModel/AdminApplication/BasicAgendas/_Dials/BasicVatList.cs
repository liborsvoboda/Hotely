using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("BasicVatList")]
    [Index("Value", "Active", Name = "IX_VatList", IsUnique = true)]
    public partial class BasicVatList
    {
        public BasicVatList()
        {
            BasicItemLists = new HashSet<BasicItemList>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column(TypeName = "numeric(10, 2)")]
        public decimal Value { get; set; }
        public bool Default { get; set; }
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        public int UserId { get; set; }
        [Required]
        public bool? Active { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("BasicVatLists")]
        public virtual SolutionUserList User { get; set; } = null!;
        [InverseProperty("Vat")]
        public virtual ICollection<BasicItemList> BasicItemLists { get; set; }
    }
}
