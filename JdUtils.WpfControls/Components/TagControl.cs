using System.Windows;
using System.Windows.Controls;

namespace JdUtils.WpfControls.Components
{
    public class TagControl : Control
    {
        static TagControl()
        {
            var owner = typeof(TagControl);
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }
    }
}
