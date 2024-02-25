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

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public TaskListWindow()
        {
            InitializeComponent();
            TaskList = s_bl?.Task.ReadAll()!;
        }


        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }

        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));



        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.None; 

        private void cbExperience_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList = (Experience == BO.EngineerExperience.None) ?
            s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(item => (BO.EngineerExperience?)item.Difficulty == Experience )!;

        }
        
        //this event handler is specifically for the task window closing, we wanted to decouple this from the selection changed so we can change them freely

        private void taskWindowClosed(object sender, EventArgs e)
        {
            TaskList = (Experience == BO.EngineerExperience.None) ?
           s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(item => (BO.EngineerExperience?)item.Difficulty == Experience)!;

        }

        private void listClickUpdateTask(object sender, MouseButtonEventArgs e)
        {
            BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;

            if (task != null)
            {
                
                TaskWindow newWindow = new TaskWindow(task.ID);

                newWindow.Closed += taskWindowClosed!;  //add our event listener to this event, so the event will be handled
                newWindow.ShowDialog();
            }
        }

        private void addTaskClick(object sender, RoutedEventArgs e)
        {
            TaskWindow newWindow = new TaskWindow();
            newWindow.Closed += taskWindowClosed!;  //add our event listener to this event, so the event will be handled 
            newWindow.ShowDialog();
        }
    }


}
