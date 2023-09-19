using MahApps.Metro;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media;
using UbytkacAdmin.Properties;
using UbytkacAdmin.SystemConfiguration;
using UbytkacAdmin.SystemHelpers;

namespace UbytkacAdmin.SystemStructure {

    public class SystemTheme : AccentColorMenuData {

        protected override void ChangeTheme(object sender) {
            Tuple<AppTheme, Accent> theme = ThemeManager.DetectAppStyle(Application.Current);
            AppTheme appTheme = ThemeManager.GetAppTheme(this.Name);
            ThemeManager.ChangeAppStyle(Application.Current, theme.Item2, appTheme);
            SaveTheme(theme.Item2, appTheme);
        }
    }

    public class AccentColorMenuData {

        protected virtual void ChangeTheme(object sender) {
            Tuple<AppTheme, Accent> theme = ThemeManager.DetectAppStyle(Application.Current);
            Accent accent = ThemeManager.GetAccent(this.Name);
            ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);
            SaveTheme(accent, theme.Item1);
        }

        protected void SaveTheme(Accent accent, AppTheme theme) {
            Settings.Default.ThemeName = theme.Name;
            Settings.Default.AccentName = accent.Name;
            Settings.Default.Save();
        }

        public AccentColorMenuData() {
            ChangeAccentCommand = new DelegateCommand<object>(this.ChangeAccent);
        }

        public string Name { get; set; }

        public string NameDisplay { get { return Regex.Replace(this.Name, "([a-z])([A-Z][a-z])", "$1 $2"); } }

        public Brush BorderColorBrush { get; set; }

        public Brush ColorBrush { get; set; }

        public DelegateCommand<object> ChangeAccentCommand { get; private set; }

        private void ChangeAccent(object parameter) {
            ChangeTheme(parameter);
        }
    }
}