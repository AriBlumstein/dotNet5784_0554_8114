using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerTaskAssigner.xaml
    /// </summary>
    public partial class EngineerTaskAssigner : Window
    {
        private readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        private BO.Engineer engineer;
        
        public static readonly DependencyProperty PossibleTasksProperty = DependencyProperty.Register("PossibleTasks", typeof(ObservableCollection<TaskInList>),
            typeof(EngineerTaskAssigner), new PropertyMetadata(null));

        public ObservableCollection<TaskInList> PossibleTasks
        {
            get { return (ObservableCollection<TaskInList>)GetValue(PossibleTasksProperty);  }
            set { SetValue(PossibleTasksProperty, value);   }
        }
        
        public EngineerTaskAssigner(int EngineerID)
        {
            InitializeComponent();
            engineer = s_bl.Engineer.Read(EngineerID);

            if (engineer.Level == EngineerExperience.None)
            {
                PossibleTasks = new ObservableCollection<TaskInList>();
            }
            else
            {
                PossibleTasks = new ObservableCollection<TaskInList>(s_bl.Task.ReadAll(t => (EngineerExperience?)t.Difficulty <= engineer.Level).Select(t => s_bl.Task.Read(t.ID)).Where(t => noDependencies(t)).Where(t=>t.Engineer==null).Select(t => new TaskInList { ID = t.ID, Description = t.Descripiton, Name = t.Name, Status = t.Status }));
            }
            
        }




        private void ListAddTask_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;
            if (task != null)
            {
                MessageBoxResult answer  =MessageBox.Show($"Are you sure you want to assign task with \"{task.ID}\" to the engineer with id \"{engineer.ID}\"", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (answer == MessageBoxResult.Yes)
                {
                    engineer.Task = new TaskInEngineer
                    {
                        ID=task.ID,
                        Alias=task.Name,  
                    };

                    BO.Task curTask = s_bl.Task.Read(task.ID);

                    curTask.ActualStart = s_bl.Clock;

                    s_bl.Task.Update(curTask);

                    s_bl.Engineer.Update(engineer);
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
