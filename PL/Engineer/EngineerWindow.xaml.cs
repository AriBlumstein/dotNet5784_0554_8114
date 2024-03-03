using BlApi;
using BO;
using PL.Task;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {

        private static readonly IBl s_bl = Factory.Get();


        public static readonly DependencyProperty EngineerProperty =
         DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));  //the engineer himself

        public BO.Engineer Engineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }

        //editing privileges
        public bool AdminPrivileges { get; init; }

        public EngineerWindow(int id = 0, bool adminPrivileges = true)
        {
            
            InitializeComponent();
            if (id == 0)
            {
                Engineer = new BO.Engineer();

            }
            else
            {
                try
                {
                    Engineer = s_bl?.Engineer.Read(id)!;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            AdminPrivileges = adminPrivileges;
        }


        /// <summary>
        /// event handler for add or update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        { 

            Button button = sender as Button;
            if (button.Content.ToString() == "Update") //we want to update
            {


                try
                {
                    Engineer = s_bl?.Engineer.Update(Engineer)!;
                    MessageBox.Show($"Successfully updated engineer {Engineer.ID}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                   

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
            else if (button.Content.ToString() == "Add")// adding a new engineer

                try
                {

                    Engineer = s_bl?.Engineer.Create(Engineer)!;
                    MessageBox.Show($"Successfully added engineer {Engineer.ID}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                  
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

        
        /// <summary>
        /// event handler that will update this window when the EngineerTaskAssigner window is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void taskAssigned(object sender, EventArgs e)
        {
            Engineer=s_bl.Engineer.Read(Engineer.ID);

        }


        /// <summary>
        /// event handler for assigning a task
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void assignTaskClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Engineer=s_bl.Engineer.Update(Engineer);
                EngineerTaskAssigner taskAssigner = new EngineerTaskAssigner(Engineer);
                taskAssigner.Closed += taskAssigned!;
                taskAssigner.Show();

            }
            catch (BlIllegalOperationException ex)
            {
                MessageBox.Show($"Make sure all other fields for an engineer are legal:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(BlNullPropertyException ex)
            {
                MessageBox.Show($"Make sure all other fields for an engineer are legal:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(BlIllegalPropertyException ex)
            {
                MessageBox.Show($"Make sure all other fields for an engineer are legal:\n{ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /// <summary>
        /// event handler to view the current task we are working on
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewTask_Click(object sender, RoutedEventArgs e)
        {
            BO.Task task = s_bl.Task.Read(Engineer.Task.ID);
            
            TaskWindow taskWindow=new TaskWindow(Engineer.Task.ID, AdminPrivileges);

            taskWindow.Closed += updateWindow!;
           
            taskWindow.ShowDialog(); 
           
        }


        /// <summary>
        /// event handler when the task is updated, decoupled from  the assigning event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void updateWindow(object sender, EventArgs e)
        {
            Engineer=s_bl.Engineer.Read(Engineer.ID);
        }
    }


    

}