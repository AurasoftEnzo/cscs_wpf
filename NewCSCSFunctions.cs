using LiveChartsCore.SkiaSharpView.Painting;
using MapControl;
using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
//using System.Windows.Forms;
using System.Windows.Media;
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
            
        }
        public partial class Constants
        {
            public const string SET_MAIN_MENU = "SetMainMenu";

            public const string MAP_SETUP = "MapSetup";
            public const string MAP_ADD_POINT = "MapAddPoint";
            
            public const string GET_PATH = "GetPath";

            public const string ZIP_FILE = "ZipFile";

            public const string GET_SQL_RECORD = "GetSQLRecord";
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

            List<string> wxmenuLines = File.ReadAllLines(App.GetConfiguration("ScriptsPath", "") + "wxmenu.ini").ToList();

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
}
