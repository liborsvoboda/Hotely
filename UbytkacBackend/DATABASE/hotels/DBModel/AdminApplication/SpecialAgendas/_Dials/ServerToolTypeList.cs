using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("ServerToolTypeList")]
    [Index("Name", Name = "IX_ServerToolTypeList", IsUnique = true)]
    public partial class ServerToolTypeList
    {
        public ServerToolTypeList()
        {
            ServerToolPanelDefinitionLists = new HashSet<ServerToolPanelDefinitionList>();
        }

        [Key]
        public int Id { get; set; }
        public int Sequence { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("ServerToolTypeLists")]
        public virtual SolutionUserList User { get; set; } = null!;
        [InverseProperty("ToolType")]
        public virtual ICollection<ServerToolPanelDefinitionList> ServerToolPanelDefinitionLists { get; set; }
    }
}
