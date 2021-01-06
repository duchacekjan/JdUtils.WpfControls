namespace JdUtils.WpfControls.Utils
{
    public enum TagCommmandSource
    {
        CloseButton,
        Tag
    }

    public class TagCommand
    {
        public TagCommand(ITag sender, TagCommmandSource source)
        {
            Sender = sender;
            Source = source;
        }

        public ITag Sender { get; }

        public TagCommmandSource Source { get; }
    }
}
