using DevExpress.Xpf.Core.Native;
using DevExpress.XtraReports.Templates;
using DevExpress.XtraRichEdit.Import.Doc;
using LiveChartsCore.SkiaSharpView.Painting;
using MapControl;
//using NixxonERacun.Signing;
using OfficeOpenXml.Drawing.Chart;
using Org.BouncyCastle.Tls;
using Renci.SshNet;
using Renci.SshNet.Sftp;
//using Saxon.Api;
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
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
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
using WpfCSCS.ServiceReference1_fina_wsdl;
using WpfCSCS.ServiceReference2_B2BFinaInvoiceWebService;
using static System.Net.WebRequestMethods;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using static WpfCSCS.TEST1Function;

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
            interpreter.RegisterFunction(Constants.ValidateXsd, new ValidateXsdFunction());
            //interpreter.RegisterFunction(Constants.ValidateSch, new ValidateSchFunction());
            interpreter.RegisterFunction(Constants.EscapeQuotesInXml, new EscapeQuotesInXmlFunction());

            //interpreter.RegisterFunction(Constants.FinaTest, new FinaTestFunction());
            //...
            interpreter.RegisterFunction(Constants.XmlToDict, new XmlToDictFunction());
            interpreter.RegisterFunction(Constants.DeserializeJson, new DeserializeJsonFunction());

            interpreter.RegisterFunction(Constants.SignSoapWithCert, new SignSoapWithCertFunction());
            interpreter.RegisterFunction(Constants.CreateSOAP, new CreateSOAPFunction());
            interpreter.RegisterFunction(Constants.UUID, new UUIDFunction());


            interpreter.RegisterFunction(Constants.TestEchoFina, new TestEchoFinaFunction());
            interpreter.RegisterFunction(Constants.CreateSOAP2, new CreateSOAP2Function());

            interpreter.RegisterFunction(Constants.TEST1, new TEST1Function());

            interpreter.RegisterFunction(Constants.SignXml, new SignXmlFunction());
            //interpreter.RegisterFunction("NixxonSignXml", new NixxonSignXmlFunction());

            interpreter.RegisterFunction("ReplaceInJSON", new ReplaceInJSONFunction());
            interpreter.RegisterFunction("IsDouble", new IsDoubleFunction());
            interpreter.RegisterFunction("FindColonIndex", new FindColonIndexFunction());
            
            
            
            
            
            interpreter.RegisterFunction("ReadAllText", new ReadAllTextFunction());

            interpreter.RegisterFunction("TestXmlDict", new TestXmlDictFunction());
            interpreter.RegisterFunction("Test99", new Test99Function());



            interpreter.RegisterFunction(Constants.CreateAndSendSOAPMsg, new CreateAndSendSOAPMsgFunction());



            //funkcije iz wsdl-a
            interpreter.RegisterFunction("FINA_ECHO", new FINA_ECHOFunction());
            interpreter.RegisterFunction("FINA_SEND_OUTGOING_INVOICE", new FINA_SEND_OUTGOING_INVOICEFunction());
            interpreter.RegisterFunction("FINA_GET_OUTGOING_INVOICE_STATUS", new FINA_GET_OUTGOING_INVOICE_STATUSFunction());
            interpreter.RegisterFunction("FINA_SEND_OUTGOING_INVOICE_REPORTING", new FINA_SEND_OUTGOING_INVOICE_REPORTINGFunction());

            interpreter.RegisterFunction("FINA_ECHO_BUYER", new FINA_ECHO_BUYERFunction());
            interpreter.RegisterFunction("FINA_GET_INCOMING_INVOICE_LIST", new FINA_GET_INCOMING_INVOICE_LISTFunction());
            interpreter.RegisterFunction("FINA_GET_INCOMING_INVOICE", new FINA_GET_INCOMING_INVOICEFunction());
            interpreter.RegisterFunction("FINA_CHANGE_INCOMING_INVOICE_STATUS", new FINA_CHANGE_INCOMING_INVOICE_STATUSFunction());
            
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
            public const string ValidateXsd = "ValidateXsd";
            public const string ValidateSch = "ValidateSch";
            public const string EscapeQuotesInXml = "EscapeQuotesInXml";
            //...
            public const string XmlToDict = "XmlToDict";
            public const string DeserializeJson = "DeserializeJson";

            public const string SignSoapWithCert = "SignSoapWithCert";
            public const string CreateSOAP = "CreateSOAP";
            public const string UUID = "UUID";

            public const string TestEchoFina = "TestEchoFina";
            public const string CreateSOAP2 = "CreateSOAP2";

            public const string TEST1 = "TEST1";

            public const string SignXml = "SignXml";

            public const string CreateAndSendSOAPMsg = "CreateAndSendSOAPMsg";
        }
    }

    #region ALL
    
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
                else if (item is Separator)
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
            if (option == MapFunctionOption.Setup)
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
                mapItemFactoryChild.SetValue(System.Windows.Shapes.Path.DataProperty, new EllipseGeometry() { RadiusX = 20, RadiusY = 20 });
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
                if (pointName.Length == 0)
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
                else
                {
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

            if (widget is Label label)
            {
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
            if (port == 0)
            {
                if (protocol == "ftp")
                {
                    port = 21;
                }
                else if (protocol == "sftp")
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

            if (decimalSeparator == ",")
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

                var settings = new XmlReaderSettings
                {
                    ValidationType = ValidationType.Schema,
                    DtdProcessing = DtdProcessing.Parse,  // allow DTDs in XSDs
                    XmlResolver = new XmlUrlResolver()    // required for imports
                };

                var schemaSet = new XmlSchemaSet
                {
                    XmlResolver = new XmlUrlResolver()
                };

                foreach (string xsdFile in xsdFiles)
                {
                    if (System.IO.File.Exists(xsdFile))
                    {
                        //settings.Schemas.Add(null, xsdFile); // !!!
                        schemaSet.Add(null, xsdFile);
                    }
                    else
                    {
                        verificationMessages.Add("\n.xsd schema file not found: " + xsdFile);
                        hasErrors = true;
                        continue;
                    }
                }

                settings.Schemas = schemaSet;

                // Create XmlReaderSettings and load schema
                //XmlReaderSettings settings = new XmlReaderSettings();

                //settings.Schemas.XmlResolver = new XmlUrlResolver();
                //settings.DtdProcessing = DtdProcessing.Parse;
                //settings.ValidationType = ValidationType.Schema;
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

    //class ValidateSchFunction : ParserFunction
    //{
    //    protected override Variable Evaluate(ParsingScript script)
    //    {
    //        List<Variable> args = script.GetFunctionArgs();
    //        Utils.CheckArgs(args.Count, 4, m_name);

    //        var xmlContent = Utils.GetSafeString(args, 0);
    //        var schFilesArray = Utils.GetSafeVariable(args, 1);
    //        var transpilerPath = Utils.GetSafeString(args, 2);
    //        var logFilePath = Utils.GetSafeString(args, 3);


    //        List<string> schFiles = new List<string>();
    //        if (schFilesArray.Tuple != null)
    //        {
    //            foreach (var schFileVar in schFilesArray.Tuple)
    //            {
    //                schFiles.Add(schFileVar.AsString());
    //            }
    //        }

    //        try
    //        {
    //            StringBuilder allErrors = new StringBuilder();

    //            var processor = new Processor();
    //            var compiler = processor.NewXsltCompiler();

    //            foreach (string schFile in schFiles)
    //            {
    //                if (!System.IO.File.Exists(schFile))
    //                {
    //                    allErrors.AppendLine("\nSchematron not found: " + schFile);
    //                    continue;
    //                }

    //                // Step 1: Compile Schematron .sch to .xsl
    //                string compiledXslPath = Path.GetTempFileName() + ".xsl";

    //                var exec1 = compiler.Compile(new Uri(transpilerPath));
    //                var transformer1 = exec1.Load();
    //                transformer1.SetInputStream(new FileStream(schFile, FileMode.Open, FileAccess.Read), new Uri(schFile));

    //                using (var outputStream = new FileStream(compiledXslPath, FileMode.Create))
    //                {
    //                    var serializer = processor.NewSerializer();
    //                    serializer.SetOutputStream(outputStream);
    //                    transformer1.Run(serializer);
    //                }

    //                // Step 2: Validate XML using compiled XSLT
    //                string resultPath = Path.GetTempFileName() + ".xml";

    //                var exec2 = compiler.Compile(new Uri(compiledXslPath));
    //                var transformer2 = exec2.Load();

    //                using (var xmlStream = new MemoryStream(Encoding.UTF8.GetBytes(xmlContent)))
    //                {
    //                    transformer2.SetInputStream(xmlStream, new Uri("file:///temp.xml"));

    //                    using (var resultStream = new FileStream(resultPath, FileMode.Create))
    //                    {
    //                        var serializer = processor.NewSerializer();
    //                        serializer.SetOutputStream(resultStream);
    //                        transformer2.Run(serializer);
    //                    }
    //                }

    //                // Step 3: Parse SVRL result for errors
    //                XmlDocument svrlDoc = new XmlDocument();
    //                svrlDoc.Load(resultPath);

    //                var ns = new XmlNamespaceManager(svrlDoc.NameTable);
    //                ns.AddNamespace("svrl", "http://purl.oclc.org/dsdl/svrl");
    //                var failedAsserts = svrlDoc.SelectNodes("//svrl:failed-assert", ns);
    //                var successfulReports = svrlDoc.SelectNodes("//svrl:successful-report", ns);

    //                if (failedAsserts.Count > 0 || successfulReports.Count > 0)
    //                {
    //                    int errorCounter = 0;

    //                    allErrors.AppendLine("=== Errors from " + Path.GetFileName(schFile) + " ===");
    //                    foreach (XmlNode assert in failedAsserts)
    //                    {
    //                        allErrors.AppendLine("");
    //                        allErrors.AppendLine(++errorCounter + ". " + "FAILED: " + assert.SelectSingleNode("svrl:text", ns)?.InnerText);
    //                        var location = assert.SelectSingleNode("@location", ns);
    //                        if (location != null) allErrors.AppendLine("Location: " + location.Value);
    //                    }
    //                    allErrors.AppendLine("");
    //                    allErrors.AppendLine("");
    //                }

    //                // Cleanup
    //                if (System.IO.File.Exists(compiledXslPath))
    //                    System.IO.File.Delete(compiledXslPath);
    //                if (System.IO.File.Exists(resultPath))
    //                    System.IO.File.Delete(resultPath);
    //            }

    //            if (allErrors.Length > 0)
    //            {
    //                System.IO.File.WriteAllText(logFilePath, allErrors.ToString());
    //            }
    //            return new Variable(allErrors.Length == 0 ? true : false);
    //        }
    //        catch (Exception ex)
    //        {
    //            System.IO.File.WriteAllText(logFilePath, "ex.Message: " + ex.Message);
    //            return new Variable(false);
    //        }
    //    }
    //}

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
            Utils.CheckArgs(args.Count, 1, m_name);

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

    /*
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
    */
    /*
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

        // Fallback for synchronous calls - now safe from UI thread deadlocks
        protected override Variable Evaluate(ParsingScript script)
        {
            try
            {
                // Run the async method on a background thread and wait for the result.
                // This prevents deadlocking the UI thread.
                return Task.Run(() => EvaluateAsync(script)).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                return new Variable("ERROR: " + ex.Message);
            }
        }

        public static async Task<string> CreateAndSendSignedSOAPAsync(string xmlBody, string soapAction, string endpoint, string certIdentifier, string certPassword, int timeoutSeconds)
        {
            //... (rest of the class is unchanged)
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

            //// 3. Find the first element in the Body to be the signing reference
            //XmlElement bodyContentElement = doc.DocumentElement.GetElementsByTagName("Body", "http://schemas.xmlsoap.org/soap/envelope/")[0]?.FirstChild as XmlElement;
            //if (bodyContentElement == null)
            //{
            //    throw new InvalidOperationException("SOAP Body does not contain a valid XML element to sign.");
            //}

            // robust lookup of element child inside SOAP Body (replace the existing single-line FirstChild lookup)
            XmlNodeList bodyNodes = doc.DocumentElement.GetElementsByTagName("Body", "http://schemas.xmlsoap.org/soap/envelope/");
            if (bodyNodes == null || bodyNodes.Count == 0)
            {
                throw new InvalidOperationException("SOAP Envelope Body element not found (namespace mismatch or envelope malformed).");
            }

            XmlElement bodyElem = bodyNodes[0] as XmlElement;
            if (bodyElem == null)
            {
                throw new InvalidOperationException("SOAP Body node is not an element.");
            }

            // find first element child (skip text/comments/processing instructions)
            XmlElement bodyContentElement = null;
            foreach (XmlNode node in bodyElem.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Element)
                {
                    bodyContentElement = (XmlElement)node;
                    break;
                }
            }

            if (bodyContentElement == null)
            {
                // Provide useful diagnostic: show a short snippet of Body contents
                string snippet = bodyElem.InnerXml ?? string.Empty;
                if (snippet.Length > 200) snippet = snippet.Substring(0, 200) + "...";
                throw new InvalidOperationException("SOAP Body does not contain a valid XML element to sign. Body contents (truncated): " + snippet);
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
            if (!cert.HasPrivateKey)
            {
                throw new InvalidOperationException("Certificate does not contain a private key.");
            }

            // Explicitly get the RSA private key. This handles both CNG and older CryptoAPI keys.
            var rsaKey = cert.GetRSAPrivateKey();
            if (rsaKey == null)
            {
                throw new InvalidOperationException("Could not retrieve RSA private key from the certificate.");
            }

            SignedXml signedXml = new SignedXml(doc);
            signedXml.SigningKey = rsaKey; // Use the explicit RSA key object

            // Set the signature method and digest method to SHA256, as required by FINA.
            signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";

            Reference reference = new Reference("#" + referenceId);
            reference.AddTransform(new XmlDsigExcC14NTransform());
            reference.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
            signedXml.AddReference(reference);

            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(cert));
            signedXml.KeyInfo = keyInfo;

            signedXml.ComputeSignature();

            XmlElement signatureElement = signedXml.GetXml();

            // Find or create the SOAP Header
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(doc.NameTable);
            nsMgr.AddNamespace("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");
            XmlElement header = doc.SelectSingleNode("//soapenv:Header", nsMgr) as XmlElement;
            if (header == null)
            {
                header = doc.CreateElement("soapenv", "Header", "http://schemas.xmlsoap.org/soap/envelope/");
                doc.DocumentElement.PrependChild(header);
            }

            // Create and append the wsse:Security element
            XmlElement securityElement = doc.CreateElement("wsse", "Security", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            securityElement.SetAttribute("xmlns:wsu", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
            securityElement.AppendChild(signatureElement);
            header.AppendChild(securityElement);
        }
    }
    */
    /*
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
            int timeoutSeconds = Utils.GetSafeInt(args, 5, 60);

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

        // Fallback for synchronous calls - now safe from UI thread deadlocks
        protected override Variable Evaluate(ParsingScript script)
        {
            try
            {
                // Run the async method on a background thread and wait for the result.
                // This prevents deadlocking the UI thread.
                return Task.Run(() => EvaluateAsync(script)).GetAwaiter().GetResult();
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

            // 2. Create and Sign the SOAP Envelope using the compliant signer
            string signedSoapXml = FinaSoapSigner.SignSoap(xmlBody, cert);

            // 3. Send the request using HttpWebRequest
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Headers.Add("SOAPAction", soapAction);
            request.ContentType = "text/xml;charset=\"utf-8\"";
            request.Accept = "text/xml";
            request.Method = "POST";
            request.ClientCertificates.Add(cert);
            request.Timeout = timeoutSeconds * 1000;

            // For development/testing only. In production, ensure the server's certificate is trusted.
            request.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(signedSoapXml);
                request.ContentLength = byteArray.Length;

                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    await requestStream.WriteAsync(byteArray, 0, byteArray.Length);
                }

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            return await reader.ReadToEndAsync();
                        }
                    }
                }
            }
            catch (WebException webEx)
            {
                string errorText = $"HTTP_ERROR: {webEx.Status}";
                if (webEx.Response != null)
                {
                    using (StreamReader reader = new StreamReader(webEx.Response.GetResponseStream()))
                    {
                        errorText += "\n" + await reader.ReadToEndAsync();
                    }
                }
                else
                {
                    errorText += " - " + webEx.Message;
                }
                return errorText;
            }
            catch (Exception ex)
            {
                return $"EXCEPTION: {ex.Message}";
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
                if (certs.Count > 0)
                {
                    return certs[0];
                }
            }
            return null;
        }
    }

    public class FinaSoapSigner
    {
        // Namespace constants for clarity
        private const string SoapEnvNs = "http://schemas.xmlsoap.org/soap/envelope/";
        private const string WsseNs = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
        private const string WsuNs = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
        private const string DsNs = "http://www.w3.org/2000/09/xmldsig#";

        // Custom SignedXml class to handle wsu:Id attributes
        private class WSSignedXml : SignedXml
        {
            public WSSignedXml(XmlDocument doc) : base(doc) { }

            public override XmlElement GetIdElement(XmlDocument doc, string id)
            {
                var nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("wsu", WsuNs);
                return doc.SelectSingleNode($"//*[@wsu:Id='{id}']", nsmgr) as XmlElement ?? base.GetIdElement(doc, id);
            }
        }

        public static string SignSoap(string xmlBody, X509Certificate2 cert)
        {
            if (cert == null) throw new ArgumentNullException(nameof(cert));
            if (!cert.HasPrivateKey) throw new ArgumentException("Certificate must have a private key for signing.", nameof(cert));

            var doc = new XmlDocument { PreserveWhitespace = true };
            doc.LoadXml($@"<soapenv:Envelope xmlns:soapenv=""{SoapEnvNs}"" xmlns:wsu=""{WsuNs}"">
                             <soapenv:Header/>
                             <soapenv:Body>{xmlBody}</soapenv:Body>
                           </soapenv:Envelope>");

            // 1. Prepare unique IDs
            string bodyId = "Body-" + Guid.NewGuid().ToString("N");
            string timestampId = "TS-" + Guid.NewGuid().ToString("N");
            string tokenId = "X509-" + Guid.NewGuid().ToString("N");

            // 2. Add wsu:Id to Body
            var body = doc.SelectSingleNode($"//*[local-name()='Body' and namespace-uri()='{SoapEnvNs}']") as XmlElement;
            body.SetAttribute("Id", WsuNs, bodyId);

            // 3. Create Security Header
            var header = doc.SelectSingleNode($"//*[local-name()='Header' and namespace-uri()='{SoapEnvNs}']") as XmlElement;
            var security = doc.CreateElement("wsse", "Security", WsseNs);
            security.SetAttribute("mustUnderstand", SoapEnvNs, "1");
            header.AppendChild(security);

            // 4. Create and add Timestamp
            var timestamp = doc.CreateElement("wsu", "Timestamp", WsuNs);
            timestamp.SetAttribute("Id", WsuNs, timestampId);
            var created = doc.CreateElement("wsu", "Created", WsuNs);
            created.InnerText = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            var expires = doc.CreateElement("wsu", "Expires", WsuNs);
            expires.InnerText = DateTime.UtcNow.AddMinutes(5).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            timestamp.AppendChild(created);
            timestamp.AppendChild(expires);
            security.AppendChild(timestamp);

            // 5. Create and add BinarySecurityToken
            var bst = doc.CreateElement("wsse", "BinarySecurityToken", WsseNs);
            bst.SetAttribute("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
            bst.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
            bst.SetAttribute("Id", WsuNs, tokenId);
            bst.InnerText = Convert.ToBase64String(cert.RawData);
            security.AppendChild(bst);

            // 6. Create Signature
            var signedXml = new WSSignedXml(doc) { SigningKey = cert.GetRSAPrivateKey() };
            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url;

            // 7. Add References to Body and Timestamp
            var refBody = new Reference($"#{bodyId}") { DigestMethod = SignedXml.XmlDsigSHA256Url };
            refBody.AddTransform(new XmlDsigExcC14NTransform());
            signedXml.AddReference(refBody);

            var refTs = new Reference($"#{timestampId}") { DigestMethod = SignedXml.XmlDsigSHA256Url };
            refTs.AddTransform(new XmlDsigExcC14NTransform());
            signedXml.AddReference(refTs);

            // 8. Create KeyInfo pointing to the BinarySecurityToken
            var keyInfo = new KeyInfo();
            var str = doc.CreateElement("wsse", "SecurityTokenReference", WsseNs);
            var strRef = doc.CreateElement("wsse", "Reference", WsseNs);
            strRef.SetAttribute("URI", $"#{tokenId}");
            strRef.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
            str.AppendChild(strRef);
            keyInfo.AddClause(new KeyInfoNode(str));
            signedXml.KeyInfo = keyInfo;

            // 9. Compute signature and add to security header
            signedXml.ComputeSignature();
            security.AppendChild(doc.ImportNode(signedXml.GetXml(), true));

            return doc.OuterXml;
        }
    }
    */

    /*
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
            int timeoutSeconds = Utils.GetSafeInt(args, 5, 60);

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

        // Fallback for synchronous calls - now safe from UI thread deadlocks
        protected override Variable Evaluate(ParsingScript script)
        {
            try
            {
                // Run the async method on a background thread and wait for the result.
                // This prevents deadlocking the UI thread.
                return Task.Run(() => EvaluateAsync(script)).GetAwaiter().GetResult();
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

            // 2. Create and Sign the SOAP Envelope using the compliant signer
            string signedSoapXml = FinaSoapSigner.SignSoap(xmlBody, cert);

            // 3. Send the request using HttpWebRequest
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Headers.Add("SOAPAction", soapAction);
            request.ContentType = "text/xml;charset=\"utf-8\"";
            request.Accept = "text/xml";
            request.Method = "POST";
            request.ClientCertificates.Add(cert);
            request.Timeout = timeoutSeconds * 1000;

            // For development/testing only. In production, ensure the server's certificate is trusted.
            request.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(signedSoapXml);
                request.ContentLength = byteArray.Length;

                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    await requestStream.WriteAsync(byteArray, 0, byteArray.Length);
                }

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            return await reader.ReadToEndAsync();
                        }
                    }
                }
            }
            catch (WebException webEx)
            {
                string errorText = $"HTTP_ERROR: {webEx.Status}";
                if (webEx.Response != null)
                {
                    using (StreamReader reader = new StreamReader(webEx.Response.GetResponseStream()))
                    {
                        errorText += "\n" + await reader.ReadToEndAsync();
                    }
                }
                else
                {
                    errorText += " - " + webEx.Message;
                }
                return errorText;
            }
            catch (Exception ex)
            {
                return $"EXCEPTION: {ex.Message}";
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
                if (certs.Count > 0)
                {
                    return certs[0];
                }
            }
            return null;
        }
    }

    public class FinaSoapSigner
    {
        // Namespace constants
        private const string SoapEnvNs = "http://schemas.xmlsoap.org/soap/envelope/";
        private const string WsseNs = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
        private const string WsuNs = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
        private const string DsNs = "http://www.w3.org/2000/09/xmldsig#";
        private const string ExcC14nNs = "http://www.w3.org/2001/10/xml-exc-c14n#";

        private class WSSignedXml : SignedXml
        {
            public WSSignedXml(XmlDocument doc) : base(doc) { }
            public override XmlElement GetIdElement(XmlDocument doc, string id)
            {
                var nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("wsu", WsuNs);
                return doc.SelectSingleNode($"//*[@wsu:Id='{id}']", nsmgr) as XmlElement ?? base.GetIdElement(doc, id);
            }
        }
    */
    /*
    public static string SignSoap(string xmlBody, X509Certificate2 cert)
    {
        if (cert == null) throw new ArgumentNullException(nameof(cert));
        if (!cert.HasPrivateKey) throw new ArgumentException("Certificate must have a private key.", nameof(cert));

        var doc = new XmlDocument { PreserveWhitespace = true };
        doc.LoadXml($@"<SOAP-ENV:Envelope xmlns:SOAP-ENV=""{SoapEnvNs}"" xmlns:wsu=""{WsuNs}"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
                         <SOAP-ENV:Header/>
                         <SOAP-ENV:Body>{xmlBody}</SOAP-ENV:Body>
                       </SOAP-ENV:Envelope>");

        string bodyId = "id-" + Guid.NewGuid().ToString("N");
        string tokenId = "X509-" + Guid.NewGuid().ToString("N");
        string timestampId = "TS-" + Guid.NewGuid().ToString("N");

        var header = doc.SelectSingleNode($"//*[local-name()='Header']") as XmlElement;
        var body = doc.SelectSingleNode($"//*[local-name()='Body']") as XmlElement;

        body.SetAttribute("Id", WsuNs, bodyId);

        var security = doc.CreateElement("wsse", "Security", WsseNs);
        security.SetAttribute("mustUnderstand", SoapEnvNs, "1");
        // security.SetAttribute("xmlns:wsu", WsuNs); // This is now redundant
        header.AppendChild(security);

        var timestamp = doc.CreateElement("wsu", "Timestamp", WsuNs);
        timestamp.SetAttribute("Id", WsuNs, timestampId);
        timestamp.InnerXml = $"<wsu:Created>{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ss.fffZ}</wsu:Created><wsu:Expires>{DateTime.UtcNow.AddMinutes(5):yyyy-MM-ddTHH:mm:ss.fffZ}</wsu:Expires>";
        security.AppendChild(timestamp);

        var bst = doc.CreateElement("wsse", "BinarySecurityToken", WsseNs);
        bst.SetAttribute("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
        bst.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
        bst.SetAttribute("Id", WsuNs, tokenId);
        bst.InnerText = Convert.ToBase64String(cert.RawData);
        security.AppendChild(bst);

        var signedXml = new WSSignedXml(doc) { SigningKey = cert.GetRSAPrivateKey() };
        signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";

        // Create CanonicalizationMethod with InclusiveNamespaces
        var c14nMethod = doc.CreateElement("ds", "CanonicalizationMethod", DsNs);
        c14nMethod.SetAttribute("Algorithm", ExcC14nNs);
        var inclusiveNamespaces = doc.CreateElement("ec", "InclusiveNamespaces", ExcC14nNs);
        inclusiveNamespaces.SetAttribute("PrefixList", "SOAP-ENV wsu");
        c14nMethod.AppendChild(inclusiveNamespaces);

        // Set the canonicalization method using the CanonicalizationMethod property:
        signedXml.SignedInfo.CanonicalizationMethod = ExcC14nNs;

        var transform = signedXml.SignedInfo.CanonicalizationMethodObject as XmlDsigExcC14NTransform;
        if (transform != null)
        {
            transform.LoadInnerXml(c14nMethod.ChildNodes);
        }

        var reference = new Reference($"#{bodyId}") { DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256" };
        var c14nTransform = new XmlDsigExcC14NTransform();
        reference.AddTransform(c14nTransform);
        signedXml.AddReference(reference);


        var keyInfo = new KeyInfo();
        var str = doc.CreateElement("wsse", "SecurityTokenReference", WsseNs);
        var strRef = doc.CreateElement("wsse", "Reference", WsseNs);
        strRef.SetAttribute("URI", $"#{tokenId}");
        strRef.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
        str.AppendChild(strRef);
        keyInfo.AddClause(new KeyInfoNode(str));
        signedXml.KeyInfo = keyInfo;

        signedXml.ComputeSignature();
        security.AppendChild(doc.ImportNode(signedXml.GetXml(), true));

        return doc.OuterXml;
    }
    */
    /*
    public static string SignSoap(string xmlBody, X509Certificate2 cert)
    {
        if (cert == null) throw new ArgumentNullException(nameof(cert));
        if (!cert.HasPrivateKey) throw new ArgumentException("Certificate must have a private key.", nameof(cert));

        var doc = new XmlDocument { PreserveWhitespace = true };
        doc.LoadXml($@"<SOAP-ENV:Envelope xmlns:SOAP-ENV=""{SoapEnvNs}"" xmlns:wsu=""{WsuNs}"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
                         <SOAP-ENV:Header/>
                         <SOAP-ENV:Body>{xmlBody}</SOAP-ENV:Body>
                       </SOAP-ENV:Envelope>");

        string bodyId = "id-" + Guid.NewGuid().ToString("N");
        string tokenId = "X509-" + Guid.NewGuid().ToString("N");
        string timestampId = "TS-" + Guid.NewGuid().ToString("N");

        var header = doc.SelectSingleNode($"//*[local-name()='Header']") as XmlElement;
        var body = doc.SelectSingleNode($"//*[local-name()='Body']") as XmlElement;

        body.SetAttribute("Id", WsuNs, bodyId);

        var security = doc.CreateElement("wsse", "Security", WsseNs);
        security.SetAttribute("mustUnderstand", SoapEnvNs, "1");
        header.AppendChild(security);

        // Create Timestamp with child elements properly (not using InnerXml)
        var timestamp = doc.CreateElement("wsu", "Timestamp", WsuNs);
        timestamp.SetAttribute("Id", WsuNs, timestampId);

        var created = doc.CreateElement("wsu", "Created", WsuNs);
        created.InnerText = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        timestamp.AppendChild(created);

        var expires = doc.CreateElement("wsu", "Expires", WsuNs);
        expires.InnerText = DateTime.UtcNow.AddMinutes(5).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        timestamp.AppendChild(expires);

        security.AppendChild(timestamp);

        var bst = doc.CreateElement("wsse", "BinarySecurityToken", WsseNs);
        bst.SetAttribute("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
        bst.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
        bst.SetAttribute("Id", WsuNs, tokenId);
        bst.InnerText = Convert.ToBase64String(cert.RawData);
        security.AppendChild(bst);

        var signedXml = new WSSignedXml(doc) { SigningKey = cert.GetRSAPrivateKey() };
        signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
        signedXml.SignedInfo.CanonicalizationMethod = ExcC14nNs;

        var reference = new Reference($"#{bodyId}") { DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256" };
        var c14nTransform = new XmlDsigExcC14NTransform();
        reference.AddTransform(c14nTransform);
        signedXml.AddReference(reference);

        var keyInfo = new KeyInfo();
        var str = doc.CreateElement("wsse", "SecurityTokenReference", WsseNs);
        var strRef = doc.CreateElement("wsse", "Reference", WsseNs);
        strRef.SetAttribute("URI", $"#{tokenId}");
        strRef.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
        str.AppendChild(strRef);
        keyInfo.AddClause(new KeyInfoNode(str));
        signedXml.KeyInfo = keyInfo;

        signedXml.ComputeSignature();
        security.AppendChild(doc.ImportNode(signedXml.GetXml(), true));

        return doc.OuterXml;
    }
}
    */

    public class CreateSOAPFunction : ParserFunction
    {
        protected override async Task<Variable> EvaluateAsync(ParsingScript script)
        {
            List<Variable> args = await script.GetFunctionArgsAsync();
            Utils.CheckArgs(args.Count, 4, m_name);

            string xmlBody = Utils.GetSafeString(args, 0);
            string soapAction = Utils.GetSafeString(args, 1);
            string endpoint = Utils.GetSafeString(args, 2);
            string certIdentifier = Utils.GetSafeString(args, 3);
            string certPassword = Utils.GetSafeString(args, 4);
            int timeoutSeconds = Utils.GetSafeInt(args, 5, 60);

            try
            {
                string soapResponse = await CreateAndSendSignedSOAPAsync(xmlBody, soapAction, endpoint, certIdentifier, certPassword, timeoutSeconds);
                return new Variable(soapResponse);
            }
            catch (Exception ex)
            {
                return new Variable("ERROR: " + ex.Message + (ex.InnerException != null ? " | Inner: " + ex.InnerException.Message : ""));
            }
        }

        protected override Variable Evaluate(ParsingScript script)
        {
            try
            {
                return Task.Run(() => EvaluateAsync(script)).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                return new Variable("ERROR: " + ex.Message);
            }
        }

        public static async Task<string> CreateAndSendSignedSOAPAsync(string xmlBody, string soapAction, string endpoint, string certIdentifier, string certPassword, int timeoutSeconds)
        {
            X509Certificate2 cert = LoadCertificate(certIdentifier, certPassword);
            if (cert == null)
            {
                throw new ArgumentException("Certificate could not be loaded. Identifier: " + certIdentifier);
            }

            // Create and Sign the SOAP Envelope
            string signedSoapXml = FinaSoapSigner.SignSoap(xmlBody, cert);

            // Save for debugging
            System.IO.File.WriteAllText("debug_signed_soap.xml", signedSoapXml);

            // Send the request
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
            request.Headers.Add("SOAPAction", soapAction);
            request.ContentType = "text/xml;charset=\"utf-8\"";
            request.Accept = "text/xml";
            request.Method = "POST";
            request.ClientCertificates.Add(cert);
            request.Timeout = timeoutSeconds * 1000;
            request.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(signedSoapXml);
                request.ContentLength = byteArray.Length;

                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    await requestStream.WriteAsync(byteArray, 0, byteArray.Length);
                }

                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string responseText = await reader.ReadToEndAsync();
                            System.IO.File.WriteAllText("debug_response.xml", responseText);
                            return responseText;
                        }
                    }
                }
            }
            catch (WebException webEx)
            {
                string errorText = $"HTTP_ERROR: {webEx.Status}";
                if (webEx.Response != null)
                {
                    using (StreamReader reader = new StreamReader(webEx.Response.GetResponseStream()))
                    {
                        errorText += "\n" + await reader.ReadToEndAsync();
                    }
                }
                else
                {
                    errorText += " - " + webEx.Message;
                }
                System.IO.File.WriteAllText("debug_error.txt", errorText);
                return errorText;
            }
            catch (Exception ex)
            {
                return $"EXCEPTION: {ex.Message}";
            }
        }

        private static X509Certificate2 LoadCertificate(string identifier, string password)
        {
            if (System.IO.File.Exists(identifier))
            {
                return new X509Certificate2(identifier, password, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
            }

            using (X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                store.Open(OpenFlags.ReadOnly);
                var certs = store.Certificates.Find(X509FindType.FindByThumbprint, identifier, false);
                return certs.Count > 0 ? certs[0] : null;
            }
        }
        public class FinaSoapSigner
        {
            // Namespace constants
            private const string SoapEnvNs = "http://schemas.xmlsoap.org/soap/envelope/";
            private const string WsseNs = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd";
            private const string WsuNs = "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";
            private const string DsNs = "http://www.w3.org/2000/09/xmldsig#";
            private const string ExcC14nNs = "http://www.w3.org/2001/10/xml-exc-c14n#";

            private class WSSignedXml : SignedXml
            {
                public WSSignedXml(XmlDocument doc) : base(doc) { }
                public override XmlElement GetIdElement(XmlDocument doc, string id)
                {
                    var nsmgr = new XmlNamespaceManager(doc.NameTable);
                    nsmgr.AddNamespace("wsu", WsuNs);
                    return doc.SelectSingleNode($"//*[@wsu:Id='{id}']", nsmgr) as XmlElement ?? base.GetIdElement(doc, id);
                }
            }

            // Overload that accepts X509Certificate2 directly
            public static string SignSoap(string xmlBody, X509Certificate2 cert)
            {
                if (cert == null) throw new ArgumentNullException(nameof(cert));
                if (!cert.HasPrivateKey) throw new ArgumentException("Certificate must have a private key.", nameof(cert));

                var doc = new XmlDocument { PreserveWhitespace = true };
                doc.LoadXml($@"<SOAP-ENV:Envelope xmlns:SOAP-ENV=""{SoapEnvNs}"" xmlns:wsu=""{WsuNs}"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
                         <SOAP-ENV:Header/>
                         <SOAP-ENV:Body>{xmlBody}</SOAP-ENV:Body>
                       </SOAP-ENV:Envelope>");

                string bodyId = "id-" + Guid.NewGuid().ToString("N");
                string tokenId = "X509-" + Guid.NewGuid().ToString("N");
                string timestampId = "TS-" + Guid.NewGuid().ToString("N");

                var header = doc.SelectSingleNode($"//*[local-name()='Header']") as XmlElement;
                var body = doc.SelectSingleNode($"//*[local-name()='Body']") as XmlElement;

                body.SetAttribute("Id", WsuNs, bodyId);

                var security = doc.CreateElement("wsse", "Security", WsseNs);
                security.SetAttribute("mustUnderstand", SoapEnvNs, "1");
                header.AppendChild(security);

                // Create Timestamp with child elements properly (not using InnerXml)
                var timestamp = doc.CreateElement("wsu", "Timestamp", WsuNs);
                timestamp.SetAttribute("Id", WsuNs, timestampId);

                var created = doc.CreateElement("wsu", "Created", WsuNs);
                created.InnerText = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                timestamp.AppendChild(created);

                var expires = doc.CreateElement("wsu", "Expires", WsuNs);
                expires.InnerText = DateTime.UtcNow.AddMinutes(5).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
                timestamp.AppendChild(expires);

                security.AppendChild(timestamp);

                var bst = doc.CreateElement("wsse", "BinarySecurityToken", WsseNs);
                bst.SetAttribute("EncodingType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");
                bst.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
                bst.SetAttribute("Id", WsuNs, tokenId);
                bst.InnerText = Convert.ToBase64String(cert.RawData);
                security.AppendChild(bst);

                var signedXml = new WSSignedXml(doc) { SigningKey = cert.GetRSAPrivateKey() };
                signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
                signedXml.SignedInfo.CanonicalizationMethod = ExcC14nNs;

                // Sign BOTH Body AND Timestamp
                var refBody = new Reference($"#{bodyId}") { DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256" };
                refBody.AddTransform(new XmlDsigExcC14NTransform());
                signedXml.AddReference(refBody);

                var refTimestamp = new Reference($"#{timestampId}") { DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256" };
                refTimestamp.AddTransform(new XmlDsigExcC14NTransform());
                signedXml.AddReference(refTimestamp);

                var keyInfo = new KeyInfo();
                var str = doc.CreateElement("wsse", "SecurityTokenReference", WsseNs);
                var strRef = doc.CreateElement("wsse", "Reference", WsseNs);
                strRef.SetAttribute("URI", $"#{tokenId}");
                strRef.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
                str.AppendChild(strRef);
                keyInfo.AddClause(new KeyInfoNode(str));
                signedXml.KeyInfo = keyInfo;

                signedXml.ComputeSignature();
                security.AppendChild(doc.ImportNode(signedXml.GetXml(), true));

                return doc.OuterXml;
            }
        }
    }

    public class UUIDFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 0, m_name);

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

            xmlDoc.LoadXml(@"<?xml version=""1.0"" encoding=""UTF-8""?>
<soapenv:Envelope
    xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
    xmlns:ech=""http://fina.hr/eracun/b2b/pki/Echo/v0.1""
    xmlns:iwsc=""http://fina.hr/eracun/b2b/invoicewebservicecomponents/v0.1"">
    <soapenv:Header/>
    <soapenv:Body>
        <EchoBuyerMsg>
            <HeaderBuyer xmlns=""http://fina.hr/eracun/b2b/invoicewebservicecomponents/v0.1"">
                <MessageID>b96d8194-ff6a-4165-8d8e-39fb303ba2e1</MessageID>
                <BuyerID>9934:26389058739</BuyerID>
                <MessageType>9999</MessageType>
            </HeaderBuyer>
            <Data xmlns=""http://fina.hr/eracun/b2b/EchoBuyer/v0.1"">
                <EchoData>
                    <Echo>Hello world</Echo>
                </EchoData>
            </Data>
        </EchoBuyerMsg>
    </soapenv:Body>
</soapenv:Envelope>");

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
            string p12Path = "C:\\WinX\\certifikati\\p12_aurasoft_demo.p12";      // <-- your file
            string p12Password = "Aurasoft1";               // <-- your p12 password
            //string serviceUrl = "http://prezdigitalneusluge.fina.hr/SendB2BOutgoingInvoicePKIWebService"; // <-- replace with real endpoint
            string serviceUrl = "https://prezdigitalneusluge.fina.hr/SendB2BOutgoingInvoicePKIWebService/services/SendB2BOutgoingInvoicePKIWebService";

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
    }

    public class CreateSOAP2Function : ParserFunction
    {
        protected override async Task<Variable> EvaluateAsync(ParsingScript script)
        {
            List<Variable> args = await script.GetFunctionArgsAsync();
            Utils.CheckArgs(args.Count, 4, m_name);

            string xmlBody = Utils.GetSafeString(args, 0);
            string soapAction = Utils.GetSafeString(args, 1);
            string endpoint = Utils.GetSafeString(args, 2);
            string certIdentifier = Utils.GetSafeString(args, 3);
            string certPassword = Utils.GetSafeString(args, 4);
            string erpId = Utils.GetSafeString(args, 5, ""); // New ERPID parameter
            int timeoutSeconds = Utils.GetSafeInt(args, 6, 60);

            try
            {
                X509Certificate2 cert = LoadCertificate(certIdentifier, certPassword);
                if (cert == null)
                {
                    return new Variable("ERROR: Certificate could not be loaded. Identifier: " + certIdentifier);
                }

                string soapResponse = await CreateAndSendFINACompliantSOAPAsync(xmlBody, soapAction, endpoint, cert, erpId, timeoutSeconds);
                return new Variable(soapResponse);
            }
            catch (Exception ex)
            {
                return new Variable("ERROR: " + ex.Message + (ex.InnerException != null ? " | Inner: " + ex.InnerException.Message : ""));
            }
        }

        public static async Task<string> CreateAndSendFINACompliantSOAPAsync(string xmlBody, string soapAction, string endpoint,
            X509Certificate2 cert, string erpId, int timeoutSeconds)
        {
            // Generate unique IDs for WS-Security elements
            string timestampId = "TS-" + Guid.NewGuid().ToString("N");
            string securityTokenId = "X509-" + Guid.NewGuid().ToString("N");
            string bodyId = "Body-" + Guid.NewGuid().ToString("N");

            // Create FINA-compliant SOAP envelope
            string soapEnvelope = CreateFINACompliantSoapEnvelope(xmlBody, securityTokenId, timestampId, bodyId, cert, erpId);
            soapEnvelope = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<soapenv:Envelope
    xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
    xmlns:ech=""http://fina.hr/eracun/b2b/pki/Echo/v0.1""
    xmlns:iwsc=""http://fina.hr/eracun/b2b/invoicewebservicecomponents/v0.1"">
    <soapenv:Header/>
    <soapenv:Body>
        <EchoBuyerMsg>
            <HeaderBuyer xmlns=""http://fina.hr/eracun/b2b/invoicewebservicecomponents/v0.1"">
                <MessageID>b96d8194-ff6a-4165-8d8e-39fb303ba2e1</MessageID>
                <BuyerID>9934:26389058739</BuyerID>
                <MessageType>9999</MessageType>
            </HeaderBuyer>
            <Data xmlns=""http://fina.hr/eracun/b2b/EchoBuyer/v0.1"">
                <EchoData>
                    <Echo>Hello world</Echo>
                </EchoData>
            </Data>
        </EchoBuyerMsg>
    </soapenv:Body>
</soapenv:Envelope>";

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(soapEnvelope);

            // Add ID to Body element
            XmlElement bodyElement = doc.SelectSingleNode("//*[local-name()='Body']") as XmlElement;
            if (bodyElement != null)
            {
                bodyElement.SetAttribute("Id", bodyId);
            }

            // Sign the SOAP document
            SignFINACompliantSoap(doc, cert, securityTokenId, timestampId, bodyId);

            // Send the request
            using (var handler = new HttpClientHandler())
            {
                handler.ClientCertificates.Add(cert);
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

                using (var client = new HttpClient(handler))
                {
                    client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);

                    var content = new StringContent(doc.OuterXml, Encoding.UTF8, "text/xml");

                    // FINA requires empty SOAPAction for their services
                    content.Headers.Add("SOAPAction", string.IsNullOrEmpty(soapAction) ? "" : soapAction);
                    content.Headers.ContentType.CharSet = "utf-8";

                    // Add custom headers if needed
                    // content.Headers.Add("X-ERP-ID", erpId);

                    try
                    {
                        HttpResponseMessage response = await client.PostAsync(endpoint, content);
                        string responseString = await response.Content.ReadAsStringAsync();

                        if (!response.IsSuccessStatusCode)
                        {
                            // Try to extract error details
                            string errorDetails = ExtractFINAErrorMessage(responseString);
                            throw new WebException($"HTTP Error {(int)response.StatusCode} {response.ReasonPhrase}. {errorDetails}");
                        }

                        return responseString;
                    }
                    catch (TaskCanceledException ex)
                    {
                        throw new WebException($"Request timeout after {timeoutSeconds} seconds: {ex.Message}");
                    }
                }
            }
        }

        private static string CreateFINACompliantSoapEnvelope(string xmlBody, string securityTokenId, string timestampId,
            string bodyId, X509Certificate2 cert, string erpId)
        {
            string certBase64 = Convert.ToBase64String(cert.RawData);
            string created = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            string expires = DateTime.UtcNow.AddMinutes(5).ToString("yyyy-MM-ddTHH:mm:ss.fffZ");

            // Build FINA-compliant SOAP envelope with ERPID in custom header if provided
            string envelope = $@"<soapenv:Envelope 
    xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
    xmlns:wsse=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd""
    xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">
    <soapenv:Header>";

            // Add ERPID as a custom header if provided
            if (!string.IsNullOrEmpty(erpId))
            {
                envelope += $@"
        <ERPID xmlns=""http://www.fina.hr/e-racun/services"">{erpId}</ERPID>";
            }

            envelope += $@"
        <wsse:Security soapenv:mustUnderstand=""1"">
            <wsse:BinarySecurityToken 
                EncodingType=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary""
                ValueType=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3""
                wsu:Id=""{securityTokenId}"">
                {certBase64}
            </wsse:BinarySecurityToken>
            <wsu:Timestamp wsu:Id=""{timestampId}"">
                <wsu:Created>{created}</wsu:Created>
                <wsu:Expires>{expires}</wsu:Expires>
            </wsu:Timestamp>
        </wsse:Security>
    </soapenv:Header>
    <soapenv:Body wsu:Id=""{bodyId}"">
        {xmlBody}
    </soapenv:Body>
 </soapenv:Envelope>";

            return envelope;
        }

        private static void SignFINACompliantSoap(XmlDocument doc, X509Certificate2 cert, string securityTokenId,
            string timestampId, string bodyId)
        {
            if (cert.PrivateKey == null)
            {
                throw new InvalidOperationException("Certificate does not contain a private key.");
            }

            SignedXml signedXml = new SignedXml(doc);
            signedXml.SigningKey = cert.PrivateKey;

            signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigExcC14NTransformUrl;
            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url;

            // Reference to Body
            Reference bodyRef = new Reference("#" + bodyId);
            bodyRef.AddTransform(new XmlDsigExcC14NTransform());
            bodyRef.DigestMethod = SignedXml.XmlDsigSHA256Url;
            signedXml.AddReference(bodyRef);

            // Reference to Timestamp
            Reference timestampRef = new Reference("#" + timestampId);
            timestampRef.AddTransform(new XmlDsigExcC14NTransform());
            timestampRef.DigestMethod = SignedXml.XmlDsigSHA256Url;
            signedXml.AddReference(timestampRef);

            // Reference to BinarySecurityToken
            Reference tokenRef = new Reference("#" + securityTokenId);
            tokenRef.AddTransform(new XmlDsigExcC14NTransform());
            tokenRef.DigestMethod = SignedXml.XmlDsigSHA256Url;
            signedXml.AddReference(tokenRef);

            // Add KeyInfo with SecurityTokenReference
            KeyInfo keyInfo = new KeyInfo();
            KeyInfoX509Data x509Data = new KeyInfoX509Data(cert);
            keyInfo.AddClause(x509Data);

            XmlElement keyInfoElement = keyInfo.GetXml();
            XmlElement securityTokenRef = doc.CreateElement("wsse", "SecurityTokenReference",
                "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            XmlElement referenceElement = doc.CreateElement("wsse", "Reference",
                "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            referenceElement.SetAttribute("URI", "#" + securityTokenId);
            referenceElement.SetAttribute("ValueType", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");
            securityTokenRef.AppendChild(referenceElement);

            keyInfoElement.InsertBefore(securityTokenRef, keyInfoElement.FirstChild);
            signedXml.KeyInfo = keyInfo;

            // Compute signature
            signedXml.ComputeSignature();

            // Add signature to Security header
            XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
            nsManager.AddNamespace("wsse", "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");
            nsManager.AddNamespace("soapenv", "http://schemas.xmlsoap.org/soap/envelope/");

            XmlElement securityElement = doc.SelectSingleNode("//soapenv:Header/wsse:Security", nsManager) as XmlElement;
            if (securityElement != null)
            {
                securityElement.AppendChild(doc.ImportNode(signedXml.GetXml(), true));
            }
            else
            {
                throw new InvalidOperationException("Security element not found in SOAP header.");
            }
        }

        private static string ExtractFINAErrorMessage(string response)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(response);

                XmlNamespaceManager nsManager = new XmlNamespaceManager(doc.NameTable);
                nsManager.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
                nsManager.AddNamespace("fin", "http://www.fina.hr/e-racun/services");

                // Try to find error message in various locations
                XmlNode errorNode = doc.SelectSingleNode("//faultstring", nsManager) ??
                                   doc.SelectSingleNode("//ErrorMessage", nsManager) ??
                                   doc.SelectSingleNode("//*[contains(local-name(), 'Error')]", nsManager);

                return errorNode?.InnerText ?? "No error details available";
            }
            catch
            {
                // If XML parsing fails, return raw response or truncated version
                return response.Length > 500 ? response.Substring(0, 500) + "..." : response;
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
    }

    public class TEST1Function : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 0, m_name);

            string unsignedXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""
                   xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                   xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
    <SOAP-ENV:Header />
    <SOAP-ENV:Body
                   xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">
        <EchoMsg xmlns=""http://fina.hr/eracun/b2b/pki/Echo/v0.1"">
            <HeaderSupplier xmlns=""http://fina.hr/eracun/b2b/invoicewebservicecomponents/v0.1"">
                <MessageID>" + Guid.NewGuid() + @"</MessageID>
                <SupplierID>9934:26389058739</SupplierID>" +
            //"<ERPID>e-racun-winx</ERPID> " + 
            //@"<MessageType>9999</MessageType>" +
            //@"<MessageAttributes>echo poruka</MessageAttributes>" +
            @"</HeaderSupplier>
            <Data>
                <EchoData>
                    <Echo>hello from Aurasoft!</Echo>
                </EchoData>
            </Data>
        </EchoMsg>
    </SOAP-ENV:Body>
</SOAP-ENV:Envelope>";

            //            unsignedXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
            //<Invoice xmlns=""urn:oasis:names:specification:ubl:schema:xsd:Invoice-2""
            //    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
            //    xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
            //    xmlns:cac=""urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2""
            //    xmlns:cbc=""urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2""
            //    xmlns:ext=""urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2""
            //    xmlns:hrextac=""urn:mfin.gov.hr:schema:xsd:HRExtensionAggregateComponents-1"">
            //<ext:UBLExtensions>
            //  <ext:UBLExtension>
            //    <ext:ExtensionContent>
            //        <hrextac:HRFISK20Data>
            //            <hrextac:HRTaxTotal>
            //                <cbc:TaxAmount currencyID=""EUR"">209.54</cbc:TaxAmount>
            //                <hrextac:HRTaxSubtotal>
            //                    <cbc:TaxableAmount currencyID=""EUR"">838.15</cbc:TaxableAmount>
            //                    <cbc:TaxAmount currencyID=""EUR"">209.54</cbc:TaxAmount>
            //                    <hrextac:HRTaxCategory>
            //                        <cbc:ID>S</cbc:ID>
            //                        <cbc:Name>HR:PDV25</cbc:Name>
            //                        <cbc:Percent>25</cbc:Percent>
            //                        <hrextac:HRTaxScheme>
            //                            <cbc:ID>VAT</cbc:ID>
            //                        </hrextac:HRTaxScheme>
            //                    </hrextac:HRTaxCategory>
            //                </hrextac:HRTaxSubtotal>
            //            </hrextac:HRTaxTotal>
            //            <hrextac:HRLegalMonetaryTotal>
            //              <cbc:TaxExclusiveAmount currencyID=""EUR"">0.00</cbc:TaxExclusiveAmount>
            //              <hrextac:OutOfScopeOfVATAmount currencyID=""EUR"">0.00</hrextac:OutOfScopeOfVATAmount>
            //            </hrextac:HRLegalMonetaryTotal>
            //        </hrextac:HRFISK20Data>
            //    </ext:ExtensionContent>
            //  </ext:UBLExtension>
            //</ext:UBLExtensions>
            //<cbc:CustomizationID>urn:cen.eu:en16931:2017#compliant#urn:mfin.gov.hr:cius-2025:1.0#conformant#urn:mfin.gov.hr:ext-2025:1.0</cbc:CustomizationID>
            //<cbc:ProfileID>P7</cbc:ProfileID>
            //<cbc:ID>00003-90-1</cbc:ID>
            //<cbc:CopyIndicator>false</cbc:CopyIndicator>
            //<cbc:IssueDate>2024-01-12</cbc:IssueDate>
            //<cbc:IssueTime>11:52:07</cbc:IssueTime>
            //<cbc:DueDate>2024-01-12</cbc:DueDate>
            //<cbc:InvoiceTypeCode>380</cbc:InvoiceTypeCode>
            //<cbc:Note>

            //  TK: TARA T#AAI#</cbc:Note>
            //<cbc:TaxPointDate>2024-01-05</cbc:TaxPointDate>
            //<cbc:DocumentCurrencyCode>EUR</cbc:DocumentCurrencyCode>
            //<cbc:TaxCurrencyCode>EUR</cbc:TaxCurrencyCode>
            //<cac:AccountingSupplierParty>
            //  <cac:Party>
            //    <cbc:EndpointID schemeID=""9934"">26389058739</cbc:EndpointID>
            //    <cac:PartyIdentification><cbc:ID>9934:26389058739</cbc:ID></cac:PartyIdentification>
            //    <cac:PartyName><cbc:Name>Proba d.o.o.</cbc:Name></cac:PartyName>
            //    <cac:PostalAddress>
            //      <cbc:StreetName>Kapetana Lazarića 1D</cbc:StreetName>
            //      <cbc:CityName>Pazin</cbc:CityName>
            //      <cbc:PostalZone>52000</cbc:PostalZone>
            //      <cac:Country><cbc:IdentificationCode>HR</cbc:IdentificationCode></cac:Country>
            //    </cac:PostalAddress>
            //    <cac:PartyTaxScheme><cbc:CompanyID>HR26389058739</cbc:CompanyID><cac:TaxScheme><cbc:ID>VAT</cbc:ID></cac:TaxScheme></cac:PartyTaxScheme>
            //    <cac:PartyLegalEntity>
            //      <cbc:RegistrationName>AURA SOFT d.o.o. Primjer dugog teksta</cbc:RegistrationName>
            //    </cac:PartyLegalEntity>
            //    <cac:Contact>
            //      <cbc:Telephone>052-621-929</cbc:Telephone>
            //    </cac:Contact>
            //  </cac:Party>
            //  <cac:SellerContact>    <cbc:ID>12345678903</cbc:ID>    <cbc:Name>TARA T</cbc:Name>  </cac:SellerContact></cac:AccountingSupplierParty>
            //<cac:AccountingCustomerParty>
            //  <cac:Party>
            //    <cbc:EndpointID schemeID=""9934"">85821130368</cbc:EndpointID>
            //    <cac:PartyIdentification><cbc:ID>9934:85821130368</cbc:ID></cac:PartyIdentification>
            //    <cac:PartyName><cbc:Name>Partner 3413</cbc:Name></cac:PartyName>
            //    <cac:PostalAddress>
            //      <cbc:StreetName>BAŠTIJANOVA 13</cbc:StreetName>
            //      <cbc:CityName>RIJEKA</cbc:CityName>
            //      <cbc:PostalZone>51000</cbc:PostalZone>
            //      <cac:Country><cbc:IdentificationCode>HR</cbc:IdentificationCode></cac:Country>
            //    </cac:PostalAddress>
            //    <cac:PartyTaxScheme><cbc:CompanyID>HR26389058739</cbc:CompanyID><cac:TaxScheme><cbc:ID>VAT</cbc:ID></cac:TaxScheme></cac:PartyTaxScheme>
            //    <cac:PartyLegalEntity>
            //      <cbc:RegistrationName>Partner 3413</cbc:RegistrationName>
            //    </cac:PartyLegalEntity>
            //  </cac:Party>
            //</cac:AccountingCustomerParty>
            //<cac:PaymentMeans>
            //  <cbc:PaymentMeansCode>30</cbc:PaymentMeansCode>
            //  <cbc:InstructionNote>Transakcijski račun</cbc:InstructionNote>
            //  <cbc:PaymentID>00 103201-90100003-1</cbc:PaymentID>
            //  <cac:PayeeFinancialAccount>
            //    <cbc:ID>2380006-1147002371</cbc:ID>
            //    <cbc:Name>Privredna banka d.d.</cbc:Name>
            //  </cac:PayeeFinancialAccount>
            //</cac:PaymentMeans>
            //<cac:TaxTotal>
            //  <cbc:TaxAmount currencyID=""EUR"">209.54</cbc:TaxAmount>
            //  <cac:TaxSubtotal>
            //    <cbc:TaxableAmount currencyID=""EUR"">838.15</cbc:TaxableAmount>
            //    <cbc:TaxAmount currencyID=""EUR"">209.54</cbc:TaxAmount>
            //    <cac:TaxCategory>
            //      <cbc:ID>S</cbc:ID>
            //      <cbc:Percent>25</cbc:Percent>
            //      <cac:TaxScheme><cbc:ID>VAT</cbc:ID></cac:TaxScheme>
            //    </cac:TaxCategory>
            //  </cac:TaxSubtotal>
            //</cac:TaxTotal>
            //<cac:LegalMonetaryTotal>
            //  <cbc:LineExtensionAmount currencyID=""EUR"">838.15</cbc:LineExtensionAmount>
            //  <cbc:TaxExclusiveAmount currencyID=""EUR"">838.15</cbc:TaxExclusiveAmount>
            //  <cbc:TaxInclusiveAmount currencyID=""EUR"">1047.69</cbc:TaxInclusiveAmount>
            //  <cbc:PayableAmount currencyID=""EUR"">1047.69</cbc:PayableAmount>
            //</cac:LegalMonetaryTotal>
            //<cac:InvoiceLine>
            //  <cbc:ID>1</cbc:ID>
            //  <cbc:InvoicedQuantity unitCode=""H87"">72.0000</cbc:InvoicedQuantity>
            //  <cbc:LineExtensionAmount currencyID=""EUR"">583.26</cbc:LineExtensionAmount>
            //  <cac:AllowanceCharge>
            //    <cbc:ChargeIndicator>false</cbc:ChargeIndicator>
            //    <cbc:AllowanceChargeReason>Popust na stavku (17.00%)</cbc:AllowanceChargeReason>
            //    <cbc:MultiplierFactorNumeric>17.00</cbc:MultiplierFactorNumeric>
            //    <cbc:Amount currencyID=""EUR"">119.46</cbc:Amount>
            //    <cbc:BaseAmount currencyID=""EUR"">702.72</cbc:BaseAmount>
            //  </cac:AllowanceCharge>
            //  <cac:Item>
            //    <cbc:Name>BK SIPROFIX 160 SUPERFLEX C2TE LJEP. 25/1 (48#)</cbc:Name>
            //    <cac:SellersItemIdentification><cbc:ID>000164</cbc:ID></cac:SellersItemIdentification>
            //    <cac:StandardItemIdentification><cbc:ID schemeID=""0160"">1000000000028</cbc:ID></cac:StandardItemIdentification>
            //    <cac:CommodityClassification>
            //      <cbc:ItemClassificationCode listID=""CG"">46.89.00</cbc:ItemClassificationCode>
            //    </cac:CommodityClassification>
            //    <cac:ClassifiedTaxCategory>
            //      <cbc:ID>S</cbc:ID>
            //      <cbc:Percent>25</cbc:Percent>
            //      <cac:TaxScheme><cbc:ID>VAT</cbc:ID></cac:TaxScheme>
            //    </cac:ClassifiedTaxCategory>
            //  </cac:Item>
            //  <cac:Price>
            //    <cbc:PriceAmount currencyID=""EUR"">9.7600</cbc:PriceAmount>
            //    <cbc:BaseQuantity unitCode=""H87"">1</cbc:BaseQuantity>
            //  </cac:Price>
            //</cac:InvoiceLine>
            //<cac:InvoiceLine>
            //  <cbc:ID>2</cbc:ID>
            //  <cbc:InvoicedQuantity unitCode=""H87"">6.0000</cbc:InvoicedQuantity>
            //  <cbc:LineExtensionAmount currencyID=""EUR"">168.52</cbc:LineExtensionAmount>
            //  <cac:AllowanceCharge>
            //    <cbc:ChargeIndicator>false</cbc:ChargeIndicator>
            //    <cbc:AllowanceChargeReason>Popust na stavku (17.00%)</cbc:AllowanceChargeReason>
            //    <cbc:MultiplierFactorNumeric>17.00</cbc:MultiplierFactorNumeric>
            //    <cbc:Amount currencyID=""EUR"">34.52</cbc:Amount>
            //    <cbc:BaseAmount currencyID=""EUR"">203.04</cbc:BaseAmount>
            //  </cac:AllowanceCharge>
            //  <cac:Item>
            //    <cbc:Name>BK HIDROSTOP 2 (16,6+5) 16,6/1</cbc:Name>
            //    <cac:SellersItemIdentification><cbc:ID>000080</cbc:ID></cac:SellersItemIdentification>
            //    <cac:StandardItemIdentification><cbc:ID schemeID=""0160"">8606103352033</cbc:ID></cac:StandardItemIdentification>
            //    <cac:CommodityClassification>
            //      <cbc:ItemClassificationCode listID=""CG"">46.89.00</cbc:ItemClassificationCode>
            //    </cac:CommodityClassification>
            //    <cac:ClassifiedTaxCategory>
            //      <cbc:ID>S</cbc:ID>
            //      <cbc:Percent>25</cbc:Percent>
            //      <cac:TaxScheme><cbc:ID>VAT</cbc:ID></cac:TaxScheme>
            //    </cac:ClassifiedTaxCategory>
            //  </cac:Item>
            //  <cac:Price>
            //    <cbc:PriceAmount currencyID=""EUR"">33.8400</cbc:PriceAmount>
            //    <cbc:BaseQuantity unitCode=""H87"">1</cbc:BaseQuantity>
            //  </cac:Price>
            //</cac:InvoiceLine>
            //<cac:InvoiceLine>
            //  <cbc:ID>3</cbc:ID>
            //  <cbc:InvoicedQuantity unitCode=""H87"">6.0000</cbc:InvoicedQuantity>
            //  <cbc:LineExtensionAmount currencyID=""EUR"">50.60</cbc:LineExtensionAmount>
            //  <cac:AllowanceCharge>
            //    <cbc:ChargeIndicator>false</cbc:ChargeIndicator>
            //    <cbc:AllowanceChargeReason>Popust na stavku (17.00%)</cbc:AllowanceChargeReason>
            //    <cbc:MultiplierFactorNumeric>16.99</cbc:MultiplierFactorNumeric>
            //    <cbc:Amount currencyID=""EUR"">10.36</cbc:Amount>
            //    <cbc:BaseAmount currencyID=""EUR"">60.96</cbc:BaseAmount>
            //  </cac:AllowanceCharge>
            //  <cac:Item>
            //    <cbc:Name>BK EMULZIJA HIDROSTOP 2 (20+5) 5/1</cbc:Name>
            //    <cac:SellersItemIdentification><cbc:ID>000324</cbc:ID></cac:SellersItemIdentification>
            //    <cac:StandardItemIdentification><cbc:ID schemeID=""0160"">8606103352040</cbc:ID></cac:StandardItemIdentification>
            //    <cac:CommodityClassification>
            //      <cbc:ItemClassificationCode listID=""CG"">46.89.00</cbc:ItemClassificationCode>
            //    </cac:CommodityClassification>
            //    <cac:ClassifiedTaxCategory>
            //      <cbc:ID>S</cbc:ID>
            //      <cbc:Percent>25</cbc:Percent>
            //      <cac:TaxScheme><cbc:ID>VAT</cbc:ID></cac:TaxScheme>
            //    </cac:ClassifiedTaxCategory>
            //  </cac:Item>
            //  <cac:Price>
            //    <cbc:PriceAmount currencyID=""EUR"">10.1600</cbc:PriceAmount>
            //    <cbc:BaseQuantity unitCode=""H87"">1</cbc:BaseQuantity>
            //  </cac:Price>
            //</cac:InvoiceLine>
            //<cac:InvoiceLine>
            //  <cbc:ID>4</cbc:ID>
            //  <cbc:InvoicedQuantity unitCode=""H87"">2.0000</cbc:InvoicedQuantity>
            //  <cbc:LineExtensionAmount currencyID=""EUR"">35.77</cbc:LineExtensionAmount>
            //  <cac:AllowanceCharge>
            //    <cbc:ChargeIndicator>false</cbc:ChargeIndicator>
            //    <cbc:AllowanceChargeReason>Popust na stavku (15.00%)</cbc:AllowanceChargeReason>
            //    <cbc:MultiplierFactorNumeric>15.00</cbc:MultiplierFactorNumeric>
            //    <cbc:Amount currencyID=""EUR"">6.31</cbc:Amount>
            //    <cbc:BaseAmount currencyID=""EUR"">42.08</cbc:BaseAmount>
            //  </cac:AllowanceCharge>
            //  <cac:Item>
            //    <cbc:Name>BK PODLOGA  5L</cbc:Name>
            //    <cac:SellersItemIdentification><cbc:ID>000264</cbc:ID></cac:SellersItemIdentification>
            //    <cac:StandardItemIdentification><cbc:ID schemeID=""0160"">8606102367779</cbc:ID></cac:StandardItemIdentification>
            //    <cac:CommodityClassification>
            //      <cbc:ItemClassificationCode listID=""CG"">46.89.00</cbc:ItemClassificationCode>
            //    </cac:CommodityClassification>
            //    <cac:ClassifiedTaxCategory>
            //      <cbc:ID>S</cbc:ID>
            //      <cbc:Percent>25</cbc:Percent>
            //      <cac:TaxScheme><cbc:ID>VAT</cbc:ID></cac:TaxScheme>
            //    </cac:ClassifiedTaxCategory>
            //  </cac:Item>
            //  <cac:Price>
            //    <cbc:PriceAmount currencyID=""EUR"">21.0400</cbc:PriceAmount>
            //    <cbc:BaseQuantity unitCode=""H87"">1</cbc:BaseQuantity>
            //  </cac:Price>
            //</cac:InvoiceLine>
            //</Invoice>
            //";

            //--- od čovika --------------------------------

            //// potpisi e_racun:
            //DigitalSignature dsig = new DigitalSignature();
            //var cert = new X509Certificate2(@"D:\WinX\ERAC\certifikati\p12_aurasoft_demo.p12", "Aurasoft1",
            //        X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);
            //dsig.Certificate = cert;
            //byte[] nepotpisaniRacun = Encoding.ASCII.GetBytes(unsignedXml);
            //byte[] bajtoviPotpisanogRacuna = dsig.SignInvoiceOrCreditNote(nepotpisaniRacun);
            ////System.IO.File.WriteAllBytes(@"C:\potpisani_eracun.xml", bajtoviPotpisanogRacuna);

            //string signedXml = System.Text.Encoding.UTF8.GetString(bajtoviPotpisanogRacuna);

            //return new Variable(signedXml);

            //-----------------------------------

            string signedXml = FinaSoapSigner.SignSoap(
                unsignedXml,
                "C:\\WinX\\certifikati\\p12_aurasoft_demo.p12",
                "Aurasoft1"
            );

            //File.WriteAllText("signed.xml", signedXml);

            return new Variable(signedXml);


        }

        public class FinaSoapSigner
        {
            public static string SignSoap(string unsignedSoapXml, string pfxPath, string pfxPassword)
            {
                // Load cert
                var cert = new X509Certificate2(pfxPath, pfxPassword,
                    X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);

                // Load XML
                var doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(unsignedSoapXml);

                // Namespace manager
                var ns = new XmlNamespaceManager(doc.NameTable);
                ns.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");

                // Find Body
                var body = (XmlElement)doc.SelectSingleNode("//soap:Body", ns);
                if (body == null) throw new Exception("SOAP Body nije pronađen.");

                // Ensure both Id (for SignedXml) and wsu:Id (for WSS) are present
                // 1) add plain Id (SignedXml prepoznaje atribut "Id")
                body.SetAttribute("Id", "Body");

                // 2) add wsu:Id as well (WS-Security expects wsu:Id)
                // ensure wsu namespace is declared at envelope level
                var env = (XmlElement)doc.DocumentElement;
                if (env != null && env.GetAttribute("xmlns:wsu") == string.Empty)
                {
                    env.SetAttribute("xmlns:wsu", "http://schemas.xmlsoap.org/ws/2002/12/utility");
                }
                //body.SetAttribute("wsu:Id", "Body", "http://schemas.xmlsoap.org/ws/2002/12/utility");
                var wsuId = doc.CreateAttribute("wsu", "Id", "http://schemas.xmlsoap.org/ws/2002/12/utility");
                wsuId.Value = "Body";
                body.Attributes.Append(wsuId);

                // Find Header, create Security element
                var header = (XmlElement)doc.SelectSingleNode("//soap:Header", ns);
                if (header == null) throw new Exception("SOAP Header nije pronađen.");

                var security = doc.CreateElement("wsse", "Security", "http://schemas.xmlsoap.org/ws/2002/12/secext");
                // opcionalno: add mustUnderstand if needed
                header.AppendChild(security);

                // BinarySecurityToken
                var bst = doc.CreateElement("wsse", "BinarySecurityToken", "http://schemas.xmlsoap.org/ws/2002/12/secext");
                bst.SetAttribute("EncodingType", "http://schemas.xmlsoap.org/ws/2002/12/secext#Base64Binary");
                bst.SetAttribute("ValueType", "http://schemas.xmlsoap.org/ws/2002/12/secext#X509v3");
                // ensure wsu:Id attribute exists and has correct prefix
                var bstIdAttr = doc.CreateAttribute("wsu", "Id", "http://schemas.xmlsoap.org/ws/2002/12/utility");
                bstIdAttr.Value = "X509Token";
                bst.Attributes.Append(bstIdAttr);
                bst.InnerText = Convert.ToBase64String(cert.RawData);
                security.AppendChild(bst);

                // Prepare SignedXml
                var signedXml = new SignedXml(doc);

                // Use RSA (we already saw cert.GetRSAPrivateKey() is OK)
                RSA rsa = cert.GetRSAPrivateKey();
                if (rsa == null) throw new Exception("Nije pronađen RSA privatni ključ u certifikatu.");
                signedXml.SigningKey = rsa;

                // Ensure signature uses SHA256
                signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA256Url;

                // Create Reference to "#Body"
                var reference = new Reference();
                reference.Uri = "#Body";
                reference.DigestMethod = SignedXml.XmlDsigSHA256Url;
                reference.AddTransform(new XmlDsigExcC14NTransform()); // exclusive canonicalization
                signedXml.AddReference(reference);

                // IMPORTANT: register Id attribute so SignedXml can resolve "#Body"
                // SignedXml looks for attribute named "Id" by default. We already set "Id".
                // But to be robust, explicitly add id attribute mapping for the body element:
                // (AddIdElement available as protected in some implementations; this approach is safe:)
                //signedXml.AddReference(reference); // already added above — no harm repeating

                // Build KeyInfo using SecurityTokenReference pointing to BinarySecurityToken
                var keyInfo = new KeyInfo();
                // create <wsse:SecurityTokenReference><wsse:Reference URI="#X509Token" ValueType="..."/></wsse:SecurityTokenReference>
                var strXml = @"<wsse:SecurityTokenReference xmlns:wsse=""http://schemas.xmlsoap.org/ws/2002/12/secext"">" +
                             @"<wsse:Reference URI=""#X509Token"" ValueType=""http://schemas.xmlsoap.org/ws/2002/12/secext#X509v3""/>" +
                             @"</wsse:SecurityTokenReference>";

                var strDoc = new XmlDocument();
                strDoc.LoadXml(strXml);
                keyInfo.AddClause(new KeyInfoNode(strDoc.DocumentElement));
                signedXml.KeyInfo = keyInfo;

                // Compute signature
                signedXml.ComputeSignature();

                // Get XML for signature and append into wsse:Security
                var xmlDigitalSignature = signedXml.GetXml();
                // The signature element uses ds: namespace by default; append as-is
                security.AppendChild(doc.ImportNode(xmlDigitalSignature, true));

                // Return signed SOAP
                return doc.OuterXml;
            }

            private static XmlElement CreateSecurityTokenReference(XmlDocument doc)
            {
                XmlElement str = doc.CreateElement("wsse", "SecurityTokenReference",
                    "http://schemas.xmlsoap.org/ws/2002/12/secext");

                XmlElement reference = doc.CreateElement("wsse", "Reference",
                    "http://schemas.xmlsoap.org/ws/2002/12/secext");
                reference.SetAttribute("URI", "#X509Token");
                reference.SetAttribute("ValueType",
                    "http://schemas.xmlsoap.org/ws/2002/12/secext#X509v3");

                str.AppendChild(reference);
                return str;
            }
        }

        public static class SoapSigner
        {
            public static string SignSoapEnvelope(string xmlString, string pfxPath, string pfxPassword)
            {
                // Load certificate
                X509Certificate2 cert = new X509Certificate2(pfxPath, pfxPassword,
                    X509KeyStorageFlags.MachineKeySet |
                    X509KeyStorageFlags.Exportable);

                // Load SOAP XML
                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;       // VERY IMPORTANT
                doc.LoadXml(xmlString);

                // Find Body node
                XmlNamespaceManager nsm = new XmlNamespaceManager(doc.NameTable);
                nsm.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");

                XmlElement body = (XmlElement)doc.SelectSingleNode("//soap:Body", nsm);
                if (body == null)
                    throw new Exception("SOAP Body not found.");

                // Add wsu:Id="Body"
                body.SetAttribute("xmlns:wsu", "http://schemas.xmlsoap.org/ws/2002/12/utility");
                body.SetAttribute("wsu:Id", "Body");

                // Create wsse:Security header
                XmlElement header = (XmlElement)doc.SelectSingleNode("//soap:Header", nsm);
                if (header == null)
                    throw new Exception("SOAP Header missing.");

                XmlElement security = doc.CreateElement("wsse", "Security",
                    "http://schemas.xmlsoap.org/ws/2002/12/secext");
                header.AppendChild(security);

                // Create BinarySecurityToken
                XmlElement bst = doc.CreateElement("wsse", "BinarySecurityToken",
                    "http://schemas.xmlsoap.org/ws/2002/12/secext");
                bst.SetAttribute("EncodingType",
                    "http://schemas.xmlsoap.org/ws/2002/12/secext#Base64Binary");
                bst.SetAttribute("ValueType",
                    "http://schemas.xmlsoap.org/ws/2002/12/secext#X509v3");
                bst.SetAttribute("wsu:Id", "MyCert");
                bst.InnerText = Convert.ToBase64String(cert.GetRawCertData());
                security.AppendChild(bst);

                // Create SignedXml object
                SignedXml signedXml = new SignedXml(doc);
                signedXml.SigningKey = cert.PrivateKey;

                // Reference to Body
                Reference reference = new Reference();
                reference.Uri = "#Body";
                reference.AddTransform(new XmlDsigExcC14NTransform());
                signedXml.AddReference(reference);

                // Add KeyInfo pointing to BinarySecurityToken
                KeyInfo keyInfo = new KeyInfo();
                KeyInfoNode refNode = new KeyInfoNode(
                    CreateSecurityTokenReference(doc));
                keyInfo.AddClause(refNode);
                signedXml.KeyInfo = keyInfo;

                // Compute signature
                signedXml.ComputeSignature();

                // Append <Signature> under <wsse:Security>
                XmlElement xmlDigitalSignature = signedXml.GetXml();
                security.AppendChild(xmlDigitalSignature);

                return doc.OuterXml;
            }

            private static XmlElement CreateSecurityTokenReference(XmlDocument doc)
            {
                XmlElement str = doc.CreateElement("wsse", "SecurityTokenReference",
                    "http://schemas.xmlsoap.org/ws/2002/12/secext");

                XmlElement refElem = doc.CreateElement("wsse", "Reference",
                    "http://schemas.xmlsoap.org/ws/2002/12/secext");

                refElem.SetAttribute("URI", "#MyCert");
                refElem.SetAttribute("ValueType",
                    "http://schemas.xmlsoap.org/ws/2002/12/secext#X509v3");

                str.AppendChild(refElem);
                return str;
            }
        }
    }

    class SignXmlFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 3, m_name);

            var unsignedXml = Utils.GetSafeString(args, 0);
            var certificatePath = Utils.GetSafeString(args, 1);
            var certificatePassword = Utils.GetSafeString(args, 2);

            //string signedXml = SignXml(unsignedXml, certificatePath, certificatePassword);

            return new Variable(unsignedXml);
        }

        public string SignXml(string unsignedXml, string certificatePath, string certificatePassword)
        {
            throw new Exception("Not implemented");
        }
    }


    class TestXmlDictFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);

            var unsignedXml = Utils.GetSafeVariable(args, 0);

            return Variable.EmptyInstance;
        }
    }


    //class NixxonSignXmlFunction : ParserFunction
    //{
    //    protected override Variable Evaluate(ParsingScript script)
    //    {
    //        List<Variable> args = script.GetFunctionArgs();
    //        Utils.CheckArgs(args.Count, 1, m_name);

    //        var unsignedXml = Utils.GetSafeString(args, 0);
    //        var certPath = Utils.GetSafeString(args, 1);
    //        var certPass = Utils.GetSafeString(args, 2);

    //        // dohvati potpisni certifikat iz USER certificate store pomocu zadanog thumbprinta:
    //        //X509Store store = new X509Store("MY", StoreLocation.LocalMachine);
    //        //store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
    //        //X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;

    //        //// dohvati potpisni certifikat:
    //        //X509Certificate2Collection fcollection = (X509Certificate2Collection)collection.Find(X509FindType.FindByThumbprint, "OVDJE_STAVI_THUMPRINT_POTPISNOG_CERTIFIKATA!!!", false);
    //        //X509Certificate2 cert = fcollection[0];
    //        //store.Close();

    //        // potpisi e_racun:
    //        DigitalSignature dsig = new DigitalSignature();

    //        X509Certificate2 cert = new X509Certificate2(certPath, certPass,
    //                    X509KeyStorageFlags.Exportable | X509KeyStorageFlags.MachineKeySet);

    //        dsig.Certificate = cert;
    //        byte[] nepotpisaniRacun = Encoding.ASCII.GetBytes(unsignedXml);



    //        byte[] bajtoviPotpisanogRacuna = dsig.SignInvoiceOrCreditNote(nepotpisaniRacun);
    //        var signedXml = Encoding.ASCII.GetString(bajtoviPotpisanogRacuna);

    //        return new Variable(signedXml);
    //    }
    //}


    class ReplaceInJSONFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);

            var json = Utils.GetSafeString(args, 0);

            json = json.Replace("\\\"", "\"");
            json = json.Replace("\\\\", "\\");
            json = json.Replace("\\n", "\n");
            json = json.Replace("\\r", "\r");
            json = json.Replace("\\t", "\t");

            return new Variable(json);
        }
    }


    class IsDoubleFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);

            var stringValue = Utils.GetSafeString(args, 0);

            return new Variable(double.TryParse(stringValue, NumberStyles.Number, CultureInfo.InvariantCulture, out double result));
        }
    }

    class FindColonIndexFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);

            var stringValue = Utils.GetSafeString(args, 0);

            var inString = false;
            var escaped = false;

            for (int i = 0; i < stringValue.Length; i++)
            {
                var c = stringValue.Substring(i, 1);

                if (escaped)
                {
                    escaped = false;
                    continue;
                }

                if (c == "\\" && inString)
                {
                    escaped = true;
                    continue;
                }

                if (c == "\"")
                {
                    inString = !inString;
                }

                if (!inString && c == ":")
                {
                    return new Variable(i);
                }
            }

            return new Variable(-1);
        }
    }


    class ReadAllTextFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);

            var filePath = Utils.GetSafeString(args, 0);

            if (System.IO.File.Exists(filePath))
            {
                return new Variable(System.IO.File.ReadAllText(filePath));
            }
            else
            {
                throw new Exception("File not found: " + filePath);
            }
        }
    }

    class Test99Function : ParserFunction
    {
        //9.
        class WSSignedXml : SignedXml
        {
            public WSSignedXml(XmlDocument doc) : base(doc) { }

            public override XmlElement GetIdElement(XmlDocument doc, string id)
            {
                return doc.SelectSingleNode(
                    $"//*[@wsu:Id='{id}']",
                    CreateNamespaceManager(doc)
                ) as XmlElement;
            }

            XmlNamespaceManager CreateNamespaceManager(XmlDocument doc)
            {
                var nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("wsu",
                    "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");
                return nsmgr;
            }
        }

        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 0, m_name);

            //var soapEnvelope = Utils.GetSafeString(args, 0);

            //3.
            var cert = new X509Certificate2(
                "D:\\WinX\\ERAC\\certifikati\\p12_aurasoft_demo.p12",
                "Aurasoft1",
                X509KeyStorageFlags.MachineKeySet |
                X509KeyStorageFlags.Exportable
            );

            ////4.
            //XmlDocument doc = new XmlDocument();
            //doc.PreserveWhitespace = true;

            //doc.LoadXml(soapEnvelope);


            // 4.2
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;

            doc.LoadXml(@"
<SOAP-ENV:Envelope
    xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/'
    
xmlns:wsu=""http://schemas.xmlsoap.org/ws/2002/12/utility""
xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""

>
  <SOAP-ENV:Header/>
  <SOAP-ENV:Body/>
</SOAP-ENV:Envelope>");
            /*
             xmlns:wsse='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd'
            xmlns:wsu='http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd'
            xmlns:ds='http://www.w3.org/2000/09/xmldsig#'
             */

            //            // 5.
            //            XmlElement body = doc.DocumentElement["SOAP-ENV:Body"];
            //            string bodyId = "id-" + Guid.NewGuid().ToString("N");

            //            body.SetAttribute("wsu:Id",
            //                "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd",
            //                bodyId);

            //            body.InnerXml = @"
            //<EchoMsg xmlns='http://fina.hr/eracun/b2b/pki/Echo/v0.1'>
            //  <HeaderSupplier xmlns='http://fina.hr/eracun/b2b/invoicewebservicecomponents/v0.1'>
            //    <MessageID>" + Guid.NewGuid() + @"</MessageID>
            //    <SupplierID>9934:26389058739</SupplierID>
            //    <MessageType>9999</MessageType>
            //  </HeaderSupplier>
            //  <Data>
            //    <EchoData>
            //      <Echo>hello from .NET</Echo>
            //    </EchoData>
            //  </Data>
            //</EchoMsg>";


            //            var asdasdasd = @"<?xml version=""1.0"" encoding=""UTF-8""?>
            //<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/""                   xmlns:wsu=""http://schemas.xmlsoap.org/ws/2002/12/utility""                   xmlns:xsd=""http://www.w3.org/2001/XMLSchema""                   xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
            //  <SOAP-ENV:Header/>
            //  <SOAP-ENV:Body Id=""Body""                   d2p1:Id=""Body""                   wsu:Id=""id-802e4f37-df38-4974-80c1-a987db752533""                   xmlns:d2p1=""http://schemas.xmlsoap.org/ws/2002/12/utility""                   xmlns:util=""http://schemas.xmlsoap.org/ws/2002/12/utility""                   xmlns:wsu=""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd"">
            //    <SendB2BOutgoingInvoiceMsg>
            //      <HeaderSupplier>
            //        <MessageID>msg-51471a7d-009f-4d3c-b424-8f5e1755afc7</MessageID>
            //        <SupplierID>9934:26389058739</SupplierID>
            //        <MessageType>9001</MessageType>
            //      </HeaderSupplier>
            //      <Data>
            //        <B2BOutgoingInvoiceEnvelope>
            //          <XMLStandard>UBL</XMLStandard>
            //          <SpecificationIdentifier>urn:cen.eu:en16931:2017#compliant#urn:mfin.gov.hr:cius-2025:1.0</SpecificationIdentifier>
            //          <SupplierInvoiceID>90100002</SupplierInvoiceID>
            //          <BuyerID>9934:85821130368</BuyerID>
            //          <InvoiceEnvelope>PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz4KPEludm9pY2UgeG1sbnM9InVybjpvYXNpczpuYW1lczpzcGVjaWZpY2F0aW9uOnVibDpzY2hlbWE6eHNkOkludm9pY2UtMiIKICAgIHhtbG5zOnhzaT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS9YTUxTY2hlbWEtaW5zdGFuY2UiCiAgICB4bWxuczp4c2Q9Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvWE1MU2NoZW1hIgogICAgeG1sbnM6Y2FjPSJ1cm46b2FzaXM6bmFtZXM6c3BlY2lmaWNhdGlvbjp1Ymw6c2NoZW1hOnhzZDpDb21tb25BZ2dyZWdhdGVDb21wb25lbnRzLTIiCiAgICB4bWxuczpjYmM9InVybjpvYXNpczpuYW1lczpzcGVjaWZpY2F0aW9uOnVibDpzY2hlbWE6eHNkOkNvbW1vbkJhc2ljQ29tcG9uZW50cy0yIgogICAgeG1sbnM6ZXh0PSJ1cm46b2FzaXM6bmFtZXM6c3BlY2lmaWNhdGlvbjp1Ymw6c2NoZW1hOnhzZDpDb21tb25FeHRlbnNpb25Db21wb25lbnRzLTIiCiAgICB4bWxuczpzYWM9InVybjpvYXNpczpuYW1lczpzcGVjaWZpY2F0aW9uOnVibDpzY2hlbWE6eHNkOlNpZ25hdHVyZUFnZ3JlZ2F0ZUNvbXBvbmVudHMtMiIgCiAgICB4bWxuczpzaWc9InVybjpvYXNpczpuYW1lczpzcGVjaWZpY2F0aW9uOnVibDpzY2hlbWE6eHNkOkNvbW1vblNpZ25hdHVyZUNvbXBvbmVudHMtMiIgCiAgICB4bWxuczpocmV4dGFjPSJ1cm46bWZpbi5nb3YuaHI6c2NoZW1hOnhzZDpIUkV4dGVuc2lvbkFnZ3JlZ2F0ZUNvbXBvbmVudHMtMSI+CjxleHQ6VUJMRXh0ZW5zaW9ucz4KICA8ZXh0OlVCTEV4dGVuc2lvbj4KICAgIDxleHQ6RXh0ZW5zaW9uQ29udGVudD4KICAgICAgICA8aHJleHRhYzpIUkZJU0syMERhdGE+CiAgICAgICAgICAgIDxocmV4dGFjOkhSVGF4VG90YWw+CiAgICAgICAgICAgICAgICA8Y2JjOlRheEFtb3VudCBjdXJyZW5jeUlEPSJFVVIiPjY5LjIxPC9jYmM6VGF4QW1vdW50PgogICAgICAgICAgICAgICAgPGhyZXh0YWM6SFJUYXhTdWJ0b3RhbD4KICAgICAgICAgICAgICAgICAgICA8Y2JjOlRheGFibGVBbW91bnQgY3VycmVuY3lJRD0iRVVSIj4yNzYuODQ8L2NiYzpUYXhhYmxlQW1vdW50PgogICAgICAgICAgICAgICAgICAgIDxjYmM6VGF4QW1vdW50IGN1cnJlbmN5SUQ9IkVVUiI+NjkuMjE8L2NiYzpUYXhBbW91bnQ+CiAgICAgICAgICAgICAgICAgICAgPGhyZXh0YWM6SFJUYXhDYXRlZ29yeT4KICAgICAgICAgICAgICAgICAgICAgICAgPGNiYzpJRD5TPC9jYmM6SUQ+CiAgICAgICAgICAgICAgICAgICAgICAgIDxjYmM6TmFtZT5IUjpQRFYyNTwvY2JjOk5hbWU+CiAgICAgICAgICAgICAgICAgICAgICAgIDxjYmM6UGVyY2VudD4yNTwvY2JjOlBlcmNlbnQ+CiAgICAgICAgICAgICAgICAgICAgICAgIDxocmV4dGFjOkhSVGF4U2NoZW1lPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgPGNiYzpJRD5WQVQ8L2NiYzpJRD4KICAgICAgICAgICAgICAgICAgICAgICAgPC9ocmV4dGFjOkhSVGF4U2NoZW1lPgogICAgICAgICAgICAgICAgICAgIDwvaHJleHRhYzpIUlRheENhdGVnb3J5PgogICAgICAgICAgICAgICAgPC9ocmV4dGFjOkhSVGF4U3VidG90YWw+CiAgICAgICAgICAgIDwvaHJleHRhYzpIUlRheFRvdGFsPgogICAgICAgICAgICA8aHJleHRhYzpIUkxlZ2FsTW9uZXRhcnlUb3RhbD4KICAgICAgICAgICAgICA8Y2JjOlRheEV4Y2x1c2l2ZUFtb3VudCBjdXJyZW5jeUlEPSJFVVIiPjI3Ni44NDwvY2JjOlRheEV4Y2x1c2l2ZUFtb3VudD4KICAgICAgICAgICAgICA8aHJleHRhYzpPdXRPZlNjb3BlT2ZWQVRBbW91bnQgY3VycmVuY3lJRD0iRVVSIj4wLjAwPC9ocmV4dGFjOk91dE9mU2NvcGVPZlZBVEFtb3VudD4KICAgICAgICAgICAgPC9ocmV4dGFjOkhSTGVnYWxNb25ldGFyeVRvdGFsPgogICAgICAgIDwvaHJleHRhYzpIUkZJU0syMERhdGE+CiAgICA8L2V4dDpFeHRlbnNpb25Db250ZW50PgogIDwvZXh0OlVCTEV4dGVuc2lvbj4KPC9leHQ6VUJMRXh0ZW5zaW9ucz4KPGNiYzpDdXN0b21pemF0aW9uSUQ+dXJuOmNlbi5ldTplbjE2OTMxOjIwMTcjY29tcGxpYW50I3VybjptZmluLmdvdi5ocjpjaXVzLTIwMjU6MS4wI2NvbmZvcm1hbnQjdXJuOm1maW4uZ292LmhyOmV4dC0yMDI1OjEuMDwvY2JjOkN1c3RvbWl6YXRpb25JRD4KPGNiYzpQcm9maWxlSUQ+UDc8L2NiYzpQcm9maWxlSUQ+CjxjYmM6SUQ+MDAwMDItOTAtMTwvY2JjOklEPgo8Y2JjOkNvcHlJbmRpY2F0b3I+ZmFsc2U8L2NiYzpDb3B5SW5kaWNhdG9yPgo8Y2JjOklzc3VlRGF0ZT4yMDI0LTAxLTEyPC9jYmM6SXNzdWVEYXRlPgo8Y2JjOklzc3VlVGltZT4xMTo0ODo1ODwvY2JjOklzc3VlVGltZT4KPGNiYzpEdWVEYXRlPjIwMjQtMDEtMTI8L2NiYzpEdWVEYXRlPgo8Y2JjOkludm9pY2VUeXBlQ29kZT4zODA8L2NiYzpJbnZvaWNlVHlwZUNvZGU+CjxjYmM6Tm90ZT4KCiAgVEs6IFRBUkEgVCNBQUkjPC9jYmM6Tm90ZT4KPGNiYzpUYXhQb2ludERhdGU+MjAyNC0wMS0xMTwvY2JjOlRheFBvaW50RGF0ZT4KPGNiYzpEb2N1bWVudEN1cnJlbmN5Q29kZT5FVVI8L2NiYzpEb2N1bWVudEN1cnJlbmN5Q29kZT4KPGNiYzpUYXhDdXJyZW5jeUNvZGU+RVVSPC9jYmM6VGF4Q3VycmVuY3lDb2RlPgo8Y2FjOkFjY291bnRpbmdTdXBwbGllclBhcnR5PgogIDxjYWM6UGFydHk+CiAgICA8Y2JjOkVuZHBvaW50SUQgc2NoZW1lSUQ9Ijk5MzQiPjI2Mzg5MDU4NzM5PC9jYmM6RW5kcG9pbnRJRD4KICAgIDxjYWM6UGFydHlJZGVudGlmaWNhdGlvbj48Y2JjOklEPjk5MzQ6MjYzODkwNTg3Mzk8L2NiYzpJRD48L2NhYzpQYXJ0eUlkZW50aWZpY2F0aW9uPgogICAgPGNhYzpQYXJ0eU5hbWU+PGNiYzpOYW1lPlByb2JhIGQuby5vLjwvY2JjOk5hbWU+PC9jYWM6UGFydHlOYW1lPgogICAgPGNhYzpQb3N0YWxBZGRyZXNzPgogICAgICA8Y2JjOlN0cmVldE5hbWU+S2FwZXRhbmEgTGF6YXJpxIdhIDFEPC9jYmM6U3RyZWV0TmFtZT4KICAgICAgPGNiYzpDaXR5TmFtZT5QYXppbjwvY2JjOkNpdHlOYW1lPgogICAgICA8Y2JjOlBvc3RhbFpvbmU+NTIwMDA8L2NiYzpQb3N0YWxab25lPgogICAgICA8Y2FjOkNvdW50cnk+PGNiYzpJZGVudGlmaWNhdGlvbkNvZGU+SFI8L2NiYzpJZGVudGlmaWNhdGlvbkNvZGU+PC9jYWM6Q291bnRyeT4KICAgIDwvY2FjOlBvc3RhbEFkZHJlc3M+CiAgICA8Y2FjOlBhcnR5VGF4U2NoZW1lPjxjYmM6Q29tcGFueUlEPkhSMjYzODkwNTg3Mzk8L2NiYzpDb21wYW55SUQ+PGNhYzpUYXhTY2hlbWU+PGNiYzpJRD5WQVQ8L2NiYzpJRD48L2NhYzpUYXhTY2hlbWU+PC9jYWM6UGFydHlUYXhTY2hlbWU+CiAgICA8Y2FjOlBhcnR5TGVnYWxFbnRpdHk+CiAgICAgIDxjYmM6UmVnaXN0cmF0aW9uTmFtZT5BVVJBIFNPRlQgZC5vLm8uIFByaW1qZXIgZHVnb2cgdGVrc3RhPC9jYmM6UmVnaXN0cmF0aW9uTmFtZT4KICAgIDwvY2FjOlBhcnR5TGVnYWxFbnRpdHk+CiAgICA8Y2FjOkNvbnRhY3Q+CiAgICAgIDxjYmM6VGVsZXBob25lPjA1Mi02MjEtOTI5PC9jYmM6VGVsZXBob25lPgogICAgPC9jYWM6Q29udGFjdD4KICA8L2NhYzpQYXJ0eT4KICA8Y2FjOlNlbGxlckNvbnRhY3Q+ICAgIDxjYmM6SUQ+MTIzNDU2Nzg5MDM8L2NiYzpJRD4gICAgPGNiYzpOYW1lPlRBUkEgVDwvY2JjOk5hbWU+ICA8L2NhYzpTZWxsZXJDb250YWN0PjwvY2FjOkFjY291bnRpbmdTdXBwbGllclBhcnR5Pgo8Y2FjOkFjY291bnRpbmdDdXN0b21lclBhcnR5PgogIDxjYWM6UGFydHk+CiAgICA8Y2JjOkVuZHBvaW50SUQgc2NoZW1lSUQ9Ijk5MzQiPjI2Mzg5MDU4NzM5PC9jYmM6RW5kcG9pbnRJRD4KICAgIDxjYWM6UGFydHlJZGVudGlmaWNhdGlvbj48Y2JjOklEPjk5MzQ6MjYzODkwNTg3Mzk8L2NiYzpJRD48L2NhYzpQYXJ0eUlkZW50aWZpY2F0aW9uPgogICAgPGNhYzpQYXJ0eU5hbWU+PGNiYzpOYW1lPlBhcnRuZXIgMzQxMzwvY2JjOk5hbWU+PC9jYWM6UGFydHlOYW1lPgogICAgPGNhYzpQb3N0YWxBZGRyZXNzPgogICAgICA8Y2JjOlN0cmVldE5hbWU+QkHFoFRJSkFOT1ZBIDEzPC9jYmM6U3RyZWV0TmFtZT4KICAgICAgPGNiYzpDaXR5TmFtZT5SSUpFS0E8L2NiYzpDaXR5TmFtZT4KICAgICAgPGNiYzpQb3N0YWxab25lPjUxMDAwPC9jYmM6UG9zdGFsWm9uZT4KICAgICAgPGNhYzpDb3VudHJ5PjxjYmM6SWRlbnRpZmljYXRpb25Db2RlPkhSPC9jYmM6SWRlbnRpZmljYXRpb25Db2RlPjwvY2FjOkNvdW50cnk+CiAgICA8L2NhYzpQb3N0YWxBZGRyZXNzPgogICAgPGNhYzpQYXJ0eVRheFNjaGVtZT48Y2JjOkNvbXBhbnlJRD5IUjI2Mzg5MDU4NzM5PC9jYmM6Q29tcGFueUlEPjxjYWM6VGF4U2NoZW1lPjxjYmM6SUQ+VkFUPC9jYmM6SUQ+PC9jYWM6VGF4U2NoZW1lPjwvY2FjOlBhcnR5VGF4U2NoZW1lPgogICAgPGNhYzpQYXJ0eUxlZ2FsRW50aXR5PgogICAgICA8Y2JjOlJlZ2lzdHJhdGlvbk5hbWU+UGFydG5lciAzNDEzPC9jYmM6UmVnaXN0cmF0aW9uTmFtZT4KICAgIDwvY2FjOlBhcnR5TGVnYWxFbnRpdHk+CiAgPC9jYWM6UGFydHk+CjwvY2FjOkFjY291bnRpbmdDdXN0b21lclBhcnR5Pgo8Y2FjOkRlbGl2ZXJ5PgogIDxjYWM6RGVsaXZlcnlMb2NhdGlvbj4KICAgIDxjYWM6QWRkcmVzcz4KICAgICAgPGNiYzpTdHJlZXROYW1lPkJBxaBUSUpBTk9WQSAxMzwvY2JjOlN0cmVldE5hbWU+CiAgICAgIDxjYmM6Q2l0eU5hbWU+UklKRUtBPC9jYmM6Q2l0eU5hbWU+CiAgICAgIDxjYmM6UG9zdGFsWm9uZT41MTAwMDwvY2JjOlBvc3RhbFpvbmU+CiAgICAgIDxjYWM6Q291bnRyeT4KICAgICAgICA8Y2JjOklkZW50aWZpY2F0aW9uQ29kZT5IUjwvY2JjOklkZW50aWZpY2F0aW9uQ29kZT4KICAgICAgPC9jYWM6Q291bnRyeT4KICAgIDwvY2FjOkFkZHJlc3M+CiAgPC9jYWM6RGVsaXZlcnlMb2NhdGlvbj4KICA8Y2FjOkRlbGl2ZXJ5UGFydHk+CiAgICA8Y2FjOlBhcnR5TmFtZT4KICAgICAgPGNiYzpOYW1lPlBhcnRuZXIgMzQxMzwvY2JjOk5hbWU+CiAgICA8L2NhYzpQYXJ0eU5hbWU+CiAgPC9jYWM6RGVsaXZlcnlQYXJ0eT4KPC9jYWM6RGVsaXZlcnk+CjxjYWM6UGF5bWVudE1lYW5zPgogIDxjYmM6UGF5bWVudE1lYW5zQ29kZT4zMDwvY2JjOlBheW1lbnRNZWFuc0NvZGU+CiAgPGNiYzpJbnN0cnVjdGlvbk5vdGU+VHJhbnNha2NpanNraSByYcSNdW48L2NiYzpJbnN0cnVjdGlvbk5vdGU+CiAgPGNiYzpQYXltZW50SUQ+MDAgMTAzMjAxLTkwMTAwMDAyLTE8L2NiYzpQYXltZW50SUQ+CiAgPGNhYzpQYXllZUZpbmFuY2lhbEFjY291bnQ+CiAgICA8Y2JjOklEPjIzODAwMDYtMTE0NzAwMjM3MTwvY2JjOklEPgogICAgPGNiYzpOYW1lPlByaXZyZWRuYSBiYW5rYSBkLmQuPC9jYmM6TmFtZT4KICA8L2NhYzpQYXllZUZpbmFuY2lhbEFjY291bnQ+CjwvY2FjOlBheW1lbnRNZWFucz4KICAgICAgICAgICAgPGNhYzpUYXhUb3RhbD4KICAgICAgICAgICAgICAgIDxjYmM6VGF4QW1vdW50IGN1cnJlbmN5SUQ9IkVVUiI+NjkuMjE8L2NiYzpUYXhBbW91bnQ+CiAgICAgICAgICAgICAgICA8Y2FjOlRheFN1YnRvdGFsPgogICAgICAgICAgICAgICAgICAgIDxjYmM6VGF4YWJsZUFtb3VudCBjdXJyZW5jeUlEPSJFVVIiPjI3Ni44NDwvY2JjOlRheGFibGVBbW91bnQ+CiAgICAgICAgICAgICAgICAgICAgPGNiYzpUYXhBbW91bnQgY3VycmVuY3lJRD0iRVVSIj42OS4yMTwvY2JjOlRheEFtb3VudD4KICAgICAgICAgICAgICAgICAgICA8Y2FjOlRheENhdGVnb3J5PgogICAgICAgICAgICAgICAgICAgICAgICA8Y2JjOklEPlM8L2NiYzpJRD4KICAgICAgICAgICAgICAgICAgICAgICAgPGNiYzpOYW1lPkhSOlBEVjI1PC9jYmM6TmFtZT4KICAgICAgICAgICAgICAgICAgICAgICAgPGNiYzpQZXJjZW50PjI1PC9jYmM6UGVyY2VudD4KICAgICAgICAgICAgICAgICAgICAgICAgPGNhYzpUYXhTY2hlbWU+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8Y2JjOklEPlZBVDwvY2JjOklEPgogICAgICAgICAgICAgICAgICAgICAgICA8L2NhYzpUYXhTY2hlbWU+CiAgICAgICAgICAgICAgICAgICAgPC9jYWM6VGF4Q2F0ZWdvcnk+CiAgICAgICAgICAgICAgICA8L2NhYzpUYXhTdWJ0b3RhbD4KICAgICAgICAgICAgPC9jYWM6VGF4VG90YWw+CjxjYWM6TGVnYWxNb25ldGFyeVRvdGFsPgogIDxjYmM6TGluZUV4dGVuc2lvbkFtb3VudCBjdXJyZW5jeUlEPSJFVVIiPjI3Ni44NDwvY2JjOkxpbmVFeHRlbnNpb25BbW91bnQ+CiAgPGNiYzpUYXhFeGNsdXNpdmVBbW91bnQgY3VycmVuY3lJRD0iRVVSIj4yNzYuODQ8L2NiYzpUYXhFeGNsdXNpdmVBbW91bnQ+CiAgPGNiYzpUYXhJbmNsdXNpdmVBbW91bnQgY3VycmVuY3lJRD0iRVVSIj4zNDYuMDU8L2NiYzpUYXhJbmNsdXNpdmVBbW91bnQ+CiAgPGNiYzpQYXlhYmxlQW1vdW50IGN1cnJlbmN5SUQ9IkVVUiI+MzQ2LjA1PC9jYmM6UGF5YWJsZUFtb3VudD4KPC9jYWM6TGVnYWxNb25ldGFyeVRvdGFsPgo8Y2FjOkludm9pY2VMaW5lPgogIDxjYmM6SUQ+MTwvY2JjOklEPgogIDxjYmM6SW52b2ljZWRRdWFudGl0eSB1bml0Q29kZT0iSDg3Ij43LjAwMDA8L2NiYzpJbnZvaWNlZFF1YW50aXR5PgogIDxjYmM6TGluZUV4dGVuc2lvbkFtb3VudCBjdXJyZW5jeUlEPSJFVVIiPjU2LjY0PC9jYmM6TGluZUV4dGVuc2lvbkFtb3VudD4KICA8Y2FjOkFsbG93YW5jZUNoYXJnZT4KICAgIDxjYmM6Q2hhcmdlSW5kaWNhdG9yPmZhbHNlPC9jYmM6Q2hhcmdlSW5kaWNhdG9yPgogICAgPGNiYzpBbGxvd2FuY2VDaGFyZ2VSZWFzb24+UG9wdXN0IG5hIHN0YXZrdSAoMTUuMDAlKTwvY2JjOkFsbG93YW5jZUNoYXJnZVJlYXNvbj4KICAgIDxjYmM6TXVsdGlwbGllckZhY3Rvck51bWVyaWM+MTUuMDE8L2NiYzpNdWx0aXBsaWVyRmFjdG9yTnVtZXJpYz4KICAgIDxjYmM6QW1vdW50IGN1cnJlbmN5SUQ9IkVVUiI+MTAuMDA8L2NiYzpBbW91bnQ+CiAgICA8Y2JjOkJhc2VBbW91bnQgY3VycmVuY3lJRD0iRVVSIj42Ni42NDwvY2JjOkJhc2VBbW91bnQ+CiAgPC9jYWM6QWxsb3dhbmNlQ2hhcmdlPgogIDxjYWM6SXRlbT4KICAgIDxjYmM6TmFtZT5GVUdBQkVMTEEgQ09MT1IgMDUgR1JJR0lPIExVQ0UgM0tHPC9jYmM6TmFtZT4KICAgIDxjYWM6U2VsbGVyc0l0ZW1JZGVudGlmaWNhdGlvbj48Y2JjOklEPjAxMDI4MzwvY2JjOklEPjwvY2FjOlNlbGxlcnNJdGVtSWRlbnRpZmljYXRpb24+CiAgICA8Y2FjOlN0YW5kYXJkSXRlbUlkZW50aWZpY2F0aW9uPjxjYmM6SUQgc2NoZW1lSUQ9IjAxNjAiPjgwMjE3MDQxNTUzNzk8L2NiYzpJRD48L2NhYzpTdGFuZGFyZEl0ZW1JZGVudGlmaWNhdGlvbj4KICAgIDxjYWM6Q29tbW9kaXR5Q2xhc3NpZmljYXRpb24+CiAgICAgIDxjYmM6SXRlbUNsYXNzaWZpY2F0aW9uQ29kZSBsaXN0SUQ9IkNHIj40Ni44OS4wMDwvY2JjOkl0ZW1DbGFzc2lmaWNhdGlvbkNvZGU+CiAgICA8L2NhYzpDb21tb2RpdHlDbGFzc2lmaWNhdGlvbj4KICAgIDxjYWM6Q2xhc3NpZmllZFRheENhdGVnb3J5PgogICAgICA8Y2JjOklEPlM8L2NiYzpJRD4KICAgICAgPGNiYzpQZXJjZW50PjI1PC9jYmM6UGVyY2VudD4KICAgICAgPGNhYzpUYXhTY2hlbWU+PGNiYzpJRD5WQVQ8L2NiYzpJRD48L2NhYzpUYXhTY2hlbWU+CiAgICA8L2NhYzpDbGFzc2lmaWVkVGF4Q2F0ZWdvcnk+CiAgPC9jYWM6SXRlbT4KICA8Y2FjOlByaWNlPgogICAgPGNiYzpQcmljZUFtb3VudCBjdXJyZW5jeUlEPSJFVVIiPjkuNTIwMDwvY2JjOlByaWNlQW1vdW50PgogICAgPGNiYzpCYXNlUXVhbnRpdHkgdW5pdENvZGU9Ikg4NyI+MTwvY2JjOkJhc2VRdWFudGl0eT4KICA8L2NhYzpQcmljZT4KPC9jYWM6SW52b2ljZUxpbmU+CjxjYWM6SW52b2ljZUxpbmU+CiAgPGNiYzpJRD4yPC9jYmM6SUQ+CiAgPGNiYzpJbnZvaWNlZFF1YW50aXR5IHVuaXRDb2RlPSJIODciPjEuMDAwMDwvY2JjOkludm9pY2VkUXVhbnRpdHk+CiAgPGNiYzpMaW5lRXh0ZW5zaW9uQW1vdW50IGN1cnJlbmN5SUQ9IkVVUiI+Ni44MDwvY2JjOkxpbmVFeHRlbnNpb25BbW91bnQ+CiAgPGNhYzpBbGxvd2FuY2VDaGFyZ2U+CiAgICA8Y2JjOkNoYXJnZUluZGljYXRvcj5mYWxzZTwvY2JjOkNoYXJnZUluZGljYXRvcj4KICAgIDxjYmM6QWxsb3dhbmNlQ2hhcmdlUmVhc29uPlBvcHVzdCBuYSBzdGF2a3UgKDE1LjAwJSk8L2NiYzpBbGxvd2FuY2VDaGFyZ2VSZWFzb24+CiAgICA8Y2JjOk11bHRpcGxpZXJGYWN0b3JOdW1lcmljPjE1LjAwPC9jYmM6TXVsdGlwbGllckZhY3Rvck51bWVyaWM+CiAgICA8Y2JjOkFtb3VudCBjdXJyZW5jeUlEPSJFVVIiPjEuMjA8L2NiYzpBbW91bnQ+CiAgICA8Y2JjOkJhc2VBbW91bnQgY3VycmVuY3lJRD0iRVVSIj44LjAwPC9jYmM6QmFzZUFtb3VudD4KICA8L2NhYzpBbGxvd2FuY2VDaGFyZ2U+CiAgPGNhYzpJdGVtPgogICAgPGNiYzpOYW1lPlBPU1VEQSBaQSDFvUJVS1UgT0tSVUdMQSA1NUw8L2NiYzpOYW1lPgogICAgPGNhYzpTZWxsZXJzSXRlbUlkZW50aWZpY2F0aW9uPjxjYmM6SUQ+MDE2MDAwPC9jYmM6SUQ+PC9jYWM6U2VsbGVyc0l0ZW1JZGVudGlmaWNhdGlvbj4KICAgIDxjYWM6U3RhbmRhcmRJdGVtSWRlbnRpZmljYXRpb24+PGNiYzpJRCBzY2hlbWVJRD0iMDE2MCI+Mzg1NjAwMzY4ODk3NzwvY2JjOklEPjwvY2FjOlN0YW5kYXJkSXRlbUlkZW50aWZpY2F0aW9uPgogICAgPGNhYzpDb21tb2RpdHlDbGFzc2lmaWNhdGlvbj4KICAgICAgPGNiYzpJdGVtQ2xhc3NpZmljYXRpb25Db2RlIGxpc3RJRD0iQ0ciPjQ2Ljg5LjAwPC9jYmM6SXRlbUNsYXNzaWZpY2F0aW9uQ29kZT4KICAgIDwvY2FjOkNvbW1vZGl0eUNsYXNzaWZpY2F0aW9uPgogICAgPGNhYzpDbGFzc2lmaWVkVGF4Q2F0ZWdvcnk+CiAgICAgIDxjYmM6SUQ+UzwvY2JjOklEPgogICAgICA8Y2JjOlBlcmNlbnQ+MjU8L2NiYzpQZXJjZW50PgogICAgICA8Y2FjOlRheFNjaGVtZT48Y2JjOklEPlZBVDwvY2JjOklEPjwvY2FjOlRheFNjaGVtZT4KICAgIDwvY2FjOkNsYXNzaWZpZWRUYXhDYXRlZ29yeT4KICA8L2NhYzpJdGVtPgogIDxjYWM6UHJpY2U+CiAgICA8Y2JjOlByaWNlQW1vdW50IGN1cnJlbmN5SUQ9IkVVUiI+OC4wMDAwPC9jYmM6UHJpY2VBbW91bnQ+CiAgICA8Y2JjOkJhc2VRdWFudGl0eSB1bml0Q29kZT0iSDg3Ij4xPC9jYmM6QmFzZVF1YW50aXR5PgogIDwvY2FjOlByaWNlPgo8L2NhYzpJbnZvaWNlTGluZT4KPGNhYzpJbnZvaWNlTGluZT4KICA8Y2JjOklEPjM8L2NiYzpJRD4KICA8Y2JjOkludm9pY2VkUXVhbnRpdHkgdW5pdENvZGU9Ikg4NyI+MS4wMDAwPC9jYmM6SW52b2ljZWRRdWFudGl0eT4KICA8Y2JjOkxpbmVFeHRlbnNpb25BbW91bnQgY3VycmVuY3lJRD0iRVVSIj40NC4yMDwvY2JjOkxpbmVFeHRlbnNpb25BbW91bnQ+CiAgPGNhYzpBbGxvd2FuY2VDaGFyZ2U+CiAgICA8Y2JjOkNoYXJnZUluZGljYXRvcj5mYWxzZTwvY2JjOkNoYXJnZUluZGljYXRvcj4KICAgIDxjYmM6QWxsb3dhbmNlQ2hhcmdlUmVhc29uPlBvcHVzdCBuYSBzdGF2a3UgKDE1LjAwJSk8L2NiYzpBbGxvd2FuY2VDaGFyZ2VSZWFzb24+CiAgICA8Y2JjOk11bHRpcGxpZXJGYWN0b3JOdW1lcmljPjE1LjAwPC9jYmM6TXVsdGlwbGllckZhY3Rvck51bWVyaWM+CiAgICA8Y2JjOkFtb3VudCBjdXJyZW5jeUlEPSJFVVIiPjcuODA8L2NiYzpBbW91bnQ+CiAgICA8Y2JjOkJhc2VBbW91bnQgY3VycmVuY3lJRD0iRVVSIj41Mi4wMDwvY2JjOkJhc2VBbW91bnQ+CiAgPC9jYWM6QWxsb3dhbmNlQ2hhcmdlPgogIDxjYWM6SXRlbT4KICAgIDxjYmM6TmFtZT5MRVRWQSBBTFVNLiBTIE9LT00gSSBSVUtPSFZBVE9NIFNMWEcgMjwvY2JjOk5hbWU+CiAgICA8Y2FjOlNlbGxlcnNJdGVtSWRlbnRpZmljYXRpb24+PGNiYzpJRD4wMDEwOTM8L2NiYzpJRD48L2NhYzpTZWxsZXJzSXRlbUlkZW50aWZpY2F0aW9uPgogICAgPGNhYzpTdGFuZGFyZEl0ZW1JZGVudGlmaWNhdGlvbj48Y2JjOklEIHNjaGVtZUlEPSIwMTYwIj45MDAyNzE5MDA4NDIyPC9jYmM6SUQ+PC9jYWM6U3RhbmRhcmRJdGVtSWRlbnRpZmljYXRpb24+CiAgICA8Y2FjOkNvbW1vZGl0eUNsYXNzaWZpY2F0aW9uPgogICAgICA8Y2JjOkl0ZW1DbGFzc2lmaWNhdGlvbkNvZGUgbGlzdElEPSJDRyI+NDYuODkuMDA8L2NiYzpJdGVtQ2xhc3NpZmljYXRpb25Db2RlPgogICAgPC9jYWM6Q29tbW9kaXR5Q2xhc3NpZmljYXRpb24+CiAgICA8Y2FjOkNsYXNzaWZpZWRUYXhDYXRlZ29yeT4KICAgICAgPGNiYzpJRD5TPC9jYmM6SUQ+CiAgICAgIDxjYmM6UGVyY2VudD4yNTwvY2JjOlBlcmNlbnQ+CiAgICAgIDxjYWM6VGF4U2NoZW1lPjxjYmM6SUQ+VkFUPC9jYmM6SUQ+PC9jYWM6VGF4U2NoZW1lPgogICAgPC9jYWM6Q2xhc3NpZmllZFRheENhdGVnb3J5PgogIDwvY2FjOkl0ZW0+CiAgPGNhYzpQcmljZT4KICAgIDxjYmM6UHJpY2VBbW91bnQgY3VycmVuY3lJRD0iRVVSIj41Mi4wMDAwPC9jYmM6UHJpY2VBbW91bnQ+CiAgICA8Y2JjOkJhc2VRdWFudGl0eSB1bml0Q29kZT0iSDg3Ij4xPC9jYmM6QmFzZVF1YW50aXR5PgogIDwvY2FjOlByaWNlPgo8L2NhYzpJbnZvaWNlTGluZT4KPGNhYzpJbnZvaWNlTGluZT4KICA8Y2JjOklEPjQ8L2NiYzpJRD4KICA8Y2JjOkludm9pY2VkUXVhbnRpdHkgdW5pdENvZGU9Ikg4NyI+MS4wMDAwPC9jYmM6SW52b2ljZWRRdWFudGl0eT4KICA8Y2JjOkxpbmVFeHRlbnNpb25BbW91bnQgY3VycmVuY3lJRD0iRVVSIj4xNjkuMjA8L2NiYzpMaW5lRXh0ZW5zaW9uQW1vdW50PgogIDxjYWM6QWxsb3dhbmNlQ2hhcmdlPgogICAgPGNiYzpDaGFyZ2VJbmRpY2F0b3I+ZmFsc2U8L2NiYzpDaGFyZ2VJbmRpY2F0b3I+CiAgICA8Y2JjOkFsbG93YW5jZUNoYXJnZVJlYXNvbj5Qb3B1c3QgbmEgc3Rhdmt1ICgxMC4wMCUpPC9jYmM6QWxsb3dhbmNlQ2hhcmdlUmVhc29uPgogICAgPGNiYzpNdWx0aXBsaWVyRmFjdG9yTnVtZXJpYz4xMC4wMDwvY2JjOk11bHRpcGxpZXJGYWN0b3JOdW1lcmljPgogICAgPGNiYzpBbW91bnQgY3VycmVuY3lJRD0iRVVSIj4xOC44MDwvY2JjOkFtb3VudD4KICAgIDxjYmM6QmFzZUFtb3VudCBjdXJyZW5jeUlEPSJFVVIiPjE4OC4wMDwvY2JjOkJhc2VBbW91bnQ+CiAgPC9jYWM6QWxsb3dhbmNlQ2hhcmdlPgogIDxjYWM6SXRlbT4KICAgIDxjYmM6TmFtZT5URVNMRVIgTUpFxaBBxIwgWiBCT0pVIFBST0ZFU0lPTkFMIDE2MDBXPC9jYmM6TmFtZT4KICAgIDxjYWM6U2VsbGVyc0l0ZW1JZGVudGlmaWNhdGlvbj48Y2JjOklEPjAwODY5MzwvY2JjOklEPjwvY2FjOlNlbGxlcnNJdGVtSWRlbnRpZmljYXRpb24+CiAgICA8Y2FjOlN0YW5kYXJkSXRlbUlkZW50aWZpY2F0aW9uPjxjYmM6SUQgc2NoZW1lSUQ9IjAxNjAiPjM4NTg4OTIzMDI0OTE8L2NiYzpJRD48L2NhYzpTdGFuZGFyZEl0ZW1JZGVudGlmaWNhdGlvbj4KICAgIDxjYWM6Q29tbW9kaXR5Q2xhc3NpZmljYXRpb24+CiAgICAgIDxjYmM6SXRlbUNsYXNzaWZpY2F0aW9uQ29kZSBsaXN0SUQ9IkNHIj40Ni44OS4wMDwvY2JjOkl0ZW1DbGFzc2lmaWNhdGlvbkNvZGU+CiAgICA8L2NhYzpDb21tb2RpdHlDbGFzc2lmaWNhdGlvbj4KICAgIDxjYWM6Q2xhc3NpZmllZFRheENhdGVnb3J5PgogICAgICA8Y2JjOklEPlM8L2NiYzpJRD4KICAgICAgPGNiYzpQZXJjZW50PjI1PC9jYmM6UGVyY2VudD4KICAgICAgPGNhYzpUYXhTY2hlbWU+PGNiYzpJRD5WQVQ8L2NiYzpJRD48L2NhYzpUYXhTY2hlbWU+CiAgICA8L2NhYzpDbGFzc2lmaWVkVGF4Q2F0ZWdvcnk+CiAgPC9jYWM6SXRlbT4KICA8Y2FjOlByaWNlPgogICAgPGNiYzpQcmljZUFtb3VudCBjdXJyZW5jeUlEPSJFVVIiPjE4OC4wMDAwPC9jYmM6UHJpY2VBbW91bnQ+CiAgICA8Y2JjOkJhc2VRdWFudGl0eSB1bml0Q29kZT0iSDg3Ij4xPC9jYmM6QmFzZVF1YW50aXR5PgogIDwvY2FjOlByaWNlPgo8L2NhYzpJbnZvaWNlTGluZT4KPC9JbnZvaWNlPgo=</InvoiceEnvelope>
            //        </B2BOutgoingInvoiceEnvelope>
            //      </Data>
            //    </SendB2BOutgoingInvoiceMsg>
            //  </SOAP-ENV:Body>
            //</SOAP-ENV:Envelope>"


            //5.2
            XmlElement body = doc.DocumentElement["SOAP-ENV:Body"];
            string bodyId = "id-" + Guid.NewGuid().ToString("N");

            body.SetAttribute("Id",
                "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd",
                bodyId);

            body.InnerXml = @"
<EchoMsg xmlns='http://fina.hr/eracun/b2b/pki/Echo/v0.1'>
  <HeaderSupplier xmlns='http://fina.hr/eracun/b2b/invoicewebservicecomponents/v0.1'>
    <MessageID>" + Guid.NewGuid() + @"</MessageID>
    //<SupplierID>9934:26389058739</SupplierID>
    <MessageType>9999</MessageType>
  </HeaderSupplier>
  <Data>
    <EchoData>
      <Echo>hello from AURA</Echo>
    </EchoData>
  </Data>
</EchoMsg>";


            //6.2
            XmlElement header = doc.DocumentElement["SOAP-ENV:Header"];

            XmlElement security = doc.CreateElement("wsse", "Security",
                "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd");

            security.SetAttribute("mustUnderstand",
                "http://schemas.xmlsoap.org/soap/envelope/", "1");

            header.AppendChild(security);

            //7.2
            //            XmlElement timestamp = doc.CreateElement("wsu", "Timestamp",
            //    "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd");

            //            timestamp.SetAttribute("Id", timestamp.NamespaceURI,
            //                "TS-" + Guid.NewGuid().ToString("N"));

            //            DateTime now = DateTime.UtcNow;

            //            timestamp.InnerXml = $@"
            //<wsu:Created>{now:yyyy-MM-ddTHH:mm:ssZ}</wsu:Created>
            //<wsu:Expires>{now.AddMinutes(50):yyyy-MM-ddTHH:mm:ssZ}</wsu:Expires>";

            //            security.AppendChild(timestamp);

            const string WSU =
    "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd";

            XmlElement timestamp = doc.CreateElement("wsu", "Timestamp", WSU);

            timestamp.SetAttribute(
                "Id",
                WSU,
                "TS-" + Guid.NewGuid().ToString("N")
            );

            DateTime now = DateTime.UtcNow;

            XmlElement created = doc.CreateElement("wsu", "Created", WSU);
            created.InnerText = now.ToString("yyyy-MM-ddTHH:mm:ssZ");

            XmlElement expires = doc.CreateElement("wsu", "Expires", WSU);
            expires.InnerText = now.AddMinutes(50).ToString("yyyy-MM-ddTHH:mm:ssZ");

            timestamp.AppendChild(created);
            timestamp.AppendChild(expires);

            security.AppendChild(timestamp);


            //8.2
            XmlElement bst = doc.CreateElement("wsse", "BinarySecurityToken",
    security.NamespaceURI);

            bst.SetAttribute("EncodingType",
                "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary");

            bst.SetAttribute("ValueType",
                "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");

            string certId = "X509-" + Guid.NewGuid().ToString("N");

            bst.SetAttribute("Id",
                "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd",
                certId);

            bst.InnerText = Convert.ToBase64String(cert.RawData);
            security.AppendChild(bst);


            //9.2
            var signedXml = new WSSignedXml(doc);
            signedXml.SigningKey = cert.GetRSAPrivateKey();

            signedXml.SignedInfo.CanonicalizationMethod =
                SignedXml.XmlDsigExcC14NTransformUrl;

            signedXml.SignedInfo.SignatureMethod =
                "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";


            //9.3
            Reference reference = new Reference("#" + bodyId);
            reference.DigestMethod = SignedXml.XmlDsigSHA256Url;

            reference.AddTransform(
                new XmlDsigExcC14NTransform()
            );

            signedXml.AddReference(reference);


            //9.4
            KeyInfo keyInfo = new KeyInfo();

            XmlElement str = doc.CreateElement("wsse", "SecurityTokenReference",
                security.NamespaceURI);

            XmlElement refElem = doc.CreateElement("wsse", "Reference",
                security.NamespaceURI);

            refElem.SetAttribute("URI", "#" + certId);
            refElem.SetAttribute("ValueType",
                "http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-x509-token-profile-1.0#X509v3");

            str.AppendChild(refElem);
            keyInfo.AddClause(new KeyInfoNode(str));

            signedXml.KeyInfo = keyInfo;


            //9.5
            signedXml.ComputeSignature();

            XmlElement signature = signedXml.GetXml();
            security.AppendChild(doc.ImportNode(signature, true));





            doc.Save("C:\\users\\user\\Desktop\\my.xml");



            return Variable.EmptyInstance;
        }
    }








    class CreateAndSendSOAPMsgFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 0, m_name);

            var xmlBody = Utils.GetSafeString(args, 0);
            var soapAction = Utils.GetSafeString(args, 1);
            var endpoint = Utils.GetSafeString(args, 2);
            var certPath = Utils.GetSafeString(args, 3);
            var certPassword = Utils.GetSafeString(args, 4);
            var timeout = Utils.GetSafeInt(args, 5);

            string res = CreateAndSendSOAPMsg(xmlBody, soapAction, endpoint, certPath, certPassword, timeout);

            return new Variable(res);
        }

        public static string CreateAndSendSOAPMsg(
            string xmlBody,
            string soapAction,
            string endpoint,
            string certPath,
            string certPassword,
            int timeoutSeconds
        )
        {
            // Load certificate with private key
            var cert = new X509Certificate2(certPath, certPassword,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable);

            // Build SOAP envelope
            string soapEnvelope = $@"
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" 
                   xmlns:fin=""http://www.fina.hr/e-racun/services"">
  <soapenv:Header/>
  <soapenv:Body>
    {xmlBody}
  </soapenv:Body>
</soapenv:Envelope>";

            var doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            doc.LoadXml(soapEnvelope);

            // Get the body element to sign
            XmlElement body = doc.DocumentElement["soapenv:Body"];
            XmlElement msgElement = body["fin:SendB2BOutgoingInvoiceMsg"];
            string msgId = msgElement.GetElementsByTagName("MessageID")[0].InnerText;

            // Create enveloped XAdES-B signature
            var signedXml = new SignedXml(doc)
            {
                SigningKey = cert.GetRSAPrivateKey()
            };

            // Reference to message body
            Reference bodyRef = new Reference($"#{msgId}");
            bodyRef.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
            bodyRef.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            bodyRef.AddTransform(new XmlDsigExcC14NTransform());
            signedXml.AddReference(bodyRef);

            // XAdES-B SignedProperties reference
            string xadesId = "xades-" + Guid.NewGuid().ToString("N");
            Reference xadesRef = new Reference($"#{xadesId}")
            {
                Type = "http://uri.etsi.org/01903#SignedProperties"
            };
            xadesRef.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";
            xadesRef.AddTransform(new XmlDsigExcC14NTransform());
            signedXml.AddReference(xadesRef);

            // KeyInfo with certificate
            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(cert, X509IncludeOption.WholeChain));
            signedXml.KeyInfo = keyInfo;

            // Signature method and canonicalization
            signedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/2001/10/xml-exc-c14n#";
            signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";

            // Compute signature
            signedXml.ComputeSignature();

            // Build XAdES-B <Object>
            XmlElement xadesObject = BuildXadesObject(doc, xadesId, cert, msgId);

            // Insert <ds:Signature> as child of <SendB2BOutgoingInvoiceMsg>
            XmlElement signatureElement = signedXml.GetXml();
            signatureElement.SetAttribute("Id", "SIG-" + Guid.NewGuid().ToString("N"));
            signatureElement.AppendChild(xadesObject);
            msgElement.AppendChild(doc.ImportNode(signatureElement, true));

            // Final signed SOAP message
            string signedSoap = doc.OuterXml;

            // Send via HTTPS
            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(cert);
            handler.ServerCertificateCustomValidationCallback = (message, cert2, chain, errors) => true; // for dev only

            var client = new HttpClient(handler);
            client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);

            var content = new StringContent(signedSoap, Encoding.UTF8, "text/xml");
            if (!string.IsNullOrEmpty(soapAction))
                content.Headers.Add("SOAPAction", soapAction);

            var response = client.PostAsync(endpoint, content).GetAwaiter().GetResult();
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }

        private static XmlElement BuildXadesObject(XmlDocument doc, string xadesId, X509Certificate2 cert, string targetId)
        {
            const string xadesNs = "http://uri.etsi.org/01903/v1.3.2#";
            const string dsNs = "http://www.w3.org/2000/09/xmldsig#";

            XmlElement obj = doc.CreateElement("Object", dsNs);
            XmlElement qualifyingProps = doc.CreateElement("QualifyingProperties", xadesNs);
            qualifyingProps.SetAttribute("Target", $"#{targetId}");
            obj.AppendChild(qualifyingProps);

            XmlElement signedProps = doc.CreateElement("SignedProperties", xadesNs);
            signedProps.SetAttribute("Id", xadesId);
            qualifyingProps.AppendChild(signedProps);

            // SignedSignatureProperties
            XmlElement signedSigProps = doc.CreateElement("SignedSignatureProperties", xadesNs);
            signedProps.AppendChild(signedSigProps);

            XmlElement signingTime = doc.CreateElement("SigningTime", xadesNs);
            signingTime.InnerText = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            signedSigProps.AppendChild(signingTime);

            XmlElement signingCertV2 = doc.CreateElement("SigningCertificateV2", xadesNs);
            signedSigProps.AppendChild(signingCertV2);

            XmlElement certEl = doc.CreateElement("Cert", xadesNs);
            signingCertV2.AppendChild(certEl);

            XmlElement certDigest = doc.CreateElement("CertDigest", xadesNs);
            certEl.AppendChild(certDigest);

            XmlElement digestMethod = doc.CreateElement("DigestMethod", dsNs);
            digestMethod.SetAttribute("Algorithm", "http://www.w3.org/2001/04/xmlenc#sha256");
            certDigest.AppendChild(digestMethod);

            using (var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(cert.RawData);
                XmlElement digestValue = doc.CreateElement("DigestValue", dsNs);
                digestValue.InnerText = Convert.ToBase64String(hash);
                certDigest.AppendChild(digestValue);
            }

            XmlElement issuerSerialV2 = doc.CreateElement("IssuerSerialV2", xadesNs);
            issuerSerialV2.InnerText = Convert.ToBase64String(
                Encoding.UTF8.GetBytes($"{cert.Issuer}{cert.SerialNumber}")
            );
            certEl.AppendChild(issuerSerialV2);

            // SignedDataObjectProperties
            XmlElement dataObjProps = doc.CreateElement("SignedDataObjectProperties", xadesNs);
            signedProps.AppendChild(dataObjProps);

            XmlElement dataObjFormat = doc.CreateElement("DataObjectFormat", xadesNs);
            dataObjFormat.SetAttribute("ObjectReference", $"#{xadesId.Replace("xades-", "ref-")}");
            dataObjProps.AppendChild(dataObjFormat);

            XmlElement mimeType = doc.CreateElement("MimeType", xadesNs);
            mimeType.InnerText = "text/xml";
            dataObjFormat.AppendChild(mimeType);

            return obj;
        }
    }

    #endregion


    #region FINA FISKALIZACIJA

    class FINA_ECHOFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 8, m_name);

            var endpoint = Utils.GetSafeString(args, 0);
            var dnsIdentity = Utils.GetSafeString(args, 1);
            var serviceCertificatePath = Utils.GetSafeString(args, 2);
            var clientCertificatePath = Utils.GetSafeString(args, 3);
            var clientCertificatePassword = Utils.GetSafeString(args, 4);

            var messageId = Utils.GetSafeString(args, 5);
            var echoText = Utils.GetSafeString(args, 6);
            var supplierId = Utils.GetSafeString(args, 7);

            Variable resVar = new Fiskalizacija2.FINA.Outgoing.Echo().Send(
                endpoint,
                dnsIdentity,
                serviceCertificatePath,
                clientCertificatePath,
                clientCertificatePassword,

                messageId,
                echoText,
                supplierId);

            return resVar;
        }
    }

    class FINA_SEND_OUTGOING_INVOICEFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 11, m_name);

            var endpoint = Utils.GetSafeString(args, 0);
            var dnsIdentity = Utils.GetSafeString(args, 1);
            var serviceCertificatePath = Utils.GetSafeString(args, 2);
            var clientCertificatePath = Utils.GetSafeString(args, 3);
            var clientCertificatePassword = Utils.GetSafeString(args, 4);

            var messageId = Utils.GetSafeString(args, 5);
            var supplierId = Utils.GetSafeString(args, 6);
            var buyerId = Utils.GetSafeString(args, 7);
            var supplierInvoiceId = Utils.GetSafeString(args, 8);

            var invoiceOrCreditNote = Utils.GetSafeString(args, 9);
            ServiceReference1_fina_wsdl.ItemChoiceType itemChoiceType = ServiceReference1_fina_wsdl.ItemChoiceType.InvoiceEnvelope;
            if (invoiceOrCreditNote.ToLower() == "creditnote")
            {
                itemChoiceType = ServiceReference1_fina_wsdl.ItemChoiceType.CreditNoteEnvelope;
            }

            var unsignedInvoiceXmlPath = Utils.GetSafeString(args, 10);

            //var additionalSupplierId = Utils.GetSafeString(args, );
            //var additionalBuyerId = Utils.GetSafeString(args, );
            //var erpid = Utils.GetSafeString(args, );
            //var messageAttributes = Utils.GetSafeString(args, );

            Variable resVar = new Fiskalizacija2.FINA.Outgoing.SendOutgoingInvoice().Send(
                endpoint,
                dnsIdentity,
                serviceCertificatePath,
                clientCertificatePath,
                clientCertificatePassword,

                messageId,
                supplierId,
                buyerId,
                supplierInvoiceId,
                itemChoiceType,
                unsignedInvoiceXmlPath);

            return resVar;

            // // TEST CALL
            //new FINA_SEND_INVOICE().SendInvoice("9934:26389058739",
            //    "9934:26389058739",
            //    "111-11-1",
            //    null, // AdditinalDocumentReference
            //    "D:\\WinX\\ERAC\\D4\\test.xml",
            //    "https://prezdigitalneusluge.fina.hr/SendB2BOutgoingInvoicePKIWebService/services/SendB2BOutgoingInvoicePKIWebService",
            //    "e-invoice",
            //    "D:\\WinX\\ERAC\\certifikati\\e-invoice.cer",
            //    "D:\\WinX\\ERAC\\certifikati\\p12_aurasoft_demo.p12",
            //    "Aurasoft1");
        }
    }


    class FINA_GET_OUTGOING_INVOICE_STATUSFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 9, m_name);

            var endpoint = Utils.GetSafeString(args, 0);
            var dnsIdentity = Utils.GetSafeString(args, 1);
            var serviceCertificatePath = Utils.GetSafeString(args, 2);
            var clientCertificatePath = Utils.GetSafeString(args, 3);
            var clientCertificatePassword = Utils.GetSafeString(args, 4);

            var messageId = Utils.GetSafeString(args, 5);
            var supplierId = Utils.GetSafeString(args, 6);
            var supplierInvoiceId = Utils.GetSafeString(args, 7);
            var invoiceYear = Utils.GetSafeString(args, 8);

            //var additionalSupplierId = Utils.GetSafeString(args, );
            //var erpid = Utils.GetSafeString(args, );
            //var messageAttributes = Utils.GetSafeString(args, );
            //var documentCurrencyCode = Utils.GetSafeString(args, ); // true/false ispis valute iznosa djelomičnog plaćanja u odgovornoj poruci (neobavezan element)

            Variable resVar = new Fiskalizacija2.FINA.Outgoing.GetOutgoingInvoiceStatus().Send(
                endpoint,
                dnsIdentity,
                serviceCertificatePath,
                clientCertificatePath,
                clientCertificatePassword,

                messageId,
                supplierId,
                supplierInvoiceId,
                invoiceYear);

            return resVar;
        }
    }

    class FINA_SEND_OUTGOING_INVOICE_REPORTINGFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 9, m_name);

            var endpoint = Utils.GetSafeString(args, 0);
            var dnsIdentity = Utils.GetSafeString(args, 1);
            var serviceCertificatePath = Utils.GetSafeString(args, 2);
            var clientCertificatePath = Utils.GetSafeString(args, 3);
            var clientCertificatePassword = Utils.GetSafeString(args, 4);

            var messageId = Utils.GetSafeString(args, 5);
            var supplierId = Utils.GetSafeString(args, 6);
            var buyerId = Utils.GetSafeString(args, 7);
            var supplierInvoiceId = Utils.GetSafeString(args, 8);

            var invoiceOrCreditNote = Utils.GetSafeString(args, 9);
            ServiceReference1_fina_wsdl.ItemChoiceType1 itemChoiceType = ServiceReference1_fina_wsdl.ItemChoiceType1.InvoiceEnvelope;
            if (invoiceOrCreditNote.ToLower() == "creditnote")
            {
                itemChoiceType = ServiceReference1_fina_wsdl.ItemChoiceType1.CreditNoteEnvelope;
            }

            var unsignedInvoiceXmlPath = Utils.GetSafeString(args, 10);

            //var additionalSupplierId = Utils.GetSafeString(args, );
            //var erpid = Utils.GetSafeString(args, );
            //var messageAttributes = Utils.GetSafeString(args, );
            //var documentCurrencyCode = Utils.GetSafeString(args, ); // true/false ispis valute iznosa djelomičnog plaćanja u odgovornoj poruci (neobavezan element)

            Variable resVar = new Fiskalizacija2.FINA.Outgoing.SendOutgoingInvoiceReporting().Send(
                endpoint,
                dnsIdentity,
                serviceCertificatePath,
                clientCertificatePath,
                clientCertificatePassword,

                messageId,
                supplierId,
                buyerId,
                supplierInvoiceId,
                itemChoiceType,
                unsignedInvoiceXmlPath);

            return resVar;
        }
    }

    class FINA_ECHO_BUYERFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 8, m_name);

            var endpoint = Utils.GetSafeString(args, 0);
            var dnsIdentity = Utils.GetSafeString(args, 1);
            var serviceCertificatePath = Utils.GetSafeString(args, 2);
            var clientCertificatePath = Utils.GetSafeString(args, 3);
            var clientCertificatePassword = Utils.GetSafeString(args, 4);

            var messageId = Utils.GetSafeString(args, 5);
            var echoText = Utils.GetSafeString(args, 6);
            var buyerId = Utils.GetSafeString(args, 7);

            Variable resVar = new Fiskalizacija2.FINA.Incoming.EchoBuyer().Send(
                endpoint,
                dnsIdentity,
                serviceCertificatePath,
                clientCertificatePath,
                clientCertificatePassword,

                messageId,
                echoText,
                buyerId);

            return resVar;
        }
    }
    
    class FINA_GET_INCOMING_INVOICE_LISTFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 7, m_name);

            var endpoint = Utils.GetSafeString(args, 0);
            var dnsIdentity = Utils.GetSafeString(args, 1);
            var serviceCertificatePath = Utils.GetSafeString(args, 2);
            var clientCertificatePath = Utils.GetSafeString(args, 3);
            var clientCertificatePassword = Utils.GetSafeString(args, 4);

            var messageId = Utils.GetSafeString(args, 5);
            var buyerId = Utils.GetSafeString(args, 6);

            Variable resVar = new Fiskalizacija2.FINA.Incoming.GetIncomingInvoiceList().Send(
                endpoint,
                dnsIdentity,
                serviceCertificatePath,
                clientCertificatePath,
                clientCertificatePassword,

                messageId,
                buyerId);

            return resVar;
        }
    }
    
    class FINA_GET_INCOMING_INVOICEFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 8, m_name);

            var endpoint = Utils.GetSafeString(args, 0);
            var dnsIdentity = Utils.GetSafeString(args, 1);
            var serviceCertificatePath = Utils.GetSafeString(args, 2);
            var clientCertificatePath = Utils.GetSafeString(args, 3);
            var clientCertificatePassword = Utils.GetSafeString(args, 4);

            var messageId = Utils.GetSafeString(args, 5);
            var buyerId = Utils.GetSafeString(args, 6);
            var invoiceId = Utils.GetSafeString(args, 7);

            Variable resVar = new Fiskalizacija2.FINA.Incoming.GetIncomingInvoice().Send(
                endpoint,
                dnsIdentity,
                serviceCertificatePath,
                clientCertificatePath,
                clientCertificatePassword,

                messageId,
                buyerId,
                invoiceId);

            return resVar;
        }
    }
    
    class FINA_CHANGE_INCOMING_INVOICE_STATUSFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 9, m_name);

            var endpoint = Utils.GetSafeString(args, 0);
            var dnsIdentity = Utils.GetSafeString(args, 1);
            var serviceCertificatePath = Utils.GetSafeString(args, 2);
            var clientCertificatePath = Utils.GetSafeString(args, 3);
            var clientCertificatePassword = Utils.GetSafeString(args, 4);

            var messageId = Utils.GetSafeString(args, 5);
            var buyerId = Utils.GetSafeString(args, 6);
            var invoiceId = Utils.GetSafeString(args, 7);

            var newStatusCodeString = Utils.GetSafeString(args, 8);
            InvoiceStatusTypeStatusCode newStatusCode = InvoiceStatusTypeStatusCode.RECEIVED;
            switch (newStatusCodeString.ToUpper())
            {
                case "RECEIVED":
                    newStatusCode = InvoiceStatusTypeStatusCode.RECEIVED;
                    break;
                case "RECEIVING_CONFIRMED":
                    newStatusCode = InvoiceStatusTypeStatusCode.RECEIVING_CONFIRMED;
                    break;
                case "APPROVED":
                    newStatusCode = InvoiceStatusTypeStatusCode.APPROVED;
                    break;
                case "REJECTED":
                    newStatusCode = InvoiceStatusTypeStatusCode.REJECTED;
                    break;
                case "PAYMENT_RECEIVED":
                    newStatusCode = InvoiceStatusTypeStatusCode.PAYMENT_RECEIVED;
                    break;
                case "PAYMENT_FULFILLED":
                    newStatusCode = InvoiceStatusTypeStatusCode.PAYMENT_FULFILLED;
                    break;
                case "PAYMENT_PARTIALLY_FULFILLED":
                    newStatusCode = InvoiceStatusTypeStatusCode.PAYMENT_PARTIALLY_FULFILLED;
                    break;
                default:
                    throw new Exception("Invalid newStatusCodeString!");
            }

            var codeReasonString = Utils.GetSafeString(args, 9, "");
            bool codeReasonSpecified = false;
            InvoiceStatusTypeCodeReason codeReason = InvoiceStatusTypeCodeReason.OTHER_REASON;
            switch (codeReasonString.ToUpper())
            {
                case "":
                    codeReasonSpecified = false;
                    codeReason = InvoiceStatusTypeCodeReason.OTHER_REASON;
                    break;
                case "VAT_REASON":
                    codeReasonSpecified = true;
                    codeReason = InvoiceStatusTypeCodeReason.VAT_REASON;
                    break;
                case "NOT_VAT_REASON":
                    codeReasonSpecified = true;
                    codeReason = InvoiceStatusTypeCodeReason.NOT_VAT_REASON;
                    break;
                case "OTHER_REASON":
                    codeReasonSpecified = true;
                    codeReason = InvoiceStatusTypeCodeReason.OTHER_REASON;
                    break;
                default:
                    throw new Exception("Invalid codeReasonString!");
            }

            var note = Utils.GetSafeString(args, 10, "");
            if(note.Length == 0)
            {
                note = null;
            }

            Variable resVar = new Fiskalizacija2.FINA.Incoming.ChangeIncomingInvoiceStatus().Send(
                endpoint,
                dnsIdentity,
                serviceCertificatePath,
                clientCertificatePath,
                clientCertificatePassword,

                messageId,
                buyerId,
                invoiceId,

                newStatusCode,
                codeReasonSpecified,
                codeReason,
                note
                );

            return resVar;
        }
    }

    #endregion


}
