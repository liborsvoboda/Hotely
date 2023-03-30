using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class PropertyOrServiceUnitList
    {
        public PropertyOrServiceUnitList()
        {
            PropertyOrServiceTypeLists = new HashSet<PropertyOrServiceTypeList>();
        }

        public int Id { get; set; }
        public string SystemName { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        public virtual UserList User { get; set; }
        public virtual ICollection<PropertyOrServiceTypeList> PropertyOrServiceTypeLists { get; set; }
    }
}
