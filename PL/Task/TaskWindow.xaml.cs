using BlApi;
using System.Windows;

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



        public TaskWindow(int id=0, bool adminPrivileges = true)
        {
            AdminPrivileges = adminPrivileges;
            InitializeComponent();
            if (id == 0)
            {
                Task = new BO.Task();

            }
            else
            {
                Task = s_bl?.Task.Read(id)!;
            }
        }

    }
}
