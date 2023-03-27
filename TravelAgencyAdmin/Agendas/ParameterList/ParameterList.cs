using System;

namespace TravelAgencyAdmin.Classes
{
    public partial class ParameterList
    {
        public int Id { get; set; } = 0;
        public string SystemName { get; set; } = null;
        public string Type { get; set; } = null;
        public string Value { get; set; } = null;
        public string Description { get; set; } = null;
        public int? UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        public string Translation { get; set; } = null;
    }

}
