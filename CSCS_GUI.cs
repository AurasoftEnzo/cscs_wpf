//using CefSharp.DevTools.FedCm;
using CSCS.InterpreterManager;
using LiveChartsCore.SkiaSharpView.WPF;
using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Xps;
using WpfControlsLibrary;
using WpfCSCS;
using WpfCSCS.Reporting;
using static WpfCSCS.Btrieve;

namespace SplitAndMerge
{
	public class CscsGuiModule : ICscsModule
	{
		public ICscsModuleInstance CreateInstance(Interpreter interpreter)
		{
			return new CscsGuiModuleInstance(interpreter);
		}

		public void Terminate()
		{
		}
	}

	public class CscsGuiModuleInstance : ICscsModuleInstance
	{
		public CscsGuiModuleInstance(Interpreter interpreter)
		{
			interpreter.RegisterFunction("TestClass1", new TestClass1Function());
			interpreter.RegisterFunction("TestButton", new TestButtonFunction());
			
			interpreter.RegisterFunction("PrintWindow", new PrintWindowFunction());
			
			interpreter.RegisterFunction("DownloadScripts", new DownloadScriptsFunction());
			interpreter.RegisterFunction("ServerAddress", new ServerAddressFunction());
						
			interpreter.RegisterFunction(Constants.MSG, new VariableArgsFunction(true));
			interpreter.RegisterFunction(Constants.DEFINE, new VariableArgsFunction(true));
			interpreter.RegisterFunction(Constants.SET_OBJECT, new VariableArgsFunction(true));
			interpreter.RegisterFunction(Constants.DISPLAY_ARRAY, new VariableArgsFunction(true));
			interpreter.RegisterFunction(Constants.DISPLAY_ARR_SETUP, new VariableArgsFunction(false));
			interpreter.RegisterFunction(Constants.DISPLAY_ARR_REFRESH, new VariableArgsFunction(false));
			interpreter.RegisterFunction(Constants.DATA_GRID, new VariableArgsFunction(true));
			interpreter.RegisterFunction(Constants.ADD_COLUMN, new VariableArgsFunction(true));
			interpreter.RegisterFunction(Constants.DELETE_COLUMN, new VariableArgsFunction(true));
			interpreter.RegisterFunction(Constants.SHIFT_COLUMN, new VariableArgsFunction(true));

			interpreter.RegisterFunction(Constants.CHAIN, new ChainFunction(false));
			interpreter.RegisterFunction(Constants.PARAM, new ChainFunction(true));

			interpreter.RegisterFunction(Constants.WITH, new ConstantsFunction());
			interpreter.RegisterFunction(Constants.NEWRUNTIME, new ConstantsFunction());

			interpreter.RegisterFunction(Constants.FREE, new FreeMemoryFunction());

			interpreter.RegisterFunction("OpenFile", new OpenFileFunction(false));
			interpreter.RegisterFunction("OpenFileContents", new OpenFileFunction(true));
			interpreter.RegisterFunction("SaveFile", new SaveFileFunction());

			interpreter.RegisterFunction("ShowWidget", new ShowHideWidgetFunction(true));
			interpreter.RegisterFunction("HideWidget", new ShowHideWidgetFunction(false));

			interpreter.RegisterFunction("GetText", new GetTextWidgetFunction());
			interpreter.RegisterFunction("SetText", new SetTextWidgetFunction());
			interpreter.RegisterFunction("AddWidgetData", new AddWidgetDataFunction());
			interpreter.RegisterFunction("SetWidgetOptions", new SetWidgetOptionsFunction());

			interpreter.RegisterFunction("SetWindowOptions", new SetWindowOptionsFunction());

			interpreter.RegisterFunction("Get_comp_year", new Get_comp_yearFunction());
			interpreter.RegisterFunction("Get_dbase", new Get_dbaseFunction());
			interpreter.RegisterFunction("GetSelected", new GetSelectedFunction());
			interpreter.RegisterFunction("SetBackgroundColor", new SetColorFunction(true));
			interpreter.RegisterFunction("SetForegroundColor", new SetColorFunction(false));
			interpreter.RegisterFunction("SetImage", new SetImageFunction());

			interpreter.RegisterFunction("DisplayArrFunc", new DisplayArrFuncFunction());

			interpreter.RegisterFunction("MessageBox", new MessageBoxFunction());
			interpreter.RegisterFunction("msg", new MessageBoxFunction());

			interpreter.RegisterFunction("SendToPrinter", new PrintFunction());

			interpreter.RegisterFunction("AddMenuItem", new AddMenuEntryFunction(false));
			interpreter.RegisterFunction("AddMenuSeparator", new AddMenuEntryFunction(true));
			interpreter.RegisterFunction("RemoveMenu", new RemoveMenuFunction());

			interpreter.RegisterFunction("RunOnMain", new RunOnMainFunction());
			interpreter.RegisterFunction("RunExec", new RunExecFunction());
			interpreter.RegisterFunction("RunScript", new RunScriptFunction());

			interpreter.RegisterFunction("CheckVATNumber", new CheckVATFunction());
			interpreter.RegisterFunction("GetVATName", new CheckVATFunction(CheckVATFunction.MODE.NAME));
			interpreter.RegisterFunction("GetVATAddress", new CheckVATFunction(CheckVATFunction.MODE.ADDRESS));

			interpreter.RegisterFunction("GetGridRowCount", new GetGridRowCountFunction());
			interpreter.RegisterFunction("FillBufferFromGridRow", new FillBufferFromGridRowFunction());

			interpreter.RegisterFunction("CreateWindow", new NewWindowFunction(NewWindowFunction.MODE.NEW));
			interpreter.RegisterFunction("CloseWindow", new NewWindowFunction(NewWindowFunction.MODE.DELETE));
			interpreter.RegisterFunction("ShowWindow", new NewWindowFunction(NewWindowFunction.MODE.SHOW));
			interpreter.RegisterFunction("HideWindow", new NewWindowFunction(NewWindowFunction.MODE.HIDE));
			interpreter.RegisterFunction("NextWindow", new NewWindowFunction(NewWindowFunction.MODE.NEXT));
			interpreter.RegisterFunction("ModalWindow", new NewWindowFunction(NewWindowFunction.MODE.MODAL));
			interpreter.RegisterFunction("SetMainWindow", new NewWindowFunction(NewWindowFunction.MODE.SET_MAIN));
			interpreter.RegisterFunction("UnsetMainWindow", new NewWindowFunction(NewWindowFunction.MODE.UNSET_MAIN));
			interpreter.RegisterFunction("FillWidget", new FillWidgetFunction());

			interpreter.RegisterFunction("AsyncCall", new AsyncCallFunction());
			interpreter.RegisterFunction(Constants.EXIT, new TerminateFunction());
			interpreter.RegisterFunction(Constants.QUIT, new WpfQuitCommand());

			interpreter.RegisterFunction("SetWindowModalResult", new SetWindowModalResultFunction());

			interpreter.AddAction(Constants.ASSIGNMENT, new MyAssignFunction());
			interpreter.AddAction(Constants.INCREMENT, new MyAssignFunction(MyAssignFunction.MODE.INCREMENT));
			interpreter.AddAction(Constants.DECREMENT, new MyAssignFunction(MyAssignFunction.MODE.DECREMENT));
			interpreter.AddAction(Constants.POINTER, new MyPointerFunction());
			for (int i = 0; i < Constants.OPER_ACTIONS.Length; i++)
			{
				interpreter.AddAction(Constants.OPER_ACTIONS[i], new MyAssignFunction());
			}
		}
	}

     public partial class Constants
     {
	public const string SETUP_REPORT = "SetupReport";
	public const string OUTPUT_REPORT = "OutputReport";
	public const string UPDATE_REPORT = "UpdateReport";
	public const string PRINT_REPORT = "PrintReport";

	public const string SET_PARAM_REPORT = "SetParamReport";
	public const string GET_PARAM_REPORT = "GetParamReport";

	//public const string PRINT_SQL_REPORT = "PrintSqlReport";

	public const string MAINMENU = "#MAINMENU";
	public const string WINFORM = "#WINFORM";

	public const string OPENV = "Openv";
	public const string FINDV = "Findv";
	public const string CLOSEV = "Closev";

	public const string REPL = "Repl";

	public const string CLR = "Clr";
	public const string RCNGET = "RCNGet";
	public const string RCNSET = "RCNSet";

	public const string ACTIVE = "Active";
	public const string DEL = "Del";
	public const string SAVE = "Save";

	public const string RDA = "Rda";
	public const string WRTA = "Wrta";

	public const string TRC = "Trc";

	public const string SCAN = "Scan";
	public const string SCAN_WHERE = "ScanWhere";

	public const string DISPLAY_TABLE_SETUP = "DisplayTableSetup";
	public const string DISPLAY_TABLE_SETUP_WHERE = "DisplayTableSetupWhere";
	public const string DISPLAY_ARRAY_SETUP = "DisplayArraySetup";
	public const string DISPLAY_ARRAY_REFRESH = "DisplayArrayRefresh";
	public const string DISPLAYARRAY = "DisplayArray";
	public const string DISPLAYTABLE = "DisplayTable";

	public const string DATAGRID = "DataGrid";

	public const string FORMAT = "Format";

	public const string CURSOR = "Cursor";

	public const string RESET_FIELD = "ResetField";

	public const string CO_GET = "COGet";
	public const string CO_SET = "COSet";

	public const string COMMONDB_GET = "CommonDBGet";
	public const string YEAR = "Year";
	public const string DOM = "DOM";
	public const string DOW = "DOW";
	public const string FLSZE = "FLSZE";
	public const string DSPCE = "DSPCE";
	public const string HEX = "HEX";
	public const string REGEDIT = "REGEDIT";
	public const string EMAIL = "EMAIL";

	public const string FLERR = "Flerr";

	public const string NAVIGATOR = "Navigator";

	public const string SQL_TO_XLSX = "SqlToXlsx";

	public const string X_FILE_NEW = "XFileNew";
	public const string X_FILE_OPEN = "XFileOpen";
	public const string X_FILE_SAVE = "XFileSave";
	public const string X_FILE_SAVE_AS = "XFileSaveAs";
	public const string X_FILE_DELETE = "XFileDelete";
	public const string X_SHEET_ADD = "XSheetAdd";
	public const string X_SHEET_DELETE = "XSheetDelete";
	public const string X_SHEET_COUNT = "XSheetCount";
	public const string X_SHEET_CLEAR = "XSheetClear";
	public const string X_SHEET_RENAME = "XSheetRename";
	public const string X_CELL_WRITE_STRING = "XCellWriteString";
	public const string X_CELL_WRITE_INTEGER = "XCellWriteInteger";
	public const string X_CELL_WRITE_DOUBLE = "XCellWriteDouble";
	public const string X_CELL_WRITE_BOOLEAN = "XCellWriteBoolean";
	public const string X_CELL_WRITE_TIME = "XCellWriteTime";
	public const string X_CELL_WRITE_DATE = "XCellWriteDate";
	public const string X_CELL_WRITE_DATETIME = "XCellWriteDateTime";
	public const string X_CELL_WRITE_FORMULA = "XCellWriteFormula";
	public const string X_CELL_READ_STRING = "XCellReadString";
	public const string X_CELL_READ_INTEGER = "XCellReadInteger";
	public const string X_CELL_READ_DOUBLE = "XCellReadDouble";
	public const string X_CELL_READ_BOOLEAN = "XCellReadBoolean";
	public const string X_CELL_READ_TIME = "XCellReadTime";
	public const string X_CELL_READ_DATE = "XCellReadDate";
	public const string X_CELL_READ_FORMULA = "XCellReadFormula";
	public const string X_CELL_EMPTY = "XCellEmpty";
	public const string X_FIND_SHEET = "XFindSheet";
	public const string X_FIND_COLUMN = "XFindColumn";
	public const string X_FIND_ROW = "XFindRow";
	public const string X_COPY_CELL = "XCopyCell";
	public const string X_COPY_ROW = "XCopyRow";
	public const string X_COPY_ROW_TO_ROW = "XCopyRowToRow";
	public const string X_COPY_COLUMN = "XCopyColumn";
	public const string X_FORMAT_COLUMN = "XFormatColumn";
	public const string X_LAST_ROW = "XLastRow";
	public const string X_LAST_COLUMN = "XLastColumn";
	//public const string X_LAST_COLUMN = "XLastColumn";
	public const string X_LAST_ADDRESS = "XLastAddress";
	public const string X_INSERT_ROWS = "XInsertRows";
	public const string X_DELETE_ROW = "XDeleteRow";
	public const string X_COLUMNS_AUTO_FIT = "XColumnsAutoFit";
	public const string X_SET_TABLE = "XSetTable";
	public const string X_NAMED_CELL_POSITION = "XNamedCellPosition";
	public const string X_NAMED_RANGE_ADD = "XNamedRangeAdd";
	public const string X_HEADER = "XHeader";
	public const string X_FOOTER = "XFooter";
	public const string X_FONT_NAME = "XFontName";
	public const string X_FONT_SIZE = "XFontSize";
	public const string X_FONT_COLOR = "XFontColor";
	public const string X_BACKGROUND_COLOR = "XBackgroundColor";
	public const string X_ALIGN = "XAlign";
	public const string X_FONT_FORMAT = "XFontFormat";
	public const string X_BORDER = "XBorder";
	public const string X_PIVOT_TABLE_REFRESH = "XPivotTableRefresh";

	public const string X_ERR = "XErr";

	public const string READ_XML_FILE = "ReadXmlFile";
	public const string READ_TAGCONTENT_FROM_XMLSTRING = "readTagContentFromXmlString";

	public const string SET_FOCUS = "SetFocus";
	public const string LAST_OBJ = "LastObj";
	public const string LAST_OBJ_CLICKED = "LastObjClick";

	public const string STRINGS = "Strings";

	public const string STATUS_BAR = "StatusBar";
	public const string GET_FILE = "GET_FILE";

	public const string HORIZONTAL_BAR = "HorizontalBar";

	public const string DUAL_LIST_EXEC = "DUAL_LIST_EXEC";

	public const string LIKE = "LIKE";
	public const string FFILE = "FFILE";
	public const string DLLFC = "DLLFC";
	public const string PARSEFILE = "PARSEFILE";
	public const string LCHR = "LCHR";
	public const string MAKE_DIR = "MAKE_DIR";
	public const string OS = "OS";
	public const string DIFF = "DIFF";
	public const string FARRAY = "FARRAY";
	public const string TRAP = "TRAP";
	public const string WebBrowse = "WebBrowse";
	public const string ISUP = "ISUP";
	public const string ISLO = "ISLO";
	public const string ISNUM = "ISNUM";
	public const string GET_FORM_NAME = "GET_FORM_NAME";
	public const string Decryptstr = "Decryptstr";
	public const string ENCRYPTSTR = "ENCRYPTSTR";
	public const string PRINTER_NAME = "PRINTER_NAME";
	public const string DELF = "DELF";
	public const string BELL = "BELL";
	public const string CDOW = "CDOW";
	public const string DTOS = "DTOS";
	public const string DTOC = "DTOC";
	public const string CTOD = "CTOD";
	public const string DTOSQL = "DTOSQL";
	public const string CPATH = "CPATH";
	public const string XPATH = "XPATH";
	public const string PLAYWAV = "PLAYWAV";
	public const string FILE_STORE = "FILE_STORE";
	public const string CHR = "CHR";
	public const string CMNTH = "CMNTH";
	public const string DIR_EXISTS = "DIR_EXISTS";
	public const string ELOC = "ELOC";
	public const string LOC = "LOC";
	public const string DEC = "DEC";
	public const string ASC = "ASC";
	public const string EXP = "EXP";
	public const string INT = "INT";
	public const string ISAL = "ISAL";
	public const string TPATH = "TPATH";
	public const string IPATH = "IPATH";
	public const string MPATH = "MPATH";
	public const string WPATH = "WPATH";
	public const string GFLD = "GFLD";
	public const string SNDX = "SNDX";
	public const string JUST = "JUST";


	public const string GET_SELECTED_GRID_ROW = "GetSelectedGridRow";

	public const string CHART = "Chart";
	public const string PIE_CHART = "PieChart";
	public const string GAUGE_CHART = "GaugeChart";

	public const string DEFINE = "DEFINE";
	public const string DISPLAY_ARRAY = "DISPLAYARR";
	public const string DISPLAY_ARR_SETUP = "DISPLAYARRSETUP";
	public const string DISPLAY_ARR_REFRESH = "DISPLAYARRREFRESH";
	public const string DATA_GRID = "DATA_GRID";
	public const string ADD_COLUMN = "NEWCOLUMN";
	public const string DELETE_COLUMN = "DELETECOLUMN";
	public const string SHIFT_COLUMN = "SHIFTCOLUMN";
	public const string MSG = "MSG";
	public const string SET_OBJECT = "SET_OBJECT";

	public const string SET_TEXT = "SetText";

	public const string CHAIN = "chain";
	public const string PARAM = "param";
	public const string WITH = "with";
	public const string NEWRUNTIME = "newruntime";
     }
}

namespace WpfCSCS
{

	public class CSCS_GUI
	{
		public NavigatorClass NavigatorClass { get; set; } = new NavigatorClass();
		public Btrieve Btrieve { get; set; } = new Btrieve();
		//public TasFunctions TasFunctions { get; set; } = new TasFunctions();

		public string LastObjWidgetName;
		public string LastObjClickedWidgetName;

		public static CSCS_GUI LastInstance { get; set; }

		public static CSCS_GUI GetInstance(ParsingScript script)
		{
			if (script == null)
			{
				return LastInstance;
			}
			var result = script.Context as CSCS_GUI;
			if (result == null)
			{
				script.Context = result = LastInstance;
			}
			result.Script = script;
			return result;
		}

		public Dictionary<string, List<Variable>> Parameters = new Dictionary<string, List<Variable>>();
		public Dictionary<string, Window> Tag2Parent { get; set; } = new Dictionary<string, Window>();

		public void CacheParentWindow(string tag, Window parent)
		{
			Tag2Parent[tag] = parent;
		}
		public Window GetParentWindow(string filename)
		{
			if (!Tag2Parent.TryGetValue(filename, out Window win))
			{
				return null;
			}
			return win;
		}

		public Window GetParentWindow(ParsingScript script)
		{
			if (script.ParentScript != null &&
				File2Window.TryGetValue(script.ParentScript.Filename, out Window win))
			{
				return win;
			}
			return CSCS_GUI.MainWindow;
		}
		public Window GetScriptWindow(ParsingScript script)
		{
			if (script != null && !string.IsNullOrWhiteSpace(script.Filename) &&
				File2Window.TryGetValue(script.Filename, out Window win))
			{
				return win;
			}
			return CSCS_GUI.MainWindow;
		}

		static int s_id;
		public int ID { get; private set; }

		public ParsingScript Script { get; set; }
		public CSCS_GUI(ParsingScript script = null)
		{
			LastInstance = this;
			ID = ++s_id;
			Script = script;
			if (script != null)
			{
				script.Context = this;
			}
		}

		public static Dispatcher Dispatcher { get; set; }

		public class WidgetData
		{
			public enum COL_TYPE { STRING, NUMBER };

			public WidgetData(FrameworkElement w)
			{
				widget = w;
			}
			public FrameworkElement widget;

			public List<string> headerNames = new List<string>();
			public List<string> headerBindings = new List<string>();
			public List<COL_TYPE> colTypes = new List<COL_TYPE>();
			public Dictionary<string, Variable> headers = new Dictionary<string, Variable>();

			public string lineCounterName;
			public int lineCounter;
			public Variable lineCounterVar;

			public string actualElemsName;
			public int actualElems;
			public Variable actualElemsVar;

			public string maxElemsName;
			public int maxElems;
			public Variable maxElemsVar;

			public bool needsReset;
		}

		public static App TheApp { get; set; }
		public static Window MainWindow { get; set; }
		public static bool ChangingBoundVariable { get; set; }
		public static string RequireDEFINE { get; set; }
		public static string DefaultDB { get; set; }
		public static string CommonDB { get; set; }
		public static int MaxCacheSize { get; set; }
		public static string DefaultDateFormat { get; set; }
		public static string DateFormat10 { get; set; }
		public static string DateFormat8 { get; set; }

		bool m_initialized;

		public Dictionary<string, FrameworkElement> Controls { get; set; } = new Dictionary<string, FrameworkElement>();
		public Dictionary<FrameworkElement, Window> Control2Window { get; set; } = new Dictionary<FrameworkElement, Window>();
		public Window ActiveWindow;

		public static Dictionary<Window, string> Window2File { get; set; } = new Dictionary<Window, string>();
		public static Dictionary<string, Window> File2Window { get; set; } = new Dictionary<string, Window>();
		public Dictionary<string, List<string>> GroupBoxesAndRadioButtons { get; set; } = new Dictionary<string, List<string>>();

		public Interpreter Interpreter { get; private set; }

		//public static Action<string, string> OnWidgetClick;

		Dictionary<string, string> m_actionHandlers = new Dictionary<string, string>();
		Dictionary<string, string> m_preActionHandlers = new Dictionary<string, string>();
		Dictionary<string, string> m_postActionHandlers = new Dictionary<string, string>();
		Dictionary<string, string> m_keyDownHandlers = new Dictionary<string, string>();
		Dictionary<string, string> m_keyUpHandlers = new Dictionary<string, string>();
		Dictionary<string, string> m_textChangedHandlers = new Dictionary<string, string>();
		Dictionary<string, string> m_lostFocusHandlers = new Dictionary<string, string>();
		Dictionary<string, string> m_selChangedHandlers = new Dictionary<string, string>();
		Dictionary<string, string> m_mouseHoverHandlers = new Dictionary<string, string>();
		Dictionary<string, string> m_dateSelectedHandlers = new Dictionary<string, string>();

		//Pre, Post
		Dictionary<string, string> m_PreHandlers = new Dictionary<string, string>();
		Dictionary<string, string> m_PostHandlers = new Dictionary<string, string>();

		Dictionary<string, string> m_NavigatorChangeHandlers = new Dictionary<string, string>();
		Dictionary<string, string> m_NavigatorAfterChangeHandlers = new Dictionary<string, string>();

		Dictionary<string, string> m_ChangeHandlers = new Dictionary<string, string>();

		public Dictionary<string, string> m_MoveHandlers = new Dictionary<string, string>();
		Dictionary<string, string> m_SelectHandlers = new Dictionary<string, string>();

		Dictionary<string, Variable> m_boundVariables = new Dictionary<string, Variable>();
		Dictionary<string, List<FrameworkElement>> m_bound2Widgets = new Dictionary<string, List<FrameworkElement>>();

		HashSet<string> m_updatingWidget = new HashSet<string>();
		//static Dictionary<string, TabPage> s_tabPages           = new Dictionary<string, TabPage>();
		//static TabControl s_tabControl;

		internal CSCS_SQL SQLInstance { get; set; } = new CSCS_SQL();
		internal Btrieve BtrieveInstance { get; set; } = new Btrieve();
		internal TasFunctions TasFunctionsInstance { get; set; } = new TasFunctions();
        public NewCSCSFunctions NewCSCSFunctionsInstance { get; set; } = new NewCSCSFunctions();
        internal Charts ChartsInstance { get; set; } = new Charts();
		internal NavigatorClass NavigatorClassInstance { get; set; } = new NavigatorClass();
		internal Commands CommandsInstance { get; set; } = new Commands();

		static public InterpreterManagerModule InterpreterManager = new InterpreterManagerModule();

		public static AdictionaryLocal.Adictionary Adictionary { get; set; } = new AdictionaryLocal.Adictionary();

		public Dictionary<string, DefineVariable> DEFINES { get; set; } =
			new Dictionary<string, DefineVariable>(40000);
		public Dictionary<string, WidgetData> WIDGETS { get; set; } =
			new Dictionary<string, WidgetData>();

		public Dictionary<string, Dictionary<string, bool>> m_varExists =
			new Dictionary<string, Dictionary<string, bool>>();


		protected static List<ICscsModule> GetModuleList()
		{
			return new List<ICscsModule>
			{
				new CscsGuiModule(),
                //new CscsMathModule(),
                InterpreterManager
			};
		}
		public void Init()
		{
			if (m_initialized)
			{
				return;
			}
			m_initialized = true;

			InterpreterManager.OnInterpreterCreated += InterpreterCreated;
			InterpreterManager.Modules = GetModuleList();
			var interpreterId = InterpreterManager.NewInterpreter();
			InterpreterManager.SetInterpreter(interpreterId);
			Interpreter = InterpreterManager.CurrentInterpreter;
			InterpreterManager.CreateInstance(Interpreter);

			//Interpreter.OnOutput += Print;
			ParserFunction.OnVariableChange += OnVariableChange;

			Constants.FUNCT_WITH_SPACE.Add(Constants.DEFINE);
			Constants.FUNCT_WITH_SPACE.Add(Constants.DISPLAY_ARRAY);
			Constants.FUNCT_WITH_SPACE.Add(Constants.DATA_GRID);
			Constants.FUNCT_WITH_SPACE.Add(Constants.ADD_COLUMN);
			Constants.FUNCT_WITH_SPACE.Add(Constants.DELETE_COLUMN);
			Constants.FUNCT_WITH_SPACE.Add(Constants.SHIFT_COLUMN);
			Constants.FUNCT_WITH_SPACE.Add(Constants.MSG);
			Constants.FUNCT_WITH_SPACE.Add(Constants.SET_OBJECT);
			Constants.FUNCT_WITH_SPACE.Add(Constants.SET_TEXT);

			Constants.FUNCT_WITH_SPACE.Add(Constants.CHAIN);
			Constants.FUNCT_WITH_SPACE.Add(Constants.PARAM);

			Precompiler.AddNamespace("using WpfCSCS;");
			Precompiler.AddNamespace("using System.Windows;");
			Precompiler.AddNamespace("using System.Windows.Controls;");
			Precompiler.AddNamespace("using System.Windows.Controls.Primitives;");
			Precompiler.AddNamespace("using System.Windows.Data;");
			Precompiler.AddNamespace("using System.Windows.Documents;");
			Precompiler.AddNamespace("using System.Windows.Input;");
			Precompiler.AddNamespace("using System.Windows.Media;");
			Precompiler.AsyncMode = false;

			RequireDEFINE = App.GetConfiguration("Require_Define", "false");
			DefaultDB = App.GetConfiguration("DefaultDB", "ad");
			CommonDB = App.GetConfiguration("CommonDB", "");
			DefaultDateFormat = App.GetConfiguration("DateFormat", "dd/MM/yyyy");
			DateFormat10 = App.GetConfiguration("DateFormat", "dd/MM/yyyy");
			DateFormat8 = App.GetConfiguration("DateFormat", "dd/MM/yy");

			if (int.TryParse(App.GetConfiguration("MaxCacheSize", "300"), out int cacheSize))
			{
				MaxCacheSize = cacheSize;
			}

			SQLInstance.SqlServerConnection = new SqlConnection(CSCS_SQL.ConnectionString);
			SQLInstance.Init(Interpreter);

			Locking.Init(SQLInstance.SqlServerConnection);

			BtrieveInstance.Init(this);
			TasFunctionsInstance.Init(this);
			NewCSCSFunctionsInstance.Init(this);
            ChartsInstance.Init(this);
			Reporting.Reporting.Init(Interpreter);
			Excel.Init(Interpreter);
			NavigatorClassInstance.Init(Interpreter);
			CommandsInstance.Init(this);

			LoadCompilerConstantsTxt();
		}

		private void LoadCompilerConstantsTxt()
		{
			//var curr = Directory.GetCurrentDirectory();
			//MessageBox.Show("curr = " + curr);

			//var curr2 = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			//MessageBox.Show("curr2 = " + curr2);

			var curr3 = AppDomain.CurrentDomain.BaseDirectory;
			//MessageBox.Show("curr3 = " + curr3);

			//var curr4 = System.IO.Directory.GetCurrentDirectory();
			//MessageBox.Show("curr4 = " + curr4);

			//var curr5 = Environment.CurrentDirectory;
			//MessageBox.Show("curr5 = " + curr5);

			//var curr6 = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
			//MessageBox.Show("curr6 = " + curr6);

			//var curr7 = System.IO.Path.GetDirectory(Application.ExecutablePath);
			//MessageBox.Show("curr7 = " + curr7);


			//var lines = File.ReadLines(Path.Combine(curr3, "../../scripts/CompilerConstants.txt"));
			var lines = File.ReadLines(Path.Combine(curr3, "CompilerConstants.txt"));
			foreach (var line in lines)
			{
				if (line.StartsWith("{") || line.StartsWith(";") || line.Trim().Count() == 0)
				{
					continue;
				}

				var lineParts = line.Split('=');
				if (lineParts.Count() != 2)
				{
					continue;
				}

				//DefineVariable newDefVar = new DefineVariable(lineParts[0], lineParts[0], "a", 300, 0);

				//if(int.TryParse(lineParts[1], out int parsed))
				//{
				//    Variable newVar = new Variable(parsed);
				//    newDefVar.InitVariable(newVar, this);
				//}                

				DefineVariable newDefVar = new DefineVariable(lineParts[0], null, "a", 300, 0);
				newDefVar.InitVariable(new Variable(lineParts[0]), this);
			}
		}

		private static void InterpreterCreated(object sender, EventArgs e)
		{
			// Subscribe to the printing events from the interpreter.
			// A printing event will be triggered after each successful statement
			// execution. On error an exception will be thrown.
			if (sender is Interpreter interpreter)
			{
				InterpreterManager.CreateInstance(interpreter);
				interpreter.OnOutput += Print;
			}
		}

		public Variable ProcessScript(string scriptStr, string filename = "", bool mainFIle = false)
		{
			var value = Interpreter.Process(scriptStr, filename, mainFIle, this);
			return value;
		}

		public bool CacheAdictionary()
		{
			try
			{
				SqlConnection conn = new SqlConnection(CSCS_SQL.ConnectionString);

				conn.Open();

				CSCS_GUI.Adictionary.SY_DATABASESList = AdictionaryLocal.CacheAdictionary.GetSY_DATABASES(conn);
				CSCS_GUI.Adictionary.SY_TABLESList = AdictionaryLocal.CacheAdictionary.GetSY_TABLES(conn);
				CSCS_GUI.Adictionary.SY_FIELDSList = AdictionaryLocal.CacheAdictionary.GetSY_FIELDS(conn);
				CSCS_GUI.Adictionary.SY_INDEXESList = AdictionaryLocal.CacheAdictionary.GetSY_INDEXES(conn);

				conn.Close();

				return true;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void FillDatabasesDictionary()
		{
			foreach (var db in Adictionary.SY_DATABASESList)
			{
				Databases[db.SYCD_USERCODE] = db.SYCD_DBASENAME;
			}
		}

		public ParsingScript GetScript(CSCS_GUI gui, FrameworkElement widget)
		{
			if (!gui.Control2Window.TryGetValue(widget, out Window win))
			{
				return ParsingScript.Default(gui);
			}
			return GetScript(win);
		}

		public ParsingScript GetScript(Window window)
		{
			if (window != null && Window2File.TryGetValue(window, out string filename) &&
				ChainFunction.Chains.TryGetValue(filename, out ParsingScript result))
			{
				return result;
			}
			return ParsingScript.Default(this);
		}

		public void CacheWindow(Window window, string filename)
		{
			if (!string.IsNullOrWhiteSpace(filename))
			{
				Window2File[window] = filename;
				File2Window[filename] = window;
			}
		}
		public void UncacheWindow(Window window, string tag)
		{
			if (Window2File.TryGetValue(window, out string filename))
			{
				Window2File.Remove(window);
				File2Window.Remove(filename);
			}
			Tag2Parent.Remove(tag);
		}

		public void CloseAllWindows()
		{
			CSCS_GUI.Dispatcher.Invoke((Action)delegate ()
			{
				var keys = Window2File.Keys.ToList();
				foreach (var win in keys)
				{
					if (win == CSCS_GUI.MainWindow)
					{
						continue;
					}
					var filename = Window2File[win];
					win.Close();
					Window2File.Remove(win);
					File2Window.Remove(filename);
				}
				var rest = Window2File.Count;
			}, null);
		}

		public static string GetWidgetBindingName(FrameworkElement widget)
		{
			if (widget == null)
			{
				return "";
			}
			var nm = string.IsNullOrWhiteSpace(widget.Name) ? "" : widget.Name;
			var dc = widget.DataContext == null ? "" : widget.DataContext.ToString();
			var bindingName = string.IsNullOrWhiteSpace(dc) ? nm : dc;
			return bindingName;
		}

		public static string GetWidgetName(FrameworkElement widget)
		{
			if (widget == null || string.IsNullOrWhiteSpace(widget.Name))
			{
				return "";
			}
			var widgetName = widget.Name.ToString();
			return widgetName;
		}

		static string s_variableChanged;
		public void OnVariableChange(string name, Variable newValue, bool exists = true)
		{
			if (ChangingBoundVariable)
			{
				return;
			}
			if (!exists && RequireDEFINE != "false" && (RequireDEFINE == "*" || name.StartsWith(RequireDEFINE)))
			{
				throw new ArgumentException("Variable [" + name + "] must be defined with DEFINE function first.");
			}

			var widgetName = name.ToLower();
			if (string.Equals(widgetName, s_variableChanged) ||
				!m_boundVariables.TryGetValue(widgetName, out Variable bounded))
			{
				return;
			}

			s_variableChanged = widgetName;
			var widget = GetWidget(widgetName);
			if (m_addingActions && string.IsNullOrEmpty(newValue.String))
			{
				var text = bounded.AsString();
				newValue.String = string.IsNullOrWhiteSpace(text) ? newValue.AsString() : text;
			}

			SetTextWidgetFunction.SetText(widget, newValue.AsString());
			if (m_bound2Widgets.TryGetValue(widgetName, out List<FrameworkElement> widgets))
			{
				foreach (var wid in widgets.ToList())
				{
					SetTextWidgetFunction.SetText(wid, newValue.AsString());
				}
				var ct = widgets.Count;
			}

			m_boundVariables[widgetName] = newValue;
			s_variableChanged = null;
		}

		void UpdateVariable(FrameworkElement widget, Variable newValue)
		{
			var widgetName = GetWidgetBindingName(widget).ToLower();
			if (string.IsNullOrWhiteSpace(widgetName))
			{
				return;
			}

			if (ChangingBoundVariable)
			{
				return;
			}
			var prev = ChangingBoundVariable;
			ChangingBoundVariable = true;
			if (DEFINES.TryGetValue(widgetName, out DefineVariable defVar))
			{
				var assign = new MyAssignFunction(widgetName);
				assign.DoAssign(Script, widgetName, defVar, ref newValue);
				ChangingBoundVariable = false;
				return;
			}

			Interpreter.AddGlobalOrLocalVariable(widgetName,
							new GetVarFunction(newValue));
			ChangingBoundVariable = false;
		}

		static void Print(object sender, OutputAvailableEventArgs e)
		{
			System.Diagnostics.Trace.WriteLine(e.Output);
		}

		static bool m_addingActions;
		public void AddActions(Window win, bool force = false, ParsingScript script = null)
		{
			m_addingActions = true;
			var controls = CacheControls(win, force);
			foreach (var entry in controls)
			{
				AddWidgetActions(entry);
			}
			m_addingActions = false;
		}

		public Variable RunScript(string funcName, Window win, Variable arg1, Variable arg2 = null)
		{
			CustomFunction customFunction = Interpreter.GetFunction(funcName) as CustomFunction;
			if (customFunction != null)
			{
				List<Variable> args = new List<Variable>();
				args.Add(arg1);
				args.Add(arg2 != null ? arg2 : Variable.EmptyInstance);

				var script = GetScript(win);
				if (script != null && script.StackLevel != null)
				{
					foreach (var item in script.StackLevel.Variables)
					{
						if (!string.IsNullOrWhiteSpace(item.Key))
						{
							var func = item.Value as GetVarFunction;
							func.Value.ParamName = item.Key;
							args.Add(func.Value);
						}
					}
				}
				try
				{
					return Interpreter.Run(customFunction, args, script);
				}
				catch (Exception exc)
				{
					Console.WriteLine("Exception: " + exc.Message);
					Console.WriteLine(exc.StackTrace);
					MessageBoxFunction.ShowMessageBox(exc.Message, "Error", "ok", "error");
					throw;
				}
			}
			return Variable.EmptyInstance;
		}

		public bool AddBinding(string name, FrameworkElement widget)
		{
			var text = GetTextWidgetFunction.GetText(widget);
			Variable baseValue = text;// new Variable(text);
			Interpreter.AddGlobal(name, new GetVarFunction(baseValue), false /* not native */);

			//var current = new Variable(widget);
			var current = new Variable(baseValue);
			if (widget is DataGrid)
			{
				var dg = widget as DataGrid;
				WidgetData wd = new WidgetData(dg);
				DEFINES[name] = new DefineVariable(name, "datagrid", dg);
				for (int i = 0; i < dg.Columns.Count; i++)
				{
					var textCol = dg.Columns[i] as DataGridTextColumn;
					var templCol = dg.Columns[i] as DataGridTemplateColumn;
					var header = textCol != null ? textCol.Header as string : templCol.Header as string;
					if (textCol != null && textCol.Binding == null)
					{
						textCol.Binding = new Binding(header);
					}
					if (textCol != null)
					{
						Binding binding = textCol.Binding as Binding;
						wd.headerBindings.Add(binding.Path.Path);

						if (!string.IsNullOrWhiteSpace(header))
						{
							var headerStr = binding.Path.Path.ToLower();
							var headerVar = new DefineVariable(headerStr, "datagrid", dg, i);
							headerVar.InitFromExisting(this, headerStr);
							DEFINES[headerStr] = headerVar;
							wd.headers[headerStr] = headerVar;
							wd.headerNames.Add(headerStr);
							wd.colTypes.Add(WidgetData.COL_TYPE.STRING);

							//var array = new DefineVariable(new List<Variable>());
							Interpreter.AddGlobal(headerStr, new GetVarFunction(headerVar), false /* not native */);
						}
					}
				}
				WIDGETS[name] = wd;
				dg.Sorting += Dg_Sorting;
				//dg.ItemsSource = new 
			}

			name = name.ToLower();
			m_boundVariables[name] = current;

			if (!m_bound2Widgets.TryGetValue(name, out List<FrameworkElement> widgets))
			{
				widgets = new List<FrameworkElement>();
			}
			widgets.Add(widget);
			m_bound2Widgets[name] = widgets;

			if (DEFINES.TryGetValue(name, out DefineVariable defVar))
			{
				OnVariableChange(name, defVar);
			}
			return true;
		}

		private void Dg_Sorting(object sender, DataGridSortingEventArgs e)
		{
			var dg = sender as DataGrid;
			string dgName = dg.DataContext as string;
			var funcName = dgName + "@Header";
			RunScript(funcName, dg.Parent as Window, new Variable(dgName), new Variable(e.Column.DisplayIndex));

			Dispatcher.BeginInvoke((Action)delegate ()
			{
				IEnumerable<ExpandoObject> sortedCast = null;
				try
				{
					var casted = dg.Items.Cast<ExpandoObject>();
					sortedCast = casted.ToList();
				}
				catch (Exception exc)
				{
					Console.WriteLine(exc);
					var sorted = dg.Items.SourceCollection;
					sortedCast = sorted.Cast<ExpandoObject>();
				}

				FillWidgetFunction.ResetArrays(this, sender as FrameworkElement, sortedCast);
			}, null);

			//Console.WriteLine(e.Handled);
		}

		public bool OnAddingRow(DataGrid dg)
		{
			string dgName = dg.DataContext as string;
			var funcName = dgName + "@Add";
			var rowList = dg.ItemsSource as List<ExpandoObject>;
			var currentSize = rowList == null ? 0 : rowList.Count;
			var res = RunScript(funcName, dg.Parent as Window, new Variable(dgName), new Variable(currentSize));
			bool canAddRow = res == Variable.EmptyInstance || res.AsDouble() != 0;

			return canAddRow;
		}

		public bool AddActionHandler(string name, string action, FrameworkElement widget)
		{
			var clickable = widget as ButtonBase;
			if (widget is ASNumericBox)
			{
				var numericBoxsGrid = (widget as ASNumericBox).Content;
				var gridChildren = (numericBoxsGrid as Grid).Children;
				foreach (var item in gridChildren)
				{
					if (item is ButtonBase)
					{
						clickable = item as ButtonBase;
					}
				}
			}

			if (clickable == null)
			{
				return false;
			}
			m_actionHandlers[name] = action;
			clickable.Click -= new RoutedEventHandler(Widget_Click);
			clickable.Click += new RoutedEventHandler(Widget_Click);
			return true;
		}

		public bool AddGroupBoxClickedHandler(string name, string action, FrameworkElement widget)
		{
			var clickable = widget as GroupBox;

			if (clickable == null)
			{
				return false;
			}
			m_actionHandlers[name] = action;
			clickable.MouseDoubleClick -= new MouseButtonEventHandler(GroupBox_Click);
			clickable.MouseDoubleClick += new MouseButtonEventHandler(GroupBox_Click);
			return true;
		}

		public bool AddRadioButtonClickedHandler(string name, string action, FrameworkElement widget)
		{
			var clickable = widget as RadioButton;

			if (clickable == null)
			{
				return false;
			}
			m_actionHandlers[name] = action;
			clickable.Click -= new RoutedEventHandler(RadioButton_Click);
			clickable.Click += new RoutedEventHandler(RadioButton_Click);
			return true;
		}

		public bool AddPreActionHandler(string name, string action, FrameworkElement widget)
		{
			m_preActionHandlers[name] = action;
			if (widget is ComboBox)
			{
				var combo = widget as ComboBox;
				combo.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(Widget_PreClick);
				return true;
			}
			widget.MouseDown += new MouseButtonEventHandler(Widget_PreClick);
			return true;
		}
		public bool AddPostActionHandler(string name, string action, FrameworkElement widget)
		{
			m_postActionHandlers[name] = action;
			if (widget is ComboBox)
			{
				var combo = widget as ComboBox;
				combo.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(Widget_PostClick);
				return true;
			}
			widget.MouseUp += new MouseButtonEventHandler(Widget_PostClick);
			return true;
		}

		public bool AddKeyDownHandler(string name, string action, FrameworkElement widget)
		{
			m_keyDownHandlers[name] = action;
			widget.KeyDown += new KeyEventHandler(Widget_KeyDown);
			return true;
		}
		public bool AddKeyUpHandler(string name, string action, FrameworkElement widget)
		{
			m_keyUpHandlers[name] = action;
			widget.KeyUp += new KeyEventHandler(Widget_KeyUp);
			return true;
		}
		public bool AddTextChangedHandler(string name, string action, FrameworkElement widget)
		{
			if (widget is ASDateEditer)
			{
				var dateEditer = widget as ASDateEditer;
				if (dateEditer == null)
				{
					return false;
				}

				return false;

				m_textChangedHandlers[name] = action;
				// x2
				dateEditer.SelectedDateChanged -= new EventHandler<SelectionChangedEventArgs>(Widget_DateChanged);
				dateEditer.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(Widget_DateChanged);

				return true;
			}
			else if (widget is ASDateEditer2)
			{
				var dateEditer2 = widget as ASDateEditer2;
				if (dateEditer2 == null)
				{
					return false;
				}

				m_textChangedHandlers[name] = action;
				// x2
				//dateEditer2.SelectedDateChanged -= new EventHandler<SelectionChangedEventArgs>(Widget_DateChanged);
				//dateEditer2.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(Widget_DateChanged);

				return true;
			}

			var textable = widget as TextBoxBase;
			if (textable == null)
			{
				return false;
			}

			if (textable.Parent is Grid grid)
			{
				if (grid.Parent is ASDateEditer2 asde2)
				{
					m_textChangedHandlers[name] = action;

					textable.TextChanged -= dateEditer2_TextChanged;
					textable.TextChanged += dateEditer2_TextChanged;

					return true;
				}
			}

			m_textChangedHandlers[name] = action;
			// x2
			textable.TextChanged -= new TextChangedEventHandler(Widget_TextChanged);
			textable.TextChanged += new TextChangedEventHandler(Widget_TextChanged);

			return true;
		}

		public bool AddLostFocusHandler(string name, string action, FrameworkElement widget)
		{
			if (widget is ASDateEditer asde)
			{
				m_lostFocusHandlers[name] = action;

				asde.LostFocus -= widget_LostFocus;
				asde.LostFocus += widget_LostFocus;

				return true;
			}
			//else if(widget is ComboBox cb)
			//{
			//             //m_lostFocusHandlers[name] = action;

			//	//cb.LostFocus -= comboBox_LostFocus;
			//	//cb.LostFocus += comboBox_LostFocus;

			//	//cb.SelectionChanged -= comboBox_SelectionChanged;
			//	//cb.SelectionChanged += comboBox_SelectionChanged;

			//             return true;
			//         }

			var textable = widget as TextBoxBase;
			if (textable == null)
			{
				return false;
			}

			if (textable.Parent is Grid grid)
			{
				if (grid.Parent is ASDateEditer2 asde2)
				{
					m_lostFocusHandlers[name] = action;

					textable.LostFocus -= dateEditer2_LostFocus;
					textable.LostFocus += dateEditer2_LostFocus;

					return true;
				}
			}

			return false;
		}

		private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			FrameworkElement widget = sender as FrameworkElement;

			var widgetName = GetWidgetName(widget);
			if (string.IsNullOrWhiteSpace(widgetName) ||
				m_updatingWidget.Contains(widgetName))
			{
				return;
			}

			//var text = GetTextWidgetFunction.GetText(widget);
			var text2 = new Variable(((ComboBox)widget).SelectedValue.ToString());

			FrameworkElement widget2 = sender as FrameworkElement;
			var widgetName2 = GetWidgetBindingName(widget2);
			if (string.IsNullOrWhiteSpace(widgetName2) ||
				m_updatingWidget.Contains(widgetName2))
			{
				return;
			}

			m_updatingWidget.Add(widgetName2);
			UpdateVariable(widget2, text2);

			string funcName;
			if (m_selChangedHandlers.TryGetValue(widgetName, out funcName))
			{
				Control2Window.TryGetValue(widget, out Window win);
				Interpreter.Run(funcName, new Variable(widgetName), text2,
					Variable.EmptyInstance, GetScript(win));
			}
			m_updatingWidget.Remove(widgetName2);
		}

		private void comboBox_LostFocus(object sender, RoutedEventArgs e)
		{
			FrameworkElement widget = sender as FrameworkElement;

			var widgetName = GetWidgetName(widget);
			if (string.IsNullOrWhiteSpace(widgetName) ||
				m_updatingWidget.Contains(widgetName))
			{
				return;
			}

			var text = GetTextWidgetFunction.GetText(widget);

			FrameworkElement widget2 = sender as FrameworkElement;
			var widgetName2 = GetWidgetBindingName(widget2);
			if (string.IsNullOrWhiteSpace(widgetName2) ||
				m_updatingWidget.Contains(widgetName2))
			{
				return;
			}

			m_updatingWidget.Add(widgetName2);
			UpdateVariable(widget2, text);

			string funcName;
			if (m_lostFocusHandlers.TryGetValue(widgetName, out funcName))
			{
				Control2Window.TryGetValue(widget, out Window win);
				Interpreter.Run(funcName, new Variable(widgetName), text,
					Variable.EmptyInstance, GetScript(win));
			}
			m_updatingWidget.Remove(widgetName2);
		}

		private void widget_LostFocus(object sender, RoutedEventArgs e)
		{
			FrameworkElement widget = sender as FrameworkElement;

			string funcName;
			if (m_lostFocusHandlers.TryGetValue(widget.Name, out funcName))
			{
				Control2Window.TryGetValue(widget, out Window win);
				Interpreter.Run(funcName, new Variable(""), new Variable(""),
					Variable.EmptyInstance, GetScript(win));
			}

		}

		private void dateEditer2_LostFocus(object sender, RoutedEventArgs e)
		{
			TextBoxBase widget = sender as TextBoxBase;
			var widgetName = GetWidgetName(widget);
			if (string.IsNullOrWhiteSpace(widgetName) ||
				m_updatingWidget.Contains(widgetName))
			{
				return;
			}

			//if(sender is ASNumericTextBox)
			//{
			//    var asntb = (sender as ASNumericTextBox);
			//    if (asntb.SkipWidgetTextChanged)
			//    {
			//        asntb.SkipWidgetTextChanged = false;
			//        ///return;
			//    }
			//}

			//m_updatingWidget.Add(widgetName);
			var text = GetTextWidgetFunction.GetText(widget);
			//UpdateVariable(widget, text);
			//m_updatingWidget.Remove(widgetName);
			//
			TextBoxBase widget2 = sender as TextBoxBase;
			var widgetName2 = GetWidgetBindingName(widget2);
			if (string.IsNullOrWhiteSpace(widgetName2) ||
				m_updatingWidget.Contains(widgetName2))
			{
				return;
			}

			m_updatingWidget.Add(widgetName2);
			//var text = GetTextWidgetFunction.GetText(widget2);
			if (DEFINES.TryGetValue(widgetName2.ToLower(), out DefineVariable defVar))
			{
				switch (defVar.DefType)
				{
					case "a":
						UpdateVariable(widget2, text);
						break;

					case "i":
					case "n":
					case "r":
					case "b":
						if (double.TryParse(text.AsString(), out double parsedDouble))
						{
							if (text.AsString() == parsedDouble.ToString())
							{
								UpdateVariable(widget2, new Variable(parsedDouble));
							}

						}
						break;

					case "d":
						if (DateTime.TryParseExact(text.AsString(), defVar.GetDateFormat(), CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
						{
							UpdateVariable(widget2, text/*new Variable(dt)*/);
						}
						else if (text.AsString() == "00/00/00")
						{
							UpdateVariable(widget2, new Variable("01/01/00"));
						}
						else if (text.AsString() == "00/00/0000")
						{
							UpdateVariable(widget2, new Variable("01/01/1900"));
						}
						break;
					case "t":
						//if (true)
						//{
						//    if (TimeSpan.TryParse(text.AsString(), out TimeSpan result))
						//    {
						//        UpdateVariable(widget2, text);
						//    }
						//}
						break;

					default:
						//UpdateVariable(widget2, text);
						break;
				}
			}

			string funcName;
			if (m_lostFocusHandlers.TryGetValue(widgetName, out funcName))
			{
				Control2Window.TryGetValue(widget, out Window win);
				Interpreter.Run(funcName, new Variable(widgetName), text,
					Variable.EmptyInstance, GetScript(win));
			}
			m_updatingWidget.Remove(widgetName2);
		}

		private void dateEditer2_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextBoxBase widget = sender as TextBoxBase;
			var widgetName = GetWidgetName(widget);
			if (string.IsNullOrWhiteSpace(widgetName) ||
				m_updatingWidget.Contains(widgetName))
			{
				return;
			}

			var text = GetTextWidgetFunction.GetText(widget);

			string funcName;
			if (m_textChangedHandlers.TryGetValue(widgetName, out funcName))
			{
				Control2Window.TryGetValue(widget, out Window win);
				Interpreter.Run(funcName, new Variable(widgetName), text,
					Variable.EmptyInstance, GetScript(win));
			}
		}

		public bool AddSelectionChangedHandler(string name, string action, FrameworkElement widget)
		{
			if (widget is ComboBox cb)
			{
				m_selChangedHandlers[name] = action;

				cb.SelectionChanged -= comboBox_SelectionChanged;
				cb.SelectionChanged += comboBox_SelectionChanged;

				return true;
			}

			var sel = widget as Selector;
			if (sel == null)
			{
				return false;
			}
			m_selChangedHandlers[name] = action;
			sel.SelectionChanged += new SelectionChangedEventHandler(Widget_SelectionChanged);
			return true;
		}
		public bool AddRadioButtonSelectedChangedHandler(string name, string action, FrameworkElement widget)
		{
			var sel = widget as RadioButton;
			if (sel == null)
			{
				return false;
			}
			m_selChangedHandlers[name] = action;
			sel.Checked += new RoutedEventHandler(RadioButton_Checked);
			sel.Unchecked += new RoutedEventHandler(RadioButton_Unchecked);
			return true;
		}
		public bool AddDateChangedHandler(string name, string action, FrameworkElement widget)
		{
			var datePicker = widget as DatePicker;
			if (datePicker == null)
			{
				return false;
			}

			if (widget is ASDateEditer)
			{
				m_dateSelectedHandlers[name] = action;
				datePicker.SelectedDateChanged -= new EventHandler<SelectionChangedEventArgs>(Widget_DateChanged);
				datePicker.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(Widget_DateChanged);
				return true;
			}

			m_dateSelectedHandlers[name] = action;
			datePicker.SelectedDateChanged += DatePicker_SelectedDateChanged;

			return true;
		}

		public bool AddWidgetPreHandler(string name, string action, FrameworkElement widget)
		{
			//var textable = widget as TextBoxBase;
			var textable = widget as Control;
			//if(widget is ASEnterBox)
			//{
			//    var ASEnterBox = widget as ASEnterBox;
			//}
			if (textable == null)
			{
				return false;
			}

			m_PreHandlers[name] = action;
			textable.GotFocus -= new RoutedEventHandler(Widget_Pre);
			textable.GotFocus += new RoutedEventHandler(Widget_Pre);

			return true;
		}

		public bool AddWidgetPostHandler(string name, string action, FrameworkElement widget)
		{
			var textable = widget as Control;
			if (textable == null)
			{
				return false;
			}

			m_PostHandlers[name] = action;
			textable.PreviewLostKeyboardFocus -= new KeyboardFocusChangedEventHandler(Widget_Post);
			textable.PreviewLostKeyboardFocus += new KeyboardFocusChangedEventHandler(Widget_Post);

			//textable.LostFocus -= Textable_LostFocus;
			//textable.LostFocus += Textable_LostFocus;

			return true;
		}

		private void Textable_LostFocus(object sender, RoutedEventArgs e)
		{

		}

		public bool AddWidgetChangeHandler(string name, string action, FrameworkElement widget)
		{
			if (widget is TabControl)
			{
				var tabControl = widget as TabControl;
				if (tabControl == null)
				{
					return false;
				}

				m_ChangeHandlers[name] = action;
				tabControl.SelectionChanged += new SelectionChangedEventHandler(Widget_Change);

				return true;
			}
			else if (widget is ASNavigator)
			{
				var nav = widget as WpfControlsLibrary.ASNavigator;
				if (nav == null)
				{
					return false;
				}

				m_NavigatorChangeHandlers[name] = action;
				nav.Navigator_buttonClicked += new EventHandler(Navigator_Change);

				return true;
			}

			return false;
		}

		public bool AddWidgetAfterChangeHandler(string name, string action, FrameworkElement widget)
		{
			if (widget is ASNavigator)
			{
				var nav = widget as WpfControlsLibrary.ASNavigator;
				if (nav == null)
				{
					return false;
				}

				m_NavigatorAfterChangeHandlers[name] = action;
				nav.Navigator_buttonClicked -= new EventHandler(Navigator_AfterChange);
				nav.Navigator_buttonClicked += new EventHandler(Navigator_AfterChange);

				return true;
			}

			return false;
		}

		public bool AddWidgetMoveHandler(string name, string action, FrameworkElement widget)
		{
			if (widget is DataGrid)
			{
				var dg = widget as DataGrid;
				if (dg == null)
				{
					return false;
				}

				m_MoveHandlers[name] = action;
				dg.SelectionChanged -= new SelectionChangedEventHandler(DataGrid_Move);
				dg.SelectionChanged += new SelectionChangedEventHandler(DataGrid_Move);

				return true;
			}

			return false;
		}

		public bool AddWidgetSelectHandler(string name, string action, FrameworkElement widget)
		{
			if (widget is DataGrid)
			{
				var dg = widget as DataGrid;
				if (dg == null)
				{
					return false;
				}

				m_SelectHandlers[name] = action;
				dg.MouseDoubleClick -= new MouseButtonEventHandler(DataGrid_Select);
				dg.MouseDoubleClick += new MouseButtonEventHandler(DataGrid_Select);

				dg.PreviewKeyDown += new KeyEventHandler(DataGrid_EnterKeyPressed);
				dg.PreviewKeyDown += new KeyEventHandler(DataGrid_EnterKeyPressed);

				return true;
			}

			return false;
		}

		private void ValueUpdated(string funcName, string widgetName, FrameworkElement widget, Variable newValue)
		{
			UpdateVariable(widget, newValue);
			Control2Window.TryGetValue(widget, out Window win);
			ActiveWindow = win;
			RunScript(funcName, win, new Variable(widgetName), newValue);
			ActiveWindow = null;
		}

		//public bool skipSelectedDateChanged = false;

		private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
		{
			var widget = sender as FrameworkElement;
			var widgetBindingName = GetWidgetBindingName(widget);
			var widgetName = GetWidgetName(widget);
			var picker = sender as DatePicker;
			DateTime? date = picker?.SelectedDate;
			if (string.IsNullOrWhiteSpace(widgetBindingName) || date == null ||
			   !m_dateSelectedHandlers.TryGetValue(widgetName, out string funcName))
			{
				return;
			}
			if (DEFINES.TryGetValue(widgetBindingName.ToLower(), out DefineVariable defVar))
			{
				//skipSelectedDateChanged = false;
				var dateFormat = defVar.GetDateFormat();
				ValueUpdated(funcName, widgetName, widget, new Variable(date.Value.ToString(dateFormat)));
			}
		}

		public bool AddMouseHoverHandler(string name, string action, FrameworkElement widget)
		{
			m_mouseHoverHandlers[name] = action;
			widget.MouseEnter += new MouseEventHandler(Widget_Hover);
			return true;
		}

		static bool shouldButtonClick = false;

		private void Widget_Click(object sender, RoutedEventArgs e)
		{
			LastObjClickedWidgetName = ((Control)sender).Name;
			LastObjWidgetName = LastObjClickedWidgetName;

			var widget = sender as FrameworkElement;
			var widgetName = GetWidgetName(widget);
			if (string.IsNullOrEmpty(widgetName))
			{
				return;
			}

			shouldButtonClick = true;
			if (sender is Button)
			{
				var btn = sender as Button;
				if (btn.Parent is FrameworkElement)
				{
					var parent1 = btn.Parent;
					if ((parent1 as FrameworkElement).Parent is ASEnterBox)
					{
						var entBox = (parent1 as FrameworkElement).Parent as ASEnterBox;

						var entBoxGrid = entBox.Content as Grid;
						foreach (var item in entBoxGrid.Children)
						{
							if (item is ASEnterTextBox)
							{
								var entTB = item as ASEnterTextBox;
								entTB.Focus();

								//if (((Control)e.NewFocus).Name == entTB.Name)
								//{
								//    return;
								//}
								if (!shouldButtonClick)
									return;
								break;
							}
						}
					}
					else if ((parent1 as FrameworkElement).Parent is ASNumericBox)
					{
						var numBox = (parent1 as FrameworkElement).Parent as ASNumericBox;

						var numBoxGrid = numBox.Content as Grid;
						foreach (var item in numBoxGrid.Children)
						{
							if (item is ASNumericTextBox)
							{
								var numTB = item as ASNumericTextBox;
								numTB.Focus();

								//if (((Control)e.NewFocus).Name == numTB.Name)
								//{
								//    return;
								//}
								if (!shouldButtonClick)
									return;
								break;
							}
						}
					}
				}
			}


			string funcName;
			if (!m_actionHandlers.TryGetValue(widgetName, out funcName))
			{
				return;
			}

			Variable result = null;
			if (widget is CheckBox)
			{
				var checkBox = widget as CheckBox;
				var val = checkBox.IsChecked == true ? true : false;
				result = new Variable(val);
			}
			else
			{
				result = new Variable(widgetName);
			}

			if (widget is Button)
			{
				if (widget.Parent is Grid)
				{
					if ((widget.Parent as Grid).Parent is ASNumericBox)
					{
						var nb = (widget.Parent as Grid).Parent as ASNumericBox;
						nb.FormatNumericTextBox();
					}
				}
			}

			ValueUpdated(funcName, widgetName, widget, result);
		}

		private void GroupBox_Click(object sender, MouseButtonEventArgs e)
		{
			LastObjClickedWidgetName = ((Control)sender).Name;
			LastObjWidgetName = LastObjClickedWidgetName;

			var widget = sender as FrameworkElement;
			var widgetName = GetWidgetName(widget);
			if (string.IsNullOrEmpty(widgetName))
			{
				return;
			}

			shouldButtonClick = true;
			if (sender is Button)
			{
				var btn = sender as Button;
				if (btn.Parent is FrameworkElement)
				{
					var parent1 = btn.Parent;
					if ((parent1 as FrameworkElement).Parent is ASEnterBox)
					{
						var entBox = (parent1 as FrameworkElement).Parent as ASEnterBox;

						var entBoxGrid = entBox.Content as Grid;
						foreach (var item in entBoxGrid.Children)
						{
							if (item is ASEnterTextBox)
							{
								var entTB = item as ASEnterTextBox;
								entTB.Focus();

								//if (((Control)e.NewFocus).Name == entTB.Name)
								//{
								//    return;
								//}
								if (!shouldButtonClick)
									return;
								break;
							}
						}
					}
					else if ((parent1 as FrameworkElement).Parent is ASNumericBox)
					{
						var numBox = (parent1 as FrameworkElement).Parent as ASNumericBox;

						var numBoxGrid = numBox.Content as Grid;
						foreach (var item in numBoxGrid.Children)
						{
							if (item is ASNumericTextBox)
							{
								var numTB = item as ASNumericTextBox;
								numTB.Focus();

								//if (((Control)e.NewFocus).Name == numTB.Name)
								//{
								//    return;
								//}
								if (!shouldButtonClick)
									return;
								break;
							}
						}
					}
				}
			}


			string funcName;
			if (!m_actionHandlers.TryGetValue(widgetName, out funcName))
			{
				return;
			}

			Variable result = null;
			if (widget is CheckBox)
			{
				var checkBox = widget as CheckBox;
				var val = checkBox.IsChecked == true ? true : false;
				result = new Variable(val);
			}
			else
			{
				result = new Variable(widgetName);
			}

			if (widget is Button)
			{
				if (widget.Parent is Grid)
				{
					if ((widget.Parent as Grid).Parent is ASNumericBox)
					{
						var nb = (widget.Parent as Grid).Parent as ASNumericBox;
						nb.FormatNumericTextBox();
					}
				}
			}

			ValueUpdated(funcName, widgetName, widget, result);
		}

		private void RadioButton_Click(object sender, RoutedEventArgs e)
		{
			LastObjClickedWidgetName = ((Control)sender).Name;
			LastObjWidgetName = LastObjClickedWidgetName;

			var widget = sender as FrameworkElement;
			var widgetName = GetWidgetName(widget);
			if (string.IsNullOrEmpty(widgetName))
			{
				return;
			}

			string funcName;
			if (m_actionHandlers.TryGetValue(widgetName, out funcName))
			{
				Control2Window.TryGetValue(widget, out Window win);
				Interpreter.Run(funcName, new Variable(widgetName), Variable.EmptyInstance, Variable.EmptyInstance,
					GetScript(win));
			}

		}

		private void Widget_PreClick(object sender, MouseButtonEventArgs e)
		{
			var widget = sender as FrameworkElement;
			var widgetName = GetWidgetName(widget);
			if (string.IsNullOrWhiteSpace(widgetName) || e.ChangedButton != MouseButton.Left)
			{
				return;
			}

			string funcName;
			if (m_preActionHandlers.TryGetValue(widgetName, out funcName))
			{
				var arg = GetTextWidgetFunction.GetText(widget);
				Control2Window.TryGetValue(widget, out Window win);
				Interpreter.Run(funcName, new Variable(widgetName), new Variable(arg), Variable.EmptyInstance,
					GetScript(win));
			}
		}

		private void Widget_PostClick(object sender, MouseButtonEventArgs e)
		{
			var widget = sender as FrameworkElement;
			var widgetName = GetWidgetName(widget);
			if (string.IsNullOrWhiteSpace(widgetName) || e.ChangedButton != MouseButton.Left)
			{
				return;
			}

			string funcName;
			if (m_postActionHandlers.TryGetValue(widgetName, out funcName))
			{
				var arg = GetTextWidgetFunction.GetText(widget);
				Control2Window.TryGetValue(widget, out Window win);
				Interpreter.Run(funcName, new Variable(widgetName), new Variable(arg),
					Variable.EmptyInstance, GetScript(win));
			}
		}

		private void Widget_KeyDown(object sender, KeyEventArgs e)
		{
			var widget = sender as FrameworkElement;
			var widgetName = GetWidgetName(widget);
			if (string.IsNullOrWhiteSpace(widgetName))
			{
				return;
			}

			string funcName;
			if (m_keyDownHandlers.TryGetValue(widgetName, out funcName))
			{
				Control2Window.TryGetValue(widget, out Window win);
				Interpreter.Run(funcName, new Variable(widgetName),
					new Variable(((char)e.Key).ToString()),
					Variable.EmptyInstance, GetScript(win));
			}
		}
		private void Widget_KeyUp(object sender, KeyEventArgs e)
		{
			var widget = sender as FrameworkElement;
			var widgetName = GetWidgetName(widget);
			if (string.IsNullOrWhiteSpace(widgetName))
			{
				return;
			}

			string funcName;
			if (m_keyUpHandlers.TryGetValue(widgetName, out funcName))
			{
				Control2Window.TryGetValue(widget, out Window win);
				Interpreter.Run(funcName, new Variable(widgetName),
					new Variable(((char)e.Key).ToString()),
					Variable.EmptyInstance, GetScript(win));
			}
		}

		private void Widget_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextBoxBase widget = sender as TextBoxBase;
			var widgetName = GetWidgetName(widget);
			if (string.IsNullOrWhiteSpace(widgetName) ||
				m_updatingWidget.Contains(widgetName))
			{
				return;
			}

			//if(sender is ASNumericTextBox)
			//{
			//    var asntb = (sender as ASNumericTextBox);
			//    if (asntb.SkipWidgetTextChanged)
			//    {
			//        asntb.SkipWidgetTextChanged = false;
			//        ///return;
			//    }
			//}

			//m_updatingWidget.Add(widgetName);
			var text = GetTextWidgetFunction.GetText(widget);
			//UpdateVariable(widget, text);
			//m_updatingWidget.Remove(widgetName);
			//
			TextBoxBase widget2 = sender as TextBoxBase;
			var widgetName2 = GetWidgetBindingName(widget2);
			if (string.IsNullOrWhiteSpace(widgetName2) ||
				m_updatingWidget.Contains(widgetName2))
			{
				return;
			}

			m_updatingWidget.Add(widgetName2);
			//var text = GetTextWidgetFunction.GetText(widget2);
			if (DEFINES.TryGetValue(widgetName2.ToLower(), out DefineVariable defVar))
			{
				switch (defVar.DefType)
				{
					case "a":
						UpdateVariable(widget2, text);
						break;

					case "i":
					case "n":
					case "r":
					case "b":
						if (double.TryParse(text.AsString(), out double parsedDouble))
						{
							if (text.AsString() == parsedDouble.ToString())
							{
								UpdateVariable(widget2, new Variable(parsedDouble));
							}

						}
						break;

					case "d":

						break;
					case "t":
						//if (true)
						//{
						//    if (TimeSpan.TryParse(text.AsString(), out TimeSpan result))
						//    {
						//        UpdateVariable(widget2, text);
						//    }
						//}
						break;

					default:
						//UpdateVariable(widget2, text);
						break;
				}
			}

			string funcName;
			if (m_textChangedHandlers.TryGetValue(widgetName, out funcName))
			{
				Control2Window.TryGetValue(widget, out Window win);
				Interpreter.Run(funcName, new Variable(widgetName), text,
					Variable.EmptyInstance, GetScript(win));
			}
			m_updatingWidget.Remove(widgetName2);
		}

		private void Widget_DateChanged(object sender, SelectionChangedEventArgs e)
		{
			var widget = sender as ASDateEditer;
			var widgetName = GetWidgetBindingName(widget);

			//var text = GetTextWidgetFunction.GetText(widget);
			DateTime? text = widget.SelectedDate;

			m_updatingWidget.Add(widgetName);
			//var text = GetTextWidgetFunction.GetText(widget2);
			if (DEFINES.TryGetValue(widgetName.ToLower(), out DefineVariable defVar))
			{
				switch (defVar.DefType)
				{
					case "a":
						//UpdateVariable(widget, text);
						break;

					case "i":
					case "n":
					case "r":
					case "b":
						//if (double.TryParse(text.AsString(), out double parsedDouble))
						//{
						//    UpdateVariable(widget, new Variable(parsedDouble));
						//}
						break;

					case "d":
						var variableName = GetWidgetBindingName(widget).ToLower();
						if (DEFINES.TryGetValue(variableName, out DefineVariable defVar2))
						{
							UpdateVariable(widget, new Variable(text?.ToString(defVar2.GetDateFormat())));
						}
						break;
					case "t":
						//if (true)
						//{
						//    if (TimeSpan.TryParse(text.AsString(), out TimeSpan result))
						//    {
						//        UpdateVariable(widget2, text);
						//    }
						//}
						break;

					default:
						//UpdateVariable(widget2, text);
						break;
				}
			}



			string funcName;
			if (m_dateSelectedHandlers.TryGetValue(widget.Name, out funcName))
			{
				var item = e.AddedItems.Count > 0 ? e.AddedItems[0].ToString() : e.RemovedItems.Count > 0 ? e.RemovedItems[0].ToString() : "";
				Control2Window.TryGetValue(widget, out Window win);
				Interpreter.Run(funcName, new Variable(widgetName), new Variable(item),
					Variable.EmptyInstance, GetScript(win));
			}

			m_updatingWidget.Remove(widgetName);
		}

		public Dictionary<string, List<object>> gridsSelectedRow = new Dictionary<string, List<object>>();

		private void Widget_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var widget = sender as Selector;
			var widgetName = GetWidgetName(widget);

			if (widget is DataGrid)
			{
				try
				{
					var dg = widget as DataGrid;

					if (dg.SelectedItem != null)
					{
						var row = new List<object>();

						foreach (KeyValuePair<string, object> kvp in (dg.SelectedItem as ExpandoObject))
						{
							row.Add(kvp.Value);
						}

						gridsSelectedRow[widget.Name.ToLower()] = row;
					}
				}
				catch (Exception ex)
				{

				}
			}

			if (m_selChangedHandlers.TryGetValue(widgetName, out string funcName))
			{
				var item = e.AddedItems.Count > 0 ? e.AddedItems[0].ToString() : e.RemovedItems.Count > 0 ? e.RemovedItems[0].ToString() : "";
				Control2Window.TryGetValue(widget, out Window win);
				Interpreter.Run(funcName, new Variable(widgetName), new Variable(item),
					Variable.EmptyInstance, GetScript(win));
			}
		}

		private void RadioButton_Checked(object sender, RoutedEventArgs e)
		{
			var widget = sender as RadioButton;

			UpdateVariable(widget, new Variable(true));

			var widgetName = GetWidgetName(widget);
			if (m_selChangedHandlers.TryGetValue(widgetName, out string funcName))
			{
				Control2Window.TryGetValue(widget, out Window win);
				Interpreter.Run(funcName, new Variable(widgetName), /*new Variable(item)*/ null,
					Variable.EmptyInstance, GetScript(win));
			}
		}

		private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
		{
			var widget = sender as RadioButton;

			UpdateVariable(widget, new Variable(false));

			var widgetName = GetWidgetName(widget);
			if (m_selChangedHandlers.TryGetValue(widgetName, out string funcName))
			{
				Control2Window.TryGetValue(widget, out Window win);
				Interpreter.Run(funcName, new Variable(widgetName), /*new Variable(item)*/ null,
					Variable.EmptyInstance, GetScript(win));
			}
		}

		private void Widget_Hover(object sender, MouseEventArgs e)
		{
			var widget = sender as FrameworkElement;
			var widgetName = GetWidgetName(widget);
			if (string.IsNullOrWhiteSpace(widgetName))
			{
				return;
			}

			if (m_mouseHoverHandlers.TryGetValue(widgetName, out string funcName))
			{
				Control2Window.TryGetValue(widget, out Window win);
				Interpreter.Run(funcName, new Variable(widgetName), new Variable(e.ToString()),
					Variable.EmptyInstance, GetScript(win));
			}
		}


		string lastFocusedWidgetName = "";
		public bool skipPostEvent;

		private void Widget_Pre(object sender, RoutedEventArgs e)
		{
			LastObjWidgetName = ((Control)sender).Name;

			Control widget = sender as Control;
			var widgetName = GetWidgetName(widget);
			if (string.IsNullOrWhiteSpace(widgetName))
			{
				return;
			}

			if ((Control)sender is Button)
			{
				var btn = sender as Button;
				if (btn.Parent is FrameworkElement)
				{
					var parent1 = btn.Parent;
					if ((parent1 as FrameworkElement).Parent is ASEnterBox)
					{
						var entBox = (parent1 as FrameworkElement).Parent as ASEnterBox;

						var entBoxGrid = entBox.Content as Grid;
						foreach (var item in entBoxGrid.Children)
						{
							if (item is ASEnterTextBox)
							{
								var entTB = item as ASEnterTextBox;

								if (lastFocusedWidgetName == entTB.Name)
								{
									lastFocusedWidgetName = widgetName;

									var request = new TraversalRequest(FocusNavigationDirection.Next);
									request.Wrapped = true;
									btn.MoveFocus(request);

									return;
								}
								else
								{
									entTB.Focus();
									return;
								}
								break;
							}
						}
					}
					else if ((parent1 as FrameworkElement).Parent is ASNumericBox)
					{
						var numBox = (parent1 as FrameworkElement).Parent as ASNumericBox;

						var numBoxGrid = numBox.Content as Grid;
						foreach (var item in numBoxGrid.Children)
						{
							if (item is ASNumericTextBox)
							{
								var numTB = item as ASNumericTextBox;

								if (lastFocusedWidgetName == numTB.Name)
								{
									lastFocusedWidgetName = widgetName;

									var request = new TraversalRequest(FocusNavigationDirection.Next);
									request.Wrapped = true;
									btn.MoveFocus(request);

									return;
								}
								else
								{
									numTB.Focus();
									return;
								}
								break;
							}
						}
					}
				}
			}

			if ((Control)sender is ASEnterTextBox)
			{
				var etb = sender as ASEnterTextBox;
				if (etb.Parent is FrameworkElement)
				{
					var parent1 = etb.Parent;
					if ((parent1 as FrameworkElement).Parent is ASEnterBox)
					{
						var entBox = (parent1 as FrameworkElement).Parent as ASEnterBox;

						var entBoxGrid = entBox.Content as Grid;
						foreach (var item in entBoxGrid.Children)
						{
							if (item is Button)
							{
								var entBtn = item as Button;

								if (lastFocusedWidgetName == entBtn.Name)
								{
									lastFocusedWidgetName = widgetName;
									return;
								}
								break;
							}
						}
					}
				}
			}

			if ((Control)sender is ASNumericTextBox)
			{
				var ntb = sender as ASNumericTextBox;
				if (ntb.Parent is FrameworkElement)
				{
					var parent1 = ntb.Parent;
					if ((parent1 as FrameworkElement).Parent is ASNumericBox)
					{
						var numBox = (parent1 as FrameworkElement).Parent as ASNumericBox;

						var numBoxGrid = numBox.Content as Grid;
						foreach (var item in numBoxGrid.Children)
						{
							if (item is Button)
							{
								var numBtn = item as Button;

								if (lastFocusedWidgetName == numBtn.Name)
								{
									lastFocusedWidgetName = widgetName;
									return;
								}
								break;
							}
						}
					}
				}
			}

			skipPostEvent = false;

			string funcName;
			if (m_PreHandlers.TryGetValue(widgetName, out funcName))
			{
				Control2Window.TryGetValue(widget, out Window win);
				var result = Interpreter.Run(funcName, new Variable(widgetName), null,
					Variable.EmptyInstance, GetScript(win));
				if (result.Type == Variable.VarType.NUMBER && !result.AsBool()) // if script returned false
				{
					if (widget is ASEnterTextBox || widget is ASNumericTextBox)
						shouldButtonClick = false;
					skipPostEvent = true;
					var widgetToFocusTo = GetWidget(lastFocusedWidgetName);
					if (widgetToFocusTo != null && (widgetToFocusTo is Control))
					{
						widgetToFocusTo.Focus();
					}
					else
					{
						var request = new TraversalRequest(FocusNavigationDirection.Next);
						request.Wrapped = true;
						widget.MoveFocus(request);
					}
				}
				else
				{
					if (widget is ASEnterTextBox || widget is ASNumericTextBox)
						shouldButtonClick = true;
					lastFocusedWidgetName = widgetName;
				}
			}
		}

		private void Widget_Post(object sender, KeyboardFocusChangedEventArgs e)
		{
			if (skipPostEvent)
			{
				skipPostEvent = false;
				return;
			}

			Control widget = sender as Control;
			var widgetName = GetWidgetName(widget);
			if (string.IsNullOrWhiteSpace(widgetName))
			{
				return;
			}
			if (((Control)e.NewFocus) != null)
				LastObjWidgetName = ((Control)e.NewFocus).Name;

			if ((Control)sender is Button)
			{
				var btn = sender as Button;
				if (btn.Parent is FrameworkElement)
				{
					var parent1 = btn.Parent;
					if ((parent1 as FrameworkElement).Parent is ASEnterBox)
					{
						var entBox = (parent1 as FrameworkElement).Parent as ASEnterBox;

						var entBoxGrid = entBox.Content as Grid;
						foreach (var item in entBoxGrid.Children)
						{
							if (item is ASEnterTextBox)
							{
								var entTB = item as ASEnterTextBox;

								if (((Control)e.NewFocus)?.Name == entTB.Name)
								{
									return;
								}
								break;
							}
						}
					}
					else if ((parent1 as FrameworkElement).Parent is ASNumericBox)
					{
						var numBox = (parent1 as FrameworkElement).Parent as ASNumericBox;

						var numBoxGrid = numBox.Content as Grid;
						foreach (var item in numBoxGrid.Children)
						{
							if (item is ASNumericTextBox)
							{
								var numTB = item as ASNumericTextBox;

								if (((Control)e.NewFocus)?.Name == numTB.Name)
								{
									return;
								}
								break;
							}
						}
					}
				}
			}

			if ((Control)sender is ASEnterTextBox)
			{
				var etb = sender as ASEnterTextBox;
				if (etb.Parent is FrameworkElement)
				{
					var parent1 = etb.Parent;
					if ((parent1 as FrameworkElement).Parent is ASEnterBox)
					{
						var entBox = (parent1 as FrameworkElement).Parent as ASEnterBox;

						var entBoxGrid = entBox.Content as Grid;
						foreach (var item in entBoxGrid.Children)
						{
							if (item is Button)
							{
								var entBtn = item as Button;

								if (((Control)e.NewFocus)?.Name == entBtn.Name)
								{
									return;
								}
								break;
							}
						}
					}
				}
			}

			if ((Control)sender is ASNumericTextBox)
			{
				var ntb = sender as ASNumericTextBox;
				if (ntb.Parent is FrameworkElement)
				{
					var parent1 = ntb.Parent;
					if ((parent1 as FrameworkElement).Parent is ASNumericBox)
					{
						var numBox = (parent1 as FrameworkElement).Parent as ASNumericBox;

						var numBoxGrid = numBox.Content as Grid;
						foreach (var item in numBoxGrid.Children)
						{
							if (item is Button)
							{
								var numBtn = item as Button;

								if (((Control)e.NewFocus)?.Name == numBtn.Name)
								{
									return;
								}
								break;
							}
						}
					}
				}
			}

			if ((Control)sender is DatePicker)
			{
				var asdeDatePicker = sender as DatePicker;

				if ((Control)e.NewFocus is System.Windows.Controls.Calendar)
				{
					var parent1 = ((Control)e.NewFocus)?.Parent;
					if (parent1 != null)
					{
						if (parent1 is Popup popup)
						{
							if (popup.Name.Replace("_Popup", "") == asdeDatePicker.Name)
							{
								return;
							}
						}

					}
				}
				else if ((Control)e.NewFocus is CalendarDayButton)
				{
					return;
				}
				else if ((Control)e.NewFocus is DatePickerTextBox)
				{
					if (((Control)e.NewFocus).Name.Replace("_TextBox", "") == asdeDatePicker.Name)
					{
						return;
					}
				}
				else if ((Control)e.NewFocus is CalendarButton)
				{
					return;
				}
			}


			string funcName;
			if (m_PostHandlers.TryGetValue(widgetName, out funcName))
			{
				Control2Window.TryGetValue(widget, out Window win);
				var result = Interpreter.Run(funcName, new Variable(widgetName), null,
					Variable.EmptyInstance, GetScript(win));
				if (result.Type == Variable.VarType.NUMBER && !result.AsBool())
				{
					e.Handled = true;
					LastObjWidgetName = widgetName;
				}
				else
				{
					//
					TextBoxBase widget2 = sender as TextBoxBase;
					var widgetName2 = GetWidgetBindingName(widget2);
					if (string.IsNullOrWhiteSpace(widgetName2) ||
						m_updatingWidget.Contains(widgetName2))
					{
						return;
					}

					m_updatingWidget.Add(widgetName2);
					var text = GetTextWidgetFunction.GetText(widget2);
					if (DEFINES.TryGetValue(widgetName2.ToLower(), out DefineVariable defVar))
					{
						switch (defVar.DefType)
						{
							case "a":
								//UpdateVariable(widget2, text);
								break;

							case "i":
							case "n":
							case "r":
							case "b":
								//if (double.TryParse(text.AsString(), out double parsedDouble))
								//{
								//    UpdateVariable(widget2, new Variable(parsedDouble));
								//}
								break;

							case "d":

								break;
							case "t":
								UpdateVariable(widget2, text);
								break;

							default:
								//UpdateVariable(widget2, text);
								break;
						}
					}

					m_updatingWidget.Remove(widgetName2);
				}
			}
		}

		private void Widget_Change(object sender, SelectionChangedEventArgs e)
		{
			if (SetWidgetOptionsFunction.settingTabControlPosition)
			{
				return;
			}

			TabControl widget = sender as TabControl;
			var widgetName = GetWidgetName(widget);
			if (string.IsNullOrWhiteSpace(widgetName))
			{
				return;
			}

			string funcName;
			if (m_ChangeHandlers.TryGetValue(widgetName, out funcName))
			{
				Control2Window.TryGetValue(widget, out Window win);
				Interpreter.Run(funcName, new Variable(widgetName), null,
					Variable.EmptyInstance, GetScript(win));
				e.Handled = true;
			}
		}

		bool skipAfterChange = true;
		private void Navigator_Change(object sender, EventArgs e)
		{
			WpfControlsLibrary.ASNavigator widget = sender as WpfControlsLibrary.ASNavigator;
			var widgetName = GetWidgetName(widget);
			if (string.IsNullOrWhiteSpace(widgetName))
			{
				return;
			}

			string funcName;
			if (m_NavigatorChangeHandlers.TryGetValue(widgetName, out funcName))
			{
				Control2Window.TryGetValue(widget, out Window win);
				var result = Interpreter.Run(funcName, new Variable(widgetName), null,
					Variable.EmptyInstance, GetScript(win));

				if (result.Type == Variable.VarType.NUMBER && !result.AsBool())
				{
					skipAfterChange = true;
				}
				else
				{
					skipAfterChange = false;
				}
			}
		}

		private void Navigator_AfterChange(object sender, EventArgs e)
		{
			if (skipAfterChange)
			{
				return;
			}

			WpfControlsLibrary.ASNavigator widget = sender as WpfControlsLibrary.ASNavigator;
			var widgetName = GetWidgetName(widget);
			if (string.IsNullOrWhiteSpace(widgetName))
			{
				return;
			}


			var option = widget.buttonClicked;
			switch (option)
			{
				case NavigatorButton.First:
					NavigatorClass.NavigateFirst(widgetName);
					break;
				case NavigatorButton.Last:
					NavigatorClass.NavigateLast(widgetName);
					break;
				case NavigatorButton.Next:
					NavigatorClass.NavigateNext(widgetName);
					break;
				case NavigatorButton.Previous:
					NavigatorClass.NavigatePrevious(widgetName);
					break;
				default:
					return;
			}

			string funcName;
			if (m_NavigatorAfterChangeHandlers.TryGetValue(widgetName, out funcName))
			{
				Control2Window.TryGetValue(widget, out Window win);
				var result = Interpreter.Run(funcName, new Variable(widgetName), null,
					Variable.EmptyInstance, GetScript(win));
			}
		}

		private void DataGrid_Move(object sender, SelectionChangedEventArgs e)
		{
			DataGrid widget = sender as DataGrid;
			var widgetName = GetWidgetName(widget);
			if (string.IsNullOrWhiteSpace(widgetName))
			{
				return;
			}

			string funcName;
			if (m_MoveHandlers.TryGetValue(widgetName, out funcName))
			{
				Control2Window.TryGetValue(widget, out Window win);
				var result = Interpreter.Run(funcName, new Variable(widgetName), null,
					Variable.EmptyInstance, GetScript(win));
			}
		}

		private void DataGrid_Select(object sender, MouseButtonEventArgs e)
		{
			DataGrid widget = sender as DataGrid;
			var widgetName = GetWidgetName(widget);
			if (string.IsNullOrWhiteSpace(widgetName))
			{
				return;
			}

			//if dataGrid is in Edit mode this event is disabled
			if (widget.IsReadOnly == false)
			{
				return;
			}

			string funcName;
			if (m_SelectHandlers.TryGetValue(widgetName, out funcName))
			{
				Control2Window.TryGetValue(widget, out Window win);
				var result = Interpreter.Run(funcName, new Variable(widgetName), null,
					Variable.EmptyInstance, GetScript(win));
			}
		}

		private void DataGrid_EnterKeyPressed(object sender, KeyEventArgs e)
		{
			DataGrid widget = sender as DataGrid;
			var widgetName = GetWidgetName(widget);
			if (string.IsNullOrWhiteSpace(widgetName))
			{
				return;
			}

			if (widget.SelectedIndex < 0)
			{
				return;
			}

			//if dataGrid is in Edit mode this event is disabled
			if (widget.IsReadOnly == false)
			{
				return;
			}

			if (e.Key == Key.Enter)
			{
				e.Handled = true;
				string funcName;
				if (m_SelectHandlers.TryGetValue(widgetName, out funcName))
				{
					Control2Window.TryGetValue(widget, out Window win);
					var result = Interpreter.Run(funcName, new Variable(widgetName), null,
						Variable.EmptyInstance, GetScript(win));
				}
			}
		}

		public FrameworkElement GetWidget(string name)
		{
			CacheControls(MainWindow);
			if (Controls.TryGetValue(name.ToLower(), out FrameworkElement control))
			{
				return control;
			}
			return null;
		}

		public List<FrameworkElement> CacheControls(Window win, bool force = false)
		{
			List<FrameworkElement> controls = new List<FrameworkElement>();

			if ((!force && Controls.Count > 0) || win == null)
			{
				return controls;
			}

			var content = win.Content;
			List<UIElement> children = null;

			if (content is Grid)
			{
				var grid = content as Grid;
				if (grid.Children.Count > 0 && grid.Children[0] is StackPanel)
				{
					var stack = grid.Children[0] as StackPanel;
					children = stack.Children.Cast<UIElement>().ToList();
				}
				else
				{
					children = grid.Children.Cast<UIElement>().ToList();
				}
			}
			else if (content is Panel)
			{
				var panel = content as Panel;
				children = (content as Panel).Children.Cast<UIElement>().ToList();
			}
			else if (content is StackPanel)
			{
				var stack = content as StackPanel;
				children = stack.Children.Cast<UIElement>().ToList();
			}
			else if (content is Viewbox)
			{
				var viewbox = content as Viewbox;
				children = new List<UIElement>() { viewbox.Child };
			}

			CacheChildren(children, controls, win);
			return controls;
		}

		void CacheChildren(List<UIElement> children, List<FrameworkElement> controls, Window win)
		{
			if (children == null || children.Count == 0)
			{
				return;
			}
			foreach (var child in children)
			{
				if (child is Grid)
				{
					var gridControl = child as Grid;
					CacheChildren(gridControl.Children.Cast<UIElement>().ToList(), controls, win);
				}
				else if (child is TabControl)
				{
					var tabControl = child as TabControl;
					CacheControl(tabControl, win, controls);

					var count = VisualTreeHelper.GetChildrenCount(tabControl);
					for (int i = 0; i < count; i++)
					{
						DependencyObject item = VisualTreeHelper.GetChild(tabControl, i);
						if (item is Grid)
						{
							var tabGrid = item as Grid;
							var count2 = VisualTreeHelper.GetChildrenCount(tabGrid);
							for (int j = 0; j < count2; j++)
							{
								DependencyObject item2 = VisualTreeHelper.GetChild(tabGrid, j);
								if (item2 is TabPanel)
								{
									var tabPanel = item2 as TabPanel;
									var count3 = VisualTreeHelper.GetChildrenCount(tabPanel);
									for (int k = 0; k < count3; k++)
									{
										DependencyObject item3 = VisualTreeHelper.GetChild(tabPanel, k);
										if (item3 is TabItem)
										{
											var tabItem = item3 as TabItem;
											var content2 = tabItem.Content as Grid;
											foreach (var child2 in content2.Children)
											{

												if (child2 is ASButton)
												{
													var asButton = child2 as ASButton;
													var insideButton = asButton.Content as Button;

													//CacheControl(insideButton as FrameworkElement, win, controls);
													CacheASButton(asButton as FrameworkElement, win, controls, insideButton);
												}
												else if (child2 is ASEnterBox)
												{
													var enterBox = child2 as ASEnterBox;
													var enterBoxGrid = enterBox.Content as Grid;
													foreach (var item4 in enterBoxGrid.Children)
													{
														CacheASEnterBoxChild(item4 as FrameworkElement, win, controls, enterBox);
													}

												}
												else if (child2 is ASNumericBox)
												{
													var numBox = child2 as ASNumericBox;
													var numBoxGrid = numBox.Content as Grid;
													foreach (var item5 in numBoxGrid.Children)
													{
														CacheNumericBoxChild(item5 as FrameworkElement, win, controls, numBox);
													}
												}
												else if (child2 is GroupBox)//for RadioButtons
												{
													CacheControl(child2 as FrameworkElement, win, controls);

													var groupBox = child2 as GroupBox;
													var groupBoxGrid = groupBox.Content as Grid;
													foreach (var item6 in groupBoxGrid.Children)
													{
														if (item6 is RadioButton)
														{
															CacheControl(item6 as FrameworkElement, win, controls);
															if (!GroupBoxesAndRadioButtons.Any(p => p.Key.ToLower() == groupBox.Name.ToLower()))
																GroupBoxesAndRadioButtons.Add(groupBox.Name, new List<string>());
															if (!GroupBoxesAndRadioButtons[groupBox.Name].Any(p => p == (item6 as RadioButton).Name.ToLower()))
																GroupBoxesAndRadioButtons[groupBox.Name].Add((item6 as RadioButton).Name.ToLower());
														}

														else if (item6 is CheckBox)
															CacheControl(item6 as FrameworkElement, win, controls);
													}
												}
												else if (child2 is ASDateEditer2)
												{
													//CacheControl(child as ASDateEditer2, win, controls);

													var asde2 = child2 as ASDateEditer2;
													var asde2Grid = asde2.Content as Grid;
													foreach (var item6 in asde2Grid.Children)
													{
														var fe = (item6 as FrameworkElement);
														fe.DataContext = asde2.FieldName;
														CacheControl(fe, win, controls);
													}
												}
												else
												{
													if (child2 is DataGrid dg)
													{
														gridsSelectedRow.Remove(dg.Name.ToLower());
													}
													CacheControl(child2 as FrameworkElement, win, controls);
													if (child2 is ItemsControl)
													{
														var parent = child2 as ItemsControl;
														var items = parent.Items;
														if (items != null && items.Count > 0)
														{
															try
															{
																CacheChildren(items.Cast<UIElement>().ToList(), controls, win);
															}
															catch (Exception ex)
															{
																//MessageBox.Show("Vassili help needed");
															}
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				else if (child is ToolBarTray)
				{
					var tbTray = child as ToolBarTray;
					var toolbars = tbTray.ToolBars;

					foreach (var toolbar in toolbars)
					{
						foreach (var item in toolbar.Items)
						{
							if(item is Button)
							{
                                CacheControl(item as FrameworkElement, win, controls);
                            }
						}
					}
                    //CacheControl(insideButton as FrameworkElement, win, controls);
                    //CacheASButton(asButton as FrameworkElement, win, controls, insideButton);
				}
				else if (child is ASButton)
				{
					var asButton = child as ASButton;
					var insideButton = asButton.Content as Button;

					//CacheControl(insideButton as FrameworkElement, win, controls);
					CacheASButton(asButton as FrameworkElement, win, controls, insideButton);
				}
				else if (child is ASEnterBox)
				{
					var enterBox = child as ASEnterBox;
					var enterBoxGrid = enterBox.Content as Grid;
					foreach (var item in enterBoxGrid.Children)
					{
						CacheASEnterBoxChild(item as FrameworkElement, win, controls, enterBox);
					}

				}
				else if (child is ASNumericBox)
				{
					var numBox = child as ASNumericBox;
					var numBoxGrid = numBox.Content as Grid;
					foreach (var item in numBoxGrid.Children)
					{
						CacheNumericBoxChild(item as FrameworkElement, win, controls, numBox);
					}
				}
				else if (child is GroupBox)//for RadioButtons
				{
					CacheControl(child as FrameworkElement, win, controls);

					var groupBox = child as GroupBox;
					var groupBoxGrid = groupBox.Content as Grid;
					foreach (var item in groupBoxGrid.Children)
					{
						if (item is RadioButton)
						{
							CacheControl(item as FrameworkElement, win, controls);
							if (!GroupBoxesAndRadioButtons.Any(p => p.Key.ToLower() == groupBox.Name.ToLower()))
								GroupBoxesAndRadioButtons.Add(groupBox.Name, new List<string>());
							if (!GroupBoxesAndRadioButtons[groupBox.Name].Any(p => p == (item as RadioButton).Name.ToLower()))
								GroupBoxesAndRadioButtons[groupBox.Name].Add((item as RadioButton).Name.ToLower());
						}

						else if (item is CheckBox)
							CacheControl(item as FrameworkElement, win, controls);
					}
				}
				else if (child is ASDateEditer2)
				{
					//CacheControl(child as ASDateEditer2, win, controls);

					var asde2 = child as ASDateEditer2;
					var asde2Grid = asde2.Content as Grid;
					foreach (var item in asde2Grid.Children)
					{
						var fe = (item as FrameworkElement);
						fe.DataContext = asde2.FieldName;
						CacheControl(fe, win, controls);
					}
				}
				else
				{
					if (child is DataGrid dg)
					{
						gridsSelectedRow.Remove(dg.Name.ToLower());
					}
					CacheControl(child as FrameworkElement, win, controls);
					if (child is ItemsControl)
					{
						var parent = child as ItemsControl;
						var items = parent.Items;
						if (items != null && items.Count > 0)
						{
							try
							{
								CacheChildren(items.Cast<UIElement>().ToList(), controls, win);
							}
							catch (Exception ex)
							{
								//MessageBox.Show("Vassili help needed");
							}
						}
					}
				}
			}
		}

		public void CacheControl(FrameworkElement widget, Window win = null, List<FrameworkElement> controls = null)
		{
			var wName = GetWidgetName(widget);
			if (!string.IsNullOrWhiteSpace(wName))
			{
				Controls[wName.ToLower()] = widget;
				controls?.Add(widget);
				if (win != null)
				{
					Control2Window[widget] = win;
				}
			}

			/*if (widget != null && !string.IsNullOrWhiteSpace(widget.Name))
			{
			    Controls[widget.Name.ToString().ToLower()] = widget;
			    controls?.Add(widget);
			    if (win != null)
			    {
			        Control2Window[widget] = win;
			    }
			}
			if (widget != null && widget.DataContext != null)
			{
			    Controls[widget.DataContext.ToString().ToLower()] = widget;
			    controls?.Add(widget);
			    if (win != null)
			    {
			        Control2Window[widget] = win;
			    }
			}*/
		}

		public void CacheASButton(FrameworkElement widget, Window win = null, List<FrameworkElement> controls = null, Button insideButton = null)
		{
			var temp = widget.Name;
			widget.Name = temp + "_x";
			insideButton.Name = temp;

			var wName = GetWidgetName(widget);
			if (!string.IsNullOrWhiteSpace(wName))
			{
				Controls[wName.ToLower()] = insideButton;
				controls?.Add(insideButton);
				if (win != null)
				{
					Control2Window[insideButton] = win;
				}
			}
		}

		public void CacheASEnterBoxChild(FrameworkElement widget, Window win = null, List<FrameworkElement> controls = null, ASEnterBox enterBox = null)
		{
			if (widget is ASEnterTextBox)
			{
				widget.Name += enterBox.Name;
				widget.DataContext = enterBox.FieldName;
				if (enterBox.FieldName != null && DEFINES.TryGetValue(enterBox.FieldName.ToLower(), out DefineVariable defVar))
				{
					if(enterBox.Size == 0)
					{
						enterBox.Size = defVar.Size;
					}
					else if (defVar.Size < enterBox.Size)
					{
						enterBox.Size = defVar.Size;
					}
				}
				if (enterBox.Case?.ToLower() != "up" && enterBox.Case?.ToLower() != "down")
				{
					if (enterBox.FieldName != null && DEFINES.TryGetValue(enterBox.FieldName.ToLower(), out DefineVariable defVar2))
					{
						if (defVar2.Up)
						{
							enterBox.Case = "Up";
						}
						if (defVar2.Down)
						{
							enterBox.Case = "Down";
						}
					}
				}

				if (enterBox.KeyTraps != null)
				{
					var splitted = enterBox.KeyTraps.Split('|');
					for (int i = 0; i < splitted.Length; i += 2)
					{
						var keyFromXaml = splitted[i];
						var funcName = splitted[i + 1];

						var routedCommand = new RoutedCommand();

						Key key = Key.None;
						ModifierKeys modifier = ModifierKeys.None;
						if (keyFromXaml.ToLower() == "f2")
						{
							key = Key.F2;
							modifier = ModifierKeys.None;
						}
						else if (keyFromXaml.ToLower() == "f3")
						{
							key = Key.F3;
							modifier = ModifierKeys.None;
						}

						routedCommand.InputGestures.Add(new KeyGesture(key, modifier, keyFromXaml));

						if (!(widget as ASEnterTextBox).paramsForKeyTraps.Any(p => p.Key == keyFromXaml))
							(widget as ASEnterTextBox).paramsForKeyTraps.Add(keyFromXaml, new List<object>() { funcName, widget, enterBox.Name });

						widget.CommandBindings.Add(new CommandBinding(routedCommand, runFunctionHandler));
					}
				}

				var wName = GetWidgetName(widget);
				if (!string.IsNullOrWhiteSpace(wName))
				{
					Controls[wName.ToLower()] = widget;
					controls?.Add(widget);
					if (win != null)
					{
						Control2Window[widget] = win;
					}
				}
				/*if (widget != null && widget.DataContext != null)
				{
				    Controls[widget.DataContext.ToString().ToLower()] = widget;
				    controls?.Add(widget);
				    if (win != null)
				    {
				        Control2Window[widget] = win;
				    }
				}
				if (widget != null && !string.IsNullOrEmpty(enterBox.FieldName))
				{

				    Controls[enterBox.FieldName.ToString().ToLower()] = widget;
				    controls?.Add(widget);
				    if (win != null)
				    {
				        Control2Window[widget] = win;
				    }
				}
				if (widget != null && enterBox.FieldName != null)
				{
				    Controls[enterBox.FieldName.ToString().ToLower()] = widget;

				    if (controls != null && !controls.Contains(widget))
				        controls.Add(widget);

				    if (win != null)
				    {
				        Control2Window[widget] = win;
				    }
				}*/
			}
			else if (widget is Button)
			{
				widget.Name += enterBox.Name;
				var wName = GetWidgetName(widget);
				if (!string.IsNullOrWhiteSpace(wName))
				{
					Controls[wName.ToLower()] = widget;
					controls?.Add(widget);
					if (win != null)
					{
						Control2Window[widget] = win;
					}
				}
				/*if (widget != null && !string.IsNullOrEmpty(enterBox.Name))
				{
				    Controls[enterBox.Name.ToString().ToLower()] = widget;
				    controls?.Add(widget);
				    if (win != null)
				    {
				        Control2Window[widget] = win;
				    }
				}*/
			}
		}


		private void runFunctionHandler(object sender, ExecutedRoutedEventArgs e)
		{
			if (sender is ASNumericTextBox)
			{
				var thisTB = (sender as ASNumericTextBox);
				var rc = (e.Command as RoutedCommand);
				var kg = (rc.InputGestures[0] as KeyGesture);
				var keyFromXaml = kg.DisplayString;

				var parameters = thisTB.paramsForKeyTraps[keyFromXaml];

				var toRunFuncName = (string)parameters[0];
				var toRunWidget = (FrameworkElement)parameters[1];
				var toRunWidgetName = (string)parameters[2];

				if (toRunFuncName.ToLower().EndsWith("@clicked"))
				{
					thisTB.FormatOnLostFocus();
				}

				Control2Window.TryGetValue(toRunWidget, out Window win);
				ActiveWindow = win;
				RunScript(toRunFuncName, win, new Variable(toRunWidgetName), new Variable(toRunWidgetName));
			}
			else if (sender is ASEnterTextBox)
			{
				var thisTB = (sender as ASEnterTextBox);
				var rc = (e.Command as RoutedCommand);
				var kg = (rc.InputGestures[0] as KeyGesture);
				var keyFromXaml = kg.DisplayString;

				var parameters = thisTB.paramsForKeyTraps[keyFromXaml];

				var toRunFuncName = (string)parameters[0];
				var toRunWidget = (FrameworkElement)parameters[1];
				var toRunWidgetName = (string)parameters[2];

				Control2Window.TryGetValue(toRunWidget, out Window win);
				ActiveWindow = win;
				RunScript(toRunFuncName, win, new Variable(toRunWidgetName), new Variable(toRunWidgetName));
			}

		}

		public void CacheNumericBoxChild(FrameworkElement widget, Window win = null, List<FrameworkElement> controls = null, ASNumericBox numBox = null)
		{
			if (widget is ASNumericTextBox)
			{
				widget.Name += numBox.Name; // "_" + numBox.Name;
				widget.DataContext = numBox.FieldName;
				if (numBox.FieldName != null && DEFINES.TryGetValue(numBox.FieldName.ToLower(), out DefineVariable defVar))
				{
					if (defVar.Size < (widget as ASNumericTextBox).Size)
					{
						(widget as ASNumericTextBox).Size = defVar.Size;
					}
				}

				if (numBox.KeyTraps != null)
				{
					var splitted = numBox.KeyTraps.Split('|');
					for (int i = 0; i < splitted.Length; i += 2)
					{
						var keyFromXaml = splitted[i];
						var funcName = splitted[i + 1];

						var routedCommand = new RoutedCommand();

						Key key = Key.None;
						ModifierKeys modifier = ModifierKeys.None;
						if (keyFromXaml.ToLower() == "f2")
						{
							key = Key.F2;
							modifier = ModifierKeys.None;
						}
						else if (keyFromXaml.ToLower() == "f3")
						{
							key = Key.F3;
							modifier = ModifierKeys.None;
						}

						routedCommand.InputGestures.Add(new KeyGesture(key, modifier, keyFromXaml));

						if (!(widget as ASNumericTextBox).paramsForKeyTraps.Any(p => p.Key == keyFromXaml))
							(widget as ASNumericTextBox).paramsForKeyTraps.Add(keyFromXaml, new List<object>() { funcName, widget, numBox.Name });

						widget.CommandBindings.Add(new CommandBinding(routedCommand, runFunctionHandler));
					}
				}

				var wName = GetWidgetBindingName(widget);
				if (!string.IsNullOrWhiteSpace(wName))
				{
					Controls[wName.ToLower()] = widget;
					controls?.Add(widget);
					if (win != null)
					{
						Control2Window[widget] = win;
					}
				}
				/*if (widget != null && !string.IsNullOrEmpty(numBox.FieldName))
				{
				    Controls[widget.Name.ToLower()] = widget;
				    controls?.Add(widget);
				    if (win != null)
				    {
				        Control2Window[widget] = win;
				    }
				}
				if (widget != null && numBox.FieldName != null)
				{
				    Controls[widget.Name.ToLower()] = widget;

				    if (controls != null && !controls.Contains(widget))
				        controls.Add(widget);

				    if (win != null)
				    {
				        Control2Window[widget] = win;
				    }
				}*/
			}
			else if (widget is Button)
			{
				widget.Name += numBox.Name; //"button_" + numBox.Name;
				var wName = GetWidgetBindingName(widget);
				if (!string.IsNullOrWhiteSpace(wName))
				{
					Controls[wName.ToLower()] = widget;
					controls?.Add(widget);
					if (win != null)
					{
						Control2Window[widget] = win;
					}
				}
				/*if (widget != null && !string.IsNullOrEmpty(numBox.Name))
				{
				    Controls[widget.Name.ToString().ToLower()] = widget;
				    controls?.Add(widget);
				    if (win != null)
				    {
				        Control2Window[widget] = win;
				    }
				}*/
			}
		}
		public void RemoveControl(FrameworkElement widget)
		{
			widget.Visibility = Visibility.Hidden;

			var wName = GetWidgetBindingName(widget);
			Controls.Remove(wName.ToLower());
		}

		public void AddWidgetActions(FrameworkElement widget)
		{	
			if ((widget.Parent as FrameworkElement).Parent is ASEnterBox)
			{
				var ASEnterBox = (widget.Parent as FrameworkElement).Parent as ASEnterBox;

				if (widget is ASEnterTextBox)
				{
					//binding
					var widgetBindingName = ASEnterBox.FieldName;
					if (!string.IsNullOrWhiteSpace(widgetBindingName))
					{
						AddBinding(widgetBindingName, widget);
					}

					//events
					string textChangeAction = ASEnterBox.Name + "@TextChange";

					string widgetPreAction = ASEnterBox.Name + "@Pre";
					string widgetPostAction = ASEnterBox.Name + "@Post";

					AddTextChangedHandler(widget.Name, textChangeAction, widget);

					AddWidgetPreHandler(widget.Name, widgetPreAction, widget);
					AddWidgetPostHandler(widget.Name, widgetPostAction, widget);
				}
				else if (widget is Button)
				{
					//events
					string clickAction = ASEnterBox.Name + "@Clicked";

					string widgetPreAction = ASEnterBox.Name + "@Pre";
					string widgetPostAction = ASEnterBox.Name + "@Post";

					AddActionHandler(widget.Name, clickAction, widget);

					AddWidgetPreHandler(widget.Name, widgetPreAction, widget);
					AddWidgetPostHandler(widget.Name, widgetPostAction, widget);
				}
			}
			else if ((widget.Parent as FrameworkElement).Parent is ASNumericBox)
			{
				var ASNumericBox = (widget.Parent as FrameworkElement).Parent as ASNumericBox;

				if (widget is ASNumericTextBox)
				{
					//events
					string textChangeAction = ASNumericBox.Name + "@TextChange";

					string widgetPreAction = ASNumericBox.Name + "@Pre";
					string widgetPostAction = ASNumericBox.Name + "@Post";

					//AddTextChangedHandler("numBoxTextBox_" + ASNumericBox.Name, textChangeAction, widget);

					//AddWidgetPreHandler("numBoxTextBox_" + ASNumericBox.Name, widgetPreAction, widget);
					//AddWidgetPostHandler(/*"numBoxTextBox_" + */ASNumericBox.Name, widgetPostAction, widget);

					/////////

					//AddTextChangedHandler(widget.Name, textChangeAction, widget);

					//AddWidgetPreHandler(widget.Name, widgetPreAction, widget);
					//AddWidgetPostHandler(widget.Name, widgetPostAction, widget);

					AddTextChangedHandler(widget.Name, textChangeAction, widget);

					AddWidgetPreHandler(widget.Name, widgetPreAction, widget);
					AddWidgetPostHandler(widget.Name, widgetPostAction, widget);

					//binding
					var widgetBindingName = ASNumericBox.FieldName;
					if (!string.IsNullOrWhiteSpace(widgetBindingName))
					{
						AddBinding(widgetBindingName, widget);
					}
				}
				else if (widget is Button)
				{
					//events
					string clickAction = ASNumericBox.Name + "@Clicked";

					string widgetPreAction = ASNumericBox.Name + "@Pre";
					string widgetPostAction = ASNumericBox.Name + "@Post";

					//AddActionHandler("button_" + ASNumericBox.Name, clickAction, widget);

					//AddWidgetPreHandler("button_" + ASNumericBox.Name, widgetPreAction, widget);
					//AddWidgetPostHandler("button_" + ASNumericBox.Name, widgetPostAction, widget);

					AddActionHandler(widget.Name, clickAction, widget);

					AddWidgetPreHandler(widget.Name, widgetPreAction, widget);
					AddWidgetPostHandler(widget.Name, widgetPostAction, widget);
				}
			}
			else if ((widget.Parent as FrameworkElement).Parent is ASDateEditer2)
			{
				var ASDateEditer2 = (widget.Parent as FrameworkElement).Parent as ASDateEditer2;

				if (widget is TextBox)
				{
					////events
					string textChangeAction = ASDateEditer2.Name + "@TextChange";
					string lostFocusAction = ASDateEditer2.Name + "@LostFocus";

					//string widgetPreAction = ASDateEditer2.Name + "@Pre";
					//string widgetPostAction = ASDateEditer2.Name + "@Post";

					////AddTextChangedHandler("numBoxTextBox_" + ASNumericBox.Name, textChangeAction, widget);

					////AddWidgetPreHandler("numBoxTextBox_" + ASNumericBox.Name, widgetPreAction, widget);
					////AddWidgetPostHandler(/*"numBoxTextBox_" + */ASNumericBox.Name, widgetPostAction, widget);

					///////////

					////AddTextChangedHandler(widget.Name, textChangeAction, widget);

					////AddWidgetPreHandler(widget.Name, widgetPreAction, widget);
					////AddWidgetPostHandler(widget.Name, widgetPostAction, widget);

					AddTextChangedHandler(widget.Name, textChangeAction, widget);
					AddLostFocusHandler(widget.Name, lostFocusAction, widget);

					//AddWidgetPreHandler(widget.Name, widgetPreAction, widget);
					//AddWidgetPostHandler(widget.Name, widgetPostAction, widget);

					//binding
					var widgetBindingName = ASDateEditer2.FieldName;
					if (!string.IsNullOrWhiteSpace(widgetBindingName))
					{
						//AddBinding(widgetBindingName, widget);
						AddBinding(widgetBindingName, widget);
					}
				}
				else if (widget is Button)
				{
					////events
					//string clickAction = ASDateEditer2.Name + "@Clicked";

					//string widgetPreAction = ASDateEditer2.Name + "@Pre";
					//string widgetPostAction = ASDateEditer2.Name + "@Post";

					////AddActionHandler("button_" + ASNumericBox.Name, clickAction, widget);

					////AddWidgetPreHandler("button_" + ASNumericBox.Name, widgetPreAction, widget);
					////AddWidgetPostHandler("button_" + ASNumericBox.Name, widgetPostAction, widget);

					//AddActionHandler(widget.Name, clickAction, widget);

					//AddWidgetPreHandler(widget.Name, widgetPreAction, widget);
					//AddWidgetPostHandler(widget.Name, widgetPostAction, widget);
				}
			}
			else
			{
				//xaml Name property
				var widgetName = GetWidgetName(widget);

				if (!string.IsNullOrWhiteSpace(widgetName))
				{
					if (widget is GroupBox)
					{
						//events
						string groupBoxClick = widgetName + "@Clicked";

						AddGroupBoxClickedHandler(widget.Name, groupBoxClick, widget);
					}
					else if (widget is RadioButton)
					{
						string radioButtonClick = widgetName + "@Clicked";
						string selectionChangedAction = widgetName + "@Changed";

						AddRadioButtonSelectedChangedHandler(widgetName, selectionChangedAction, widget);
						AddRadioButtonClickedHandler(widgetName, radioButtonClick, widget);
					}
					else
					{
						string clickAction = widgetName + "@Clicked";
						string preClickAction = widgetName + "@PreClicked";
						string postClickAction = widgetName + "@PostClicked";
						string keyDownAction = widgetName + "@KeyDown";
						string keyUpAction = widgetName + "@KeyUp";
						string textChangeAction = widgetName + "@TextChange";
						string mouseHoverAction = widgetName + "@MouseHover";
						string selectionChangedAction = widgetName + "@SelectionChanged";
						string dateChangedAction = widgetName + "@DateChanged";

						string lostFocusAction = widgetName + "@LostFocus";

						//textBox
						string widgetPreAction = widgetName + "@Pre";
						string widgetPostAction = widgetName + "@Post";

						//tabs
						string widgetChangeAction = widgetName + "@Change";
						string widgetAfterChangeAction = widgetName + "@AfterChange";

						//dataGrid
						string widgetMoveAction = widgetName + "@Move";
						string widgetSelectAction = widgetName + "@Select";

						AddActionHandler(widgetName, clickAction, widget);
						AddPreActionHandler(widgetName, preClickAction, widget);
						AddPostActionHandler(widgetName, postClickAction, widget);
						AddKeyDownHandler(widgetName, keyDownAction, widget);
						AddKeyUpHandler(widgetName, keyUpAction, widget);


						AddTextChangedHandler(widgetName, textChangeAction, widget);

						AddSelectionChangedHandler(widgetName, selectionChangedAction, widget);
						AddMouseHoverHandler(widgetName, mouseHoverAction, widget);
						AddDateChangedHandler(widgetName, dateChangedAction, widget);

						AddLostFocusHandler(widgetName, lostFocusAction, widget);

						//Pre, Post
						AddWidgetPreHandler(widgetName, widgetPreAction, widget);
						AddWidgetPostHandler(widgetName, widgetPostAction, widget);

						//ASNavigator(Change and AfterChange) and TabControl(Change) events
						AddWidgetChangeHandler(widgetName, widgetChangeAction, widget);
						AddWidgetAfterChangeHandler(widgetName, widgetAfterChangeAction, widget);

						//Grid(Move and Select)
						AddWidgetMoveHandler(widgetName, widgetMoveAction, widget);
						AddWidgetSelectHandler(widgetName, widgetSelectAction, widget);
					}
				}

				//xaml DataContext property
				var widgetBindingName = GetWidgetBindingName(widget);
				if (!string.IsNullOrWhiteSpace(widgetBindingName))
				{
					if (!(widget is RadioButton))
						AddBinding(widgetBindingName, widget);
				}
			}
		}

		public static string Encode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}

		public static string Decode(string base64EncodedData)
		{
			var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}

		public Variable RunScript(string fileName, bool encode = false)
		{
			Init();

			if (encode)
			{
				EncodeFileFunction.EncodeDecode(fileName, false);
			}
			string script = Utils.GetFileContents(fileName);
			if (encode)
			{
				EncodeFileFunction.EncodeDecode(fileName, true);
			}

			PreprocessScripts();

			//preprocess this file
			var tokenSet = GetPreprocessTokens();
			var scriptsDirStr = App.GetConfiguration("ScriptsPath", "");
			var split2 = Utils.PreprocessScriptFile(fileName, tokenSet, scriptsDirStr, this);

			Variable result = null;
			try
			{
				result = Interpreter.Process(split2, fileName, false, this);
			}
			catch (Exception exc)
			{
				Console.WriteLine("Exception: " + exc.Message);
				Console.WriteLine(exc.StackTrace);
				Interpreter.InvalidateStacksAfterLevel(0);
				var onException = CustomFunction.Run(Interpreter, Constants.ON_EXCEPTION, new Variable("Global Scope"),
							new Variable(exc.Message), Variable.EmptyInstance);
				if (onException == null)
				{
					throw;
				}
			}

			return result;
		}

		void PreprocessScripts()
		{
			var filesStr = App.GetConfiguration("PreprocessFiles", "");
			var tokenSet = GetPreprocessTokens();
			if (string.IsNullOrWhiteSpace(filesStr) || tokenSet.Count == 0)
			{
				return;
			}
			var scriptsDirStr = App.GetConfiguration("ScriptsPath", "");

			var files = filesStr.Split(',');
			foreach (var file in files)
			{
				Utils.PreprocessScriptFile(file, tokenSet, scriptsDirStr, this);
			}
		}

		public static HashSet<string> GetPreprocessTokens()
		{
			var tokenSet = new HashSet<string>();
			var tokensStr = App.GetConfiguration("PreprocessTokens", "");
			var doPreprocess = App.GetConfiguration("Preprocess", "");
			if (string.IsNullOrWhiteSpace(tokensStr) ||
				!string.Equals(doPreprocess, "true", StringComparison.OrdinalIgnoreCase))
			{
				return tokenSet;
			}

			var tokens = tokensStr.Split(',');
			foreach (var token in tokens)
			{
				tokenSet.Add(token);
			}
			return tokenSet;
		}
	}

	class GetSelectedFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 1, m_name);

			var gui = CSCS_GUI.GetInstance(script);
			var widgetName = Utils.GetSafeString(args, 0);
			var widget = gui.GetWidget(widgetName);
			if (widget == null)
			{
				return Variable.EmptyInstance;
			}

			if (widget is DataGrid)
			{
				Variable selectedItems = new Variable(Variable.VarType.ARRAY);
				var dg = widget as DataGrid;
				var sel = dg.SelectedItems;
				int total = sel.Count;
				for (int i = 0; i < total; i++)
				{
					var item = sel[i] as ExpandoObject;
					var itemList = item.ToList();
					selectedItems.AddVariable(new Variable(itemList[0].Value.ToString()));
				}
				return selectedItems;
			}

			return GetTextWidgetFunction.GetText(widget);
		}
	}

	class DisplayArrFuncFunction : ParserFunction
	{
		Dictionary<string, List<string>> arraysOfGrids = new Dictionary<string, List<string>>();

		CSCS_GUI Gui;
		Dictionary<string, ObservableCollection<object[]>> rowsOfGrids { get; set; } = new Dictionary<string, ObservableCollection<object[]>>();

		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 1, m_name);

			var widgetName = Utils.GetSafeString(args, 0);
			Gui = CSCS_GUI.GetInstance(script);
			var dg = Gui.GetWidget(widgetName) as DataGrid;
			if (dg == null)
			{
				return Variable.EmptyInstance;
			}

			List<List<Variable>> cols = new List<List<Variable>>();
			List<string> headers = new List<string>();
			List<string> tags = new List<string>();

			var columns = dg.Columns;
			foreach (var column in columns)
			{
				if (column is DataGridTemplateColumn)
				{
					var dgtc = column as DataGridTemplateColumn;

					var header = dgtc.Header;
					headers.Add(header.ToString());

					var displayIndex = dgtc.DisplayIndex;

					var content = dgtc.CellTemplate.LoadContent();


					if (content is TextBox)
					{
						var tb = content as TextBox;
						var arrayToBindTo = tb.Tag.ToString().ToLower();

						tags.Add(arrayToBindTo);

						if (Gui.DEFINES.TryGetValue(arrayToBindTo, out DefineVariable defVar))
						{
							if (defVar.Array > 0)
							{
								cols.Add(defVar.Tuple);
							}
						}
					}
				}
			}

			arraysOfGrids[dg.Name] = tags;

			ObservableCollection<object[]> rows = new ObservableCollection<object[]>();

			for (int i = 0; i < cols[0].Count; i++)
			{
				object[] row = new object[cols.Count];
				for (int j = 0; j < cols.Count; j++)
				{
					var cell = cols[j][i];
					switch (cell.Type)
					{
						case Variable.VarType.STRING:
							row[j] = cell.AsString();
							break;
						case Variable.VarType.NUMBER:
							row[j] = cell.AsDouble();
							break;
					}
				}
				rows.Add(row);
			}

			rowsOfGrids[dg.Name] = rows;

			dg.ItemsSource = rows;

			dg.Columns.Clear();
			for (int i = 0; i < rows[0].Length; ++i)
				dg.Columns.Add(new DataGridTextColumn { Binding = new Binding("[" + i.ToString() + "]"), Header = headers[i] });

			dg.CurrentCellChanged += Dg_CurrentCellChanged;

			return new Variable("");
		}

		private void Dg_CurrentCellChanged(object sender, EventArgs e)
		{
			var gridName = (sender as DataGrid).Name;
			var arrayNames = arraysOfGrids[gridName];
			for (int i = 0; i < arrayNames.Count(); i++)
			{
				for (int j = 0; j < rowsOfGrids[gridName].Count; j++)
				{
					var array = Gui.DEFINES[arrayNames[i]];
					array.Tuple[j] = new Variable(rowsOfGrids[gridName][j][i]);
				}
			}
		}
	}

	public class FreeMemoryFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			var gui = CSCS_GUI.GetInstance(script);
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 1, m_name);

			var name = args[0].ParamName;

			var removed = gui.DEFINES.Remove(name);
			removed = (InterpreterInstance != null && InterpreterInstance.RemoveVariable(name)) || removed;
			removed = gui.Interpreter.RemoveVariable(name) || removed;
			return new Variable(removed);
		}
	}

	public class RunExecFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			string execName = Utils.GetItem(script).AsString();
			var argsStr = Utils.GetBodyBetween(script, '\0', ')', Constants.END_STATEMENT);
			var args = argsStr.Replace(',', ' ');
			var result = RunExec(execName, args);
			return result;
		}

		public static Variable RunExec(string filename, string args)
		{
			var proc = System.Diagnostics.Process.Start(filename, args);
			return new Variable(proc.Id);
		}
	}

	public class RunOnMainFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			string funcName = Utils.GetToken(script, Constants.NEXT_OR_END_ARRAY);
			var gui = CSCS_GUI.GetInstance(script);

			ParserFunction func = gui.Interpreter.GetFunction(funcName);
			Utils.CheckNotNull(funcName, func, script);

			Variable result = Variable.EmptyInstance;
			if (func is CustomFunction)
			{
				List<Variable> args = script.GetFunctionArgs();
				result = RunOnMainThread(func as CustomFunction, args);
			}
			else
			{
				var argsStr = Utils.GetBodyBetween(script, '\0', ')', Constants.END_STATEMENT);
				result = RunOnMainThread(gui, func, argsStr);
			}
			return result;
		}

		public static object RunOnMainThread(Action action)
		{
			return Application.Current.Dispatcher.Invoke(action, null);
		}

		public static Variable RunOnMainThread(CustomFunction callbackFunction, List<Variable> args)
		{
			Variable result = Variable.EmptyInstance;
			/*Application.Current.Dispatcher.Invoke(new Action(() =>
			{
			    result = callbackFunction.Run(args);
			}));*/

			CSCS_GUI.Dispatcher.Invoke((Action)delegate ()
			{
				result = callbackFunction.Run(args);
			}, null);

			return result;
		}
		public static Variable RunOnMainThread(CSCS_GUI gui, ParserFunction func, string argsStr)
		{
			Variable result = Variable.EmptyInstance;
			ParsingScript tempScript = new ParsingScript(gui.Interpreter, argsStr);
			Application.Current.Dispatcher.Invoke(new Action(() =>
			{
				result = func.GetValue(tempScript);
			}));
			return result;
		}
	}

	class PrintFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 1, m_name);

			var text = Utils.GetSafeString(args, 0);
			var gui = CSCS_GUI.GetInstance(script);
			var widget = gui.GetWidget(text);

			PrintDialog printDlg = new PrintDialog();
			if (printDlg.ShowDialog() != true)
			{ // user cancelled printing.
				return new Variable("");
			}

			if (widget == null)
			{
				FlowDocument doc = new FlowDocument(new Paragraph(new Run(text)));
				IDocumentPaginatorSource idpSource = doc;
				printDlg.PrintDocument(idpSource.DocumentPaginator, "CSCS Printing.");

			}
			else
			{
				printDlg.PrintVisual(widget as FrameworkElement, "Window Printing.");
			}

			return new Variable(printDlg.PrintQueue.FullName);
		}
	}

	class GetTextWidgetFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 1, m_name);

			var gui = CSCS_GUI.GetInstance(script);
			var widgetName = Utils.GetSafeString(args, 0);
			var widget = gui.GetWidget(widgetName);
			return GetText(widget);
		}

		public static Variable GetText(FrameworkElement widget)
		{
			string result = "";
			//if (widget is ASHorizontalBar)
			//{
			//	var ashb = widget as ASHorizontalBar;
			//	result = ashb.Text;
			//}
			
			if (widget is ContentControl)
			{
				var contentable = widget as ContentControl;
				result = contentable.Content.ToString();
			}
			else if (widget is CheckBox)
			{
				var checkBox = widget as CheckBox;
				result = checkBox.IsChecked.HasValue && checkBox.IsChecked.Value ? "true" : "false";
			}
			else if (widget is TextBox)
			{
				var textBox = widget as TextBox;
				result = textBox.Text;
			}
			else if (widget is RichTextBox)
			{
				var richTextBox = widget as RichTextBox;
				result = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text;
			}
			else if (widget is ComboBox)
			{
				var comboBox = widget as ComboBox;
				result = comboBox.Text;
			}
			else if (widget is DatePicker)
			{
				var datePicker = widget as DatePicker;
				result = datePicker.Text;
			}

			return new Variable(result);
		}
	}

	public class SetTextWidgetFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			var rest = script.Rest;
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 2, m_name);

			var widgetName = Utils.GetSafeString(args, 0);
			var text = Utils.GetSafeString(args, 1);

			var gui = CSCS_GUI.GetInstance(script);
			var widget = gui.GetWidget(widgetName);
			if (widget == null)
			{
				return Variable.EmptyInstance;
			}

			int index = widget is ComboBox && args[0].Type == Variable.VarType.NUMBER ? (int)args[0].Value : -1;
			var set = SetText(widget, text, index);

			return new Variable(set);
		}

		public static bool SetText(FrameworkElement widget, string text, int index = -1)
		{
			var dispatcher = Application.Current.Dispatcher;

			// for skipping setting text on some widgets, to prevent displaying text "System.Windows.Controls.Grid" in place of content
			if (widget is ASHorizontalBar)
			{
				return true;
			}
			else if(widget is Button)
			{
                return true;
            }
			else if(widget is GroupBox)
			{
                return true;
            }
			else if (widget is CartesianChart)
			{
				return true;
			}
			else if (widget is PieChart)
			{
				return true;
			}
			//else if (widget is ...)
   //         {
   //             return true;
   //         }


            else if (widget is ComboBox)
			{
				var combo = widget as ComboBox;
				if (index < 0)
				{
					index = 0;
					foreach (var item in combo.Items)
					{
						if (item.ToString() == text)
						{
							break;
						}
						index++;
					}
				}
				if (index >= 0 && combo.Items != null && index < combo.Items.Count)
				{
					dispatcher.Invoke(new Action(() =>
					{
						combo.SelectedIndex = index;
					}));
				}
			}
			else if (widget is CheckBox)
			{
				var checkBox = widget as CheckBox;
				dispatcher.Invoke(new Action(() =>
				{
					checkBox.IsChecked = text == "1" || text.ToLower() == "true";
				}));
			}
			else if (widget is ContentControl)
			{
				var contentable = widget as ContentControl;
				dispatcher.Invoke(new Action(() =>
				{
					contentable.Content = text;
				}));
			}
			else if (widget is ASNumericTextBox)
			{
				var numericTextBox = widget as ASNumericTextBox;
				dispatcher.Invoke(new Action(() =>
				{
					//numericTextBox.SkipTextChangedHandler = true;
					numericTextBox.Text = text;
				}));
			}
			else if (widget is ASEnterTextBox)
			{
				var enterTextBox = widget as ASEnterTextBox;
				dispatcher.Invoke(new Action(() =>
				{
					//enterTextBox.SkipTextChangedHandler = true;
					enterTextBox.Text = text;
				}));
			}
			else if (widget is TextBox)
			{
				var textBox = widget as TextBox;
				dispatcher.Invoke(new Action(() =>
				{
					if (widget.Parent is Grid grid)
					{
						if (grid.Parent is ASDateEditer2 asde2)
						{
							if (text == "01/01/1900")
							{
								//asde2.skipSelectedDateChangedHandler = true;
								//asde2.TempDate = DateTime.Now;
								textBox.Text = "00/00/0000";
								//
								return;
							}
							//else if (text == "01/01/00")
							//{
							//                         //asde2.skipSelectedDateChangedHandler = true;
							//                         //asde2.TempDate = DateTime.Now;

							//	textBox.Text = "00/00/00";

							//	return;
							//                     }
							else
							{
								textBox.Text = text;
								return;
							}
						}
					}

					textBox.Text = text;
				}));
			}
			else if (widget is RichTextBox)
			{
				var richTextBox = widget as RichTextBox;
				dispatcher.Invoke(new Action(() =>
				{
					richTextBox.Document.Blocks.Clear();
					richTextBox.Document.Blocks.Add(new Paragraph(new Run(text)));
				}));
			}
			else if (widget is ASDateEditer && !string.IsNullOrWhiteSpace(text))
			{
				var dateEditer = widget as ASDateEditer;
				var dateStr = ProcessDateStr(text);
				var format = dateStr.Length == 10 ? "dd/MM/yyyy" : dateStr.Length == 8 ? "dd/MM/yy" : "yyyy/MM/dd hh:mm:ss";
				dispatcher.Invoke(new Action(() =>
				{
					dateEditer.SelectedDate = DateTime.ParseExact(dateStr, format, CultureInfo.InvariantCulture);
				}));
			}
			else if (widget is ASDateEditer2 && !string.IsNullOrWhiteSpace(text))
			{
				var dateEditer = widget as ASDateEditer2;
				var dateStr = ProcessDateStr(text);
				var format = dateStr.Length == 10 ? "dd/MM/yyyy" : dateStr.Length == 8 ? "dd/MM/yy" : "yyyy/MM/dd hh:mm:ss";
				dispatcher.Invoke(new Action(() =>
				{
					dateEditer.TempDate = DateTime.ParseExact(dateStr, format, CultureInfo.InvariantCulture);
				}));
			}
			else if (widget is DatePicker && !string.IsNullOrWhiteSpace(text))
			{
				var datePicker = widget as DatePicker;
				var format = text.Length == 10 ? "dd/MM/yyyy" : text.Length == 8 ? "hh:mm:ss" :
					   text.Length == 12 ? "hh:mm:ss.fff" : "yyyy/MM/dd hh:mm:ss";
				dispatcher.Invoke(new Action(() =>
				{
					datePicker.SelectedDate = DateTime.ParseExact(text, format, CultureInfo.InvariantCulture);
				}));
			}
			else
			{
				return false;
			}
			return true;
		}
		public static string ProcessDateStr(string dateStr)
		{
			if (dateStr.Length >= 10)
			{
				return dateStr;
			}
			char sep = '/';
			var parts = dateStr.Split(sep);
			if (parts.Length == 1)
			{
				sep = '.';
				parts = dateStr.Split(sep);
			}
			if (parts.Length <= 2)
			{
				return dateStr;
			}
			var first = parts[0];
			var second = parts[1];
			var third = parts[2];
			if (first.Length == 1)
			{
				first = "0" + first;
			}
			if (second.Length == 1)
			{
				second = "0" + second;
			}
			var result = first + sep + second + sep + third;
			return result;
		}
	}

	public class MessageBoxFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 1, m_name);

			var message = Utils.GetSafeString(args, 0);
			var caption = Utils.GetSafeString(args, 1, "Info");
			var answerType = Utils.GetSafeString(args, 2, "ok").ToLower();
			var messageType = Utils.GetSafeString(args, 3, "info").ToLower();

			var ret = ShowMessageBox(message, caption, answerType, messageType);

			return new Variable(ret);
		}

		public static string ShowMessageBox(string message, string caption = "Info", string answerType = "ok", string messageType = "info")
		{
			MessageBoxButton buttons =
				answerType == "ok" ? MessageBoxButton.OK :
				answerType == "okcancel" ? MessageBoxButton.OKCancel :
				answerType == "yesno" ? MessageBoxButton.YesNo :
				answerType == "yesnocancel" ? MessageBoxButton.YesNoCancel : MessageBoxButton.OK;

			MessageBoxImage icon =
				messageType == "question" ? MessageBoxImage.Question :
				messageType == "info" ? MessageBoxImage.Information :
				messageType == "warning" ? MessageBoxImage.Warning :
				messageType == "error" ? MessageBoxImage.Error :
				messageType == "exclamation" ? MessageBoxImage.Exclamation :
				messageType == "stop" ? MessageBoxImage.Stop :
				messageType == "hand" ? MessageBoxImage.Hand :
				messageType == "asterisk" ? MessageBoxImage.Asterisk :
							MessageBoxImage.None;
			var result = MessageBox.Show(message, caption,
							 buttons, icon);

			var ret = result == MessageBoxResult.OK ? "OK" :
				result == MessageBoxResult.Cancel ? "Cancel" :
				result == MessageBoxResult.Yes ? "Yes" :
				result == MessageBoxResult.No ? "No" : "None";

			return ret;
		}
	}

	public class YearFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 1, m_name);
			CultureInfo provider = CultureInfo.InvariantCulture;
			var dateVariable = Utils.GetSafeVariable(args, 0);
			DateTime datum = DateTime.ParseExact(dateVariable.String, "dd/MM/yy", provider);
			var GODINA = datum.Year;
			return new Variable(GODINA);
		}
	}
	public class DOMFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 1, m_name);
			var dateVariable = Utils.GetSafeVariable(args, 0);
			var rezultat = dateVariable.String.Substring(0, 2);
			return new Variable(rezultat);
		}
	}
	public class DOWFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 1, m_name);
			CultureInfo provider = CultureInfo.InvariantCulture;
			var dateVariable = Utils.GetSafeVariable(args, 0);
			DateTime datum = DateTime.ParseExact(dateVariable.String, "dd/MM/yy", provider);
			var danTjedan = datum.DayOfWeek;
			var rezultat = danTjedan;
			return new Variable(rezultat);
		}
	}

	class AddWidgetDataFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			string widgetName = Utils.GetToken(script, Constants.TOKEN_SEPARATION);
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 1, m_name);
			var data = args[0];
			var gui = CSCS_GUI.GetInstance(script);
			var widget = gui.GetWidget(widgetName);
			var itemsAdded = 0;
			if (widget is ComboBox)
			{
				var combo = widget as ComboBox;
				if (data.Type == Variable.VarType.ARRAY)
				{
					foreach (var item in data.Tuple)
					{
						combo.Items.Add(item.AsString());
					}
					itemsAdded = data.Tuple.Count;
				}
				else
				{
					combo.Items.Add(data.AsString());
					itemsAdded = 1;
				}
			}
			else if (widget is DataGrid)
			{
				List<string> source = new List<string>();
				DataGrid dg = widget as DataGrid;
				if (data.Type == Variable.VarType.ARRAY && data.Tuple.Count > 0)
				{
					dynamic row = new ExpandoObject();

					for (int i = 0; i < dg.Columns.Count; i++)
					{
						var column = dg.Columns[i].Header.ToString();
						var val = data.Tuple.Count > i ? data.Tuple[i].AsString() : "";
						((IDictionary<String, Object>)row)[column.Replace(' ', '_')] = val;
					}

					dg.Items.Add(row);
				}
				else
				{
					var dataItems = data.AsString().Split(',');
					for (int i = 0; i < dataItems.Length; i++)
					{
						dg.Items.Add(dataItems[i]);
					}
					itemsAdded = dataItems.Length;
				}
			}

			else if (widget is ListView)
			{
				List<string> source = new List<string>();
				ListView listView = widget as ListView;
				if (data.Type == Variable.VarType.ARRAY && data.Tuple.Count > 0)
				{
					StringBuilder viewItem = new StringBuilder();
					for (int i = 0; i < data.Tuple.Count; i++)
					{
						viewItem.Append(data.Tuple[i].AsString());
						source.Add(data.Tuple[i].AsString());
					}
				}
				else
				{
					var dataItems = data.AsString().Split(',');
					for (int i = 0; i < dataItems.Length; i++)
					{
						listView.Items.Add(dataItems[i]);
					}
					itemsAdded = dataItems.Length;
				}
			}

			return new Variable(itemsAdded);
		}
	}
	public class Get_comp_yearFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 0, m_name);

			if (args.Count == 0)
			{
				var result = CSCS_GUI.Adictionary.SY_DATABASESList.FirstOrDefault(p => p.SYCD_USERCODE.ToLower().TrimEnd() == CSCS_GUI.DefaultDB.ToLower().TrimEnd());
				if (result != null)
					return new Variable(result.SYCD_YEAR);
			}
			else if (args.Count == 1)
			{
				var result = CSCS_GUI.Adictionary.SY_DATABASESList.FirstOrDefault(p => p.SYCD_USERCODE.ToLower().TrimEnd() == Utils.GetSafeString(args, 0).ToLower());
				if (result != null)
					return new Variable(result.SYCD_YEAR);
			}
			else if (args.Count == 2)
			{
				if (Utils.GetSafeString(args, 1).ToLower() == "dbase")
				{
					var result = CSCS_GUI.Adictionary.SY_DATABASESList.FirstOrDefault(p => p.SYCD_DBASENAME.ToLower().TrimEnd() == Utils.GetSafeString(args, 0).ToLower());
					if (result != null)
						return new Variable(result.SYCD_YEAR);
				}
			}

			return new Variable("");
		}


	}
	public class Get_dbaseFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 1, m_name);

			var USERCODE = Utils.GetSafeString(args, 0).ToLower();
			var offset = Utils.GetSafeInt(args, 1);
			//SqlConnection conn = new SqlConnection(CSCS_SQL.ConnectionString);

			//conn.Open();

			//CSCS_GUI.Adictionary.SY_DATABASESList = AdictionaryLocal.CacheAdictionary.GetSY_DATABASES(conn);
			var tmp = CSCS_GUI.Adictionary.SY_DATABASESList.FirstOrDefault(p => p.SYCD_USERCODE.ToLower().TrimEnd() == USERCODE.TrimEnd());
			if (tmp != null)
			{
				var startYear = tmp.SYCD_YEAR;
				var SYCD_COMPCODE = tmp.SYCD_COMPCODE;
				var startYearWithOffset = int.Parse(startYear) + offset;
				var tmp2 = CSCS_GUI.Adictionary.SY_DATABASESList.FirstOrDefault(p => p.SYCD_COMPCODE.ToLower().TrimEnd() == SYCD_COMPCODE.ToLower().TrimEnd() && p.SYCD_YEAR.ToLower().TrimEnd() == startYearWithOffset.ToString());
				if (tmp2 != null)
					return new Variable(tmp2.SYCD_DBASENAME);
			}

			//conn.Close();
			return new Variable();
		}


	}
	//public class Get_comp_yearFunction : ParserFunction
	//{
	//	protected override Variable Evaluate(ParsingScript script)
	//	{
	//		List<Variable> args = script.GetFunctionArgs();
	//		Utils.CheckArgs(args.Count, 2, m_name);

	//		var comp_code = Utils.GetSafeString(args, 0).ToLower();
	//		var type_of_code = Utils.GetSafeString(args, 1).ToLower();
	//		SqlConnection conn = new SqlConnection(CSCS_SQL.ConnectionString);

	//		conn.Open();

	//		CSCS_GUI.Adictionary.SY_DATABASESList = AdictionaryLocal.CacheAdictionary.GetSY_DATABASES(conn);
	//		var result = CSCS_GUI.Adictionary.SY_DATABASESList.FirstOrDefault(p => p.SYCD_COMPCODE.ToLower().TrimEnd() == comp_code.TrimEnd()).SYCD_YEAR;

	//		conn.Close();
	//		return new Variable(result); ;
	//	}


	//}

	class SetWindowOptionsFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 2, m_name);
			var windowName = Utils.GetSafeString(args, 0).ToLower();
			var option = Utils.GetSafeString(args, 1).ToLower();
			
			var gui = CSCS_GUI.GetInstance(script);

			Window window = CSCS_GUI.Window2File.Keys.FirstOrDefault(p => p.Name.ToLower() == windowName);
			
			var parameter = Utils.GetSafeString(args, 2);
			
			// try generically
			foreach (var prop in window.GetType().GetProperties())
				if (prop.Name.ToLower() == option.ToLower())
					prop.SetValue(window, Convert.ChangeType(parameter, prop.PropertyType), null);


			return new Variable(true);
		}
	}
	
	class SetWidgetOptionsFunction : ParserFunction
	{
		//emin

		public static bool settingTabControlPosition;

		Dictionary<string, Color> m_bgcolors = new Dictionary<string, Color>();
		Dictionary<string, Color> m_fgcolors = new Dictionary<string, Color>();

		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 2, m_name);
			var widgetName = Utils.GetSafeString(args, 0).ToLower();
			var option = Utils.GetSafeString(args, 1).ToLower();
			var gui = CSCS_GUI.GetInstance(script);
			FrameworkElement widget = gui.GetWidget(widgetName);
			var parameter = Utils.GetSafeString(args, 2);
			var prop_mapped = WidgetPropertyMap // try mapped
			.OrderBy(a => a.Item1 == "*" ? 1 : 0)  // first the non *'s
			.Where(a => (a.Item1.ToLower() == widget.GetType().Name.ToLower() || a.Item1.ToLower() == "*") && a.Item2.ToLower() == option) // overrides or *'s
			.Select(a => a.Item3).FirstOrDefault();// real mapped prop name
			var prop_by_prop_mapped = widget.GetType().GetProperties().Where(a => a.Name.ToLower() == (prop_mapped ?? "").ToLower()).FirstOrDefault();

			if (!string.IsNullOrEmpty(prop_mapped) && prop_by_prop_mapped != null)
            {
                //Chart("ChartPoMjesecima", "init");
                //Chart("ChartPoMjesecima", "seriesType", "Columnseries");
                //Chart("ChartPoMjesecima", "title", "Naslov grafa", 20);
                //Chart("ChartPoMjesecima", "labels", "y", 13);
                //Chart("ChartPoMjesecima", "labels", "x", 13, { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"});
                //Chart("ChartPoMjesecima", "xlabelsRotation", 0);
                //Chart("ChartPoMjesecima", "values", a1, "aaaaaa1");
                //Chart("ChartPoMjesecima", "values", a2, "aaaaaa2");
                //Chart("ChartPoMjesecima", "values", a3, "aaaaaa3");
                //Chart("ChartPoMjesecima", "values", a4, "aaaaaa4");
                //Chart("ChartPoMjesecima", "SeparatorStep", 1);
                //Chart("ChartPoMjesecima", "Margins", { 50, 20, 0, 30});
                //Chart("ChartPoMjesecima", "TooltipDecimalPlaces", 2);

                if (option.StartsWith("Color".ToLower())) // from WidgetPropertyMap
                {
                    // simple fix for color
                    prop_by_prop_mapped.SetValue(widget, new SolidColorBrush((Color)ColorConverter.ConvertFromString(parameter)), null);
                }
                else if (option.StartsWith("Visible".ToLower())) // from WidgetPropertyMap
                {
                    // fix for Visibility
                    prop_by_prop_mapped.SetValue(widget, Set(parameter) ? Visibility.Visible : Visibility.Hidden, null);
                }
                else if (option.StartsWith("Enabled".ToLower())) // from WidgetPropertyMap
                {
                    // fix for Enabled
                    prop_by_prop_mapped.SetValue(widget, Set(parameter), null);
                }
				else if (option.StartsWith("Source".ToLower())) // from WidgetPropertyMap
                {
                    prop_by_prop_mapped.SetValue(widget, new Uri(parameter), null);
                }
                else
                {
                    // try by name directly
                    prop_by_prop_mapped.SetValue(widget, Convert.ChangeType(parameter, prop_by_prop_mapped.PropertyType), null);
                }
            }
            else
            {
                // try generically
                foreach (var prop in widget.GetType().GetProperties())
                    if (prop.Name.ToLower() == option.ToLower())
                        prop.SetValue(widget, Convert.ChangeType(parameter, prop.PropertyType), null);
            }

            return new Variable(true);

        }


		public string ToHex(Color c) => $"#{c.R:X2}{c.G:X2}{c.B:X2}";

		private bool Set(string parameter)
		{
			return new List<string> { "True".ToLower(), "On".ToLower(), "Yes".ToLower(), "Enabled".ToLower(), "1", "👍" }.Contains(parameter.ToLower());
		}

		private static List<(string, string, string)> WidgetPropertyMap = new List<(string, string, string)>
		{
			// common
			("*", "Text", "Content"),
			("*", "Text.Hover", "ToolTip"),
			("*", "Color", "Foreground"),
			("*", "Color", "Color.Foreground"),
			("*", "Color.Border", "BorderBrush"),
			("*", "Color.Text", "Foreground"),
			("*", "Color.Background", "Background"),
			("*", "Enabled", "IsEnabled"),
			("*", "Visible", "Visibility"),

			//charts
			("*", "Color.Series", "Series"),
			("*", "Text.SeriesNames", "Series"),


			("Button", "Text.Description", ""),
			("Button", "Text.Size", ""),
			("Button", "Text.Name", ""),
			("Button", "Text.Style", ""),

			("Label", "Text.Description", ""),
			("Label", "Text.Size", ""),
			("Label", "Text.Name", ""),
			("Label", "Text.Style", ""),

			("Grid", "Text.Description", ""),
			("Grid", "Text.Size", ""),
			("Grid", "Text.Name", ""),
			("Grid", "Text.Style", ""),

			("Ellipse", "Color", "Fill"),
			("Ellipse", "Color.Background", "Fill"),
			("Ellipse", "Color.Foreground", "Fill"),
			("Ellipse", "Color.Border", "Stroke"),
			("Ellipse", "Text.Description", ""),
			("Ellipse", "Text.Size", ""),
			("Ellipse", "Text.Name", ""),
			("Ellipse", "Text.Style", ""),

			("WebView2", "Source", "Source"),

			//("CartesianChart", "Color.Series", "Series"),
			//("CartesianChart", "Text.SeriesNames", "Series")

			//("PlaceholderToCopy", "Color", ""),
			//("PlaceholderToCopy", "Color.Foreground", ""),
			//("PlaceholderToCopy", "Color.Background", ""),
			//("PlaceholderToCopy", "Color.Border", ""),
			//("PlaceholderToCopy", "Color.Text", ""),
			//("PlaceholderToCopy", "Text", ""),
			//("PlaceholderToCopy", "Text.Description", ""),
			//("PlaceholderToCopy", "Text.Hover", ""),
			//("PlaceholderToCopy", "Text.Size", ""),
			//("PlaceholderToCopy", "Text.Name", ""),
			//("PlaceholderToCopy", "Text.Style", ""),
		};

		public static bool ClearWidget(CSCS_GUI gui, string widgetName, FrameworkElement widget = null)
		{
			widget = widget == null ? gui.GetWidget(widgetName) as DataGrid : widget;
			if (widget is DataGrid)
			{
				if (gui.WIDGETS.TryGetValue(widgetName, out CSCS_GUI.WidgetData wd))
				{
					foreach (var entry in wd.headers)
					{
						entry.Value.Tuple.Clear();
						gui.Interpreter.AddGlobal(entry.Key, new GetVarFunction(entry.Value), false);
					}
				}

				var dg = widget as DataGrid;
				var rowList = dg.ItemsSource as List<ExpandoObject>;
				FillWidgetFunction.ResetArrays(gui, dg);
				rowList?.Clear();
				dg.ItemsSource = null;
				dg.Items.Refresh();
				dg.UpdateLayout();
			}

			return widget is DataGrid;
		}

		static bool IsUserVisible(FrameworkElement element, FrameworkElement container)
		{
			if (element == null || !element.IsVisible)
				return false;
			Rect bounds = element.TransformToAncestor(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
			Rect rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
			return rect.Contains(bounds.TopLeft) || rect.Contains(bounds.BottomRight);
		}

		public static void RedisplayWidget(DataGrid dg, string option = "current", int row = -1)
		{
			var rowList = dg.ItemsSource as List<ExpandoObject>;

			if (dg.Items == null || rowList.Count == 0)
			{
				return;
			}
			row = row >= 0 ? row : option.EndsWith("top") ? 0 : option.EndsWith("end") || option.EndsWith("bottom") ? rowList.Count - 1 : dg.SelectedIndex;
			row = row < 0 ? 0 : row;
			//var visibles = dg.Scr;
			dg.ScrollIntoView(rowList[row]);

			if (row != 0 && row != rowList.Count - 1)
			{
				DataGridRow rowElem = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex);
				rowElem?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
				int visible = rowElem != null ? (int)(dg.ActualHeight / rowElem.ActualHeight) - 2 : 0;

				var adjusted = row - visible >= 0 ? row + visible : 0;
				if (adjusted != row)
				{
					dg.ScrollIntoView(rowList[adjusted]);
				}
			}
		}

		void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			DataGrid dg = sender as DataGrid;
			var gui = dg.DataContext as CSCS_GUI;
			string widgetName = CSCS_GUI.GetWidgetBindingName(dg);
			Color bgcolor, fgcolor;
			if (m_bgcolors.TryGetValue(widgetName, out bgcolor))
			{
				e.Row.Background = new SolidColorBrush(bgcolor);
			}
			if (m_fgcolors.TryGetValue(widgetName, out fgcolor))
			{
				e.Row.Foreground = new SolidColorBrush(fgcolor);
			}
		}

		public static Color StringToColor(string strColor)
		{
			switch (strColor.ToLower())
			{
				case "black": return Colors.Black;
				case "white": return Colors.White;
				case "green": return Colors.Green;
				case "red": return Colors.Red;
				case "blue": return Colors.Blue;
				case "brown": return Colors.Brown;
				case "yellow": return Colors.Yellow;
				case "rose": return Colors.MistyRose;
				case "purple": return Colors.Purple;
				case "orange": return Colors.Orange;
				case "magenta": return Colors.Magenta;
				case "maroon": return Colors.Maroon;
				case "aqua": return Colors.Aqua;
				case "aquamarine": return Colors.Aquamarine;
				case "azure": return Colors.Azure;
				case "beige": return Colors.Beige;
				case "chocolate": return Colors.Chocolate;
				case "coral": return Colors.Coral;
				case "cyan": return Colors.Cyan;
				case "darkblue": return Colors.DarkBlue;
				case "darkcyan": return Colors.DarkCyan;
				case "darkgray": return Colors.DarkGray;
				case "darkgreen": return Colors.DarkGreen;
				case "darkkhaki": return Colors.DarkKhaki;
				case "darkorange": return Colors.DarkOrange;
				case "darkred": return Colors.DarkRed;
				case "darkturquoise": return Colors.DarkTurquoise;
				case "deeppink": return Colors.DeepPink;
				case "deepskyblue": return Colors.DeepSkyBlue;
				case "dimgray": return Colors.DimGray;
				case "gray": return Colors.Gray;
				case "gold": return Colors.Gold;
				case "greenyellow": return Colors.GreenYellow;
				case "hotpink": return Colors.HotPink;
				case "indigo": return Colors.Indigo;
				case "khaki": return Colors.Khaki;
				case "lightblue": return Colors.LightBlue;
				case "lightcyan": return Colors.LightCyan;
				case "lightgray": return Colors.LightGray;
				case "lightgreen": return Colors.LightGreen;
				case "lightpink": return Colors.LightPink;
				case "lightskyblue": return Colors.LightSkyBlue;
				case "lime": return Colors.Lime;
				case "limegreen": return Colors.LimeGreen;
				case "navy": return Colors.Navy;
				case "olive": return Colors.Olive;
				case "salmon": return Colors.Salmon;
				case "silver": return Colors.Silver;
				case "skyblue": return Colors.SkyBlue;
				case "snow": return Colors.Snow;
				case "violet": return Colors.Violet;
			}

			var color = (Color)ColorConverter.ConvertFromString(strColor);
			return color;
		}
	}



	class SetWidgetOptionsFunction_old : ParserFunction
	{
		public static bool settingTabControlPosition;

		Dictionary<string, Color> m_bgcolors = new Dictionary<string, Color>();
		Dictionary<string, Color> m_fgcolors = new Dictionary<string, Color>();

		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 2, m_name);

			var widgetName = Utils.GetSafeString(args, 0).ToLower();
			var option = Utils.GetSafeString(args, 1).ToLower();

			var gui = CSCS_GUI.GetInstance(script);
			var widget = gui.GetWidget(widgetName);
			var parameter = Utils.GetSafeString(args, 2).ToLower();

			switch (option)
			{

				case "visible":
					widget.Visibility = parameter == "true" ? Visibility.Visible : Visibility.Hidden;
					break;
				case "enabled":
					widget.IsEnabled = parameter == "true" ? true : false;
					break;
				case "width":
					widget.Width = int.Parse(parameter);
					break;
				case "height":
					widget.Height = int.Parse(parameter);
					break;
			}



			if (widget is DataGrid)
			{
				DataGrid dg = widget as DataGrid;
				dg.DataContext = gui;
				if (option == "colors")
				{
					var bgColor = Utils.GetSafeString(args, 2).ToLower();
					var fgColor = Utils.GetSafeString(args, 3, "black").ToLower();
					m_bgcolors[widgetName] = StringToColor(bgColor);
					m_fgcolors[widgetName] = StringToColor(fgColor);

					dg.LoadingRow += new EventHandler<DataGridRowEventArgs>(DataGrid_LoadingRow);
				}
				else if (option == "columns")
				{
					var colNames = args[2];
					if (colNames.Type != Variable.VarType.ARRAY)
					{
						string label = colNames.AsString();
						DataGridTextColumn column = new DataGridTextColumn();
						column.Header = label;
						column.Binding = new Binding(label.Replace(' ', '_'));

						dg.Columns.Add(column);
					}
					else
					{
						foreach (var item in colNames.Tuple)
						{
							string label = item.ToString();
							DataGridTextColumn column = new DataGridTextColumn();
							column.Header = label;
							column.Binding = new Binding(label.Replace(' ', '_'));

							dg.Columns.Add(column);
						}
					}
				}
				else if (option == "clear")
				{
					ClearWidget(gui, widgetName, dg);
				}
				else if (option == "editmode")
				{
					dg.IsReadOnly = false;
				}
				else if (option == "selectmode")
				{
					dg.IsReadOnly = true;
				}
			}

			else if (widget is TabControl)
			{
				TabControl tc = widget as TabControl;
				if (option == "position")
				{
					settingTabControlPosition = true;
					tc.SelectedIndex = Utils.GetSafeInt(args, 2);
					settingTabControlPosition = false;
				}
			}

			else if (widget is TextBox)
			{
				TextBox tb = widget as TextBox;
				if (option == "text")
				{
					tb.Text = Utils.GetSafeString(args, 2);
				}
				if (option == "font")
					tb.FontFamily = new FontFamily(parameter);
				if (option == "fontsize")
					tb.FontSize = int.Parse(parameter);
			}
			else if (widget is TextBlock)
			{
				TextBlock tb = widget as TextBlock;
				if (option == "text")
				{
					tb.Text = Utils.GetSafeString(args, 2);
				}
				if (option == "font")
					tb.FontFamily = new FontFamily(parameter);
				if (option == "fontsize")
					tb.FontSize = int.Parse(parameter);
			}
			else if (widget is Label)
			{
				Label tb = widget as Label;
				if (option == "text")
				{
					tb.Content = Utils.GetSafeString(args, 2);
				}
				if (option == "font")
					tb.FontFamily = new FontFamily(parameter);
				if (option == "fontsize")
					tb.FontSize = int.Parse(parameter);
			}
			else if (widget is Grid)
			{
				//TextBox tb = widget as TextBox;
				//if (option == "text")
				//{
				//    tb.Text = Utils.GetSafeString(args, 2);
				//}
			}

			return new Variable(true);
		}

		public static bool ClearWidget(CSCS_GUI gui, string widgetName, FrameworkElement widget = null)
		{
			widget = widget == null ? gui.GetWidget(widgetName) as DataGrid : widget;
			if (widget is DataGrid)
			{
				if (gui.WIDGETS.TryGetValue(widgetName, out CSCS_GUI.WidgetData wd))
				{
					foreach (var entry in wd.headers)
					{
						entry.Value.Tuple.Clear();
						gui.Interpreter.AddGlobal(entry.Key, new GetVarFunction(entry.Value), false);
					}
				}

				var dg = widget as DataGrid;
				var rowList = dg.ItemsSource as List<ExpandoObject>;
				FillWidgetFunction.ResetArrays(gui, dg);
				rowList?.Clear();
				dg.ItemsSource = null;
				dg.Items.Refresh();
				dg.UpdateLayout();
			}

			return widget is DataGrid;
		}

		static bool IsUserVisible(FrameworkElement element, FrameworkElement container)
		{
			if (element == null || !element.IsVisible)
				return false;
			Rect bounds = element.TransformToAncestor(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
			Rect rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
			return rect.Contains(bounds.TopLeft) || rect.Contains(bounds.BottomRight);
		}

		public static void RedisplayWidget(DataGrid dg, string option = "current", int row = -1)
		{
			var rowList = dg.ItemsSource as List<ExpandoObject>;

			if (dg.Items == null || rowList.Count == 0)
			{
				return;
			}
			row = row >= 0 ? row : option.EndsWith("top") ? 0 : option.EndsWith("end") || option.EndsWith("bottom") ? rowList.Count - 1 : dg.SelectedIndex;
			row = row < 0 ? 0 : row;
			//var visibles = dg.Scr;
			dg.ScrollIntoView(rowList[row]);

			if (row != 0 && row != rowList.Count - 1)
			{
				DataGridRow rowElem = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex);
				rowElem?.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
				int visible = rowElem != null ? (int)(dg.ActualHeight / rowElem.ActualHeight) - 2 : 0;

				var adjusted = row - visible >= 0 ? row + visible : 0;
				if (adjusted != row)
				{
					dg.ScrollIntoView(rowList[adjusted]);
				}
			}
		}

		void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			DataGrid dg = sender as DataGrid;
			var gui = dg.DataContext as CSCS_GUI;
			string widgetName = CSCS_GUI.GetWidgetBindingName(dg);
			Color bgcolor, fgcolor;
			if (m_bgcolors.TryGetValue(widgetName, out bgcolor))
			{
				e.Row.Background = new SolidColorBrush(bgcolor);
			}
			if (m_fgcolors.TryGetValue(widgetName, out fgcolor))
			{
				e.Row.Foreground = new SolidColorBrush(fgcolor);
			}
		}

		public static Color StringToColor(string strColor)
		{
			switch (strColor.ToLower())
			{
				case "black": return Colors.Black;
				case "white": return Colors.White;
				case "green": return Colors.Green;
				case "red": return Colors.Red;
				case "blue": return Colors.Blue;
				case "brown": return Colors.Brown;
				case "yellow": return Colors.Yellow;
				case "rose": return Colors.MistyRose;
				case "purple": return Colors.Purple;
				case "orange": return Colors.Orange;
				case "magenta": return Colors.Magenta;
				case "maroon": return Colors.Maroon;
				case "aqua": return Colors.Aqua;
				case "aquamarine": return Colors.Aquamarine;
				case "azure": return Colors.Azure;
				case "beige": return Colors.Beige;
				case "chocolate": return Colors.Chocolate;
				case "coral": return Colors.Coral;
				case "cyan": return Colors.Cyan;
				case "darkblue": return Colors.DarkBlue;
				case "darkcyan": return Colors.DarkCyan;
				case "darkgray": return Colors.DarkGray;
				case "darkgreen": return Colors.DarkGreen;
				case "darkkhaki": return Colors.DarkKhaki;
				case "darkorange": return Colors.DarkOrange;
				case "darkred": return Colors.DarkRed;
				case "darkturquoise": return Colors.DarkTurquoise;
				case "deeppink": return Colors.DeepPink;
				case "deepskyblue": return Colors.DeepSkyBlue;
				case "dimgray": return Colors.DimGray;
				case "gray": return Colors.Gray;
				case "gold": return Colors.Gold;
				case "greenyellow": return Colors.GreenYellow;
				case "hotpink": return Colors.HotPink;
				case "indigo": return Colors.Indigo;
				case "khaki": return Colors.Khaki;
				case "lightblue": return Colors.LightBlue;
				case "lightcyan": return Colors.LightCyan;
				case "lightgray": return Colors.LightGray;
				case "lightgreen": return Colors.LightGreen;
				case "lightpink": return Colors.LightPink;
				case "lightskyblue": return Colors.LightSkyBlue;
				case "lime": return Colors.Lime;
				case "limegreen": return Colors.LimeGreen;
				case "navy": return Colors.Navy;
				case "olive": return Colors.Olive;
				case "salmon": return Colors.Salmon;
				case "silver": return Colors.Silver;
				case "skyblue": return Colors.SkyBlue;
				case "snow": return Colors.Snow;
				case "violet": return Colors.Violet;
			}

			var color = (Color)ColorConverter.ConvertFromString(strColor);
			return color;
		}
	}

	class OpenFileFunction : ParserFunction
	{
		bool m_getFileContents;

		public OpenFileFunction(bool getContents)
		{
			m_getFileContents = getContents;
		}

		protected override Variable Evaluate(ParsingScript script)
		{
			/*List<Variable> args =*/
			script.GetFunctionArgs();
			return OpenFile(m_getFileContents);
		}
		public static Variable OpenFile(bool getContents = false)
		{
			Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog();
			if (openFile.ShowDialog() != true)
			{
				return Variable.EmptyInstance;
			}

			var fileName = openFile.FileName;
			if (!getContents)
			{
				return new Variable(fileName);
			}
			string contents = Utils.GetFileContents(fileName);
			contents = contents.Replace("\n", Environment.NewLine);
			return new Variable(contents);
		}
	}

	//class SaveFileFunction : ParserFunction
	//{
	//	protected override Variable Evaluate(ParsingScript script)
	//	{
	//		List<Variable> args = script.GetFunctionArgs();
	//		string text = Utils.GetSafeString(args, 0);

	//		return SaveFile(text);
	//	}
	//	public static Variable SaveFile(string text)
	//	{
	//		Microsoft.Win32.SaveFileDialog saveFile = new Microsoft.Win32.SaveFileDialog();
	//		if (saveFile.ShowDialog() != true)
	//		{
	//			return Variable.EmptyInstance;
	//		}

	//		var fileName = saveFile.FileName;
	//		File.WriteAllText(fileName, text);
	//		return new Variable(fileName);
	//	}
	//}

    class SaveFileFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            // Get the function arguments
            List<Variable> args = script.GetFunctionArgs();

            // Ensure at least one argument (textToSave) is provided
            if (args.Count == 0)
            {
                throw new ArgumentException("At least one argument (textToSave) is required.");
            }

            string textToSave = Utils.GetSafeString(args, 0); // First argument: text to save
            string fileName = Utils.GetSafeString(args, 1);   // Second argument: file name (optional)
            string appendFlag = Utils.GetSafeString(args, 2); // Third argument: append flag (optional)

            // Call the SaveToFile method with the provided arguments
            return SaveToFile(textToSave, fileName, appendFlag);
        }

        public static Variable SaveToFile(string textToSave, string fileName = null, string appendFlag = null)
        {
            // If no file name is provided, prompt the user to select a file
            if (string.IsNullOrEmpty(fileName))
            {
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                if (saveFileDialog.ShowDialog() != true)
                {
                    return Variable.EmptyInstance; // User canceled the dialog
                }
                fileName = saveFileDialog.FileName;
            }

            // Determine whether to append or overwrite the file
            bool append = !string.IsNullOrEmpty(appendFlag) && appendFlag.ToUpper() == "A";

            //create directory if it doesn't exist
            string directory = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Save or append the text to the file
            try
            {
                if (append)
                {
					var ienumString = new List<string>();
					ienumString.Add(textToSave);
                    File.AppendAllLines(fileName, ienumString);
                }
                else
                {
                    File.WriteAllText(fileName, textToSave);
                }
                return new Variable(fileName); // Return the file name
            }
            catch (Exception ex)
            {
                // Handle any errors (e.g., invalid path, unauthorized access)
                return new Variable($"Error: {ex.Message}");
            }
        }
    }


    class SetColorFunction : ParserFunction
	{
		bool m_bgColor;

		public SetColorFunction(bool bgcolor)
		{
			m_bgColor = bgcolor;
		}

		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 1, m_name);

			var gui = CSCS_GUI.GetInstance(script);
			var widgetName = Utils.GetSafeString(args, 0);
			var colorName = Utils.GetSafeString(args, 1);
			var widget = gui.GetWidget(widgetName);
			if (widget == null || !(widget is Control))
			{
				return Variable.EmptyInstance;
			}

			var color = SetWidgetOptionsFunction.StringToColor(colorName);
			SolidColorBrush brush = new SolidColorBrush(color);

			if (m_bgColor)
			{
				((Control)widget).Background = brush;
			}
			else
			{
				((Control)widget).Foreground = brush;
			}
			return new Variable(true);
		}
	}

	class SetImageFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 1, m_name);

			var gui = CSCS_GUI.GetInstance(script);
			var widgetName = Utils.GetSafeString(args, 0);
			var imageName = Utils.GetSafeString(args, 1);
			var widget = gui.GetWidget(widgetName);
			if (widget == null)
			{
				return Variable.EmptyInstance;
			}

			bool found = false;
			if (widget is ContentControl)
			{
				var control = widget as ContentControl;
				control.Content = new Image
				{
					Source = new BitmapImage(new Uri(imageName, UriKind.RelativeOrAbsolute)),
					VerticalAlignment = VerticalAlignment.Center,
					Stretch = Stretch.None
				};
				found = true;
			}
			else if (widget is System.Windows.Controls.Image)
			{
				var img = widget as Image;
				img.Source = new BitmapImage(new Uri(imageName, UriKind.RelativeOrAbsolute));
				found = true;
			}

			return new Variable(found);
		}
	}

	class AddMenuEntryFunction : ParserFunction
	{
		bool m_separator;

		public AddMenuEntryFunction(bool separator = false)
		{
			m_separator = separator;
		}

		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 1, m_name);

			var parentName = Utils.GetSafeString(args, 0);
			var gui = CSCS_GUI.GetInstance(script);
			ItemsControl parent = gui.GetWidget(parentName) as ItemsControl;
			if (parent == null)
			{
				return Variable.EmptyInstance;
			}

			gui.Control2Window.TryGetValue(parent, out Window win);

			if (m_separator)
			{
				parent.Items.Add(new Separator());
				return new Variable(parentName);
			}

			Utils.CheckArgs(args.Count, 2, m_name);
			var menuName = Utils.GetSafeString(args, 1);
			var menuLabel = Utils.GetSafeString(args, 2, menuName);
			var menuAction = Utils.GetSafeString(args, 3);

			MenuItem newMenuItem = new MenuItem();
			newMenuItem.Header = menuLabel;
			newMenuItem.Name = GetValidName(menuName);
			newMenuItem.DataContext = menuName;

			if (!string.IsNullOrWhiteSpace(menuAction))
			{
				newMenuItem.Click += (sender, eventArgs) =>
				{
					gui.Interpreter.Run(menuAction, new Variable(menuName), new Variable(eventArgs.Source.ToString()),
			null, script);
				};
			}

			parent.Items.Add(newMenuItem);
			gui.CacheControl(newMenuItem, win);

			return new Variable(menuName);
		}

		public static string GetValidName(string name)
		{
			string newName = name.Replace('.', '_').Replace('(', '_').Replace(')', '_').Replace('-', '_').Replace(' ', '_');
			foreach (char c in Path.GetInvalidFileNameChars())
			{
				newName = newName.Replace(c, '_');
			}
			return newName;
		}
	}

	class RemoveMenuFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 1, m_name);

			var gui = CSCS_GUI.GetInstance(script);
			var parentName = Utils.GetSafeString(args, 0);
			ItemsControl parent = gui.GetWidget(parentName) as ItemsControl;
			if (parent == null || parent.Items == null)
			{
				return Variable.EmptyInstance;
			}

			RemoveMenu(gui, parent);
			return new Variable(true);
		}

		static void RemoveMenu(CSCS_GUI gui, ItemsControl parent)
		{
			if (parent == null || parent.Items == null)
			{
				return;
			}

			foreach (var item in parent.Items)
			{
				gui.RemoveControl(item as FrameworkElement);
				RemoveMenu(gui, item as ItemsControl);
			}
			parent.Items.Clear();
		}
	}

	class ShowHideWidgetFunction : ParserFunction
	{
		bool m_showWidget;

		public ShowHideWidgetFunction(bool showWidget)
		{
			m_showWidget = showWidget;
		}

		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 1, m_name);

			var gui = CSCS_GUI.GetInstance(script);
			var widgetName = Utils.GetSafeString(args, 0);
			var widget = gui.GetWidget(widgetName);
			if (widget == null)
			{
				return Variable.EmptyInstance;
			}

			widget.Visibility = m_showWidget ? Visibility.Visible : Visibility.Hidden;

			return new Variable(true);
		}
	}
	class ChainFunction : ParserFunction
	{
		bool m_paramMode;
		CSCS_GUI Gui;
		public static Dictionary<string, ParsingScript> Chains { get; set; } = new Dictionary<string, ParsingScript>();
		public static Dictionary<string, bool> MainWindow { get; set; } = new Dictionary<string, bool>();

		public ChainFunction(bool paramMode = false)
		{
			m_paramMode = paramMode;
		}

		protected override Variable Evaluate(ParsingScript script)
		{
			var separator = new char[] { ',' };
			List<Variable> parameters;
			Gui = CSCS_GUI.GetInstance(script);

			if (m_paramMode)
			{
				var argsStr = Utils.GetBodyBetween(script, '\0', '\0', Constants.END_STATEMENT);
				if (argsStr.EndsWith(")"))
				{
					argsStr = argsStr.Substring(0, argsStr.Length - 1);
				}
				string[] argsArray = argsStr.Split(separator);
				//string msg = "CmdArgs:";
				var fileFullName = script.GetFilePath(script.Filename);
				if (!Gui.Parameters.TryGetValue(fileFullName, out parameters))
				{
					parameters = new List<Variable>();
					string[] cmdArgs = Environment.GetCommandLineArgs();
					var cmdArgsArr = cmdArgs.Length > 2 ? cmdArgs[2].Split(separator) : new string[0];
					for (int i = 0; i < cmdArgsArr.Length; i++)
					{
						parameters.Add(new Variable(cmdArgsArr[i]));
						//msg += "[" + cmdArgsArr[i] + "]";
					}
				}

				for (int i = 0; i < argsArray.Length && i < parameters.Count; i++)
				{
					var func = new GetVarFunction(parameters[i]);
					func.Name = argsArray[i];
					//script.StackLevel.Variables[argsArray[i]] = func;
					script.InterpreterInstance.AddGlobalOrLocalVariable(func.Name, func);

					//msg += func.Name + "=[" + parameters[i].AsString() + "] ";
				}
				//MessageBox.Show(msg, parameters.Count + " args", MessageBoxButton.OK, MessageBoxImage.Hand);

				Chains[script.Filename] = script;
				CheckScriptIsMain(script.Filename);
				return Variable.EmptyInstance;
			}

			int currentScriptPos = script.Pointer;
			string argsExpr = ReplaceSpaces(script);
			var tempScript = script.GetTempScript(argsExpr);
			tempScript.ScriptOffset = script.ScriptOffset + currentScriptPos;
			//List<Variable> args = tempScript.GetFunctionArgs();
			List<Variable> args = GetChainArgs(tempScript);
			Utils.CheckArgs(args.Count, 1, m_name);

			string chainName = args[0].AsString();
			string chainFullName = script.GetFilePath(chainName);
			parameters = new List<Variable>();
			string paramsStr = " \"";
			bool canAdd = false;
			bool newRuntime = false;
			for (int i = 1; i < args.Count; i++)
			{
				if (canAdd)
				{
					if (newRuntime)
					{
						paramsStr += args[i].AsString() + ",";
					}
					else
					{
						parameters.Add(args[i]);
					}
					continue;
				}
				if (string.Equals(args[i].AsString(), Constants.NEWRUNTIME, StringComparison.OrdinalIgnoreCase))
				{
					newRuntime = true;
					continue;
				}
				canAdd = args[i].AsString().ToLower() == Constants.WITH;
				if (!canAdd)
				{
					var parami = args[i].AsString();
					paramsStr += parami + " ";
				}
			}

			if (newRuntime)
			{
				paramsStr = paramsStr.Substring(0, paramsStr.Length - 1) + '"';
				var exec = Process.GetCurrentProcess().MainModule.FileName;
				var result1 = RunExecFunction.RunExec(exec, chainFullName + paramsStr);
				return result1;
			}

			Gui = new CSCS_GUI();
			Gui.Init();

			var tokenSet = CSCS_GUI.GetPreprocessTokens();
			string split1 = ""; string split2 = "";
			string fileName = script.GetFilePath(chainName);
			if (tokenSet.Count > 0)
			{
				Utils.SplitScriptFile(chainName, tokenSet, out split1, out split2, script.PWD, script.Context);
			}
			else
			{
				string data = Utils.File2String(chainName, out fileName, script.PWD);
				split2 = Utils.ConvertToScript(Gui.Interpreter, data, out _, fileName);
			}

			CheckScriptIsMain(chainName);
			Gui.Parameters[fileName] = parameters;

			Variable result = RunTask(Gui, split1, script, chainName);
			if (!string.IsNullOrWhiteSpace(split2))
			{
				result = RunTask(Gui, split2, script, chainName);
			}

			if (result.Type == Variable.VarType.QUIT)
			{
				result.Type = Variable.VarType.NONE;
			}
			return result;
		}

		public static string ReplaceSpaces(ParsingScript script, char replaceChar = ',', char end = Constants.END_STATEMENT)
		{
			StringBuilder sb = new StringBuilder();
			while (script.StillValid() && script.TryCurrent() != end)
			{
				var token = Utils.GetBodyBetween(script, '\0', ' ', end);
				if (token.Equals(Constants.WITH, StringComparison.OrdinalIgnoreCase) && sb.ToString().Last() == replaceChar)
				{
					sb.Remove(sb.Length - 1, 1);
					sb.Append(" " + token + " ");
				}
				else
				{
					sb.Append(token + replaceChar);
				}
			}
			if (sb.Length > 0 && sb[sb.Length - 1] == replaceChar)
			{
				sb.Remove(sb.Length - 1, 1);
			}
			return sb.ToString();
		}

		public static List<Variable> GetChainArgs(ParsingScript script)
		{
			List<Variable> args = new List<Variable>();
			var pos = script.Pointer;
			Variable scrptName = Utils.GetItem(script);
			args.Add(scrptName);

			script.Pointer = pos;
			var token = Utils.GetToken(script, Constants.END_SPACE_ARRAY);
			script.MoveForwardIf(Constants.SPACE);
			if (script.Rest.StartsWith(Constants.WITH + " ", StringComparison.OrdinalIgnoreCase))
			{
				args.Add(new Variable(Constants.WITH));
				script.Pointer += Constants.WITH.Length + 1;
			}
			if (!script.StillValid() || script.Current == Constants.END_STATEMENT)
			{
				return args;
			}

			var moreArgs = script.GetFunctionArgs();
			foreach (var arg in moreArgs)
			{
				args.Add(arg);
			}
			return args;
		}

		static Variable RunTask(CSCS_GUI gui, string scriptStr, ParsingScript parent, string chainName)
		{
			if (string.IsNullOrWhiteSpace(scriptStr))
			{
				return Variable.EmptyInstance;
			}
			string fileName = parent.GetFilePath(chainName);

			//if (Chains.ContainsKey(fileName))
			//{
			//    return Variable.EmptyInstance;
			//}

			ParsingScript chainScript = new ParsingScript(gui.Interpreter, scriptStr);
			chainScript.Filename = fileName;
			chainScript.ParentScript = parent;
			chainScript.InTryBlock = parent.InTryBlock;
			chainScript.StackLevel = gui.Interpreter.AddStackLevel(chainScript.Filename);
			chainScript.CurrentModule = chainName;
			chainScript.ParentScript = parent;
			Chains[chainScript.Filename] = chainScript;
			gui.Script = chainScript;

			chainScript.SetInterpreter(gui.Interpreter);
			chainScript.Context = gui;

			Variable result = Variable.EmptyInstance;
			//Application.Current.Dispatcher.Invoke(new Action(() => {
			while (chainScript.StillValid())
			{
				result = chainScript.Execute();
				if (result.Type == Variable.VarType.QUIT)
				{
					break;
				}
				chainScript.GoToNextStatement();
			}
			//}));

			if (!string.IsNullOrWhiteSpace(chainScript.CurrentModule))
			{
				//throw new ArgumentException("Chained script finished without Quit statement.");
			}

			return result;
		}

		static bool CheckScriptIsMain(string fileName)
		{
			var fullName = Path.GetFullPath(fileName);
			if (!File.Exists(fullName))
			{
				return false;
			}
			var data = File.ReadAllText(fullName);
			bool isMain = data.StartsWith(Constants.MAINMENU);
			MainWindow[fullName] = isMain;
			return isMain;
		}

		public static bool CheckParentScriptIsMain(ParsingScript script)
		{
			var fileName = script != null && script.ParentScript != null ? script.ParentScript.Filename : null;
			if (string.IsNullOrWhiteSpace(fileName))
			{
				return false;
			}
			return CheckScriptIsMain(fileName);
		}
		public static bool IsScriptMain(string fileName)
		{
			var fullName = Path.GetFullPath(fileName);
			if (!MainWindow.TryGetValue(fullName, out bool isMain))
			{
				isMain = CheckScriptIsMain(fullName);
			}
			return isMain;
		}
	}





	class VariableArgsFunction : ParserFunction
	{
		bool m_processFirstToken = true;
		Dictionary<string, Variable> m_parameters;
		string m_lastParameter;
		CSCS_GUI Gui;

		public VariableArgsFunction(bool processFirst = true)
		{
			m_processFirstToken = processFirst;
		}

		void GetParameters(ParsingScript script)
		{
			var gui = CSCS_GUI.GetInstance(script);
			var separator = new char[] { ' ', ';' };
			m_parameters = new Dictionary<string, Variable>();

			while (script.Current != Constants.END_STATEMENT && script.StillValid())
			{
				var labelName = Utils.GetToken(script, Constants.TOKEN_SEPARATION).ToLower();
				var value = labelName == "up" || labelName == "down" || labelName == "local" || labelName == "setup" ||
					labelName == "close" || labelName == "reset" ||
					labelName == "addrow" || labelName == "insertrow" || labelName == "deleterow" ?
					new Variable(true) :
					  script.Current == Constants.END_STATEMENT ? Variable.EmptyInstance :
					  new Variable(Utils.GetToken(script, separator));
				if (labelName != "type" && script.Prev != '"' && !string.IsNullOrWhiteSpace(value.String))
				{
					var existing = gui.Interpreter.GetVariableValue(value.String, script);
					value = existing == null ? value : existing;
				}
				m_parameters[labelName] = value;
				m_lastParameter = labelName;
			}
		}

		string GetParameter(string key, string defValue = "")
		{
			Variable res;
			if (!m_parameters.TryGetValue(key.ToLower(), out res))
			{
				return defValue;
			}
			return res.AsString();
		}
		double GetDoubleParameter(string key, double defValue = 0.0)
		{
			Variable res;
			if (!m_parameters.TryGetValue(key.ToLower(), out res))
			{
				return defValue;
			}
			return res.AsDouble();
		}
		int GetIntParameter(string key, int defValue = 0)
		{
			Variable res;
			if (!m_parameters.TryGetValue(key.ToLower(), out res))
			{
				return defValue;
			}
			return res.AsInt();
		}
		bool GetBoolParameter(string key, bool defValue = false)
		{
			Variable res;
			if (!m_parameters.TryGetValue(key.ToLower(), out res))
			{
				return defValue;
			}
			return res.AsBool();
		}
		Variable GetVariableParameter(string key, Variable defValue = null)
		{
			Variable res;
			if (!m_parameters.TryGetValue(key.ToLower(), out res))
			{
				return defValue;
			}
			return res;
		}

		protected override Variable Evaluate(ParsingScript script)
		{
			var objectName = m_processFirstToken ? Utils.GetToken(script, new char[] { ' ', '}', ')', ';' }) : "";

			Name = Name.ToUpper();
			Gui = CSCS_GUI.GetInstance(script);

			if (Name == Constants.DISPLAY_ARR_SETUP)
			{
				List<Variable> args = script.GetFunctionArgs();
				Utils.CheckArgs(args.Count, 3, m_name);
				var name = args[0].AsString();
				var lineCounter = Utils.GetSafeString(args, 1);
				var actualElems = Utils.GetSafeString(args, 2);
				var maxElems = Utils.GetSafeString(args, 3);
				var result = DisplayArrSetup(script, name, lineCounter, actualElems, maxElems);
				return result;
			}
			if (Name == Constants.DISPLAY_ARR_REFRESH)
			{
				List<Variable> args = script.GetFunctionArgs();
				Utils.CheckArgs(args.Count, 1, m_name);
				var name = args[0].AsString();
				DisplayArrRefresh(Gui, name);
				return Variable.EmptyInstance;
			}
			GetParameters(script);

			if (Name == Constants.MSG)
			{
				string caption = GetParameter("caption");
				int duration = GetIntParameter("duration");
				return new Variable(objectName);
			}
			if (Name == Constants.DEFINE)
			{
				Variable newVar = CreateVariable(script, objectName, GetVariableParameter("value"), GetVariableParameter("init"),
					GetParameter("type"), GetIntParameter("size"), GetIntParameter("dec"), GetIntParameter("array"),
					GetBoolParameter("local"), GetBoolParameter("up"), GetBoolParameter("down"), GetParameter("dup"), GetBoolParameter("reset"));
				return newVar;
			}
			if (Name == Constants.DISPLAY_ARRAY)
			{
				Variable newVar = DisplayArray(script, objectName, GetParameter("linecounter"), GetParameter("maxelements"),
					GetParameter("actualelements"), m_lastParameter);
				return newVar;
			}
			if (Name == Constants.DATA_GRID)
			{
				Variable newVar = DataGrid(script, objectName, GetBoolParameter("addrow"), GetBoolParameter("insertrow"),
					GetBoolParameter("deleterow"), m_lastParameter);
				return newVar;
			}
			if (Name == Constants.ADD_COLUMN)
			{
				AddGridColumn(script, objectName, GetParameter("header"), GetParameter("binding"));
				return new Variable(true);
			}
			if (Name == Constants.DELETE_COLUMN)
			{
				DeleteGridColumn(script, objectName, GetIntParameter("num"));
				return new Variable(true);
			}
			if (Name == Constants.SHIFT_COLUMN)
			{
				ShiftGridColumns(script, objectName, GetIntParameter("num"), GetIntParameter("to"));
				return new Variable(true);
			}
			if (Name == Constants.SET_OBJECT)
			{
				string prop = GetParameter("property");
				bool val = GetBoolParameter("value");
				return new Variable(objectName);
			}

			return new Variable(objectName);
		}

		void AdjustGridSelection(DataGrid dg, CSCS_GUI.WidgetData wd)
		{
			wd.lineCounter = Gui.Interpreter.GetVariableValue(wd.lineCounterName).AsInt();
			Application.Current.Dispatcher.Invoke(new Action(() =>
			{
				var rowList = dg.ItemsSource as ObservableCollection<FillOutGridFunction.Row>;
				if (rowList == null || wd.lineCounter < 0 || wd.lineCounter >= rowList.Count)
				{
					dg.SelectedIndex = -1;
					dg.SelectedItem = null;
					dg.UnselectAll();
					dg.UnselectAllCells();
				}
				else
				{
					dg.SelectedIndex = wd.lineCounter;
					object item = rowList[dg.SelectedIndex];
					dg.SelectedItem = item;
					dg.ScrollIntoView(item);
					DataGridRow row = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex);
					row.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
				}
			}));
		}

		void AdjustmaxElems(DataGrid dg, CSCS_GUI.WidgetData wd)
		{
			var maxElems = Gui.Interpreter.GetVariableValue(wd.maxElemsName).AsInt();
			Application.Current.Dispatcher.Invoke(new Action(() =>
			{
				var items = dg.ItemsSource as ObservableCollection<FillOutGridFunction.Row>;
				if (items == null)
				{
					return;
				}
				while (items.Count > maxElems && maxElems >= 0)
				{
					items.RemoveAt(items.Count - 1);
				}
				dg.Items.Refresh();
				dg.UpdateLayout();
			}));
		}

		int GetSelectedRow(DataGrid dg, CSCS_GUI.WidgetData wd)
		{
			Application.Current.Dispatcher.Invoke(new Action(() =>
			{
				dg.SelectionUnit = DataGridSelectionUnit.CellOrRowHeader;
				wd.lineCounter = dg.SelectedIndex;
				if (wd.lineCounter < 0)
				{
					var selItems = dg.SelectedCells;
					if (selItems != null && selItems.Count > 0)
					{
						var item = selItems[0];
						var obj = item.Item as FillOutGridFunction.Row;
						wd.lineCounter = obj.RowNumber;
					}
				}
				var items = dg.ItemsSource as ObservableCollection<FillOutGridFunction.Row>;
				wd.actualElems = items == null ? 0 : items.Count;
				wd.lineCounterVar = new Variable(wd.lineCounter);

			}));
			if (!string.IsNullOrWhiteSpace(wd.lineCounterName))
			{
				Gui.Interpreter.AddGlobal(wd.lineCounterName, new GetVarFunction(wd.lineCounterVar), false);
			}
			return wd.lineCounter;
		}

		public static DefineVariable GetDatagridData(CSCS_GUI gui, string name, out CSCS_GUI.WidgetData wd)
		{
			wd = null;
			if (!gui.DEFINES.TryGetValue(name, out DefineVariable gridVar))
			{
				return null;
			}
			if (gridVar.DefType != "datagrid")
			{
				throw new ArgumentException("Variable of wrong type: [" + gridVar.DefType + "]");
			}
			if (!gui.WIDGETS.TryGetValue(name, out wd))
			{
				throw new ArgumentException("Couldn't find widget data for widget: [" + name + "]");
			}
			return gridVar;
		}

		void DisplayArrRefresh(CSCS_GUI gui, string name)
		{
			var gridVar = GetDatagridData(gui, name, out CSCS_GUI.WidgetData wd);
			if (gridVar == null)
			{
				throw new ArgumentException("Couldn't find variable [" + name + "]");
			}
			DataGrid dg = gridVar.Object as DataGrid;
			AdjustmaxElems(dg, wd);
			AdjustGridSelection(dg, wd);
		}

		Variable DisplayArrSetup(ParsingScript script, string name, string lineCounterStr, string actualElemsStr, string maxElemsStr)
		{
			var gui = CSCS_GUI.GetInstance(script);
			var gridVar = GetDatagridData(gui, name, out CSCS_GUI.WidgetData wd);
			if (gridVar == null)
			{
				throw new ArgumentException("Couldn't find variable [" + name + "]");
			}
			lineCounterStr = lineCounterStr.ToLower();
			actualElemsStr = actualElemsStr.ToLower();

			DataGrid dg = gridVar.Object as DataGrid;
			wd.widget = dg;
			Variable result = Variable.EmptyInstance;

			if (Utils.CheckLegalName(lineCounterStr, script, false))
			{
				wd.lineCounterName = lineCounterStr;
				gui.DEFINES[lineCounterStr] = new DefineVariable(name, "lineCounter", gridVar.Object, true);
			}
			if (Utils.CheckLegalName(actualElemsStr, script, false))
			{
				wd.actualElemsName = actualElemsStr;
				wd.actualElemsVar = new Variable(wd.actualElems);
				Gui.Interpreter.AddGlobal(actualElemsStr, new GetVarFunction(wd.actualElemsVar), false);
				gui.DEFINES[actualElemsStr] = new DefineVariable(name, "actualElems", gridVar.Object, true);
			}
			if (Utils.CheckLegalName(maxElemsStr, script, false))
			{
				var maxElemsVar = Gui.Interpreter.GetVariableValue(maxElemsStr);
				wd.maxElems = maxElemsVar == null ? -1 : maxElemsVar.AsInt();
				wd.maxElemsName = maxElemsStr;
				wd.maxElemsVar = new Variable(wd.maxElems);
				Gui.Interpreter.AddGlobal(maxElemsStr, new GetVarFunction(wd.maxElemsVar), false);
				gui.DEFINES[maxElemsStr] = new DefineVariable(name, "maxElems", gridVar.Object, true);
			}
			AdjustmaxElems(dg, wd);
			GetSelectedRow(dg, wd);

			dg.SelectionChanged += (s, e) =>
			{
				int selected = GetSelectedRow(dg, wd);
				var funcName = name + "@Move";
				gui.RunScript(funcName, s as Window, new Variable(name), new Variable(dg.SelectedIndex));
			};
			dg.SelectedCellsChanged += (s, e) =>
			{
				int selected = GetSelectedRow(dg, wd);
			};
			dg.AddingNewItem += (s, e) =>
			{
				var max = Gui.Interpreter.GetVariableValue(maxElemsStr, script);
				if (max != null)
				{
				}
				if (dg.ItemsSource != null)
				{
					// dg.ItemsSource.re
				}
			};

			result = new Variable(wd.lineCounter);
			return result;
		}

		public static Variable CreateVariable(ParsingScript script, string name, Variable value, Variable init,
			string type = "", int size = 0, int dec = 3, int array = 0, bool local = false,
			bool up = false, bool down = false, string dup = null, bool reset = false)
		{
			var gui = CSCS_GUI.GetInstance(script);
			DefineVariable dupVar = null;
			if (!string.IsNullOrWhiteSpace(dup) && !gui.DEFINES.TryGetValue(dup, out dupVar))
			{
				throw new ArgumentException("Couldn't find variable [" + dup + "]");
			}

			var valueStr = value == null ? "" : value.AsString();
			init = init == null ? Variable.NewEmpty() : init;

			DefineVariable newVar = null;
			var parts = name.Split(new char[] { ',' });
			foreach (var objName in parts)
			{
				newVar = dupVar != null ? new DefineVariable(objName, dupVar, local) :
							  new DefineVariable(objName, valueStr, type, size, dec, array, local, up, down);
				newVar.InitVariable(dupVar != null ? dupVar.Init : init, gui, script);
				if (reset && script.ParentScript != null)
				{
					var guiParent = CSCS_GUI.GetInstance(script.ParentScript);
					if (guiParent.DEFINES.TryGetValue(objName.ToLower(), out DefineVariable parentVar))
					{
						init = parentVar;
						newVar.InitVariable(parentVar, gui, script);
					}
					MyAssignFunction.AddVariableMap(objName, script.ParentScript);
					var newV = CreateVariable(script.ParentScript, objName, value, init, type, size, dec, array, local, up, down, dup, false);
				}
			}
			return newVar;
		}

		DefineVariable DataGrid(ParsingScript script, string name, bool addrow, bool insertrow, bool deleterow, string action)
		{
			var gui = CSCS_GUI.GetInstance(script);
			var gridVar = GetDatagridData(gui, name, out CSCS_GUI.WidgetData wd);
			if (gridVar == null)
			{
				throw new ArgumentException("Couldn't find variable [" + name + "]");
			}

			DataGrid dg = gridVar.Object as DataGrid;
			wd.lineCounter = dg.SelectedIndex;
			var where = wd.lineCounter >= 0 ? wd.lineCounter : 0;
			var rowList = dg.ItemsSource as List<ExpandoObject>;

			if ((addrow || where >= rowList.Count) && !dg.IsReadOnly && gui.OnAddingRow(dg))
			{
				var expando = MyAssignFunction.GetNewRow(dg, wd);
				rowList.Add(expando);
			}
			else if (insertrow && !dg.IsReadOnly && gui.OnAddingRow(dg))
			{
				var expando = MyAssignFunction.GetNewRow(dg, wd);
				rowList.Insert(where, expando);
			}
			else if (deleterow && !dg.IsReadOnly)
			{
				rowList.RemoveAt(where);
			}

			if (action == "goedit")
			{
				dg.IsReadOnly = false;
			}
			else if (action == "gorowselect")
			{
				dg.IsReadOnly = true;
			}

			if ((insertrow || addrow) && !dg.IsReadOnly)
			{
				FillWidgetFunction.ResetArrays(gui, dg);
			}

			FillWidgetFunction.UpdateGridCounts(gui, dg, wd);
			FillWidgetFunction.UpdateGridSelection(dg, wd);
			return gridVar;
		}

		//DISPLAYARR ‘DataGridName’ LINECOUNTER cntr1 MAXELEMENTS cntr2 ACTUALELEMENTS cntr3 SETUP
		DefineVariable DisplayArray(ParsingScript script, string name, string
			lineCounter, string maxElems, string actualElems, string action)
		{
			var gui = CSCS_GUI.GetInstance(script);
			if (!gui.DEFINES.TryGetValue(name, out DefineVariable gridVar))
			{
				throw new ArgumentException("Couldn't find variable [" + name + "]");
			}
			if (gridVar.DefType != "datagrid")
			{
				throw new ArgumentException("Variable of wrong type: [" + gridVar.DefType + "]");
			}
			if (!gui.WIDGETS.TryGetValue(name, out CSCS_GUI.WidgetData wd))
			{
				throw new ArgumentException("Couldn't find widget data for widget: [" + name + "]");
			}

			DataGrid dg = gridVar.Object as DataGrid;
			gridVar.Active = action != "close" && (gridVar.Active || action == "setup");
			if (action == "close")
			{
				SetWidgetOptionsFunction.ClearWidget(gui, name, dg);
				return gridVar;
			}
			else if (action != "setup")
			{
				SetWidgetOptionsFunction.RedisplayWidget(dg, action);
				return gridVar;
			}

			var rowList = dg.ItemsSource as List<ExpandoObject>;
			FillWidgetFunction.ResetArrays(gui, dg);

			gui.DEFINES[lineCounter] = new DefineVariable(name, "lineCounter", gridVar.Object, true);
			gui.DEFINES[maxElems] = new DefineVariable(name, "maxElems", gridVar.Object, true);
			gui.DEFINES[actualElems] = new DefineVariable(name, "actualElems", gridVar.Object, true);

			var max = Gui.Interpreter.GetVariableValue(maxElems, script);
			int maxValue = max == null ? 0 : max.AsInt();

			if (Utils.CheckLegalName(lineCounter, script, false))
			{
				Gui.Interpreter.AddGlobal(lineCounter, new GetVarFunction(new Variable(dg.SelectedIndex)), false);
			}
			if (Utils.CheckLegalName(actualElems, script, false))
			{
				Gui.Interpreter.AddGlobal(actualElems, new GetVarFunction(new Variable(rowList == null ? 0 : rowList.Count)), false);
			}
			if (Utils.CheckLegalName(maxElems, script, false))
			{
				Gui.Interpreter.AddGlobal(maxElems, new GetVarFunction(new Variable(maxValue)), false);
			}

			dg.SelectionChanged += (s, e) =>
			{
				Gui.Interpreter.AddGlobal(lineCounter, new GetVarFunction(new Variable(dg.SelectedIndex)), false);
				var funcName = name + "@Move";
				if (dg.SelectedIndex >= 0)
				{
					wd.lineCounter = dg.SelectedIndex;
				}
				gui.RunScript(funcName, s as Window, new Variable(name), new Variable(dg.SelectedIndex));
			};

			dg.MouseDoubleClick += (s, e) =>
			{
				var funcName = name + "@Select";
				gui.RunScript(funcName, s as Window, new Variable(name), new Variable(dg.SelectedIndex));
			};

			dg.AddingNewItem += (s, e) =>
			{
				max = Gui.Interpreter.GetVariableValue(maxElems, script);
				if (max != null)
				{
				}
			};
			/*dg.CellEditEnding += (s, e) =>
			{
			    DataGridRow row = e.Row;
			    DataGridColumn col = e.Column;
			    int row_index = dg.ItemContainerGenerator.IndexFromContainer(row);
			    int col_index = col.DisplayIndex;

			    var item = e.Row.Item as ExpandoObject;
			    var newVal = (e.EditingElement as TextBox).Text;
			    if (item != null)
			    {
			        var p = item as IDictionary<String, object>;
			        for (int i = 0; i < dg.Columns.Count; i++)
			        {
				  var x = p[wd.headerNames[i]];
				  var z = 0;
			        }

			    }
			};*/

			dg.RowEditEnding += (s, e) =>
			{
			};

			wd.lineCounterName = lineCounter;
			wd.lineCounter = dg.SelectedIndex;
			wd.actualElemsName = actualElems;
			wd.actualElems = rowList == null ? 0 : rowList.Count;
			wd.maxElemsName = maxElems;
			wd.maxElems = maxValue;

			foreach (var headerName in wd.headerNames)
			{
				FillWidgetFunction.AddGridData(gui, name, headerName);
			}

			FillWidgetFunction.UpdateGridSelection(dg, wd);
			return gridVar;
		}

		static void AddGridColumn(ParsingScript script, string name, string header, string binding)
		{
			var gui = CSCS_GUI.GetInstance(script);
			if (!gui.DEFINES.TryGetValue(name, out DefineVariable gridVar))
			{
				throw new ArgumentException("Couldn't find variable [" + name + "]");
			}
			if (gridVar.DefType != "datagrid")
			{
				throw new ArgumentException("Variable of wrong type: [" + gridVar.DefType + "]");
			}
			if (!gui.WIDGETS.TryGetValue(name, out CSCS_GUI.WidgetData wd))
			{
				throw new ArgumentException("Couldn't find widget data for widget: [" + name + "]");
			}

			DataGrid dg = gridVar.Object as DataGrid;

			DataGridTextColumn textColumn = new DataGridTextColumn();
			textColumn.Header = header;
			textColumn.Binding = new Binding(binding);
			dg.Columns.Add(textColumn);

			wd.headerBindings.Add(binding);
			var headerStr = binding.ToLower();
			var headerVar = new DefineVariable(headerStr, "datagrid", dg, dg.Columns.Count - 1);
			headerVar.InitFromExisting(gui, headerStr);
			headerVar.Active = true;
			gui.DEFINES[headerStr] = headerVar;
			wd.headers[headerStr] = headerVar;
			wd.headerNames.Add(headerStr);
			wd.colTypes.Add(CSCS_GUI.WidgetData.COL_TYPE.STRING);
			gui.Interpreter.AddGlobal(headerStr, new GetVarFunction(headerVar), false);

			var rowList = dg.ItemsSource as List<ExpandoObject>;
			for (int i = 0; i < rowList.Count; i++)
			{
				Variable cellValue = headerVar.Tuple != null && headerVar.Tuple.Count > i ?
						 headerVar.Tuple[i] : Variable.EmptyInstance;
				MyAssignFunction.AddCell(gui, dg, i, dg.Columns.Count - 1, cellValue);
			}

			FillWidgetFunction.ResetArrays(gui, dg);
			FillWidgetFunction.UpdateGridSelection(dg, wd);
		}

		static void ShiftGridColumns(ParsingScript script, string name, int from, int to)
		{
			var gui = CSCS_GUI.GetInstance(script);
			if (!gui.DEFINES.TryGetValue(name, out DefineVariable gridVar))
			{
				throw new ArgumentException("Couldn't find variable [" + name + "]");
			}
			if (gridVar.DefType != "datagrid")
			{
				throw new ArgumentException("Variable of wrong type: [" + gridVar.DefType + "]");
			}
			if (!gui.WIDGETS.TryGetValue(name, out CSCS_GUI.WidgetData wd))
			{
				throw new ArgumentException("Couldn't find widget data for widget: [" + name + "]");
			}

			DataGrid dg = gridVar.Object as DataGrid;

			var cols = dg.Columns;
			cols[from].DisplayIndex = to;
			cols[to].DisplayIndex = from;

			var binding1 = wd.headerNames[from];
			var binding2 = wd.headerNames[to];
			var colType1 = wd.colTypes[from];
			var colType2 = wd.colTypes[to];

			wd.headerNames[from] = binding2;
			wd.headerNames[to] = binding1;
			wd.colTypes[from] = colType2;
			wd.colTypes[to] = colType1;

			var array1 = gui.Interpreter.GetVariableValue(binding1);
			var array2 = gui.Interpreter.GetVariableValue(binding2);
			var max = array1 == null || array1 == null || array1.Tuple == null || array2.Tuple == null ? 0 :
				Math.Min(array1.Tuple.Count, array2.Tuple.Count);

			/*for (int rowNb = 0; rowNb < max; rowNb++)
			{
			    var tmp = array1.Tuple[rowNb];
			    array1.Tuple[rowNb] = array2.Tuple[rowNb];
			    array2.Tuple[rowNb] = tmp;
			}*/

			dg.Items.Refresh();
			dg.UpdateLayout();
		}

		static void DeleteGridColumn(ParsingScript script, string name, int colId)
		{
			var gui = CSCS_GUI.GetInstance(script);
			if (!gui.DEFINES.TryGetValue(name, out DefineVariable gridVar))
			{
				throw new ArgumentException("Couldn't find variable [" + name + "]");
			}
			if (gridVar.DefType != "datagrid")
			{
				throw new ArgumentException("Variable of wrong type: [" + gridVar.DefType + "]");
			}
			if (!gui.WIDGETS.TryGetValue(name, out CSCS_GUI.WidgetData wd))
			{
				throw new ArgumentException("Couldn't find widget data for widget: [" + name + "]");
			}

			DataGrid dg = gridVar.Object as DataGrid;

			dg.Columns.RemoveAt(colId);
			var cols = dg.Columns;
			for (int i = colId; i < cols.Count - 1; i++)
			{
				wd.headerNames[i] = wd.headerNames[i + 1];
				wd.colTypes[i] = wd.colTypes[i + 1];
			}

			dg.Items.Refresh();
			dg.UpdateLayout();
		}
	}

	class ReturnStatement : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			if (script.ProcessReturn())
			{
				return Variable.EmptyInstance;
			}

			script.MoveForwardIf(Constants.SPACE);
			if (!script.FromPrev(Constants.RETURN.Length).Contains(Constants.RETURN))
			{
				script.Backward();
			}
			Variable result = Utils.GetItem(script);

			if (string.IsNullOrWhiteSpace(script.CurrentModule))
			{
				script.SetDone();
				result.IsReturn = true;
			}

			return result;
		}
	}

	class TestClass1Function : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			return new Variable(new TestClass1());
		}
	}

	class TestButtonFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 1, m_name);

			var widgetName = Utils.GetSafeString(args, 0);
			CSCS_GUI gui = script.Context as CSCS_GUI;
			var button = gui.GetWidget(widgetName) as Button;

			return new Variable(button);
		}
	}

	class PrintWindowFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);

            var fileName = Utils.GetSafeString(args, 0);
			var imagesPath = App.GetConfiguration("ImagesPath", "");
			Directory.CreateDirectory(Path.Combine(imagesPath, "screenshots"));
            var imagePath = Path.Combine(imagesPath, "screenshots", fileName);

			//var imagePath = Path.Combine(App.GetConfiguration("ImagesPath", ""), "tempScreenshot.jpg");
            screenShot(imagePath);

            printImage(imagePath);

			return Variable.EmptyInstance;
		}

		public bool screenShot(string saveAs)
        {
            try
            {
                //Get the Current instance of the window
                Window window = Application.Current.Windows.OfType<Window>().Single(x => x.IsActive);

                //Render the current control (window) with specified parameters of: Widht, Height, horizontal DPI of the bitmap, vertical DPI of the bitmap, The format of the bitmap
                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)window.ActualWidth, (int)window.ActualHeight, 96, 96, PixelFormats.Pbgra32);
                renderTargetBitmap.Render(window);

                //Encoding the rendered bitmap as desired (PNG,on my case because I wanted losless compression)
                PngBitmapEncoder png = new PngBitmapEncoder();
                png.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

                //Save the image on the desired location, on my case saveAs was C:\test.png
                using (Stream stm = File.Create(saveAs))
                {
                    png.Save(stm);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void printImage(string path)
        {
            string fileName = path;//pass in or whatever you need
            var p = new Process();
            p.StartInfo.FileName = fileName;
            p.StartInfo.Verb = "Print";
            //p.EnableRaisingEvents = true;
            //p.Exited += delegate
            //{
            //    try
            //    {
            //        File.Delete(path);
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //};
            p.Start();

            //var bi = new BitmapImage();
            //bi.BeginInit();
            //bi.CacheOption = BitmapCacheOption.OnLoad;
            //bi.UriSource = new Uri(path);
            //bi.EndInit();

            //var vis = new DrawingVisual();
            //using (var dc = vis.RenderOpen())
            //{
            //	dc.DrawImage(bi, new Rect { Width = bi.Width, Height = bi.Height });
            //}

            //var pdialog = new PrintDialog();
            //if (pdialog.ShowDialog() == true)
            //{
            //	pdialog.PrintVisual(vis, "My Image");
            //}
        }
    }
	
	class DownloadScriptsFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 0, m_name);

			string downloadScriptsString = App.GetConfiguration("DownloadScripts", "false");
			if (bool.TryParse(downloadScriptsString, out bool result))
			{
				return new Variable(result);
			}

			return new Variable(false);
		}
	}

	class ServerAddressFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 0, m_name);

			string serverAddressString = App.GetConfiguration("ServerAddress", "");

			return new Variable(serverAddressString);
		}
	}

	class RunScriptFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 1, m_name);

			var fileName = args[0].AsString();
			var encode = Utils.GetSafeInt(args, 1) > 0;
			CSCS_GUI gui = new CSCS_GUI(script);
			var result = gui.RunScript(fileName, encode);

			return result;
		}
	}

	class NewWindowFunction : ParserFunction
	{
		static string ns = "WpfCSCS.";

		static Dictionary<string, Window> s_windows = new Dictionary<string, Window>();
		static Dictionary<string, string> s_windowType = new Dictionary<string, string>();
		static Dictionary<string, string> s_typeWindow = new Dictionary<string, string>();

		static int s_currentWindow = -1;

		protected CSCS_GUI Gui;

		internal enum MODE { NEW, SHOW, HIDE, DELETE, NEXT, MODAL, SET_MAIN, UNSET_MAIN };
		MODE m_mode;

		internal NewWindowFunction(MODE mode = MODE.NEW)
		{
			m_mode = mode;
		}

		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Gui = CSCS_GUI.GetInstance(script);

			if (m_mode == MODE.NEXT)
			{
				HideAll();
				var windows = s_windows.Values.ToArray();
				if (windows.Length > 0)
				{
					if (++s_currentWindow > windows.Length - 1)
					{
						s_currentWindow = 0;
					}
					windows[s_currentWindow].Show();
				}
				return new Variable(s_currentWindow);
			}

			Utils.CheckArgs(args.Count, 1, m_name);
			string instanceName = args[0].AsString();
			// ../../scripts/Window4.xaml

			Window wind = null;
			if (m_mode == MODE.NEW || m_mode == MODE.MODAL)
			{
				var parentWin = Gui.ActiveWindow != null ? Gui.ActiveWindow : Gui.GetParentWindow(script);
				parentWin = parentWin != null ? parentWin : Gui.GetScriptWindow(script);

				var title = Utils.GetSafeString(args, 1);
				var winMode = m_mode == MODE.NEW ? SpecialWindow.MODE.NORMAL : //SpecialWindow.MODE.SPECIAL_MODAL;
					parentWin == CSCS_GUI.MainWindow ? SpecialWindow.MODE.MODAL : SpecialWindow.MODE.SPECIAL_MODAL;

				Variable result = Variable.EmptyInstance;
				Application.Current.Dispatcher.Invoke(new Action(() =>
				{
					SpecialWindow modalwin = CreateNew(instanceName, parentWin, winMode, script);
					//modalwin.Instance.Title = string.IsNullOrWhiteSpace(title) ? modalwin.Instance.Title : title;
					result = new Variable(modalwin.DialogResult);
				}));
				return result;

			}

			if (!s_windows.TryGetValue(instanceName, out wind))
			{
				if ((!s_windowType.TryGetValue(instanceName, out string windName) &&
					 !s_typeWindow.TryGetValue(instanceName, out windName)) ||
					!s_windows.TryGetValue(windName, out wind))
				{
					throw new ArgumentException("Couldn't find window [" + instanceName + "]");
				}
				instanceName = windName;
			}

			if (m_mode == MODE.HIDE)
			{
				wind.Hide();
			}
			else if (m_mode == MODE.SHOW)
			{
				wind.Show();
			}
			else if (m_mode == MODE.SET_MAIN || m_mode == MODE.UNSET_MAIN)
			{
				var special = SpecialWindow.GetInstance(wind);
				if (special != null)
				{
					special.IsMain = m_mode == MODE.SET_MAIN;
				}
			}
			else if (m_mode == MODE.DELETE)
			{
				wind.Close();
				RemoveWindow(wind, Gui);
			}

			return new Variable(instanceName);
		}

		public SpecialWindow CreateNew(string instanceName, Window parentWin = null,
			SpecialWindow.MODE winMode = SpecialWindow.MODE.NORMAL, ParsingScript script = null)
		{
			//var isMain = ChainFunction.CheckParentScriptIsMain(script);
			//winMode = isMain ? SpecialWindow.MODE.NORMAL : SpecialWindow.MODE.MODAL;
			SpecialWindow modalwin = new SpecialWindow(Gui, instanceName, winMode, parentWin);
			//winMode != SpecialWindow.MODE.NORMAL ? parentWin : null);
			var wind = modalwin.Instance;

			var tag = wind.Tag.ToString();
			s_windows[tag] = wind;
			s_windowType[instanceName] = tag;
			s_typeWindow[tag] = instanceName;
			s_currentWindow = 0;

			string cscsFilename = script == null ? "" : script.Filename;
			Gui.CacheWindow(wind, cscsFilename);
			Gui.CacheParentWindow(tag, parentWin);

			//wind.Show();

			if (winMode == SpecialWindow.MODE.MODAL || winMode == SpecialWindow.MODE.SPECIAL_MODAL)
			{
				modalwin.DialogResult = wind.ShowDialog();
			}
			else //NORMAL
			{
				if (parentWin == CSCS_GUI.MainWindow || parentWin == null)
				{
					wind.Show();
				}
				else
				{
					wind.ShowDialog();
				}
			}

			//if (parentWin == null /*|| isMain*/)
			//{
			//    wind.Show();
			//}
			//else
			//{
			//    //parentWin.Hide();
			//    wind.Hide();
			//    wind.ShowDialog();
			//}


			return modalwin;
		}

		public static void RemoveWindow(Window wind, CSCS_GUI Gui)
		{
			var tag = wind.Tag.ToString();
			s_windows.Remove(tag);
			if (s_typeWindow.TryGetValue(tag, out string instanceName))
			{
				s_typeWindow.Remove(tag);
				s_windowType.Remove(instanceName);
			}

			SpecialWindow.RemoveInstance(wind);
			Gui.UncacheWindow(wind, tag);
		}

		static void HideAll()
		{
			foreach (var item in s_windows)
			{
				item.Value.Hide();
			}
		}
		static void ShowAll()
		{
			foreach (var item in s_windows)
			{
				item.Value.Show();
			}
		}

		static object GetInstance(string strFullyQualifiedName)
		{
			Type t = Type.GetType(strFullyQualifiedName);
			if (t == null)
			{
				return null;
			}
			return Activator.CreateInstance(t);
		}
	}

	class FillWidgetFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 2, m_name);
			var widgetName = Utils.GetSafeString(args, 0);
			var varName = Utils.GetSafeString(args, 1);
			var gui = CSCS_GUI.GetInstance(script);

			int count = AddGridData(gui, widgetName, varName);
			return new Variable(count);
		}

		public static int AddGridData(CSCS_GUI gui, string widgetName, string headerName)
		{
			if (!gui.WIDGETS.TryGetValue(widgetName, out CSCS_GUI.WidgetData wd) ||
				!(wd.widget is DataGrid))
			{
				return 0;
			}
			DataGrid dg = wd.widget as DataGrid;

			var data = gui.Interpreter.GetVariableValue(headerName);
			if (!gui.DEFINES.TryGetValue(headerName, out DefineVariable defVar) || data == null)
			{
				return 0;
			}

			if (gui.DEFINES.TryGetValue(dg.DataContext as string, out DefineVariable headerDef) &&
				dg.Columns.Count > defVar.Index && headerDef.Tuple.Count > defVar.Index)
			{
				var textCol = dg.Columns[defVar.Index] as DataGridTextColumn;
				textCol.Header = headerDef.Tuple[defVar.Index].AsString();
			}

			var entries = data.Tuple;
			defVar.Active = true;
			int count = wd.maxElems <= 0 ? entries.Count : Math.Min(entries.Count, wd.maxElems);
			for (int i = 0; i < count; i++)
			{
				MyAssignFunction.AddCell(gui, dg, i, defVar.Index, entries[i]);
			}

			UpdateGridCounts(gui, dg, wd);

			wd.headers[headerName] = data;
			dg.Items.Refresh();
			dg.UpdateLayout();

			return count;
		}

		public static void UpdateGridSelection(DataGrid dg, CSCS_GUI.WidgetData wd)
		{
			if (dg.Items == null)
			{
				return;
			}
			dg.Items.Refresh();
			if (dg.SelectedIndex < 0 && wd.lineCounter >= 0)
			{
				dg.SelectedIndex = wd.lineCounter;
			}
			dg.UpdateLayout();
		}

		public static void UpdateGridCounts(CSCS_GUI gui, DataGrid dg, CSCS_GUI.WidgetData wd = null)
		{
			if (wd == null && !gui.WIDGETS.TryGetValue(dg.DataContext as string, out wd))
			{
				return;
			}

			//dg.Items.Refresh();
			var rowList = dg.ItemsSource as List<ExpandoObject>;
			if (gui.DEFINES.TryGetValue(wd.actualElemsName, out DefineVariable actualElems))
			{
				actualElems.Value = wd.actualElems = rowList.Count;// dg.Items.Count;
				gui.Interpreter.AddGlobal(wd.actualElemsName, new GetVarFunction(actualElems), false);
				if (gui.DEFINES.TryGetValue(wd.lineCounterName, out DefineVariable lineCounter) &&
					dg.SelectedIndex < 0)
				{
					if (wd.lineCounter < 0)
					{
						wd.lineCounter = 0;
					}
					lineCounter.Value = wd.lineCounter;
					dg.SelectedIndex = wd.lineCounter;
					gui.Interpreter.AddGlobal(wd.lineCounterName, new GetVarFunction(lineCounter), false);
				}
			}
		}

		static int Compare(double num1, double num2)
		{
			return num1 < num2 ? -1 : num2 > num1 ? 1 : 0;
		}

		public static void ResetArrays(CSCS_GUI gui, FrameworkElement widget, IEnumerable<ExpandoObject> newCollection = null)
		{
			DataGrid dg = widget as DataGrid;
			if (dg == null)
			{
				return;
			}
			var name = dg.DataContext as string;
			if (string.IsNullOrWhiteSpace(name) ||
				!gui.WIDGETS.TryGetValue(name, out CSCS_GUI.WidgetData wd))
			{
				return;
			}

			var rowList = dg.ItemsSource as List<ExpandoObject>;
			if (rowList == null)
			{
				return;
			}

			bool sorting = newCollection != null;
			if (!sorting)
			{
				newCollection = rowList;
			}
			var correctList = newCollection.ToList();

			if (sorting)
			{
				rowList.Clear();
			}
			else
			{
				dg.ItemsSource = null;
			}

			for (int rowNb = 0; rowNb < correctList.Count; rowNb++)
			{
				var row = correctList[rowNb] as IDictionary<String, object>; ;
				for (int colNb = 0; colNb < dg.Columns.Count; colNb++)
				{
					var colStr = wd.headerNames[colNb];
					var v = row[colStr];
					Variable cellValue = wd.colTypes[colNb] == CSCS_GUI.WidgetData.COL_TYPE.STRING ||
						v is string ? new Variable(v.ToString()) : new Variable((double)v);

					var headerData = gui.Interpreter.GetVariableValue(colStr);
					headerData.SetAsArray();
					while (headerData.Tuple.Count <= rowNb)
					{
						headerData.Tuple.Add(Variable.EmptyInstance);
					}
					headerData.Tuple[rowNb] = cellValue;
					MyAssignFunction.AddCell(gui, dg, rowNb, colNb, cellValue);
				}
			}
			//dg.Items.Refresh();
		}
	}

	internal class CheckVATFunction : ParserFunction
	{
		internal enum MODE { CHECK, NAME, ADDRESS };

		static string s_request = @"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" 
                                                      xmlns:urn=""urn:ec.europa.eu:taxud:vies:services:checkVat:types"">
                          <soapenv:Header/><soapenv:Body><urn:checkVat>
                            <urn:countryCode>COUNTRY</urn:countryCode>
                            <urn:vatNumber>VATNUMBER</urn:vatNumber>
                          </urn:checkVat></soapenv:Body></soapenv:Envelope>";
		static Dictionary<string, string> s_cache = new Dictionary<string, string>();
		MODE m_mode;

		internal CheckVATFunction(MODE mode = MODE.CHECK)
		{
			m_mode = mode;
		}

		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 1, m_name);

			string vat = args[0].AsString(); // 26389058739
			string country = Utils.GetSafeString(args, 1, "HR");
			/*string callBack = Utils.GetSafeString(args, 2);

			CustomFunction callbackFunction = gui.Interpreter.GetFunction(callBack, null) as CustomFunction;
			if (callbackFunction == null)
			{
			    throw new ArgumentException("Error: Couldn't find function [" + callBack + "]");
			}*/

			CacheVAT(vat, country);
			switch (m_mode)
			{
				case MODE.CHECK:
					return new Variable(s_cache[vat + "valid"] == "true");
				case MODE.ADDRESS:
					return new Variable(s_cache[vat + "address"]);
				case MODE.NAME:
					return new Variable(s_cache[vat + "name"]);
			}

			return Variable.EmptyInstance; ;
		}

		static string ExtractTag(string xml, string tag)
		{
			var start = xml.IndexOf("<" + tag + ">");
			if (start < 0)
			{
				return "";
			}
			var wordStart = start + tag.Length + 2;
			var end = xml.IndexOf("</" + tag + ">", wordStart);
			if (end < 0)
			{
				return "";
			}
			var result = xml.Substring(wordStart, end - wordStart);
			return result;
		}

		internal static void CacheVAT(string vat, string country)
		{
			if (s_cache.TryGetValue(vat + "name", out string name) && !string.IsNullOrEmpty(name))
			{
				return;
			}

			s_cache[vat + "valid"] = "false";
			s_cache[vat + "name"] = "";
			s_cache[vat + "address"] = "";

			var wc = new WebClient();
			var request = s_request.Replace("COUNTRY", country).Replace("VATNUMBER", vat);

			string response = "";
			try
			{
				response = wc.UploadString("http://ec.europa.eu/taxation_customs/vies/services/checkVatService", request);
			}
			catch (Exception exc)
			{
				s_cache[vat + "name"] = exc.Message;
				return;
			}

			//<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/"><soap:Body><checkVatResponse xmlns="urn:ec.europa.eu:taxud:vies:services:checkVat:types"><countryCode>HR</countryCode><vatNumber>26389058739</vatNumber><requestDate>2020-05-01+02:00</requestDate><valid>true</valid>
			//<name>AURA SOFT D.O.O.</name><address>KAPETANA LAZARIÄ†A 1 D, PAZIN, 52000 PAZIN</address></checkVatResponse></soap:Body></soap:Envelope>
			var validTag = ExtractTag(response, "valid");
			if (validTag == "true")
			{
				s_cache[vat + "valid"] = validTag;
				s_cache[vat + "name"] = ExtractTag(response, "name");
				s_cache[vat + "address"] = ExtractTag(response, "address");
			}
		}
	}

	internal class GetGridRowCountFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 1, m_name);

			var gui = CSCS_GUI.GetInstance(script);
			var widgetName = Utils.GetSafeString(args, 0);
			var widget = gui.GetWidget(widgetName);
			if (widget == null)
			{
				return Variable.EmptyInstance;
			}

			if (widget is DataGrid)
			{
				var dg = widget as DataGrid;
				var rowCount = (double)dg.Items.Count;
				return new Variable(rowCount);
			}

			return Variable.EmptyInstance;
		}
	}

	internal class FillBufferFromGridRowFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 2, m_name);

			var gui = CSCS_GUI.GetInstance(script);
			var widgetName = Utils.GetSafeString(args, 0).ToLower();
			var rowIndex = Utils.GetSafeInt(args, 1);
			var widget = gui.GetWidget(widgetName);
			if (widget == null)
			{
				return Variable.EmptyInstance;
			}

			if (widget is DataGrid)
			{
				var dg = widget as DataGrid;

				for (int i = 0; i < NewBindSQLFunction.gridsTags[widgetName].Count; i++)
				{
					var defName = NewBindSQLFunction.gridsTags[widgetName][i];
					var rowColumnVarObject = ((IDictionary<String, Object>)dg.Items[rowIndex])[defName];
					var varType = NewBindSQLFunction.gridsTypes[widgetName][i];
					gui.DEFINES[defName.ToLower()].Type = varType;
					//CSCS_GUI.DEFINES[defName.ToLower()].InitVariable(new Variable(varType));
					switch (varType)
					{
						case Variable.VarType.NONE:
							break;
						case Variable.VarType.UNDEFINED:
							break;
						case Variable.VarType.NUMBER:
							gui.DEFINES[defName.ToLower()].InitVariable(new Variable(double.Parse((string)rowColumnVarObject)), gui);
							break;
						case Variable.VarType.STRING:
							gui.DEFINES[defName.ToLower()].InitVariable(new Variable((string)rowColumnVarObject), gui);
							break;
						case Variable.VarType.ARRAY:
							break;
						case Variable.VarType.ARRAY_NUM:
							break;
						case Variable.VarType.ARRAY_STR:
							break;
						case Variable.VarType.ARRAY_INT:
							break;
						case Variable.VarType.INT:
							break;
						case Variable.VarType.MAP_INT:
							break;
						case Variable.VarType.MAP_NUM:
							break;
						case Variable.VarType.MAP_STR:
							break;
						case Variable.VarType.BYTE_ARRAY:
							break;
						case Variable.VarType.QUIT:
							break;
						case Variable.VarType.BREAK:
							break;
						case Variable.VarType.CONTINUE:
							break;
						case Variable.VarType.OBJECT:
							gui.DEFINES[defName.ToLower()].InitVariable(new Variable(rowColumnVarObject), gui);
							break;
						case Variable.VarType.ENUM:
							break;
						case Variable.VarType.VARIABLE:
							break;
						case Variable.VarType.DATETIME:
							//CSCS_GUI.DEFINES[defName.ToLower()].InitVariable(new Variable(varType));
							gui.DEFINES[defName.ToLower()].DateTime = DateTime.ParseExact((string)rowColumnVarObject, "dd/MM/yyyy", CultureInfo.InvariantCulture);
							//CSCS_GUI.DEFINES[defName.ToLower()].InitVariable(new Variable(DateTime.ParseExact((string)rowColumnVarObject, "dd/MM/yyyy", CultureInfo.InvariantCulture)));
							break;
						case Variable.VarType.CUSTOM:
							break;
						case Variable.VarType.POINTER:
							break;
						default:
							break;
					}
					//CSCS_GUI.DEFINES[defName.ToLower()].InitVariable(new Variable(rowColumnVarObject));

				}
			}

			return Variable.EmptyInstance;
		}
	}

	public class DefineVariable : Variable
	{
		string DATE_FORMAT
		{
			get;
			set;
		} = " ";
		string TIME_FORMAT
		{
			get;
			set;
		} = "";

		public string Name { get; set; }
		public string DefValue { get; set; }
		public string DefType { get; set; } = "";

		public int Size { get; set; } = 0;
		public int Current { get; set; } = 0;
		public int MaxElements { get; set; } = -1;
		public int Dec { get; set; } = 0;
		public int Array { get; set; } = 0;
		public bool Local { get; set; } = false;
		public bool Up { get; set; } = false;
		public bool Down { get; set; } = false;
		public bool Active { get; set; } = false;
		public DefineVariable Dup { get; set; }
		public Variable Init { get; set; }
		public List<int> Indices { get; set; } = new List<int>();

        public string AssignedString { get; set; }
		public double AssignedNumber { get; set; }

		public bool LocalAssign { get; set; }

		public static new readonly DefineVariable EmptyInstance = new DefineVariable();

		public override Variable Default()
		{
			return EmptyInstance;
		}

		public override double Value
		{
			get
			{
				if (!string.IsNullOrEmpty(DefValue))
				{
					m_value = CSCS_GUI.LastInstance.ProcessScript(DefValue).AsDouble();
				}
				m_value = Math.Round(m_value, Dec);
				return m_value;
			}
			set
			{
				if (!string.IsNullOrEmpty(DefValue))
				{
					return;
				}
				if (!LocalAssign)
				{
					Size = 0;
				}
				AssignedNumber = value;
				m_value = value;
				Type = VarType.NUMBER;
			}
		}

		public override string String
		{
			get
			{
				if (!string.IsNullOrEmpty(DefValue))
				{
					m_string = CSCS_GUI.LastInstance.ProcessScript(DefValue).AsString();
				}
				return m_string;
			}
			set
			{
				if (!string.IsNullOrEmpty(DefValue))
				{
					return;
				}
				AssignedString = value;
				//// ???????????
				//if (!LocalAssign)
				//{
				//	Size = 0;
				//}
				m_string = value;
				Type = VarType.STRING;
			}
		}

		public DefineVariable()
		{
		}

		public DefineVariable(DefineVariable other)
		{
			Name = other.Name;
			Tuple = other.Tuple;
			DefType = other.DefType;
			Index = other.Index;
			Object = other.Object;
			Active = other.Active;
			Value = other.Value;
			String = other.String;
			DateTime = other.DateTime;
			Type = other.Type;
		}

		public DefineVariable(string name, string type, Object obj, int index = -1)
		{
			Name = name.ToLower();
			Tuple = new List<Variable>();
			DefType = type.ToLower();
			Index = index;
			Object = obj;
			Type = VarType.ARRAY;
			Active = false;
		}

		public DefineVariable(string name, string type, Object obj, bool active)
		{
			Name = name.ToLower();
			DefType = type.ToLower();
			Object = obj;
			Active = active;
		}

		public DefineVariable(string name, string value,
			string type = "", int size = 0, int dec = 3, int array = 0, bool local = false, bool up = false, bool down = false)
		{
			Name = name.ToLower();
			DefValue = value;
			DefType = type.ToLower();
			Size = size;
			Dec = dec;
			Local = local;
			Up = up;
			Down = down;
			Array = array;
			Active = true;
		}

		public DefineVariable(string name, DefineVariable dup, bool local = false)
		{
			Name = name.ToLower();
			Local = local;
			DefValue = dup.DefValue;
			DefType = dup.DefType.ToLower();
			Size = dup.Size;
			Dec = dup.Dec;
			Up = dup.Up;
			Down = dup.Down;
			Array = dup.Array;
			Dup = dup;
			Active = dup.Active;
		}

		public DefineVariable(List<Variable> a)
		{
			Tuple = a;
		}

		static double CheckValue(string type, int size, int dec, Variable varValue)
		{
			double val = Math.Round(varValue.AsDouble(), dec);
			//if (varValue.Type == VarType.NONE || varValue.Type == VarType.ARRAY || string.IsNullOrWhiteSpace(varValue.String))
			if (varValue.Type == VarType.NONE)
			{
				return val;
			}
			switch (type)
			{
				case "b":
					if (val < Byte.MinValue || val > Byte.MaxValue)
					{
						throw new ArgumentException("Error: Incorrect value [" + varValue.String + "], required [" + type + "]");
					}
					break;
				case "i":
					if (val < short.MinValue || val > short.MaxValue)
					{
						throw new ArgumentException("Error: Incorrect value [" + varValue.String + "], required [" + type + "]");
					}
					break;
				case "r":
					if (val < Int32.MinValue || val > Int32.MaxValue)
					{
						throw new ArgumentException("Error: Incorrect value [" + varValue.String + "], required [" + type + "]");
					}
					break;
			}
			if (size > 0)
			{
				var strValue = val.ToString();
				if (dec > 0)
				{ // add missing 0s after decimal point.
					var decPt = strValue.Replace(",", NumberFormatInfo.CurrentInfo.NumberDecimalSeparator).IndexOf(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
					if (decPt < 0)
					{
						strValue += NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + new string('0', dec);
					}
					else
					{ // 1.2
						var present = strValue.Length - decPt - 1;
						if (present < dec)
						{
							strValue += new string('0', dec - present);
						}
					}
				}
				if (strValue.Length > size || (dec > 0 && !Double.TryParse(strValue, out val)))
				{
					/* old code:
					bool isNeg = strValue.StartsWith("-"); // -12.346 --> -2.35 (for size 5, dec 2)
					var newVal = isNeg ? strValue.Substring(strValue.Length - size + 1, size - 1) :
                                         strValue.Substring(strValue.Length - size, size);
					if (newVal.StartsWith("."))
					{
                        newVal = newVal.Substring(1, newVal.Length - 1);
                    }
                    newVal = isNeg ? "-" + newVal : newVal;
                    if (!Double.TryParse(newVal, out val))
					{
                        throw new ArgumentException("Error: Couldn't convert [" + strValue + "], to [" + type + "]");
                    }*/
					val = 0;
				}
			}
			if ((varValue.Type != VarType.NUMBER && varValue.Type != VarType.ARRAY) &&
				(type == "n" || type == "b" || type == "i" || type == "r") &&
				!Double.TryParse(varValue.String, out val))
			{
				throw new ArgumentException("Error: Variable type [" + varValue.Type + "], required [" + type + "]");
			}

			return val;
		}

		public void AssignArrayValue(Variable val)
		{
            if (Indices.Count == 1)
            {
                Tuple[Indices[0]] = val;
            }
            else if (Indices.Count == 2)
            {
                Tuple[Indices[0]].Tuple[Indices[1]] = val;
            }
            else if (Indices.Count == 3)
            {
                Tuple[Indices[0]].Tuple[Indices[1]].Tuple[Indices[2]] = val;
            }
            else
            {
                Tuple[Indices[0]].Tuple[Indices[1]].Tuple[Indices[2]].Tuple[Indices[3]] = val;
            }
        }
        public void InitVariable(Variable init, CSCS_GUI gui, ParsingScript script = null, bool update = true, int arrayIndex = -1)
		{
			/*
	        I  - signed small int (2 bytes), from -32,768 to 32.767
	        R  - signed int (4 bytes), from -2,147,483,648 to 2,147,483,647
	        B – unsigned tinyInt (1 byte), from 0 to 255
	        N – standard FLOAT type (8 bytes)
	        Sign 	Max Value 	Minimum Value 
	        Negative	– 1.79E+308	-2.23E-308
	        Positive	1.79E+308	2.23E-308

	        When using maths, internal precision is max possible for the type. SIZE parameter in DEFINE means – ‘display’ size, that can be returned to the GUI or report.

	        L – logic/boolean (1 byte), internaly represented as 0 or 1, as constant as true  false  .true.  .false.  .t.  .f.  ‘Y’  ‘N’
			 * */
			Init = init;
			LocalAssign = true;
            if (Indices.Count > 0 && Tuple != null && Tuple.Count > Math.Max(Indices[0], Array-1))
            {
                Variable tmp = Tuple[Indices[0]];
                for (int i = 1; i < Indices.Count && tmp.Tuple != null && tmp.Tuple.Count > Indices[i]; i++)
                {
                    tmp = tmp.Tuple[Indices[i]];
                }
                tmp = init;
				AssignArrayValue(tmp);
                return;
            }
            switch (DefType)
			{
				case "a":
					String = init.AsString();
					Type = VarType.STRING;
					if (Size > 0 && m_string.Length > Size)
					{
						m_string = m_string.Substring(0, Size);
					}
					if (Up)
					{
						m_string = m_string.ToUpper();
					}
					if (Down)
					{
						m_string = m_string.ToLower();
					}
					if (init.Type != VarType.NONE)
					{
						init.String = m_string;
					}

					break;

				case "p":
				case "f":
					Pointer = init.AsString();
					Type = VarType.POINTER;
					if (init.Type != VarType.NONE)
					{
						init.Type = Type;
						init.Pointer = Pointer;
					}
					break;
				case "d":
				case "t":
					DateTime = ToDateTime(init);
					Format = !string.IsNullOrWhiteSpace(Format) ? Format :
						DefType == "d" ? GetDateFormat() : GetTimeFormat();
					Type = VarType.DATETIME;
					if (init.Type != VarType.NONE)
					{
						init.DateTime = DateTime;
						init.Type = Type;
						init.Format = Format;
					}
					break;
				case "l": // "logic" (boolean)
					Value = ToBool(init.AsString()) ? 1 : 0;
					Type = VarType.NUMBER;
					if (init.Type != VarType.NONE)
					{
						init.Value = Value;
						init.Type = Type;
					}
					break;
				case "b": // byte
				case "i": // integer
				case "n": // number
				case "r": // small int
				default:
					Value = CheckValue(DefType, Size, Dec, init);
					Type = VarType.NUMBER;
					if (init.Type != VarType.NONE)
					{
						init.Value = Value;
						init.Type = Type;
					}
					break;
            }
            if (Array > 0)
			{
				var maxElems = Math.Max(Array, arrayIndex);
				var missingElems = Tuple == null || arrayIndex < 0 ? maxElems : maxElems - Tuple.Count;
				DefineVariable item = missingElems > 0 ?
						  this.DeepClone() as DefineVariable : null;
				if (Tuple == null || arrayIndex < 0)
				{
					Tuple = new List<Variable>(Array);
				}
				if (missingElems > 0)
				{
					item.Array = 0;
					Tuple.AddRange(System.Linq.Enumerable.Repeat(item, missingElems));
				}
				Type = VarType.ARRAY;
				if (arrayIndex >= 0)
				{
					Tuple[arrayIndex] = init.Clone();
				}
				if (init.Type == VarType.NONE)
				{
					init.Type = VarType.ARRAY;
					init.Tuple = new List<Variable>(Array);
					for (int i = 0; i < Tuple.Count; i++)
					{
						init.Tuple.Add(Tuple[i]);
					}
				}
			}

			CSCS_GUI.ChangingBoundVariable = true;
			if (update)
			{
				if (Local)
				{
					gui.Interpreter.AddLocalVariable(new GetVarFunction(this), Name);
				}
				else
				{
					gui.Interpreter.AddGlobalOrLocalVariable(Name, new GetVarFunction(this), script);
				}
				//InitFromExisting(gui, Name);
				gui.DEFINES[Name] = this;
			}
			CSCS_GUI.ChangingBoundVariable = false;

			LocalAssign = false;
		}

		public void InitFromExisting(CSCS_GUI gui, string name)
		{
			if (!gui.DEFINES.TryGetValue(name, out DefineVariable existing))
			{
				var existingVar = gui.Interpreter.GetVariableValue(name);
				if (existingVar != null && existingVar.Tuple != null)
				{
					this.Tuple = existingVar.Tuple;
				}
				return;
			}
			if (existing.Object != null)
			{
				this.Object = existing.Object;
				this.DefType = existing.DefType;
				this.Index = existing.Index;
			}
			if (existing.Tuple != null)
			{
				this.Tuple = existing.Tuple;
				this.Size = existing.Size;
			}
		}

		public string GetDateFormat()
		{
			if (!string.IsNullOrWhiteSpace(Format))
			{
				return Format;
			}
			if (!string.IsNullOrWhiteSpace(DATE_FORMAT))
			{
				return DATE_FORMAT;
			}
			// disable US date formatting for now
			string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
			var usDate = false;// !sysFormat.StartsWith("dd") && !sysFormat.EndsWith("dd");
			DATE_FORMAT = CSCS_GUI.DefaultDateFormat;
			switch (Size)
			{
				case 5:
					DATE_FORMAT = usDate ? "MM/dd" : "dd/MM";
					break;
				case 7:
					DATE_FORMAT = usDate ? "MM/yyyy" : "MM/yyyy";
					break;
				case 8:
					DATE_FORMAT = usDate ? "MM/dd/yy" : "dd/MM/yy";
					break;
				case 10:
					DATE_FORMAT = usDate ? "MM/dd/yyyy" : "dd/MM/yyyy";
					break;
			}
			return DATE_FORMAT;
		}

		public string GetTimeFormat()
		{
			if (!string.IsNullOrWhiteSpace(TIME_FORMAT))
			{
				return TIME_FORMAT;
			}
			TIME_FORMAT = "HH:mm:ss";
			switch (Size)
			{
				case 3:
					TIME_FORMAT = "fff";
					break;
				case 5:
					TIME_FORMAT = "HH:mm";
					break;
				case 6:
					TIME_FORMAT = "ss.fff";
					break;
				case 8:
					TIME_FORMAT = "HH:mm:ss";
					break;
				case 11:
					TIME_FORMAT = "HH:mm:ss.ff";
					break;
				case 12:
					TIME_FORMAT = "HH:mm:ss.fff";
					break;
			}
			return TIME_FORMAT;
		}

		public bool ToBool(string strValue)
		{
			char ch = string.IsNullOrWhiteSpace(strValue) ? '0' : strValue.ToLower()[0];
			return ch == 't' || ch == 'y' || ch == '1';
		}

		public DateTime ToDateTime(Variable val)
		{
			DateTime oldest = DateTime.MinValue;
			DateTime dt = oldest;
			if (val.Type == VarType.NONE || (val.Type == VarType.NUMBER && val.Value == 0.0))
			{
				return dt;
			}
			if (val.Type == VarType.NUMBER && val.Value > 0.0)
			{ // Number of days since 01/01/1900
				var baseDate = new DateTime(1900, 1, 1);
				dt = baseDate.AddDays((int)val.Value);
				var hours = (int)((val.Value - (int)val.Value) * 24.0);
				dt = dt.AddHours(hours);
				return dt;
			}

			if ((DefType == "d" || DefType == "t") && val.Type == VarType.ARRAY &&
				val.Tuple != null && val.Tuple.Count > 0)
			{
				return val.Tuple[0].DateTime;
			}

			var strValue = val.AsString();
			var format = DefType == "d" ? (strValue.Length == 8 ? CSCS_GUI.DateFormat8 :
										  strValue.Length == 10 ? CSCS_GUI.DateFormat10 :
										  GetDateFormat()) : GetTimeFormat();
			var theValue = val.Type == VarType.DATETIME ? val.DateTime.ToString(format) : strValue;
			if (DefType == "d")
			{
				if (!string.IsNullOrWhiteSpace(theValue) &&
					!DateTime.TryParseExact(theValue, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
				{
					if (theValue.Length >= 10 && format.Length < 10 && format.Length >= 8)
					{
						var shortenedDate = theValue.Substring(0, 6) + theValue.Substring(8, 2);

						if (!DateTime.TryParseExact(shortenedDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
						{
							throw new ArgumentException("Error: Couldn't parse [" + theValue + "] with format [" + format + "]");
						}
					}
					else
						throw new ArgumentException("Error: Couldn't parse [" + theValue + "] with format [" + format + "]");
				}
				else if (dt.CompareTo(oldest) < 0)
				{
					MessageBox.Show("Date range is out of limit: " + dt.ToString(format) + "\nDate is set to 0");
					dt = oldest;
				}
			}
			if (DefType == "t")
			{
				if (theValue.Length >= 8 && format.Length < 8 && format.Length >= 5)
				{
					theValue = theValue.Substring(0, 5);
				}
				if (!string.IsNullOrWhiteSpace(theValue) &&
					!DateTime.TryParseExact(theValue, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
				{
					throw new ArgumentException("Error: Couldn't parse [" + theValue + "] with format [" + format + "]");
				}
				dt = dt.Subtract(new TimeSpan(dt.Date.Ticks));
			}

			return dt;
		}

		public override DateTime AsDateTime()
		{
			return m_datetime;
		}

		public override string AsString(bool isList = true,
				   bool sameLine = true,
				   int maxCount = -1)
		{
			if (Array > 0)
			{
				return BaseAsString(isList, sameLine, maxCount);
			}
			if (DefType == "d")
			{
				return DateTime.ToString(GetDateFormat());
			}
			if (DefType == "t")
			{
				return DateTime.ToString(GetTimeFormat());
			}
			if (DefType == "l")
			{
				return AsDouble() == 0 ? "false" : "true";
			}
			if (Size > 0 && (DefType == "n" || DefType == "i" || DefType == "b" || DefType == "r"))
			{
				var strValue = Value.ToString();
				if (strValue.Length > Size)
				{
					return CheckValue(DefType, Size, Dec, this).ToString();
				}
				return strValue;
			}
			if (Size > 0 && DefType == "a" && m_string.Length > Size)
			{
				m_string = m_string.Substring(0, Size);
				return m_string;
			}
			return BaseAsString(isList, sameLine, maxCount);
		}

		string BaseAsString(bool isList = true,
				   bool sameLine = true,
				   int maxCount = -1)
		{
			string result = "";
			try
			{
				Application.Current.Dispatcher.Invoke(new Action(() =>
				{
					result = base.AsString(isList, sameLine, maxCount);
				}));
				return result;
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc);
				return result;
			}
		}

		public override double AsDouble()
		{
			return base.AsDouble();
		}

		public override bool Preprocess()
		{
			/*if (DefType == "datagrid" && Type == VarType.ARRAY)
			{
			    var dg = Object as DataGrid;
			    if (dg != null && dg.DataContext is string &&
			        CSCS_GUI.WIDGETS.TryGetValue(dg.DataContext as string, out CSCS_GUI.WidgetData wd) &&
			        wd.needsReset && wd.headers.ContainsKey(Name))
			    {
			        FillWidgetFunction.ResetArrays(dg);
			        wd.needsReset = false;
			    }
			}*/
			return base.Preprocess();
		}

		public override void AddToDate(Variable valueB, int sign)
		{
			if (valueB.Type == Variable.VarType.NUMBER)
			{
				var dt = AsDateTime();
				var delta = valueB.Value * sign;
				if (DefType == "t")
				{
					DateTime = dt.AddSeconds(delta);
				}
				else
				{
					DateTime = dt.AddDays(delta);
				}
			}
			else
			{
				base.AddToDate(valueB, sign);
			}
		}
	}

	class MyPointerFunction : PointerFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<string> args = Utils.GetTokens(script);
			Utils.CheckArgs(args.Count, 1, m_name);

			var result = PointerAssign(script, m_name, args[0]);
			if (result == null)
			{
				return base.Evaluate(script);
			}
			return result;
		}
		public static Variable PointerAssign(ParsingScript script, string name, string pointerVal)
		{
			var gui = CSCS_GUI.GetInstance(script);

			DefineVariable defVar;
			if (!gui.DEFINES.TryGetValue(name, out defVar))
			{
				return null;
			}

			defVar.Pointer = pointerVal;
			var existing = gui.Interpreter.GetVariableValue(defVar.Pointer, script);
			if (existing != null)
			{
				gui.Interpreter.AddGlobalOrLocalVariable(Constants.POINTER_REF + name,
						 new GetVarFunction(existing));
			}
			return defVar;
		}
		public static Variable TryPointerAssign(ParsingScript script, string name)
		{
			var oper = Char.ToString(script.PrevPrev) + Char.ToString(script.Prev);
			if (oper != Constants.POINTER)
			{
				return null;
			}
			List<string> args = Utils.GetTokens(script);
			Utils.CheckArgs(args.Count, 1, name);

			var result = PointerAssign(script, name, args[0]);
			return result;
		}
	}

	class MyAssignFunction : AssignFunction
	{
		public enum MODE { ASSIGN, INCREMENT, DECREMENT, ASSIGNPLUS, ASSIGNMINUS, ASSIGNMULTIPLY, ASSIGNDIVIDE }

		bool m_pointerAssign;
		string m_originalName;
		int m_arrayIndex = -1;
		bool m_prefix = true;
		public MODE Mode { get; private set; }

		static Dictionary<string, ParsingScript> s_variableMap = new Dictionary<string, ParsingScript>();
		public MyAssignFunction(string name = "")
		{
			m_name = name;
		}
		public MyAssignFunction(MODE mode)
		{
			Mode = mode;
		}
		public static void AddVariableMap(string varName, ParsingScript parentScript)
		{
			s_variableMap[varName.ToLower()] = parentScript;
		}
		public static bool ProcessParentScript(ParsingScript script, string varName, Variable varValue)
		{
			var lower = varName.ToLower();
			if (!s_variableMap.TryGetValue(lower, out ParsingScript parentScript) ||
				parentScript == script)
			{
				return false;
			}
			var assign = new MyAssignFunction(lower);
			DefineVariable defVar = assign.IsDefinedVariable(parentScript);
			if (defVar == null)
			{
				return false;
			}
			var result = assign.DoAssign(parentScript, assign.m_name, defVar, ref varValue);
			return true;
		}

		protected override Variable Evaluate(ParsingScript script)
		{
			var result = ProcessLocalAssign(script);
			if (result != null)
			{
				return result;
			}
			result = Assign(script, m_originalName);
			return ResetNotDefined(result);
		}

		protected override async Task<Variable> EvaluateAsync(ParsingScript script)
		{
			var result = ProcessLocalAssign(script);
			if (result != null)
			{
				return result;
			}
			result = await AssignAsync(script, m_originalName);
			return ResetNotDefined(result);
		}

		Variable ProcessLocalAssign(ParsingScript script)
		{
			var res = MyPointerFunction.TryPointerAssign(script, m_name);
			if (res != null)
			{
				return res;
			}
			CSCS_GUI gui = (CSCS_GUI)script.Context;

			InterpreterInstance = script.InterpreterInstance;
			DefineVariable defVar = IsDefinedVariable(script);
			if (defVar != null)
			{
				Variable varValue = new Variable(Variable.VarType.NONE);
				var result = DoAssign(script, m_name, defVar, ref varValue);

				if (gui != null && gui.GroupBoxesAndRadioButtons != null)
				{
					foreach (string groupBox in gui.GroupBoxesAndRadioButtons.Keys)
					{
						var firstRBName = gui.GroupBoxesAndRadioButtons[groupBox].FirstOrDefault(p => gui.GetWidget(p)?.DataContext.ToString().ToLower() == m_name.ToLower());
						if (firstRBName != null)
						//if (CSCS_GUI.GroupBoxesAndRadioButtons[groupBox].Any(p=> gui.GetWidget(p).DataContext.ToString().ToLower() == m_name.ToLower()))
						{
							//var widget = gui.Controls.First(p => (string)p.Value.DataContext == m_name.ToLower());
							var widget2 = gui.GetWidget(firstRBName.ToLower());

							if (widget2 is RadioButton)
							{
								var radioButton = widget2 as RadioButton;
								radioButton.IsChecked = defVar.AsBool();
							}
						}
					}
				}

				ProcessParentScript(script, m_name, varValue);
				return result;
			}
			var name = m_originalName.EndsWith("]") ? m_originalName : m_name;
			if (Mode == MODE.INCREMENT || Mode == MODE.DECREMENT)
			{
				var result = IncrementDecrementFunction.ProcessAction(name, m_action, m_prefix, script);
				return result;
			}
			if (m_action.Length > 1)
			{
				var result = OperatorAssignFunction.ProcessOperator(name, m_action, script);
				return result;
			}
			return null;
		}
		Variable ResetNotDefined(Variable result)
		{
			DefineVariable defVar = result as DefineVariable;
			if (defVar != null && defVar.Active)
			{
				defVar.DefType = "";
			}
			return result;
		}

		protected DefineVariable IsDefinedVariable(ParsingScript script)
		{
			m_prefix = string.IsNullOrWhiteSpace(m_name);
			if (m_prefix)
			{// If it is a prefix we do not have the variable name yet.
				m_name = Utils.GetToken(script, Constants.TOKEN_SEPARATION);
			}
			m_originalName = m_name;
			m_pointerAssign = m_name.StartsWith(Constants.POINTER_REF);
			var gui = CSCS_GUI.GetInstance(script);
			if (m_pointerAssign)
			{
				m_name = m_name.Substring(Constants.POINTER_REF.Length);
			}

			var origName = m_name;
            int argStart = m_name.IndexOf(Constants.START_ARRAY);
			if (argStart > 0)
			{
                m_name = m_name.Substring(0, argStart);
            }
            if (!gui.DEFINES.TryGetValue(m_name, out DefineVariable defVar))
			{
				return null;
			}
			defVar.Indices.Clear();
			while (argStart > 0)
			{
				int argEnd = origName.IndexOf(Constants.END_ARRAY, argStart + 1);
				if (argEnd < 0)
				{
					break;
				}
				var indStr = origName.Substring(argStart + 1, argEnd - argStart - 1);
				if (Int32.TryParse(indStr, out int ind))
				{
					defVar.Indices.Add(ind);
				}
				else
				{
					var value = gui.ProcessScript(indStr);
					if (value.Type == Variable.VarType.NUMBER)
					{
						defVar.Indices.Add(value.AsInt());
					}
				}
                argStart = origName.IndexOf(Constants.START_ARRAY, argEnd + 1);
            }

            var newValue = new GetVarFunction(defVar);
			script.InterpreterInstance.AddGlobalOrLocalVariable(m_name, newValue);
			if (!string.IsNullOrWhiteSpace(defVar.Pointer))
			{
				script.InterpreterInstance.AddGlobalOrLocalVariable(defVar.Pointer, newValue);
			}

			if (argStart > 0)
			{
				int argEnd = m_originalName.IndexOf(Constants.END_ARRAY, argStart + 1);
				var index = m_originalName.Substring(argStart + 1, argEnd - argStart - 1);
				m_arrayIndex = gui.ProcessScript(index).AsInt();
				/*if (defVar.DefType != "datagrid" && defVar.Tuple != null &&
				    m_arrayIndex >= 0 && m_arrayIndex <= defVar.Tuple.Count - 1)
				{
				    defVar = defVar.Tuple.ElementAt(m_arrayIndex) as DefineVariable;
				}
				if (m_arrayIndex < 0)
				{
				    Console.WriteLine(m_arrayIndex);
				}*/
			}
			return defVar;
		}

		public Variable DoAssign(ParsingScript script, string varName, DefineVariable defVar, ref Variable varValue)
		{
			m_name = Constants.GetRealName(varName);
			script.CurrentAssign = m_name;
			var gui = CSCS_GUI.GetInstance(script);

			bool passedEmpty = varValue.Type == Variable.VarType.NONE;
			if (passedEmpty)
			{
				if (Mode == MODE.INCREMENT)
				{
					varValue.Value = ++defVar.Value;
				}
				else if (Mode == MODE.DECREMENT)
				{
					varValue.Value = --defVar.Value;
				}
				else
				{
					varValue = Utils.GetItem(script);
				}
			}
			if (m_pointerAssign)
			{
				if (gui.DEFINES.TryGetValue(defVar.Pointer, out DefineVariable refValue))
				{
					refValue.InitVariable(varValue, gui, script, false);
					gui.Interpreter.AddGlobalOrLocalVariable(m_originalName,
							new GetVarFunction(refValue));
					gui.Interpreter.AddGlobalOrLocalVariable(defVar.Pointer,
							new GetVarFunction(varValue));
					return refValue;
				}
			}
			else
			{
				if (defVar.DefType == "datagrid")
				{
					var dg = defVar.Object as DataGrid;
					CSCS_GUI.WidgetData wd;
					if (defVar.Index < 0 && gui.WIDGETS.TryGetValue(m_name, out wd))
					{
						if (defVar.Active)
						{
							var column = dg.Columns[m_arrayIndex] as DataGridTextColumn;
							column.Header = varValue.AsString();
						}
						if (gui.DEFINES.TryGetValue(m_name, out DefineVariable headerDef))
						{
							headerDef.SetAsArray();
							while (headerDef.Tuple.Count < dg.Columns.Count)
							{
								headerDef.Tuple.Add(Variable.EmptyInstance);
							}
							headerDef.Tuple[m_arrayIndex] = varValue;
						}
					}
					else
					{
						var rowList = dg.ItemsSource as List<ExpandoObject>;
						if (!gui.WIDGETS.TryGetValue(dg.DataContext as string, out wd) ||
							!gui.DEFINES.TryGetValue(wd.actualElemsName, out DefineVariable actualElems))
						{
							return Variable.EmptyInstance;
						}
						if (wd.maxElems <= m_arrayIndex)
						{
							throw new ArgumentException("Requested element is too big: " + m_arrayIndex + ". Max=" + wd.maxElems);
						}
						while (defVar.Tuple.Count < m_arrayIndex + 1)
						{
							defVar.Tuple.Add(Variable.EmptyInstance);
						}
						if (defVar.Active && m_arrayIndex >= 0 && defVar.Index >= 0)
						{ // Changing value of an existing cell
							AddCell(gui, dg, m_arrayIndex, defVar.Index, varValue);

							FillWidgetFunction.ResetArrays(gui, dg);
							actualElems.Value = rowList.Count;

							FillWidgetFunction.UpdateGridSelection(dg, wd);
						}
					}
				}
				/* else if (defVar.DefType == "linecounter")
				 {
				     var dg = defVar.Object as DataGrid;
				     Application.Current.Dispatcher.Invoke(new Action(() =>
				     {
				         dg.SelectionUnit = DataGridSelectionUnit.CellOrRowHeader;
				         dg.SelectedIndex = varValue.AsInt();
				         var rowList = dg.ItemsSource as ObservableCollection<FillOutGridFunction.Row>;
				         object item = rowList[dg.SelectedIndex];
				         dg.SelectedItem = item;
				         dg.ScrollIntoView(item);
				         DataGridRow row = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(dg.SelectedIndex);
				         row.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
				     }));

				 }*/
				else if (defVar.DefType == "maxelems")
				{
					var dg = defVar.Object as DataGrid;
					var wd = gui.WIDGETS[dg.DataContext as string];
					wd.maxElems = varValue.AsInt();
					if (wd.actualElems <= 0 || wd.actualElems > wd.maxElems)
					{
						wd.actualElems = wd.maxElems;
						if (!string.IsNullOrWhiteSpace(wd.actualElemsName) &&
							gui.DEFINES.TryGetValue(wd.actualElemsName, out DefineVariable actualElems))
						{
							actualElems.Value = wd.maxElems;
							gui.Interpreter.AddGlobal(wd.actualElemsName, new GetVarFunction(actualElems), false);
						}
					}
					var rowList = dg.ItemsSource == null ? new List<ExpandoObject>() :
							dg.ItemsSource as List<ExpandoObject>;
					while (rowList.Count > wd.maxElems)
					{
						rowList.RemoveAt(rowList.Count - 1);
					}
					dg.ItemsSource = rowList;
					dg.Items.Refresh();
					dg.UpdateLayout();
				}
				else
				{
					if (m_action != null && m_action.EndsWith("=") && m_action.Length > 1)
					{ // an action like +=, *=, etc.
                        var leftVar = new DefineVariable(defVar);
						bool initDone = false;
                        if (defVar.Type == Variable.VarType.ARRAY && defVar.Indices.Count > 0)
						{
							Variable tmp = defVar;
							for (int i = 0; i < defVar.Indices.Count && tmp.Tuple != null && tmp.Tuple.Count > defVar.Indices[i]; i++)
							{
                                tmp = tmp.Tuple[defVar.Indices[i]];
							}
							var assigning = tmp.DeepClone();
                            OperatorAssignFunction.ProcessOperator(assigning, varValue, m_action, script, m_name);
                            defVar.AssignArrayValue(assigning);
							initDone = true;
                        }
                        else
						{
                            OperatorAssignFunction.ProcessOperator(leftVar, varValue, m_action, script, m_name);
                        }
						if (!initDone)
						{
                            defVar.InitVariable(leftVar, gui, script, false, m_arrayIndex);
                            varValue = leftVar;
                        }
                    }
					else
					{
						varValue = varValue.DeepClone();
						defVar.InitVariable(varValue, gui, script, false, m_arrayIndex);
					}

					OnVariableChange(m_name, defVar, true);
				}
			}

			if (/*defVar.Object == null &&*/ defVar.Tuple == null)
			{
				gui.Interpreter.AddGlobalOrLocalVariable(m_name, new GetVarFunction(varValue));
			}

			if ((Mode == MODE.INCREMENT || Mode == MODE.DECREMENT) && !m_prefix)
			{
				DefineVariable postfixVar = new DefineVariable(defVar);
				postfixVar.Value = Mode == MODE.INCREMENT ? postfixVar.Value - 1 : postfixVar.Value + 1;
				return postfixVar;
			}
			return defVar;
		}

		public static void AddCell(CSCS_GUI gui, DataGrid dg, int rowNb, int colNb, Variable varValue)
		{
			var name = dg.DataContext as string;
			if (string.IsNullOrWhiteSpace(name) || !gui.WIDGETS.TryGetValue(name, out CSCS_GUI.WidgetData wd))
			{
				return;
			}
			if (dg.ItemsSource == null)
			{
				dg.ItemsSource = new List<ExpandoObject>();
				//dg.ItemsSource = new ObservableCollection<ExpandoObject>();
			}

			if (varValue.Type == Variable.VarType.NUMBER)
			{
				AddCell(dg, rowNb, colNb, wd, varValue.AsDouble());
				wd.colTypes[colNb] = CSCS_GUI.WidgetData.COL_TYPE.NUMBER;
			}
			else
			{
				AddCell(dg, rowNb, colNb, wd, varValue.AsString());
			}
		}

		static void AddCell<T>(DataGrid dg, int rowNb, int colNb, CSCS_GUI.WidgetData wd, T value)
		{
			var rowList = dg.ItemsSource as List<ExpandoObject>;

			while (rowList.Count < rowNb + 1)
			{
				var expando = GetNewRow(dg, wd);
				rowList.Add(expando);
			}

			var pp = rowList[rowNb] as IDictionary<String, object>;
			pp[wd.headerNames[colNb]] = value;
		}

		public static ExpandoObject GetNewRow(DataGrid dg, CSCS_GUI.WidgetData wd)
		{
			dynamic expando = new ExpandoObject();
			var p = expando as IDictionary<String, object>;
			for (int i = 0; i < dg.Columns.Count; i++)
			{
				p[wd.headerNames[i]] = "";
			}
			return expando;
		}

		override public ParserFunction NewInstance()
		{
			return new MyAssignFunction(Mode);
		}
	}

	class AsyncCallFunction : ParserFunction, INumericFunction
	{
		CSCS_GUI Gui;

		protected override Variable Evaluate(ParsingScript script)
		{
			string funcName = Utils.GetToken(script, Constants.TOKEN_SEPARATION);
			script.MoveForwardIf(',');
			string callback = Utils.GetToken(script, Constants.TOKEN_SEPARATION);
			script.MoveForwardIf(',');
			Gui = CSCS_GUI.GetInstance(script);

			List<Variable> args = script.GetFunctionArgs();

			CustomFunction newThreadFunction = Gui.Interpreter.GetFunction(funcName) as CustomFunction;
			if (newThreadFunction == null)
			{
				throw new ArgumentException("Error: Couldn't find function [" + funcName + "]");
			}
			CustomFunction callbackFunction = Gui.Interpreter.GetFunction(callback) as CustomFunction;
			if (callbackFunction == null)
			{
				throw new ArgumentException("Error: Couldn't find function [" + callback + "]");
			}

			ThreadPool.QueueUserWorkItem(unused => ThreadProc(newThreadFunction, callbackFunction, args, script));
			return Variable.EmptyInstance;
		}

		void ThreadProc(CustomFunction newThreadFunction, CustomFunction callbackFunction, List<Variable> args,
			ParsingScript script)
		{
			Variable result = Gui.Interpreter.Run(newThreadFunction, args, script);

			if (result.Type == Variable.VarType.QUIT || result.IsReturn)
			{
				return;
			}

			var resultArgs = new List<Variable>() { new Variable(newThreadFunction.Name), result };

			RunOnMainFunction.RunOnMainThread(callbackFunction, resultArgs);
		}
	}

	class WpfQuitCommand : ParserFunction, INumericFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			var code = Utils.GetSafeInt(args, 0, 0);

			var gui = CSCS_GUI.GetInstance(script);
			gui.CloseAllWindows();

			return QuitFunction.QuitScript(script, code);
		}
	}

	class SetWindowModalResultFunction : ParserFunction
	{
		protected override Variable Evaluate(ParsingScript script)
		{
			List<Variable> args = script.GetFunctionArgs();
			Utils.CheckArgs(args.Count, 1, m_name);

			//var windowName = Utils.GetSafeString(args, 0);
			var modalResult = Utils.GetSafeVariable(args, 0).AsBool();

			var gui = CSCS_GUI.GetInstance(script);

			var scriptWindow = gui.GetScriptWindow(script);
			scriptWindow.DialogResult = modalResult;

			return Variable.EmptyInstance;
		}
	}
}
