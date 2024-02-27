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


        public static readonly DependencyProperty PossibleEngineersProperty = DependencyProperty.Register("PossibleEngineers", typeof(ObservableCollection<BO.Engineer>),
           typeof(EngineerTaskAssigner), new PropertyMetadata(null));

        public ObservableCollection<BO.Engineer> PossibleEngineers
        {
            get { return (ObservableCollection<BO.Engineer>)GetValue(PossibleEngineersProperty); }
            set { SetValue(PossibleEngineersProperty, value); }
        }

        public TaskEngineerAssigner(BO.Task task)
        {
            InitializeComponent();

            this.task = task;

            //PossibleEngineers = new ObservableCollection<BO.Engineer> { new BO.Engineer { ID = 3, Name = "hi" } };
            if (task.Complexity== EngineerExperience.None)
            {
                PossibleEngineers = new ObservableCollection<BO.Engineer>();
            }
            else
            {
                PossibleEngineers = new ObservableCollection<BO.Engineer>(
                                             from item in s_bl.Engineer.ReadAll(e => (EngineerExperience?)e.Exp >= task.Complexity)
                                             where item.Task == null
                                             select item
                                             ); ;
               

            }

        }

        private void ListAddEngineer_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        
    }
}
