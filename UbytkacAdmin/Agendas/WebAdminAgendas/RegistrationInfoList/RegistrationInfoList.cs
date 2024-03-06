using System;

namespace EasyITSystemCenter.Classes {

    public partial class RegistrationInfoList {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = null;
        public int Sequence { get; set; } = 0;
        public string DescriptionCz { get; set; } = null;
        public string DescriptionEn { get; set; } = null;
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}