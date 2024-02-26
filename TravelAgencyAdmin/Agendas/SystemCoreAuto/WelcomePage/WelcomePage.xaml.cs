using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Media.Animation;
using UbytkacAdmin.Api;
using UbytkacAdmin.Classes;
using UbytkacAdmin.GlobalOperations;

namespace UbytkacAdmin.Pages {

    public partial class WelcomePage : MetroWindow {
        public WelcomePage() {
            InitializeComponent();
            Loaded += MainManagerOnOnLoaded;
            Show();
        }

        private async void MainManagerOnOnLoaded(object sender, RoutedEventArgs e) {
            try {
                videPlayer.Source = new Uri(Path.Combine(App.appRuntimeData.startupPath, "Data", "welcome.mp4"));
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