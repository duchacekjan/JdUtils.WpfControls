using JdUtils.WpfControls.Utils;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace JdUtils.WpfControls.Components
{
    public class TagControl : Control
    {
        public static readonly DependencyProperty ItemsSourceProperty;
        static TagControl()
        {
            var owner = typeof(TagControl);
            ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable<ITag>), owner, new FrameworkPropertyMetadata());
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public IEnumerable<ITag> ItemsSource
        {
            get => (IEnumerable<ITag>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
    }
}
