using BO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace PL.Task
{
    /// <summary>
    /// Interaction logic for the add dependencies window (AddDependencies.xaml)
    /// </summary>
    public partial class AddDependencies : Window
    {
        private static readonly BlApi.IBl s_bl = BlApi.Factory.Get();  //business layer/logic access

        public IEnumerable<BO.TaskInList> TaskList
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(PossibleTaskListProperty); }
            set { SetValue(PossibleTaskListProperty, value); }
        }


        public static readonly DependencyProperty PossibleTaskListProperty =
            DependencyProperty.Register("PossibleTaskList", typeof(IEnumerable<BO.TaskInList>), typeof(AddDependencies), new PropertyMetadata(null));

        private BO.Task task;
        public AddDependencies(BO.Task task)
        {
            this.task = task;

            try
            {
                TaskList = s_bl.Task.ReadAll().Where(t => t.ID != task.ID);

                foreach (var d in task.Dependencies)
                {
                    TaskList = TaskList.Where(t => t.ID != d.ID);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
              
            InitializeComponent();


        }

        /// <summary>
        /// This method handles the double click selection of a dependent task. It will then add the selected task as a dependency to the task that we orginally came from in the UI.
        /// </summary>
        /// <param name="sender">the object that raised the event</param>
        /// <param name="e">Mouse related events</param>
        private void addDependency_DoubleClick(object sender, MouseButtonEventArgs e)
        {

            BO.TaskInList? dependency = (sender as ListView)?.SelectedItem as BO.TaskInList;

            if (dependency != null)
            {

                MessageBoxResult answer = MessageBox.Show($"Are you sure you want to add task with id \"{dependency.ID}\" as a requisite to task with id \"{task.ID}\"", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (answer == MessageBoxResult.Yes)
                {

                    task.Dependencies.Add(dependency);

                    try
                    {
                        s_bl.Task.Update(task);
                        MessageBox.Show($"Successfully added task {dependency.ID} as a requisite to task {task.ID}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                    catch (BlIllegalOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        task.Dependencies.RemoveAll(t => t.ID == dependency.ID);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        task.Dependencies.RemoveAll(t => t.ID == dependency.ID);
                    }
                }
            }
        }
    }
}
