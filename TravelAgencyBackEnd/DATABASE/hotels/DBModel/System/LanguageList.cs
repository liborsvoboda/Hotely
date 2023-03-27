using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class LanguageList
    {
        public int Id { get; set; }
        public string SystemName { get; set; }
        public string DescriptionCz { get; set; }
        public string DescriptionEn { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual UserList User { get; set; }
        public virtual DocumentTypeList DocumentTypeList { get; set; }
    }
}
