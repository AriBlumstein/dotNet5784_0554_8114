using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {

        private static readonly BlApi.IBl s_bl = BlApi.Factory.Get(); //business layer/logic access

        

        public BO.Task Task { get; set; }
        
        public bool AdminPrivileges { get; init; }


    public TaskListWindow(BO.Task task=null, bool adminPrivileges=true)
        {
            Task = task;
            AdminPrivileges = adminPrivileges;

            InitializeComponent();
          
            if (task == null) 
            { 
                TaskList = s_bl?.Task.ReadAll()!; 
                
            }
            else
            {
                TaskList = task.Dependencies;
                
            }

           
        }


        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));



        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.None; 

        //Method to handle the front end change of the experience level filter.
        private void cbExperience_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList = (Experience == BO.EngineerExperience.None) ?
            s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(item => (BO.EngineerExperience?)item.Difficulty == Experience )!;

        }
        
        //this event handler is specifically for the task window closing
        ///we wanted to decouple this from the selection changed so that we can change them freely
        private void windowClosed(object sender, EventArgs e)
        {
            if(Task!= null)
            {
                Task=s_bl?.Task.Read(Task.ID)!;
                TaskList = Task.Dependencies;
            }
            else
            {
                TaskList = (Experience == BO.EngineerExperience.None) ?
                s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(item => (BO.EngineerExperience?)item.Difficulty == Experience)!;
            }
        }


        private void listClickUpdateTask(object sender, MouseButtonEventArgs e)
        {
            //Don't allow the user to view the task details if we accessed the page as a dependency
            //This could allow the user to open a large number of windows, making it non user friendly.
            if (Task!=null) { return; } 

            BO.TaskInList? newTask = (sender as ListView)?.SelectedItem as BO.TaskInList;

            if (newTask != null)
            {
                
                TaskWindow newWindow = new TaskWindow(newTask.ID);

                newWindow.Closed += windowClosed!;  //add our event listener to this event, so the event will be handled
                newWindow.ShowDialog();
            }
        }

        private void addTaskClick(object sender, RoutedEventArgs e)
        {
            if (Task == null)
            {
                TaskWindow newWindow = new TaskWindow();
                newWindow.Closed += windowClosed!;  //add our event listener to this event, so the event will be handled 
                newWindow.ShowDialog();
            }
            else
            {
                AddDependencies newWindow = new AddDependencies(Task);
                newWindow.Closed += windowClosed!;
                newWindow.ShowDialog();
            }

        }
    }
}
