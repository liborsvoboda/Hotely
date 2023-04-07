using System;

namespace TravelAgencyAdmin.Classes
{
    public partial class UserList
    {
        public int Id { get; set; } = 0;
        public int RoleId { get; set; }
        public string UserName { get; set; } = null;
        public string Password { get; set; } = null;
        public string Name { get; set; } = null;
        public string SurName { get; set; } = null;
        public string Description { get; set; } = null;
        public byte[] Photo { get; set; }
        public string PhotoPath { get; set; } = "";
        public bool Active { get; set; } = false;
        public string ApiToken { get; set; } = null;
        public DateTime Timestamp { get; set; }

        public string Translation { get; set; } = null;
    }

}
