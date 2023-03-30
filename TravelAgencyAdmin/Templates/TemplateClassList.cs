using System;

namespace TravelAgencyAdmin.Classes
{

    public partial class TemplateClassList
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = null;
        public string Description { get; set; } = null;
        public int UserId { get; set; }
        public bool Active { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public partial class ExtendedTemplateClassList : TemplateClassList
    {
        public string TotalCurrency { get; set; }
    }
}
