using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WpfControlsLibrary
{
    /// <summary>
    /// Converter for ASDateEditer that handles display format conversion.
    /// Uses App.config settings for date format configuration.
    /// </summary>
    public class ASDateEditerConverter : IValueConverter
    {
        private static string _displayFormat10;
        private static string _displayFormat8;
        private static string _nullDateInternal;
        private static string _dateSeparator;
        private static DateTime _nullDateTime;
        private static bool _initialized = false;

        private static void EnsureInitialized()
        {
            if (_initialized) return;

            try
            {
                _dateSeparator = GetConfigValue("DateSeparator", "/");
                _displayFormat10 = GetConfigValue("DateFormat", "dd/MM/yyyy");
                _displayFormat8 = GetConfigValue("DateFormat8", "dd/MM/yy");
                _nullDateInternal = GetConfigValue("NullDateInternal", "1900-01-01");

                if (DateTime.TryParseExact(_nullDateInternal, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                {
                    _nullDateTime = dt;
                }
                else
                {
                    _nullDateTime = new DateTime(1900, 1, 1);
                }

                _initialized = true;
            }
            catch
            {
                _dateSeparator = "/";
                _displayFormat10 = "dd/MM/yyyy";
                _displayFormat8 = "dd/MM/yy";
                _nullDateInternal = "1900-01-01";
                _nullDateTime = new DateTime(1900, 1, 1);
                _initialized = true;
            }
        }

        private static string GetConfigValue(string key, string defaultValue)
        {
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                return string.IsNullOrEmpty(value) ? defaultValue : value;
            }
            catch
            {
                return defaultValue;
            }
        }

        private static bool IsNullDate(DateTime dt)
        {
            return dt.Year <= 1900 && dt.Month == 1 && dt.Day == 1;
        }

        private static string GetNullDateDisplay(int size)
        {
            EnsureInitialized();
            if (size == 8)
                return $"00{_dateSeparator}00{_dateSeparator}00";
            return $"00{_dateSeparator}00{_dateSeparator}0000";
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            EnsureInitialized();

            int size = parameter != null ? (int)parameter : 10;
            
            try
            {
                if (value == null)
                {
                    return GetNullDateDisplay(size);
                }

                DateTime dateTime;
                
                // Handle string input (internal format yyyy-MM-dd)
                if (value is string strValue)
                {
                    if (string.IsNullOrEmpty(strValue) || strValue == _nullDateInternal)
                    {
                        return GetNullDateDisplay(size);
                    }
                    
                    if (DateTime.TryParseExact(strValue, "yyyy-MM-dd", 
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        if (IsNullDate(dateTime))
                            return GetNullDateDisplay(size);
                        
                        return dateTime.ToString(size == 8 ? _displayFormat8 : _displayFormat10);
                    }
                    return GetNullDateDisplay(size);
                }
                
                // Handle DateTime input
                dateTime = (DateTime)value;
                if (IsNullDate(dateTime))
                {
                    return GetNullDateDisplay(size);
                }
                
                return dateTime.ToString(size == 8 ? _displayFormat8 : _displayFormat10);
            }
            catch
            {
                return GetNullDateDisplay(size);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            EnsureInitialized();

            string strValue = value as string;
            int size = parameter != null ? (int)parameter : 10;
            string format = size == 8 ? _displayFormat8 : _displayFormat10;

            // Check for null date display
            string nullDisplay = GetNullDateDisplay(size);
            if (string.IsNullOrEmpty(strValue) || strValue == nullDisplay)
            {
                // Return internal format null date for string targets
                if (targetType == typeof(string))
                    return _nullDateInternal;
                // Return DateTime for DateTime targets
                return _nullDateTime;
            }

            // Check if all zeros
            var digitsOnly = strValue.Replace(_dateSeparator, "").Replace("/", "").Replace("-", "").Replace(".", "");
            if (!string.IsNullOrEmpty(digitsOnly) && digitsOnly.All(c => c == '0'))
            {
                if (targetType == typeof(string))
                    return _nullDateInternal;
                return _nullDateTime;
            }

            if (DateTime.TryParseExact(strValue, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultDateTime))
            {
                // Return based on target type
                if (targetType == typeof(string))
                    return resultDateTime.ToString("yyyy-MM-dd");
                return resultDateTime;
            }

            return DependencyProperty.UnsetValue;
        }
    }
}