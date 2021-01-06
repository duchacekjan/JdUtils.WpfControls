﻿using JdUtils.Infrastructure;
using JdUtils.WpfControls.Utils;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace JdUtils.WpfControls.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        public static readonly DependencyProperty CurrentPageProperty;

        public event PropertyChangedEventHandler PropertyChanged;

        static MainWindow()
        {
            var owner = typeof(MainWindow);
            CurrentPageProperty = DependencyProperty.Register(nameof(CurrentPage), typeof(int), owner, new FrameworkPropertyMetadata(1));
        }

        public MainWindow()
        {
            InitializeComponent();
            Type = typeof(Test);
            Cmd = new DelegateCommand<PageNavigatorCommand?>(DoCmd);
            m_tagCmd = new DelegateCommand<TagCommand>(DoTagCommand);
            TestItems = new ObservableCollection<ITag>
            {
                new TagTest{ Id = 1, Text="A", Background = Brushes.Red, Command = m_tagCmd},
                new TagTest{ Id = 2, Text="B", Command = m_tagCmd}
            };

            DataContext = this;
        }

        private void DoTagCommand(TagCommand tag)
        {
            switch (tag?.Sender?.Id)
            {
                case null:
                    break;
                default:
                    TestItems.Remove(TestItems.FirstOrDefault(f => f.Id == tag?.Sender.Id));
                    break;
            }
        }

        public ObservableCollection<ITag> TestItems { get; set; }

        public int CurrentPage
        {
            get => (int)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
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

        private double m_value;
        public double Value
        {
            get => m_value;
            set => SetProperty(ref m_value, value);
        }

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

        public DelegateCommand<PageNavigatorCommand?> Cmd { get; set; }

        private DelegateCommand<TagCommand> m_tagCmd;

        public Test Enum { get; set; }
        public Type Type { get; set; }

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
    }

    public class TagTest : ITag
    {
        public object Id { get; set; }

        public string Text { get; set; }

        public ICommand Command { get; set; }

        public Brush Background { get; set; }
    }

    public enum Test
    {
        A,
        B
    }
}
