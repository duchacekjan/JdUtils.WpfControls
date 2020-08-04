using System;
using System.Windows;
using System.Windows.Controls;

namespace JdUtils.WpfControls.Components
{
    public class EnumValueSelector : ComboBox
    {
        public static readonly DependencyProperty EnumTypeProperty;

        static EnumValueSelector()
        {
            var owner = typeof(EnumValueSelector);
            EnumTypeProperty = DependencyProperty.Register(nameof(EnumType), typeof(Type), owner, new FrameworkPropertyMetadata());
        }

        public Type EnumType
        {
            get => (Type)GetValue(EnumTypeProperty);
            set => SetValue(EnumTypeProperty, value);
        }
    }
}
