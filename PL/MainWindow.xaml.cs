using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL;

using PL.Engineer;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Task;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <summary>
    /// show the task list window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    private void showTaskList_Click(object sender, RoutedEventArgs e)
    {
        new TaskListWindow().Show();
    }

    /// <summary>
    /// show the engineer list window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    private void showEngineers_Click(Object sender, RoutedEventArgs e)
    {
        new EngineerListWindow().Show();
    }


    /// <summary>
    /// init the database with random data
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void initializeData_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("Do you want to proceed? (doing so will reset data if there was)", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            DalTest.Initialization.Do();
        }
        
    }

}