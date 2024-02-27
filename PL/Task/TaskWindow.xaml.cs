using BlApi;
using BO;
using PL.Engineer;
using System.Windows;
using System.Windows.Controls;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        private readonly IBl s_bl = Factory.Get();

        public static readonly DependencyProperty TaskProperty =
        DependencyProperty.Register("Task", typeof(BO.Task), typeof(TaskWindow), new PropertyMetadata(null));  //the task itself

        public BO.Task Task
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }


        public static readonly DependencyProperty AdminPrivilegesProperty =
        DependencyProperty.Register("AdminPrivileges", typeof(bool), typeof(TaskWindow), new PropertyMetadata(null));

        public bool AdminPrivileges
        {
            get => (bool)GetValue(AdminPrivilegesProperty); set { SetValue(AdminPrivilegesProperty, value); }
        }


        public TaskWindow(int id = 0, bool adminPrivileges = true)
        {
            AdminPrivileges = adminPrivileges;
            InitializeComponent();
            if (id == 0)
            {
                Task = new BO.Task();
                Task.Created = s_bl.Clock;
            }
            else
            {
                Task = s_bl?.Task.Read(id)!;
            }
        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button.Content.ToString() == "Update")
            {


                try
                {
                    Task = s_bl?.Task.Update(Task)!;
                    MessageBox.Show($"Successfully updated task {Task.ID}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);


                    Close();
                }
                catch (BlDoesNotExistException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); //the errors are well written
                }
                catch (BlNullPropertyException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BlIllegalPropertyException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BlIllegalOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            else if (button.Content.ToString() == "Add")

                try
                {

                    Task = s_bl?.Task.Create(Task)!;
                    MessageBox.Show($"Successfully added task {Task.ID}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    Close();

                }
                catch (BlDoesNotExistException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error); //the errors are well written
                }
                catch (BlNullPropertyException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BlIllegalPropertyException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (BlIllegalOperationException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

        }

        private void engineerAssigned(object sender, EventArgs e)
        {
            Task = s_bl.Task.Read(Task.ID);

        }

        private void AssignEngineer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Task = s_bl.Task.Update(Task);
                TaskEngineerAssigner engineerAssigner = new TaskEngineerAssigner(Task);
                engineerAssigner.Closed += engineerAssigned!;
                engineerAssigner.Show();

            }
            catch (BlDoesNotExistException ex)
            {
                MessageBox.Show($"Make sure all fields for a task are legal:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error); //the errors are well written
            }
            catch (BlNullPropertyException ex)
            {
                MessageBox.Show($"Make sure all fields for a task are legal:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlIllegalPropertyException ex)
            {
                MessageBox.Show($"Make sure all fields for a task are legal:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (BlIllegalOperationException ex)
            {
                MessageBox.Show($"Make sure all fields for a task are legal:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        private void updateTaskAsComplete_Click(object sender, RoutedEventArgs e)
        {


            Task.ActualEnd = s_bl.Clock;

            Task.Engineer = null;

            s_bl.Task.Update(Task);


            Task = s_bl.Task.Update(Task);
        }
    }
}
