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
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public EngineerListWindow()
        {
            InitializeComponent();
            EngineerList=s_bl.Engineer.ReadAll()!;
            
        }

        public IEnumerable<BO.Engineer> EngineerList
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }

        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));

        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.None;

        private void cbExperience_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EngineerList = (Experience == BO.EngineerExperience.None) ?
            s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => (BO.EngineerExperience?)item.Exp == Experience)!;

        }

        private void engineerWindow_Closed(Object Sender, EventArgs e)
        {
            EngineerList = (Experience == BO.EngineerExperience.None) ?
            s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => (BO.EngineerExperience?)item.Exp == Experience)!;
        }

        private void btnAddNewEngineer(object sender, RoutedEventArgs e)
        {
            EngineerWindow newWindow = new EngineerWindow();
            newWindow.Closed += engineerWindow_Closed!;
            newWindow.ShowDialog();
            
        }

        private void listClickUpdateEngineer(object sender, RoutedEventArgs e)
        {
            BO.Engineer? engineer = (sender as ListView)?.SelectedItem as BO.Engineer;

            if (engineer!=null)
            {
                EngineerWindow newWindow = new EngineerWindow(engineer.ID);
                newWindow.Closed += engineerWindow_Closed!;
                newWindow.ShowDialog();
            }
        }


        

        


    }


    
}
