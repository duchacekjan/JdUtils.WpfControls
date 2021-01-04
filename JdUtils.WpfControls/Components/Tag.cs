using System.Windows;
using System.Windows.Controls;

namespace JdUtils.WpfControls.Components
{
    public class Tag : Control
    {

        static Tag()
        {
            var owner = typeof(Tag);
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }
    }
}
