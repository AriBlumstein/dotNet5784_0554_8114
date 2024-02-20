using System.Windows;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void openAdminWindowClick(object sender, RoutedEventArgs e)
        {
            new Admin().ShowDialog();
        }
    }
}
