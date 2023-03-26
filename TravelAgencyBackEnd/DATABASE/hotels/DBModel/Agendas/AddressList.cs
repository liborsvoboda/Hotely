using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class AddressList
    {
        public int Id { get; set; }
        public string AddressType { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string BankAccount { get; set; }
        public string Ico { get; set; }
        public string Dic { get; set; }
        public int UserId { get; set; }
        public int OwnerId { get; set; }
        public DateTime TimeStamp { get; set; }

        public virtual UserList User { get; set; }
    }
}
