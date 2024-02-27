


namespace PL
{
    using BO;
    using System.Collections;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

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

    class StringToNullableInt : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convert null to empty string
            if (value == null)
                return string.Empty;

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convert empty string to null
            if (string.IsNullOrWhiteSpace(value?.ToString()))
                return null;

            // Convert non-empty string to int?
            if (int.TryParse(value.ToString(), out int result))
                return result;

            return DependencyProperty.UnsetValue;
        }

       
    }


    class NullReplaceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value ?? parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.Equals(parameter) ? null : value;
        }
    }

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
    /// converter for the task properties in the engineer window
    /// </summary>
    public class CannotAssignATaskConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {

            bool collapsed = false;

            foreach (var value in values)
            {
                if (value is int)
                {
                    collapsed = (int)value == 0;
                    if(collapsed)
                    {
                        return Visibility.Collapsed;
                    }
                }
                   

                //we are now in out second check
                if (value is BO.TaskInEngineer)
                {
                    return (BO.TaskInEngineer)value!=null ? Visibility.Collapsed : Visibility.Visible;
                }

                //second check if converter is being used in a task window
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


    class CanAssignAnEngineer : IMultiValueConverter
    {
        

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var value in values)
            {
                if (value is int)
                {
                    if ((int)value==0)
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


  

}
