using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace UbytkacAdmin.GlobalStyles {

    internal class ProgramaticStyles {
        public static Style gridTextRightAligment = GridTextRightAligment();

        public static Style GridTextRightAligment() {
            Style columnRightStyle = new Style();
            columnRightStyle.Setters.Add(new Setter(Control.HorizontalContentAlignmentProperty, HorizontalAlignment.Right));
            columnRightStyle.Setters.Add(new Setter(Control.BackgroundProperty, new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Transparent)));
            columnRightStyle.Setters.Add(new Setter(Control.VerticalContentAlignmentProperty, VerticalAlignment.Stretch));
            columnRightStyle.Setters.Add(new Setter(Control.BorderBrushProperty, new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Transparent)));

            columnRightStyle.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center));
            columnRightStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));

            return columnRightStyle;
        }


      

    }
}