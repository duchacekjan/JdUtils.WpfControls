﻿using JdUtils.Extensions;
using JdUtils.WpfControls.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using t = System.Windows.Threading;

namespace JdUtils.WpfControls.Components
{
    public class SuggestionEdit : Control
    {
        private enum MoveSelection
        {
            Up = -1,
            Down = 1
        }

        public const string PartEditor = "PART_Editor";
        public const string PartClear = "PART_Clear";
        public const string PartDisplay = "PART_Display";
        public const string PartSelector = "PART_Selector";
        public const string PartPopup = "PART_Popup";

        public static readonly DependencyProperty ProviderProperty;
        public static readonly DependencyProperty SelectedItemProperty;
        public static readonly DependencyProperty ItemTemplateProperty;
        public static readonly DependencyProperty MaxSuggestionsProperty;
        public static readonly DependencyProperty LoadingContentProperty;
        public static readonly DependencyProperty FetchDelayProperty;
        public static readonly DependencyProperty WatermarkProperty;
        public static readonly DependencyProperty IconProperty;
        public static readonly DependencyProperty IconVisibilityProperty;
        public static readonly DependencyProperty ItemHighlightBrushProperty;

        public static readonly DependencyProperty IsLoadingProperty;
        private static readonly DependencyPropertyKey IsLoadingPropertyKey;

        private readonly t.Dispatcher m_uiDispatcher;
        private readonly t.DispatcherTimer m_timer;
        private Button m_clearButton;
        private ListBox m_displayItems;
        private TextBox m_display;
        private Popup m_popup;
        private TextBox m_editor;
        private IList<object> m_items;
        private bool m_selectingValue;
        private bool m_mouseSelectedItem;

        static SuggestionEdit()
        {

            var owner = typeof(SuggestionEdit);
            ProviderProperty = DependencyProperty.Register(nameof(Provider), typeof(Func<string, IEnumerable>), owner, new FrameworkPropertyMetadata());
            ItemTemplateProperty = DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), owner, new FrameworkPropertyMetadata());
            IconProperty = DependencyProperty.Register(nameof(Icon), typeof(ImageSource), owner, new FrameworkPropertyMetadata());
            ItemHighlightBrushProperty = DependencyProperty.Register(nameof(ItemHighlightBrush), typeof(Brush), owner, new FrameworkPropertyMetadata());
            IconVisibilityProperty = DependencyProperty.Register(nameof(IconVisibility), typeof(Visibility), owner, new FrameworkPropertyMetadata(Visibility.Visible));
            WatermarkProperty = DependencyProperty.Register(nameof(Watermark), typeof(string), owner, new FrameworkPropertyMetadata("resx.Watermark"));
            MaxSuggestionsProperty = DependencyProperty.Register(nameof(MaxSuggestions), typeof(int), owner, new FrameworkPropertyMetadata(50));
            LoadingContentProperty = DependencyProperty.Register(nameof(LoadingContent), typeof(object), owner, new FrameworkPropertyMetadata("resx.LoadingContent"));
            SelectedItemProperty = DependencyProperty.Register(nameof(SelectedItem), typeof(object), owner, new TwoWayPropertyMetadata(null, OnSelectedItemChangedCallback));
            FetchDelayProperty = DependencyProperty.Register(nameof(FetchDelay), typeof(int), owner, new FrameworkPropertyMetadata(300, OnFetchDelayChangedCallback));

            IsLoadingPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsLoading), typeof(bool), owner, new FrameworkPropertyMetadata(false));
            IsLoadingProperty = IsLoadingPropertyKey.DependencyProperty;
            DefaultStyleKeyProperty.OverrideMetadata(owner, new FrameworkPropertyMetadata(owner));
        }

        public SuggestionEdit()
        {
            m_uiDispatcher = t.Dispatcher.CurrentDispatcher;
            m_timer = new t.DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(300),
                IsEnabled = false
            };
            m_timer.Tick += OnFetch;
        }

        public Brush ItemHighlightBrush
        {
            get => (Brush)GetValue(ItemHighlightBrushProperty);
            set => SetValue(ItemHighlightBrushProperty, value);
        }

        public Visibility IconVisibility
        {
            get => (Visibility)GetValue(IconVisibilityProperty);
            set => SetValue(IconVisibilityProperty, value);
        }

        public ImageSource Icon
        {
            get => (ImageSource)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public string Watermark
        {
            get => (string)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        public int FetchDelay
        {
            get => (int)GetValue(FetchDelayProperty);
            set => SetValue(FetchDelayProperty, value);
        }

        public object LoadingContent
        {
            get => GetValue(LoadingContentProperty);
            set => SetValue(LoadingContentProperty, value);
        }

        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            protected set => SetValue(IsLoadingPropertyKey, value);
        }

        public int MaxSuggestions
        {
            get => (int)GetValue(MaxSuggestionsProperty);
            set => SetValue(MaxSuggestionsProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public Func<string, IEnumerable> Provider
        {
            get => (Func<string, IEnumerable>)GetValue(ProviderProperty);
            set => SetValue(ProviderProperty, value);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            InitializeClearButton();
            InitializeDisplay();
            InitializePopup();
            InitializeEditor();
        }

        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            if (m_mouseSelectedItem)
            {
                m_mouseSelectedItem = false;
            }
            else
            {
                var p = e.GetPosition(this);
                if (p.X <= m_display.ActualWidth)
                {
                    ActivateEditor();
                }
            }
        }

        private static void OnFetchDelayChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            if (d is SuggestionEdit control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.OnFetchDelayChanged();
                }
            }

        }

        private static void OnSelectedItemChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SuggestionEdit control)
            {
                if (!Equals(e.OldValue, e.NewValue))
                {
                    control.ActivateDisplay();
                }
            }
        }

        private void InitializeClearButton()
        {
            m_clearButton = this.FindTemplatePart<Button>(PartClear)
                .AndIfNotNull(b =>
                {
                    b.Click += OnClearButtonClick;
                });
        }

        private void InitializeDisplay()
        {
            m_display = this.FindTemplatePart<TextBox>(PartDisplay)
                .AndIfNotNull(t =>
                {
                    t.PreviewKeyDown += (s, e) =>
                    {
                        if (e.Key == Key.Escape)
                        {
                            SelectedItem = null;
                        }
                        else if (e.Key != Key.Tab)
                        {
                            ActivateEditor();
                        }
                    };

                    t.PreviewMouseUp += (s, e) => ActivateEditor();
                });
        }

        private void InitializePopup()
        {
            m_displayItems = this.FindTemplatePart<ListBox>(PartSelector);
            m_popup = this.FindTemplatePart<Popup>(PartPopup)
                .AndIfNotNull(p =>
                {
                    p.Closed += (s, e) =>
                    {
                        if (m_editor?.Visibility == Visibility.Visible)
                        {
                            ActivateDisplay();
                        }
                    };
                });
        }

        private void InitializeEditor()
        {
            m_editor = this.FindTemplatePart<TextBox>(PartEditor)
                .AndIfNotNull(e =>
                {
                    e.TextChanged += OnEditorTextChanged;
                    e.PreviewKeyDown += OnEditorPreviewKeyDown;
                    e.PreviewLostKeyboardFocus += (s, e) =>
                    {
                        if (m_popup.IsOpen)
                        {
                            m_popup.IsOpen = false;
                            if (e.NewFocus is ListBoxItem item)
                            {
                                m_mouseSelectedItem = true;
                                SelectedItem = item.DataContext;
                            }
                        }
                    };
                });
        }

        private void ActivateDisplay()
        {
            if (m_editor.Visibility == Visibility.Visible)
            {
                m_editor.Visibility = Visibility.Collapsed;
                m_clearButton.Visibility = Visibility.Visible;
                m_display.Visibility = Visibility.Visible;
                m_selectingValue = true;
                m_display.Focus();
                m_selectingValue = false;
            }
        }

        private void ActivateEditor()
        {
            if (!m_selectingValue && m_display.Visibility == Visibility.Visible)
            {
                m_display.Visibility = Visibility.Collapsed;
                m_clearButton.Visibility = Visibility.Collapsed;
                m_editor.Text = string.Empty;
                m_editor.Visibility = Visibility.Visible;
                m_editor.Focus();
            }
        }

        private void OnFetchDelayChanged()
        {
            var old = m_timer.IsEnabled;
            m_timer.Stop();
            m_timer.Interval = TimeSpan.FromMilliseconds(FetchDelay);
            if (old)
            {
                m_timer.Start();
            }
        }

        private void OnClearButtonClick(object sender, RoutedEventArgs e)
        {
            if (m_editor != null && m_popup != null)
            {
                if (m_popup.IsOpen)
                {
                    m_popup.IsOpen = false;
                }

                m_editor.Text = string.Empty;
                SelectedItem = null;
            }
        }

        private void OnEditorPreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Escape:
                    if (m_popup.IsOpen)
                    {
                        m_popup.IsOpen = false;
                    }

                    ActivateDisplay();
                    break;
                case Key.Down when m_popup?.IsOpen == true:
                    MoveFocus(MoveSelection.Down);
                    break;
                case Key.Up when m_popup?.IsOpen == true:
                    MoveFocus(MoveSelection.Up);
                    break;
                case Key.Enter when m_popup?.IsOpen == true:
                    e.Handled = true;
                    SetItemAndActivateDisplay();
                    break;
                case Key.Tab:
                    e.Handled = true;
                    SetItemAndActivateDisplay(false);
                    break;
            }
        }

        private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
        {
            m_timer.Stop();

            if (m_popup?.IsOpen == true && string.IsNullOrEmpty(m_editor?.Text))
            {
                m_popup.IsOpen = false;
            }
            else if (!string.IsNullOrEmpty(m_editor?.Text.Trim()))
            {
                m_timer.Start();
            }
        }

        private void OnFetch(object sender, EventArgs e)
        {
            m_timer.Stop();
            Fetch();
            if (m_popup?.IsOpen == false)
            {
                m_popup.StaysOpen = false;
                m_popup.PlacementTarget = this;
                m_popup.IsOpen = true;
            }
        }

        private void SetItemAndActivateDisplay(bool selectItem = true)
        {
            m_popup.IsOpen = false;
            if (selectItem)
            {
                SelectedItem = m_displayItems?.SelectedItem;
            }
            ActivateDisplay();
        }

        private void MoveFocus(MoveSelection direction)
        {
            var index = m_items?.IndexOf(m_displayItems.SelectedItem);
            if (index.HasValue)
            {
                var nIndex = index.Value + (int)direction;
                if (nIndex > -1 && nIndex < m_items.Count)
                {
                    m_displayItems.SelectedItem = m_items[nIndex];
                }
            }
        }

        private void Fetch()
        {
            IsLoading = true;
            m_displayItems.SelectedItem = null;

            IList<object> DoWork(Func<string, IEnumerable> provider, string fulltext, int take)
            {
                return provider?
                    .Invoke(fulltext)?
                    .OfType<object>()
                    .Take(take)
                    .ToList();
            }

            void OnSuccess(IList<object> data)
            {
                m_items = data;
                m_displayItems.ItemsSource = m_items;
                m_displayItems.SelectedItem = m_items?.FirstOrDefault();
                IsLoading = false;
            }

            void OnError(Exception e)
            {
                IsLoading = false;
            }

            var tProvider = new Func<string, IEnumerable>(Provider);
            var tFulltext = m_editor?.Text;
            var tTake = MaxSuggestions;
            ExecuteSafe(() => DoWork(tProvider, tFulltext, tTake), OnSuccess, OnError);
        }

        private void ExecuteSafe<T>(Func<T> worker, Action<T> success, Action<Exception> failure)
        {
            Task.Run(async () =>
            {
                try
                {
                    await Task.CompletedTask;
                    if (worker != null)
                    {
                        var result = worker.Invoke();
                        PostToUi(success, result);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);

                    PostToUi(failure, e);
                }
            });
        }

        /// <summary>
        /// Invokes action on UI thread
        /// </summary>
        /// <param name="action"></param>
        /// <param name="parameter"></param>
        private void PostToUi<T>(Action<T> action, T parameter)
        {
            void Wrapper()
            {
                action?.Invoke(parameter);
            }

            PostToUi(Wrapper);
        }

        /// <summary>
        /// Invokes action on UI thread
        /// </summary>
        /// <param name="action"></param>
        private void PostToUi(Action action)
        {
            if (m_uiDispatcher.CheckAccess())
            {
                action?.Invoke();
            }
            else
            {
                m_uiDispatcher.Invoke(action);
            }
        }
    }
}