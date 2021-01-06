using System.Windows.Input;

namespace JdUtils.WpfControls.Utils
{
    public interface ITag
    {
        object Id { get; set; }

        string Text { get; set; }

        ICommand Command { get; set; }
    }
}
