using System;

namespace JdUtils.WpfControls.Utils
{
    [Flags]
    public enum JgsOpenDialogOptions
    {
        None = 0,
        AddExtension = 1,
        Multiselect = 2,
        PickFolders = 4,
        DereferenceLinks = 8
    }
}
