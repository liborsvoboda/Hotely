using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Web.UI.WebControls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace TravelAgencyAdmin.GlobalClasses {
    class UriToBitmapConverter : IValueConverter {

        public object Convert
        (
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
        ) {
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.DecodePixelWidth = 200;
            bi.CacheOption = BitmapCacheOption.OnLoad;
            bi.UriSource = new Uri(value.ToString());
            bi.EndInit();
            return bi;
        }

        public object ConvertBack
        (
            object value,
            Type targetType,
            object parameter,
            CultureInfo culture
         ) {
            throw new Exception("The method or operation is not implemented.");
        }

    }

    public class PhotoCollection  : ObservableCollection<Photo> {
         DirectoryInfo _directory;

        public PhotoCollection() { }

        public PhotoCollection(string path) : this(new DirectoryInfo(path)) { }

        public PhotoCollection(DirectoryInfo directory) {
            _directory = directory;
            Update();
           
        }

        public string Path {
            set {
                _directory = new DirectoryInfo(value);
                Update();
                
            }
            get { return _directory.FullName; }
        }

        public DirectoryInfo Directory {
            set {
                _directory = value;
                Update();
                
            }
            get { return _directory; }
        }

        private void Update() {
            this.Clear();
            foreach (FileInfo f in _directory.GetFiles("*.*"))
            {
                if (System.Web.MimeMapping.GetMimeMapping(f.FullName).StartsWith("image/"))
                    Add(new Photo(f.FullName, 0, false));
            }
        }

        public void Add(string fullName, int dbId, bool isPrimary) {
            Add(new Photo(fullName, dbId, isPrimary));
        }

    }

    public class Photo {
        private string _path;
        private bool _isPrimary;
        private int _dbId;
        private Uri _source;
        private BitmapFrame _image;

        public Photo(string path, int dbId, bool isPrimary) {
            _path = path;
            _source = new Uri(path);
            _image = BitmapFrame.Create(_source);
            _isPrimary = isPrimary;
            _dbId = dbId;
        }

        public Photo(BitmapFrame image, int dbId, bool isPrimary) {
            _path = Environment.CurrentDirectory + "\\" +
                DateTime.Now.ToString(new CultureInfo("en-US")) + ".jpg";
            _source = new Uri(_path);
            _image = image;
            _isPrimary = isPrimary;
            _dbId = dbId;
        }

        public override string ToString() {
            return _source.ToString();
        }
        public int DbId { get { return _dbId; } }

        public bool IsPrimary { get { return _isPrimary; } set { _isPrimary = value; } }

        public string Source { get { return _path; } }

        public BitmapFrame Image { get { return _image; } set { _image = value; } }
    }
}