using System.Windows;
using System.Windows.Controls;
using JdUtils.Extensions;
using JdUtils.WpfControls.Utils;

namespace JdUtils.WpfControls.Components
{
    public class BrowseInput : Control
    {
        public const string PartBrowse = "PART_Browse";
        public const string PartInput = "PART_Input";

        public static readonly DependencyProperty UserCanWriteProperty;
        public static readonly DependencyProperty IsSelectingFoldersProperty;
        public static readonly DependencyProperty IsMultiselectProperty;

        private TextBox m_input;
        static BrowseInput()
        {
            var owner = typeof(BrowseInput);
            UserCanWriteProperty = DependencyProperty.Register(nameof(UserCanWrite), typeof(bool), owner, new FrameworkPropertyMetadata());
            IsSelectingFoldersProperty = DependencyProperty.Register(nameof(IsSelectingFolders), typeof(bool), owner, new FrameworkPropertyMetadata());
            IsMultiselectProperty = DependencyProperty.Register(nameof(IsMultiselect), typeof(bool), owner, new FrameworkPropertyMetadata());
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public bool IsMultiselect
        {
            get => (bool)GetValue(IsMultiselectProperty);
            set => SetValue(IsMultiselectProperty, value);
        }

        public bool IsSelectingFolders
        {
            get => (bool)GetValue(IsSelectingFoldersProperty);
            set => SetValue(IsSelectingFoldersProperty, value);
        }

        public bool UserCanWrite
        {
            get => (bool)GetValue(UserCanWriteProperty);
            set => SetValue(UserCanWriteProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var button = this.FindTemplatePart<Button>(PartBrowse)
                .AndIfNotNull(b =>
                {
                    b.Click += OnBrowseClick;
                });
            m_input = this.FindTemplatePart<TextBox>(PartInput);
        }

        private void OnBrowseClick(object sender, RoutedEventArgs e)
        {
            var dlg = new JgsOpenFileDialog
            {
                Multiselect = IsMultiselect,
                PickFolders = IsSelectingFolders
            };

            var hwnd = this.GetHWND();
            dlg.ShowDialog(hwnd);
            var files = dlg.FileNames;
        }
    }
}
