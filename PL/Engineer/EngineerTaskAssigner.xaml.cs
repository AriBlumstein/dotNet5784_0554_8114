using BO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerTaskAssigner.xaml
    /// </summary>
    public partial class EngineerTaskAssigner : Window
    {
        private static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        private BO.Engineer engineer;
        
        //dependency property that will show the tasks the engineer can take on
        public static readonly DependencyProperty PossibleTasksProperty = DependencyProperty.Register("PossibleTasks", typeof(IEnumerable<BO.TaskInList>),
            typeof(EngineerTaskAssigner), new PropertyMetadata(null));

        public IEnumerable<BO.TaskInList> PossibleTasks
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(PossibleTasksProperty);  }
            set { SetValue(PossibleTasksProperty, value);   }
        }

      
        
        public EngineerTaskAssigner(BO.Engineer engineer)
        {
            InitializeComponent();
            
            this.engineer = engineer;

            if (engineer.Level == EngineerExperience.None)
            {
                //no experience, no tasks
                PossibleTasks = new ObservableCollection<BO.TaskInList>();
            }
            else
            {
                try
                {
                    //only the tasks that are within his experience
                    PossibleTasks = from item in s_bl.Task.ReadAll(t => (EngineerExperience?)t.Difficulty <= engineer.Level)
                                    let curTask = s_bl.Task.Read(item.ID)
                                    where noDependencies(curTask)
                                    where curTask.Engineer == null
                                    where curTask.Status != Status.Completed
                                    select item;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                                              

                
            }



        }





        private void listAddTask_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;
            if (task != null)
            {
                MessageBoxResult answer  =MessageBox.Show($"Are you sure you want to assign task with id \"{task.ID}\" to the engineer with id \"{engineer.ID}\"", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (answer == MessageBoxResult.Yes)
                {

                    //create the taskinengineer
                    engineer.Task = new TaskInEngineer
                    {
                        ID=task.ID,
                        Alias=task.Name,  
                    };


                    try //to add, will fail if not in production, or it belongs to a different engineer, though this window displays tasks that are not yet assigned
                    {
                        s_bl.Engineer.Update(engineer);

                        BO.Task curTask = s_bl.Task.Read(task.ID);

                        curTask.ActualStart = s_bl.Clock;

                        s_bl.Task.Update(curTask);
                    } catch (BlIllegalOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch(Exception)
                    {
                        MessageBox.Show("Unknown Error, try again later", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                   

                   
                    Close();
                }
            }

        }



        /// <summary>
        /// helper method, determines if a task has no more dependencies that still need to be handled
        /// </summary>
        /// <param name="cur"></param>
        /// <returns>if a task has no more dependencies that have not been handled</returns>
        private bool noDependencies(BO.Task cur)
        {
            return s_bl.Task.ReadUncompletedDependencies(cur).Count() == 0;
        }
    }
}
