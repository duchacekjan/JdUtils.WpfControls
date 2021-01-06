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
            var result = DefaultBackground;
            if (values != null)
            {
                Brush brush = null;
                Brush defaultBrush = null;
                foreach (var value in values)
                {
                    switch (value)
                    {
                        case Brush b:
                            defaultBrush = b;
                            break;
                        case string s:
                            brush = (Brush)new BrushConverter().ConvertFromString(s);
                            break;
                    }
                }
                //INFO CompoundAssignment 
                defaultBrush = defaultBrush ?? DefaultBackground;
                result = brush ?? defaultBrush;
            }

            return result;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new[] { DependencyProperty.UnsetValue };
        }
    }
}
