using DevExpress.Xpf.Core.Native;
using DevExpress.XtraRichEdit.Import.Doc;
using LiveChartsCore.SkiaSharpView.Painting;
using MapControl;
using org.apache.xerces.xni;
using Org.BouncyCastle.Tls;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using Saxon.Api;
using SkiaSharp;
using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
//using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Xsl;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace WpfCSCS
{
    public class NewCSCSFunctions
    {
        CSCS_GUI Gui { get; set; }

        public void Init(CSCS_GUI gui)
        {
            Gui = gui;
            Interpreter interpreter = gui.Interpreter;

            interpreter.RegisterFunction(Constants.SET_MAIN_MENU, new SetMainMenuFunction());

            interpreter.RegisterFunction(Constants.MAP_SETUP, new MapFunction(MapFunctionOption.Setup));
            interpreter.RegisterFunction(Constants.MAP_ADD_POINT, new MapFunction(MapFunctionOption.AddItem));
            
            interpreter.RegisterFunction(Constants.GET_PATH, new GetPathFunction());

            interpreter.RegisterFunction(Constants.ZIP_FILE, new ZipFileFunction());

            interpreter.RegisterFunction(Constants.GET_SQL_RECORD, new GetSQLRecordFunction());

            interpreter.RegisterFunction(Constants.SCHEDULE_FUNCTION, new ScheduleFunctionFunction());
            interpreter.RegisterFunction(Constants.CANCEL_SCHEDULED_FUNCTION, new CancelScheduledFunctionFunction());
            
            interpreter.RegisterFunction(Constants.CLOCK, new ClockFunction());

            interpreter.RegisterFunction(Constants.FTP, new FTPFunction());

            interpreter.RegisterFunction(Constants.GET_COMP_NAME, new GetCompNameFunction());

            interpreter.RegisterFunction(Constants.DAYS_IN_MONTH, new DaysInMonthFunction());


            interpreter.RegisterFunction(Constants.FORMAT_NUM, new FormatNumFunction());

            interpreter.RegisterFunction(Constants.WEB_REQUEST_MPFD, new WebRequestMPFDFunction());
            //interpreter.RegisterFunction(Constants.MATH_RANDOM2, CSCS.Math. ???  GetRandomFunction(false));

            interpreter.RegisterFunction(Constants.SQLNonQuery, new SQLNonQueryFunction()); // override of SQLNonQueryFunction in Functions.SQL


            interpreter.RegisterFunction(Constants.File2Base64, new File2Base64Function());
            interpreter.RegisterFunction(Constants.Base642File, new Base642FileFunction());
            interpreter.RegisterFunction(Constants.SignXml, new SignXmlFunction());
            interpreter.RegisterFunction(Constants.ValidateXsd, new ValidateXsdFunction());
            interpreter.RegisterFunction(Constants.ValidateSch, new ValidateSchFunction());
            interpreter.RegisterFunction(Constants.EscapeQuotesInXml, new EscapeQuotesInXmlFunction());

            //interpreter.RegisterFunction(Constants.FinaTest, new FinaTestFunction());
            //...
            interpreter.RegisterFunction(Constants.XmlToDict, new XmlToDictFunction());
            interpreter.RegisterFunction(Constants.DeserializeJson, new DeserializeJsonFunction());

            interpreter.RegisterFunction(Constants.SignSoapWithCert, new SignSoapWithCertFunction());
            interpreter.RegisterFunction(Constants.CreateSOAP, new CreateSOAPFunction());
            interpreter.RegisterFunction(Constants.UUID, new UUIDFunction());


            interpreter.RegisterFunction("TestEchoFina", new TestEchoFinaFunction());
        }
        public partial class Constants
        {
            public const string SET_MAIN_MENU = "SetMainMenu";

            public const string MAP_SETUP = "MapSetup";
            public const string MAP_ADD_POINT = "MapAddPoint";
            
            public const string GET_PATH = "GetPath";

            public const string ZIP_FILE = "ZipFile";

            public const string GET_SQL_RECORD = "GetSQLRecord";

            public const string SCHEDULE_FUNCTION = "ScheduleFunction";
            public const string CANCEL_SCHEDULED_FUNCTION = "CancelScheduledFunction";

            public const string CLOCK = "Clock";

            public const string FTP = "FTP";

            public const string GET_COMP_NAME = "GetCompName";

            public const string DAYS_IN_MONTH = "DaysInMonth";

            public const string FORMAT_NUM = "FormatNum";

            public const string WEB_REQUEST_MPFD = "WebRequestMPFD";
            //public const string MATH_RANDOM2 = "Math.Random2";
            
            public const string SQLNonQuery = "SQLNonQuery";

            public const string File2Base64 = "File2Base64";
            public const string Base642File = "Base642File";
            public const string SignXml = "SignXml";
            public const string ValidateXsd = "ValidateXsd";
            public const string ValidateSch = "ValidateSch";
            public const string EscapeQuotesInXml = "EscapeQuotesInXml";
            //...
            public const string XmlToDict = "XmlToDict";
            public const string DeserializeJson = "DeserializeJson";

            public const string SignSoapWithCert = "SignSoapWithCert";
            public const string CreateSOAP = "CreateSOAP";
            public const string UUID = "UUID";
        }
    }

    class SetMainMenuFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);

            var gui = CSCS_GUI.GetInstance(script);
            var menuDC = Utils.GetSafeString(args, 0);
            var menuAction = Utils.GetSafeString(args, 1);
            var menu = gui.GetWidget(menuDC) as Menu;
            if (menu == null || !(menu is Menu))
            {
                return Variable.EmptyInstance;
            }

            gui.Control2Window.TryGetValue(menu as FrameworkElement, out Window win);

            menu.Items.Clear();

            var firstMenuItem = new MenuItem();
            firstMenuItem.Name = "firstMenuItem";
            firstMenuItem.DataContext = "firstMenuItem";
            firstMenuItem.Header = "_Programi";

            menu.Items.Add(firstMenuItem);

            List<string> wxmenuLines = System.IO.File.ReadAllLines(App.GetConfiguration("ScriptsPath", "") + "wxmenu.ini").ToList();

            var lastLevels = new Dictionary<int, string>();

            foreach (var line in wxmenuLines)
            {
                if (line.Length < 1)
                {
                    continue;
                }

                if (line.Substring(0, 1) == ";" || line.Substring(0, 1) == @"\")
                {
                    continue;
                }

                var splitted = line.Split(',');
                for (int i = 0; i < splitted.Length; i++)
                {
                    splitted[i] = splitted[i].Trim();
                }

                if (splitted.Length < 4)
                {
                    continue;
                }

                if (splitted[3] == "0")
                {
                    continue;
                }

                

                if (splitted[0] == "0")
                {
                    var newMenuItem = new MenuItem();

                    splitted[2] = splitted[2].Replace("&", "_");

                    newMenuItem.Header = splitted[2];

                    splitted[2] = splitted[2].Replace("/", "").Replace("_", "").Replace("-", "");

                    newMenuItem.Name = splitted[2];
                    newMenuItem.DataContext = splitted[2];
                    lastLevels[0] = splitted[2];

                    menu.Items.Add(newMenuItem);
                    gui.CacheControl(newMenuItem, win);

                    //var clone = CloneMenuItem(newMenuItem);
                    //clone.Header = splitted[1];
                    //clone.Name = splitted[2] + "_firstMenuItem";
                    //clone.DataContext = splitted[2];
                    //FindMenuItemByName(menu, "firstMenuItem").Items.Add(clone);
                    //gui.CacheControl(clone, win);

                    continue;
                }

                if (Convert.ToInt32(splitted[0]) > 0)
                {
                    var lastParentLevel = Convert.ToInt32(splitted[0]) - 1;

                    splitted[2] = splitted[2].Replace("&", "").Replace("-", "").Replace("/", "");//.Replace("_", "");

                    if (splitted[1].Trim() == "-")
                    {
                        FindMenuItemByName(menu, lastLevels[lastParentLevel]).Items.Add(new Separator());
                        //FindMenuItemByName(menu, lastLevels[lastParentLevel] + "_firstMenuItem").Items.Add(new Separator());
                        continue;
                    }
                    else
                    {
                        var newMenuItem = new MenuItem();

                        newMenuItem.Header = splitted[1];
                        newMenuItem.Name = splitted[2];
                        newMenuItem.DataContext = splitted[2];
                        lastLevels[Convert.ToInt32(splitted[0])] = splitted[2];

                        if (splitted[2].ToLower().StartsWith("wk"))
                        {
                            newMenuItem.Click += (sender, eventArgs) =>
                            {
                                gui.Interpreter.Run(menuAction, new Variable(splitted[2]), new Variable(eventArgs.Source.ToString()), null, script);
                            };
                        }
                        FindMenuItemByName(menu, lastLevels[lastParentLevel]).Items.Add(newMenuItem);
                        gui.CacheControl(newMenuItem, win);

                        //var clone = CloneMenuItem(newMenuItem);
                        //clone.Name = splitted[2] + "_firstMenuItem";
                        //clone.DataContext = splitted[2];
                        //FindMenuItemByName(menu, lastLevels[lastParentLevel] + "_firstMenuItem").Items.Add(clone);
                        //gui.CacheControl(clone, win);

                        continue;
                    }
                }

            }

            //foreach (MenuItem menuItem in menu.Items)
            //{
            //    if (menuItem.Name == "firstMenuItem")
            //    {
            //        continue;
            //    }

            //    var newItem = CloneMenuItem(menuItem);
            //    FindMenuItemByName(menu, "firstMenuItem").Items.Add(newItem);
            //}

            foreach (var line in wxmenuLines)
            {
                if (line.Length < 1)
                {
                    continue;
                }

                if (line.Substring(0, 1) == ";" || line.Substring(0, 1) == @"\")
                {
                    continue;
                }

                var splitted = line.Split(',');
                for (int i = 0; i < splitted.Length; i++)
                {
                    splitted[i] = splitted[i].Trim();
                }

                if (splitted.Length < 4)
                {
                    continue;
                }

                if (splitted[3] == "0")
                {
                    continue;
                }



                if (splitted[0] == "0")
                {
                    var newMenuItem = new MenuItem();

                    splitted[2] = splitted[2].Replace("&", "_");

                    newMenuItem.Header = splitted[1];

                    splitted[2] = splitted[2].Replace("/", "").Replace("_", "").Replace("-", "");

                    newMenuItem.Name = splitted[2] + "_firstMenuItem";
                    newMenuItem.DataContext = splitted[2];
                    lastLevels[0] = splitted[2] + "_firstMenuItem";

                    FindMenuItemByName(menu, "firstMenuItem").Items.Add(newMenuItem);
                    gui.CacheControl(newMenuItem, win);

                    continue;
                }

                if (Convert.ToInt32(splitted[0]) > 0)
                {
                    var lastParentLevel = Convert.ToInt32(splitted[0]) - 1;

                    splitted[2] = splitted[2].Replace("&", "").Replace("-", "").Replace("/", "");//.Replace("_", "");

                    if (splitted[1].Trim() == "-")
                    {
                        FindMenuItemByName(menu, lastLevels[lastParentLevel]).Items.Add(new Separator());
                        continue;
                    }
                    else
                    {
                        var newMenuItem = new MenuItem();

                        newMenuItem.Header = splitted[1];
                        newMenuItem.Name = splitted[2] + "_firstMenuItem";
                        newMenuItem.DataContext = splitted[2];
                        lastLevels[Convert.ToInt32(splitted[0])] = splitted[2] + "_firstMenuItem";

                        if (splitted[2].ToLower().StartsWith("wk"))
                        {
                            newMenuItem.Click += (sender, eventArgs) =>
                            {
                                gui.Interpreter.Run(menuAction, new Variable(splitted[2]), new Variable(eventArgs.Source.ToString()), null, script);
                            };
                        }
                        FindMenuItemByName(menu, lastLevels[lastParentLevel]).Items.Add(newMenuItem);
                        gui.CacheControl(newMenuItem, win);

                        continue;
                    }
                }

            }

            return Variable.EmptyInstance;
        }

        public static MenuItem FindMenuItemByName(ItemsControl menu, string name)
        {
            // Check if the menu is null
            if (menu == null) return null;

            // Iterate through the items in the menu
            foreach (var item in menu.Items)
            {
                // Cast the item to a MenuItem
                if (item is MenuItem menuItem)
                {
                    // Check if the menuItem has the specified name
                    if (menuItem.Name == name)
                    {
                        return menuItem;
                    }

                    // Recursively search the sub-items
                    var foundItem = FindMenuItemByName(menuItem, name);
                    if (foundItem != null)
                    {
                        return foundItem;
                    }
                }
            }

            // Return null if the MenuItem is not found
            return null;
        }

        public static MenuItem CloneMenuItem(MenuItem source)
        {
            if (source == null) return null;

            // Create a new MenuItem
            MenuItem newMenuItem = new MenuItem
            {
                Header = source.Header,
                InputGestureText = source.InputGestureText,
                IsCheckable = source.IsCheckable,
                IsChecked = source.IsChecked,
                IsEnabled = source.IsEnabled,
                StaysOpenOnClick = source.StaysOpenOnClick
            };

            try
            {
                // Copy over the event handlers
                newMenuItem.Click += (RoutedEventHandler)source.GetType().GetEvent("Click").EventHandlerType
                    .GetField("Delegate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .GetValue(source);
            }
            catch (Exception)
            {

            }

            // Recursively clone and add sub-items
            foreach (var item in source.Items)
            {
                if (item is MenuItem subMenuItem)
                {
                    newMenuItem.Items.Add(CloneMenuItem(subMenuItem));
                }
                else if(item is Separator)
                {
                    newMenuItem.Items.Add(new Separator());
                }
                else
                {
                    newMenuItem.Items.Add(item);
                }
            }

            return newMenuItem;
        }
    }
    
    enum MapFunctionOption
    {
        Setup,
        AddItem
    }

    class MapFunction : ParserFunction
    {
        MapFunctionOption option;
        public MapFunction(MapFunctionOption _option)
        {
            option = _option;
        }

        class PointItem
        {
            public string Name { get; set; }
            public Location Location { get; set; }
        }

        static List<PointItem> Points { get; } = new List<PointItem>();
        
        protected override Variable Evaluate(ParsingScript script)
        {
            if(option == MapFunctionOption.Setup)
            {
                List<Variable> args = script.GetFunctionArgs();
                Utils.CheckArgs(args.Count, 1, m_name);

                var gui = CSCS_GUI.GetInstance(script);

                var mapName = Utils.GetSafeString(args, 0);

                var map = gui.GetWidget(mapName) as Map;
                if (map == null || !(map is Map))
                {
                    return Variable.EmptyInstance;
                }

                map.MapLayer = new MapTileLayer()
                {
                    //Description = "[OpenStreetMap contributors](http://www.openstreetmap.org/copyright)",
                    TileSource = new TileSource() { UriTemplate = "https://tile.openstreetmap.org/{z}/{x}/{y}.png" }
                };

                var mic = new MapItemsControl();
                var ics = new Style(typeof(MapItem));
                ics.Setters.Add(new Setter(MapItem.LocationProperty, new Binding("Location")));
                ics.Setters.Add(new Setter(MapItem.ContentProperty, new Binding("Name")));

                var ct = new ControlTemplate(typeof(MapItem));

                //FrameworkElementFactory mapItemFactory = new FrameworkElementFactory(typeof(Canvas));
                
                //mapItemFactory.SetValue(Canvas.HeightProperty, 200);
                //mapItemFactory.SetValue(Canvas.WidthProperty, 200);

                FrameworkElementFactory mapItemFactoryChild = new FrameworkElementFactory(typeof(System.Windows.Shapes.Path));

                //mapItemFactoryChild.SetValue(System.Windows.Shapes.Path.WidthProperty, 50);
                //mapItemFactoryChild.SetValue(System.Windows.Shapes.Path.HeightProperty, 50);
                mapItemFactoryChild.SetValue(System.Windows.Shapes.Path.DataProperty, new EllipseGeometry() { RadiusX = 20, RadiusY = 20});
                mapItemFactoryChild.SetValue(System.Windows.Shapes.Path.StrokeProperty, Brushes.DarkGray);
                mapItemFactoryChild.SetValue(System.Windows.Shapes.Path.FillProperty, Brushes.DodgerBlue);
                mapItemFactoryChild.SetValue(System.Windows.Shapes.Path.OpacityProperty, 0.7);

                //mapItemFactory.AppendChild(mapItemFactoryChild);

                ct.VisualTree = mapItemFactoryChild;

                ics.Setters.Add(new Setter(MapItem.TemplateProperty, ct));

                mic.ItemContainerStyle = ics;

                mic.ItemsSource = Points;
                map.Children.Add(mic);

                return Variable.EmptyInstance;
            }
            else if (option == MapFunctionOption.AddItem)
            {
                List<Variable> args = script.GetFunctionArgs();
                Utils.CheckArgs(args.Count, 2, m_name);

                var gui = CSCS_GUI.GetInstance(script);

                var xcoo = Utils.GetSafeDouble(args, 0);
                var ycoo = Utils.GetSafeDouble(args, 1);
                var pointName = Utils.GetSafeString(args, 2);
                if(pointName.Length == 0)
                {
                    pointName = "Point " + Points.Count;
                }

                var map = gui.GetWidget("map") as Map;
                if (map == null || !(map is Map))
                {
                    return Variable.EmptyInstance;
                }

                Points.Add(new PointItem { Name = pointName, Location = new Location(xcoo, ycoo) });

                return Variable.EmptyInstance;
            }

            return Variable.EmptyInstance;
        }
    }

    class GetPathFunction : ParserFunction
    {

        protected override Variable Evaluate(ParsingScript script)
        {

            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 2, m_name);

            var gui = CSCS_GUI.GetInstance(script);

            var defaultPath = Utils.GetSafeString(args, 0);
            var dialogCaption = Utils.GetSafeString(args, 1);

            using (var fbd = new System.Windows.Forms.FolderBrowserDialog())
            {
                fbd.Description = dialogCaption;
                fbd.SelectedPath = defaultPath;
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return new Variable(fbd.SelectedPath);
                }
                else{
                    return new Variable("");
                }
            }
        }
    }
    
    class ZipFileFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 2, m_name);

            var gui = CSCS_GUI.GetInstance(script);

            var filePath = Utils.GetSafeString(args, 0);
            var zipFilePath = Utils.GetSafeString(args, 1);
            var newOrAdd = Utils.GetSafeString(args, 2).ToLower();
            //var password = Utils.GetSafeString(args, 3);

            try
            {
                if (newOrAdd == "new" || newOrAdd == "")
                {
                    using (ZipArchive zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
                    {
                        zip.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
                        return new Variable(true);
                    }
                }
                else
                {
                    //add
                    using (ZipArchive zip = ZipFile.Open(zipFilePath, ZipArchiveMode.Update))
                    {
                        zip.CreateEntryFromFile(filePath, Path.GetFileName(filePath));
                        return new Variable(true);
                    }
                }
            }
            catch (Exception ex)
            {
                return new Variable(false);
            }            
        }
    }
    
    class GetSQLRecordFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 2, m_name);

            var gui = CSCS_GUI.GetInstance(script);

            var sqlColumnNames = Utils.GetSafeVariable(args, 0);
            var sqlRowData = Utils.GetSafeVariable(args, 1);

            for (int i = 0; i < sqlColumnNames.Tuple.Count; i++)
            {
                string defineVarName = sqlColumnNames.Tuple[i].String.ToLower();
                switch (gui.DEFINES[defineVarName].DefType)
                {
                    case "a":
                        gui.DEFINES[defineVarName].String = sqlRowData.Tuple[i].String;
                        break;
                    case "i":
                    case "n":
                    case "b":
                    case "r":
                    case "l":
                        gui.DEFINES[defineVarName].Value = sqlRowData.Tuple[i].Value;
                        break;
                    case "d":
                        var dateFormat = gui.DEFINES[defineVarName].GetDateFormat();
                        gui.DEFINES[defineVarName].DateTime = DateTime.ParseExact(sqlRowData.Tuple[i].String, dateFormat, CultureInfo.InvariantCulture);

                        gui.DEFINES[defineVarName].String = sqlRowData.Tuple[i].String;
                        break;
                    case "t":
                        var timeFormat = gui.DEFINES[defineVarName].GetTimeFormat();
                        gui.DEFINES[defineVarName].DateTime = DateTime.ParseExact(sqlRowData.Tuple[i].String, timeFormat, CultureInfo.InvariantCulture);

                        gui.DEFINES[defineVarName].String = sqlRowData.Tuple[i].String;
                        break;
                    default:
                        break;
                }
                gui.DEFINES[defineVarName].InitVariable(gui.DEFINES[defineVarName], gui);
                gui.OnVariableChange(defineVarName, gui.DEFINES[defineVarName], true);
            }

            return Variable.EmptyInstance;          
        }
    }

    public static class Timers
    {
        public static System.Timers.Timer timer1;
        public static System.Timers.Timer timer2;
    }
    
    class ScheduleFunctionFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 2, m_name);

            var gui = CSCS_GUI.GetInstance(script);

            var timeVar = Utils.GetSafeVariable(args, 0);
            var functionName = Utils.GetSafeString(args, 1);
            
            var alertTime = timeVar.DateTime.TimeOfDay;
            DateTime current = DateTime.Now;
            TimeSpan timeToGo;

            if (alertTime < current.TimeOfDay)
            {
                //timeToGo = alertTime + (TimeSpan.Parse("24:00:00") - current.TimeOfDay);
                timeToGo = alertTime + (new TimeSpan(24, 0, 0) - current.TimeOfDay);
            }
            else
            {
                timeToGo = alertTime - current.TimeOfDay;
            }

            if (Timers.timer1 != null)
            {
                Timers.timer1.Stop();
                Timers.timer1.Dispose();
            }
            if (Timers.timer2 != null)
            {
                Timers.timer2.Stop();
                Timers.timer2.Dispose();
            }

            Timers.timer1 = new System.Timers.Timer(timeToGo.TotalMilliseconds);
            Timers.timer1.Elapsed += (Object source, ElapsedEventArgs e) =>
            {
                gui.Interpreter.Run(functionName);

                Timers.timer2 = new System.Timers.Timer(1000 * 60 * 60 * 24);
                //Timers.timer2 = new System.Timers.Timer(1000 * 60 * 3);
                Timers.timer2.Elapsed += (Object source2, ElapsedEventArgs e2) =>
                {
                    gui.Interpreter.Run(functionName);
                };
                Timers.timer2.AutoReset = true;
                Timers.timer2.Enabled = true;

                Timers.timer1.Stop();
                Timers.timer1.Dispose();
            };
            Timers.timer1.AutoReset = false;
            Timers.timer1.Enabled = true;

            return Variable.EmptyInstance;
        }
    }
    
    class CancelScheduledFunctionFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 0, m_name);

            if(Timers.timer1 != null)
            {
                Timers.timer1.Stop();
                Timers.timer1.Dispose();
            }
            if(Timers.timer2 != null)
            {
                Timers.timer2.Stop();
                Timers.timer2.Dispose();
            }

            return Variable.EmptyInstance;
        }
    }
    
    class ClockFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);

            var gui = CSCS_GUI.GetInstance(script);

            var widgetName = Utils.GetSafeString(args, 0).ToLower();

            var widget = gui.GetWidget(widgetName);

            if (widget is Label label) {
                DispatcherTimer LiveTime = new DispatcherTimer();
                LiveTime.Interval = TimeSpan.FromMilliseconds(200);
                LiveTime.Tick += (object sender, EventArgs e) => {
                    label.Content = DateTime.Now.ToString("HH:mm:ss");
                };
                LiveTime.Start();
            }

            return Variable.EmptyInstance;
        }
    }
    
    class FTPFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 5, m_name);

            var gui = CSCS_GUI.GetInstance(script);

            var protocol = Utils.GetSafeString(args, 0).ToLower();
            var sendOrReceive = Utils.GetSafeString(args, 1).ToLower();
            var serverAddress = Utils.GetSafeString(args, 2);
            var port = Utils.GetSafeInt(args, 3);
            if(port == 0)
            {
                if (protocol == "ftp")
                {
                    port = 21;
                }
                else if(protocol == "sftp")
                {
                    port = 22;
                }
            }

            var username = Utils.GetSafeString(args, 4);
            var password = Utils.GetSafeString(args, 5);

            var serverDirectoryPath = Utils.GetSafeString(args, 6);
            var localDirectoryPath = Utils.GetSafeString(args, 7);

            var filename = Utils.GetSafeString(args, 8);

            if (protocol == "ftp")
            {
                if (sendOrReceive == "s") // "send"
                {
                    using (var client = new WebClient())
                    {
                        client.Credentials = new NetworkCredential(username, password);
                        client.UploadFile("ftp://" + $"{serverAddress}/{serverDirectoryPath}/{filename}".Replace("///", "/").Replace("///", "/"), WebRequestMethods.Ftp.UploadFile, Path.Combine(localDirectoryPath, filename));
                    }
                }
                else if (sendOrReceive == "r") // receive
                {
                    using (var client = new WebClient())
                    {
                        client.Credentials = new NetworkCredential(username, password);
                        client.DownloadFile("ftp://" + $"{serverAddress}/{serverDirectoryPath}/{filename}".Replace("///", "/").Replace("///", "/"), Path.Combine(localDirectoryPath, filename));
                    }
                }
            }
            else if (protocol == "sftp")
            {
                if (sendOrReceive == "s") // "send"
                {
                    using (var client = new SftpClient(serverAddress, username, password))
                    {
                        client.Connect();

                        using (FileStream fs = System.IO.File.OpenRead(Path.Combine(localDirectoryPath, filename)))
                        {
                            client.UploadFile(fs, $"/{serverDirectoryPath}/".Replace("///", "/").Replace("///", "/") + filename);
                        }
                    }
                }
                else if (sendOrReceive == "r") // receive
                {
                    using (var client = new SftpClient(serverAddress, username, password))
                    {
                        client.Connect();

                        using (Stream fs = System.IO.File.Create(Path.Combine(localDirectoryPath, filename)))
                        {
                            client.DownloadFile($"/{serverDirectoryPath}/".Replace("///", "/").Replace("///", "/") + filename, fs);
                        }
                    }
                }
            }

            return Variable.EmptyInstance;
        }
    }
    
        
    class GetCompNameFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 0, m_name);

            return new Variable(System.Environment.MachineName.Trim());
        }
    }
    
    class DaysInMonthFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 2, m_name);

            var month = Utils.GetSafeInt(args, 0);
            var year = Utils.GetSafeInt(args, 1);

            return new Variable(DateTime.DaysInMonth(year, month));
        }
    }
    
    class FormatNumFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 3, m_name);

            var value = Utils.GetSafeDouble(args, 0);
            var decimalPlaces = Utils.GetSafeInt(args, 1);
            var decimalSeparator = Utils.GetSafeString(args, 2);

            string resultString = value.ToString("F" + decimalPlaces.ToString());

            if(decimalSeparator == ",")
            {
                resultString = resultString.Replace(".", ",");
            }
            else if (decimalSeparator == ".")
            {
                resultString = resultString.Replace(",", ".");
            }

            return new Variable(resultString);
        }
    }

    public class WebRequestMPFDFunction : ParserFunction
    {
        static string[] s_allowedMethods = { "GET", "POST", "PUT", "DELETE", "HEAD", "OPTIONS", "TRACE" };

        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 2, m_name);
            string method = args[0].AsString().ToUpper();
            string uri = args[1].AsString();
            Variable bodyArray = Utils.GetSafeVariable(args, 2);
            string trackingString = Utils.GetSafeString(args, 3);
            string onSuccess = Utils.GetSafeString(args, 4);
            string onFailure = Utils.GetSafeString(args, 5, onSuccess);
            string boundaryString = Utils.GetSafeString(args, 6, "----------" + DateTime.Now.Ticks.ToString("x"));
            Variable headers = Utils.GetSafeVariable(args, 7);
            int timeoutMs = Utils.GetSafeInt(args, 8, 10 * 1000);
            bool justFire = Utils.GetSafeInt(args, 9) > 0;

            if (!s_allowedMethods.Contains(method))
            {
                throw new ArgumentException("Unknown web request method: " + method);
            }

            if (justFire)
            {
                Task.Run(() => ProcessWebRequest(uri, method, bodyArray, onSuccess, onFailure, trackingString,
                                                 boundaryString, headers));
                return Variable.EmptyInstance;
            }
            var res = ProcessWebRequest(uri, method, bodyArray, onSuccess, onFailure, trackingString,
                                                 boundaryString, headers);
            return res;
        }

        protected override async Task<Variable> EvaluateAsync(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 2, m_name);
            string method = args[0].AsString().ToUpper();
            string uri = args[1].AsString();
            string load = Utils.GetSafeString(args, 2);
            string tracking = Utils.GetSafeString(args, 3);
            string onSuccess = Utils.GetSafeString(args, 4);
            string onFailure = Utils.GetSafeString(args, 5, onSuccess);
            string contentType = Utils.GetSafeString(args, 6, "application/x-www-form-urlencoded");
            Variable headers = Utils.GetSafeVariable(args, 7);
            int timeoutMs = Utils.GetSafeInt(args, 8, 10 * 1000);
            bool justFire = Utils.GetSafeInt(args, 9) > 0;

            if (!s_allowedMethods.Contains(method))
            {
                throw new ArgumentException("Unknown web request method: " + method);
            }

            var result = await ProcessWebRequestAsync(uri, method, load, onSuccess, onFailure,
                               tracking, contentType, headers, timeoutMs, justFire);
            return result;
        }

        private void WriteFormData(Stream stream, string boundary, string name, string value)
        {
            byte[] header = Encoding.UTF8.GetBytes($"--{boundary}\r\nContent-Disposition: form-data; name=\"{name}\"\r\n\r\n{value}\r\n");
            stream.Write(header, 0, header.Length);
        }

        private void WriteFileData(Stream stream, string boundary, string name, string filePath)
        {
            byte[] header = Encoding.UTF8.GetBytes($"--{boundary}\r\nContent-Disposition: form-data; name=\"{name}\"; filename=\"{Path.GetFileName(filePath)}\"\r\nContent-Type: application/octet-stream\r\n\r\n");
            stream.Write(header, 0, header.Length);
            byte[] fileData = System.IO.File.ReadAllBytes(filePath);
            stream.Write(fileData, 0, fileData.Length);
            stream.Write(Encoding.UTF8.GetBytes("\r\n"), 0, 2);
        }

        Variable ProcessWebRequest(string uri, string method, Variable bodyArray,
                                            string onSuccess, string onFailure,
                                            string tracking, string boundary,
                                            Variable headers)
        {
            Variable res = Variable.EmptyInstance;
            try
            {
                // Create a boundary for the multipart form data
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = method;
                request.ContentType = "multipart/form-data; boundary=" + boundary;

                // Add headers
                if (headers != null && headers.Tuple != null)
                {
                    var keys = headers.GetKeys();
                    foreach (var header in keys)
                    {
                        var headerValue = headers.GetVariable(header).AsString();
                        request.Headers.Add(header, headerValue);
                    }
                }


                // Write form data
                if (bodyArray.Tuple != null && bodyArray.Tuple.Count > 0)
                {
                    using (Stream requestStream = request.GetRequestStream())
                    {
                        foreach (var bodyEntry in bodyArray.Tuple)
                        {
                            if (bodyEntry.Tuple != null && bodyEntry.Tuple.Count == 3)
                            {
                                if (bodyEntry.Tuple[2].String.ToLower() == "text")
                                {
                                    WriteFormData(requestStream, boundary, bodyEntry.Tuple[0].String, bodyEntry.Tuple[1].String);
                                }
                                else if (bodyEntry.Tuple[2].String.ToLower() == "file")
                                {
                                    WriteFileData(requestStream, boundary, bodyEntry.Tuple[0].String, bodyEntry.Tuple[1].String);
                                }
                            }
                        }


                        // End the request
                        byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                        requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                    }
                }

                // Get the response
                HttpWebResponse resp = request.GetResponse() as HttpWebResponse;
                string result;
                using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }
                int responseCode = resp == null ? 0 : (int)resp.StatusCode;
                res = new Variable(result);
                if (!string.IsNullOrWhiteSpace(onSuccess))
                {
                    CustomFunction.Run(InterpreterInstance, onSuccess, new Variable(tracking), new Variable(responseCode), res);
                }

            }
            catch (Exception exc)
            {
                res = new Variable(exc.Message);
                if (!string.IsNullOrWhiteSpace(onFailure))
                {
                    CustomFunction.Run(InterpreterInstance, onFailure, new Variable(tracking),
                                       new Variable(0), res);
                }
            }
            return res;
        }

        async Task<Variable> ProcessWebRequestAsync(string uri, string method, string load,
                                            string onSuccess, string onFailure,
                                            string tracking, string contentType,
                                            Variable headers, int timeout,
                                            bool justFire = false)
        {
            try
            {
                WebRequest request = WebRequest.CreateHttp(uri);
                request.Method = method;
                request.ContentType = contentType;

                if (!string.IsNullOrWhiteSpace(load))
                {
                    var bytes = Encoding.UTF8.GetBytes(load);
                    request.ContentLength = bytes.Length;

                    using (var requestStream = request.GetRequestStream())
                    {
                        requestStream.Write(bytes, 0, bytes.Length);
                    }
                }

                if (headers != null && headers.Tuple != null)
                {
                    var keys = headers.GetKeys();
                    foreach (var header in keys)
                    {
                        var headerValue = headers.GetVariable(header).AsString();
                        request.Headers.Add(header, headerValue);
                    }
                }

                Task<WebResponse> task = request.GetResponseAsync();
                var finishTask = FinishRequest(onSuccess, onFailure,
                                                tracking, task, timeout);
                if (justFire)
                {
                    return Variable.EmptyInstance;
                }
                var result = await finishTask;
                return result;
            }
            catch (Exception exc)
            {
                if (!string.IsNullOrWhiteSpace(onFailure))
                {
                    await CustomFunction.RunAsync(InterpreterInstance, onFailure, new Variable(tracking),
                                                  new Variable(""), new Variable(exc.Message));
                }
                return new Variable(exc.Message);
            }
        }

        async Task<Variable> FinishRequest(string onSuccess, string onFailure,
                                        string tracking, Task<WebResponse> responseTask,
                                        int timeoutMs)
        {
            string result = "";
            string method = onSuccess;
            HttpWebResponse response = null;
            Task timeoutTask = Task.Delay(timeoutMs);

            try
            {
                Task first = await Task.WhenAny(timeoutTask, responseTask);
                if (first == timeoutTask)
                {
                    await timeoutTask;
                    throw new Exception("Timeout waiting for response.");
                }

                response = await responseTask as HttpWebResponse;
                if ((int)response.StatusCode >= 400)
                {
                    throw new Exception(response.StatusDescription);
                }

                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }
            }
            catch (Exception exc)
            {
                result = exc.Message;
                method = onFailure;
            }

            var res = new Variable(result);

            string responseCode = response == null ? "" : response.StatusCode.ToString();
            if (!string.IsNullOrWhiteSpace(method))
            {
                await CustomFunction.RunAsync(InterpreterInstance, method, new Variable(tracking),
                                              new Variable(responseCode), res);
            }
            return res;
        }
    }

    class SQLNonQueryFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);
            CSCS_SQL.CheckConnectionString(script, m_name);

            var queryStatement = Utils.GetSafeString(args, 0);
            var spArgs = Utils.GetSafeVariable(args, 1);
            var sp = SQLQueryFunction.GetParameters(spArgs);
            var timeoutSeconds = Utils.GetSafeInt(args, 2, 60);

            int result = 0;
            using (SqlConnection con = new SqlConnection(CSCS_SQL.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(queryStatement, con))
                {
                    if (sp != null)
                    {
                        cmd.Parameters.AddRange(sp.ToArray());
                    }
                    con.Open();
                    cmd.CommandTimeout = timeoutSeconds;
                    result = cmd.ExecuteNonQuery();
                }
            }
            return new Variable(result);
        }
    }

    class File2Base64Function : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);

            var filePath = Utils.GetSafeString(args, 0);

            if (!System.IO.File.Exists(filePath))
                return Variable.EmptyInstance;

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var base64String = Convert.ToBase64String(fileBytes);

            return new Variable(base64String);
        }
    }
    
    class Base642FileFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 2, m_name);

            var base64String = Utils.GetSafeString(args, 0);
            var filePath = Utils.GetSafeString(args, 1);

            try
            {
                var fileBytes = Convert.FromBase64String(base64String);
                System.IO.File.WriteAllBytes(filePath, fileBytes);

                return new Variable(true);
            }
            catch (Exception ex)
            {
                return new Variable(false);
            }
        }
    }
    
    
    class SignXmlFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 4, m_name);

            var xmlPath = Utils.GetSafeString(args, 0);
            var certificatePath = Utils.GetSafeString(args, 1);
            var certificatePassword = Utils.GetSafeString(args, 2);
            var signedXmlPath = Utils.GetSafeString(args, 3);

            if (!System.IO.File.Exists(xmlPath))
                return new Variable(false);
            if (!System.IO.File.Exists(certificatePath))
                return new Variable(false);

            var xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);

            var cert = new X509Certificate2(certificatePath, certificatePassword);

            return new Variable(SignAndSaveUblInvoice(xmlDoc, cert, signedXmlPath));
        }

        public bool SignAndSaveUblInvoice(XmlDocument xmlDoc, X509Certificate2 cert, string outputFilePath)
        {
            try
            {
                // Perform the signing (as demonstrated earlier)
                SignedXml signedXml = new SignedXml(xmlDoc);
                signedXml.SigningKey = cert.GetRSAPrivateKey();

                Reference reference = new Reference();
                reference.Uri = "";
                reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
                signedXml.AddReference(reference);

                KeyInfo keyInfo = new KeyInfo();
                keyInfo.AddClause(new KeyInfoX509Data(cert));
                signedXml.KeyInfo = keyInfo;

                signedXml.ComputeSignature();
                XmlElement xmlDigitalSignature = signedXml.GetXml();

                XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmlDoc.NameTable);
                nsMgr.AddNamespace("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");

                XmlNode ublExtensionsNode = xmlDoc.SelectSingleNode("//ext:UBLExtensions/ext:UBLExtension/ext:ExtensionContent", nsMgr);
                if (ublExtensionsNode == null)
                {
                    throw new Exception("UBLExtension ExtensionContent node not found for inserting signature.");
                }

                ublExtensionsNode.AppendChild(xmlDoc.ImportNode(xmlDigitalSignature, true));

                // Save the signed XML invoice to the specified file path
                xmlDoc.Save(outputFilePath);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }

    class ValidateXsdFunction : ParserFunction
    {
        private List<string> verificationMessages;
        bool hasErrors;
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 3, m_name);

            var xmlContent = Utils.GetSafeString(args, 0);
            var xsdFilesArray = Utils.GetSafeVariable(args, 1);
            var logFilePath = Utils.GetSafeString(args, 2);

            verificationMessages = new List<string>();
            hasErrors = false;

            ////var invoiceSchema = Directory.GetFiles("C:\\temp\\schemas\\maindoc", )

            //var invoiceSchema = "C:\\temp\\schemas\\maindoc\\UBL-Invoice-2.1.xsd";

            //XmlSchemaSet schemas = new XmlSchemaSet();
            //schemas.Add("urn:oasis:names:specification:ubl:schema:xsd:Invoice-2", invoiceSchema); // You can specify a namespace if your XSD uses one

            //XmlReaderSettings settings = new XmlReaderSettings();
            //settings.Schemas = schemas;
            //settings.ValidationType = ValidationType.Schema;
            //settings.ValidationEventHandler += ValidationHandler;

            //using (StringReader stringReader = new StringReader(xmlContent))
            //using (XmlReader xmlReader = XmlReader.Create(stringReader, settings))
            //{
            //    try
            //    {
            //        while (xmlReader.Read()) { }

            //    }
            //    catch (XmlException ex)
            //    {
            //        verificationMessages.Add($"XML Parsing Error: {ex.Message}");
            //        return new Variable(false);
            //    }
            //    finally
            //    {
            //        System.IO.File.WriteAllText("C:\\temp\\xml_verification_log.txt", string.Join(Environment.NewLine, verificationMessages));
            //    }

            //    if (!hasErrors)
            //    {
            //        return new Variable(true);
            //    }
            //    else
            //    {
            //        return new Variable(false);
            //    }
            //}

            try
            {
                List<string> xsdFiles = new List<string>();
                if (xsdFilesArray.Tuple != null)
                {
                    foreach (var xsdFileVar in xsdFilesArray.Tuple)
                    {
                        xsdFiles.Add(xsdFileVar.AsString());
                    }
                }

                // Create XmlReaderSettings and load schema
                XmlReaderSettings settings = new XmlReaderSettings();
                foreach (string xsdFile in xsdFiles)
                {
                    if (System.IO.File.Exists(xsdFile))
                    {
                        settings.Schemas.Add(null, xsdFile); // !!!
                    }
                    else
                    {
                        verificationMessages.Add("\n.xsd schema file not found: " + xsdFile);
                        hasErrors = true;
                        continue;
                    }
                }
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationEventHandler += new ValidationEventHandler(ValidationHandler);

                // Validate the XML string
                using (StringReader stringReader = new StringReader(xmlContent))
                using (XmlReader reader = XmlReader.Create(stringReader, settings))
                {
                    try
                    {
                        while (reader.Read()) { }
                        if (hasErrors)
                        {
                            System.IO.File.WriteAllText(logFilePath, string.Join("\n\n", verificationMessages));
                        }
                        return new Variable(!hasErrors);
                    }
                    catch (XmlException ex)
                    {
                        System.IO.File.WriteAllText(logFilePath, $"XML parsing error: {ex.Message}");
                        return new Variable(false);
                    }
                }
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(logFilePath, "ex.Message: " + ex.Message);
                return new Variable(false);
            }
        }

        private void ValidationHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Error)
            {
                verificationMessages.Add($"Validation ERROR: {e.Message}");
                hasErrors = true;
            }
            else
            {
                verificationMessages.Add($"Validation Warning: {e.Message}");
            }
        }
    }

    class ValidateSchFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 4, m_name);

            var xmlContent = Utils.GetSafeString(args, 0);
            var schFilesArray = Utils.GetSafeVariable(args, 1);
            var transpilerPath = Utils.GetSafeString(args, 2);
            var logFilePath = Utils.GetSafeString(args, 3);


            List<string> schFiles = new List<string>();
            if (schFilesArray.Tuple != null)
            {
                foreach (var schFileVar in schFilesArray.Tuple)
                {
                    schFiles.Add(schFileVar.AsString());
                }
            }

            try
            {
                StringBuilder allErrors = new StringBuilder();

                var processor = new Processor();
                var compiler = processor.NewXsltCompiler();

                foreach (string schFile in schFiles)
                {
                    if (!System.IO.File.Exists(schFile))
                    {
                        allErrors.AppendLine("\nSchematron not found: " + schFile);
                        continue;
                    }

                    // Step 1: Compile Schematron .sch to .xsl
                    string compiledXslPath = Path.GetTempFileName() + ".xsl";

                    var exec1 = compiler.Compile(new Uri(transpilerPath));
                    var transformer1 = exec1.Load();
                    transformer1.SetInputStream(new FileStream(schFile, FileMode.Open, FileAccess.Read), new Uri(schFile));

                    using (var outputStream = new FileStream(compiledXslPath, FileMode.Create))
                    {
                        var serializer = processor.NewSerializer();
                        serializer.SetOutputStream(outputStream);
                        transformer1.Run(serializer);
                    }

                    // Step 2: Validate XML using compiled XSLT
                    string resultPath = Path.GetTempFileName() + ".xml";

                    var exec2 = compiler.Compile(new Uri(compiledXslPath));
                    var transformer2 = exec2.Load();

                    using (var xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlContent)))
                    {
                        transformer2.SetInputStream(xmlStream, new Uri("file:///temp.xml"));

                        using (var resultStream = new FileStream(resultPath, FileMode.Create))
                        {
                            var serializer = processor.NewSerializer();
                            serializer.SetOutputStream(resultStream);
                            transformer2.Run(serializer);
                        }
                    }

                    // Step 3: Parse SVRL result for errors
                    XmlDocument svrlDoc = new XmlDocument();
                    svrlDoc.Load(resultPath);

                    var ns = new XmlNamespaceManager(svrlDoc.NameTable);
                    ns.AddNamespace("svrl", "http://purl.oclc.org/dsdl/svrl");
                    var failedAsserts = svrlDoc.SelectNodes("//svrl:failed-assert", ns);
                    var successfulReports = svrlDoc.SelectNodes("//svrl:successful-report", ns);

                    if (failedAsserts.Count > 0 || successfulReports.Count > 0)
                    {
                        int errorCounter = 0;

                        allErrors.AppendLine("=== Errors from " + Path.GetFileName(schFile) + " ===");
                        foreach (XmlNode assert in failedAsserts)
                        {
                            allErrors.AppendLine("");
                            allErrors.AppendLine(++errorCounter + ". " + "FAILED: " + assert.SelectSingleNode("svrl:text", ns)?.InnerText);
                            var location = assert.SelectSingleNode("@location", ns);
                            if (location != null) allErrors.AppendLine("Location: " + location.Value);
                        }
                        allErrors.AppendLine("");
                        allErrors.AppendLine("");
                    }

                    // Cleanup
                    if (System.IO.File.Exists(compiledXslPath))
                        System.IO.File.Delete(compiledXslPath);
                    if (System.IO.File.Exists(resultPath))
                        System.IO.File.Delete(resultPath);
                }

                if (allErrors.Length > 0)
                {
                    System.IO.File.WriteAllText(logFilePath, allErrors.ToString());
                }
                return new Variable(allErrors.Length == 0 ? true : false);
            }
            catch (Exception ex)
            {
                System.IO.File.WriteAllText(logFilePath, "ex.Message: " + ex.Message);
                return new Variable(false);
            }
        }
    }
    
    class EscapeQuotesInXmlFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);

            var xmlContent = Utils.GetSafeString(args, 0);
            
            return new Variable(xmlContent.Replace("\"", "\\\""));
        }
    }

    public class XmlToDictFunction : ParserFunction
    {
        // Glavni namespace-i za UBL 2.1 (HR standard)
        private static readonly Dictionary<string, XNamespace> UblNamespaces = new Dictionary<string, XNamespace>
    {
        { "cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2" },
        { "cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2" },
        { "ubl", "urn:oasis:names:specification:ubl:schema:xsd:Invoice-2" },
        { "cr",  "urn:oasis:names:specification:ubl:schema:xsd:CreditNote-2" },
        { "db",  "urn:oasis:names:specification:ubl:schema:xsd:DebitNote-2" }
    };

        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, "XmlToJson");

            string xmlText = args[0].AsString();

            try
            {
                XDocument doc = XDocument.Parse(xmlText);
                XElement root = doc.Root;

                if (root == null)
                    return new Variable("ERROR: Empty XML");

                // Detektiraj root element i postavi odgovarajući namespace
                XNamespace defaultNs = root.GetDefaultNamespace();
                if (defaultNs == XNamespace.None || string.IsNullOrEmpty(defaultNs.NamespaceName))
                {
                    // Ako nema default namespace, probaj pronaći po poznatim UBL urn-ovima
                    string rootName = root.Name.LocalName;
                    if (rootName == "Invoice") defaultNs = UblNamespaces["ubl"];
                    else if (rootName == "CreditNote") defaultNs = UblNamespaces["cr"];
                    else if (rootName == "DebitNote") defaultNs = UblNamespaces["db"];
                    else defaultNs = UblNamespaces["ubl"]; // fallback
                }

                // Dodaj standardne namespace-ove
                var nsMap = new Dictionary<string, XNamespace>(UblNamespaces);
                nsMap[""] = defaultNs; // default namespace

                // Parsiraj sa namespace podrškom
                Variable result = ParseElement(root, nsMap);
                return result;
            }
            catch (Exception ex)
            {
                return new Variable("XML_PARSE_ERROR: " + ex.Message);
            }
        }

        private Variable ParseElement(XElement el, Dictionary<string, XNamespace> nsMap)
        {
            var dict = new Variable(Variable.VarType.ARRAY);

            // Ako element ima samo tekst (npr. <cbc:ID>12345</cbc:ID>)
            string textValue = el.Value.Trim();
            if (!el.HasElements && !string.IsNullOrWhiteSpace(textValue))
            {
                //if (double.TryParse(textValue, NumberStyles.Any, CultureInfo.InvariantCulture, out double num))
                //    return new Variable(num);
                if (textValue == "true" || textValue == "false") // boolean
                    return new Variable(textValue == "true");
                return new Variable(textValue);
            }

            // Dodaj atribute (npr. unitCode, currencyID)
            foreach (var attr in el.Attributes())
            {
                string key = "@" + attr.Name.LocalName;
                dict.SetHashVariable(key, new Variable(attr.Value));
            }

            // Grupe po LocalName (jer više elemenata može imati isti tag, npr. InvoiceLine)
            var grouped = el.Elements()
                            .GroupBy(child => child.Name.LocalName)
                            .ToList();

            foreach (var group in grouped)
            {
                string key = group.Key;

                if (group.Count() == 1)
                {
                    // Jedan element → direktno
                    Variable value = ParseElement(group.First(), nsMap);
                    dict.SetHashVariable(key, value);
                }
                else
                {
                    // Više elemenata (npr. InvoiceLine, TaxCategory...) → array
                    var array = new Variable(Variable.VarType.ARRAY);
                    foreach (var child in group)
                    {
                        array.AddVariable(ParseElement(child, nsMap));
                    }
                    dict.SetHashVariable(key, array);
                }
            }

            return dict;
        }
    }

    class DeserializeJsonFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);

            string jsonString = Utils.GetSafeString(args, 0).Trim();

            // Deserialize the JSON string into a variable
            Variable deserializedVariable = ParseJSON(jsonString.Trim());

            // Return the deserialized variable
            return deserializedVariable;
        }

        private Variable ParseJSON(string json)
        {
            json = json.Trim();

            if (string.IsNullOrEmpty(json))
            {
                return new Variable(Variable.VarType.UNDEFINED);
            }

            // Handle null
            if (json == "null")
            {
                return new Variable(Variable.VarType.UNDEFINED);
            }

            // Handle boolean
            if (json == "true")
            {
                return new Variable(true);
            }
            if (json == "false")
            {
                return new Variable(false);
            }

            // Handle string
            if (json.StartsWith("\"") && json.EndsWith("\""))
            {
                string stringValue = json.Substring(1, json.Length - 2);
                // Unescape JSON string
                stringValue = stringValue.Replace("\\\"", "\"")
                                        .Replace("\\\\", "\\")
                                        .Replace("\\n", "\n")
                                        .Replace("\\r", "\r")
                                        .Replace("\\t", "\t");
                return new Variable(stringValue);
            }

            // Handle number
            // decimal point must be "."
            if (double.TryParse(json /*.Replace(".", ",")*/, NumberStyles.Any, CultureInfo.InvariantCulture, out double numValue))
            {
                return new Variable(numValue);
            }

            // Handle array
            if (json.StartsWith("[") && json.EndsWith("]"))
            {
                return ParseJSONArray(json);
            }

            // Handle object
            if (json.StartsWith("{") && json.EndsWith("}"))
            {
                return ParseJSONObject(json);
            }

            // If we can't parse it, return as string
            return new Variable(json);
        }

        private Variable ParseJSONArray(string json)
        {
            Variable arrayVar = new Variable(Variable.VarType.ARRAY);

            // Remove brackets
            string content = json.Substring(1, json.Length - 2).Trim();

            if (string.IsNullOrEmpty(content))
            {
                return arrayVar;
            }

            List<string> elements = SplitJSONArray(content);

            foreach (string element in elements)
            {
                Variable elementVar = ParseJSON(element.Trim());
                arrayVar.AddVariable(elementVar);
            }

            return arrayVar;
        }

        private Variable ParseJSONObject(string json)
        {
            Variable objectVar = new Variable(Variable.VarType.ARRAY);

            // Remove braces
            string content = json.Substring(1, json.Length - 2).Trim();

            if (string.IsNullOrEmpty(content))
            {
                return objectVar;
            }

            List<string> pairs = SplitJSONObject(content);

            foreach (string pair in pairs)
            {
                int colonIndex = FindColonIndex(pair);
                if (colonIndex > 0)
                {
                    string key = pair.Substring(0, colonIndex).Trim();
                    string value = pair.Substring(colonIndex + 1).Trim();

                    // Remove quotes from key
                    if (key.StartsWith("\"") && key.EndsWith("\""))
                    {
                        key = key.Substring(1, key.Length - 2);
                    }

                    Variable valueVar = ParseJSON(value);
                    objectVar.SetHashVariable(key, valueVar);
                }
            }

            return objectVar;
        }

        private List<string> SplitJSONArray(string content)
        {
            List<string> elements = new List<string>();
            int bracketCount = 0;
            int braceCount = 0;
            int startIndex = 0;
            bool inString = false;
            bool escaped = false;

            for (int i = 0; i < content.Length; i++)
            {
                char c = content[i];

                if (escaped)
                {
                    escaped = false;
                    continue;
                }

                if (c == '\\' && inString)
                {
                    escaped = true;
                    continue;
                }

                if (c == '"')
                {
                    inString = !inString;
                }

                if (!inString)
                {
                    if (c == '[') bracketCount++;
                    else if (c == ']') bracketCount--;
                    else if (c == '{') braceCount++;
                    else if (c == '}') braceCount--;
                    else if (c == ',' && bracketCount == 0 && braceCount == 0)
                    {
                        elements.Add(content.Substring(startIndex, i - startIndex));
                        startIndex = i + 1;
                    }
                }
            }

            if (startIndex < content.Length)
            {
                elements.Add(content.Substring(startIndex));
            }

            return elements;
        }

        private List<string> SplitJSONObject(string content)
        {
            List<string> pairs = new List<string>();
            int bracketCount = 0;
            int braceCount = 0;
            int startIndex = 0;
            bool inString = false;
            bool escaped = false;

            for (int i = 0; i < content.Length; i++)
            {
                char c = content[i];

                if (escaped)
                {
                    escaped = false;
                    continue;
                }

                if (c == '\\' && inString)
                {
                    escaped = true;
                    continue;
                }

                if (c == '"')
                {
                    inString = !inString;
                }

                if (!inString)
                {
                    if (c == '[') bracketCount++;
                    else if (c == ']') bracketCount--;
                    else if (c == '{') braceCount++;
                    else if (c == '}') braceCount--;
                    else if (c == ',' && bracketCount == 0 && braceCount == 0)
                    {
                        pairs.Add(content.Substring(startIndex, i - startIndex));
                        startIndex = i + 1;
                    }
                }
            }

            if (startIndex < content.Length)
            {
                pairs.Add(content.Substring(startIndex));
            }

            return pairs;
        }

        private int FindColonIndex(string pair)
        {
            bool inString = false;
            bool escaped = false;

            for (int i = 0; i < pair.Length; i++)
            {
                char c = pair[i];

                if (escaped)
                {
                    escaped = false;
                    continue;
                }

                if (c == '\\' && inString)
                {
                    escaped = true;
                    continue;
                }

                if (c == '"')
                {
                    inString = !inString;
                }

                if (!inString && c == ':')
                {
                    return i;
                }
            }

            return -1;
        }
    }

    public class SignSoapWithCertFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 3, "SignSoapWithCert");

            string soapBody = Utils.GetSafeString(args, 0);
            string certPath = Utils.GetSafeString(args, 1);
            string certPass = Utils.GetSafeString(args, 2);

            string signedSoap = SignSoap(soapBody, certPath, certPass);
            if (signedSoap.StartsWith("ERROR:"))
                throw new ArgumentException(signedSoap);

            return new Variable(signedSoap);
        }

        protected override async Task<Variable> EvaluateAsync(ParsingScript script)
        {
            List<Variable> args = await script.GetFunctionArgsAsync();
            Utils.CheckArgs(args.Count, 3, "SignSoapWithCert");

            string soapBody = Utils.GetSafeString(args, 0);
            string certPath = Utils.GetSafeString(args, 1);
            string certPass = Utils.GetSafeString(args, 2);

            string signedSoap = await SignSoapAsync(soapBody, certPath, certPass);
            if (signedSoap.StartsWith("ERROR:"))
                throw new ArgumentException(signedSoap);

            return new Variable(signedSoap);
        }

        // Synchronous version (for legacy calls)
        public static string SignSoap(string soap, string certPath, string certPass)
        {
            var task = SignSoapAsync(soap, certPath, certPass);
            return task.GetAwaiter().GetResult();
        }

        // Async version - fully non-blocking
        public static async Task<string> SignSoapAsync(string soap, string certPath, string certPass)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (!System.IO.File.Exists(certPath))
                        return "ERROR: Certificate file not found: " + certPath;

                    // Load certificate with private key
                    X509Certificate2 cert = new X509Certificate2(certPath, certPass,
                        X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);

                    XmlDocument doc = new XmlDocument();
                    doc.PreserveWhitespace = true;
                    doc.LoadXml(soap);

                    // Ensure namespaces
                    const string wsseNs = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
                    const string wsuNs = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

                    // 1) find SOAP Body and assign a wsu:Id so signature can reference it
                    XmlElement body = null;
                    foreach (XmlNode n in doc.DocumentElement.ChildNodes)
                    {
                        if (n.NodeType == XmlNodeType.Element && n.LocalName == "Body")
                        {
                            body = (XmlElement)n;
                            break;
                        }
                    }
                    if (body == null)
                        return "ERROR: SOAP Body element not found.";

                    string bodyId = "id-" + Guid.NewGuid().ToString("N");
                    XmlAttribute idAttr = doc.CreateAttribute("wsu", "Id", wsuNs);
                    idAttr.Value = bodyId;
                    body.Attributes.Append(idAttr);

                    // 2) build BinarySecurityToken (will be placed into wsse:Security header later)
                    string bstId = "X509-" + Guid.NewGuid().ToString("N");
                    XmlElement binaryToken = doc.CreateElement("wsse", "BinarySecurityToken", wsseNs);
                    binaryToken.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
                    binaryToken.SetAttribute("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
                    XmlAttribute bstIdAttr = doc.CreateAttribute("wsu", "Id", wsuNs);
                    bstIdAttr.Value = bstId;
                    binaryToken.Attributes.Append(bstIdAttr);
                    binaryToken.InnerText = Convert.ToBase64String(cert.RawData);

                    // 3) create the SecurityTokenReference element that will point to the BST
                    XmlElement str = doc.CreateElement("wsse", "SecurityTokenReference", wsseNs);
                    XmlElement strRef = doc.CreateElement("wsse", "Reference", wsseNs);
                    strRef.SetAttribute("URI", "#" + bstId);
                    strRef.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
                    str.AppendChild(strRef);

                    // 4) prepare SignedXml to sign the Body using RSA-SHA256
                    SignedXml signedXml = new SignedXml(doc);
                    var rsa = cert.GetRSAPrivateKey();
                    if (rsa == null) return "ERROR: RSA private key not available in certificate.";
                    signedXml.SigningKey = rsa;
                    signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;

                    // Reference the body by wsu:Id and use SHA256
                    Reference reference = new Reference("#" + bodyId);
                    reference.DigestMethod = SignedXml.XmlDsigSHA256Url;
                    // Use exclusive c14n transform for the referenced node
                    reference.AddTransform(new XmlDsigExcC14NTransform());
                    signedXml.AddReference(reference);

                    // Use RSA-SHA256 for signature method
                    signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url;

                    // 5) Build KeyInfo with X509Data and SecurityTokenReference BEFORE computing signature
                    KeyInfo keyInfo = new KeyInfo();
                    keyInfo.AddClause(new KeyInfoX509Data(cert));

                    // import STR element into doc context and wrap as KeyInfoNode
                    XmlElement importedStr = (XmlElement)doc.ImportNode(str, true);
                    KeyInfoNode kin = new KeyInfoNode(importedStr);
                    keyInfo.AddClause(kin);

                    signedXml.KeyInfo = keyInfo;

                    // 6) Compute signature (STR is already present inside KeyInfo and will be serialized into Signature)
                    signedXml.ComputeSignature();
                    XmlElement signatureElement = signedXml.GetXml();

                    // 7) build wsse:Security header and insert BinarySecurityToken and Signature
                    XmlElement security = doc.CreateElement("wsse", "Security", wsseNs);
                    security.SetAttribute("xmlns:wsu", wsuNs);

                    // append BST and Signature (signature already contains KeyInfo with STR)
                    security.AppendChild(binaryToken);
                    security.AppendChild(doc.ImportNode(signatureElement, true));

                    // Insert Security header into SOAP Header (create Header if missing)
                    XmlElement envelope = doc.DocumentElement;
                    XmlElement header = null;
                    foreach (XmlNode n in envelope.ChildNodes)
                    {
                        if (n.NodeType == XmlNodeType.Element && n.LocalName == "Header")
                        {
                            header = (XmlElement)n;
                            break;
                        }
                    }
                    if (header == null)
                    {
                        header = doc.CreateElement(envelope.Prefix, "Header", envelope.NamespaceURI);
                        envelope.PrependChild(header);
                    }
                    header.PrependChild(security);

                    return doc.OuterXml;
                }
                catch (Exception ex)
                {
                    return "ERROR: " + ex.Message + "\n" + ex.StackTrace;
                }
            });
        }
    }
    
    public class CreateSOAPFunction : ParserFunction
    {
        // Use async override for network operations
        protected override async Task<Variable> EvaluateAsync(ParsingScript script)
        {
            List<Variable> args = await script.GetFunctionArgsAsync();
            Utils.CheckArgs(args.Count, 4, m_name);

            string xmlBody = Utils.GetSafeString(args, 0);
            string soapAction = Utils.GetSafeString(args, 1);
            string endpoint = Utils.GetSafeString(args, 2);
            string certIdentifier = Utils.GetSafeString(args, 3); // Can be thumbprint or file path
            string certPassword = Utils.GetSafeString(args, 4); // Password for file-based cert
            int timeoutSeconds = Utils.GetSafeInt(args, 5, 30);

            try
            {
                string soapResponse = await CreateAndSendSignedSOAPAsync(xmlBody, soapAction, endpoint, certIdentifier, certPassword, timeoutSeconds);
                return new Variable(soapResponse);
            }
            catch (Exception ex)
            {
                // Return a detailed error message to the script
                return new Variable("ERROR: " + ex.Message + (ex.InnerException != null ? " | Inner: " + ex.InnerException.Message : ""));
            }
        }

        // Fallback for synchronous calls - not recommended
        protected override Variable Evaluate(ParsingScript script)
        {
            try
            {
                return EvaluateAsync(script).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                return new Variable("ERROR: " + ex.Message);
            }
        }

        public static async Task<string> CreateAndSendSignedSOAPAsync(string xmlBody, string soapAction, string endpoint, string certIdentifier, string certPassword, int timeoutSeconds)
        {
            // 1. Load Certificate
            X509Certificate2 cert = LoadCertificate(certIdentifier, certPassword);
            if (cert == null)
            {
                throw new ArgumentException("Certificate could not be loaded. Identifier: " + certIdentifier);
            }

            // 2. Create SOAP Envelope and add the body
            string soapEnvelope = $@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"">
   <soapenv:Header/>
   <soapenv:Body>
     {xmlBody}
   </soapenv:Body>
</soapenv:Envelope>";

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(soapEnvelope);

            // 3. Find the first element in the Body to be the signing reference
            XmlElement bodyContentElement = doc.DocumentElement.GetElementsByTagName("Body", "http://schemas.xmlsoap.org/soap/envelope/")[0]?.FirstChild as XmlElement;
            if (bodyContentElement == null)
            {
                throw new InvalidOperationException("SOAP Body does not contain a valid XML element to sign.");
            }

            // 4. Add an ID to the element for signing
            string referenceId = "id-" + Guid.NewGuid().ToString("N");
            bodyContentElement.SetAttribute("Id", referenceId);

            // 5. Sign the XML document
            SignXml(doc, cert, referenceId);

            // 6. Send the request using HttpClient
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(cert);
            // For development: bypass server certificate validation. Remove in production.
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            using (var client = new HttpClient(handler))
            {
                client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
                var content = new StringContent(doc.OuterXml, Encoding.UTF8, "text/xml");
                if (!string.IsNullOrEmpty(soapAction))
                {
                    content.Headers.Add("SOAPAction", soapAction);
                }

                HttpResponseMessage response = await client.PostAsync(endpoint, content);
                string responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new WebException($"HTTP Error {(int)response.StatusCode} {response.ReasonPhrase}: {responseString}");
                }

                return responseString;
            }
        }

        private static X509Certificate2 LoadCertificate(string identifier, string password)
        {
            // Try loading from file path first
            if (System.IO.File.Exists(identifier))
            {
                return new X509Certificate2(identifier, password, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            }

            // Otherwise, try loading from certificate store by thumbprint
            using (X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                store.Open(OpenFlags.ReadOnly);
                var certs = store.Certificates.Find(X509FindType.FindByThumbprint, identifier, false);
                return certs.Count > 0 ? certs[0] : null;
            }
        }

        private static void SignXml(XmlDocument doc, X509Certificate2 cert, string referenceId)
        {
            if (cert.PrivateKey == null)
            {
                throw new InvalidOperationException("Certificate does not contain a private key.");
            }

            SignedXml signedXml = new SignedXml(doc);
            signedXml.SigningKey = cert.PrivateKey;
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url; // Explicitly set to SHA256

            Reference reference = new Reference("#" + referenceId);
            reference.AddTransform(new XmlDsigExcC14NTransform());
            reference.DigestMethod = SignedXml.XmlDsigSHA256Url; // Use SHA256 for digest
            signedXml.AddReference(reference);

            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(cert));
            signedXml.KeyInfo = keyInfo;

            signedXml.ComputeSignature();

            // The signature needs to be inserted into the SOAP Header inside a wsse:Security element
            XmlElement signatureElement = signedXml.GetXml();
            
            XmlElement header = doc.DocumentElement.GetElementsByTagName("Header", "http://schemas.xmlsoap.org/soap/envelope/")[0] as XmlElement;
            if (header == null)
            {
                header = doc.CreateElement("soapenv", "Header", "http://schemas.xmlsoap.org/soap/envelope/");
                doc.DocumentElement.PrependChild(header);
            }

            XmlElement securityElement = doc.CreateElement("wsse", "Security", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            securityElement.AppendChild(signatureElement);
            header.AppendChild(securityElement);
        }
    }
    
    public class UUIDFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 5, m_name);

            return new Variable(Guid.NewGuid().ToString());
        }
    }

    public class TestEchoFinaFunction : ParserFunction
    {
        protected override async Task<Variable> EvaluateAsync(ParsingScript script)
        {
            List<Variable> args = await script.GetFunctionArgsAsync();
            Utils.CheckArgs(args.Count, 2, m_name);

            string certPath = Utils.GetSafeString(args, 0);
            string certPass = Utils.GetSafeString(args, 1);
            string url = Utils.GetSafeString(args, 2, "https://prezdigitalneusluge.fina.hr/EchoPKIWebService/services/EchoPKIWebService");
            string message = Utils.GetSafeString(args, 3, "Hello from CSCS!");

            if (!System.IO.File.Exists(certPath))
            {
                return new Variable("ERROR: Certificate file not found: " + certPath);
            }

            try
            {
                // 1. Load client certificate
                var cert = new X509Certificate2(
                    certPath,
                    certPass,
                    X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable
                );

                // 2. Setup HttpClient with handler and certificate
                var handler = new HttpClientHandler();
                handler.ClientCertificates.Add(cert);
                // The following line is for development only to bypass server certificate validation.
                // In production, you should have FINA's root CA installed and remove this line.
                handler.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                using (var client = new HttpClient(handler))
                {
                    // 3. Create SOAP envelope
                    string soapEnvelope =
                        $@"<?xml version=""1.0"" encoding=""utf-8""?>
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:fin=""http://fina.hr/soap"">
   <soapenv:Header/>
   <soapenv:Body>
      <fin:EchoMsg>
         <fin:Message>{message}</fin:Message>
      </fin:EchoMsg>
   </soapenv:Body>
</soapenv:Envelope>";

                    var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");

                    // 4. Add SOAPAction header
                    content.Headers.Add("SOAPAction", "EchoMsg");

                    // 5. Send request asynchronously
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    // 6. Read response
                    string result = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        return new Variable($"ERROR: {(int)response.StatusCode} {response.ReasonPhrase}\n{result}");
                    }

                    // 7. Return the successful response from the server
                    return new Variable(result);
                }
            }
            catch (Exception ex)
            {
                // Return a descriptive error message to the script
                return new Variable("EXCEPTION: " + ex.Message + (ex.InnerException != null ? " | Inner: " + ex.InnerException.Message : ""));
            }
        }

        // The synchronous Evaluate method is not recommended for network calls.
        // It's better to call the async version from your script.
        protected override Variable Evaluate(ParsingScript script)
        {
            //// This will block the UI thread. Use EvaluateAsync instead.
            //return EvaluateAsync(script).GetAwaiter().GetResult();

            ExampleUsage();

            return Variable.EmptyInstance;
        }

        //// Namespaces
        //private const string NS_SOAPENV = "http://schemas.xmlsoap.org/soap/envelope/";
        //private const string NS_WSSE = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
        //private const string NS_WSU = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
        //private const string NS_DS = "http://www.w3.org/2000/09/xmldsig#";
        //private const string NS_ECH = "http://fina.hr/eracun/b2b/pki/Echo/v0.1";
        //private const string NS_IWSC = "http://fina.hr/eracun/b2b/invoicewebservicecomponents/v0.1";

        // Namespaces used
        private const string NS_SOAPENV = "http://schemas.xmlsoap.org/soap/envelope/";
        private const string NS_WSSE = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
        private const string NS_WSU = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
        private const string NS_DS = "http://www.w3.org/2000/09/xmldsig#";
        private const string NS_ECH = "http://fina.hr/eracun/b2b/pki/Echo/v0.1";
        private const string NS_IWSC = "http://fina.hr/eracun/b2b/invoicewebservicecomponents/v0.1";

        // Helper class to allow SignedXml to resolve elements by any "Id" attribute
        public class SignedXmlWithId : SignedXml
        {
            public SignedXmlWithId(XmlDocument doc) : base(doc) { }

            public override XmlElement GetIdElement(XmlDocument doc, string idValue)
            {
                // look for any attribute named "Id" or wsu:Id
                var xpathCandidates = new string[] {
                $"//*[@Id='{idValue}']",
                $"//*[@wsu:Id='{idValue}']",
                $"//*[@id='{idValue}']"
            };

                var nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("wsu", NS_WSU);

                foreach (var xp in xpathCandidates)
                {
                    var node = doc.SelectSingleNode(xp, nsmgr) as XmlElement;
                    if (node != null)
                        return node;
                }

                return base.GetIdElement(doc, idValue);
            }
        }

        /// <summary>
        /// Create signed SOAP envelope and return XML string
        /// </summary>
        public static string CreateSignedEchoSoap(
            X509Certificate2 signingCert,
            string messageId,
            string supplierId,
            string erpId,
            int messageType,
            string echoText)
        {
            if (signingCert == null) throw new ArgumentNullException(nameof(signingCert));
            if (!signingCert.HasPrivateKey) throw new ArgumentException("Certificate must contain private key.");

            var xmlDoc = new XmlDocument { PreserveWhitespace = true };

            // Envelope
            var envelope = xmlDoc.CreateElement("soapenv", "Envelope", NS_SOAPENV);
            xmlDoc.AppendChild(envelope);

            // namespace attributes
            envelope.SetAttribute("xmlns:ech", NS_ECH);
            envelope.SetAttribute("xmlns:iwsc", NS_IWSC);
            envelope.SetAttribute("xmlns:wsse", NS_WSSE);
            envelope.SetAttribute("xmlns:wsu", NS_WSU);
            envelope.SetAttribute("xmlns:ds", NS_DS);

            // Header
            var header = xmlDoc.CreateElement("soapenv", "Header", NS_SOAPENV);
            envelope.AppendChild(header);

            // wsse:Security (mustUnderstand=1)
            var security = xmlDoc.CreateElement("wsse", "Security", NS_WSSE);
            var mustUnderstand = xmlDoc.CreateAttribute("soapenv", "mustUnderstand", NS_SOAPENV);
            mustUnderstand.Value = "1";
            security.Attributes.Append(mustUnderstand);
            header.AppendChild(security);

            // Timestamp (wsu:Id="TS-1")
            var timestamp = xmlDoc.CreateElement("wsu", "Timestamp", NS_WSU);
            var tsId = xmlDoc.CreateAttribute("wsu", "Id", NS_WSU);
            tsId.Value = "TS-1";
            timestamp.Attributes.Append(tsId);

            var created = xmlDoc.CreateElement("wsu", "Created", NS_WSU);
            created.InnerText = XmlConvert.ToString(DateTime.UtcNow, XmlDateTimeSerializationMode.Utc);
            timestamp.AppendChild(created);

            var expires = xmlDoc.CreateElement("wsu", "Expires", NS_WSU);
            expires.InnerText = XmlConvert.ToString(DateTime.UtcNow.AddMinutes(5), XmlDateTimeSerializationMode.Utc);
            timestamp.AppendChild(expires);

            security.AppendChild(timestamp);

            // BinarySecurityToken with base64 certificate, Id = X509-1
            var bst = xmlDoc.CreateElement("wsse", "BinarySecurityToken", NS_WSSE);
            var bstId = xmlDoc.CreateAttribute("wsu", "Id", NS_WSU);
            bstId.Value = "X509-1";
            bst.Attributes.Append(bstId);
            bst.SetAttribute("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
            bst.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
            bst.InnerText = Convert.ToBase64String(signingCert.RawData);
            security.AppendChild(bst);

            // Body with wsu:Id="Body-1"
            var body = xmlDoc.CreateElement("soapenv", "Body", NS_SOAPENV);
            var bodyId = xmlDoc.CreateAttribute("wsu", "Id", NS_WSU);
            bodyId.Value = "Body-1";
            body.Attributes.Append(bodyId);
            envelope.AppendChild(body);

            // EchoMsg payload
            var echoMsg = xmlDoc.CreateElement("ech", "EchoMsg", NS_ECH);
            body.AppendChild(echoMsg);

            // HeaderSupplier
            var headerSupplier = xmlDoc.CreateElement("iwsc", "HeaderSupplier", NS_IWSC);
            echoMsg.AppendChild(headerSupplier);

            var elMessageID = xmlDoc.CreateElement("iwsc", "MessageID", NS_IWSC);
            elMessageID.InnerText = messageId;
            headerSupplier.AppendChild(elMessageID);

            var elSupplierID = xmlDoc.CreateElement("iwsc", "SupplierID", NS_IWSC);
            elSupplierID.InnerText = supplierId;
            headerSupplier.AppendChild(elSupplierID);

            var elERPID = xmlDoc.CreateElement("iwsc", "ERPID", NS_IWSC);
            elERPID.InnerText = erpId;
            headerSupplier.AppendChild(elERPID);

            var elMessageType = xmlDoc.CreateElement("iwsc", "MessageType", NS_IWSC);
            elMessageType.InnerText = messageType.ToString();
            headerSupplier.AppendChild(elMessageType);

            // Data/Echo
            var data = xmlDoc.CreateElement("ech", "Data", NS_ECH);
            var echo = xmlDoc.CreateElement("ech", "Echo", NS_ECH);
            echo.InnerText = echoText;
            data.AppendChild(echo);
            echoMsg.AppendChild(data);

            // SIGNATURE: sign Timestamp (#TS-1) and Body (#Body-1)
            var signedXml = new SignedXmlWithId(xmlDoc)
            {
                SigningKey = signingCert.GetRSAPrivateKey()
            };

            // Force RSA-SHA256
            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url;

            // Reference to Timestamp
            var refTs = new Reference("#TS-1") { DigestMethod = SignedXml.XmlDsigSHA256Url };
            refTs.AddTransform(new XmlDsigExcC14NTransform());
            signedXml.AddReference(refTs);

            // Reference to Body
            var refBody = new Reference("#Body-1") { DigestMethod = SignedXml.XmlDsigSHA256Url };
            refBody.AddTransform(new XmlDsigExcC14NTransform());
            signedXml.AddReference(refBody);

            // Build KeyInfo as SecurityTokenReference to BinarySecurityToken
            var keyInfo = new KeyInfo();
            // Add dummy X509Data so SignedXml will generate KeyInfo (we will replace it)
            keyInfo.AddClause(new KeyInfoX509Data(signingCert));
            signedXml.KeyInfo = keyInfo;

            // Compute signature
            signedXml.ComputeSignature();

            // Get signature XML and replace KeyInfo with wsse:SecurityTokenReference -> wsse:Reference URI="#X509-1"
            var sigXml = signedXml.GetXml();

            // Build new KeyInfo node
            var newKeyInfo = xmlDoc.CreateElement("ds", "KeyInfo", NS_DS);
            var str = xmlDoc.CreateElement("wsse", "SecurityTokenReference", NS_WSSE);
            var refNode = xmlDoc.CreateElement("wsse", "Reference", NS_WSSE);

            var uriAttr = xmlDoc.CreateAttribute("URI");
            uriAttr.Value = "#X509-1";
            refNode.Attributes.Append(uriAttr);

            var vtAttr = xmlDoc.CreateAttribute("ValueType");
            vtAttr.Value = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3";
            refNode.Attributes.Append(vtAttr);

            str.AppendChild(refNode);
            newKeyInfo.AppendChild(str);

            // Replace KeyInfo element
            var oldKeyInfo = sigXml.GetElementsByTagName("KeyInfo", NS_DS)[0];
            if (oldKeyInfo != null)
                sigXml.ReplaceChild(newKeyInfo, oldKeyInfo);

            // Import signature into main document under wsse:Security
            XmlNode importedSig = xmlDoc.ImportNode(sigXml, true);
            security.AppendChild(importedSig);

            // Return the formatted XML
            return xmlDoc.OuterXml;
        }

        /// <summary>
        /// Example showing load p12, create SOAP, and post to service endpoint.
        /// </summary>
        public static void ExampleUsage()
        {
            string p12Path = "D:\\WinX\\ERAC\\certifikati\\p12_aurasoft_demo.p12";      // <-- your file
            string p12Password = "Aurasoft1";               // <-- your p12 password
            //string serviceUrl = "http://prezdigitalneusluge.fina.hr/SendB2BOutgoingInvoicePKIWebService"; // <-- replace with real endpoint
            string serviceUrl = "https://prezdigitalneusluge.fina.hr/SendB2BOutgoingInvoicePKIWebService/services/SendB2BOutgoingInvoicePKIWebService"; // <-- replace with real endpoint

            // Load signing cert (P12)
            var signingCert = new X509Certificate2(
                p12Path,
                p12Password,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable | X509KeyStorageFlags.PersistKeySet
            );

            Console.WriteLine("Has private key: " + signingCert.HasPrivateKey);

            // Build signed SOAP
            string messageId = "MSG-0001";
            string supplierId = "9934:26389058739";
            string erpId = "e-racun-winx";
            int messageType = 9999;
            string echoText = "Hello from CSCS";

            string soapXml = CreateSignedEchoSoap(signingCert, messageId, supplierId, erpId, messageType, echoText);

            Console.WriteLine("SOAP length: " + soapXml.Length);
            // Optionally save to file for inspection:
            System.IO.File.WriteAllText("signedEchoMsg.xml", soapXml, System.Text.Encoding.UTF8);

            // POST with HttpClient
            using (var client = new HttpClient())
            {
                var content = new StringContent(soapXml, Encoding.UTF8, "text/xml");

                // If server expects a SOAPAction header, set it. WSDL had no soapAction => try empty or operation-specific.
                content.Headers.Remove("SOAPAction");
                // content.Headers.Add("SOAPAction", "\"\"");

                var response = client.PostAsync(serviceUrl, content).GetAwaiter().GetResult();
                var respText = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                Console.WriteLine("Status: " + response.StatusCode);
                Console.WriteLine("Response: " + respText);

                // Optionally validate response signature using e-invoice.cer (server's public cert)
            }
        }

        /// <summary>
        /// Build and sign the EchoMsg SOAP envelope using the provided certificate.
        /// </summary>
        //public static string CreateSignedEchoSoap(
        //    X509Certificate2 cert,
        //    string messageId,
        //    string supplierId,
        //    string erpId,
        //    int messageType,
        //    string echoText)
        //{
        //    if (cert == null) throw new ArgumentNullException(nameof(cert));
        //    if (!cert.HasPrivateKey) throw new ArgumentException("Certificate must contain a private key for signing.");

        //    var xmlDoc = new XmlDocument { PreserveWhitespace = true };
        //    // Create SOAP Envelope root
        //    var envelope = xmlDoc.CreateElement("soapenv", "Envelope", NS_SOAPENV);
        //    xmlDoc.AppendChild(envelope);

        //    // Add namespace attributes
        //    envelope.SetAttribute("xmlns:ech", NS_ECH);
        //    envelope.SetAttribute("xmlns:iwsc", NS_IWSC);
        //    envelope.SetAttribute("xmlns:wsse", NS_WSSE);
        //    envelope.SetAttribute("xmlns:wsu", NS_WSU);
        //    envelope.SetAttribute("xmlns:ds", NS_DS);

        //    // Header
        //    var header = xmlDoc.CreateElement("soapenv", "Header", NS_SOAPENV);
        //    envelope.AppendChild(header);

        //    // wsse:Security
        //    var security = xmlDoc.CreateElement("wsse", "Security", NS_WSSE);
        //    var mustUnderstandAttr = xmlDoc.CreateAttribute("soapenv", "mustUnderstand", NS_SOAPENV);
        //    mustUnderstandAttr.Value = "1";
        //    security.Attributes.Append(mustUnderstandAttr);
        //    header.AppendChild(security);

        //    // Timestamp (wsu:Id="TS-1")
        //    var timestamp = xmlDoc.CreateElement("wsu", "Timestamp", NS_WSU);
        //    var tsIdAttr = xmlDoc.CreateAttribute("wsu", "Id", NS_WSU);
        //    tsIdAttr.Value = "TS-1";
        //    timestamp.Attributes.Append(tsIdAttr);

        //    var created = xmlDoc.CreateElement("wsu", "Created", NS_WSU);
        //    created.InnerText = XmlConvert.ToString(DateTime.UtcNow, XmlDateTimeSerializationMode.Utc);
        //    timestamp.AppendChild(created);

        //    var expires = xmlDoc.CreateElement("wsu", "Expires", NS_WSU);
        //    expires.InnerText = XmlConvert.ToString(DateTime.UtcNow.AddMinutes(5), XmlDateTimeSerializationMode.Utc);
        //    timestamp.AppendChild(expires);

        //    security.AppendChild(timestamp);

        //    // BinarySecurityToken (X.509)
        //    var bst = xmlDoc.CreateElement("wsse", "BinarySecurityToken", NS_WSSE);
        //    var bstId = xmlDoc.CreateAttribute("wsu", "Id", NS_WSU);
        //    bstId.Value = "X509-1";
        //    bst.Attributes.Append(bstId);

        //    bst.SetAttribute("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
        //    bst.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");

        //    bst.InnerText = Convert.ToBase64String(cert.RawData);
        //    security.AppendChild(bst);

        //    // Body (soapenv:Body wsu:Id="Body-1")
        //    var body = xmlDoc.CreateElement("soapenv", "Body", NS_SOAPENV);
        //    var bodyIdAttr = xmlDoc.CreateAttribute("wsu", "Id", NS_WSU);
        //    bodyIdAttr.Value = "Body-1";
        //    body.Attributes.Append(bodyIdAttr);
        //    envelope.AppendChild(body);

        //    // Build EchoMsg content inside Body
        //    var echoMsg = xmlDoc.CreateElement("ech", "EchoMsg", NS_ECH);
        //    body.AppendChild(echoMsg);

        //    // HeaderSupplier (iwsc)
        //    var headerSupplier = xmlDoc.CreateElement("iwsc", "HeaderSupplier", NS_IWSC);
        //    echoMsg.AppendChild(headerSupplier);

        //    var elMessageID = xmlDoc.CreateElement("iwsc", "MessageID", NS_IWSC);
        //    elMessageID.InnerText = messageId;
        //    headerSupplier.AppendChild(elMessageID);

        //    var elSupplierID = xmlDoc.CreateElement("iwsc", "SupplierID", NS_IWSC);
        //    elSupplierID.InnerText = supplierId;
        //    headerSupplier.AppendChild(elSupplierID);

        //    var elERPID = xmlDoc.CreateElement("iwsc", "ERPID", NS_IWSC);
        //    elERPID.InnerText = erpId;
        //    headerSupplier.AppendChild(elERPID);

        //    var elMessageType = xmlDoc.CreateElement("iwsc", "MessageType", NS_IWSC);
        //    elMessageType.InnerText = messageType.ToString();
        //    headerSupplier.AppendChild(elMessageType);

        //    // Data/Echo
        //    var data = xmlDoc.CreateElement("ech", "Data", NS_ECH);
        //    var echo = xmlDoc.CreateElement("ech", "Echo", NS_ECH);
        //    echo.InnerText = echoText;
        //    data.AppendChild(echo);
        //    echoMsg.AppendChild(data);

        //    // --- SIGNATURE SECTION ---
        //    // We'll sign Body (Body-1) and Timestamp (TS-1)
        //    // Create SignedXml with the private key
        //    var signedXml = new SignedXml(xmlDoc)
        //    {
        //        // Set signing key (RSA)
        //        SigningKey = cert.GetRSAPrivateKey()
        //    };

        //    // Use RSA-SHA256
        //    signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url;

        //    // Reference: Timestamp (URI="#TS-1")
        //    var refTimestamp = new Reference("#TS-1")
        //    {
        //        DigestMethod = SignedXml.XmlDsigSHA256Url
        //    };
        //    // Exclusive C14N transform
        //    var excTransform1 = new XmlDsigExcC14NTransform();
        //    refTimestamp.AddTransform(excTransform1);
        //    signedXml.AddReference(refTimestamp);

        //    // Reference: Body (URI="#Body-1")
        //    var refBody = new Reference("#Body-1")
        //    {
        //        DigestMethod = SignedXml.XmlDsigSHA256Url
        //    };
        //    var excTransform2 = new XmlDsigExcC14NTransform();
        //    refBody.AddTransform(excTransform2);
        //    signedXml.AddReference(refBody);

        //    // Create KeyInfo but we will replace its content to reference the BinarySecurityToken
        //    var keyInfo = new KeyInfo();
        //    // Add a dummy X509Data (optional) - not used, we will craft wsse:SecurityTokenReference later
        //    var x509Data = new KeyInfoX509Data(cert);
        //    keyInfo.AddClause(x509Data);
        //    signedXml.KeyInfo = keyInfo;

        //    // Compute signature
        //    signedXml.ComputeSignature();

        //    // Get the XML representation of the signature and modify KeyInfo to include wsse:SecurityTokenReference
        //    var signatureXml = signedXml.GetXml();

        //    // Replace KeyInfo content with:
        //    // <ds:KeyInfo>
        //    //   <wsse:SecurityTokenReference>
        //    //     <wsse:Reference URI="#X509-1" ValueType="...#X509v3"/>
        //    //   </wsse:SecurityTokenReference>
        //    // </ds:KeyInfo>
        //    // Build the new KeyInfo node
        //    var newKeyInfo = xmlDoc.CreateElement("ds", "KeyInfo", NS_DS);
        //    var str = xmlDoc.CreateElement("wsse", "SecurityTokenReference", NS_WSSE);
        //    var refNode = xmlDoc.CreateElement("wsse", "Reference", NS_WSSE);
        //    var uriAttr = xmlDoc.CreateAttribute("URI");
        //    uriAttr.Value = "#X509-1";
        //    refNode.Attributes.Append(uriAttr);
        //    var valTypeAttr = xmlDoc.CreateAttribute("ValueType");
        //    valTypeAttr.Value = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3";
        //    refNode.Attributes.Append(valTypeAttr);

        //    str.AppendChild(refNode);
        //    newKeyInfo.AppendChild(str);

        //    // Replace old KeyInfo
        //    var oldKeyInfo = signatureXml.GetElementsByTagName("KeyInfo", NS_DS)[0];
        //    if (oldKeyInfo != null)
        //    {
        //        signatureXml.ReplaceChild(newKeyInfo, oldKeyInfo);
        //    }

        //    // Append the signature element inside wsse:Security (as a child)
        //    XmlNode importedSig = xmlDoc.ImportNode(signatureXml, true);
        //    security.AppendChild(importedSig);

        //    // DONE - Return the XML string
        //    return xmlDoc.OuterXml;
        //}

        //// Helper to load certificate from PFX (example)
        //public static X509Certificate2 LoadCertificateFromPfx(string pfxPath, string password)
        //{
        //    return new X509Certificate2(pfxPath, password, X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);
        //}

        //// Example usage
        //public static void Example()
        //{
        //    // === Replace these with your real values ===
        //    string certPath = "D:\\WinX\\ERAC\\certifikati\\p12_aurasoft_demo.p12";
        //    string certPassword = "Aurasoft1";
        //    string messageId = "MSG-0009";
        //    string supplierId = "9934:26389058739";
        //    string erpId = "e-racun-winx";
        //    int messageType = 9999;
        //    string echoText = "Hello world!";

        //    var cert = LoadCertificateFromPfx(certPath, certPassword);

        //    // Alternatively, load from Windows Certificate Store (commented):
        //    /*
        //    var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        //    store.Open(OpenFlags.ReadOnly);
        //    var certs = store.Certificates.Find(X509FindType.FindByThumbprint, "THUMBPRINT_HERE", false);
        //    if (certs.Count == 0) throw new Exception("Certificate not found in store.");
        //    var cert = certs[0];
        //    */

        //    string signedSoap = CreateSignedEchoSoap(cert, messageId, supplierId, erpId, messageType, echoText);
        //    Console.WriteLine(signedSoap);
        //}
    }
}
