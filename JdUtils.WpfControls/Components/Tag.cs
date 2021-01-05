using JdUtils.Extensions;
using JdUtils.WpfControls.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace JdUtils.WpfControls.Components
{
    public class Tag : Control
    {
        public const string PartCloseButton = "PART_CloseButton";
        public const string PartBorder = "PART_Border";
        public static readonly DependencyProperty CommandProperty;
        private bool m_buttonDown;

        static Tag()
        {
            var owner = typeof(Tag);
            CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), owner, new FrameworkPropertyMetadata());
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.FindTemplatePart<Button>(PartCloseButton)
                .IfNotNull(b =>
                {
                    b.Click += (s, e) => InvokeCmd(TagCommmandSource.CloseButton);
                });
            this.FindTemplatePart<Border>(PartBorder)
                .IfNotNull(b =>
                {
                    b.MouseDown += OnBorderMouseDown;
                    b.MouseUp += OnBorderMouseUp;
                });
        }

        private void OnBorderMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Released && m_buttonDown)
            {
                m_buttonDown = false;
                InvokeCmd(TagCommmandSource.Tag);
            }
        }

        private void OnBorderMouseDown(object sender, MouseButtonEventArgs e)
        {
            m_buttonDown = e.MiddleButton == MouseButtonState.Pressed;
        }

        private void InvokeCmd(TagCommmandSource source)
        {
            var args = new TagCommand(this, source);
            if (Command?.CanExecute(args) == true)
            {
                Command.Execute(args);
            }
        }
    }
}
