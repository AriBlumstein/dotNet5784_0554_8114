﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PL.Task
{
    /// <summary>
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {

        private static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public static readonly DependencyProperty TaskProperty =
        DependencyProperty.Register("Task", typeof(BO.Task), typeof(TaskListWindow), new PropertyMetadata(null));

        
        public BO.Task Task
        {
            get { return (BO.Task)GetValue(TaskProperty); }
            set { SetValue(TaskProperty, value); }
        }


        public static readonly DependencyProperty AdminPrivilegesProperty =
            DependencyProperty.Register("AdminPrivileges", typeof(bool), typeof(TaskListWindow), new PropertyMetadata(null));

    
        public bool AdminPrivileges
        {
            get => (bool)GetValue(AdminPrivilegesProperty);
            set => SetValue(AdminPrivilegesProperty, value);
        }


    public TaskListWindow(BO.Task task=null, bool adminPrivileges=true)
        {

            this.Task = task;
            AdminPrivileges = adminPrivileges;
            InitializeComponent();
          
            if (task == null) 
            { 
                TaskList = s_bl?.Task.ReadAll()!; 
                
            }
            else
            {
                TaskList = task.Dependencies;
                
            }

            
        
            
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
        
        //this event handler is specifically for the task window closing, we wanted to decouple this from the selection changed so we can change them freely

        private void windowClosed(object sender, EventArgs e)
        {
            if(Task!= null)
            {
                Task=s_bl?.Task.Read(Task.ID)!;
                TaskList= Task.Dependencies;
            }
            else
            {
                TaskList = (Experience == BO.EngineerExperience.None) ?
                s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(item => (BO.EngineerExperience?)item.Difficulty == Experience)!;
            }

           

        }

        private void listClickUpdateTask(object sender, MouseButtonEventArgs e)
        {
            if (Task!=null) { return; } //prevent view Task details if we look at them as depe, would lead to a non-user friendly set up


            BO.TaskInList? newTask = (sender as ListView)?.SelectedItem as BO.TaskInList;



            if (newTask != null)
            {
                
                TaskWindow newWindow = new TaskWindow(newTask.ID);

                newWindow.Closed += windowClosed!;  //add our event listener to this event, so the event will be handled
                newWindow.ShowDialog();
            }
        }

        private void addTaskClick(object sender, RoutedEventArgs e)
        {


            if (Task == null)
            {
                TaskWindow newWindow = new TaskWindow();
                newWindow.Closed += windowClosed!;  //add our event listener to this event, so the event will be handled 
                newWindow.ShowDialog();
            }
            else
            {
                AddDependencies newWindow = new AddDependencies(Task);
                newWindow.Closed += windowClosed!;
                newWindow.ShowDialog();
            }

        }
    }


}
