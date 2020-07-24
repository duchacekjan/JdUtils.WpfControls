using System.Windows;
using System.Windows.Controls;

namespace JdUtils.WpfControls.Components
{
    public class LoginView : Control
    {
        static LoginView()
        {
            var owner = typeof(LoginView);
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }   
    }
}
