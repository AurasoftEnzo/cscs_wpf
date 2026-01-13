using System;
using System.Configuration;
using System.Globalization;
using System.Linq;

namespace WpfCSCS
{
    /// <summary>
    /// Centralized date configuration for CSCS_WPF.
    /// Manages all date-related settings, conversions, and formatting.
    /// Internal format is always yyyy-MM-dd (ISO 8601).
    /// </summary>
    public static class DateConfiguration
    {
        #region Constants

        /// <summary>
        /// Internal format - ALWAYS "yyyy-MM-dd" for storage, scripts, and database.
        /// </summary>
        public const string InternalFormat = "yyyy-MM-dd";

        #endregion

        #region Configuration Properties

        /// <summary>
        /// Null date internal representation (default: 1900-01-01)
        /// </summary>
        public static string NullDateInternal { get; private set; } = "1900-01-01";

        /// <summary>
        /// Null date as DateTime object
        /// </summary>
        public static DateTime NullDateTime { get; private set; } = new DateTime(1900, 1, 1);

        /// <summary>
        /// Date separator for display (default: /)
        /// </summary>
        public static string DateSeparator { get; private set; } = "/";

        /// <summary>
        /// Display format for 10-character dates (default: dd/MM/yyyy)
        /// </summary>
        public static string DisplayFormat10 { get; private set; } = "dd/MM/yyyy";

        /// <summary>
        /// Display format for 8-character dates (default: dd/MM/yy)
        /// </summary>
        public static string DisplayFormat8 { get; private set; } = "dd/MM/yy";

        /// <summary>
        /// Null date display for 10-character format (default: 00/00/0000)
        /// </summary>
        public static string NullDateDisplay10 { get; private set; } = "00/00/0000";

        /// <summary>
        /// Null date display for 8-character format (default: 00/00/00)
        /// </summary>
        public static string NullDateDisplay8 { get; private set; } = "00/00/00";

        /// <summary>
        /// Whether the configuration has been initialized
        /// </summary>
        public static bool IsInitialized { get; private set; } = false;

        #endregion

        #region Initialization

        /// <summary>
        /// Initialize date configuration from App.config settings.
        /// Call this once at application startup.
        /// </summary>
        public static void Initialize()
        {
            if (IsInitialized) return;

            try
            {
                // Read from App.config
                DateSeparator = GetConfigValue("DateSeparator", "/");
                DisplayFormat10 = GetConfigValue("DateFormat", "dd/MM/yyyy");
                DisplayFormat8 = GetConfigValue("DateFormat8", "dd/MM/yy");
                NullDateInternal = GetConfigValue("NullDateInternal", "1900-01-01");

                // Build null date display strings based on separator
                NullDateDisplay10 = GetConfigValue("NullDateDisplay", $"00{DateSeparator}00{DateSeparator}0000");
                NullDateDisplay8 = GetConfigValue("NullDateDisplay8", $"00{DateSeparator}00{DateSeparator}00");

                // Parse null date
                if (DateTime.TryParseExact(NullDateInternal, InternalFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime nullDt))
                {
                    NullDateTime = nullDt;
                }

                IsInitialized = true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DateConfiguration.Initialize failed: {ex.Message}");
                // Use defaults if configuration fails
                IsInitialized = true;
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

        #endregion

        #region Conversion Methods - Internal to Display

        /// <summary>
        /// Converts internal format (yyyy-MM-dd) to display format.
        /// </summary>
        /// <param name="internalDate">Date in yyyy-MM-dd format</param>
        /// <param name="size">Display size (8 or 10)</param>
        /// <returns>Formatted display string</returns>
        public static string ToDisplayFormat(string internalDate, int size = 10)
        {
            EnsureInitialized();

            if (string.IsNullOrEmpty(internalDate) || internalDate == NullDateInternal)
            {
                return GetNullDateDisplay(size);
            }

            if (DateTime.TryParseExact(internalDate, InternalFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                return ToDisplayFormat(dt, size);
            }

            return GetNullDateDisplay(size);
        }

        /// <summary>
        /// Converts DateTime to display format.
        /// </summary>
        /// <param name="dateTime">DateTime value</param>
        /// <param name="size">Display size (8 or 10)</param>
        /// <returns>Formatted display string</returns>
        public static string ToDisplayFormat(DateTime dateTime, int size = 10)
        {
            EnsureInitialized();

            if (IsNullDate(dateTime))
            {
                return GetNullDateDisplay(size);
            }

            return dateTime.ToString(GetDisplayFormat(size));
        }

        #endregion

        #region Conversion Methods - Display to Internal

        /// <summary>
        /// Converts display format to internal format (yyyy-MM-dd).
        /// </summary>
        /// <param name="displayDate">Date in display format</param>
        /// <param name="size">Display size (8 or 10)</param>
        /// <returns>Date in yyyy-MM-dd format</returns>
        public static string ToInternalFormat(string displayDate, int size = 10)
        {
            EnsureInitialized();

            if (IsNullDateDisplay(displayDate, size))
            {
                return NullDateInternal;
            }

            string format = GetDisplayFormat(size);
            if (DateTime.TryParseExact(displayDate, format,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                return dt.ToString(InternalFormat);
            }

            return NullDateInternal;
        }

        /// <summary>
        /// Converts display format to DateTime.
        /// </summary>
        /// <param name="displayDate">Date in display format</param>
        /// <param name="size">Display size (8 or 10)</param>
        /// <returns>DateTime value</returns>
        public static DateTime ToDateTime(string displayDate, int size = 10)
        {
            EnsureInitialized();

            if (IsNullDateDisplay(displayDate, size))
            {
                return NullDateTime;
            }

            string format = GetDisplayFormat(size);
            if (DateTime.TryParseExact(displayDate, format,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                return dt;
            }

            return NullDateTime;
        }

        /// <summary>
        /// Converts internal format string to DateTime.
        /// </summary>
        /// <param name="internalDate">Date in yyyy-MM-dd format</param>
        /// <returns>DateTime value</returns>
        public static DateTime InternalToDateTime(string internalDate)
        {
            EnsureInitialized();

            if (string.IsNullOrEmpty(internalDate) || internalDate == NullDateInternal)
            {
                return NullDateTime;
            }

            if (DateTime.TryParseExact(internalDate, InternalFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                return dt;
            }

            return NullDateTime;
        }

        /// <summary>
        /// Converts DateTime to internal format string.
        /// </summary>
        /// <param name="dateTime">DateTime value</param>
        /// <returns>Date in yyyy-MM-dd format</returns>
        public static string DateTimeToInternal(DateTime dateTime)
        {
            EnsureInitialized();

            if (IsNullDate(dateTime))
            {
                return NullDateInternal;
            }

            return dateTime.ToString(InternalFormat);
        }

        #endregion

        #region Database Conversion Methods

        /// <summary>
        /// Converts a database value to internal format string.
        /// Handles DBNull, DateTime, and string values.
        /// </summary>
        /// <param name="dbValue">Value from database</param>
        /// <returns>Date in yyyy-MM-dd format</returns>
        public static string FromDatabase(object dbValue)
        {
            EnsureInitialized();

            if (dbValue == null || dbValue == DBNull.Value)
            {
                return NullDateInternal;
            }

            if (dbValue is DateTime dt)
            {
                return IsNullDate(dt) ? NullDateInternal : dt.ToString(InternalFormat);
            }

            if (dbValue is string strValue)
            {
                // Try parsing as internal format first
                if (DateTime.TryParseExact(strValue, InternalFormat,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsed))
                {
                    return IsNullDate(parsed) ? NullDateInternal : strValue;
                }

                // Try parsing as general date
                if (DateTime.TryParse(strValue, out DateTime generalParsed))
                {
                    return IsNullDate(generalParsed) ? NullDateInternal : generalParsed.ToString(InternalFormat);
                }
            }

            return NullDateInternal;
        }

        /// <summary>
        /// Converts internal format string to database-compatible DateTime.
        /// </summary>
        /// <param name="internalDate">Date in yyyy-MM-dd format</param>
        /// <returns>DateTime for database insertion</returns>
        public static DateTime ToDatabase(string internalDate)
        {
            return InternalToDateTime(internalDate);
        }

        /// <summary>
        /// Formats a date for SQL query (as string literal).
        /// </summary>
        /// <param name="internalDate">Date in yyyy-MM-dd format</param>
        /// <returns>SQL date literal (e.g., '1900-01-01')</returns>
        public static string ToSqlLiteral(string internalDate)
        {
            EnsureInitialized();

            if (string.IsNullOrEmpty(internalDate))
            {
                return $"'{NullDateInternal}'";
            }

            return $"'{internalDate}'";
        }

        /// <summary>
        /// Formats a DateTime for SQL query (as string literal).
        /// </summary>
        /// <param name="dateTime">DateTime value</param>
        /// <returns>SQL date literal (e.g., '1900-01-01')</returns>
        public static string ToSqlLiteral(DateTime dateTime)
        {
            return ToSqlLiteral(DateTimeToInternal(dateTime));
        }

        #endregion

        #region Validation and Helper Methods

        /// <summary>
        /// Checks if a DateTime represents a null/empty date.
        /// </summary>
        public static bool IsNullDate(DateTime dt)
        {
            EnsureInitialized();
            return dt == NullDateTime || dt == DateTime.MinValue || dt.Year < 1900;
        }

        /// <summary>
        /// Checks if a display string represents a null date.
        /// </summary>
        public static bool IsNullDateDisplay(string displayDate, int size = 10)
        {
            EnsureInitialized();

            if (string.IsNullOrEmpty(displayDate))
                return true;

            string nullDisplay = GetNullDateDisplay(size);
            if (displayDate == nullDisplay)
                return true;

            // Check if all digits are zeros
            var digitsOnly = displayDate.Replace(DateSeparator, "").Replace("/", "").Replace("-", "").Replace(".", "");
            return !string.IsNullOrEmpty(digitsOnly) && digitsOnly.All(c => c == '0');
        }

        /// <summary>
        /// Checks if an internal format string represents a null date.
        /// </summary>
        public static bool IsNullDateInternal(string internalDate)
        {
            EnsureInitialized();
            return string.IsNullOrEmpty(internalDate) || internalDate == NullDateInternal;
        }

        /// <summary>
        /// Gets the display format pattern based on size.
        /// </summary>
        public static string GetDisplayFormat(int size)
        {
            EnsureInitialized();
            return size == 8 ? DisplayFormat8 : DisplayFormat10;
        }

        /// <summary>
        /// Gets the null date display string based on size.
        /// </summary>
        public static string GetNullDateDisplay(int size)
        {
            EnsureInitialized();
            return size == 8 ? NullDateDisplay8 : NullDateDisplay10;
        }

        /// <summary>
        /// Validates if a string is a valid internal format date.
        /// </summary>
        public static bool IsValidInternalFormat(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
                return false;

            return DateTime.TryParseExact(dateString, InternalFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

        /// <summary>
        /// Validates if a string is a valid display format date.
        /// </summary>
        public static bool IsValidDisplayFormat(string dateString, int size = 10)
        {
            EnsureInitialized();

            if (IsNullDateDisplay(dateString, size))
                return true;

            string format = GetDisplayFormat(size);
            return DateTime.TryParseExact(dateString, format,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }

        /// <summary>
        /// Gets the position indices that should be skipped (separator positions).
        /// </summary>
        public static int[] GetSeparatorPositions(int size)
        {
            // For dd/MM/yyyy format, separators are at positions 2 and 5
            // For dd/MM/yy format, separators are at positions 2 and 5
            return new[] { 2, 5 };
        }

        /// <summary>
        /// Gets the valid input positions (non-separator positions).
        /// </summary>
        public static int[] GetValidInputPositions(int size)
        {
            if (size == 8)
            {
                return new[] { 0, 1, 3, 4, 6, 7 };
            }
            return new[] { 0, 1, 3, 4, 6, 7, 8, 9 };
        }

        /// <summary>
        /// Tries to parse a date string in various formats and returns internal format.
        /// Useful for handling user input that might be in different formats.
        /// </summary>
        /// <param name="dateString">Date string in any recognized format</param>
        /// <param name="internalDate">Output: date in yyyy-MM-dd format</param>
        /// <returns>True if parsing succeeded</returns>
        public static bool TryParseAny(string dateString, out string internalDate)
        {
            EnsureInitialized();
            internalDate = NullDateInternal;

            if (string.IsNullOrEmpty(dateString))
                return false;

            // Try internal format first
            if (DateTime.TryParseExact(dateString, InternalFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt1))
            {
                internalDate = dt1.ToString(InternalFormat);
                return true;
            }

            // Try display formats
            string[] formats = { DisplayFormat10, DisplayFormat8, "dd/MM/yyyy", "dd/MM/yy", "MM/dd/yyyy", "yyyy-MM-dd" };
            foreach (var format in formats)
            {
                if (DateTime.TryParseExact(dateString, format,
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt2))
                {
                    internalDate = dt2.ToString(InternalFormat);
                    return true;
                }
            }

            // Try general parsing as last resort
            if (DateTime.TryParse(dateString, out DateTime dt3))
            {
                internalDate = dt3.ToString(InternalFormat);
                return true;
            }

            return false;
        }

        private static void EnsureInitialized()
        {
            if (!IsInitialized)
            {
                Initialize();
            }
        }

        #endregion
    }
}
