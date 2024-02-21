using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("BusinessAddressList")]
    [Index("AddressType", Name = "IX_AddressList")]
    public partial class BusinessAddressList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string AddressType { get; set; }
        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string CompanyName { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string ContactName { get; set; }
        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string Street { get; set; }
        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string City { get; set; }
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string PostCode { get; set; }
        [Required]
        [StringLength(20)]
        [Unicode(false)]
        public string Phone { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string Email { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string BankAccount { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string Ico { get; set; }
        [StringLength(20)]
        [Unicode(false)]
        public string Dic { get; set; }
        public int UserId { get; set; }
        [Required]
        public bool? Active { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("BusinessAddressLists")]
        public virtual SolutionUserList User { get; set; }
    }
}
