using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace JdUtils.WpfControls.Components
{
    public class PathButton : Button
    {
        public static readonly DependencyProperty PathProperty;
        public static readonly DependencyProperty TextPositionProperty;

        static PathButton()
        {
            var owner = typeof(PathButton);
            PathProperty = DependencyProperty.Register(nameof(Path), typeof(Path), owner, new FrameworkPropertyMetadata());
            TextPositionProperty = DependencyProperty.Register(nameof(TextPosition), typeof(Dock), owner, new FrameworkPropertyMetadata());
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public Dock TextPosition
        {
            get => (Dock)GetValue(TextPositionProperty);
            set => SetValue(TextPositionProperty, value);
        }

        public Path Path
        {
            get => (Path)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
    }
}
