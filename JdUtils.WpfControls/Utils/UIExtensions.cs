using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace JdUtils.WpfControls.Utils
{
    public static class UIExtensions
    {
        public static LinearGradientBrush AddGradient(this LinearGradientBrush brush, Color color, double offset = 1)
        {
            if (brush != null)
            {
                var gradient = new GradientStop(color, offset);
                brush.GradientStops.Add(gradient);
            }
            return brush;
        }

        public static IntPtr GetHWND(this DependencyObject element)
        {
            var window = Window.GetWindow(element);
            var result = IntPtr.Zero;
            if (window != null)
            {
                result = new WindowInteropHelper(window).EnsureHandle();
            }
            return result;
        }
    }
}
