using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UbytkacBackend.DBModel
{
    [Keyless]
    public partial class BasicViewAttachmentList
    {
        public int Id { get; set; }
        [Required]
        [StringLength(150)]
        [Unicode(false)]
        public string FileName { get; set; }
        [StringLength(50)]
        [Unicode(false)]
        public string PartNumber { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
