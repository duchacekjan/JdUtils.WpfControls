using JdUtils.Extensions;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace JdUtils.WpfControls.Components
{
    public class Strip : Control
    {
        public const string PartImage = "PART_Image";
        public static readonly DependencyProperty LinkProperty;
        public static readonly DependencyProperty CountProperty;

        private ImageBrush m_image;
        private BitmapImage m_bitmap;
        private int m_current;
        static Strip()
        {
            var owner = typeof(Strip);
            LinkProperty = DependencyProperty.Register(nameof(Link), typeof(string), owner, new FrameworkPropertyMetadata(string.Empty, OnLinkChangedCallback));
            CountProperty = DependencyProperty.Register(nameof(Count), typeof(int), owner, new FrameworkPropertyMetadata(0, OnCountChangedCallback));
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public int Count
        {
            get => (int)GetValue(CountProperty);
            set => SetValue(CountProperty, value);
        }

        public string Link
        {
            get => (string)GetValue(LinkProperty);
            set => SetValue(LinkProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            m_image = this.FindTemplatePart<ImageBrush>(PartImage);
            UpdateImage();
        }

        private static void OnLinkChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Strip control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnLinkChanged();
                }
            }
        }

        private static void OnCountChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Strip control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnCountChanged();
                }
            }
        }

        private void OnCountChanged()
        {
            m_current = 0;
            UpdateImage();
        }

        private void OnLinkChanged()
        {
            m_bitmap = new BitmapImage();
            if (!string.IsNullOrEmpty(Link))
            {
                m_bitmap.BeginInit();
                m_bitmap.UriSource = new Uri(Link, UriKind.Absolute);
                m_bitmap.EndInit();
            }
            UpdateImage();
        }

        private void UpdateImage()
        {
            m_image.SetValueSafe(s => s.ImageSource, m_bitmap);
            if (m_bitmap != null && Count > 0)
            {
                var relativeWidth = 1.0 / Count;
                m_image.SetValueSafe(s => s.Viewbox, new Rect(m_current * relativeWidth, 0, relativeWidth, 1));
                m_image.SetValueSafe(s => s.ViewboxUnits, BrushMappingMode.RelativeToBoundingBox);
                Visibility = Visibility.Visible;
            }
            else
            {
                Visibility = Visibility.Collapsed;
            }
        }
    }
}
