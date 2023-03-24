using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TravelAgencyBackEnd.DBModel
{
    public partial class HotelRoomList
    {
        public HotelRoomList()
        {
            HotelReservedRoomLists = new HashSet<HotelReservedRoomList>();
        }

        public int Id { get; set; }
        public int HotelId { get; set; }
        public int RoomTypeId { get; set; }
        public double Price { get; set; }
        public int MaxCapacity { get; set; }
        public int RoomsCount { get; set; }
        public int UserId { get; set; }
        public string AccessRole { get; set; }
        public DateTime Timestamp { get; set; }

        [JsonIgnore]
        [ValidateNever]
        public virtual HotelList Hotel { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual HotelRoomTypeList RoomType { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public virtual UserList User { get; set; }
        public virtual ICollection<HotelReservedRoomList> HotelReservedRoomLists { get; set; }
    }
}
