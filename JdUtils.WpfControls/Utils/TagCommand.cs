using JdUtils.WpfControls.Components;

namespace JdUtils.WpfControls.Utils
{
    public enum TagCommmandSource
    {
        CloseButton,
        Tag
    }

    public class TagCommand
    {
        public TagCommand(Tag sender, TagCommmandSource source)
        {
            Sender = sender;
            Source = source;
        }

        public Tag Sender { get; }

        public TagCommmandSource Source { get; }
    }
}
