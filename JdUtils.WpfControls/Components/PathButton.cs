using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace JdUtils.WpfControls.Components
{
    public class PathButton : Button
    {
        public static readonly DependencyProperty PathProperty;
        public static readonly DependencyProperty IconPositionProperty;
        public static readonly DependencyProperty IconSizeProperty;

        static PathButton()
        {
            var owner = typeof(PathButton);
            PathProperty = DependencyProperty.Register(nameof(Path), typeof(Path), owner, new FrameworkPropertyMetadata());
            IconPositionProperty = DependencyProperty.Register(nameof(IconPosition), typeof(Dock), owner, new FrameworkPropertyMetadata(Dock.Bottom));
            IconSizeProperty = DependencyProperty.Register(nameof(IconSize), typeof(int), owner, new FrameworkPropertyMetadata(16));
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public int IconSize
        {
            get => (int)GetValue(IconSizeProperty);
            set => SetValue(IconSizeProperty, value);
        }

        public Dock IconPosition
        {
            get => (Dock)GetValue(IconPositionProperty);
            set => SetValue(IconPositionProperty, value);
        }

        public Path Path
        {
            get => (Path)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
    }
}
