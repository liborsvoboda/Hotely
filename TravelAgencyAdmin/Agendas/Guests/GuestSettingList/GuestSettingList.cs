using System;

namespace UbytkacAdmin.Classes {

    public partial class GuestSettingList {
        public int Id { get; set; } = 0;
        public string Key { get; set; } = null;
        public string Value { get; set; } = null;
        public int GuestId { get; set; }
        public DateTime TimeStamp { get; set; }

        public string GuestName { get; set; }
        public string KeyTranslation { get; set; }
    }
}