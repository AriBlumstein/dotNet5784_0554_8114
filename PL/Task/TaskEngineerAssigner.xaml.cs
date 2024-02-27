using BO;
using PL.Engineer;
using System.Collections.ObjectModel;
using System.Windows;


namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskEngineerAssigner.xaml
    /// </summary>
    public partial class TaskEngineerAssigner : Window
    {

        private BO.Task task;
        private readonly BlApi.IBl s_bl = BlApi.Factory.Get();


        public static readonly DependencyProperty PossibleEngineersProperty = DependencyProperty.Register("PossibleEngineers", typeof(ObservableCollection<BO.EngineerInTask>),
           typeof(TaskEngineerAssigner), new PropertyMetadata(null));

        public ObservableCollection<BO.EngineerInTask> PossibleEngineers
        {
            get { return (ObservableCollection<BO.EngineerInTask>)GetValue(PossibleEngineersProperty); }
            set { SetValue(PossibleEngineersProperty, value); }
        }

        public TaskEngineerAssigner(BO.Task task)
        {
            InitializeComponent();

            this.task = task;

            
            if (task.Complexity== EngineerExperience.None)
            {
                PossibleEngineers = new ObservableCollection<BO.EngineerInTask>();
            }
            else
            {
                PossibleEngineers = new ObservableCollection<BO.EngineerInTask>(
                                             from item in s_bl.Engineer.ReadAll(e => (EngineerExperience?)e.Exp >= task.Complexity)
                                             where item.Task == null
                                             select new EngineerInTask
                                             {
                                                 ID = item.ID,
                                                 Name = item.Name,
                                             }
                                             ); ;
               

            }

        }

        private void ListAddEngineer_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        
    }
}
