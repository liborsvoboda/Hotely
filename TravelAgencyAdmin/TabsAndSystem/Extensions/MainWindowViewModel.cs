using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Media;
using MahApps.Metro;
using TravelAgencyAdmin.Properties;
using Dragablz;
using TravelAgencyAdmin.Pages;

namespace TravelAgencyAdmin.SystemCoreExtensions
{
    public class MainWindowViewModel
    {
        private readonly IInterTabClient _interTabClient;
        private readonly ObservableCollection<TabContent> _tabContents = new ObservableCollection<TabContent>();

        public List<AccentColorMenuData> AccentColors { get; set; }
        public List<AppThemeMenuData> AppThemes { get; set; }
        //MainWindowViewModel mwm;

        public MainWindowViewModel()
        {
            _interTabClient = new DefaultInterTabClient();
            LoadTheme();
        }

        public ObservableCollection<TabContent> TabContents
        {
            get { return _tabContents; }
        }
        public IInterTabClient InterTabClient
        {
            get { return _interTabClient; }
        }

        public static Func<object> NewItemFactory
        {
            get { return () => new TabContent("", new TemplateListViewPage()); }
        }

        #region Theme
        public void LoadTheme()
        {

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
                        new AppThemeMenuData()
                        {
                            Name = a.Name,
                            BorderColorBrush = a.Resources["BlackColorBrush"] as Brush,
                            ColorBrush = a.Resources["WhiteColorBrush"] as Brush
                        })
                .ToList();

            AppTheme theme = ThemeManager.AppThemes.FirstOrDefault(t => t.Name.Equals(Properties.Settings.Default.ThemeName));
            Accent accent = ThemeManager.Accents.FirstOrDefault(a => a.Name.Equals(Properties.Settings.Default.AccentName));

            if ((theme != null) && (accent != null))
            {
                ThemeManager.ChangeAppStyle(Application.Current, accent, theme);
            }
        }

        public static void SaveTheme()
        {
            Settings.Default.Height = Application.Current.MainWindow.Height;
            Settings.Default.Width = Application.Current.MainWindow.Width;
            Settings.Default.Save();
        }
        #endregion

    }
}