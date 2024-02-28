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
        private readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        private BO.Engineer engineer;
        
        public static readonly DependencyProperty PossibleTasksProperty = DependencyProperty.Register("PossibleTasks", typeof(ObservableCollection<BO.TaskInList>),
            typeof(EngineerTaskAssigner), new PropertyMetadata(null));

        public ObservableCollection<BO.TaskInList> PossibleTasks
        {
            get { return (ObservableCollection<BO.TaskInList>)GetValue(PossibleTasksProperty);  }
            set { SetValue(PossibleTasksProperty, value);   }
        }

      
        
        public EngineerTaskAssigner(BO.Engineer engineer)
        {
            InitializeComponent();
            
            this.engineer = engineer;

            if (engineer.Level == EngineerExperience.None)
            {
                PossibleTasks = new ObservableCollection<BO.TaskInList>();
            }
            else
            {
                PossibleTasks = new ObservableCollection<BO.TaskInList>(
                                             from item in s_bl.Task.ReadAll(t => (EngineerExperience?)t.Difficulty <= engineer.Level)
                                             let curTask = s_bl.Task.Read(item.ID)
                                             where noDependencies(curTask)
                                             where curTask.Engineer == null
                                             where curTask.Status != Status.Completed
                                             select item
                                             ) ;

                
            }



        }




        private void ListAddTask_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;
            if (task != null)
            {
                MessageBoxResult answer  =MessageBox.Show($"Are you sure you want to assign task with id \"{task.ID}\" to the engineer with id \"{engineer.ID}\"", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (answer == MessageBoxResult.Yes)
                {


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
                    catch(Exception ex)
                    {
                        MessageBox.Show("Unknown Error, try again later", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                   

                   
                    Close();
                }
            }

        }


        private bool noDependencies(BO.Task cur)
        {
            return s_bl.Task.ReadUncompletedDependencies(cur).Count() == 0;
        }
    }
}
