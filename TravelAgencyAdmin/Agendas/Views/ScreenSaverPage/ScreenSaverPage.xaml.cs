using System.Windows.Media.Animation;


using System.Windows.Controls;
using TravelAgencyAdmin.ImageEffects;
using TravelAgencyAdmin.SystemCoreExtensions;
using MahApps.Metro.Controls;
using System.Windows;

namespace TravelAgencyAdmin.Pages
{
    public partial class ScreenSaverPage : UserControl {

        //private List<MottoList> MottoList = new List<MottoList>();
        //private readonly string MottoListPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"Data","Mottos");

        private Trackball _trackball;


        public ScreenSaverPage() {

            InitializeComponent();
            // setup trackball for moving the model around
            _trackball = new Trackball();
            _trackball.Attach(this);
            _trackball.Slaves.Add(myViewport3D);
            _trackball.Enabled = true;

            var s = (Storyboard)FindResource("RotateStoryboard");
            BeginStoryboard(s);

           
        }
    }
}
