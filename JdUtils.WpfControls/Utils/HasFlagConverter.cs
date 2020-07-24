using System;
using System.Globalization;
using System.Windows;
using JdUtils.Converters;

namespace JdUtils.WpfControls.Utils
{
    public class HasFlagConverter : AValueConverter
    {
        public AllowedEmpty Flag { get; set; } = AllowedEmpty.None;

        public bool Inverse { get; set; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = DependencyProperty.UnsetValue;
            if (value is AllowedEmpty partVisibility)
            {
                var hasFlag = partVisibility.HasFlag(Flag);
                if (Inverse)
                {
                    hasFlag = !hasFlag;
                }

                result = hasFlag;
            }

            return result;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
