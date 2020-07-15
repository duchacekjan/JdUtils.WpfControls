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
    }
}
