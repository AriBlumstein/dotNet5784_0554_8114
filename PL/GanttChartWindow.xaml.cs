using BlApi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PL
{
    public partial class GanttChartWindow : Window
    {
        private IEnumerable<TaskData> _items { get; set; }

        private static readonly IBl s_bl = BlApi.Factory.Get();


        private HashSet<int> _coloredRed;


        public GanttChartWindow()
        {
            InitializeComponent();
            _items = generateItems();
            DrawGanttChart();
        }

        private void DrawGanttChart()
        {
            // Clear any existing content
            GanttChartCanvas.Children.Clear();

            // Define some constants for rendering
            double rowHeight = 80; // Increased row height
            double columnWidth = 150; // Increased column width
            double xOffset = 150; // Margin for labels and name column
            double yOffset = 50; // Margin for labels
            double y = yOffset;

            // Find the earliest start date and latest end date
            DateTime startDate = _items.Min(item => item.StartDate);
            DateTime endDate = _items.Max(item => item.EndDate);

            // Calculate the total duration in weeks
            TimeSpan totalDuration = endDate - startDate;
            int numWeeks = (int)Math.Ceiling(totalDuration.TotalDays / 14); // Calculate number of 2-week intervals

            // Calculate the width and height of the canvas
            double canvasWidth = numWeeks * columnWidth + xOffset; // Adding margins for name column
            double canvasHeight = _items.Count() * rowHeight + yOffset;

            // Set the width and height of the canvas
            GanttChartCanvas.Width = canvasWidth;
            GanttChartCanvas.Height = canvasHeight;

            // Draw column headers (dates)
            for (int i = 0; i < numWeeks; i++)
            {
                DateTime currentWeekStart = startDate.AddDays(i * 14); // Increment by 14 days for 2-week interval
                TextBlock dateLabel = new TextBlock
                {
                    Text = currentWeekStart.ToShortDateString(),
                    Margin = new Thickness(xOffset + i * columnWidth + (columnWidth - 80) / 2, 0, 0, 0), // Centering the date
                    Width = 80, // Setting a fixed width to ensure entire date is visible
                    TextAlignment = TextAlignment.Center // Centering the text
                };
                GanttChartCanvas.Children.Add(dateLabel);

                // Draw black lines between each row and extend into the first column
                Rectangle columnLine = new Rectangle
                {
                    Width = 1,
                    Height = canvasHeight,
                    Fill = Brushes.Black,
                    Margin = new Thickness(xOffset + i * columnWidth, 0, 0, 0)
                };
                GanttChartCanvas.Children.Add(columnLine);
            }

            // Draw rows (items)
            foreach (var item in _items)
            {
                // Draw black lines between each row
                Rectangle rowLine = new Rectangle
                {
                    Width = canvasWidth - xOffset,
                    Height = 1,
                    Fill = Brushes.Black,
                    Margin = new Thickness(xOffset, y - 1, 0, 0)
                };
                GanttChartCanvas.Children.Add(rowLine);

                // Draw the name column
                TextBlock itemLabel = new TextBlock
                {
                    Text = item.Name,
                    Margin = new Thickness(0, y + 5, 0, 0),
                    Width = xOffset - 10, // Width of the name column
                    TextWrapping = TextWrapping.Wrap
                };
                GanttChartCanvas.Children.Add(itemLabel);

                // Draw the rectangle
                Rectangle rect = new Rectangle
                {
                    Width = (item.EndDate - item.StartDate).TotalDays / 14 * columnWidth, // Adjust width to 2-week interval
                    Height = rowHeight,
                    Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(item.Color)),
                    Margin = new Thickness(xOffset + (item.StartDate - startDate).TotalDays / 14 * columnWidth, y, 0, 0)
                };
                GanttChartCanvas.Children.Add(rect);

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
            GanttChartCanvas.Children.Add(bottomLine);
        }




        public class TaskData
        {
            public string Name { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string Color { get; set; }
        }


        /// <summary>
        /// helper method that gets the data items for the task
        /// </summary>
        /// <returns>IEnumerable of task data</returns>
        private IEnumerable<TaskData> generateItems()
        {
            _coloredRed = new HashSet<int>();

            return  from task in s_bl.Task.ReadAll()
                          let fullTask = s_bl.Task.Read(task.ID)
                          orderby fullTask.ProjectedEnd
                           select new TaskData
                         {
                            Name = getName(fullTask),
                            StartDate = getStart(fullTask),
                            EndDate = getEnd(fullTask),
                            Color = getColor(fullTask)

                         };

           
                  
        }


        private string getName(BO.Task task)
        {
            StringBuilder nameBuilder = new StringBuilder();
            nameBuilder.Append(task.ID).Append(": ").Append(task.Name).Append("\n");
            nameBuilder.Append("Dependencies:\n");

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
        /// helper method to get the proper start time to add to the Gantt chart
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>

        private DateTime getStart(BO.Task task)
        {
            if(task.ActualStart!=null)
            {
                return task.ActualStart.Value;
            }

            return task.ProjectedStart!.Value;
        }

        /// <summary>
        /// helper method to get the proper end time to add to the Gantt Chart
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>

        private DateTime getEnd(BO.Task task)
        {
            if(task.ActualEnd!=null)
            {
                return task.ActualEnd!.Value;
            }    
            return task.ProjectedEnd!.Value;
        }

        /// <summary>
        /// helper method to return the proper color the task should be colored
        /// gray for not started, green for in progress, blue complete, red if overtime and if a requisite is overtime
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private String getColor(BO.Task task)
        {
            //#6e6c67-gray
            //#37e031-green
            //#e60b07-red
            //#361c9e-blue

            //a requisite task is overdue
            foreach(var dependency in task.Dependencies)
            {
                if(_coloredRed.Contains(dependency.ID))
                    return "#e60b07";
            }
                
            

            //completed task
            if (task.Status==BO.Status.Completed)
            {
                return "#361c9e";
            }

            //in progress on schedule
            if(task.Status==BO.Status.OnTrack && task.ProjectedEnd>=s_bl.Clock.Date) 
            {
                return "#37e031";
            }

            //in progress behind schedule
            if(task.Status==BO.Status.OnTrack && task.ProjectedEnd < s_bl.Clock.Date)
            {
                //add it to the to the hash set
                _coloredRed.Add(task.ID);

                return "#e60b07";
            }


            //gray we have not started yet
            return "#6e6c67";





        }


    }






    
}
