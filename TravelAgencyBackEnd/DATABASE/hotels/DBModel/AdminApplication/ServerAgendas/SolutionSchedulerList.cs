using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("SolutionSchedulerList")]
    [Index("Name", Name = "IX_SolutionSchedulerList", IsUnique = true)]
    public partial class SolutionSchedulerList
    {
        public SolutionSchedulerList()
        {
            SolutionSchedulerProcessLists = new HashSet<SolutionSchedulerProcessList>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string InheritedGroupName { get; set; }
        [Required]
        [StringLength(255)]
        [Unicode(false)]
        public string Name { get; set; }
        public int Sequence { get; set; }
        [StringLength(255)]
        [Unicode(false)]
        public string Email { get; set; }
        [Required]
        [Column(TypeName = "text")]
        public string Data { get; set; }
        [Column(TypeName = "text")]
        public string Description { get; set; }
        public bool StartNowOnly { get; set; }
        public DateTime? StartAt { get; set; }
        public DateTime? FinishAt { get; set; }
        public int Interval { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string InheritedIntervalType { get; set; }
        public int UserId { get; set; }
        [Required]
        public bool Active { get; set; }
        public DateTime TimeStamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("SolutionSchedulerLists")]
        public virtual SolutionUserList User { get; set; }
        [InverseProperty("ScheduledTask")]
        public virtual ICollection<SolutionSchedulerProcessList> SolutionSchedulerProcessLists { get; set; }
    }
}
