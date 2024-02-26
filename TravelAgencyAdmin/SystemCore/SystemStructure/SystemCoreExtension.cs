using System;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace UbytkacAdmin {

    internal abstract class AppExtension : App {

        internal static T GetParentOfType<T>(Visual visual) where T : Visual {
            DependencyObject parent = visual;
            do {
                parent = VisualTreeHelper.GetParent(parent);
            } while (parent != null && !(parent is T));

            return parent as T;
        }

        public override string ToString() {
            if (this == null) {
                return string.Empty;
            }
            else {
                return Convert.ToString(this);
            }
        }
    }

    internal static class ResourceAccessor {

        public static Uri Get(string resourcePath = "Data/no_photo.png") {
            var uri = string.Format(
                "pack://application:,,,/{0};component/{1}"
                , Assembly.GetExecutingAssembly().GetName().Name
                , resourcePath
            );

            return new Uri(uri);
        }
    }
}