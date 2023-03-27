using TravelAgencyBackEnd.DBModel;

namespace TravelAgencyBackEnd.CoreClasses
{
    public class ApiClassesExtension
    {
        public partial class SetReportFilter
        {
            public string TableName { get; set; } = null;
            public string Filter { get; set; } = null;
            public string Search { get; set; } = null;
            public int RecId { get; set; } = 0;
        }
    }

}
