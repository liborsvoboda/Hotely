using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("ServerToolPanelDefinitionList")]
    [Index("SystemName", Name = "IX_ServerToolPanelDefinitionList", IsUnique = true)]
    public partial class ServerToolPanelDefinitionList
    {
        [Key]
        public int Id { get; set; }
        public int ToolTypeId { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string SystemName { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string Type { get; set; } = null!;
        [StringLength(500)]
        [Unicode(false)]
        public string Command { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string IconName { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string IconColor { get; set; } = null!;
        [StringLength(50)]
        [Unicode(false)]
        public string BackgroundColor { get; set; } = null!;
        [Column(TypeName = "text")]
        public string? Description { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("ToolTypeId")]
        [InverseProperty("ServerToolPanelDefinitionLists")]
        public virtual ServerToolTypeList ToolType { get; set; } = null!;
        [ForeignKey("UserId")]
        [InverseProperty("ServerToolPanelDefinitionLists")]
        public virtual SolutionUserList User { get; set; } = null!;
    }
}
