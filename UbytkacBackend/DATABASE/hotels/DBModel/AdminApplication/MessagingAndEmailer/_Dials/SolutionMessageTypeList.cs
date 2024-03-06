using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SolutionMessageTypeList")]
    [Index("Name", Name = "IX_SolutionMessageTypeList", IsUnique = true)]
    public partial class SolutionMessageTypeList
    {
        public SolutionMessageTypeList()
        {
            SolutionMessageModuleLists = new HashSet<SolutionMessageModuleList>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? Variables { get; set; }
        public bool AnswerAllowed { get; set; }
        public bool IsSystemOnly { get; set; }
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("SolutionMessageTypeLists")]
        public virtual SolutionUserList User { get; set; } = null!;
        [InverseProperty("MessageType")]
        public virtual ICollection<SolutionMessageModuleList> SolutionMessageModuleLists { get; set; }
    }
}
