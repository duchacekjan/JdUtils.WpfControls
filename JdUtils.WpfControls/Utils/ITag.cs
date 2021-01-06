using System.Windows.Input;
using System.Windows.Media;

namespace JdUtils.WpfControls.Utils
{
    public interface ITag
    {
        object Id { get; set; }

        string Text { get; set; }

        Brush Background { get; set; }
    }
}
