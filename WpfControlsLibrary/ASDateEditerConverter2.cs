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
    public class ASDateEditerConverter2 : IValueConverter
    {
        private static string _dateSeparator;
        private static string _displayFormat10;
        private static string _displayFormat8;
        private static string _nullDateDisplay10;
        private static string _nullDateDisplay8;
        private static bool _initialized = false;

        private static void EnsureInitialized()
        {
            if (_initialized) return;

            try
            {
                _dateSeparator = GetConfigValue("DateSeparator", "/");
                _displayFormat10 = GetConfigValue("DateFormat", "dd/MM/yyyy");
                _displayFormat8 = GetConfigValue("DateFormat8", "dd/MM/yy");
                _nullDateDisplay10 = GetConfigValue("NullDateDisplay", $"00{_dateSeparator}00{_dateSeparator}0000");
                _nullDateDisplay8 = GetConfigValue("NullDateDisplay8", $"00{_dateSeparator}00{_dateSeparator}00");
                _initialized = true;
            }
            catch
            {
                _dateSeparator = "/";
                _displayFormat10 = "dd/MM/yyyy";
                _displayFormat8 = "dd/MM/yy";
                _nullDateDisplay10 = "00/00/0000";
                _nullDateDisplay8 = "00/00/00";
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

        private static string GetNullDateDisplay(int size)
        {
            EnsureInitialized();
            return size == 8 ? _nullDateDisplay8 : _nullDateDisplay10;
        }

        private static string GetDisplayFormat(int size)
        {
            EnsureInitialized();
            return size == 8 ? _displayFormat8 : _displayFormat10;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            EnsureInitialized();
            int size = (int)parameter;
            DateTime dateTime = new DateTime(1,1,1);
            try
            {
                dateTime = (DateTime)value;
                if (size == 8)
                {
                    if (dateTime.Year == 1900 && dateTime.Day == 1 && dateTime.Month == 1)
                    {
                        return GetNullDateDisplay(8);
                    }
                    return dateTime.ToString(GetDisplayFormat(8));
                }
                else if (size == 10)
                {
                    if (dateTime.Year == 1900 && dateTime.Day == 1 && dateTime.Month == 1)
                    {
                        return GetNullDateDisplay(10);
                    }
                    return dateTime.ToString(GetDisplayFormat(10));
                }
            }
            catch
            {
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            EnsureInitialized();
            string strValue = value as string;
            DateTime resultTimeSpan;

            int size = (int)parameter;
            string format = GetDisplayFormat(size);
            string nullDisplay = GetNullDateDisplay(size);

            if (strValue == nullDisplay || IsAllZeros(strValue))
            {
                return new DateTime(1900, 1, 1);
            }
            else if (DateTime.TryParseExact(strValue, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out resultTimeSpan))
            {
                return resultTimeSpan;
            }
            return DependencyProperty.UnsetValue;
        }

        private static bool IsAllZeros(string displayDate)
        {
            if (string.IsNullOrEmpty(displayDate))
                return true;
            EnsureInitialized();
            var digitsOnly = displayDate.Replace(_dateSeparator, "").Replace("/", "").Replace("-", "").Replace(".", "");
            return !string.IsNullOrEmpty(digitsOnly) && digitsOnly.All(c => c == '0');
        }
    }
}