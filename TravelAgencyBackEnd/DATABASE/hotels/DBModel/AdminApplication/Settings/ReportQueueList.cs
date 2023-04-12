using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class ReportQueueList
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public int Sequence { get; set; }
        public string Sql { get; set; }
        public string TableName { get; set; }
        public string Filter { get; set; }
        public string Search { get; set; }
        public string SearchColumnList { get; set; }
        public bool SearchFilterIgnore { get; set; }
        public int? RecId { get; set; }
        public bool RecIdFilterIgnore { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual UserList User { get; set; }
    }
}
