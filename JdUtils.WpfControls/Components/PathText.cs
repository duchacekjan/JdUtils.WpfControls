using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;

namespace JdUtils.WpfControls.Components
{
    [ContentProperty(nameof(Path))]
    public class PathText : Control
    {
        public static readonly DependencyProperty PathProperty;
        public static readonly DependencyProperty IconPositionProperty;
        public static readonly DependencyProperty IconSizeProperty;
        public static readonly DependencyProperty ContentProperty;

        static PathText()
        {
            var owner = typeof(PathText);
            PathProperty = DependencyProperty.Register(nameof(Path), typeof(Path), owner, new FrameworkPropertyMetadata());
            IconPositionProperty = DependencyProperty.Register(nameof(IconPosition), typeof(Dock), owner, new FrameworkPropertyMetadata(Dock.Left));
            IconSizeProperty = DependencyProperty.Register(nameof(IconSize), typeof(int), owner, new FrameworkPropertyMetadata(16));
            ContentProperty = DependencyProperty.Register(nameof(Content), typeof(object), owner, new FrameworkPropertyMetadata());
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }


        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
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
