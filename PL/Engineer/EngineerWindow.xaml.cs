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

        private readonly IBl s_bl = Factory.Get();


        public static readonly DependencyProperty EngineerProperty =
         DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));  //the engineer himself

        public BO.Engineer Engineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }



        public static readonly DependencyProperty AdminPrivilegesProperty =
        DependencyProperty.Register("AdminPrivileges", typeof(bool), typeof(EngineerWindow), new PropertyMetadata(null));

        public bool AdminPrivileges
        {
            get => (bool)GetValue(AdminPrivilegesProperty); set { SetValue(AdminPrivilegesProperty, value);}
        }

        public EngineerWindow(int id = 0, bool adminPrivileges = true)
        {
            AdminPrivileges = adminPrivileges;
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
        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {

          

            Button button = sender as Button;
            if (button.Content.ToString() == "Update")
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
            else if (button.Content.ToString() == "Add")

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

        

        private void taskAssigned(object sender, EventArgs e)
        {
            Engineer=s_bl.Engineer.Read(Engineer.ID);

        }



        private void AssignTaskClick(object sender, RoutedEventArgs e)
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

        private void viewTask_Click(object sender, RoutedEventArgs e)
        {
            BO.Task task = s_bl.Task.Read(Engineer.Task.ID);
            
            TaskWindow taskWindow=new TaskWindow(Engineer.Task.ID, AdminPrivileges);

            taskWindow.Closed += updateWindow!;
           
            taskWindow.ShowDialog(); 
           
        }


        private void updateWindow(object sender, EventArgs e)
        {
            Engineer=s_bl.Engineer.Read(Engineer.ID);
        }
    }


    

}