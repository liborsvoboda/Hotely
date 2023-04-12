using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class UserRoleList
    {
        public UserRoleList()
        {
            UserLists = new HashSet<UserList>();
        }

        public int Id { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public int? UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual ICollection<UserList> UserLists { get; set; }
    }
}
