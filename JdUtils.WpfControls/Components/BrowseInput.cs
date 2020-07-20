using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        public static readonly DependencyProperty SeparatorProperty;

        public static readonly DependencyPropertyKey FileNamesPropertyKey;
        public static readonly DependencyProperty FileNamesProperty;

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
            SeparatorProperty = DependencyProperty.Register(nameof(Separator), typeof(string), owner, new FrameworkPropertyMetadata(", ", OnSeparatorChangeCallback));

            FileNamesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(FileNames), typeof(IList<string>), owner, new FrameworkPropertyMetadata());
            FileNamesProperty = FileNamesPropertyKey.DependencyProperty;

            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public string Separator
        {
            get => (string)GetValue(SeparatorProperty);
            set => SetValue(SeparatorProperty, value);
        }

        public IList<string> FileNames
        {
            get => (IList<string>)GetValue(FileNamesProperty);
            private set => SetValue(FileNamesPropertyKey, value);
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

        private static void OnSeparatorChangeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BrowseInput control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnSeparatorChanged();
                }
            }
        }

        private void OnSeparatorChanged()
        {
            m_input.SetValueSafe(s => s.Text, string.Join(Separator, FileNames));
            if (FileNames.Count > 0)
            {
                m_input.SetValueSafe(s => s.ToolTip, CreateTextBoxTooltip());
            }
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
            m_input.SetValueSafe(s => s.ToolTip, null);
            if (dlg.ShowDialog(hwnd))
            {               
                FileNames = dlg.FileNames;
                OnSeparatorChanged();
            }
        }

        private ToolTip CreateTextBoxTooltip()
        {
            var tooltip =  new ToolTip()
            {
                Content = new ItemsControl
                {
                    ItemsSource = FileNames
                },
                PlacementTarget = m_input,
                Placement = PlacementMode.Custom
            };

            tooltip.CustomPopupPlacementCallback = OnTooltipCustom;
            return tooltip;
        }

        private CustomPopupPlacement[] OnTooltipCustom(Size popupSize, Size targetSize, Point offset)
        {
            return new CustomPopupPlacement[] 
            {
                new CustomPopupPlacement(new Point(-1, targetSize.Height), PopupPrimaryAxis.Vertical)
            };
        }
    }
}