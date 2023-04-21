using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using TravelAgencyBackEnd.DBModel;
using Microsoft.AspNetCore.Identity;

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


        public class SimpleImageList : HotelList {
            public bool isPrimary { get; set; }
            public string FileName { get; set; }
        }

    }

}
