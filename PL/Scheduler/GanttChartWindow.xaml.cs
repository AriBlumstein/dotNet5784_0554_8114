using BlApi;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PL
{
    public partial class GanttChartWindow : Window
    {
        private IEnumerable<TaskData> _items; //the data needed for the gantt chart

        private static readonly IBl s_bl = BlApi.Factory.Get();

        private HashSet<int> _coloredRed; //helper hash set so we know what to color red

        private Canvas _ganttChartCanvas; //The canvas we will draw the gantt chart on

        public GanttChartWindow()
        {
            InitializeComponent();
        }

        private void canvas_Loaded(object sender, RoutedEventArgs e)
        {

            _ganttChartCanvas = sender as Canvas; //retrieve the canvas

            if (_ganttChartCanvas != null) 
            {
                try
                {
                    _items = GenerateItems();
                    DrawGanttChart();
                } catch (Exception)
                {
                    MessageBox.Show("Error in system time, please check system clock", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
            }
            else //dealing with any possible error in the xaml file
            {
                MessageBox.Show("Xaml file corrupted. Cannot display the Gantt Chart.", "Error", MessageBoxButton.OK, MessageBoxImage.Error );
                Close();
            }
        }

        /// <summary>
        /// method to actually draw the gantt chart and added it to the canvas
        /// </summary>
        private void DrawGanttChart()
        {
            if (_ganttChartCanvas == null || _items == null || _items.Count() == 0)
                return; // Prevent error

            // Clear any existing content
            _ganttChartCanvas.Children.Clear();

            // Define some constants for rendering
            const double rowHeight = 50; // Row height for each bar
            const double columnWidth = 150; // Column width for the timing
            double xOffset = 150; // Margin for labels and name column
            double yOffset = 50; // Margin for labels
            double y = yOffset; // Helper entity to help the drawing process

            // Find the earliest start date and latest end date to define the bounds of our Gantt chart 
            DateTime startDate = _items.Min(item => item.StartDate);
            DateTime endDate = _items.Max(item => item.EndDate);

            // Calculate the total duration in 4-week stints
            TimeSpan totalDuration = endDate - startDate;
            int numStints = (int)Math.Ceiling(totalDuration.TotalDays / 28); // Calculate number of 4-week intervals

            // Calculate the width and height of the canvas
            double canvasWidth = numStints * columnWidth + xOffset; // Adding margins for name column so it can be fully displayed
            double canvasHeight = (_items.Count() + 1) * rowHeight + yOffset; // Add additional space for bottom line

            // Set the width and height of the canvas
            _ganttChartCanvas.Width = canvasWidth;
            _ganttChartCanvas.Height = canvasHeight;

            // Draw column headers (dates)
            for (int i = 0; i < numStints; i++)
            {
                DateTime currentStintStart = startDate.AddDays(i * 28); // Increment by 28 days (4 weeks)
                TextBlock dateLabel = new TextBlock
                {
                    Text = currentStintStart.ToShortDateString(),
                    Margin = new Thickness(xOffset + i * columnWidth + (columnWidth - 80) / 2, 0, 0, 0), // center the date
                    Width = 80, // set a fixed width to ensure the entire date is visible
                    TextAlignment = TextAlignment.Center // center the text
                };
                _ganttChartCanvas.Children.Add(dateLabel); // The name may not fit within the rectangle so it gets its own column.

                // Draw grid lines between each row and extend into the first column
                Rectangle columnLine = new Rectangle
                {
                    Width = 1,
                    Height = canvasHeight - yOffset,
                    Fill = Brushes.Black,
                    Margin = new Thickness(xOffset + i * columnWidth, yOffset, 0, 0)
                };
                _ganttChartCanvas.Children.Add(columnLine);
            }

            // Draw rows (items) the task rectangle itself
            foreach (var item in _items)
            {
                // Draw black lines between each row for clarity
                Rectangle rowLine = new Rectangle
                {
                    Width = canvasWidth - xOffset,
                    Height = 1,
                    Fill = Brushes.Black,
                    Margin = new Thickness(xOffset, y - 1, 0, 0)
                };
                _ganttChartCanvas.Children.Add(rowLine);

                // Draw the name column
                TextBlock itemLabel = new TextBlock
                {
                    Text = item.Name,
                    Margin = new Thickness(0, y + 5, 0, 0),
                    Width = xOffset - 10, // Width of the name column
                    TextWrapping = TextWrapping.Wrap
                };
                _ganttChartCanvas.Children.Add(itemLabel);

                // Draw the rectangle
                Rectangle rect = new Rectangle
                {
                    Width = (item.EndDate - item.StartDate).TotalDays / 28 * columnWidth, // Adjust width to 4-week intervals of the chart
                    Height = rowHeight,
                    Fill = new SolidColorBrush(item.Color),
                    Margin = new Thickness(xOffset + (item.StartDate - startDate).TotalDays / 28 * columnWidth, y, 0, 0)
                };
                _ganttChartCanvas.Children.Add(rect);

                // Adjust the row position for expanded height
                y += rowHeight+20; // Increase space between rows
               
            }

            // Draw a black line at the bottom to add clarity
            Rectangle bottomLine = new Rectangle
            {
                Width = canvasWidth - xOffset,
                Height = 1,
                Fill = Brushes.Black,
                Margin = new Thickness(xOffset, canvasHeight - 1, 0, 0)
            };
            _ganttChartCanvas.Children.Add(bottomLine);
        }




        /// <summary>
        /// helper method to generate the data needed for the gantt chart
        /// </summary>
        /// <returns>a list of TaskData items, one for each task</returns>
        private IEnumerable<TaskData> GenerateItems()
        {
            _coloredRed = new HashSet<int>(); //used in helper methods

            return from task in s_bl.Task.ReadAll()
                   let fullTask = s_bl.Task.Read(task.ID)
                   orderby fullTask.ProjectedEnd //makes the graph clearer, purely asthetic.
                   select new TaskData
                   {
                       Name = getName(fullTask),
                       StartDate = getStart(fullTask),
                       EndDate = getEnd(fullTask),
                       Color = getColor(fullTask)
                   };
        }


        /// <summary>
        /// Method to get the proper name of a task for display on the gantt chart
        /// </summary>
        /// <param name="task"></param>
        /// <returns>a data item name</returns>
        private string getName(BO.Task task)
        {
            StringBuilder nameBuilder = new StringBuilder();
            nameBuilder.Append(task.ID).Append(": ").Append(task.Name).Append("\n"); //the actual task name and ID

            nameBuilder.Append("{ ");

            //get the dependencies
            int dependencyCount = task.Dependencies.Count;
            for (int i = 0; i < dependencyCount; i++)
            {
                nameBuilder.Append(task.Dependencies[i].ID);
                if (i < dependencyCount - 1)
                {
                    nameBuilder.Append(", ");
                }
            }

            nameBuilder.Append(" }");

            return nameBuilder.ToString();
        }


        /// <summary>
        /// helper method to get the proper start date for a task
        /// </summary>
        /// <param name="task"></param>
        /// <returns>datetime</returns>
        private DateTime getStart(BO.Task task)
        {
            if (task.ActualStart != null)
            {
                return task.ActualStart.Value;
            }

            return task.ProjectedStart!.Value;
        }


        /// <summary>
        /// helper method to get the proper end date for a task
        /// </summary>
        /// <param name="task"></param>
        /// <returns>DateTime</returns>
        private DateTime getEnd(BO.Task task)
        {
            if (task.ActualEnd != null)
            {
                return task.ActualEnd!.Value;
            }
            return task.ProjectedEnd!.Value;
        }


        /// <summary>
        /// helper method to determine what color the task should be
        /// Gray if not started
        /// Green on schedule
        /// Blue if already completed
        /// Red if behind schedule or a requisite task is behind schedule
        /// </summary>
        /// <param name="task"></param>
        /// <returns>Color</returns>
        private Color getColor(BO.Task task)
        {

            const string gray = "#6e6c67";
            const string green = "#37e031";
            const string red = "#e60b07";
            const string blue = "#361c9e";

            //a requisite task is overdue
            foreach (var dependency in task.Dependencies)
                if (_coloredRed.Contains(dependency.ID))
                    return (Color)ColorConverter.ConvertFromString(red);
                    
            
            //the task is completed 
            if (task.Status == BO.Status.Completed)
                return (Color)ColorConverter.ConvertFromString(blue);
               
            

            //the task is in progress and on schedule
            if (task.Status == BO.Status.OnTrack && task.ProjectedEnd >= s_bl.Clock.Date)
                return (Color)ColorConverter.ConvertFromString(green);
          

            //the task is in progress and behind schedule
            if (task.Status == BO.Status.OnTrack && task.ProjectedEnd < s_bl.Clock.Date)
            {
                //add the task id to the to the hash set
                _coloredRed.Add(task.ID);
                return (Color)ColorConverter.ConvertFromString(red);
            }


            //gray we have not started yet
            return (Color)ColorConverter.ConvertFromString(gray);
        }


        /// <summary>
        /// A data class to hold the relevant information needed to draw the gantt chart 
        /// </summary>
        private class TaskData
        {
            public string Name { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public Color Color { get; set; }
        }

        
    }
}

