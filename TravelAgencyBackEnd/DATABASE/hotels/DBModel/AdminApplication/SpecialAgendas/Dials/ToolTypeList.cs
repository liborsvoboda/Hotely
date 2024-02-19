using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("ToolTypeList")]
    [Index("Name", Name = "IX_ToolTypeList", IsUnique = true)]
    public partial class ToolTypeList
    {
        public ToolTypeList()
        {
            ToolPanelDefinitionLists = new HashSet<ToolPanelDefinitionList>();
        }

        [Key]
        public int Id { get; set; }
        public int Sequence { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("ToolTypeLists")]
        public virtual UserList User { get; set; }
        [InverseProperty("ToolType")]
        public virtual ICollection<ToolPanelDefinitionList> ToolPanelDefinitionLists { get; set; }
    }
}
