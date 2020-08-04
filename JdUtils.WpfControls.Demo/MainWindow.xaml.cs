using System;

namespace JdUtils.WpfControls.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Type = typeof(Test);
            DataContext = this;
        }
        public Test Enum { get; set; }
        public Type Type { get; set; }
    }

    public enum Test
    {
        A,
        B
    }
}
