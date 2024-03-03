using System.Windows;

namespace PL;

using BlApi;
using PL.Components;
using PL.Engineer;
using Task;

/// <summary>
/// Interaction logic for Admin.xaml
/// </summary>
public partial class Admin : Window
{
    private static readonly IBl s_bl = BlApi.Factory.Get();

    public Admin()
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

        try
        {
            if (result == MessageBoxResult.Yes)
            {
                DalTest.Initialization.Do();
            }
        } catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        
    }


    /// <summary>
    /// event handler to reset the database
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void reset_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult result = MessageBox.Show("Do you want to proceed? (doing so will clear all data if there was any)", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {   
         s_bl.Reset();   
        }
    }


    /// <summary>
    /// event handler to lead us to the scheduling window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void beginProduction_Click(object sender, RoutedEventArgs e)
    {
        new SchedulerWindow().ShowDialog();
    }


    /// <summary>
    /// method handler to lead us to the gannt chart
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void showGanttChart_Click(object sender, RoutedEventArgs e)
    {
        if(!s_bl.Schedular.InProduction())
        {
            MessageBox.Show("Gantt Chart unavailable, please start production", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        new GanttChartWindow().ShowDialog();
    }
}