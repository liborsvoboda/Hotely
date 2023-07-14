using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Animation;
using TravelAgencyAdmin.Api;
using TravelAgencyAdmin.Classes;
using TravelAgencyAdmin.GlobalOperations;

namespace TravelAgencyAdmin.Pages {

    public partial class WelcomePage : MetroWindow {
        public WelcomePage() {
            InitializeComponent();
            Loaded += MainManagerOnOnLoaded;
            Show();
        }

        private async void MainManagerOnOnLoaded(object sender, RoutedEventArgs e) {
            try {
                videPlayer.Source = new Uri(Path.Combine(App.startupPath, "Data", "welcome.mp4"));
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