using System.Windows;
using System.Windows.Controls;

namespace JdUtils.WpfControls.Components
{
    public class Strip : Control
    {
        static Strip()
        {
            var owner = typeof(Strip);
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }
    }
}
