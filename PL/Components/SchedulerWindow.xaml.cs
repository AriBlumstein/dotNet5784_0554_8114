using BlApi;
using BO;
using System.Windows;

namespace PL.Components
{
    /// <summary>
    /// Interaction logic for SchedulerWindow.xaml
    /// </summary>
    public partial class SchedulerWindow : Window
    {
        private static readonly IBl s_bl = BlApi.Factory.Get();

        public static readonly DependencyProperty ProjectStartDateProperty = DependencyProperty.Register("ProjectStartDate", typeof(DateTime), typeof(SchedulerWindow), new PropertyMetadata(null));
        public DateTime ProjectStartDate
        {
            get { return (DateTime)GetValue(ProjectStartDateProperty); }
            set { SetValue(ProjectStartDateProperty, value); }
        } 
        public SchedulerWindow()
        {
            InitializeComponent();
            ProjectStartDate = DateTime.Now;
        }

        private void startProduction_Click(object sender, RoutedEventArgs e)
        {
            try
            {s_bl.Schedular.createSchedule(ProjectStartDate);
            }
            catch (BlIllegalOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();
        }

    }
}
