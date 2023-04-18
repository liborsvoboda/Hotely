using System;

namespace TravelAgencyAdmin.Classes
{
    public class HotelImagesList {
        public int Id { get; set; } = 0;
        public int HotelId { get; set; }
        public bool IsPrimary { get; set; }
        public string FileName { get; set; }
        public byte[] Attachment { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        public string Hotel { get; set; }
    }
}



