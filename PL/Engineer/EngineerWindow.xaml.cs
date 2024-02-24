using BlApi;
using BO;
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


       


        public EngineerWindow(int id = 0, bool editingRights=true)
        {
            InitializeComponent();
            if (id == 0)
            {
                Engineer = new BO.Engineer();

            }
            else
            {
                Engineer = s_bl?.Engineer.Read(id)!;
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
                s_bl.Engineer.Update(Engineer);
                EngineerTaskAssigner taskAssigner = new EngineerTaskAssigner(Engineer.ID);
                taskAssigner.Closed += taskAssigned!;
                taskAssigner.Show();

            }
            catch (BlDoesNotExistException ex)
            {
                try {
                    s_bl.Engineer.Create(Engineer);
                    EngineerTaskAssigner taskAssigner = new EngineerTaskAssigner(Engineer.ID);
                    taskAssigner.Closed += taskAssigned!;
                    taskAssigner.Show();
                }  
                catch (Exception)
                {
                    MessageBox.Show("Make sure all other fields for an engineer are legal", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            catch( Exception)
            {
                MessageBox.Show("Make sure all other fields for an engineer are legal", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void updateTaskAsComplete_Click(object sender, RoutedEventArgs e)
        {
            BO.Task task = s_bl.Task.Read(Engineer.Task.ID);
            
            task.ActualEnd = s_bl.Clock;

            task.Engineer = null;

            s_bl.Task.Update(task);



            Engineer.Task = null;


            Engineer = s_bl.Engineer.Update(Engineer);

           
        }
    }


    

}