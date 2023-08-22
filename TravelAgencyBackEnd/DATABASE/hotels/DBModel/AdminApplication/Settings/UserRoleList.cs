using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("UserRoleList")]
    [Index("SystemName", Name = "IX_UserRoleList", IsUnique = true)]
    public partial class UserRoleList
    {
        public UserRoleList()
        {
            UserLists = new HashSet<UserList>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string SystemName { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        public int? UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [InverseProperty("Role")]
        public virtual ICollection<UserList> UserLists { get; set; }
    }
}
