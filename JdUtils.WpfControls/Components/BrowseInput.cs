using System.Windows;
using System.Windows.Controls;

namespace JdUtils.WpfControls.Components
{
    public class BrowseInput : Control
    {

        static BrowseInput()
        {
            var owner = typeof(BrowseInput);
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }
    }
}
