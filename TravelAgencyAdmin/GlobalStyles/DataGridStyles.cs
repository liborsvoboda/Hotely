using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TravelAgencyAdmin.GlobalStyles
{
    class DatagridStyles
    {
        public static Style gridTextRightAligment = GridTextRightAligment();

        public static Style GridTextRightAligment()
        {
            Style columnRightStyle = new Style();
            columnRightStyle.Setters.Add(new Setter(Control.HorizontalContentAlignmentProperty, HorizontalAlignment.Right));
            columnRightStyle.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Colors.Transparent)));
            columnRightStyle.Setters.Add(new Setter(Control.VerticalContentAlignmentProperty, VerticalAlignment.Stretch));
            columnRightStyle.Setters.Add(new Setter(Control.BorderBrushProperty, new SolidColorBrush(Colors.Transparent)));

            columnRightStyle.Setters.Add(new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center));
            columnRightStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));

            return columnRightStyle;
        }

    }
}
