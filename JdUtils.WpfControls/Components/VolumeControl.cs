using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using JdUtils.Extensions;
using JdUtils.WpfControls.Utils;

namespace JdUtils.WpfControls.Components
{
    public class VolumeControl : Control
    {
        public delegate void ValueChangedEventHandler(VolumeControl sender, double value);

        public const string PartPath = "PART_Path";
        public const string PartDisplayValue = "PART_DisplayValue";
        public const string PartSlider = "PART_Slider";
        public const string PartIconButton = "PART_IconButton";

        public static readonly DependencyProperty ValueProperty;

        private const double Max = 1.25d;
        private const double Min = 0d;
        private const double GreenLimit = 0.8d;
        private const double YellowLimit = 0.85d;
        private const int Precision = 2;

        private Path m_path;
        private TextBlock m_displayValue;
        private Border m_slider;
        private double m_valueInternal = 1d;
        private bool m_internalUpdate;

        static VolumeControl()
        {
            var owner = typeof(VolumeControl);
            ValueProperty = DependencyProperty.Register(nameof(Value), typeof(double), owner, new FrameworkPropertyMetadata(1d, OnValueChangedCallback));
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public VolumeControl()
        {
            DependencyPropertyDescriptor.FromProperty(IsEnabledProperty, typeof(VolumeControl))
                .AddValueChanged(this, OnIsEnabledChanged);
        }
        public event ValueChangedEventHandler ValueChanged;

        private double m_beforeMuteClickValue;

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private Color Green => IsEnabled ? Colors.LimeGreen : Colors.LightGray;
        private Color Yellow => IsEnabled ? Colors.Yellow : Colors.LightGray;
        private Color Red => IsEnabled ? Colors.Red : Colors.LightGray;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            m_path = this.FindTemplatePart<Path>(PartPath);
            m_displayValue = this.FindTemplatePart<TextBlock>(PartDisplayValue);
            m_slider = this.FindTemplatePart<Border>(PartSlider);
            AssignIconButton();
            OnValueChanged();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            CaptureMouse();
            SetValueByPosition(e.GetPosition(m_slider));
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            if (IsMouseCaptured)
            {
                Mouse.Capture(null);
                m_internalUpdate = true;
                Value = m_valueInternal;
                ValueChanged?.Invoke(this, Value);
                m_internalUpdate = false;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                SetValueByPosition(e.GetPosition(m_slider));
            }
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            var step = e.Delta / 120d / 100d;
            var newValue = Value + step;
            if (newValue < Min)
            {
                newValue = Min;
            }

            if (newValue > Max)
            {
                newValue = Max;
            }

            Value = Math.Round(newValue, Precision);
            ValueChanged?.Invoke(this, Value);
        }

        private static void OnValueChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is VolumeControl control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnValueChanged();
                }
            }
        }

        private void OnIsEnabledChanged(object sender, EventArgs e)
        {
            OnValueChanged();
        }

        private void OnValueChanged()
        {
            if (!m_internalUpdate)
            {
                SetValueInternal(Value);
                m_path.SetValueSafe(s => s.Fill, CreateBrush(Value / Max));
            }
        }

        private void SetValueInternal(double value)
        {
            m_valueInternal = Math.Round(value, Precision);
            m_displayValue.SetValueSafe(s => s.Text, $"{Math.Round(m_valueInternal * 100)}%");
        }

        private void SetValueByPosition(Point point)
        {
            var position = GetPosition(point);
            m_path.SetValueSafe(s => s.Fill, CreateBrush(position));
            SetValueInternal(position * Max);
        }

        private double GetPosition(Point point)
        {
            var actualWidth = m_path?.ActualWidth ?? m_slider?.ActualWidth ?? ActualWidth;
            var position = point.X / actualWidth;
            if (position > 1)
            {
                position = 1;
            }

            if (position < 0)
            {
                position = 0;
            }

            return position;
        }

        private LinearGradientBrush CreateBrush(double position)
        {
            var start = new Point(0, 0);
            var end = new Point(position, 0);
            var brush = new LinearGradientBrush
            {
                StartPoint = start,
                EndPoint = end
            };
            var greenPosition = GreenLimit / position;
            if (position <= GreenLimit)
            {
                greenPosition = 1;
            }
            var yellowPosition = YellowLimit / position;
            if (position <= YellowLimit)
            {
                yellowPosition = 1;
            }
            brush.AddGradient(Green, greenPosition)
                 .AddGradient(Yellow, yellowPosition)
                 .AddGradient(Red)
                 .AddGradient(Colors.Transparent);
            return brush;
        }

        private void AssignIconButton()
        {
            this.FindTemplatePart<PathButton>(PartIconButton)
                .IfNotNull(iconButton =>
                {
                    iconButton.Click += (s, e) =>
                    {
                        if (s is PathButton btn && int.TryParse(btn.Tag.ToString(), out var tag))
                        {
                            SetVolumeByButton(tag);
                        }
                    };
                });
        }

        private void SetVolumeByButton(int tag)
        {
            if (tag == -1)
            {
                if (m_beforeMuteClickValue != 0d)
                {
                    Value = m_beforeMuteClickValue;
                }
                else
                {
                    Value = VolumeLevelConverter.Volume2Level;
                }
            }
            else
            {
                m_beforeMuteClickValue = Value;
                Value = 0d;
            }
            ValueChanged?.Invoke(this, Value);
        }
    }
}
