using UbytkacAdmin.Properties;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;

namespace UbytkacAdmin.ProgramaticXamlBehaviors {


    /// <summary>
    /// TextBox Behavior SelectAll on Focus
    ///
    /// Example of Link in XAML
    /// xmlns:behaviors="clr-namespace:UbytkacAdmin.ProgramaticXamlBehaviors"
    /// in TextBox Add behaviors:TextBoxBehavior.SelectAllTextOnFocus="True"
    /// </summary>
    public class TextBoxBehavior : Behavior<TextBox> {

        public static bool GetSelectAllTextOnFocus(TextBox textBox) {
            return (bool)textBox.GetValue(SelectAllTextOnFocusProperty);
        }

        public static void SetSelectAllTextOnFocus(TextBox textBox, bool value) {
            textBox.SetValue(SelectAllTextOnFocusProperty, value);
        }

        public static readonly DependencyProperty SelectAllTextOnFocusProperty =
            DependencyProperty.RegisterAttached(
                "SelectAllTextOnFocus",
                typeof(bool),
                typeof(TextBoxBehavior),
                new UIPropertyMetadata(false, OnSelectAllTextOnFocusChanged));

        private static void OnSelectAllTextOnFocusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var textBox = d as TextBox;
            if (textBox == null) return;

            if (e.NewValue is bool == false) return;

            if ((bool)e.NewValue) {
                textBox.GotFocus += SelectAll;
                textBox.PreviewMouseDown += IgnoreMouseButton;
            }
            else {
                textBox.GotFocus -= SelectAll;
                textBox.PreviewMouseDown -= IgnoreMouseButton;
            }
        }

        private static void SelectAll(object sender, RoutedEventArgs e) {
            var textBox = e.OriginalSource as TextBox;
            if (textBox == null) return;
            textBox.SelectAll();
        }

        private static void IgnoreMouseButton(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            var textBox = sender as TextBox;
            if (textBox == null || textBox.IsKeyboardFocusWithin) return;

            e.Handled = true;
            textBox.Focus();
        }
    }



  

   
   
}