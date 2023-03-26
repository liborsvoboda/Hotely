using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class HotelPropertyFeeList
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public int PropertyId { get; set; }
        public double? Fee { get; set; }
        public int UserId { get; set; }
        public int OwnerId { get; set; }
        public DateTime Timestamp { get; set; }

        public virtual UserList User { get; set; }
    }
}
