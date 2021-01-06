using JdUtils.Converters;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace JdUtils.WpfControls.Utils
{
    public class TagControlBackgroundConverter : AMultiValueConverter
    {
        public Brush DefaultBackground { get; set; } = Brushes.White;

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var brushes = values?.OfType<Brush>().ToList();
            var background = brushes.FirstOrDefault();
            var result = background ?? DefaultBackground;
            if (background == null && brushes.Count == 2)
            {
                var defaultBackground = brushes[1];
                if (defaultBackground != null)
                {
                    result = defaultBackground;
                }
            }

            return result;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new[] { DependencyProperty.UnsetValue };
        }
    }
}
