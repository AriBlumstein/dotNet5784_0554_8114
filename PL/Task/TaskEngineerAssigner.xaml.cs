using BO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskEngineerAssigner.xaml
    /// </summary>
    public partial class TaskEngineerAssigner : Window
    {

        private BO.Task task;
        private static readonly BlApi.IBl s_bl = BlApi.Factory.Get(); //business layer/logic access


        public static readonly DependencyProperty PossibleEngineersProperty = DependencyProperty.Register("PossibleEngineers", typeof(IEnumerable<BO.EngineerInTask>),
           typeof(TaskEngineerAssigner), new PropertyMetadata(null));

        public IEnumerable<BO.EngineerInTask> PossibleEngineers
        {
            get { return (IEnumerable<BO.EngineerInTask>)GetValue(PossibleEngineersProperty); }
            set { SetValue(PossibleEngineersProperty, value); }
        }

        public TaskEngineerAssigner(BO.Task task)
        {
            InitializeComponent();

            this.task = task;

            
            if (task.Complexity== EngineerExperience.None) //if the complexity isn't set, then set the possible engineers to an empty "list"
            {
                PossibleEngineers = new ObservableCollection<BO.EngineerInTask>();
            }
            else
            {
                try
                {
                    PossibleEngineers = from item in s_bl.Engineer.ReadAll(e => (EngineerExperience?)e.Exp >= task.Complexity)
                                        where item.Task == null
                                        select new EngineerInTask
                                        {
                                            ID = item.ID,
                                            Name = item.Name,
                                        }; // Select the the engineers that are have a sufficient experience level for the task
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }
        /// <summary>
        /// This method handles the selection of an engineer to a particular task
        /// </summary>
        /// <param name="sender">the object that raised the event</param>
        /// <param name="e">Mouse related events</param>
        private void ListAddEngineer_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            BO.EngineerInTask? engineer = (sender as ListView)?.SelectedItem as BO.EngineerInTask;
            if (engineer != null)
            {
                MessageBoxResult answer = MessageBox.Show($"Are you sure you want to assign engineer with \"{engineer.ID}\" to the task with id \"{task.ID}\"", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (answer == MessageBoxResult.Yes)
                {

                    task.Engineer=engineer;
                    task.ActualStart = s_bl.Clock;
                   
                    try //this will fail if we are not in production or if it belongs to a different engineer (though this window only displays tasks that are not yet assigned)
                    {
                        s_bl.Task.Update(task);
                    }
                    catch (BlIllegalOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Unknown Error, try again later", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                    Close();
                }
            }
        }

        
    }
}
