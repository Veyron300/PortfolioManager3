using System.Windows;

namespace PortfolioManager3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            MainWindowViewModel window = new MainWindowViewModel();
            this.DataContext = window;

            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            InitializeComponent();
        }


        
    }
}
