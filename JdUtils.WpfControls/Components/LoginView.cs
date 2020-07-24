using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using resx = JdUtils.WpfControls.Resources.Resources;

namespace JdUtils.WpfControls.Components
{
    [ContentProperty(nameof(Logo))]
    public class LoginView : Control
    {
        public static readonly DependencyProperty LogoProperty;
        public static readonly DependencyProperty LoginLabelProperty;
        public static readonly DependencyProperty PasswordLabelProperty;
        public static readonly DependencyProperty LoginButtonLabelProperty;
        public static readonly DependencyProperty RememberLabelProperty;

        public static readonly DependencyProperty LoginTooltipProperty;
        public static readonly DependencyProperty PasswordTooltipProperty;
        public static readonly DependencyProperty RememberTooltipProperty;
        public static readonly DependencyProperty LoginButtonTooltipProperty;

        static LoginView()
        {
            var owner = typeof(LoginView);
            LogoProperty = DependencyProperty.Register(nameof(Logo), typeof(object), owner, new FrameworkPropertyMetadata());
            LoginLabelProperty = DependencyProperty.Register(nameof(LoginLabel), typeof(string), owner, new FrameworkPropertyMetadata(resx.LoginLabel));
            PasswordLabelProperty = DependencyProperty.Register(nameof(PasswordLabel), typeof(string), owner, new FrameworkPropertyMetadata(resx.PasswordLabel));
            LoginButtonLabelProperty = DependencyProperty.Register(nameof(LoginButtonLabel), typeof(string), owner, new FrameworkPropertyMetadata(resx.LoginButtonLabel));
            RememberLabelProperty = DependencyProperty.Register(nameof(RememberLabel), typeof(string), owner, new FrameworkPropertyMetadata(resx.RemeberLabel));

            LoginTooltipProperty = DependencyProperty.Register(nameof(LoginTooltip), typeof(string), owner, new FrameworkPropertyMetadata(resx.LoginTooltip));
            PasswordTooltipProperty = DependencyProperty.Register(nameof(PasswordTooltip), typeof(string), owner, new FrameworkPropertyMetadata(null));
            RememberTooltipProperty = DependencyProperty.Register(nameof(RememberTooltip), typeof(string), owner, new FrameworkPropertyMetadata(resx.RememberTooltip));
            LoginButtonTooltipProperty = DependencyProperty.Register(nameof(LoginButtonTooltip), typeof(string), owner, new FrameworkPropertyMetadata(null));

            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public object Logo
        {
            get => GetValue(LogoProperty);
            set => SetValue(LogoProperty, value);
        }

        public string LoginLabel
        {
            get => (string)GetValue(LoginLabelProperty);
            set => SetValue(LoginLabelProperty, value);
        }

        public string PasswordLabel
        {
            get => (string)GetValue(PasswordLabelProperty);
            set => SetValue(PasswordLabelProperty, value);
        }

        public string LoginButtonLabel
        {
            get => (string)GetValue(LoginButtonLabelProperty);
            set => SetValue(LoginButtonLabelProperty, value);
        }

        public string RememberLabel
        {
            get => (string)GetValue(RememberLabelProperty);
            set => SetValue(RememberLabelProperty, value);
        }

        public string LoginTooltip
        {
            get => (string)GetValue(LoginTooltipProperty);
            set => SetValue(LoginTooltipProperty, value);
        }

        public string PasswordTooltip
        {
            get => (string)GetValue(PasswordTooltipProperty);
            set => SetValue(PasswordTooltipProperty, value);
        }

        public string RememberTooltip
        {
            get => (string)GetValue(RememberTooltipProperty);
            set => SetValue(RememberTooltipProperty, value);
        }

        public string LoginButtonTooltip
        {
            get => (string)GetValue(LoginButtonTooltipProperty);
            set => SetValue(LoginButtonTooltipProperty, value);
        }
    }
}
