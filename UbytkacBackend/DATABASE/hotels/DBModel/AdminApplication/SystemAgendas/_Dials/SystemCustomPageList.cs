using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SystemCustomPageList")]
    [Index("PageName", Name = "IX_SystemCustomPageList", IsUnique = true)]
    public partial class SystemCustomPageList
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string InheritedFormType { get; set; } = null!;
        [StringLength(250)]
        [Unicode(false)]
        public string PageName { get; set; } = null!;
        [Unicode(false)]
        public string? Description { get; set; }
        public bool IsInteractAgenda { get; set; }
        public bool IsSystemUrl { get; set; }
        public bool IsServerUrl { get; set; }
        public bool IsOwnServerUrl { get; set; }
        public bool DevModeEnabled { get; set; }
        public bool ShowHelpTab { get; set; }
        public bool HelpTabShowOnly { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? InheritedHelpTabSourceType { get; set; }
        [StringLength(512)]
        [Unicode(false)]
        public string StartupUrl { get; set; } = null!;
        [StringLength(512)]
        [Unicode(false)]
        public string? HelpTabUrl { get; set; }
        [Column("DBTableName")]
        [StringLength(255)]
        [Unicode(false)]
        public string? DbtableName { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string? ColumnName { get; set; }
        [Column("UseIOOverDom")]
        public bool UseIooverDom { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string? InheritedSetName { get; set; }
        [Column("DOMHtmlElementName")]
        [StringLength(255)]
        [Unicode(false)]
        public string? DomhtmlElementName { get; set; }
        [Column("SetWebDataJScriptCmd")]
        [StringLength(1024)]
        [Unicode(false)]
        public string? SetWebDataJscriptCmd { get; set; }
        [Column("GetWebDataJScriptCmd")]
        [StringLength(1024)]
        [Unicode(false)]
        public string? GetWebDataJscriptCmd { get; set; }
        [StringLength(150)]
        [Unicode(false)]
        public string? StartupSubFolder { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string? StartupCommand { get; set; }
        public bool Active { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("SystemCustomPageLists")]
        public virtual SolutionUserList User { get; set; } = null!;
    }
}
