using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfControlsLibrary
{
    /// <summary>
    /// Interaction logic for ASHorizontalBar.xaml
    /// </summary>
    public partial class ASHorizontalBar : UserControl
    {
        public static readonly DependencyProperty BarWidthProperty = DependencyProperty.Register("BarWidth", typeof(int), typeof(ASHorizontalBar));
        public int BarWidth
        {
            get
            {
                return (int)base.GetValue(BarWidthProperty);
            }
            set
            {
                base.SetValue(BarWidthProperty, value);

                if(value < 0)
                {
                    BarFill.HorizontalAlignment = HorizontalAlignment.Right;
                }
                else
                {
                    BarFill.HorizontalAlignment = HorizontalAlignment.Left;
                }

                newBarFillWidth = Math.Abs(Width * (double)value / 100.0);
                AnimateBarFillWidth();

            }
        }
        
        public static readonly DependencyProperty FontSizeProperty = DependencyProperty.Register("FontSize", typeof(int), typeof(ASHorizontalBar));
        public int FontSize
        {
            get
            {
                return (int)base.GetValue(FontSizeProperty);
            }
            set
            {
                base.SetValue(FontSizeProperty, value);
                InsideText.FontSize = value;
            }
        }
        
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ASHorizontalBar));
        public string Text
        {
            get
            {
                return (string)base.GetValue(TextProperty);
            }
            set
            {
                base.SetValue(TextProperty, value);
                InsideText.Text = value;
            }
        }
        
        public static readonly DependencyProperty BarColorProperty = DependencyProperty.Register("BarColor", typeof(Brush), typeof(ASHorizontalBar));
        public Brush BarColor
        {
            get
            {
                return (Brush)base.GetValue(BarColorProperty);
            }
            set
            {

                var startColor = (value as SolidColorBrush).Color;
                startColor.A = 0;
                
                //Colors.Transparent
                LinearGradientBrush lgb = new LinearGradientBrush(startColor, (value as SolidColorBrush).Color, 0);

                base.SetValue(BarColorProperty, lgb);
                BarFill.Background = lgb;
            }
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(float), typeof(ASHorizontalBar));
        public float CornerRadius
        {
            get
            {
                return (float)base.GetValue(CornerRadiusProperty);
            }
            set
            {
                base.SetValue(CornerRadiusProperty, value);
            }
        }

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("Background", typeof(Brush), typeof(ASHorizontalBar));
        public Brush Background
        {
            get
            {
                return (Brush)base.GetValue(BackgroundProperty);
            }
            set
            {
                base.SetValue(BackgroundProperty, value);
                BarFill.Background = value;
            }
        }

        public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register("Foreground", typeof(Brush), typeof(ASHorizontalBar));
        public Brush Foreground
        {
            get
            {
                return (Brush)base.GetValue(ForegroundProperty);
            }
            set
            {
                base.SetValue(ForegroundProperty, value);
                //BarFill.Background = value;
            }
        }

        public static readonly DependencyProperty AnimationDurationProperty = DependencyProperty.Register("AnimationDuration", typeof(double), typeof(ASHorizontalBar), new PropertyMetadata(1.3));
        public double AnimationDuration
        {
            get
            {
                return (double)base.GetValue(AnimationDurationProperty);
            }
            set
            {
                base.SetValue(AnimationDurationProperty, value);
            }
        }

        public ASHorizontalBar()
        {
            InitializeComponent();
        }

        bool loaded = false;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!loaded)
            {
                InsideText.IsReadOnly = true;
                InsideText.IsTabStop = false;
                InsideText.IsHitTestVisible = false;

                BarFill.Width = Math.Abs(Width * BarWidth / 100);
                InsideText.Text = Text;
                if(FontSize != 0)
                    InsideText.FontSize = FontSize;

                BarFill.CornerRadius = new CornerRadius(CornerRadius, 0, 0, CornerRadius);

                widgetBackground.CornerRadius = new CornerRadius(CornerRadius);

                var tbBorder = InsideText.Template.FindName("border", InsideText);
                ((Border)tbBorder).CornerRadius = new CornerRadius(CornerRadius);

                widgetBackground.Background = Background;

                InsideText.Foreground = Foreground == null ? new SolidColorBrush() { Color = Colors.Black } : Foreground;

                loaded = true;
            }
        }


        double newBarFillWidth = 0;
        DoubleAnimation widthAnimation;
        private void AnimateBarFillWidth()
        {
            // Create a DoubleAnimation to animate the width of a button
            widthAnimation = new DoubleAnimation
            {
                From = 0,
                To = Math.Abs(newBarFillWidth),
                Duration = new Duration(TimeSpan.FromSeconds(AnimationDuration))
            };

            // Create a Storyboard to contain the animation
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(widthAnimation);

            // Set the target property and target name for the animation
            Storyboard.SetTargetName(widthAnimation, this.BarFill.Name);
            Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(Border.WidthProperty));

            ////// Store the storyboard in the resources for later use
            //this.Resources.Add("MyStoryboard", storyboard);

            ////// Retrieve the storyboard from the resources and begin the animation
            //storyboard = this.Resources["MyStoryboard"] as Storyboard;
            storyboard.Begin(this);
        }
    }
}
