


namespace PL
{
    using BO;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;


    /// <summary>
    /// conversion of an engineer ID to proper content for a button
    /// </summary>
    class ConvertIdToContent : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value == 0 ? "Add" : "Update";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// converter that will determine if I can change an ID value, only on add
    /// </summary>

    class ConvertIdToUpdatable : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value == 0 ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

  
    /// <summary>
    /// if the value is null, we don't want the element to be visible
    /// </summary>
    class ZeroToInvisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value == 0 ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// converter for the task properties in the engineer/task window, for deciding whether or not to display buttons that allow task assignment
    /// </summary>
    public class CannotAssignATaskConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            bool collapsed = false;

            foreach (var value in values)
            {
                if (value is int) // the engineer id, cannot be assigned a task if he does not exist yet
                {
                    collapsed = (int)value == 0;
                    if(collapsed)
                    {
                        return Visibility.Collapsed;
                    }
                }
                   

                //we are now in out second check
                if (value is BO.TaskInEngineer) //if a task was already assigned to gim
                {
                    return (BO.TaskInEngineer)value!=null ? Visibility.Collapsed : Visibility.Visible;
                }

                //second check if converter is being used in a task window, task was already assigned
                if(value is BO.EngineerInTask)
                {
                    return (BO.EngineerInTask)value!=null? Visibility.Collapsed : Visibility.Visible;
                }
            }
           

            // Otherwise, return Visible
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// for displaying the details of the task assigned to the engineer, or the engineer assigined to the task
    /// </summary>

    class ItemIsDisplayableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TaskInEngineer || value is EngineerInTask)
            {
                return  Visibility.Visible;
            }
            

            return Visibility.Collapsed;
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// determine if the page allows editing rights on admin only fields
    /// </summary>
    class AdminPrivilegesToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value? Visibility.Visible: Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    /// <summary>
    /// in task window, show the ability to assign an engineer, If I don't exist yet, or I already ended I cannot assign an engineer
    /// </summary>
    class CanAssignAnEngineer : IMultiValueConverter
    {
        
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var value in values)
            {
                if (value is int)
                {
                    if ((int)value==0) //task did not exist yet
                    {
                        return Visibility.Collapsed;
                    }
                }

                // second check
                if (value is (DateTime))
                {
                    return Visibility.Collapsed;
                }
            }

            return Visibility.Visible;
        }

       

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    /// <summary>
    /// in task list window, if I was not dealing with a specific task at that time, I allow the add/update button ot be visible
    /// </summary>

    class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    

    /// <summary>
    /// if the title of to show we are dealing with dependencies is visible or not
    /// </summary>

    class NullToInvisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
             return value==null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }

    class BoolToInvisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value == true ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


}
