﻿using JdUtils.Extensions;
using JdUtils.WpfControls.Utils;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using resx = JdUtils.WpfControls.Resources.Resources;

namespace JdUtils.WpfControls.Components
{
    [ContentProperty(nameof(Logo))]
    public class LoginView : Control
    {
        public enum ErrorMessageVisibility
        {
            Collapsed = 2,
            Hidden = 1
        }

        public const string PartPassword = "PART_PasswordInput";
        public const string PartLogin = "PART_Login";
        public const string PartLoginInput = "PART_LoginInput";
        public const string PartPasswordInputSwitch = "PART_PasswordInputSwitch";
        public const string PartPasswordInputPlain = "PART_PasswordInputPlain";

        public static readonly DependencyProperty LogoProperty;
        public static readonly DependencyProperty LoginLabelProperty;
        public static readonly DependencyProperty PasswordLabelProperty;
        public static readonly DependencyProperty LoginButtonLabelProperty;
        public static readonly DependencyProperty RememberLabelProperty;

        public static readonly DependencyProperty LoginTooltipProperty;
        public static readonly DependencyProperty PasswordTooltipProperty;
        public static readonly DependencyProperty RememberTooltipProperty;
        public static readonly DependencyProperty LoginButtonTooltipProperty;

        public static readonly DependencyProperty LoginCmdProperty;
        public static readonly DependencyProperty UserNameProperty;
        public static readonly DependencyProperty PasswordProperty;
        public static readonly DependencyProperty RememberProperty;
        public static readonly DependencyProperty ErrorMessageProperty;
        public static readonly DependencyProperty EmptyErrorMessageVisibilityProperty;
        public static readonly DependencyProperty IsSubmittedProperty;

        public static readonly DependencyProperty AllowedEmptyProperty;

        private PasswordBox m_password;
        private bool m_passwordUpdating;
        private PathToggleButton m_passwordInputSwitch;
        private TextBox m_passwordPlain;

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

            LoginCmdProperty = DependencyProperty.Register(nameof(LoginCmd), typeof(ICommand), owner, new FrameworkPropertyMetadata());
            UserNameProperty = DependencyProperty.Register(nameof(UserName), typeof(string), owner, new TwoWayPropertyMetadata());
            PasswordProperty = DependencyProperty.Register(nameof(Password), typeof(string), owner, new TwoWayPropertyMetadata(OnPasswordChangedCallback));
            RememberProperty = DependencyProperty.Register(nameof(Remember), typeof(bool), owner, new TwoWayPropertyMetadata());

            ErrorMessageProperty = DependencyProperty.Register(nameof(ErrorMessage), typeof(string), owner, new FrameworkPropertyMetadata());
            EmptyErrorMessageVisibilityProperty = DependencyProperty.Register(nameof(EmptyErrorMessageVisibility), typeof(ErrorMessageVisibility), owner, new FrameworkPropertyMetadata(ErrorMessageVisibility.Collapsed));

            IsSubmittedProperty = DependencyProperty.Register(nameof(IsSubmitted), typeof(bool), owner, new FrameworkPropertyMetadata());
            AllowedEmptyProperty = DependencyProperty.Register(nameof(AllowedEmpty), typeof(AllowedEmpty), owner, new FrameworkPropertyMetadata(AllowedEmpty.None));
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public AllowedEmpty AllowedEmpty
        {
            get => (AllowedEmpty)GetValue(AllowedEmptyProperty);
            set => SetValue(AllowedEmptyProperty, value);
        }

        public bool IsSubmitted
        {
            get => (bool)GetValue(IsSubmittedProperty);
            set => SetValue(IsSubmittedProperty, value);
        }

        public ErrorMessageVisibility EmptyErrorMessageVisibility
        {
            get => (ErrorMessageVisibility)GetValue(EmptyErrorMessageVisibilityProperty);
            set => SetValue(EmptyErrorMessageVisibilityProperty, value);
        }

        public string ErrorMessage
        {
            get => (string)GetValue(ErrorMessageProperty);
            set => SetValue(ErrorMessageProperty, value);
        }

        public bool Remember
        {
            get => (bool)GetValue(RememberProperty);
            set => SetValue(RememberProperty, value);
        }

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public string UserName
        {
            get => (string)GetValue(UserNameProperty);
            set => SetValue(UserNameProperty, value);
        }

        public ICommand LoginCmd
        {
            get => (ICommand)GetValue(LoginCmdProperty);
            set => SetValue(LoginCmdProperty, value);
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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            m_password = this.FindTemplatePart<PasswordBox>(PartPassword)
                .AndIfNotNull(p =>
                {
                    p.PasswordChanged += OnPasswordChanged;
                });
            m_passwordPlain = this.FindTemplatePart<TextBox>(PartPasswordInputPlain)
                .AndIfNotNull(p =>
                {
                    p.TextChanged += OnTextChanged;
                });
            this.FindTemplatePart<Button>(PartLogin)
                .IfNotNull(b =>
                {
                    b.Click += OnLoginButtonClick;
                });
            this.FindTemplatePart<TextBox>(PartLoginInput)
                .IfNotNull(t =>
                {
                    t.TextChanged += (s, e) => IsSubmitted = false;
                });
            m_passwordInputSwitch = this.FindTemplatePart<PathToggleButton>(PartPasswordInputSwitch)
                .AndIfNotNull(p =>
                {
                    p.Checked += (s, e) => { m_passwordPlain.SetValueSafe(s => s.Text, m_password?.Password); };
                    p.Unchecked += (s, e) => { m_password.SetValueSafe(s => s.Password, m_passwordPlain?.Text); };
                });
            OnPasswordChanged();
        }

        private static void OnPasswordChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is LoginView control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnPasswordChanged();
                }
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox password && !m_passwordUpdating)
            {
                m_passwordUpdating = true;
                Password = password.Text;
                m_passwordUpdating = false;
                IsSubmitted = false;
            }
        }

        private void OnPasswordChanged()
        {
            if (!m_passwordUpdating)
            {
                m_passwordUpdating = true;
                if (m_passwordInputSwitch?.IsChecked == true)
                {
                    m_passwordPlain.SetValueSafe(s => s.Text, Password);
                }
                else
                {
                    m_password.SetValueSafe(s => s.Password, Password);
                }
                m_passwordUpdating = false;
            }
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox password && !m_passwordUpdating)
            {
                m_passwordUpdating = true;
                Password = password.Password;
                m_passwordUpdating = false;
                IsSubmitted = false;
            }
        }

        private void OnLoginButtonClick(object sender, RoutedEventArgs e)
        {
            if (LoginCmd?.CanExecute(null) == true)
            {
                LoginCmd.Execute(null);
            }
            IsSubmitted = true;
        }
    }
}
