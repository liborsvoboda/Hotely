using Dragablz;
using TravelAgencyAdmin.Pages;
using TravelAgencyAdmin.SystemConfiguration;
using MahApps.Metro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace TravelAgencyAdmin.SystemStructure {

    public class SystemWindowDataModel {
        private readonly IInterTabClient _interTabClient;
        private readonly ObservableCollection<SystemTabs> _tabContents = new ObservableCollection<SystemTabs>();

        public List<AccentColorMenuData> AccentColors { get; set; }
        public List<SystemTheme> AppThemes { get; set; }
        private SystemWindowDataModel mwm;

        public SystemWindowDataModel() {
            _interTabClient = new DefaultInterTabClient();
            LoadTheme();
        }

        #region Theme

        public void LoadTheme() {
            // create accent color menu items for the demo
            this.AccentColors = ThemeManager.Accents
                .Select(
                    a =>
                        new AccentColorMenuData() { Name = a.Name, ColorBrush = a.Resources["AccentColorBrush"] as Brush })
                .ToList();

            // create metro theme color menu items for the demo
            this.AppThemes = ThemeManager.AppThemes
                .Select(
                    a =>
                        new SystemTheme()
                        {
                            Name = a.Name,
                            BorderColorBrush = a.Resources["BlackColorBrush"] as Brush,
                            ColorBrush = a.Resources["WhiteColorBrush"] as Brush
                        })
                .ToList();

            AppTheme theme = ThemeManager.AppThemes.FirstOrDefault(t => t.Name.Equals(SystemConfiguration.Settings.Default.ThemeName));
            Accent accent = ThemeManager.Accents.FirstOrDefault(a => a.Name.Equals(SystemConfiguration.Settings.Default.AccentName));

            if ((theme != null) && (accent != null))
            {
                ThemeManager.ChangeAppStyle(Application.Current, accent, theme);
            }
        }

        public static void SaveTheme() {
            Settings.Default.Height = Application.Current.MainWindow.Height;
            Settings.Default.Width = Application.Current.MainWindow.Width;
            Settings.Default.Save();
        }

        #endregion Theme

        public ObservableCollection<SystemTabs> TabContents {
            get { return _tabContents; }
        }

        public IInterTabClient InterTabClient {
            get { return _interTabClient; }
        }

        public static Func<object> NewItemFactory {
            get { return () => new SystemTabs("", new TemplateListViewPage()); }
        }
    }
}