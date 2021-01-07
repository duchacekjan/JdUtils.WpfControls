using JdUtils.Infrastructure;
using JdUtils.WpfControls.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace JdUtils.WpfControls.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public static readonly DependencyProperty CurrentPageProperty;

        private readonly Random m_rnd;
        private readonly List<string> m_suggestions = new List<string>
        {
            "abc",
            "bcd",
            "cde"
        };
        private double m_value;
        private int m_cnt;


        static MainWindow()
        {
            var owner = typeof(MainWindow);
            CurrentPageProperty = DependencyProperty.Register(nameof(CurrentPage), typeof(int), owner, new FrameworkPropertyMetadata(1));
        }

        public MainWindow()
        {
            var now = DateTime.Now.Second;
            m_rnd = new Random(now);
            InitializeComponent();
            Type = typeof(Test);
            Cmd = new DelegateCommand<PageNavigatorCommand?>(DoCmd);
            TagCmd = new DelegateCommand<TagCommand>(DoTagCommand);
            TestItems = new ObservableCollection<ITag>
            {
                new TagTest { Id = 1, Text="A", Description="Filter" },
                new TagTest { Id = 2, Text="B", Background="#FF0000", CloseButtonTooltip="Zrusit" }
            };

            m_cnt = TestItems.Count;
            Provider = GetSuggestions;
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Func<string, IEnumerable> Provider { get; set; }

        public DelegateCommand<TagCommand> TagCmd { get; set; }

        private void DoTagCommand(TagCommand tag)
        {
            switch (tag?.Id)
            {
                case null:
                    break;
                default:
                    TestItems.Remove(TestItems.FirstOrDefault(f => f.Id == tag.Id));
                    break;
            }
        }

        public ObservableCollection<ITag> TestItems { get; set; }

        public int CurrentPage
        {
            get => (int)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        public double Value
        {
            get => m_value;
            set => SetProperty(ref m_value, value);
        }

        public DelegateCommand<PageNavigatorCommand?> Cmd { get; set; }

        public Test Enum { get; set; }

        public Type Type { get; set; }

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DoCmd(PageNavigatorCommand? obj)
        {
            if (obj.HasValue)
            {
                switch (obj)
                {
                    case PageNavigatorCommand.First:
                        CurrentPage = 1;
                        break;
                    case PageNavigatorCommand.Last:
                        CurrentPage = 10;
                        break;
                    case PageNavigatorCommand.Previous:
                        CurrentPage--;
                        break;
                    case PageNavigatorCommand.Next:
                        CurrentPage++;
                        break;
                    default:
                        break;
                }
            }
        }

        private void SwitchColor(object sender, RoutedEventArgs e)
        {
            if (TagTest.Tag is string tag && tag == "1")
            {
                TagTest.Background = Brushes.Red;
                TagTest.Tag = "0";
            }
            else
            {
                TagTest.Background = Brushes.White;
                TagTest.Tag = "1";
            }
        }

        private void AddTag(object sender, RoutedEventArgs e)
        {
            TestItems.Add(new TagTest
            {
                Id = (int)TestItems.Max(m => m.Id) + 1,
                Text = $"Test{m_cnt}",
                Background = GetRandomColor(),
                Description = "Test tag"
            });
            m_cnt++;
        }

        private string GetRandomColor()
        {
            var bytes = BitConverter.GetBytes(m_rnd.Next(0, 16777215));
            var parts = BitConverter.ToString(bytes).Split('-').ToList();
            parts.Remove(parts.Last());
            return $"#{string.Join("", parts)}";
        }

        private IEnumerable GetSuggestions(string arg)
        {
            IEnumerable result = new List<string>();
            if(!string.IsNullOrEmpty(arg))
            {
                result = m_suggestions.Where(w => w.Contains(arg.ToLower()));
            }
            return result;
        }
    }

    public class TagTest : ITag
    {
        public object Id { get; set; }

        public string Text { get; set; }

        public string Description { get; set; }

        public string Background { get; set; }

        public string CloseButtonTooltip { get; set; }
    }

    public enum Test
    {
        A,
        B
    }
}
