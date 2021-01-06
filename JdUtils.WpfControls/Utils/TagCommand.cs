namespace JdUtils.WpfControls.Utils
{
    public enum TagCommmandSource
    {
        CloseButton,
        Tag
    }

    public class TagCommand
    {
        public TagCommand(object id, TagCommmandSource source)
        {
            Id = id;
            Source = source;
        }

        public object Id { get; }

        public TagCommmandSource Source { get; }
    }
}
