using System;
using System.Collections.Generic;

#nullable disable

namespace TABackend.DBModel
{
    public partial class SavedHotel
    {
        public int GuestId { get; set; }
        public int HotelId { get; set; }

        public virtual Guest Guest { get; set; }
        public virtual Hotel Hotel { get; set; }
    }
}
