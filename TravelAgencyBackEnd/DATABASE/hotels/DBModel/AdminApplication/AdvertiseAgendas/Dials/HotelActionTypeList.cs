using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Table("HotelActionTypeList")]
    [Index("SystemName", Name = "IX_HotelActionList", IsUnique = true)]
    public partial class HotelActionTypeList
    {
        public HotelActionTypeList()
        {
            HotelAccommodationActionLists = new HashSet<HotelAccommodationActionList>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [Unicode(false)]
        public string SystemName { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        [ForeignKey("UserId")]
        [InverseProperty("HotelActionTypeLists")]
        public virtual SolutionUserList User { get; set; }
        [InverseProperty("HotelActionType")]
        public virtual ICollection<HotelAccommodationActionList> HotelAccommodationActionLists { get; set; }
    }
}
