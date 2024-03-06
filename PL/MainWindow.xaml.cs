using BlApi;
using Microsoft.VisualBasic;
using PL.Engineer;
using System.Windows;
using System.Windows.Threading;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly IBl s_bl = BlApi.Factory.Get();

        public MainWindow()
        {
            InitializeComponent();

            // Start a DispatcherTimer to update the Clock property periodically in a thread safe matter
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // 1 second
            timer.Tick += (sender, e) =>
            {
                using (var mutex = new Mutex(false, "ClockMutex")) //same mutex from backend, but I do not get it first on initialization
                {
                    Clock = s_bl.Clock;
                }
            };
            timer.Start();


        }


        public static readonly DependencyProperty ClockProperty =
        DependencyProperty.Register("Clock", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(null));

        public DateTime Clock
        {
            get { return (DateTime)GetValue(ClockProperty); }
            set { SetValue(ClockProperty, value);  }
        }

        private void openAdminWindowClick(object sender, RoutedEventArgs e)
        {
            new Admin().ShowDialog();
        }




        private void forwardHour_Click(object sender, RoutedEventArgs e)
        {
            s_bl.MoveForwardHour();
            Clock = s_bl.Clock;
        }

        private void forwardDay_Click(object sender, RoutedEventArgs e)
        {
            s_bl.MoveForwardDay();
            Clock = s_bl.Clock;
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            s_bl.TimeReset();
            Clock = s_bl.Clock;
        }

        private void openEngineerWindowClick(object sender, RoutedEventArgs e)
        {
            int input;

            string inputString = Interaction.InputBox("Enter your ID here:", "Engineer Access", "00000");

            if (inputString == "")
            {
                // User pressed Cancel
                return;
            }
            else if (!int.TryParse(inputString, out input))
            {
                MessageBox.Show($"\"{inputString}\" is not a valid ID", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                BO.Engineer engineer;
                try
                {
                    engineer = s_bl.Engineer.Read(input);
                    new EngineerWindow(engineer.ID, false).ShowDialog();
                }
                catch (BO.BlDoesNotExistException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }



}
