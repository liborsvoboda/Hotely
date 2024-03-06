using System;
using System.Windows.Media.Imaging;

namespace EasyITSystemCenter.Classes {

    public partial class SystemSvgIconList {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = null;
        public string SvgIconPath { get; set; } = null;
        public int UserId { get; set; }
        public DateTime TimeStamp { get; set; }

        public BitmapImage BitmapImage { get; set; }
    }
}