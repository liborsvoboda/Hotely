using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("DocumentationGroupList")]
    [Index("Name", Name = "IX_DocumentationGroupList", IsUnique = true)]
    public partial class DocumentationGroupList
    {
        public DocumentationGroupList()
        {
            DocumentationLists = new HashSet<DocumentationList>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        public int Sequence { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        public int UserId { get; set; }
        [Required]
        public bool Active { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("DocumentationGroupLists")]
        public virtual UserList User { get; set; }
        [InverseProperty("DocumentationGroup")]
        public virtual ICollection<DocumentationList> DocumentationLists { get; set; }
    }
}
