using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class SystemFailList
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public DateTime TimeStamp { get; set; }

        public virtual UserList User { get; set; }
    }
}
