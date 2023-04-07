using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class ParameterList
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        public virtual UserList User { get; set; }
    }
}
