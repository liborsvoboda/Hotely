using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class ReportList
    {
        public int Id { get; set; }
        public string PageName { get; set; }
        public string SystemName { get; set; }
        public bool JoinedId { get; set; }
        public string Description { get; set; }
        public string ReportPath { get; set; }
        public byte[] File { get; set; }
        public int UserId { get; set; }
        public string AccessRole { get; set; }
        public DateTime Timestamp { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public virtual UserList User { get; set; }
    }
}
