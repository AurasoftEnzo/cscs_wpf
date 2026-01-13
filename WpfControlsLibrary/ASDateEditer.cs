using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfControlsLibrary
{
    public class ASDateEditer : DatePicker
    {
        string textBeforeChange;
        private string _dateSeparator;
        private string _displayFormat10;
        private string _displayFormat8;
        private string _nullDateDisplay10;
        private string _nullDateDisplay8;
        private string _nullDateInternal;

        public ASDateEditer()
        {
            // Get date configuration from App.config settings
            LoadDateConfiguration();
        }

        private void LoadDateConfiguration()
        {
            try
            {
                _dateSeparator = GetConfigValue("DateSeparator", "/");
                _displayFormat10 = GetConfigValue("DateFormat", "dd/MM/yyyy");
                _displayFormat8 = GetConfigValue("DateFormat8", "dd/MM/yy");
                _nullDateInternal = GetConfigValue("NullDateInternal", "1900-01-01");
                _nullDateDisplay10 = GetConfigValue("NullDateDisplay", $"00{_dateSeparator}00{_dateSeparator}0000");
                _nullDateDisplay8 = GetConfigValue("NullDateDisplay8", $"00{_dateSeparator}00{_dateSeparator}00");
            }
            catch
            {
                // Fallback defaults
                _dateSeparator = "/";
                _displayFormat10 = "dd/MM/yyyy";
                _displayFormat8 = "dd/MM/yy";
                _nullDateInternal = "1900-01-01";
                _nullDateDisplay10 = "00/00/0000";
                _nullDateDisplay8 = "00/00/00";
            }
        }

        private string GetConfigValue(string key, string defaultValue)
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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //this.Padding = new Thickness(0, 0, 0, 0);
            //this.Resources.

            //this.DataContext = this;

            var root = this.Template.FindName("PART_Root", this) as Grid;
            if (root != null)
            {
            }

            var textBox = this.Template.FindName("PART_TextBox", this) as UIElement;
            if (textBox != null)
            {
                var dptb = textBox as DatePickerTextBox;

                dptb.FontWeight = FontWeight;
                dptb.Background = Background == null ? new SolidColorBrush() { Color = Colors.White } : Background;
                dptb.Foreground = Foreground == null ? new SolidColorBrush() { Color = Colors.Black } : Foreground;

                dptb.Margin = new Thickness(-2);
                dptb.Padding = new Thickness(0);

                dptb.IsReadOnly = IsReadOnly;

                dptb.Height = this.Height;
                dptb.Width = this.Width - this.ButtonWidth + 2;
                dptb.HorizontalAlignment = HorizontalAlignment.Left;

                dptb.PreviewTextInput += dptb_PreviewTextInput;

                dptb.PreviewKeyDown += dptb_PreviewKeyDown;
                dptb.SelectionChanged += dptb_SelectionChanged;

                dptb.LostFocus += Dptb_LostFocus;

                dptb.GotFocus += Dptb_GotFocus;
                dptb.PreviewMouseUp += Dptb_PreviewMouseUp;

                dptb.Loaded += Dptb_Loaded;

                dptb.Name = this.Name + "_TextBox";

                //dptb.Name = "asdeTextBox";

                //dptb.Focus();

                //Binding bind = new Binding("Date");
                //bind.Converter = new ASDateEditerConverter2();
                //bind.ConverterParameter = DisplaySize;
                ////bind.StringFormat = "dd..MM..yyyy";
                //bind.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ASDateEditer), 2);
                //bind.Mode = BindingMode.TwoWay;
                //bind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                //dptb.SetBinding(TextProperty, bind);
                //dptb.DataContext = this;

                //var alskdj2 = dptb.GetBindingExpression(TextProperty);
            }

            var button = this.Template.FindName("PART_Button", this) as Button;
            if (button != null)
            {
                //Binding bind = new Binding();
                //bind.ElementName = "asdeTextBox";
                ////bind.Converter = new ASDateEditerConverter2();
                ////bind.ConverterParameter = DisplaySize;
                ////bind.StringFormat = "dd..MM..yyyy";
                ////bind.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(ASDateEditer), 2);
                ////bind.Mode = BindingMode.TwoWay;
                ////bind.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                //button.SetBinding(FocusManager.FocusedElementProperty, bind);
                //button.DataContext = this;

                //button.SetValue(FocusManager.FocusedElementProperty, textBox as DatePickerTextBox);

                button.Name = "asdeButton";

                button.Width = ButtonWidth;
                button.Margin = new Thickness(-2, 0, -4, 0);
                button.Padding = new Thickness(0);

                button.HorizontalContentAlignment = HorizontalAlignment.Center;
                button.VerticalContentAlignment = VerticalAlignment.Center;

                button.HorizontalAlignment = HorizontalAlignment.Right;
                button.VerticalAlignment = VerticalAlignment.Center;

                button.Background = Brushes.Red;

                Style MyButtonStyle = new Style();

                ControlTemplate templateButton = new ControlTemplate(typeof(Button));

                FrameworkElementFactory elemFactory = new FrameworkElementFactory(typeof(Image));
                elemFactory.SetValue(Image.SourceProperty, new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.png"), UriKind.Absolute)));
                templateButton.VisualTree = elemFactory;

                button.Template = templateButton;
            }
            
            var popup = this.Template.FindName("PART_Popup", this) as Popup;
            if (popup != null)
            {
                popup.Name = this.Name + "_Popup";
            }
        }

        //public static readonly DependencyProperty DateProperty = DependencyProperty.Register("Date", typeof(DateTime), typeof(ASDateEditer));
        //public DateTime Date
        //{
        //    get
        //    {
        //        return (DateTime)base.GetValue(DateProperty);
        //    }
        //    set
        //    {
        //        base.SetValue(DateProperty, value);
        //    }
        //}
        
        public static readonly DependencyProperty DisplaySizeProperty = DependencyProperty.Register("DisplaySize", typeof(int), typeof(ASDateEditer));
        public int DisplaySize
        {
            get
            {
                return (int)base.GetValue(DisplaySizeProperty);
            }
            set
            {
                base.SetValue(DisplaySizeProperty, value);
            }
        }
        
        public static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register("FontWeight", typeof(FontWeight), typeof(ASDateEditer));
        public FontWeight FontWeight
        {
            get
            {
                return (FontWeight)base.GetValue(FontWeightProperty);
            }
            set
            {
                base.SetValue(FontWeightProperty, value);
            }
        }
        
        public static readonly DependencyProperty ButtonWidthProperty = DependencyProperty.Register("ButtonWidth", typeof(int), typeof(ASDateEditer));
        public int ButtonWidth
        {
            get
            {
                return (int)base.GetValue(ButtonWidthProperty);
            }
            set
            {
                base.SetValue(ButtonWidthProperty, value);
            }
        }

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(ASDateEditer));
        public Brush Background
        {
            get
            {
                return (Brush)base.GetValue(BackgroundProperty);
            }
            set
            {
                base.SetValue(BackgroundProperty, value);
            }
        }

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground", typeof(Brush), typeof(ASDateEditer));
        public Brush Foreground
        {
            get
            {
                return (Brush)base.GetValue(ForegroundProperty);
            }
            set
            {
                base.SetValue(ForegroundProperty, value);
            }
        }
        
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(ASDateEditer));
        public bool IsReadOnly
        {
            get
            {
                return (bool)base.GetValue(IsReadOnlyProperty);
            }
            set
            {
                base.SetValue(IsReadOnlyProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the date in internal format (yyyy-MM-dd).
        /// This is the format used for CSCS scripts and database operations.
        /// </summary>
        public string InternalDate
        {
            get
            {
                if (SelectedDate.HasValue)
                {
                    var dt = SelectedDate.Value;
                    // Check if it's a null date
                    if (IsNullDate(dt))
                    {
                        return _nullDateInternal;
                    }
                    return dt.ToString("yyyy-MM-dd");
                }
                return _nullDateInternal;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || value == _nullDateInternal)
                {
                    SelectedDate = null;
                    // Set display to null date format
                    var textBox = GetTemplateChild("PART_TextBox") as DatePickerTextBox;
                    if (textBox != null)
                    {
                        textBox.Text = GetNullDateDisplay();
                    }
                }
                else if (DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                {
                    if (IsNullDate(dt))
                    {
                        SelectedDate = null;
                        var textBox = GetTemplateChild("PART_TextBox") as DatePickerTextBox;
                        if (textBox != null)
                        {
                            textBox.Text = GetNullDateDisplay();
                        }
                    }
                    else
                    {
                        SelectedDate = dt;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current display text.
        /// </summary>
        public string DisplayText
        {
            get
            {
                var textBox = GetTemplateChild("PART_TextBox") as DatePickerTextBox;
                return textBox?.Text ?? GetNullDateDisplay();
            }
        }

        /// <summary>
        /// Checks if a DateTime represents a null/empty date (1900-01-01 or earlier).
        /// </summary>
        private bool IsNullDate(DateTime dt)
        {
            return dt.Year <= 1900 && dt.Month == 1 && dt.Day == 1;
        }

        /// <summary>
        /// Gets the null date display string based on DisplaySize.
        /// </summary>
        private string GetNullDateDisplay()
        {
            return DisplaySize == 8 ? _nullDateDisplay8 : _nullDateDisplay10;
        }

        private void Dptb_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            var dptb = (e.Source as DatePickerTextBox);

            if (firstClick)
            {
                firstClick = false;
                e.Handled = true;
                return;
            }
        }

        bool firstClick;

        private void Dptb_GotFocus(object sender, RoutedEventArgs e)
        {
            var dptb = (e.Source as DatePickerTextBox);

            dptb.SelectionChanged -= dptb_SelectionChanged;

            dptb.SelectAll();

            if (dptb.IsMouseOver)
            {
                firstClick = true;
            }

            dptb.SelectionChanged += dptb_SelectionChanged;
        }

        private void Dptb_Loaded(object sender, RoutedEventArgs e)
        {
            var dptb = (e.Source as DatePickerTextBox);

            dptb.SelectionChanged -= dptb_SelectionChanged;

            if (string.IsNullOrEmpty(dptb.Text))
            {
                // Use configured null date display format
                dptb.Text = GetNullDateDisplay();

                dptb.SelectionStart = 0;
                dptb.SelectionLength = 1;
            }
            else
            {
                dptb.SelectAll();
            }

            dptb.SelectionChanged += dptb_SelectionChanged;
        }

        private void dptb_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var dptb = (e.Source as DatePickerTextBox);

            dptb.SelectionChanged -= dptb_SelectionChanged;

            if(dptb.SelectionStart == 0 && dptb.SelectionLength == dptb.Text.Length)
            {
                dptb.SelectionChanged += dptb_SelectionChanged;
                return;
            }

            int[] allowedPositions = { 0, 1, 3, 4, 6, 7, 8, 9};
            if (!allowedPositions.Contains(dptb.SelectionStart))
            {
                    dptb.SelectionStart++;
            }
            
            if ((dptb.SelectionStart == 8 && DisplaySize == 8) || (dptb.SelectionStart == 10 && DisplaySize == 10))
                dptb.SelectionStart--;

            dptb.SelectionLength = 1;
            
            dptb.SelectionChanged += dptb_SelectionChanged;
        }
        private void dptb_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var dptb = (e.Source as DatePickerTextBox);

            if (e.Key == System.Windows.Input.Key.Delete || e.Key == System.Windows.Input.Key.Back)
            {

                if (dptb.SelectionLength == DisplaySize)
                {
                    var date111 = new DateTime(1, 1, 1);
                    dptb.Text = date111.ToString(GetPattern());
                }
                else
                {
                    var index = dptb.SelectionStart;
                    var textArray = dptb.Text.ToArray();
                    textArray[index] = '0';
                    dptb.Text = new string(textArray);
                    dptb.SelectionStart = index;
                }
                e.Handled = true;
            }
            else if (e.Key == Key.OemPlus || e.Key == Key.Add)
            {
                if(DateTime.TryParseExact(dptb.Text, GetPattern(), CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime result))
                {
                    result = result.AddDays(1);
                    dptb.Text = result.ToString(GetPattern());
                }
            }
            else if (e.Key == Key.OemMinus || e.Key == Key.Subtract)
            {
                if (DateTime.TryParseExact(dptb.Text, GetPattern(), CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime result))
                {
                    result = result.AddDays(-1);
                    dptb.Text = result.ToString(GetPattern());
                }
            }

            if (dptb.SelectionLength != 1)
                dptb.SelectionLength = 1;

            textBeforeChange = dptb.Text;

            if (e.Key == System.Windows.Input.Key.Left || e.Key == System.Windows.Input.Key.Up || e.Key == System.Windows.Input.Key.Back)
            {
                e.Handled = true;
                if (dptb.SelectionStart == 0)
                    return;
                if (dptb.SelectionStart == 3 || dptb.SelectionStart == 6)
                    dptb.SelectionStart = dptb.SelectionStart - 2;
                else
                    dptb.SelectionStart--;
            }
        }
        private void dptb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out int x))
            {
                e.Handled = true;
            }
        }

        private void Dptb_LostFocus(object sender, RoutedEventArgs e)
        {
            var dptb = (e.Source as DatePickerTextBox);

            var text = dptb.Text;

            // Check if it's a null date display (all zeros)
            string nullDisplay = GetNullDateDisplay();
            if (text == nullDisplay || IsAllZeros(text))
            {
                dptb.Text = nullDisplay;
                return;
            }

            if (!DateTime.TryParseExact(text, GetPattern(), CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime res))
            {
                var selStart = dptb.SelectionStart;
                dptb.Text = textBeforeChange;
                var newPosition = selStart - 1;
                dptb.SelectionStart = (newPosition < 0) ? 0 : newPosition;
            }
        }

        /// <summary>
        /// Checks if a date display string is all zeros (null date).
        /// </summary>
        private bool IsAllZeros(string displayDate)
        {
            if (string.IsNullOrEmpty(displayDate))
                return true;
            var digitsOnly = displayDate.Replace(_dateSeparator, "").Replace("/", "").Replace("-", "").Replace(".", "");
            return !string.IsNullOrEmpty(digitsOnly) && digitsOnly.All(c => c == '0');
        }

        private string GetPattern()
        {
            // Use configured display format from App.config
            if (DisplaySize == 8)
            {
                return _displayFormat8;
            }
            else if (DisplaySize == 10)
            {
                return _displayFormat10;
            }
            else return _displayFormat10;
        }

    }
}
