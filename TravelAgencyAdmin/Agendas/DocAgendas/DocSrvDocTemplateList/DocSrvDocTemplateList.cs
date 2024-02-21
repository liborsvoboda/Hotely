using System;

namespace EasyITSystemCenter.Classes {

    public partial class DocSrvDocTemplateList {
        public int Id { get; set; } = 0;
        public int GroupId { get; set; }
        public int Sequence { get; set; }
        public string Name { get; set; } = null;
        public string Description { get; set; }
        public string Template { get; set; }
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        public string GroupName { get; set; } = null;
    }
}