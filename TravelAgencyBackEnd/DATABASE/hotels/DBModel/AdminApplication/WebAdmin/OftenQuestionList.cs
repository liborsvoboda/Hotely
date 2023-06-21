using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TravelAgencyBackEnd.DBModel
{
    [Table("OftenQuestionList")]
    [Index("Name", Name = "IX_OftenQuestionList", IsUnique = true)]
    public partial class OftenQuestionList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        public int Sequence { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string DescriptionCz { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string DescriptionEn { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("OftenQuestionLists")]
        public virtual UserList User { get; set; }
    }
}
