using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SystemDocumentTypeList")]
    [Index("SystemName", Name = "IX_DocumentTypeList", IsUnique = true)]
    public partial class SystemDocumentTypeList
    {
        public SystemDocumentTypeList()
        {
            SystemDocumentAdviceLists = new HashSet<SystemDocumentAdviceList>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string SystemName { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("SystemDocumentTypeLists")]
        public virtual SolutionUserList User { get; set; }
        public virtual ICollection<SystemDocumentAdviceList> SystemDocumentAdviceLists { get; set; }
    }
}
