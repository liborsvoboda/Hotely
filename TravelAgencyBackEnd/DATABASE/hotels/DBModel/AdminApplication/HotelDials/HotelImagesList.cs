using System;
using System.Collections.Generic;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class HotelImagesList
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public bool IsPrimary { get; set; }
        public string FileName { get; set; }
        public byte[] Attachment { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        public virtual HotelList Hotel { get; set; }
        public virtual UserList User { get; set; }
    }
}
