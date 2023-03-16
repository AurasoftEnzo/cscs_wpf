﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Windows;

namespace WpfControlsLibrary
{
    public class ASNumericTextBox : TextBox
    {
        const string thousandsDelimiter = ".";
        const string decimalSign = ",";

        public bool SkipTextChangedHandler = false;
        public bool SkipWidgetTextChanged = false;

        public Dictionary<string, List<object>> paramsForKeyTraps = new Dictionary<string, List<object>>();

        #region DependencyProperties

        //public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(int), typeof(ASNumericTextBox));
        //public int Size
        //{
        //    get
        //    {
        //        return (int)base.GetValue(SizeProperty);
        //    }
        //    set
        //    {
        //        base.SetValue(SizeProperty, value);
        //    }
        //}

        //public static readonly DependencyProperty DecProperty = DependencyProperty.Register("Dec", typeof(int), typeof(ASNumericTextBox));
        //public int Dec
        //{
        //    get
        //    {
        //        return (int)base.GetValue(DecProperty);
        //    }
        //    set
        //    {
        //        base.SetValue(DecProperty, value);
        //    }
        //}

        //public static readonly DependencyProperty ThousandsProperty = DependencyProperty.Register("Thousands", typeof(bool), typeof(ASNumericTextBox));
        //public bool Thousands
        //{
        //    get
        //    {
        //        return (bool)base.GetValue(ThousandsProperty);
        //    }
        //    set
        //    {
        //        base.SetValue(ThousandsProperty, value);
        //    }
        //}

        #endregion

        public int Size;
        public int Dec;
        public double MinValue = double.MinValue;
        public double MaxValue = double.MaxValue;

        public bool Thousands;


        string textBeforeChange;

        int SelectionStartBeforeChange = 0;
        int numOfThousandsDelimiters = 0;


        public override void OnApplyTemplate()
        {
            //Size = 600;
            //Dec = 300;

            base.OnApplyTemplate();

            this.PreviewTextInput += this_PreviewTextInput;

            this.PreviewKeyDown += this_PreviewKeyDown;


            this.GotFocus += NumericTextBox_GotFocus;
            this.LostFocus += NumericTextBox_LostFocus;


            this.Loaded += NumericTextBox_Loaded;

            this.TextChanged += NumericTextBox_TextChanged;

            //FormatOnLostFocus();
        }

        private void NumericTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            LoadedEvent();
        }

        public void LoadedEvent()
        {
            this.TextChanged -= NumericTextBox_TextChanged;
            SkipWidgetTextChanged = true;

            this.HorizontalContentAlignment = HorizontalAlignment.Right;

            var tempText = this.Text;

            if (string.IsNullOrEmpty(tempText))
            {
                tempText += 0;
            }

            if (Dec != 0)
            {
                if (!tempText.Contains(decimalSign))
                {
                    tempText += decimalSign;
                    for (int i = 0; i < Dec; i++)
                    {
                        tempText += "0";
                    }
                }
            }

            var splitted = tempText.Split(decimalSign.ToCharArray().First());

            if (Dec > 0)
            {
                if (!tempText.Contains(decimalSign))
                {
                    tempText += decimalSign;
                }
                if (splitted[1].Length != Dec)
                {
                    tempText = Math.Round(double.Parse(tempText.Replace(thousandsDelimiter, "").Replace(decimalSign, ",")), Dec).ToString().Replace(",", decimalSign);

                    if (!tempText.Contains(decimalSign))
                    {
                        tempText += decimalSign;
                    }

                    while (tempText.Split(decimalSign.ToCharArray().First())[1].Length < Dec)
                    {
                        tempText += "0";
                    }
                }
            }


            if (Dec > 0)
            {
                if (splitted[0].Length == 0)
                {
                    tempText = "0" + tempText;
                }
            }

            if (splitted[0].Length == 1 && splitted[0] == "-")
            {
                tempText = "0" + tempText.Substring(1);
            }

            this.Text = tempText;

            if (Thousands)
                formatThousandsDelimiter(this);

            SkipWidgetTextChanged = false;
            this.TextChanged += NumericTextBox_TextChanged;
        }

        private void NumericTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            FormatOnLostFocus();

            this.TextChanged -= NumericTextBox_TextChanged;
            SkipWidgetTextChanged = true;

            if (Thousands)
                formatThousandsDelimiter(this);

            SkipWidgetTextChanged = false;
            this.TextChanged += NumericTextBox_TextChanged;

            //var ntb = (e.Source as ASNumericTextBox);

            //ntb.TextChanged -= NumericTextBox_TextChanged;

            //if (double.TryParse(ntb.Text, out double resDouble))
            //{
            //    if (resDouble > MaxValue || resDouble < MinValue)
            //    {
            //        ntb.Text = "0";
            //    }
            //}

            //if (string.IsNullOrEmpty(ntb.Text))
            //{
            //    ntb.Text += 0;
            //}

            //if (Dec != 0)
            //{
            //    if (!ntb.Text.Contains(decimalSign))
            //    {
            //        ntb.Text += decimalSign;
            //        for (int i = 0; i < Dec; i++)
            //        {
            //            ntb.Text += "0";
            //        }
            //    }
            //}

            //var splitted = ntb.Text.Split(decimalSign.ToCharArray().First());

            //if (Dec > 0)
            //{
            //    ntb.TextChanged -= NumericTextBox_TextChanged;
            //    if (!ntb.Text.Contains(decimalSign))
            //    {
            //        ntb.Text += decimalSign;
            //    }
            //    if (splitted[1].Length != Dec)
            //    {
            //        ntb.Text = Math.Round(double.Parse(ntb.Text.Replace(thousandsDelimiter, "").Replace(decimalSign, ",")), Dec).ToString().Replace(",", decimalSign);

            //        if (!ntb.Text.Contains(decimalSign))
            //        {
            //            ntb.Text += decimalSign;
            //        }

            //        while (ntb.Text.Split(decimalSign.ToCharArray().First())[1].Length < Dec)
            //        {
            //            ntb.Text += "0";
            //        }
            //    }
            //}

            //if(Dec > 0)
            //{
            //    if (splitted[0].Length == 0)
            //    {
            //        ntb.Text = "0" + ntb.Text;
            //    }
            //}

            //if (splitted[0].Length == 1 && splitted[0] == "-")
            //{
            //    ntb.Text = "0" + ntb.Text.Substring(1);
            //}

            //if (Thousands)
            //    formatThousandsDelimiter(ntb);

            //ntb.TextChanged += NumericTextBox_TextChanged;
        }

        public void FormatOnLostFocus()
        {
            //var ntb = (e.Source as ASNumericTextBox);

            this.TextChanged -= NumericTextBox_TextChanged;
            SkipWidgetTextChanged = true;

            if (double.TryParse(this.Text, out double resDouble))
            {
                if (resDouble > MaxValue || resDouble < MinValue)
                {
                    this.Text = "0";
                }
                else
                {
                    this.Text = resDouble.ToString();
                }
            }
            else
            {
                this.Text = "0";
            }

            if (string.IsNullOrEmpty(this.Text))
            {
                this.Text += 0;
            }

            if (Dec != 0)
            {
                if (!this.Text.Contains(decimalSign))
                {
                    SkipTextChangedHandler = true;
                    var append = "";
                    append += decimalSign;
                    for (int i = 0; i < Dec; i++)
                    {
                        SkipTextChangedHandler = true;
                        append += "0";
                    }
                    this.Text += append;
                }
            }

            var splitted = this.Text.Split(decimalSign.ToCharArray().First());

            if (Dec > 0)
            {
                this.TextChanged -= NumericTextBox_TextChanged;
                if (!this.Text.Contains(decimalSign))
                {
                    this.Text += decimalSign;
                }
                if (splitted[1].Length != Dec)
                {
                    this.Text = Math.Round(double.Parse(this.Text.Replace(thousandsDelimiter, "").Replace(decimalSign, ",")), Dec).ToString().Replace(",", decimalSign);

                    if (!this.Text.Contains(decimalSign))
                    {
                        this.Text += decimalSign;
                    }

                    while (this.Text.Split(decimalSign.ToCharArray().First())[1].Length < Dec)
                    {
                        this.Text += "0";
                    }
                }
            }

            if (Dec > 0)
            {
                if (splitted[0].Length == 0)
                {
                    this.Text = "0" + this.Text;
                }
            }

            if (splitted[0].Length == 1 && splitted[0] == "-")
            {
                this.Text = "0" + this.Text.Substring(1);
            }

            SkipWidgetTextChanged = false;
            this.TextChanged += NumericTextBox_TextChanged;
        }

        private void NumericTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var ntb = (e.Source as ASNumericTextBox);

            ntb.TextChanged -= NumericTextBox_TextChanged;
            SkipWidgetTextChanged = true;

            if (string.IsNullOrEmpty(ntb.Text))
            {
                ntb.Text += 0;
            }

            if (Dec != 0)
            {
                if (!ntb.Text.Contains(decimalSign))
                {
                    ntb.Text += decimalSign;
                    for(int i = 0; i < Dec; i++)
                    {
                        ntb.Text += "0";
                    }
                }
            }

            ntb.Text = ntb.Text.Replace(thousandsDelimiter, "");

            SkipWidgetTextChanged = false;
            ntb.TextChanged += NumericTextBox_TextChanged;
        }

        private void NumericTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SkipTextChangedHandler)
            {
                SkipTextChangedHandler = false;
                return;
            }

            var ntb = (e.Source as ASNumericTextBox);

            ntb.TextChanged -= NumericTextBox_TextChanged;
            SkipWidgetTextChanged = true;

            var splitted = ntb.Text.Split(decimalSign.ToCharArray().First());
       
            if (Int32.TryParse(splitted[0], out int result1))
            {
                splitted[0] = result1.ToString();
                ntb.Text = splitted[0] + (splitted.Length > 1 ? decimalSign + splitted[1] : "");
            }

            if (splitted[0].Length > Size - ((Dec > 0 ? 1 : 0) + Dec))
            {
                ntb.Text = textBeforeChange;
                ntb.SelectionStart = SelectionStartBeforeChange;
            }

            if(splitted.Length > 1 && splitted[1].Length > Dec)
            {
                ntb.Text = textBeforeChange;
                ntb.SelectionStart = SelectionStartBeforeChange;
            }

            SkipWidgetTextChanged = false;
            ntb.TextChanged += NumericTextBox_TextChanged;

            if(!ntb.IsKeyboardFocused)
                if (Thousands)
                    formatThousandsDelimiter(ntb);
        }

        private void formatThousandsDelimiter(ASNumericTextBox ntb)
        {
            numOfThousandsDelimiters = 0;

            var charList = ntb.Text.ToList();

            var splitted = ntb.Text.Split(decimalSign.ToCharArray().First());

            bool hasNegativeSign = false;
            if (splitted[0].StartsWith("-"))
            {
                hasNegativeSign = true;
                splitted[0] = splitted[0].Remove(0, 1);
            }

            ntb.TextChanged -= NumericTextBox_TextChanged;
            SkipWidgetTextChanged = true;

            splitted[0] = splitted[0].Replace(thousandsDelimiter, "");

            string result = "";
            if (splitted[0].Length > 3)
            {
                
                var j = -3;
                for (int i = splitted[0].Length - 1; i >= 0;  i--)
                {

                    if(j % 3 == 0)
                    {
                        result = thousandsDelimiter + result;
                        numOfThousandsDelimiters++;
                    }
                    result = splitted[0][i] + result;
                    j++;
                }
                result = result.Substring(0, result.Length - 1);

                ntb.Text = result + (splitted.Length > 1 ? decimalSign + splitted[1] : "");

                if (hasNegativeSign)
                {
                    ntb.Text = "-" + ntb.Text;
                }
            }

            SkipWidgetTextChanged = false;
            ntb.TextChanged += NumericTextBox_TextChanged;
        }

        private void this_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var ntb = (e.Source as ASNumericTextBox);

            textBeforeChange = ntb.Text;
            SelectionStartBeforeChange = ntb.SelectionStart;
        }

        private void this_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var ntb = e.Source as ASNumericTextBox;

            if (ntb.SelectionStart == 0 && e.Text.Equals("-"))
            {
                return;
            }

            if (e.Text.Length + ntb.Text.Length - ntb.SelectionLength > Size)
            {
                e.Handled = true;
            }
            else if (ntb.Text.StartsWith("-") && ntb.SelectionStart == 0 && ntb.SelectionLength == 0)
            {
                e.Handled = true;
            }
            if (ntb.Text.Contains(decimalSign) && e.Text.Equals(decimalSign))
            {
                e.Handled = true;
            }
            else if (!int.TryParse(e.Text, out int x) && !e.Text.Equals(decimalSign))
            {
                e.Handled = true;
            }
            else if(e.Text.Equals(decimalSign) && Dec == 0)
            {
                e.Handled = true;
            }
        }
    }
}
