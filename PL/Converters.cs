﻿


namespace PL
{
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

    public class StringToNullableInt : IValueConverter
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

    public class EnumerableNullReplaceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var collection = (IEnumerable)value;

            return
                collection
                .Cast<object>()
                .Select(x => x ?? parameter)
                .ToArray();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int?)value;
        }
    }
}
