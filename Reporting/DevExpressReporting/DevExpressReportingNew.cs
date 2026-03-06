using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting.Caching;
using DevExpress.Xpf.Printing;
using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace WpfCSCS.Reporting.DevExpressReportingNew
{
    /// <summary>
    /// Report operation types
    /// </summary>
    public enum ReportOption
    {
        Setup,              // Load REPX template
        Output,             // Add data row
        Update,             // Update last row
        Print,              // Show print preview
        Export,             // Export to PDF/Excel/Word
        SetParameter,       // Set report parameter
        GetParameter,       // Get report parameter
        ClearData,          // Clear data without reloading template
        Validate,           // Validate template
        GetFields,          // Get list of fields in template
        Configure,          // Configure output options
        SetDataSource,      // Bind SQL/JSON data source
        Dispose             // Free resources
    }

    /// <summary>
    /// Main reporting function class
    /// </summary>
    public class ReportFunction : ParserFunction
    {
        private ReportOption _option;
        private ParsingScript _script;
        private CSCS_GUI _gui;

        // Report manager instance
        private static ReportManager _reportManager = new ReportManager();

        public ReportFunction(ReportOption option)
        {
            _option = option;
        }

        /// <summary>
        /// Register all reporting functions
        /// </summary>
        public static void Init(Interpreter interpreter)
        {
            // Core functions
            interpreter.RegisterFunction(Constants.SETUP_REPORT, new ReportFunction(ReportOption.Setup));
            interpreter.RegisterFunction(Constants.OUTPUT_REPORT, new ReportFunction(ReportOption.Output));
            interpreter.RegisterFunction(Constants.UPDATE_REPORT, new ReportFunction(ReportOption.Update));
            interpreter.RegisterFunction(Constants.PRINT_REPORT, new ReportFunction(ReportOption.Print));

            // New critical functions
            interpreter.RegisterFunction(Constants.EXPORT_REPORT, new ReportFunction(ReportOption.Export));
            interpreter.RegisterFunction(Constants.SET_REPORT_PARAMETER, new ReportFunction(ReportOption.SetParameter));
            interpreter.RegisterFunction(Constants.GET_REPORT_PARAMETER, new ReportFunction(ReportOption.GetParameter));
            interpreter.RegisterFunction(Constants.CLEAR_REPORT_DATA, new ReportFunction(ReportOption.ClearData));

            // Enhanced functions
            interpreter.RegisterFunction(Constants.VALIDATE_REPORT_TEMPLATE, new ReportFunction(ReportOption.Validate));
            interpreter.RegisterFunction(Constants.GET_REPORT_FIELDS, new ReportFunction(ReportOption.GetFields));
            interpreter.RegisterFunction(Constants.CONFIGURE_REPORT_OUTPUT, new ReportFunction(ReportOption.Configure));

            // Advanced functions
            interpreter.RegisterFunction(Constants.SET_REPORT_DATASOURCE, new ReportFunction(ReportOption.SetDataSource));
            interpreter.RegisterFunction(Constants.DISPOSE_REPORT, new ReportFunction(ReportOption.Dispose));
        }

        protected override Variable Evaluate(ParsingScript script)
        {
            _script = script;
            _gui = CSCS_GUI.GetInstance(script);

            try
            {
                switch (_option)
                {
                    case ReportOption.Setup:
                        return SetupReport();

                    case ReportOption.Output:
                        return OutputReport();

                    case ReportOption.Update:
                        return UpdateReport();

                    case ReportOption.Print:
                        return PrintReport();

                    case ReportOption.Export:
                        return ExportReport();

                    case ReportOption.SetParameter:
                        return SetParameter();

                    case ReportOption.GetParameter:
                        return GetParameter();

                    case ReportOption.ClearData:
                        return ClearData();

                    case ReportOption.Validate:
                        return ValidateTemplate();

                    case ReportOption.GetFields:
                        return GetFields();

                    case ReportOption.Configure:
                        return ConfigureOutput();

                    case ReportOption.SetDataSource:
                        return SetDataSource();

                    case ReportOption.Dispose:
                        return DisposeReport();

                    default:
                        throw new ArgumentException($"Unknown report option: {_option}");
                }
            }
            catch (Exception ex)
            {
                var errorMsg = $"Report Error ({_option}): {ex.Message}";
                if (ex.InnerException != null)
                {
                    errorMsg += $"\nInner: {ex.InnerException.Message}";
                }
                throw new Exception(errorMsg, ex);
            }
        }

        #region Core Functions

        /// <summary>
        /// SETUP_REPORT(templatePath, [parentReportHandle])
        /// Returns: report handle (integer)
        /// </summary>
        private Variable SetupReport()
        {
            List<Variable> args = _script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);

            string templatePath = Utils.GetSafeString(args, 0);
            int parentHandle = Utils.GetSafeInt(args, 1, 0);

            // Validate template file exists
            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Report template not found: {templatePath}");
            }

            int reportHandle = _reportManager.CreateReport(templatePath, parentHandle, _gui.Interpreter);

            return new Variable(reportHandle);
        }

        /// <summary>
        /// OUTPUT_REPORT(reportHandle)
        /// Adds current variable values as a new row
        /// </summary>
        private Variable OutputReport()
        {
            List<Variable> args = _script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);

            int reportHandle = Utils.GetSafeInt(args, 0);

            _reportManager.AddDataRow(reportHandle, _gui.Interpreter);

            return Variable.EmptyInstance;
        }

        /// <summary>
        /// UPDATE_REPORT(reportHandle, fieldName1, fieldName2, ...)
        /// Updates specified fields in the last added row
        /// </summary>
        private Variable UpdateReport()
        {
            List<Variable> args = _script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 2, m_name);

            int reportHandle = Utils.GetSafeInt(args, 0);
            List<string> fieldNames = new List<string>();

            // Get comma-separated field names or multiple arguments
            for (int i = 1; i < args.Count; i++)
            {
                string fieldList = Utils.GetSafeString(args, i);
                var fields = fieldList.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                fieldNames.AddRange(fields.Select(f => f.Trim()));
            }

            _reportManager.UpdateLastRow(reportHandle, fieldNames, _gui.Interpreter);

            return Variable.EmptyInstance;
        }

        /// <summary>
        /// PRINT_REPORT(reportHandle, [showDialog])
        /// Shows print preview window
        /// </summary>
        private Variable PrintReport()
        {
            List<Variable> args = _script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);

            int reportHandle = Utils.GetSafeInt(args, 0);
            bool showDialog = Utils.GetSafeInt(args, 1, 1) > 0;

            _reportManager.PrintReport(reportHandle, showDialog);

            return Variable.EmptyInstance;
        }

        #endregion

        #region New Critical Functions

        /// <summary>
        /// EXPORT_REPORT(reportHandle, format, outputPath)
        /// format: "PDF", "XLSX", "DOCX", "HTML", "RTF", "CSV"
        /// </summary>
        private Variable ExportReport()
        {
            List<Variable> args = _script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 3, m_name);

            int reportHandle = Utils.GetSafeInt(args, 0);
            string format = Utils.GetSafeString(args, 1).ToUpper();
            string outputPath = Utils.GetSafeString(args, 2);

            _reportManager.ExportReport(reportHandle, format, outputPath);

            return new Variable(outputPath);
        }

        /// <summary>
        /// SET_REPORT_PARAMETER(reportHandle, parameterName, value)
        /// Sets a parameter value for the report
        /// </summary>
        private Variable SetParameter()
        {
            List<Variable> args = _script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 3, m_name);

            int reportHandle = Utils.GetSafeInt(args, 0);
            string paramName = Utils.GetSafeString(args, 1);
            Variable value = args[2];

            _reportManager.SetParameter(reportHandle, paramName, value);

            return Variable.EmptyInstance;
        }

        /// <summary>
        /// GET_REPORT_PARAMETER(reportHandle, parameterName)
        /// Returns the parameter value
        /// </summary>
        private Variable GetParameter()
        {
            List<Variable> args = _script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 2, m_name);

            int reportHandle = Utils.GetSafeInt(args, 0);
            string paramName = Utils.GetSafeString(args, 1);

            Variable value = _reportManager.GetParameter(reportHandle, paramName);

            return value;
        }

        /// <summary>
        /// CLEAR_REPORT_DATA(reportHandle)
        /// Clears all data rows without reloading template
        /// </summary>
        private Variable ClearData()
        {
            List<Variable> args = _script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);

            int reportHandle = Utils.GetSafeInt(args, 0);

            _reportManager.ClearData(reportHandle);

            return Variable.EmptyInstance;
        }

        #endregion

        #region Enhanced Functions

        /// <summary>
        /// VALIDATE_REPORT_TEMPLATE(templatePath, [requiredFields])
        /// Returns array of error messages (empty if valid)
        /// </summary>
        private Variable ValidateTemplate()
        {
            List<Variable> args = _script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);

            string templatePath = Utils.GetSafeString(args, 0);
            List<string> requiredFields = new List<string>();

            if (args.Count > 1)
            {
                string fieldList = Utils.GetSafeString(args, 1);
                requiredFields = fieldList.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(f => f.Trim()).ToList();
            }

            List<string> errors = ReportValidator.ValidateTemplate(templatePath, requiredFields);

            // Return as CSCS array
            Variable result = new Variable(Variable.VarType.ARRAY);
            foreach (var error in errors)
            {
                result.Tuple.Add(new Variable(error));
            }

            return result;
        }

        /// <summary>
        /// GET_REPORT_FIELDS(templatePath)
        /// Returns array of field names found in template
        /// </summary>
        private Variable GetFields()
        {
            List<Variable> args = _script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);

            string templatePath = Utils.GetSafeString(args, 0);

            List<string> fields = ReportValidator.GetTemplateFields(templatePath);

            // Return as CSCS array
            Variable result = new Variable(Variable.VarType.ARRAY);
            foreach (var field in fields)
            {
                result.Tuple.Add(new Variable(field));
            }

            return result;
        }

        /// <summary>
        /// CONFIGURE_REPORT_OUTPUT(reportHandle, option, value)
        /// Options: "PageSize", "Orientation", "Margins", "ShowPrintDialog"
        /// </summary>
        private Variable ConfigureOutput()
        {
            List<Variable> args = _script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 3, m_name);

            int reportHandle = Utils.GetSafeInt(args, 0);
            string option = Utils.GetSafeString(args, 1);
            string value = Utils.GetSafeString(args, 2);

            _reportManager.ConfigureOutput(reportHandle, option, value);

            return Variable.EmptyInstance;
        }

        #endregion

        #region Advanced Functions

        /// <summary>
        /// SET_REPORT_DATASOURCE(reportHandle, sourceType, connectionStringOrData, [query])
        /// sourceType: "SQL", "JSON", "CSV", "XML"
        /// </summary>
        private Variable SetDataSource()
        {
            List<Variable> args = _script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 3, m_name);

            int reportHandle = Utils.GetSafeInt(args, 0);
            string sourceType = Utils.GetSafeString(args, 1).ToUpper();
            string connectionOrData = Utils.GetSafeString(args, 2);
            string query = Utils.GetSafeString(args, 3, "");

            _reportManager.SetDataSource(reportHandle, sourceType, connectionOrData, query);

            return Variable.EmptyInstance;
        }

        /// <summary>
        /// DISPOSE_REPORT(reportHandle)
        /// Explicitly releases report resources
        /// </summary>
        private Variable DisposeReport()
        {
            List<Variable> args = _script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);

            int reportHandle = Utils.GetSafeInt(args, 0);

            _reportManager.DisposeReport(reportHandle);

            return Variable.EmptyInstance;
        }

        #endregion
    }

    #region Report Manager

    /// <summary>
    /// Manages report instances, data, and operations
    /// </summary>
    internal class ReportManager
    {
        // Template cache for performance
        private static Dictionary<string, XtraReport> _templateCache = new Dictionary<string, XtraReport>();

        // Active report instances
        private Dictionary<int, ReportInstance> _reports = new Dictionary<int, ReportInstance>();

        // Next available report handle
        private int _nextHandle = 1;

        /// <summary>
        /// Create a new report instance
        /// </summary>
        public int CreateReport(string templatePath, int parentHandle, Interpreter interpreter)
        {
            XtraReport report = GetCachedTemplate(templatePath);

            int handle = _nextHandle++;
            bool isSubreport = parentHandle > 0;

            ReportInstance instance = new ReportInstance
            {
                Handle = handle,
                TemplatePath = templatePath,
                Report = report,
                IsSubreport = isSubreport,
                ParentHandle = parentHandle,
                DataSet = new DataSet($"DataSet{handle}"),
                Interpreter = interpreter
            };

            // Create data table
            instance.DataTable = new DataTable($"Table{handle}");
            instance.DataSet.Tables.Add(instance.DataTable);

            // Configure report data binding
            instance.Report.DataSource = instance.DataSet;
            instance.Report.DataMember = instance.DataTable.TableName;

            // Discover fields from template
            DiscoverFields(instance);

            // Setup data table columns
            SetupDataColumns(instance);

            // Link subreport if parent exists
            if (isSubreport && _reports.ContainsKey(parentHandle))
            {
                LinkSubreport(instance, _reports[parentHandle]);
            }

            _reports[handle] = instance;

            return handle;
        }

        /// <summary>
        /// Get cached template or load and cache it
        /// </summary>
        private XtraReport GetCachedTemplate(string templatePath)
        {
            string cacheKey = Path.GetFullPath(templatePath).ToLower();

            if (!_templateCache.ContainsKey(cacheKey))
            {
                var template = new XtraReport();
                template.LoadLayout(templatePath);
                _templateCache[cacheKey] = template;
            }

            // Create a deep copy of the cached template by serializing its layout to memory
            var cached = _templateCache[cacheKey];
            using (var ms = new MemoryStream())
            {
                // Save layout of cached template into stream and load into a new XtraReport
                cached.SaveLayoutToXml(ms);
                ms.Position = 0;
                var clone = new XtraReport();
                clone.LoadLayout(ms);
                return clone;
            }
        }

        /// <summary>
        /// Discover fields from template controls
        /// </summary>
        private void DiscoverFields(ReportInstance instance)
        {
            instance.Fields.Clear();

            // Labels
            var labels = instance.Report.AllControls<XRLabel>();
            foreach (var label in labels)
            {
                if (!string.IsNullOrEmpty(label.Tag?.ToString()))
                {
                    string fieldName = label.Tag.ToString().ToLower();
                    if (!instance.Fields.Contains(fieldName))
                    {
                        instance.Fields.Add(fieldName);
                    }
                    label.ExpressionBindings.Clear();
                    label.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", $"[{fieldName}]"));
                }
            }

            // Barcodes
            var barcodes = instance.Report.AllControls<XRBarCode>();
            foreach (var barcode in barcodes)
            {
                if (!string.IsNullOrEmpty(barcode.Tag?.ToString()))
                {
                    string fieldName = barcode.Tag.ToString().ToLower();
                    if (!instance.Fields.Contains(fieldName))
                    {
                        instance.Fields.Add(fieldName);
                    }
                    barcode.ExpressionBindings.Clear();
                    barcode.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", $"[{fieldName}]"));
                }
            }

            // Pictures
            var pictures = instance.Report.AllControls<XRPictureBox>();
            foreach (var picture in pictures)
            {
                if (!string.IsNullOrEmpty(picture.Tag?.ToString()))
                {
                    string fieldName = picture.Tag.ToString().ToLower();
                    if (!instance.Fields.Contains(fieldName))
                    {
                        instance.Fields.Add(fieldName);
                    }
                    // Pictures can be bound to file paths or byte arrays
                }
            }

            // Charts
            var charts = instance.Report.AllControls<XRChart>();
            foreach (var chart in charts)
            {
                if (!string.IsNullOrEmpty(chart.Tag?.ToString()))
                {
                    string fieldName = chart.Tag.ToString().ToLower();
                    if (!instance.Fields.Contains(fieldName))
                    {
                        instance.Fields.Add(fieldName);
                    }
                    instance.ChartFields.Add(fieldName);
                }
            }

            // Add special fields for subreports
            if (instance.IsSubreport)
            {
                instance.Fields.Add("_parentid");
            }

            instance.Fields.Add("_rowid");
        }

        /// <summary>
        /// Setup data table columns based on discovered fields
        /// </summary>
        private void SetupDataColumns(ReportInstance instance)
        {
            foreach (string fieldName in instance.Fields)
            {
                if (fieldName == "_rowid" || fieldName == "_parentid")
                {
                    instance.DataTable.Columns.Add(fieldName, typeof(int));
                }
                else if (instance.ChartFields.Contains(fieldName))
                {
                    instance.DataTable.Columns.Add(fieldName, typeof(DataTable));
                }
                else
                {
                    // Try to determine type from interpreter
                    Type columnType = GetFieldType(fieldName, instance.Interpreter);
                    instance.DataTable.Columns.Add(fieldName, columnType);
                }
            }
        }

        /// <summary>
        /// Get field type from interpreter variable
        /// </summary>
        private Type GetFieldType(string fieldName, Interpreter interpreter)
        {
            try
            {
                Variable var = interpreter.GetVariableValue(fieldName);
                if (var != null)
                {
                    switch (var.Type)
                    {
                        case Variable.VarType.NUMBER:
                            return typeof(double);
                        case Variable.VarType.STRING:
                            return typeof(string);
                        case Variable.VarType.ARRAY:
                            return typeof(string); // Serialize arrays
                        case Variable.VarType.DATETIME:
                            return typeof(DateTime);
                        default:
                            return typeof(object);
                    }
                }
            }
            catch
            {
                // Variable doesn't exist yet, default to string
            }

            return typeof(string);
        }

        /// <summary>
        /// Link subreport to parent report
        /// </summary>
        //private void LinkSubreport(ReportInstance subreport, ReportInstance parent)
        //{
        //    // Setup filter for subreport
        //    subreport.Report.FilterString = $"[_parentid] = ?parentIdParam";

        //    // Add parameter
        //    if (!subreport.Report.Parameters.Any(p => p.Name == "parentIdParam"))
        //    {
        //        subreport.Report.Parameters.Add(new DevExpress.XtraReports.Parameters.Parameter
        //        {
        //            Name = "parentIdParam",
        //            Type = typeof(int),
        //            Visible = false
        //        });
        //    }

        //    // Find subreport control in parent and link
        //    var subreportControls = parent.Report.AllControls<XRSubreport>();
        //    foreach (var ctrl in subreportControls)
        //    {
        //        string ctrlName = ctrl.Name.ToLower();
        //        string templateName = Path.GetFileNameWithoutExtension(subreport.TemplatePath).ToLower();

        //        if (ctrlName.Contains(templateName) || templateName.Contains(ctrlName))
        //        {
        //            ctrl.ReportSource = subreport.Report;

        //            // Bind parameter
        //            ctrl.ParameterBindings.Clear();
        //            ctrl.ParameterBindings.Add(new ParameterBinding(
        //                "parentIdParam",
        //                parent.DataSet,
        //                parent.DataTable.TableName + "._rowid"
        //            ));
        //            break;
        //        }
        //    }
        //}
        private void LinkSubreport(ReportInstance subreport, ReportInstance parent)
        {
            // Setup filter for subreport
            subreport.Report.FilterString = $"[_parentid] = ?parentIdParam";

            // Add parameter
            if (!subreport.Report.Parameters
                .OfType<DevExpress.XtraReports.Parameters.Parameter>()
                .Any(p => p.Name == "parentIdParam"))
            {
                subreport.Report.Parameters.Add(new DevExpress.XtraReports.Parameters.Parameter
                {
                    Name = "parentIdParam",
                    Type = typeof(int),
                    Visible = false
                });
            }

            // Find subreport control in parent and link
            var subreportControls = parent.Report.AllControls<XRSubreport>();
            foreach (var ctrl in subreportControls)
            {
                string ctrlName = ctrl.Name.ToLower();
                string templateName = Path.GetFileNameWithoutExtension(subreport.TemplatePath).ToLower();

                if (ctrlName.Contains(templateName) || templateName.Contains(ctrlName))
                {
                    ctrl.ReportSource = subreport.Report;

                    // Bind parameter
                    ctrl.ParameterBindings.Clear();
                    ctrl.ParameterBindings.Add(new ParameterBinding(
                        "parentIdParam",
                        parent.DataSet,
                        parent.DataTable.TableName + "._rowid"
                    ));
                    break;
                }
            }
        }

        /// <summary>
        /// Add data row from current interpreter variables
        /// </summary>
        public void AddDataRow(int handle, Interpreter interpreter)
        {
            if (!_reports.TryGetValue(handle, out ReportInstance instance))
            {
                throw new ArgumentException($"Report handle {handle} not found");
            }

            DataRow row = instance.DataTable.NewRow();

            // Auto-increment row ID
            instance.RowCounter++;
            row["_rowid"] = instance.RowCounter;

            // Set parent ID for subreports
            if (instance.IsSubreport && _reports.ContainsKey(instance.ParentHandle))
            {
                row["_parentid"] = _reports[instance.ParentHandle].RowCounter;
            }

            // Populate fields from interpreter variables
            foreach (string fieldName in instance.Fields)
            {
                if (fieldName == "_rowid" || fieldName == "_parentid")
                    continue;

                if (instance.ChartFields.Contains(fieldName))
                {
                    row[fieldName] = GetChartData(fieldName, interpreter);
                }
                else
                {
                    row[fieldName] = GetFieldValue(fieldName, interpreter);
                }
            }

            instance.DataTable.Rows.Add(row);
        }

        /// <summary>
        /// Get field value from interpreter
        /// </summary>
        private object GetFieldValue(string fieldName, Interpreter interpreter)
        {
            try
            {
                Variable var = interpreter.GetVariableValue(fieldName);
                if (var != null)
                {
                    switch (var.Type)
                    {
                        case Variable.VarType.NUMBER:
                            return var.AsDouble();
                        case Variable.VarType.STRING:
                            return var.AsString();
                        case Variable.VarType.ARRAY:
                            return string.Join(", ", var.Tuple.Select(v => v.AsString()));
                        case Variable.VarType.DATETIME:
                            return var.AsDateTime();
                        default:
                            return var.AsString();
                    }
                }
            }
            catch
            {
                // Variable not found or conversion error
            }

            return DBNull.Value;
        }

        /// <summary>
        /// Get chart data from interpreter variables
        /// </summary>
        private DataTable GetChartData(string chartFieldName, Interpreter interpreter)
        {
            DataTable dt = new DataTable(chartFieldName);
            dt.Columns.Add("Argument", typeof(string));
            dt.Columns.Add("Value1", typeof(double));

            try
            {
                // Look for chartName_args and chartName_val_1, chartName_val_2, etc.
                Variable argsVar = interpreter.GetVariableValue($"{chartFieldName}_args");
                Variable val1Var = interpreter.GetVariableValue($"{chartFieldName}_val_1");

                if (argsVar != null && val1Var != null && argsVar.Tuple != null && val1Var.Tuple != null)
                {
                    int count = Math.Min(argsVar.Tuple.Count, val1Var.Tuple.Count);

                    // Check for additional series
                    Variable val2Var = interpreter.GetVariableValue($"{chartFieldName}_val_2");
                    if (val2Var != null && val2Var.Tuple != null)
                    {
                        dt.Columns.Add("Value2", typeof(double));

                        Variable val3Var = interpreter.GetVariableValue($"{chartFieldName}_val_3");
                        if (val3Var != null && val3Var.Tuple != null)
                        {
                            dt.Columns.Add("Value3", typeof(double));

                            for (int i = 0; i < count; i++)
                            {
                                DataRow row = dt.NewRow();
                                row["Argument"] = argsVar.Tuple[i].AsString();
                                row["Value1"] = val1Var.Tuple[i].AsDouble();
                                row["Value2"] = val2Var.Tuple[i].AsDouble();
                                row["Value3"] = val3Var.Tuple[i].AsDouble();
                                dt.Rows.Add(row);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < count; i++)
                            {
                                DataRow row = dt.NewRow();
                                row["Argument"] = argsVar.Tuple[i].AsString();
                                row["Value1"] = val1Var.Tuple[i].AsDouble();
                                row["Value2"] = val2Var.Tuple[i].AsDouble();
                                dt.Rows.Add(row);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            DataRow row = dt.NewRow();
                            row["Argument"] = argsVar.Tuple[i].AsString();
                            row["Value1"] = val1Var.Tuple[i].AsDouble();
                            dt.Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting chart data for {chartFieldName}: {ex.Message}");
            }

            return dt;
        }

        /// <summary>
        /// Update last added row
        /// </summary>
        public void UpdateLastRow(int handle, List<string> fieldNames, Interpreter interpreter)
        {
            if (!_reports.TryGetValue(handle, out ReportInstance instance))
            {
                throw new ArgumentException($"Report handle {handle} not found");
            }

            if (instance.DataTable.Rows.Count == 0)
            {
                throw new InvalidOperationException("No rows to update");
            }

            DataRow lastRow = instance.DataTable.Rows[instance.DataTable.Rows.Count - 1];

            foreach (string fieldName in fieldNames)
            {
                string field = fieldName.ToLower();
                if (instance.DataTable.Columns.Contains(field))
                {
                    if (instance.ChartFields.Contains(field))
                    {
                        lastRow[field] = GetChartData(field, interpreter);
                    }
                    else
                    {
                        lastRow[field] = GetFieldValue(field, interpreter);
                    }
                }
            }
        }

        /// <summary>
        /// Print report
        /// </summary>
        //public void PrintReport(int handle, bool showDialog)
        //{
        //    if (!_reports.TryGetValue(handle, out ReportInstance instance))
        //    {
        //        throw new ArgumentException($"Report handle {handle} not found");
        //    }

        //    // Get main report (handle 1 or first non-subreport)
        //    ReportInstance mainReport = instance;
        //    if (instance.IsSubreport)
        //    {
        //        mainReport = _reports.Values.FirstOrDefault(r => !r.IsSubreport) ?? instance;
        //    }

        //    var storage = new MemoryDocumentStorage();
        //    var cachedReportSource = new CachedReportSource(mainReport.Report, storage);

        //    if (showDialog)
        //    {
        //        PrintHelper.ShowRibbonPrintPreview(null, cachedReportSource);
        //    }
        //    else
        //    {
        //        PrintHelper.ShowRibbonPrintPreviewDialog(null, cachedReportSource);
        //    }
        //}

        //public void PrintReport(int handle, bool showDialog)
        //{
        //    if (!_reports.TryGetValue(handle, out ReportInstance instance))
        //    {
        //        throw new ArgumentException($"Report handle {handle} not found");
        //    }

        //    // Get main report (handle 1 or first non-subreport)
        //    ReportInstance mainReport = instance;
        //    if (instance.IsSubreport)
        //    {
        //        mainReport = _reports.Values.FirstOrDefault(r => !r.IsSubreport) ?? instance;
        //    }

        //    var storage = new MemoryDocumentStorage();
        //    var cachedReportSource = new CachedReportSource(mainReport.Report, storage);

        //    // Prepare action that calls DevExpress preview APIs and handles fallback
        //    Action showAction = () =>
        //    {
        //        var owner = System.Windows.Application.Current?.MainWindow;

        //        try
        //        {
        //            // Keep original mapping but call with a real owner
        //            if (showDialog)
        //            {
        //                PrintHelper.ShowRibbonPrintPreview(owner, cachedReportSource);
        //            }
        //            else
        //            {
        //                PrintHelper.ShowRibbonPrintPreviewDialog(owner, cachedReportSource);
        //            }
        //        }
        //        catch (NotImplementedException)
        //        {
        //            Debug.WriteLine("DevExpress preview variant threw NotImplementedException — attempting alternate preview API.");
        //            try
        //            {
        //                // Try the alternate API if the first one is not implemented in this environment
        //                if (showDialog)
        //                {
        //                    PrintHelper.ShowRibbonPrintPreviewDialog(owner, cachedReportSource);
        //                }
        //                else
        //                {
        //                    PrintHelper.ShowRibbonPrintPreview(owner, cachedReportSource);
        //                }
        //            }
        //            catch (Exception ex2)
        //            {
        //                Debug.WriteLine($"Print preview fallback failed: {ex2}");
        //                throw;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine($"Print preview failed: {ex}");
        //            throw;
        //        }
        //    };

        //    // Ensure we execute on the WPF UI thread
        //    var app = System.Windows.Application.Current;
        //    if (app?.Dispatcher?.CheckAccess() == true)
        //    {
        //        showAction();
        //    }
        //    else
        //    {
        //        app?.Dispatcher?.Invoke(showAction);
        //    }
        //}

        public void PrintReport(int handle, bool showDialog)
        {
            if (!_reports.TryGetValue(handle, out ReportInstance instance))
            {
                throw new ArgumentException($"Report handle {handle} not found");
            }

            // Get main report (handle 1 or first non-subreport)
            ReportInstance mainReport = instance;
            if (instance.IsSubreport)
            {
                mainReport = _reports.Values.FirstOrDefault(r => !r.IsSubreport) ?? instance;
            }

            var storage = new MemoryDocumentStorage();
            var cachedReportSource = new CachedReportSource(mainReport.Report, storage);

            Action showAction = () =>
            {
                System.Windows.Window owner = null;
                System.Windows.Window tempOwner = null;

                try
                {
                    // Prefer active window, then MainWindow
                    owner = System.Windows.Application.Current?
                                .Windows
                                .OfType<System.Windows.Window>()
                                .FirstOrDefault(w => w.IsActive) ?? System.Windows.Application.Current?.MainWindow;

                    // If still null, create a hidden temporary owner window
                    if (owner == null)
                    {
                        tempOwner = new System.Windows.Window
                        {
                            Width = 0,
                            Height = 0,
                            ShowInTaskbar = false,
                            WindowStyle = System.Windows.WindowStyle.None,
                            AllowsTransparency = true,
                            Opacity = 0
                        };
                        tempOwner.Show();
                        owner = tempOwner;
                    }

                    if (showDialog)
                    {
                        PrintHelper.ShowRibbonPrintPreview(owner, cachedReportSource);
                    }
                    else
                    {
                        PrintHelper.ShowRibbonPrintPreviewDialog(owner, cachedReportSource);
                    }
                }
                catch (NotImplementedException)
                {
                    Debug.WriteLine("DevExpress preview raised NotImplementedException — try alternate API.");
                    // try alternate ordering as a fallback
                    if (showDialog)
                        PrintHelper.ShowRibbonPrintPreviewDialog(owner, cachedReportSource);
                    else
                        PrintHelper.ShowRibbonPrintPreview(owner, cachedReportSource);
                }
                finally
                {
                    // Close temporary owner if created
                    try
                    {
                        if (tempOwner != null)
                        {
                            tempOwner.Close();
                        }
                    }
                    catch { }
                }
            };

            var app = System.Windows.Application.Current;
            if (app?.Dispatcher?.CheckAccess() == true)
            {
                showAction();
            }
            else
            {
                app?.Dispatcher?.Invoke(showAction);
            }
        }

        /// <summary>
        /// Export report to file
        /// </summary>
        public void ExportReport(int handle, string format, string outputPath)
        {
            if (!_reports.TryGetValue(handle, out ReportInstance instance))
            {
                throw new ArgumentException($"Report handle {handle} not found");
            }

            // Get main report
            ReportInstance mainReport = instance;
            if (instance.IsSubreport)
            {
                mainReport = _reports.Values.FirstOrDefault(r => !r.IsSubreport) ?? instance;
            }

            // Ensure output directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            switch (format.ToUpper())
            {
                case "PDF":
                    mainReport.Report.ExportToPdf(outputPath);
                    break;

                case "XLSX":
                case "EXCEL":
                    mainReport.Report.ExportToXlsx(outputPath);
                    break;

                case "DOCX":
                case "WORD":
                    mainReport.Report.ExportToDocx(outputPath);
                    break;

                case "HTML":
                    mainReport.Report.ExportToHtml(outputPath);
                    break;

                case "RTF":
                    mainReport.Report.ExportToRtf(outputPath);
                    break;

                case "CSV":
                    mainReport.Report.ExportToCsv(outputPath);
                    break;

                case "XLS":
                    mainReport.Report.ExportToXls(outputPath);
                    break;

                default:
                    throw new ArgumentException($"Unsupported export format: {format}");
            }
        }

        /// <summary>
        /// Set report parameter
        /// </summary>
        public void SetParameter(int handle, string paramName, Variable value)
        {
            if (!_reports.TryGetValue(handle, out ReportInstance instance))
            {
                throw new ArgumentException($"Report handle {handle} not found");
            }

            // Find or create parameter
            var param = instance.Report.Parameters[paramName];
            if (param == null)
            {
                param = new DevExpress.XtraReports.Parameters.Parameter
                {
                    Name = paramName,
                    Visible = false
                };
                instance.Report.Parameters.Add(param);
            }

            // Set value based on type
            switch (value.Type)
            {
                case Variable.VarType.NUMBER:
                    param.Type = typeof(double);
                    param.Value = value.AsDouble();
                    break;

                case Variable.VarType.STRING:
                    param.Type = typeof(string);
                    param.Value = value.AsString();
                    break;

                case Variable.VarType.DATETIME:
                    param.Type = typeof(DateTime);
                    param.Value = value.AsDateTime();
                    break;

                default:
                    param.Type = typeof(string);
                    param.Value = value.AsString();
                    break;
            }

            // Store in instance
            instance.Parameters[paramName] = value;
        }

        /// <summary>
        /// Get report parameter
        /// </summary>
        public Variable GetParameter(int handle, string paramName)
        {
            if (!_reports.TryGetValue(handle, out ReportInstance instance))
            {
                throw new ArgumentException($"Report handle {handle} not found");
            }

            if (instance.Parameters.TryGetValue(paramName, out Variable value))
            {
                return value;
            }

            var param = instance.Report.Parameters[paramName];
            if (param != null && param.Value != null)
            {
                return new Variable(param.Value.ToString());
            }

            return Variable.EmptyInstance;
        }

        /// <summary>
        /// Clear report data
        /// </summary>
        public void ClearData(int handle)
        {
            if (!_reports.TryGetValue(handle, out ReportInstance instance))
            {
                throw new ArgumentException($"Report handle {handle} not found");
            }

            instance.DataTable.Rows.Clear();
            instance.RowCounter = 0;
        }

        /// <summary>
        /// Configure output options
        /// </summary>
        public void ConfigureOutput(int handle, string option, string value)
        {
            if (!_reports.TryGetValue(handle, out ReportInstance instance))
            {
                throw new ArgumentException($"Report handle {handle} not found");
            }

            switch (option.ToUpper())
            {
                case "PAGESIZE":
                    SetPageSize(instance.Report, value);
                    break;

                case "ORIENTATION":
                    SetOrientation(instance.Report, value);
                    break;

                case "MARGINS":
                    SetMargins(instance.Report, value);
                    break;

                default:
                    instance.Configuration[option] = value;
                    break;
            }
        }

        private void SetPageSize(XtraReport report, string size)
        {
            switch (size.ToUpper())
            {
                case "A4":
                    report.PageWidth = 827; // 210mm in 1/100 inch
                    report.PageHeight = 1169; // 297mm
                    break;
                case "LETTER":
                    report.PageWidth = 850;
                    report.PageHeight = 1100;
                    break;
                case "LEGAL":
                    report.PageWidth = 850;
                    report.PageHeight = 1400;
                    break;
            }
        }

        private void SetOrientation(XtraReport report, string orientation)
        {
            if (orientation.ToUpper() == "LANDSCAPE")
            {
                int temp = report.PageWidth;
                report.PageWidth = report.PageHeight;
                report.PageHeight = temp;
            }
        }

        private void SetMargins(XtraReport report, string margins)
        {
            var parts = margins.Split(',');
            if (parts.Length == 4)
            {
                report.Margins.Left = int.Parse(parts[0].Trim());
                report.Margins.Top = int.Parse(parts[1].Trim());
                report.Margins.Right = int.Parse(parts[2].Trim());
                report.Margins.Bottom = int.Parse(parts[3].Trim());
            }
        }

        /// <summary>
        /// Set data source from SQL/JSON/etc
        /// </summary>
        public void SetDataSource(int handle, string sourceType, string connectionOrData, string query)
        {
            if (!_reports.TryGetValue(handle, out ReportInstance instance))
            {
                throw new ArgumentException($"Report handle {handle} not found");
            }

            switch (sourceType)
            {
                case "SQL":
                    SetSqlDataSource(instance, connectionOrData, query);
                    break;

                case "JSON":
                    SetJsonDataSource(instance, connectionOrData);
                    break;

                default:
                    throw new ArgumentException($"Unsupported data source type: {sourceType}");
            }
        }

        private void SetSqlDataSource(ReportInstance instance, string connectionString, string query)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        instance.DataTable.Clear();
                        adapter.Fill(instance.DataTable);
                    }
                }
            }
        }

        private void SetJsonDataSource(ReportInstance instance, string jsonData)
        {
            // Simple JSON parsing - for production use JSON.NET or System.Text.Json
            throw new NotImplementedException("JSON data source not yet implemented");
        }

        /// <summary>
        /// Dispose report and free resources
        /// </summary>
        public void DisposeReport(int handle)
        {
            if (_reports.TryGetValue(handle, out ReportInstance instance))
            {
                instance.Report?.Dispose();
                instance.DataSet?.Dispose();
                _reports.Remove(handle);
            }
        }

        /// <summary>
        /// Dispose all reports
        /// </summary>
        public void DisposeAll()
        {
            foreach (var instance in _reports.Values)
            {
                instance.Report?.Dispose();
                instance.DataSet?.Dispose();
            }
            _reports.Clear();
            _templateCache.Clear();
        }
    }

    /// <summary>
    /// Report instance data
    /// </summary>
    internal class ReportInstance
    {
        public int Handle { get; set; }
        public string TemplatePath { get; set; }
        public XtraReport Report { get; set; }
        public DataSet DataSet { get; set; }
        public DataTable DataTable { get; set; }
        public bool IsSubreport { get; set; }
        public int ParentHandle { get; set; }
        public List<string> Fields { get; set; } = new List<string>();
        public List<string> ChartFields { get; set; } = new List<string>();
        public Dictionary<string, Variable> Parameters { get; set; } = new Dictionary<string, Variable>();
        public Dictionary<string, string> Configuration { get; set; } = new Dictionary<string, string>();
        public int RowCounter { get; set; }
        public Interpreter Interpreter { get; set; }
    }

    #endregion

    #region Report Validator

    /// <summary>
    /// Validates report templates
    /// </summary>
    internal static class ReportValidator
    {
        public static List<string> ValidateTemplate(string templatePath, List<string> requiredFields)
        {
            List<string> errors = new List<string>();

            try
            {
                if (!File.Exists(templatePath))
                {
                    errors.Add($"Template file not found: {templatePath}");
                    return errors;
                }

                XtraReport report = new XtraReport();
                report.LoadLayout(templatePath);

                // Get all fields in template
                List<string> templateFields = GetTemplateFields(report);

                // Check required fields
                foreach (string requiredField in requiredFields)
                {
                    if (!templateFields.Contains(requiredField.ToLower()))
                    {
                        errors.Add($"Required field not found in template: {requiredField}");
                    }
                }

                report.Dispose();
            }
            catch (Exception ex)
            {
                errors.Add($"Template validation error: {ex.Message}");
            }

            return errors;
        }

        public static List<string> GetTemplateFields(string templatePath)
        {
            if (!File.Exists(templatePath))
            {
                return new List<string>();
            }

            XtraReport report = new XtraReport();
            report.LoadLayout(templatePath);
            List<string> fields = GetTemplateFields(report);
            report.Dispose();

            return fields;
        }

        public static List<string> GetTemplateFields(XtraReport report)
        {
            HashSet<string> fields = new HashSet<string>();

            // Labels
            var labels = report.AllControls<XRLabel>();
            foreach (var label in labels)
            {
                if (!string.IsNullOrEmpty(label.Tag?.ToString()))
                {
                    fields.Add(label.Tag.ToString().ToLower());
                }
            }

            // Barcodes
            var barcodes = report.AllControls<XRBarCode>();
            foreach (var barcode in barcodes)
            {
                if (!string.IsNullOrEmpty(barcode.Tag?.ToString()))
                {
                    fields.Add(barcode.Tag.ToString().ToLower());
                }
            }

            // Pictures
            var pictures = report.AllControls<XRPictureBox>();
            foreach (var picture in pictures)
            {
                if (!string.IsNullOrEmpty(picture.Tag?.ToString()))
                {
                    fields.Add(picture.Tag.ToString().ToLower());
                }
            }

            // Charts
            var charts = report.AllControls<XRChart>();
            foreach (var chart in charts)
            {
                if (!string.IsNullOrEmpty(chart.Tag?.ToString()))
                {
                    fields.Add(chart.Tag.ToString().ToLower());
                }
            }

            return fields.ToList();
        }
    }

    #endregion
}