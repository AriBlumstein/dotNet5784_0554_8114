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
        
        
        public static readonly DependencyProperty ActualProjectStartDateProperty = DependencyProperty.Register("ActualProjectStartDate", typeof(DateTime), typeof(SchedulerWindow), new PropertyMetadata(null));
        public DateTime ActualProjectStartDate
        {
            get { return (DateTime)GetValue(ActualProjectStartDateProperty); }
            set { SetValue(ActualProjectStartDateProperty, value); }
        }


        private bool inProduction = s_bl.Schedular.InProduction();
        public static readonly DependencyProperty InProductionProperty = DependencyProperty.Register("inProduction", typeof(bool), typeof(SchedulerWindow), new PropertyMetadata(null));
        public bool InProduction
        {
            get { return inProduction; }
        }


        public bool NotInProduction { get { return !inProduction; } }
        public SchedulerWindow()
        {
            InitializeComponent();
            ProjectStartDate = s_bl.Clock;  //currently datetime.now, logic will change with the clock to be added
            try { ActualProjectStartDate = s_bl.Schedular.GetProjectStartDate().Date; }
            catch (Exception){ ActualProjectStartDate = s_bl.Clock; } //date doesn't matter, just not null
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Close();
        }

    }
}
