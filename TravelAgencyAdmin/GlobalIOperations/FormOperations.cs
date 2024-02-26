using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace UbytkacAdmin.GlobalOperations {

    /// <summary>
    /// Centralised Forms Functions for work with Types, methods, Formats, Logic, supported methods
    /// </summary>
    internal class FormOperations {

        /// <summary>
        /// TextInput allowing Analphabetical and numeric character Only
        /// </summary>
        public static bool AnalphabetAndNumericOnlyValidated(ref TextCompositionEventArgs e, bool onlyLowerChars) {
            Regex regex = onlyLowerChars ? new Regex("[^a-z0-9]+") : new Regex("[^a-zA-Z0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            return e.Handled;
        }

        /// <summary>
        /// Text Input Allowing Numeric Characters Only
        /// </summary>
        public static bool NumberOnlyValidated(string checkedText) {
            Regex regex = new Regex("[^0-9]+");
            return regex.IsMatch(checkedText);
        }

        /// <summary>
        /// Removes the disabled spaces from referenced Inpout.
        /// </summary>
        public static void RemoveDisabledSpacesFromTextInput(ref object sender, ref TextChangedEventArgs e) {
            if (((TextBox)e.Source).Text.Length != ((TextBox)e.Source).Text.Replace(" ", "").Length) {
                ((TextBox)sender).Text = ((TextBox)e.Source).Text.Replace(" ", ""); ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);
            }
        }


        /// <summary>
        /// Secventional Show All Tooltips On
        /// Shown Objects in System
        /// </summary>
        /// <param name="userControl"></param>
        public async static void DisplayAllToolTips_OnClick(FrameworkElement userControl) {
            foreach (var element in FindVisualChildren<FrameworkElement>(userControl)) {
                if (element.ToolTip == null)
                    continue;

                var toolTip = element.ToolTip as ToolTip ??
                              new ToolTip { Content = element.ToolTip };
                try {
                    element.ToolTip = toolTip;
                    if (element.ToolTip != null && element.ToolTip.ToString().Length > 3) {
                        toolTip.PlacementTarget = element;
                        if (toolTip.Content.GetType().Name != "InfoPopup") { toolTip.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#e1a05d"); }
                        toolTip.Placement = PlacementMode.Relative;
                        toolTip.IsOpen = true;
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                } catch { }
                try {
                    if (element.ToolTip != null && element.ToolTip.ToString().Length > 0) {
                        toolTip.PlacementTarget = element;
                        toolTip.IsOpen = false;
                    }
                } catch { }
            }
        }


        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject {
            if (depObj == null) yield break;

            for (var i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++) {
                var child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T t) yield return t;
                foreach (var childOfChild in FindVisualChildren<T>(child)) yield return childOfChild;
            }
        }
    }
}