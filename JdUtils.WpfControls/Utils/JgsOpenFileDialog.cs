using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using resx = JdUtils.WpfControls.Resources.Resources;

namespace JdUtils.WpfControls.Utils
{
    public class JgsOpenFileDialog
    {
        private WindowsFormsAssemblyAccessor m_assemblyAccessor;

        public string InitialDirectory { get; set; }

        public string Title { get; set; }

        public JgsOpenDialogOptions Options { get; set; }

        public string Filter { get; set; }

        public string FileName { get; private set; }

        public IList<string> FileNames { get; private set; }

        private bool PickFolders => Options.HasFlag(JgsOpenDialogOptions.PickFolders);

        public bool ShowDialog()
        {
            return ShowDialog(IntPtr.Zero);
        }

        public bool ShowDialog(IntPtr hWndOwner)
        {

            if (Environment.OSVersion.Version.Major < 6)
            {
                throw new NotSupportedException("Not supported on this OS Version");
            }

            var dialog =  CreateDialog();
            var accessor = m_assemblyAccessor ?? (m_assemblyAccessor = new WindowsFormsAssemblyAccessor());
            var result = accessor.ShowDialog(dialog, hWndOwner, PickFolders);
            if (result)
            {
                FileName = dialog.FileName;
                FileNames = dialog.FileNames?.ToList();
            }
            return result;
        }

        private OpenFileDialog CreateDialog()
        {
            return new OpenFileDialog
            {
                CheckFileExists = false,
                Title = GetTitle(),
                InitialDirectory = GetInitialDirectory(),
                Filter = GetFilter(),
                AddExtension = GetAddExtensions(),
                DereferenceLinks = GetDeferenceLinks(),
                Multiselect = Options.HasFlag(JgsOpenDialogOptions.Multiselect)
            };
        }

        private string GetTitle()
        {
            return Title;
        }

        private string GetInitialDirectory()
        {
            return string.IsNullOrEmpty(InitialDirectory)
                ? Environment.CurrentDirectory
                : InitialDirectory;
        }

        private string GetFilter()
        {
            return PickFolders ? resx.FolderFilter : Filter;
        }

        private bool GetAddExtensions()
        {
            return PickFolders ? false : Options.HasFlag(JgsOpenDialogOptions.AddExtension);
        }

        private bool GetDeferenceLinks()
        {
            return PickFolders ? true : Options.HasFlag(JgsOpenDialogOptions.DereferenceLinks);
        }
    }
}
