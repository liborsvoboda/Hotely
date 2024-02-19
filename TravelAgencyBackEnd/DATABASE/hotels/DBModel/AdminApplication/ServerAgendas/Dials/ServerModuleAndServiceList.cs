using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("ServerModuleAndServiceList")]
    [Index("Name", Name = "IX_ServerModuleAndServiceList", IsUnique = true)]
    [Index("UrlSubPath", Name = "IX_ServerModuleAndServiceList_2", IsUnique = true)]
    public partial class ServerModuleAndServiceList
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string InheritedPageType { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string Name { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string UrlSubPath { get; set; }
        [StringLength(2048)]
        [Unicode(false)]
        public string OptionalConfiguration { get; set; }
        [StringLength(500)]
        [Unicode(false)]
        public string AllowedRoles { get; set; }
        public bool RestrictedAccess { get; set; }
        [StringLength(100)]
        [Unicode(false)]
        public string RedirectPathOnError { get; set; }
        [Column(TypeName = "text")]
        public string CustomHtmlContent { get; set; }
        public bool IsLoginModule { get; set; }
        public bool PathSetAllowed { get; set; }
        public bool RestrictionSetAllowed { get; set; }
        public bool HtmlSetAllowed { get; set; }
        public bool RedirectSetAllowed { get; set; }
        [Required]
        public bool? Active { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("ServerModuleAndServiceLists")]
        public virtual UserList User { get; set; }
    }
}
