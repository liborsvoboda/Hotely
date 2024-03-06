using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;

namespace EasyITSystemCenter.Classes {

    public partial class GuestFavoriteList {

        public int Id { get; set; } = 0;
        public int HotelId { get; set; }
        public string Description { get; set; }
        public int GuestId { get; set; }
        public DateTime TimeStamp { get; set; }

        public string HotelName { get; set; }
        public string GuestName { get; set; }
    }
}