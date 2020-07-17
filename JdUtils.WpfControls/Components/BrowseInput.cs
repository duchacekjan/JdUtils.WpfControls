using System.Windows;
using System.Windows.Controls;
using JdUtils.Extensions;
using JdUtils.WpfControls.Utils;
using resx = JdUtils.WpfControls.Resources.Resources;

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
        public static readonly DependencyProperty DialogTitleProperty;
        public static readonly DependencyProperty LabelPlacementProperty;
        public static readonly DependencyProperty BrowseButtonLabelProperty;
        public static readonly DependencyProperty BrowseButtonLabelTooltipProperty;

        private TextBox m_input;
        static BrowseInput()
        {
            var owner = typeof(BrowseInput);
            UserCanWriteProperty = DependencyProperty.Register(nameof(UserCanWrite), typeof(bool), owner, new FrameworkPropertyMetadata());
            IsSelectingFoldersProperty = DependencyProperty.Register(nameof(IsSelectingFolders), typeof(bool), owner, new FrameworkPropertyMetadata());
            IsMultiselectProperty = DependencyProperty.Register(nameof(IsMultiselect), typeof(bool), owner, new FrameworkPropertyMetadata());
            DereferenceLinksProperty = DependencyProperty.Register(nameof(DereferenceLinks), typeof(bool), owner, new FrameworkPropertyMetadata());
            AddExtensionProperty = DependencyProperty.Register(nameof(AddExtension), typeof(bool), owner, new FrameworkPropertyMetadata());
            LabelProperty = DependencyProperty.Register(nameof(Label), typeof(string), owner, new FrameworkPropertyMetadata(string.Empty));
            FilterProperty = DependencyProperty.Register(nameof(Filter), typeof(string), owner, new FrameworkPropertyMetadata(resx.OpenDialogDefaultFilter));
            DialogTitleProperty = DependencyProperty.Register(nameof(DialogTitle), typeof(string), owner, new FrameworkPropertyMetadata(string.Empty));
            BrowseButtonLabelProperty = DependencyProperty.Register(nameof(BrowseButtonLabel), typeof(string), owner, new FrameworkPropertyMetadata(resx.BrowseButtonLabel));
            BrowseButtonLabelTooltipProperty = DependencyProperty.Register(nameof(BrowseButtonLabelTooltip), typeof(string), owner, new FrameworkPropertyMetadata(resx.BrowseButtonLabelTooltip));
            LabelPlacementProperty = DependencyProperty.Register(nameof(LabelPlacement), typeof(BrowseInputLabelPlacement), owner, new FrameworkPropertyMetadata(BrowseInputLabelPlacement.Top));
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public string BrowseButtonLabel
        {
            get => (string)GetValue(BrowseButtonLabelProperty);
            set => SetValue(BrowseButtonLabelProperty, value);
        }

        public string BrowseButtonLabelTooltip
        {
            get => (string)GetValue(BrowseButtonLabelTooltipProperty);
            set => SetValue(BrowseButtonLabelTooltipProperty, value);
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

        public string DialogTitle
        {
            get => (string)GetValue(DialogTitleProperty);
            set => SetValue(DialogTitleProperty, value);
        }

        public BrowseInputLabelPlacement LabelPlacement
        {
            get => (BrowseInputLabelPlacement)GetValue(LabelPlacementProperty);
            set => SetValue(LabelPlacementProperty, value);
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
                Title = DialogTitle,
                Filter = Filter
            };

            var hwnd = this.GetHWND();
            if (dlg.ShowDialog(hwnd))
            {
                m_input.SetValueSafe(s => s.Text, string.Join(", ", dlg.FileNames));
            }
        }
    }
}
