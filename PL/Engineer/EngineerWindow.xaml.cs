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
         DependencyProperty.Register("Engineer", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

        public BO.Engineer Engineer
        {
            get { return (BO.Engineer)GetValue(EngineerProperty); }
            set { SetValue(EngineerProperty, value); }
        }


        //for the task update
        public static readonly DependencyProperty TaskIDProperty =
         DependencyProperty.Register("TaskID", typeof(int?), typeof(EngineerWindow), new PropertyMetadata(null));


        public int? TaskID
        {
            get { return (int?)GetValue(TaskIDProperty); }
            set { SetValue(TaskIDProperty, value); }
        }


        public static readonly DependencyProperty PossibleTasksProperty =
            DependencyProperty.Register("PossibleTasks", typeof(ObservableCollection<int?>), typeof(EngineerWindow), new PropertyMetadata(null));

        public ObservableCollection<int?> PossibleTasks
        {
            get { return (ObservableCollection<int?>)GetValue(PossibleTasksProperty); }
            set { SetValue(PossibleTasksProperty, value); }
        }


        public EngineerWindow(int id = 0)
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

            PossibleTasks = new ObservableCollection<int?>(s_bl.Task.ReadAll(t => (EngineerExperience?)t.Difficulty <= Engineer.Level).Select(t => t.ID).Select(i=>(int?)i));
            PossibleTasks.Add(null);
            if (Engineer.Task != null)
            {
                TaskID = Engineer.Task.ID;
            }
          

        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {

            if (TaskID != null)
            {
                Engineer.Task = new TaskInEngineer { ID = TaskID.Value };
            }
          

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


        private void Experience_Level_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Engineer.Level == BO.EngineerExperience.None)
            {
                PossibleTasks = new ObservableCollection<int?> { null };
            }
            else
            {

                PossibleTasks = new ObservableCollection<int?>(s_bl.Task.ReadAll(t => (EngineerExperience?)t.Difficulty <= Engineer.Level).Select(t => t.ID).Select(i => (int?)i));
                PossibleTasks.Add(null);
            }

        }

    }

}