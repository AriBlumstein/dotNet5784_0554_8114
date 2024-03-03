using BlApi;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private Canvas _ganttChartCanvas; //the ganttChart cnavas we need to deal wiht

        public GanttChartWindow()
        {
            InitializeComponent();
        }

        private void canvas_Loaded(object sender, RoutedEventArgs e)
        {

            _ganttChartCanvas = sender as Canvas; //we get the canvas with this line

            if (_ganttChartCanvas != null) 
            {
                _items = GenerateItems();
                DrawGanttChart();
            }
            else //dealing with possible error
            {
                MessageBox.Show("Unknow error. Cannot Display Gantt Chart.", "Error", MessageBoxButton.OK, MessageBoxImage.Error );
            }
        }

        private void DrawGanttChart()
        {
            if (_ganttChartCanvas == null || _items == null||_items.Count()==0)
                return; // Prevent error



            // Clear any existing content
            _ganttChartCanvas.Children.Clear();

            // Define some constants for rendering
            double rowHeight = 80; // Row height for each bar
            double columnWidth = 150; // Column width for the timing
            double xOffset = 150; // Margin for labels and name column
            double yOffset = 50; // Margin for labels
            double y = yOffset; // Helper entity to help the drawing process

            // Find the earliest start date and latest end date so our Gantt chart is properly set up
            DateTime startDate = _items.Min(item => item.StartDate);
            DateTime endDate = _items.Max(item => item.EndDate);

            // Calculate the total duration in 4-week stints
            TimeSpan totalDuration = endDate - startDate;
            int numStints = (int)Math.Ceiling(totalDuration.TotalDays / (28)); // Calculate number of 4-week intervals

            // Calculate the width and height of the canvas
            double canvasWidth = numStints * columnWidth + xOffset; // Adding margins for name column so it can be fully displayed
            double canvasHeight = _items.Count() * rowHeight + yOffset;

            // Set the width and height of the canvas
            _ganttChartCanvas.Width = canvasWidth;
            _ganttChartCanvas.Height = canvasHeight;

            // Draw column headers (dates)
            for (int i = 0; i < numStints; i++)
            {
                DateTime currentStintStart = startDate.AddDays(i * 28); // Increment by 4 weeks
                TextBlock dateLabel = new TextBlock
                {
                    Text = currentStintStart.ToShortDateString(),
                    Margin = new Thickness(xOffset + i * columnWidth + (columnWidth - 80) / 2, 0, 0, 0), // Centering the date
                    Width = 80, // Setting a fixed width to ensure entire date is visible
                    TextAlignment = TextAlignment.Center // Centering the text
                };
                _ganttChartCanvas.Children.Add(dateLabel);

                // Draw black lines between each row and extend into the first column
                Rectangle columnLine = new Rectangle
                {
                    Width = 1,
                    Height = canvasHeight,
                    Fill = Brushes.Black,
                    Margin = new Thickness(xOffset + i * columnWidth, 0, 0, 0)
                };
                _ganttChartCanvas.Children.Add(columnLine);
            }

            // Draw rows (items)
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
                    Width = (item.EndDate - item.StartDate).TotalDays / (28) * columnWidth, // Adjust width to 4-week intervals of the chart
                    Height = rowHeight,
                    Fill = new SolidColorBrush(item.Color),
                    Margin = new Thickness(xOffset + (item.StartDate - startDate).TotalDays / (28) * columnWidth, y, 0, 0)
                };
                _ganttChartCanvas.Children.Add(rect);

                y += rowHeight;

                // Adjust the row position for expanded height
                y += 20; // Increase space between rows
            }

            // Draw a black line at the bottom
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
        /// helper method to generate the proper data needed for the gantt chart
        /// </summary>
        /// <returns>a list of Data items, one for each task</returns>
        private IEnumerable<TaskData> GenerateItems()
        {
            _coloredRed = new HashSet<int>(); //define this here will be used in this methods helper methods

            return from task in s_bl.Task.ReadAll()
                   let fullTask = s_bl.Task.Read(task.ID)
                   orderby fullTask.ProjectedEnd
                   select new TaskData
                   {
                       Name = GetName(fullTask),
                       StartDate = GetStart(fullTask),
                       EndDate = GetEnd(fullTask),
                       Color = GetColor(fullTask)
                   };
        }


        /// <summary>
        /// helper method to get the proper name
        /// </summary>
        /// <param name="task"></param>
        /// <returns>a data item name</returns>
        private string GetName(BO.Task task)
        {
            StringBuilder nameBuilder = new StringBuilder();
            nameBuilder.Append(task.ID).Append(": ").Append(task.Name).Append("\n"); //the actual task name an ID
            nameBuilder.Append("Dependencies:\n");


            //getting the dependencies
            int dependencyCount = task.Dependencies.Count;
            for (int i = 0; i < dependencyCount; i++)
            {
                nameBuilder.Append(task.Dependencies[i].ID);
                if (i < dependencyCount - 1)
                {
                    nameBuilder.Append(", ");
                }
            }

            return nameBuilder.ToString();
        }


        /// <summary>
        /// helper method to get the proper start date for a task
        /// </summary>
        /// <param name="task"></param>
        /// <returns>datetime</returns>
        private DateTime GetStart(BO.Task task)
        {
            if (task.ActualStart != null)
            {
                return task.ActualStart.Value;
            }

            return task.ProjectedStart!.Value;
        }


        /// <summary>
        /// helper method to get a proper end date for a task
        /// </summary>
        /// <param name="task"></param>
        /// <returns>DateTime</returns>
        private DateTime GetEnd(BO.Task task)
        {
            if (task.ActualEnd != null)
            {
                return task.ActualEnd!.Value;
            }
            return task.ProjectedEnd!.Value;
        }


        /// <summary>
        /// helper method to get what color the task should be, gray if not started, green on schedule, blue completed, red behind schedule or a requisite task is behind schedule
        /// </summary>
        /// <param name="task"></param>
        /// <returns>Color</returns>
        private Color GetColor(BO.Task task)
        {
            //#6e6c67-gray
            //#37e031-green
            //#e60b07-red
            //#361c9e-blue

            //return ColorConverter.ConvertFromString(item.Color)

            //a requisite task is overdue
            foreach (var dependency in task.Dependencies)
            {
                if (_coloredRed.Contains(dependency.ID))
                    return (Color)ColorConverter.ConvertFromString("#e60b07");
                    
            }

            //completed task
            if (task.Status == BO.Status.Completed)
            {
                return (Color)ColorConverter.ConvertFromString("#361c9e");
               
            }

            //in progress on schedule
            if (task.Status == BO.Status.OnTrack && task.ProjectedEnd >= s_bl.Clock.Date)
            {
                return (Color)ColorConverter.ConvertFromString("#37e031");
          
            }

            //in progress behind schedule
            if (task.Status == BO.Status.OnTrack && task.ProjectedEnd < s_bl.Clock.Date)
            {
                //add it to the to the hash set
                _coloredRed.Add(task.ID);

                return (Color)ColorConverter.ConvertFromString("#e60b07");

               
            }

            

            //gray we have not started yet
            return (Color)ColorConverter.ConvertFromString("#6e6c67");
        }

        private class TaskData
        {
            public string Name { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public Color Color { get; set; }
        }

        
    }
}

