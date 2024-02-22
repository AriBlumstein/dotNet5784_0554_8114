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


        //dependency property for the automatic scheduler
        public static readonly DependencyProperty ProjectStartDateProperty = DependencyProperty.Register("ProjectStartDate", typeof(DateTime), typeof(SchedulerWindow), new PropertyMetadata(null));
        public DateTime ProjectStartDate
        {
            get { return (DateTime)GetValue(ProjectStartDateProperty); }
            set { SetValue(ProjectStartDateProperty, value); }
        } 


        public SchedulerWindow()
        {
            InitializeComponent();
            ProjectStartDate = DateTime.Now;  //currently datetime.now, logic will change with the clock to be added
        }

        private void startProduction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                s_bl.Schedular.createSchedule(ProjectStartDate);
                MessageBox.Show("Successfully Scheduled the Tasks", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();                                                  //on this event, we start the production, close only on success
            }
            catch (BlIllegalOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);  
            }
            Close();
        }

    }
}
