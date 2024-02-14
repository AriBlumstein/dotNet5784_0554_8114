using System;
using System.Collections.Generic;
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
            MessageBox.Show("The add and update functionality for tasks is not ready yet for this stage (stage5) only the list view, check out Engineer Windows for add update functionality", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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


    }


}
