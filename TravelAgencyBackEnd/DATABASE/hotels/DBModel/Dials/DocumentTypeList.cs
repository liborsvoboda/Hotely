using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class DocumentTypeList
    {
        public DocumentTypeList()
        {
            DocumentAdviceLists = new HashSet<DocumentAdviceList>();
        }

        public int Id { get; set; }
        public string SystemName { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual UserList User { get; set; }
        public virtual ICollection<DocumentAdviceList> DocumentAdviceLists { get; set; }
    }
}
