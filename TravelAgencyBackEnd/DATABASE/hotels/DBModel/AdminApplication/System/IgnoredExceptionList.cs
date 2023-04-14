using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class IgnoredExceptionList
    {
        public int Id { get; set; }
        public string ErrorNumber { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        public virtual UserList User { get; set; }
    }
}
