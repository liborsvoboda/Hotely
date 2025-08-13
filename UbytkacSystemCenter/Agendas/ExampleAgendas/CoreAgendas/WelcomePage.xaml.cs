using MahApps.Metro.Controls;
using System;
using System.IO;
using System.Windows;


namespace EasyITSystemCenter.Pages {

    public partial class WelcomePage : MetroWindow {
        public WelcomePage() {
            InitializeComponent();
            Loaded += MainManagerOnOnLoaded;
            Show();
        }

        private async void MainManagerOnOnLoaded(object sender, RoutedEventArgs e) {
            try {
                videPlayer.Source = new Uri(Path.Combine(App.appRuntimeData.startupPath, "Data","Media", "welcome.mp4"));
                videPlayer.Play();
            } catch { Close(); }
        }

        private void VidePlayerMediaEnded(object sender, EventArgs e) {
            Close();
            videPlayer.Stop();
        }

        private void Storyboard_Completed(object sender, EventArgs e) => Close();
    }
}