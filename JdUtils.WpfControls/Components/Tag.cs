﻿using JdUtils.Extensions;
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
        public const string PartViewBox = "PART_ViewBox";
        public static readonly DependencyProperty CommandProperty;
        public static readonly DependencyProperty CloseButtonVisibleProperty;

        private bool m_buttonDown;
        private Button m_closeButton;
        private Viewbox m_viewbox;

        static Tag()
        {
            var owner = typeof(Tag);
            CommandProperty = DependencyProperty.Register(nameof(Command), typeof(ICommand), owner, new FrameworkPropertyMetadata());
            CloseButtonVisibleProperty = DependencyProperty.Register(nameof(CloseButtonVisible), typeof(bool), owner, new FrameworkPropertyMetadata(true, OnCloseButtonVisiblePropertyChangedCallback));
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public bool CloseButtonVisible
        {
            get => (bool)GetValue(CloseButtonVisibleProperty);
            set => SetValue(CloseButtonVisibleProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            m_closeButton = this.FindTemplatePart<Button>(PartCloseButton)
                .AndIfNotNull(b =>
                {
                    b.Click += (s, e) => InvokeCmd(TagCommmandSource.CloseButton);
                });
            this.FindTemplatePart<Border>(PartBorder)
                .IfNotNull(b =>
                {
                    b.MouseDown += OnBorderMouseDown;
                    b.MouseUp += OnBorderMouseUp;
                });
            m_viewbox = this.FindTemplatePart<Viewbox>(PartViewBox);
        }

        private static void OnCloseButtonVisiblePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Tag control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnCloseButtonVisiblePropertyChanged();
                }
            }
        }

        private void OnCloseButtonVisiblePropertyChanged()
        {
            var visibility = Visibility.Collapsed;
            var margin = new Thickness(8, 2, 8, 2);
            if (CloseButtonVisible)
            {
                visibility = Visibility.Visible;
                margin = new Thickness(8, 2, 4, 2);
            }

            m_closeButton.SetValueSafe(s => s.Visibility, visibility);
            m_viewbox.SetValueSafe(s => s.Margin, margin);
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