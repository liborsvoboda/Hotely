using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Web.UI.WebControls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace TravelAgencyAdmin.GlobalClasses {
    /*
    public class PhotoList : ObservableCollection<ImageFile> {
        private DirectoryInfo _directory;

        public PhotoList() {
        }

        public PhotoList(string path) : this(new DirectoryInfo(path)) {
        }

        public PhotoList(DirectoryInfo directory) {
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
            foreach (var f in _directory.GetFiles("*.jpg"))
            {
                Add(new ImageFile(f.FullName));
            }
        }
    }

    public class ImageFile {
        public ImageFile(string path) {
            Path = path;
            Uri = new Uri(Path);
            Image = BitmapFrame.Create(Uri);
        }

        public string Path { get; }
        public Uri Uri { get; }
        public BitmapFrame Image { get; }

        public override string ToString() => Path;
    }
    */
}