using JdUtils.WpfControls.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace JdUtils.WpfControls.Components
{
    [ContentProperty(nameof(Items))]
    public class TagControl : Control
    {
        public static readonly DependencyProperty ItemsSourceProperty;
        public static readonly DependencyProperty ItemsProperty;

        static TagControl()
        {
            var owner = typeof(TagControl);
            ItemsSourceProperty = DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable<ITag>), owner, new FrameworkPropertyMetadata());
            ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(ObservableCollection<Tag>), owner, new UIPropertyMetadata());
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public TagControl()
        {
            Items = new ObservableCollection<Tag>();
        }

        public ObservableCollection<Tag> Items
        {
            get => (ObservableCollection<Tag>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }

        public IEnumerable<ITag> ItemsSource
        {
            get => (IEnumerable<ITag>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
    }
}
