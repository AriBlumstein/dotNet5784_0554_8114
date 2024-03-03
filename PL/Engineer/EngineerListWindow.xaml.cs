using DO;
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
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        private static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public EngineerListWindow()
        {
            InitializeComponent();

            try
            {
                EngineerList = s_bl.Engineer.ReadAll()!; //getting all the engineers to display
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }


       //the engineerlist dependency property
        public IEnumerable<BO.Engineer> EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));


        //the experience property for diplaying the proper engineers
        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.None;


        /// <summary>
        /// event handler to display the engineers of a certain experience
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbExperience_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EngineerList = (Experience == BO.EngineerExperience.None) ?
            s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => (BO.EngineerExperience?)item.Exp == Experience)!; 

        }


        /// <summary>
        /// event handler when the engineer window closes
        /// //event is different but accomplishes very similar goals of updating this screen properly depending on the selected exp, repeated code, but decoupled, allows us to handle events differently if we choose to
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>

        private void engineerWindow_Closed(Object Sender, EventArgs e)
        {
            EngineerList = (Experience == BO.EngineerExperience.None) ?
            s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => (BO.EngineerExperience?)item.Exp == Experience)!;
        }


        /// <summary>
        /// event handler to add a new engineer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void btnAddNewEngineer(object sender, RoutedEventArgs e)
        {
            EngineerWindow newWindow = new EngineerWindow();
            newWindow.Closed += engineerWindow_Closed!;  //add our event listener to this event, so the event will be handled 
            newWindow.ShowDialog();
            
        }


        /// <summary>
        /// event handler when updating an engineer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void listClickUpdateEngineer(object sender, RoutedEventArgs e)
        {
            BO.Engineer? engineer = (sender as ListView)?.SelectedItem as BO.Engineer;

            if (engineer!=null)
            {
                EngineerWindow newWindow = new EngineerWindow(engineer.ID);
                newWindow.Closed += engineerWindow_Closed!;  //add our event listener to this event, so the event will be handled
                newWindow.ShowDialog();
            }
        }


        

        


    }


    
}
