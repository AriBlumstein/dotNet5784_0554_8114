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


        //for the task update, needed cause the task may be null, and we want to show only task numbers for this screen, more concise
        public static readonly DependencyProperty TaskIDProperty =
         DependencyProperty.Register("TaskID", typeof(int?), typeof(EngineerWindow), new PropertyMetadata(null));


        public int? TaskID
        {
            get { return (int?)GetValue(TaskIDProperty); }
            set { SetValue(TaskIDProperty, value); }
        }


        public static readonly DependencyProperty PossibleTasksProperty =
            DependencyProperty.Register("PossibleTasks", typeof(ObservableCollection<int?>), typeof(EngineerWindow), new PropertyMetadata(null));  //the possible tasks to choose from to assign

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

            PossibleTasks = new ObservableCollection<int?>(s_bl.Task.ReadAll(t => (EngineerExperience?)t.Difficulty <= Engineer.Level).Select(t=>s_bl.Task.Read(t.ID)).Where(t=>noDependencies(t)).Select(t => t.ID).Select(i=>(int?)i));
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
                                        //logic to be added based on clocke to be added, setting the start time
            else
            {
                Engineer.Task = null;
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
                PossibleTasks = new ObservableCollection<int?> ();
                
            }
            else
            {

                PossibleTasks = PossibleTasks = new ObservableCollection<int?>(s_bl.Task.ReadAll(t => (EngineerExperience?)t.Difficulty <= Engineer.Level).Select(t => s_bl.Task.Read(t.ID)).Where(t => noDependencies(t)).Select(t => t.ID).Select(i => (int?)i));


            }

            if(TaskID!=null)
            {
                BO.Task cur = s_bl.Task.Read(TaskID.Value);

                if (cur.Complexity > Engineer.Level)
                {
                    TaskID = null;
                }
            }

        }



        /// <summary>
        /// private method that determines if this is a task with no more dependencies
        /// </summary>
        /// <param name="cur"></param>
        /// <returns>bool</returns>

        private bool noDependencies(BO.Task cur)
        {
            return s_bl.Task.ReadDependencies(cur).Count()==0;
        }

    }


    

}