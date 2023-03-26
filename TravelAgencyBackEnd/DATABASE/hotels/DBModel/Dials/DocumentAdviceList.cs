using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class DocumentAdviceList
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int DocumentTypeId { get; set; }
        public string Prefix { get; set; }
        public string Number { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserId { get; set; }
        public int OwnerId { get; set; }
        public DateTime TimeStamp { get; set; }

        public virtual BranchList Branch { get; set; }
        public virtual DocumentTypeList DocumentType { get; set; }
        public virtual UserList User { get; set; }
    }
}
