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
        public static readonly DependencyProperty AddExtensionProperty;
        public static readonly DependencyProperty DereferenceLinksProperty;
        public static readonly DependencyProperty LabelProperty;
        public static readonly DependencyProperty FilterProperty;

        private TextBox m_input;
        static BrowseInput()
        {
            var owner = typeof(BrowseInput);
            UserCanWriteProperty = DependencyProperty.Register(nameof(UserCanWrite), typeof(bool), owner, new FrameworkPropertyMetadata());
            IsSelectingFoldersProperty = DependencyProperty.Register(nameof(IsSelectingFolders), typeof(bool), owner, new FrameworkPropertyMetadata());
            IsMultiselectProperty = DependencyProperty.Register(nameof(IsMultiselect), typeof(bool), owner, new FrameworkPropertyMetadata());
            DereferenceLinksProperty = DependencyProperty.Register(nameof(DereferenceLinks), typeof(bool), owner, new FrameworkPropertyMetadata());
            AddExtensionProperty = DependencyProperty.Register(nameof(AddExtension), typeof(bool), owner, new FrameworkPropertyMetadata());
            LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), owner, new FrameworkPropertyMetadata());
            FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(string), owner, new FrameworkPropertyMetadata());
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

        public string Filter
        {
            get => (string)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public bool DereferenceLinks
        {
            get => (bool)GetValue(DereferenceLinksProperty);
            set => SetValue(DereferenceLinksProperty, value);
        }

        public bool AddExtension
        {
            get => (bool)GetValue(AddExtensionProperty);
            set => SetValue(AddExtensionProperty, value);
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
            var options = JgsOpenDialogOptions.None;
            if (IsMultiselect)
            {
                options |= JgsOpenDialogOptions.Multiselect;
            }

            if (IsSelectingFolders)
            {
                options |= JgsOpenDialogOptions.PickFolders;
            }

            if (AddExtension) 
            {
                options |= JgsOpenDialogOptions.AddExtension;
            }

            if (DereferenceLinks)
            {
                options |= JgsOpenDialogOptions.DereferenceLinks;
            }

            var dlg = new JgsOpenFileDialog
            {
                Options = options,
                Title = Label,
                Filter = Filter
            };

            var hwnd = this.GetHWND();
            dlg.ShowDialog(hwnd);
        }
    }
}
