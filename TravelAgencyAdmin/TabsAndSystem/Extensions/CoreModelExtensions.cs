using Org.BouncyCastle.Asn1.X509;
using System;
using System.Windows.Controls;
using System.Windows;

namespace TravelAgencyAdmin.SystemCoreExtensions
{

    /// <summary>
    /// Extensions for Core Describing for Simple Direcly
    /// Working with System Core
    /// 
    /// TODO: waith for setting of Pages 
    /// for simple control  more UserControls in One time
    /// </summary>
    public class ExtendedKeyUsageData
    {
        public ExtendedKeyUsageData() { }

        public ExtendedKeyUsageData(string displayName, KeyPurposeID extendedKeyUsageValueName)
        {
            this.DisplayName = displayName;
            this.ExtendedKeyUsageValueName = extendedKeyUsageValueName;
        }

        public string DisplayName { get; set; }
        public KeyPurposeID ExtendedKeyUsageValueName { get; set; }
    }

    public class KeyUsageData {
        public KeyUsageData() { }

        public KeyUsageData(string displayName, int keyUsageValue) {
            this.DisplayName = displayName;
            this.KeyUsageValue = keyUsageValue;
        }

        public string DisplayName { get; set; }
        public int KeyUsageValue { get; set; }

    }


    public class SubjectName { public string AlternativSubjectName { get; set; } }

    public class SetMinWidthToAutoAttachedBehaviour {
        public static bool GetSetMinWidthToAuto(DependencyObject obj) {
            return (bool)obj.GetValue(SetMinWidthToAutoProperty);
        }

        public static void SetSetMinWidthToAuto(DependencyObject obj, bool value) { obj.SetValue(SetMinWidthToAutoProperty, value); }

        public static readonly DependencyProperty SetMinWidthToAutoProperty =
            DependencyProperty.RegisterAttached("SetMinWidthToAuto", typeof(bool), typeof(SetMinWidthToAutoAttachedBehaviour), new UIPropertyMetadata(false, WireUpLoadedEvent));

        public static void WireUpLoadedEvent(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var grid = (DataGrid)d;
            var doIt = (bool)e.NewValue;
            if (doIt) { grid.Loaded += SetMinWidths; }
        }

        public static void SetMinWidths(object source, EventArgs e) {
            var grid = (DataGrid)source;
            foreach (var column in grid.Columns)
            {
                column.MinWidth = column.ActualWidth;
                column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            }
        }
    }
}