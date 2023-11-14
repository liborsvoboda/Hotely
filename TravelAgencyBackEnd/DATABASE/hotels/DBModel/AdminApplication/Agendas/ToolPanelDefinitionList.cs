using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("ToolPanelDefinitionList")]
    [Index("SystemName", Name = "IX_ToolPanelDefinitionList", IsUnique = true)]
    public partial class ToolPanelDefinitionList
    {
        [Key]
        public int Id { get; set; }
        public int ToolTypeId { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string SystemName { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Type { get; set; }
        [Required]
        [StringLength(500)]
        [Unicode(false)]
        public string Command { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string IconName { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string IconColor { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string BackgroundColor { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("ToolTypeId")]
        [InverseProperty("ToolPanelDefinitionLists")]
        public virtual ToolTypeList ToolType { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("ToolPanelDefinitionLists")]
        public virtual UserList User { get; set; }
    }
}
