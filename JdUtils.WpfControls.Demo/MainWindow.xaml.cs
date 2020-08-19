using JdUtils.Infrastructure;
using JdUtils.WpfControls.Utils;
using System;
using System.Windows;

namespace JdUtils.WpfControls.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static readonly DependencyProperty CurrentPageProperty;


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
            DataContext = this;
        }
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

        public DelegateCommand<PageNavigatorCommand?>  Cmd { get; set; }
        public Test Enum { get; set; }
        public Type Type { get; set; }
    }

    public enum Test
    {
        A,
        B
    }
}
