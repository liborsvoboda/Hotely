using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class DocumentAdviceList
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int DocumentTypeId { get; set; }
        public string Prefix { get; set; }
        public string Number { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UserId { get; set; }
        public int OwnerId { get; set; }
        public string AccessRole { get; set; }
        public DateTime TimeStamp { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public virtual BranchList Branch { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual DocumentTypeList DocumentType { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual UserList Owner { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual UserList User { get; set; }
    }
}
