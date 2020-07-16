using System.Windows;
using System.Windows.Controls;

namespace JdUtils.WpfControls.Components
{
    public class BrowseInput : Control
    {
        public static readonly DependencyProperty UserCanWriteProperty;

        static BrowseInput()
        {
            var owner = typeof(BrowseInput);
            UserCanWriteProperty = DependencyProperty.Register(nameof(UserCanWrite), typeof(bool), owner, new FrameworkPropertyMetadata());
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public bool UserCanWrite
        {
            get => (bool)GetValue(UserCanWriteProperty);
            set => SetValue(UserCanWriteProperty, value);
        }
    }
}
