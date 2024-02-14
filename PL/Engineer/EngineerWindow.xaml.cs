using BlApi;
using PL.Task;
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



        public EngineerWindow(int id=0)
        {
            InitializeComponent();
            if(id == 0)
            {
                Engineer = new BO.Engineer();
            }
            else
            {
                Engineer = s_bl?.Engineer.Read(id)!;
                if(Engineer.Task!=null)
                {
                    TaskID=Engineer.Task.ID;
                }
             
            }
        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            //for the task
            if (TaskID.HasValue)
            {
                Engineer.Task = new BO.TaskInEngineer { ID = TaskID.Value };
            }
            else
            {
                Engineer.Task = null;
            }


            Button button = sender as Button;
            if(button.Content.ToString()=="Update")
            {

             
                try
                {
                    Engineer=s_bl?.Engineer.Update(Engineer)!;
                    MessageBox.Show($"Successfully updated engineer {Engineer.ID}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch(Exception ex) 
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            else if(button.Content.ToString()=="Add")
            
                try
                {
                    
                    Engineer = s_bl?.Engineer.Create(Engineer)!;
                    MessageBox.Show($"Successfully added engineer {Engineer.ID}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }

        }
    }

