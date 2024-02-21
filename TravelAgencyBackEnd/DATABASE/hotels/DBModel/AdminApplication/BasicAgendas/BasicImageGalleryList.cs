using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("BasicImageGalleryList")]
    [Index("FileName", Name = "UX_ImageGalleryList", IsUnique = true)]
    public partial class BasicImageGalleryList
    {
        [Key]
        public int Id { get; set; }
        public bool IsPrimary { get; set; }
        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string FileName { get; set; }
        [Required]
        public byte[] Attachment { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("BasicImageGalleryLists")]
        public virtual SolutionUserList User { get; set; }
    }
}
