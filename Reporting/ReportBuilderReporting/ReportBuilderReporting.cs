using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;


namespace WpfCSCS.Reporting.ReportBuilderReporting
{
    public enum ReportOption
    {
        Setup,
        Output,
        Update,
        Print
    }
    public class ReportFunction : ParserFunction
    {
        public static void Init(Interpreter interpreter)
        {
            //interpreter.RegisterFunction(Constants.SETUP_REPORT, new ReportFunction(ReportOption.Setup));
            //interpreter.RegisterFunction(Constants.OUTPUT_REPORT, new ReportFunction(ReportOption.Output));
            //interpreter.RegisterFunction(Constants.UPDATE_REPORT, new ReportFunction(ReportOption.Update));
            //interpreter.RegisterFunction(Constants.PRINT_REPORT, new ReportFunction(ReportOption.Print));


            interpreter.RegisterFunction(Constants.SETUP_REPORT, new SetupReportFunction());
            interpreter.RegisterFunction(Constants.OUTPUT_REPORT, new OutputReportFunction());
            interpreter.RegisterFunction(Constants.UPDATE_REPORT, new UpdateReportFunction());
            interpreter.RegisterFunction(Constants.PRINT_REPORT, new PrintReportFunction());

            interpreter.RegisterFunction(Constants.SET_PARAM_REPORT, new SetParamReportFunction());
            interpreter.RegisterFunction(Constants.GET_PARAM_REPORT, new GetParamReportFunction());
        }

        //ReportOption option;

        //ParsingScript Script;
        //CSCS_GUI Gui;




        //static Dictionary<int, DataSet> DataSets = new Dictionary<int, DataSet>();
        //static Dictionary<int, DataTable> DataTables = new Dictionary<int, DataTable>();

        //static List<string> chartTags = new List<string>();

        //static Dictionary<int, List<string>> fieldsOfReports = new Dictionary<int, List<string>>();

        //static Dictionary<int, int> lastReportsNumbers = new Dictionary<int, int>();




        //    public ReportFunction(ReportOption _option)
        //    {
        //        option = _option;
        //    }

        //    protected override Variable Evaluate(ParsingScript script)
        //    {
        //        Script = script;
        //        Gui = CSCS_GUI.GetInstance(script);

        //        switch (option)
        //        {
        //            case ReportOption.Setup:
        //                return SetupReport();

        //            case ReportOption.Output:
        //                OutputReport();
        //                break;

        //            case ReportOption.Update:
        //                UpdateReport();
        //                break;

        //            case ReportOption.Print:
        //                PrintReport();
        //                break;
        //        }

        //        return Variable.EmptyInstance;
        //    }

        //    private Variable SetupReport()
        //    {
        //        //

        //        return Variable.EmptyInstance;
        //    }



        //    private void OutputReport()
        //    {

        //    }

        //    private void UpdateReport()
        //    {

        //    }

        //    private void PrintReport()
        //    {

        //    }
        //}

        class SetupReportFunction : ParserFunction
        {
            #region RTMProvjera
            private int RTMProvjera(string reportFileName)
            {
                if (string.IsNullOrEmpty(reportFileName))
                {
                    MessageBox.Show("SetupReport: empty reportInputPath");
                    return 0;//error
                }

                bool b = File.Exists(reportFileName);

                if (b == true)
                {
                    return 1;//ok
                }
                else
                {
                    MessageBox.Show("SetupReport/n/nFile:/n" + reportFileName + "/n" + "ne postoji!");
                    return 0;//error
                }
            }
            #endregion

            private int SetupReport(int TAG, int parentTAG, bool IsMainReport, string filename)
            {
                int reportHandle;

                if (IsMainReport)
                {
                    FreeDictionary();
                    LoadRTM(filename);
                    InitDictionary();
                }

                reportHandle = InitReport(TAG, parentTAG);//Main

                if (IsMainReport)
                {
                    SetMainReport(reportHandle);
                }

                InitListKomponente(reportHandle);
                int countOfKomponente = KomponenteListCount(reportHandle);
                //MessageBox.Show("REPORT=" + reportHandle.ToString() + " countOfKomponente =" + countOfKomponente.ToString());
                GetKomponente(reportHandle, countOfKomponente);
                GetDBTextDataFields(reportHandle, countOfKomponente);
                GetDBMemoDataFields(reportHandle, countOfKomponente);
                GetDBRichTextDataFields(reportHandle, countOfKomponente);
                GetDBImageDataFields(reportHandle, countOfKomponente);
                GetDBCheckBoxDataFields(reportHandle, countOfKomponente);

                //ISPIS STAVI U PrintReport
                //AURA_Test.IspisLabelaUMessageBox();
                //AURA_Test.IspisDBTextovaUMessageBox();
                //AURA_Test.IspisDBMemoaUMessageBox();

                return reportHandle;
            }

            protected override Variable Evaluate(ParsingScript script)
            {
                //MessageBox.Show("AURA_SetupReportFunction");

                List<Variable> args = script.GetFunctionArgs();

                Utils.CheckArgs(args.Count, 1, m_name);

                int reportHandle = 0;

                //var reportFileName;

                if (args.Count == 1)
                {
                    var TAG = 1;
                    var parentTAG = 0;
                    var reportFileName = Utils.GetSafeString(args, 0);
                    reportHandle = SetupReport(TAG, parentTAG, true, reportFileName);
                }

                if (args.Count == 2)
                {
                    var TAG = Utils.GetSafeInt(args, 0);
                    var parentTAG = Utils.GetSafeInt(args, 1);
                    reportHandle = SetupReport(TAG, parentTAG, false, "");
                }

                return new Variable(reportHandle);
                //
                //Variable retVar = new Variable();
                ////retVar.Type = Variable.VarType.INT;
                //retVar.Value = reportHandle;
                //return retVar;

            }

            private void FreeDictionary()
            {
                UInt32 x = ReportBuilderDll.DLLTest();

                x = ReportBuilderDll.DLLFreeDictionary();

                //MessageBox.Show("x=" + x.ToString());//0=ERROR 1=OK
            }

            private void LoadRTM(string RTMFolderfilename)
            {
                MyDictionary.RTMFolderFilename = RTMFolderfilename;
                //MyDictionary.RTMFolderFilename = "D:\\DELPHI\\MyDLLReport15\\RTM\\MyReport15A3.rtm";
                //MyDictionary.RTMFolderFilename = "D:\\DELPHI\\MyDLLReport11\\RTM\\wkprda3.rtm";
                //MyDictionary.RTMFolderFilename = "D:\\DELPHI\\MyDLLReport11\\RTM\\wkpogfa2.rtm";

                UInt32 x = ReportBuilderDll.DLLLoadRTM_(MyDictionary.RTMFolderFilename);

                //MessageBox.Show("x=" + x.ToString());//0=ERROR 1=OK
            }

            private void InitDictionary()
            {
                UInt32 x = ReportBuilderDll.DLLInitDictionary();

                //MessageBox.Show("x=" + x.ToString());//0=ERROR 1=OK
            }

            private int InitReport(int reportTAGToFind, int parentTAG)
            {
                int reportHandle;

                UInt32 x = ReportBuilderDll.DLLInitReport_(reportTAGToFind, parentTAG, out reportHandle);

                //MessageBox.Show("C# reportHandle = " + reportHandle.ToString());

                Report report = new Report();
                report.parentHandle = parentTAG;

                MyDictionary.Reports.Add(reportHandle, report);

                return reportHandle;
                //MessageBox.Show("x=" + x.ToString());//0=ERROR 1=OK
            }

            private void SetMainReport(int reportHandle)
            {
                Report report = MyDictionary.Reports[reportHandle];
                report.IsMainReport = true;
            }

            private void InitListKomponente(int reportHandle)
            {
                //MessageBox.Show("C# reportHandle = " + reportHandle.ToString());

                UInt32 x = ReportBuilderDll.DLLInitListKomponente(reportHandle);

                //MessageBox.Show("x=" + x.ToString());//0=ERROR 1=OK
            }

            private int KomponenteListCount(int reportHandle)
            {
                int countOfKomponente;
                //MessageBox.Show("reportHandle = " + reportHandle.ToString());
                UInt32 x = ReportBuilderDll.DLLKomponenteListCount_(reportHandle, out countOfKomponente);

                //MessageBox.Show("x=" + x.ToString());//0=ERROR 1=OK

                //MessageBox.Show("countOfKomponente=" + countOfKomponente.ToString());
                return countOfKomponente;
            }

            private void GetKomponente(int reportHandle, int countOfKomponente)
            {
                Report report = MyDictionary.Reports[reportHandle];

                int i;
                Komponenta komponenta;
                for (i = 0; i < countOfKomponente; i++)
                {
                    ReportBuilderDll.DLLGetKomponenta_(reportHandle, i, out komponenta);
                    report.MyListKomponente.Add(komponenta);
                    //MessageBox.Show(komponenta.Name);

                    if (komponenta.ClassName == "TppLabel")
                    {
                        LabelParametri labelParametri = new LabelParametri();
                        komponenta.parametriKomponente = labelParametri;
                    }
                    else if (komponenta.ClassName == "TppDBText")
                    {
                        DBTextParametri dBTextParametri = new DBTextParametri();
                        komponenta.parametriKomponente = dBTextParametri;
                    }
                    else if (komponenta.ClassName == "TppDBMemo")
                    {
                        DBMemoParametri dBMemoParametri = new DBMemoParametri();
                        komponenta.parametriKomponente = dBMemoParametri;
                    }
                    else if (komponenta.ClassName == "TppDBRichText")
                    {
                        DBRichTextParametri dBRichTextParametri = new DBRichTextParametri();
                        komponenta.parametriKomponente = dBRichTextParametri;
                    }
                    else if (komponenta.ClassName == "TppDBImage")
                    {
                        DBImageParametri dBImageParametri = new DBImageParametri();
                        komponenta.parametriKomponente = dBImageParametri;
                    }
                    else if (komponenta.ClassName == "TmyDBCheckBox")
                    {
                        DBCheckBoxParametri dBCheckBoxParametri = new DBCheckBoxParametri();
                        komponenta.parametriKomponente = dBCheckBoxParametri;
                    }

                }
            }

            private void GetDBTextDataFields(int reportHandle, int countOfKomponente)
            {
                Report report = MyDictionary.Reports[reportHandle];

                int i;
                Komponenta komponenta;
                DBTextParametri dBTextParametri;
                string DataField;
                for (i = 0; i < countOfKomponente; i++)
                {
                    komponenta = report.MyListKomponente[i];

                    if (komponenta.ClassName == "TppDBText")
                    {
                        //MessageBox.Show(komponenta.Name + "\n");

                        ReportBuilderDll.DLLGetDBTextDataField_(reportHandle, i, out DataField);

                        //parametriKomponente se stvaraju u GetKomponente
                        dBTextParametri = (DBTextParametri)komponenta.parametriKomponente;

                        dBTextParametri.DataField = DataField;

                        //MessageBox.Show(komponenta.Name + "\n" + DataField);
                    }
                }
            }

            private void GetDBMemoDataFields(int reportHandle, int countOfKomponente)
            {
                Report report = MyDictionary.Reports[reportHandle];

                int i;
                Komponenta komponenta;
                DBMemoParametri dBMemoParametri;
                string DataField;
                for (i = 0; i < countOfKomponente; i++)
                {
                    komponenta = report.MyListKomponente[i];

                    if (komponenta.ClassName == "TppDBMemo")
                    {
                        //MessageBox.Show(komponenta.Name + "\n");

                        ReportBuilderDll.DLLGetDBMemoDataField_(reportHandle, i, out DataField);

                        //parametriKomponente se stvaraju u GetKomponente
                        dBMemoParametri = (DBMemoParametri)komponenta.parametriKomponente;

                        dBMemoParametri.DataField = DataField;

                        //MessageBox.Show(komponenta.Name + "\n" + DataField);
                    }
                }
            }

            private void GetDBRichTextDataFields(int reportHandle, int countOfKomponente)
            {
                Report report = MyDictionary.Reports[reportHandle];

                int i;
                Komponenta komponenta;
                DBRichTextParametri dBRichTextParametri;
                string DataField;
                for (i = 0; i < countOfKomponente; i++)
                {
                    komponenta = report.MyListKomponente[i];

                    if (komponenta.ClassName == "TppDBRichText")
                    {
                        //MessageBox.Show(komponenta.Name + "\n");

                        ReportBuilderDll.DLLGetDBRichTextDataField_(reportHandle, i, out DataField);

                        //parametriKomponente se stvaraju u GetKomponente
                        dBRichTextParametri = (DBRichTextParametri)komponenta.parametriKomponente;

                        dBRichTextParametri.DataField = DataField;

                        //MessageBox.Show(komponenta.Name + "\n" + DataField);
                    }
                }
            }

            private void GetDBImageDataFields(int reportHandle, int countOfKomponente)
            {
                Report report = MyDictionary.Reports[reportHandle];

                int i;
                Komponenta komponenta;
                DBImageParametri dBImageParametri;
                string DataField;
                for (i = 0; i < countOfKomponente; i++)
                {
                    komponenta = report.MyListKomponente[i];

                    if (komponenta.ClassName == "TppDBImage")
                    {
                        //MessageBox.Show(komponenta.Name + "\n");

                        ReportBuilderDll.DLLGetDBImageDataField_(reportHandle, i, out DataField);

                        //parametriKomponente se stvaraju u GetKomponente
                        dBImageParametri = (DBImageParametri)komponenta.parametriKomponente;

                        dBImageParametri.DataField = DataField;

                        //MessageBox.Show(komponenta.Name + "\n" + DataField);
                    }
                }
            }

            private void GetDBCheckBoxDataFields(int reportHandle, int countOfKomponente)
            {
                Report report = MyDictionary.Reports[reportHandle];

                int i;
                Komponenta komponenta;
                DBCheckBoxParametri dBCheckBoxParametri;
                string DataField;
                for (i = 0; i < countOfKomponente; i++)
                {
                    komponenta = report.MyListKomponente[i];

                    if (komponenta.ClassName == "TmyDBCheckBox")
                    {
                        //MessageBox.Show(komponenta.Name + "\n");

                        ReportBuilderDll.DLLGetDBCheckBoxDataField_(reportHandle, i, out DataField);

                        //parametriKomponente se stvaraju u GetKomponente
                        dBCheckBoxParametri = (DBCheckBoxParametri)komponenta.parametriKomponente;

                        dBCheckBoxParametri.DataField = DataField;

                        //MessageBox.Show(komponenta.Name + "\n" + DataField);
                    }
                }
            }
        }

        class OutputReportFunction : ParserFunction
        {
            CSCS_GUI Gui;
            private void OutputReport(int reportHandle)
            {
                OUTPUTNVarijabli(reportHandle);
            }

            protected override Variable Evaluate(ParsingScript script)
            {
                //MessageBox.Show("AURA_OutputReportFunction");

                Gui = CSCS_GUI.GetInstance(script);

                List<Variable> args = script.GetFunctionArgs();

                Utils.CheckArgs(args.Count, 1, m_name);

                var reportHandle = Utils.GetSafeInt(args, 0);

                OutputReport(reportHandle);

                //return Variable.EmptyInstance;

                return null;
            }

            private void OUTPUTNVarijabli(int reportHandle)
            {
                Report report = MyDictionary.Reports[reportHandle];

                int i;
                Komponenta komponenta;
                string nazivKolone;
                DataColumn column;
                DataRow dataRow;
                TipVarijable tipVarijable;
                bool columnExists;

                #region DODAJ KOLONE U TablePodaci
                //Specijalne kolone IDRow, IDParentRow
                if (report.TablePodaci.Columns.Count == 0)
                {
                    column = new System.Data.DataColumn();
                    column.ColumnName = "IDRow";
                    column.DataType = typeof(System.Int32);
                    report.TablePodaci.Columns.Add(column);

                    column = new System.Data.DataColumn();
                    column.ColumnName = "IDParentRow";
                    column.DataType = typeof(System.Int32);
                    report.TablePodaci.Columns.Add(column);
                }

                //dodaj kolone za DBText-ove
                DBTextParametri dBTextParametri;
                for (i = 0; i < report.MyListKomponente.Count; i++)
                {
                    komponenta = report.MyListKomponente[i];

                    if (komponenta.ClassName == "TppDBText")
                    {
                        dBTextParametri = komponenta.parametriKomponente as DBTextParametri;
                        nazivKolone = dBTextParametri.DataField;

                        if (Gui.DEFINES.TryGetValue(nazivKolone.ToLower(), out DefineVariable defVar))
                        {
                            tipVarijable = new TipVarijable();

                            switch (defVar.DefType)
                            {
                                case "a":
                                    tipVarijable.Tip = "STRING";
                                    tipVarijable.Size = defVar.Size;
                                    break;

                                case "b":
                                case "i":
                                case "r":
                                    tipVarijable.Tip = "INTEGER";
                                    tipVarijable.Size = -1;
                                    break;

                                case "n":
                                    tipVarijable.Tip = "DOUBLE";
                                    tipVarijable.Size = -1;
                                    break;

                                case "d":
                                case "t":
                                    tipVarijable.Tip = "DATETIME";
                                    tipVarijable.Size = -1;
                                    break;

                                case "l":
                                    tipVarijable.Tip = "BOOLEAN";
                                    tipVarijable.Size = -1;
                                    break;

                                default:
                                    break;
                            }

                            //MessageBox.Show("nazivKolone=" + nazivKolone + "\n" +
                            //                "tipVarijable.Tip=" + tipVarijable.Tip + "\n" +
                            //                "tipVarijable.Size=" + tipVarijable.Size.ToString());

                            column = new System.Data.DataColumn();
                            column.ColumnName = nazivKolone;

                            if (tipVarijable.Tip == "STRING")
                                column.DataType = typeof(System.String);
                            else if (tipVarijable.Tip == "INTEGER")
                                column.DataType = typeof(System.Int32);
                            else if (tipVarijable.Tip == "DOUBLE")
                                column.DataType = typeof(System.Double);
                            else if (tipVarijable.Tip == "DATETIME")
                                column.DataType = typeof(System.DateTime);
                            else if (tipVarijable.Tip == "BOOLEAN")
                                column.DataType = typeof(System.Boolean);

                            column.MaxLength = tipVarijable.Size;

                            columnExists = report.TablePodaci.Columns.Contains(nazivKolone);
                            if (columnExists == false)
                            {
                                report.TablePodaci.Columns.Add(column);
                            }
                        }



                    }
                }

                //dodaj kolone za DBMemo-e
                DBMemoParametri dBMemoParametri;
                for (i = 0; i < report.MyListKomponente.Count; i++)
                {
                    komponenta = report.MyListKomponente[i];

                    if (komponenta.ClassName == "TppDBMemo")
                    {
                        dBMemoParametri = komponenta.parametriKomponente as DBMemoParametri;
                        nazivKolone = dBMemoParametri.DataField;

                        if (Gui.DEFINES.TryGetValue(nazivKolone.ToLower(), out DefineVariable defVar))
                        {
                            tipVarijable = new TipVarijable();

                            switch (defVar.DefType)
                            {
                                case "a":
                                    tipVarijable.Tip = "STRING";
                                    tipVarijable.Size = defVar.Size;
                                    break;

                                case "b":
                                case "i":
                                case "r":
                                    tipVarijable.Tip = "INTEGER";
                                    tipVarijable.Size = -1;
                                    break;

                                case "n":
                                    tipVarijable.Tip = "DOUBLE";
                                    tipVarijable.Size = -1;
                                    break;

                                case "d":
                                case "t":
                                    tipVarijable.Tip = "DATETIME";
                                    tipVarijable.Size = -1;
                                    break;

                                case "l":
                                    tipVarijable.Tip = "BOOLEAN";
                                    tipVarijable.Size = -1;
                                    break;

                                default:
                                    break;
                            }

                            //MessageBox.Show("nazivKolone=" + nazivKolone + "\n" +
                            //                "tipVarijable.Tip=" + tipVarijable.Tip + "\n" +
                            //                "tipVarijable.Size=" + tipVarijable.Size.ToString());

                            column = new System.Data.DataColumn();
                            column.ColumnName = nazivKolone;

                            if (tipVarijable.Tip == "STRING")
                                column.DataType = typeof(System.String);
                            else if (tipVarijable.Tip == "INTEGER")
                                column.DataType = typeof(System.Int32);
                            else if (tipVarijable.Tip == "DOUBLE")
                                column.DataType = typeof(System.Double);
                            else if (tipVarijable.Tip == "DATETIME")
                                column.DataType = typeof(System.DateTime);
                            else if (tipVarijable.Tip == "BOOLEAN")
                                column.DataType = typeof(System.Boolean);

                            column.MaxLength = tipVarijable.Size;

                            columnExists = report.TablePodaci.Columns.Contains(nazivKolone);
                            if (columnExists == false)
                            {
                                report.TablePodaci.Columns.Add(column);
                            }
                        }



                    }
                }

                //dodaj kolone za DBRichText-ove
                DBRichTextParametri dBRichTextParametri;
                for (i = 0; i < report.MyListKomponente.Count; i++)
                {
                    komponenta = report.MyListKomponente[i];

                    if (komponenta.ClassName == "TppDBRichText")
                    {
                        dBRichTextParametri = komponenta.parametriKomponente as DBRichTextParametri;
                        nazivKolone = dBRichTextParametri.DataField;

                        if (Gui.DEFINES.TryGetValue(nazivKolone.ToLower(), out DefineVariable defVar))
                        {
                            tipVarijable = new TipVarijable();

                            switch (defVar.DefType)
                            {
                                case "a":
                                    tipVarijable.Tip = "STRING";
                                    tipVarijable.Size = defVar.Size;
                                    break;

                                case "b":
                                case "i":
                                case "r":
                                    tipVarijable.Tip = "INTEGER";
                                    tipVarijable.Size = -1;
                                    break;

                                case "n":
                                    tipVarijable.Tip = "DOUBLE";
                                    tipVarijable.Size = -1;
                                    break;

                                case "d":
                                case "t":
                                    tipVarijable.Tip = "DATETIME";
                                    tipVarijable.Size = -1;
                                    break;

                                case "l":
                                    tipVarijable.Tip = "BOOLEAN";
                                    tipVarijable.Size = -1;
                                    break;

                                default:
                                    break;
                            }

                            //MessageBox.Show("nazivKolone=" + nazivKolone + "\n" +
                            //                "tipVarijable.Tip=" + tipVarijable.Tip + "\n" +
                            //                "tipVarijable.Size=" + tipVarijable.Size.ToString());

                            column = new System.Data.DataColumn();
                            column.ColumnName = nazivKolone;

                            if (tipVarijable.Tip == "STRING")
                                column.DataType = typeof(System.String);
                            else if (tipVarijable.Tip == "INTEGER")
                                column.DataType = typeof(System.Int32);
                            else if (tipVarijable.Tip == "DOUBLE")
                                column.DataType = typeof(System.Double);
                            else if (tipVarijable.Tip == "DATETIME")
                                column.DataType = typeof(System.DateTime);
                            else if (tipVarijable.Tip == "BOOLEAN")
                                column.DataType = typeof(System.Boolean);

                            column.MaxLength = tipVarijable.Size;

                            columnExists = report.TablePodaci.Columns.Contains(nazivKolone);
                            if (columnExists == false)
                            {
                                report.TablePodaci.Columns.Add(column);
                            }
                        }



                    }
                }

                //dodaj kolone za DBImage-e
                DBImageParametri dBImageParametri;
                for (i = 0; i < report.MyListKomponente.Count; i++)
                {
                    komponenta = report.MyListKomponente[i];

                    if (komponenta.ClassName == "TppDBImage")
                    {
                        dBImageParametri = komponenta.parametriKomponente as DBImageParametri;
                        nazivKolone = dBImageParametri.DataField;

                        if (Gui.DEFINES.TryGetValue(nazivKolone.ToLower(), out DefineVariable defVar))
                        {
                            tipVarijable = new TipVarijable();

                            switch (defVar.DefType)
                            {
                                case "a":
                                    tipVarijable.Tip = "STRING";
                                    tipVarijable.Size = defVar.Size;
                                    break;

                                default:
                                    break;
                            }

                            //MessageBox.Show("nazivKolone=" + nazivKolone + "\n" +
                            //                "tipVarijable.Tip=" + tipVarijable.Tip + "\n" +
                            //                "tipVarijable.Size=" + tipVarijable.Size.ToString());

                            column = new System.Data.DataColumn();
                            column.ColumnName = nazivKolone;

                            if (tipVarijable.Tip == "STRING")
                                column.DataType = typeof(System.String);

                            column.MaxLength = tipVarijable.Size;

                            columnExists = report.TablePodaci.Columns.Contains(nazivKolone);
                            if (columnExists == false)
                            {
                                report.TablePodaci.Columns.Add(column);
                            }
                        }



                    }
                }

                //dodaj kolone za DBCheckBox-ove
                DBCheckBoxParametri dBCheckBoxParametri;
                for (i = 0; i < report.MyListKomponente.Count; i++)
                {
                    komponenta = report.MyListKomponente[i];

                    if (komponenta.ClassName == "TmyDBCheckBox")
                    {
                        dBCheckBoxParametri = komponenta.parametriKomponente as DBCheckBoxParametri;
                        nazivKolone = dBCheckBoxParametri.DataField;

                        if (Gui.DEFINES.TryGetValue(nazivKolone.ToLower(), out DefineVariable defVar))
                        {
                            tipVarijable = new TipVarijable();

                            switch (defVar.DefType)
                            {
                                case "l":
                                    tipVarijable.Tip = "BOOLEAN";
                                    tipVarijable.Size = -1;
                                    break;

                                default:
                                    break;
                            }

                            //MessageBox.Show("nazivKolone=" + nazivKolone + "\n" +
                            //                "tipVarijable.Tip=" + tipVarijable.Tip + "\n" +
                            //                "tipVarijable.Size=" + tipVarijable.Size.ToString());

                            column = new System.Data.DataColumn();
                            column.ColumnName = nazivKolone;

                            if (tipVarijable.Tip == "BOOLEAN")
                                column.DataType = typeof(System.Boolean);

                            column.MaxLength = tipVarijable.Size;

                            columnExists = report.TablePodaci.Columns.Contains(nazivKolone);
                            if (columnExists == false)
                            {
                                report.TablePodaci.Columns.Add(column);
                            }
                        }



                    }
                }
                #endregion

                for (i = 0; i < report.TablePodaci.Columns.Count; i++)
                {
                    //MessageBox.Show("Kolona " + i.ToString() + " " + report.TablePodaci.Columns[i].ColumnName);
                }


                #region PODACI IZ CSCS U TablePodaci
                int brojRedaka;
                brojRedaka = report.TablePodaci.Rows.Count;

                dataRow = report.TablePodaci.NewRow();

                dataRow.BeginEdit();

                //IDRow kolona - specijalna kolona
                dataRow["IDRow"] = brojRedaka;

                if (report.parentHandle == 0)
                {
                    //MAIN REPORT
                    dataRow["IDParentRow"] = 0;
                }
                else
                {
                    //ANY OTHER REPORT
                    Report reportParent = MyDictionary.Reports[report.parentHandle];
                    int brojRedakaParent = reportParent.TablePodaci.Rows.Count;
                    dataRow["IDParentRow"] = brojRedakaParent - 1;
                }

                //for po kolonama iz tablice
                for (i = 0; i < report.TablePodaci.Columns.Count; i++)
                {
                    column = report.TablePodaci.Columns[i];

                    if (column.DataType == typeof(System.String))
                    {
                        if (Gui.DEFINES.TryGetValue(column.ColumnName.ToLower(), out DefineVariable defVar))
                        {
                            dataRow[column.ColumnName] = defVar.AsString();
                        }
                    }
                    else if (column.DataType == typeof(System.Int32))
                    {
                        if (Gui.DEFINES.TryGetValue(column.ColumnName.ToLower(), out DefineVariable defVar))
                        {
                            dataRow[column.ColumnName] = defVar.AsInt();
                        }
                    }
                    else if (column.DataType == typeof(System.Double))
                    {
                        if (Gui.DEFINES.TryGetValue(column.ColumnName.ToLower(), out DefineVariable defVar))
                        {
                            dataRow[column.ColumnName] = defVar.AsDouble();
                        }
                    }
                    else if (column.DataType == typeof(System.DateTime))
                    {
                        if (Gui.DEFINES.TryGetValue(column.ColumnName.ToLower(), out DefineVariable defVar))
                        {
                            dataRow[column.ColumnName] = defVar.AsDateTime();
                        }
                    }
                    else if (column.DataType == typeof(System.Boolean))
                    {
                        if (Gui.DEFINES.TryGetValue(column.ColumnName.ToLower(), out DefineVariable defVar))
                        {
                            dataRow[column.ColumnName] = defVar.AsBool();
                        }
                    }
                }

                dataRow.EndEdit();

                report.TablePodaci.Rows.Add(dataRow);

                #endregion

                //ISPIS
                for (i = 0; i < report.TablePodaci.Rows.Count; i++)
                {
                    //MessageBox.Show("IDRacuna=" + report.TablePodaci.Rows[i]["IDRacuna"].ToString() + "\n" +
                    //    "Kupac=" + report.TablePodaci.Rows[i]["Kupac"] + "\n" +
                    //    "BooleanX=" + report.TablePodaci.Rows[i]["BooleanX"].ToString() + "\n" +
                    //    "Datum=" + report.TablePodaci.Rows[i]["Datum"].ToString() + "\n" +
                    //    "TOTAL=" + report.TablePodaci.Rows[i]["TOTAL"].ToString());
                }
            }
        }

        class UpdateReportFunction : ParserFunction
        {
            CSCS_GUI Gui;
            private void UpdateReport(int reportHandle, string variableList)
            {
                try
                {
                    string variableList2 = variableList.Replace(" ", "");

                    string[] variableList3 = variableList2.Split(',');

                    int i;
                    string s = "";
                    for (i = 0; i < variableList3.Length; i++)
                    {
                        s += variableList3[i] + "\n";
                    }
                    //MessageBox.Show("VARIJABLE:\n" + s);
                    ////MessageBox.Show(variableList3.Length.ToString());

                    string variable;

                    for (i = 0; i < variableList3.Length; i++)
                    {
                        variable = variableList3[i];

                        Update1Varijabla(reportHandle, variable);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("UPDATE REPORT\n" + ex.GetType() + "\n" + ex.Message);
                }
            }

            protected override Variable Evaluate(ParsingScript script)
            {
                //MessageBox.Show("AURA_UpdateReportFunction");

                Gui = CSCS_GUI.GetInstance(script);

                List<Variable> args = script.GetFunctionArgs();

                Utils.CheckArgs(args.Count, 2, m_name);

                var reportHandle = Utils.GetSafeInt(args, 0);
                var variableList = Utils.GetSafeString(args, 1);

                UpdateReport(reportHandle, variableList);

                //return Variable.EmptyInstance;
                return null;
            }

            private void Update1Varijabla(int reportHandle, string variable)
            {
                Report report = MyDictionary.Reports[reportHandle];

                bool b;
                b = Gui.DEFINES.TryGetValue(variable.ToLower(), out DefineVariable defVar);
                //MessageBox.Show("Našao=" + b.ToString() + " Varijabla=" + variable.ToLower());
                if (b == true)
                {

                    #region AKO JE LABEL U PITANJU PODESI CAPTION
                    Komponenta komponenta = report.GetKomponentaByName(variable);//ppLabelNaslov
                    if (komponenta != null)
                    {
                        if (komponenta.ClassName == "TppLabel")
                        {
                            //LabelParametri labelParametri = new LabelParametri();
                            //labelParametri.Caption = defVar.AsString(); //"NASLOV"
                            //labelParametri.CaptionChanged = true;
                            //komponenta.parametriKomponente = labelParametri;

                            LabelParametri labelParametri;
                            if (komponenta.parametriKomponente == null)
                            {
                                labelParametri = new LabelParametri();
                            }
                            else
                            {
                                labelParametri = (LabelParametri)komponenta.parametriKomponente;
                            }
                            labelParametri.Caption = defVar.AsString(); //"NASLOV"
                            MessageBox.Show("NOVI labelParametri.Caption=" + labelParametri.Caption);
                            labelParametri.CaptionChanged = true;
                            komponenta.parametriKomponente = labelParametri;
                        }
                        return;
                    }
                    #endregion


                    #region AKO JE DBText, DBMemo, DBRichText, DBImage, DBCheckBox U PITANJU DODAJ KOLONU I AŽURIRAJ ZADNJI RED DATASETA
                    //TU IDE VARIJABLA U DICTIONARY ???JOŠ NI TAMO SPREMLJENA
                    UpdateVariable updateVariableTemp = new UpdateVariable();
                    updateVariableTemp.Name = variable.ToLower();

                    #region TIP i VALUE i SIZE
                    switch (defVar.DefType)
                    {
                        case "a":
                            updateVariableTemp.Tip = "STRING";
                            updateVariableTemp.ValueString = defVar.AsString();
                            updateVariableTemp.Size = defVar.Size;
                            break;

                        case "b":
                        case "i":
                        case "r":
                            updateVariableTemp.Tip = "INTEGER";
                            updateVariableTemp.ValueInt = defVar.AsInt();
                            updateVariableTemp.Size = -1;
                            break;

                        case "n":
                            updateVariableTemp.Tip = "DOUBLE";
                            updateVariableTemp.ValueDouble = defVar.AsDouble();
                            updateVariableTemp.Size = -1;
                            break;

                        case "d":
                        case "t":
                            updateVariableTemp.Tip = "DATETIME";
                            updateVariableTemp.ValueDateTime = defVar.AsDateTime();
                            updateVariableTemp.Size = -1;
                            break;

                        case "l":
                            updateVariableTemp.Tip = "BOOLEAN";
                            updateVariableTemp.ValueBool = defVar.AsBool();
                            updateVariableTemp.Size = -1;
                            break;

                        default:
                            break;
                    }
                    #endregion

                    //MessageBox.Show("variable =" + variable + "\n" +
                    //                "tip= " + updateVariableTemp.Tip + "\n" +
                    //                "size= " + updateVariableTemp.Size.ToString() + "\n\n" +
                    //                "valueStr=" + updateVariableTemp.ValueString + "\n" +
                    //                "valueInt=" + updateVariableTemp.ValueInt.ToString() + "\n" +
                    //                "valueDouble=" + updateVariableTemp.ValueDouble.ToString() + "\n" +
                    //                "valueDateTime=" + updateVariableTemp.ValueDateTime.ToString() + "\n" +
                    //                "valueBool=" + updateVariableTemp.ValueBool.ToString() + "\n"
                    //                );



                    AzurirajVarijabluUDatasetu(report, updateVariableTemp);
                    #endregion
                }
            }
            private void AzurirajVarijabluUDatasetu(Report report, UpdateVariable updateVariableTemp)
            {
                //ako nema redaka, return
                if (report.TablePodaci.Rows.Count == 0)
                {
                    //MessageBox.Show("Najprije pozovite OUTPUT a onda UPDATE.\n" + "Varijabla: " + updateVariableTemp.Name);
                    return;
                }

                //da li ima već ta kolona,
                //ako nema dodaj kolonu
                bool kolonaPostoji = report.TablePodaci.Columns.Contains(updateVariableTemp.Name);

                //MessageBox.Show("TablePodaci " + 
                //                "\nUpdateVariableTemp.Name = " + updateVariableTemp.Name +
                //                "\nUpdateVariableTemp.Tip = " + updateVariableTemp.Tip +
                //                "\nUpdateVariableTemp.Size = " + updateVariableTemp.Size +
                //                "\nkolonaPostoji =" + kolonaPostoji.ToString());

                if (kolonaPostoji == false)
                {
                    DataColumn column = new DataColumn();
                    column.ColumnName = updateVariableTemp.Name;

                    if (updateVariableTemp.Tip == "STRING")
                    {
                        column.DataType = typeof(System.String);
                        column.MaxLength = updateVariableTemp.Size;
                    }
                    else if (updateVariableTemp.Tip == "INTEGER")
                    {
                        column.DataType = typeof(System.Int32);
                        column.MaxLength = -1;
                    }
                    else if (updateVariableTemp.Tip == "DOUBLE")
                    {
                        column.DataType = typeof(System.Double);
                        column.MaxLength = -1;
                    }
                    else if (updateVariableTemp.Tip == "DATETIME")
                    {
                        column.DataType = typeof(System.DateTime);
                        column.MaxLength = -1;
                    }
                    else if (updateVariableTemp.Tip == "BOOLEAN")
                    {
                        column.DataType = typeof(System.Boolean);
                        column.MaxLength = -1;
                    }

                    report.TablePodaci.Columns.Add(column);
                }

                //azuriraj zadnji redak
                int indexZadnjegRetka = report.TablePodaci.Rows.Count - 1;
                DataRow row = report.TablePodaci.Rows[indexZadnjegRetka];

                if (updateVariableTemp.Tip == "STRING")
                {
                    row[updateVariableTemp.Name] = updateVariableTemp.ValueString;
                }
                else if (updateVariableTemp.Tip == "INTEGER")
                {
                    row[updateVariableTemp.Name] = updateVariableTemp.ValueInt;
                }
                else if (updateVariableTemp.Tip == "DOUBLE")
                {
                    row[updateVariableTemp.Name] = updateVariableTemp.ValueDouble;
                }
                else if (updateVariableTemp.Tip == "DATETIME")
                {
                    row[updateVariableTemp.Name] = updateVariableTemp.ValueDateTime;
                }
                else if (updateVariableTemp.Tip == "BOOLEAN")
                {
                    row[updateVariableTemp.Name] = updateVariableTemp.ValueBool;
                }

            }
        }

        class PrintReportFunction : ParserFunction
        {
            CSCS_GUI Gui;
            private void PrintReport(int reportHandle)
            {
                Report report = MyDictionary.Reports[reportHandle];

                //ako je MAINREPORT u pitanju
                if (report.IsMainReport == false)
                {
                    MessageBox.Show("Report koji se printa mora biti Main report a ne Subreport.");
                    return;
                }

                PrebaciLabelCaptioneUDLL(reportHandle);
                CopyLabelParametriToReport(reportHandle);

                //PrebaciLabelCaptioneUDLL(2);
                //CopyLabelParametriToReport(2);

                //AURA_Test.IspisKomponenteUMessageBox();
                //AURA_Test.IspisLabelaUMessageBox();
                //AURA_Test.IspisDBTextovaUMessageBox();
                //AURA_Test.IspisDBMemoaUMessageBox();

                PrebaciTipovePodatakaUDLL(reportHandle);
                CreateDatasetPodaci(reportHandle);
                PrebaciPodatkeUDatasetPodaci(reportHandle);
                PipelineON(reportHandle);
                //FilteringON(1);//TO SE NE SMIJE POZVATI

                PrebaciTipovePodatakaUDLL(2);
                CreateDatasetPodaci(2);
                PrebaciPodatkeUDatasetPodaci(2);
                PipelineON(2);
                FilteringON(2);

                PrebaciTipovePodatakaUDLL(3);
                CreateDatasetPodaci(3);
                PrebaciPodatkeUDatasetPodaci(3);
                PipelineON(3);
                FilteringON(3);

                PrebaciTipovePodatakaUDLL(4);
                CreateDatasetPodaci(4);
                PrebaciPodatkeUDatasetPodaci(4);
                PipelineON(4);
                FilteringON(4);

                //Report report = MyDictionary.Reports[2];            
                //AURA_Test.ProvjeraTablice(report.TablePodaci, "EXE ReportHandle = 2", "TABLEPODACI");

                //ReportBuilderDll.DLLProba(1);
                //ReportBuilderDll.DLLProba(2);            
                //ReportBuilderDll.DLLProba(3);
                //ReportBuilderDll.DLLProba(4);

                PrebaciParametreUDatasetOstaliParametri(1);
                PrebaciParametreUDatasetOstaliParametri(2);
                ReportBuilderDll.DLLPridruziEventeZaOstaliParametri(1);
                ReportBuilderDll.DLLPridruziEventeZaOstaliParametri(2);



                //report = MyDictionary.Reports[1];
                //AURA_Test.ProvjeraTablice(report.TableOstaliParametri, "EXE ReportHandle = 1 TableOstaliParametri", "TABLEOSTALIPARAMETRI");

                UInt32 x;
                x = ReportBuilderDll.DLLProvjeraTabliceOstaliParametri(1);
                x = ReportBuilderDll.DLLProvjeraTabliceOstaliParametri(2);

                x = ReportBuilderDll.DLLPrintReport();
            }

            protected override Variable Evaluate(ParsingScript script)
            {
                //MessageBox.Show("AURA_PrintReportFunction");
                Gui = CSCS_GUI.GetInstance(script);

                List<Variable> args = script.GetFunctionArgs();

                Utils.CheckArgs(args.Count, 1, m_name);

                var reportHandle = Utils.GetSafeInt(args, 0);

                PrintReport(reportHandle);

                //return Variable.EmptyInstance;
                return null;
            }

            private void PrebaciLabelCaptioneUDLL(int reportHandle)
            {
                Report report = MyDictionary.Reports[reportHandle];

                Komponenta komponenta;
                LabelParametri labelParametri;

                //iz C# u DLL
                Object obj;
                int i;
                for (i = 0; i < report.MyListKomponente.Count; i++)
                {
                    komponenta = report.MyListKomponente[i];
                    if (komponenta.ClassName == "TppLabel")
                    {
                        obj = komponenta.parametriKomponente;

                        labelParametri = obj as LabelParametri;

                        if (labelParametri.CaptionChanged == true)
                        {
                            ReportBuilderDll.DLLSetLabelCaption_(reportHandle, i, labelParametri.Caption); //U DLL
                        }
                    }
                }
            }
            private void CopyLabelParametriToReport(int reportHandle)
            {
                ReportBuilderDll.DLLCopyLabelParametriToReport(reportHandle); //Iz DLL-a u REPORT
            }


            /// <summary>        
            /// Iz CSCS u C#
            /// Iz CSCS u DLL
            /// </summary>
            private void PrebaciTipovePodatakaUDLL(int reportHandle)
            {
                //int reportHandle = 1;
                Report report = MyDictionary.Reports[reportHandle];
                Komponenta komponenta;
                TipVarijable tipVarijable;
                DBTextParametri dBTextParametri;
                DBMemoParametri dBMemoParametri;
                DBRichTextParametri dBRichTextParametri;
                DBImageParametri dBImageParametri;
                DBCheckBoxParametri dBCheckBoxParametri;
                UInt32 x;

                int i;

                //po DBTextovima u C#
                for (i = 0; i < report.MyListKomponente.Count; i++)
                {
                    komponenta = report.MyListKomponente[i];

                    if (komponenta.ClassName == "TppDBText")
                    {
                        dBTextParametri = komponenta.parametriKomponente as DBTextParametri;

                        if (Gui.DEFINES.TryGetValue(dBTextParametri.DataField.ToLower(), out DefineVariable defVar))
                        {
                            //komponenta.PostojiVarijabla = true;

                            tipVarijable = new TipVarijable();

                            switch (defVar.DefType)
                            {
                                case "a":
                                    tipVarijable.Tip = "STRING";
                                    tipVarijable.Size = defVar.Size;
                                    break;

                                case "b":
                                case "i":
                                case "r":
                                    tipVarijable.Tip = "INTEGER";
                                    tipVarijable.Size = -1;
                                    break;

                                case "n":
                                    tipVarijable.Tip = "DOUBLE";
                                    tipVarijable.Size = -1;
                                    break;

                                case "d":
                                case "t":
                                    tipVarijable.Tip = "DATETIME";
                                    tipVarijable.Size = -1;
                                    break;

                                case "l":
                                    tipVarijable.Tip = "BOOLEAN";
                                    tipVarijable.Size = -1;
                                    break;

                                default:
                                    break;
                            }

                            dBTextParametri.Tip = tipVarijable.Tip;
                            dBTextParametri.TipChanged = true;
                            dBTextParametri.Size = tipVarijable.Size;
                            dBTextParametri.SizeChanged = true;

                            x = ReportBuilderDll.DLLSetDBTextType_(reportHandle, i, tipVarijable.Tip, tipVarijable.Size);
                        }
                    }
                }

                //po DBMemoima u C#
                for (i = 0; i < report.MyListKomponente.Count; i++)
                {
                    komponenta = report.MyListKomponente[i];

                    if (komponenta.ClassName == "TppDBMemo")
                    {
                        dBMemoParametri = komponenta.parametriKomponente as DBMemoParametri;

                        if (Gui.DEFINES.TryGetValue(dBMemoParametri.DataField.ToLower(), out DefineVariable defVar))
                        {
                            //komponenta.PostojiVarijabla = true;

                            tipVarijable = new TipVarijable();

                            switch (defVar.DefType)
                            {
                                case "a":
                                    tipVarijable.Tip = "STRING";
                                    tipVarijable.Size = defVar.Size;
                                    break;

                                case "b":
                                case "i":
                                case "r":
                                    tipVarijable.Tip = "INTEGER";
                                    tipVarijable.Size = -1;
                                    break;

                                case "n":
                                    tipVarijable.Tip = "DOUBLE";
                                    tipVarijable.Size = -1;
                                    break;

                                case "d":
                                case "t":
                                    tipVarijable.Tip = "DATETIME";
                                    tipVarijable.Size = -1;
                                    break;

                                case "l":
                                    tipVarijable.Tip = "BOOLEAN";
                                    tipVarijable.Size = -1;
                                    break;

                                default:
                                    break;
                            }

                            dBMemoParametri.Tip = tipVarijable.Tip;
                            dBMemoParametri.TipChanged = true;
                            dBMemoParametri.Size = tipVarijable.Size;
                            dBMemoParametri.SizeChanged = true;

                            x = ReportBuilderDll.DLLSetDBMemoType_(reportHandle, i, tipVarijable.Tip, tipVarijable.Size);
                        }
                    }
                }

                //po DBRichTextovima u C#
                for (i = 0; i < report.MyListKomponente.Count; i++)
                {
                    komponenta = report.MyListKomponente[i];

                    if (komponenta.ClassName == "TppDBRichText")
                    {
                        dBRichTextParametri = komponenta.parametriKomponente as DBRichTextParametri;

                        if (Gui.DEFINES.TryGetValue(dBRichTextParametri.DataField.ToLower(), out DefineVariable defVar))
                        {
                            //komponenta.PostojiVarijabla = true;

                            tipVarijable = new TipVarijable();

                            switch (defVar.DefType)
                            {
                                case "a":
                                    tipVarijable.Tip = "STRING";
                                    tipVarijable.Size = defVar.Size;
                                    break;

                                case "b":
                                case "i":
                                case "r":
                                    tipVarijable.Tip = "INTEGER";
                                    tipVarijable.Size = -1;
                                    break;

                                case "n":
                                    tipVarijable.Tip = "DOUBLE";
                                    tipVarijable.Size = -1;
                                    break;

                                case "d":
                                case "t":
                                    tipVarijable.Tip = "DATETIME";
                                    tipVarijable.Size = -1;
                                    break;

                                case "l":
                                    tipVarijable.Tip = "BOOLEAN";
                                    tipVarijable.Size = -1;
                                    break;

                                default:
                                    break;
                            }

                            dBRichTextParametri.Tip = tipVarijable.Tip;
                            dBRichTextParametri.TipChanged = true;
                            dBRichTextParametri.Size = tipVarijable.Size;
                            dBRichTextParametri.SizeChanged = true;

                            x = ReportBuilderDll.DLLSetDBRichTextType_(reportHandle, i, tipVarijable.Tip, tipVarijable.Size);
                        }
                    }
                }


                //po DBImage-ima u C#
                for (i = 0; i < report.MyListKomponente.Count; i++)
                {
                    komponenta = report.MyListKomponente[i];

                    if (komponenta.ClassName == "TppDBImage")
                    {
                        dBImageParametri = komponenta.parametriKomponente as DBImageParametri;

                        if (Gui.DEFINES.TryGetValue(dBImageParametri.DataField.ToLower(), out DefineVariable defVar))
                        {
                            //komponenta.PostojiVarijabla = true;

                            tipVarijable = new TipVarijable();

                            switch (defVar.DefType)
                            {
                                case "a":
                                    tipVarijable.Tip = "STRING";
                                    tipVarijable.Size = defVar.Size;
                                    break;

                                default:
                                    break;
                            }

                            dBImageParametri.Tip = tipVarijable.Tip;
                            dBImageParametri.TipChanged = true;
                            dBImageParametri.Size = tipVarijable.Size;
                            dBImageParametri.SizeChanged = true;

                            x = ReportBuilderDll.DLLSetDBImageType_(reportHandle, i, tipVarijable.Tip, tipVarijable.Size);
                        }
                    }
                }

                //po DBCheckBoxevima u C#
                for (i = 0; i < report.MyListKomponente.Count; i++)
                {
                    komponenta = report.MyListKomponente[i];

                    if (komponenta.ClassName == "TmyDBCheckBox")
                    {
                        dBCheckBoxParametri = komponenta.parametriKomponente as DBCheckBoxParametri;

                        if (Gui.DEFINES.TryGetValue(dBCheckBoxParametri.DataField.ToLower(), out DefineVariable defVar))
                        {
                            //komponenta.PostojiVarijabla = true;

                            tipVarijable = new TipVarijable();

                            switch (defVar.DefType)
                            {
                                case "l":
                                    tipVarijable.Tip = "BOOLEAN";
                                    tipVarijable.Size = -1;
                                    break;

                                default:
                                    break;
                            }

                            dBCheckBoxParametri.Tip = tipVarijable.Tip;
                            dBCheckBoxParametri.TipChanged = true;
                            dBCheckBoxParametri.Size = tipVarijable.Size;
                            dBCheckBoxParametri.SizeChanged = true;

                            x = ReportBuilderDll.DLLSetDBCheckBoxType_(reportHandle, i, tipVarijable.Tip, tipVarijable.Size);
                        }
                    }
                }
            }

            private void CreateDatasetPodaci(int reportHandle)
            {
                UInt32 x = ReportBuilderDll.DLLCreateDatasetPodaci(reportHandle);
            }

            private void PrebaciPodatkeUDatasetPodaci(int reportHandle)
            {
                //Prebaci podatke u dataset Podaci
                Report report = MyDictionary.Reports[reportHandle];
                int i;
                int j;
                string columnName = "";
                Type columnType;

                string valueString = "";
                int valueInt;
                double valueDouble;
                DateTime valueDateTime;
                bool valueBool;

                UInt32 x;

                for (i = 0; i < report.TablePodaci.Rows.Count; i++)
                {
                    DataRow row = report.TablePodaci.Rows[i];

                    x = ReportBuilderDll.DLLInitNewRowInDataTablePodaci(reportHandle);

                    for (j = 0; j < report.TablePodaci.Columns.Count; j++)
                    {
                        columnName = report.TablePodaci.Columns[j].ColumnName;
                        columnType = report.TablePodaci.Columns[j].DataType;

                        if (columnType == typeof(System.String))
                        {
                            if (row[j] != System.DBNull.Value)
                            {
                                valueString = (string)row[j];
                            }
                            else
                            {
                                valueString = "";
                            }

                            x = ReportBuilderDll.DLLSetCellValueStringPodaci_(reportHandle, columnName, valueString);
                        }
                        else if (columnType == typeof(System.Int32))
                        {
                            if (row[j] != System.DBNull.Value)
                            {
                                valueInt = (int)row[j];
                            }
                            else
                            {
                                valueInt = 0;
                            }

                            x = ReportBuilderDll.DLLSetCellValueIntegerPodaci_(reportHandle, columnName, valueInt);
                        }
                        else if (columnType == typeof(System.Double))
                        {
                            if (row[j] != System.DBNull.Value)
                            {
                                valueDouble = (double)row[j];
                            }
                            else
                            {
                                valueDouble = 0;
                            }

                            x = ReportBuilderDll.DLLSetCellValueDoublePodaci_(reportHandle, columnName, valueDouble);
                        }
                        else if (columnType == typeof(System.DateTime))
                        {
                            if (row[j] != System.DBNull.Value)
                            {
                                valueDateTime = (DateTime)row[j];
                            }
                            else
                            {
                                valueDateTime = new DateTime(1800, 1, 1);
                            }

                            x = ReportBuilderDll.DLLSetCellValueDateTimePodaci_(reportHandle, columnName, valueDateTime);
                        }
                        else if (columnType == typeof(System.Boolean))
                        {
                            if (row[j] != System.DBNull.Value)
                            {
                                valueBool = (bool)row[j];
                            }
                            else
                            {
                                valueBool = false;
                            }

                            x = ReportBuilderDll.DLLSetCellValueBooleanPodaci_(reportHandle, columnName, valueBool);
                        }
                    }

                    x = ReportBuilderDll.DLLWriteRowInDataTablePodaci(reportHandle);
                }

                x = ReportBuilderDll.DLLProvjeraTablicePodaci(reportHandle);//DLL Podaci tablica


            }

            private UInt32 PipelineON(int reportHandle)
            {
                UInt32 x;
                x = ReportBuilderDll.DLLPipelineON(reportHandle);
                return x;
            }

            private UInt32 FilteringON(int reportHandle)
            {
                UInt32 x;
                x = ReportBuilderDll.DLLFilteringON(reportHandle);
                return x;
            }


            private void PrebaciParametreUDatasetOstaliParametri(int reportHandle)
            {
                //Prebaci podatke u dataset Podaci
                Report report = MyDictionary.Reports[reportHandle];
                int i;
                //int j;
                //string columnName = "";
                //Type columnType;

                //string valueString = "";
                //int valueInt;
                //double valueDouble;
                //DateTime valueDateTime;
                //bool valueBool;

                UInt32 x;

                for (i = 0; i < report.TableOstaliParametri.Rows.Count; i++)
                {
                    DataRow row = report.TableOstaliParametri.Rows[i];

                    x = ReportBuilderDll.DLLUbaciRedakUTableOstaliParametri_(reportHandle,
                                                                     (int)row["IDRow"],
                                                                     (int)row["IDParentRow"],
                                                                     (string)row["NazivKontrole"],
                                                                     (string)row["NazivParametra"],
                                                                     (string)row["ValueString"],
                                                                     (int)row["ValueInt"],
                                                                     (double)row["ValueDouble"],
                                                                     (bool)row["ValueBoolean"]
                                                                     );
                }

                //x = ReportBuilderDll.DLLProvjeraTabliceOstaliParametri(reportHandle);//DLL OstaliParametri tablica


            }
        }


        public class SetParamReportFunction : ParserFunction
        {
            CSCS_GUI Gui;

            private void UbaciRedakUTableOstaliParametri(int reportHandle,
                                                         //int IDRow,
                                                         //int IDParentRow,
                                                         string nazivKontrole,
                                                         string nazivParametra,
                                                         string valueString,
                                                         int valueInt,
                                                         double valueDouble,
                                                         bool valueBoolean)
            {
                Report report = MyDictionary.Reports[reportHandle];

                int brojRedakaOstaliParametri;
                brojRedakaOstaliParametri = report.TableOstaliParametri.Rows.Count;
                int brojRedakaPodaci;
                brojRedakaPodaci = report.TablePodaci.Rows.Count;

                DataRow row = report.TableOstaliParametri.NewRow();

                row.BeginEdit();

                row["IDRow"] = brojRedakaOstaliParametri;
                row["IDParentRow"] = brojRedakaPodaci - 1;//ovo ide ako je SetParamReport nakon OutputReport
                                                          //row["IDParentRow"] = brojRedakaPodaci;      //ovo ide ako je SetParamReport prije OutputReport
                row["NazivKontrole"] = nazivKontrole;
                row["NazivParametra"] = nazivParametra;
                row["ValueString"] = valueString;
                row["ValueInt"] = valueInt;
                row["ValueDouble"] = valueDouble;
                row["ValueBoolean"] = valueBoolean;

                row.EndEdit();

                report.TableOstaliParametri.Rows.Add(row);
            }

            protected override Variable Evaluate(ParsingScript script)
            {
                //MessageBox.Show("AURA_SetParamReportFunction");

                Gui = CSCS_GUI.GetInstance(script);

                List<Variable> args = script.GetFunctionArgs();

                Utils.CheckArgs(args.Count, 4, m_name);

                var reportHandle = Utils.GetSafeInt(args, 0);
                var nazivKontrole = Utils.GetSafeString(args, 1);
                var nazivParametra = Utils.GetSafeString(args, 2);
                //var tip = Utils.GetSafeString(args, 3);//moja linija koda

                //var tip = Utils.GetSafeVariable(args, 3).Type;

                /*
                if (tip == Variable.VarType.STRING)
                {
                    var valueString = Utils.GetSafeVariable(args, 3).String;
                    SetParamReportString(reportHandle, nazivWidgeta, nazivPropertya, valueString);
                }
                else if (tip == Variable.VarType.NUMBER)
                {
                    var valueNumber = Utils.GetSafeVariable(args, 3).Value;
                    SetParamReportInteger(reportHandle, nazivWidgeta, nazivPropertya, valueNumber);
                }
                */

                //RJEŠENJE: PO MOJE
                string tip;
                string nazivParametra2 = nazivParametra.ToLower();
                if (nazivParametra2 == "visible")
                {
                    tip = "BOOLEAN";
                }
                else if (nazivParametra2 == "autosize")
                {
                    tip = "BOOLEAN";
                }
                else if (nazivParametra2 == "left")
                {
                    tip = "DOUBLE";
                }
                else if (nazivParametra2 == "top")
                {
                    tip = "DOUBLE";
                }
                else if (nazivParametra2 == "width")
                {
                    tip = "DOUBLE";
                }
                else if (nazivParametra2 == "height")
                {
                    tip = "DOUBLE";
                }
                else if (nazivParametra2 == "backgroundcolor")
                {
                    tip = "STRING";
                }
                else if (nazivParametra2 == "fontcolor")
                {
                    tip = "STRING";
                }
                else if (nazivParametra2 == "fontname")
                {
                    tip = "STRING";
                }
                else if (nazivParametra2 == "fontstyle")
                {
                    tip = "STRING";
                }
                else if (nazivParametra2 == "fontsize")
                {
                    tip = "INTEGER";
                }
                else if (nazivParametra2 == "tag")
                {
                    tip = "INTEGER";
                }
                else if (nazivParametra2 == "caption")
                {
                    tip = "STRING";
                }
                else if (nazivParametra2 == "units")
                {
                    tip = "STRING";
                }
                else
                {
                    MessageBox.Show("EXE\n" + "SetParamReport\n" + "Parametar: " + nazivParametra + " nije podržan.");
                    return null;
                }

                string valueString = "";
                int valueInteger = 0;
                double valueDouble = 0;
                bool valueBoolean = false;

                //int brojRetka = 0;

                //NOVO
                if (tip == "STRING")
                {
                    valueString = Utils.GetSafeString(args, 3);

                    UbaciRedakUTableOstaliParametri(reportHandle,
                                                    nazivKontrole,
                                                    nazivParametra,
                                                    valueString,
                                                    valueInteger,
                                                    valueDouble,
                                                    valueBoolean);
                }
                else if (tip == "INTEGER")
                {
                    valueInteger = Utils.GetSafeInt(args, 3);

                    UbaciRedakUTableOstaliParametri(reportHandle,
                                                    nazivKontrole,
                                                    nazivParametra,
                                                    valueString,
                                                    valueInteger,
                                                    valueDouble,
                                                    valueBoolean);
                }
                else if (tip == "DOUBLE")
                {
                    valueDouble = Utils.GetSafeDouble(args, 3);

                    UbaciRedakUTableOstaliParametri(reportHandle,
                                                    nazivKontrole,
                                                    nazivParametra,
                                                    valueString,
                                                    valueInteger,
                                                    valueDouble,
                                                    valueBoolean);
                }
                else if (tip == "BOOLEAN")
                {
                    valueBoolean = args[3].AsBool();

                    UbaciRedakUTableOstaliParametri(reportHandle,
                                                    nazivKontrole,
                                                    nazivParametra,
                                                    valueString,
                                                    valueInteger,
                                                    valueDouble,
                                                    valueBoolean);
                }


                //return Variable.EmptyInstance;
                return null;
            }
        }

        public class GetParamReportFunction : ParserFunction
        {
            CSCS_GUI Gui;
            private void GetParamReportString(int reportHandle, string nazivKontrole, string nazivParametra, out string valueString)
            {
                valueString = "";

                try
                {
                    UInt32 x = ReportBuilderDll.DLLGetParamReportString_(reportHandle, nazivKontrole, nazivParametra, out valueString);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("GetParamReportString\n" + ex.GetType() + "\n" + ex.Message);
                }
            }
            private void GetParamReportInteger(int reportHandle, string nazivKontrole, string nazivParametra, out int valueInteger)
            {
                valueInteger = 0;
                //MessageBox.Show("EXE GetParamReportInteger");
                try
                {
                    UInt32 x = ReportBuilderDll.DLLGetParamReportInteger_(reportHandle, nazivKontrole, nazivParametra, out valueInteger);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("GetParamReportInteger\n" + ex.GetType() + "\n" + ex.Message);
                }
            }
            private void GetParamReportDouble(int reportHandle, string nazivKontrole, string nazivParametra, out double valueDouble)
            {
                valueDouble = 0.0;

                try
                {
                    UInt32 x = ReportBuilderDll.DLLGetParamReportDouble_(reportHandle, nazivKontrole, nazivParametra, out valueDouble);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("GetParamReportDouble\n" + ex.GetType() + "\n" + ex.Message);
                }
            }
            private void GetParamReportBoolean(int reportHandle, string nazivKontrole, string nazivParametra, out bool valueBoolean)
            {
                valueBoolean = false;

                try
                {
                    UInt32 x = ReportBuilderDll.DLLGetParamReportBoolean_(reportHandle, nazivKontrole, nazivParametra, out valueBoolean);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("GetParamReportBoolean\n" + ex.GetType() + "\n" + ex.Message);
                }
            }

            protected override Variable Evaluate(ParsingScript script)
            {
                //MessageBox.Show("EXE AURA_GetParamReportFunction");

                Gui = CSCS_GUI.GetInstance(script);

                List<Variable> args = script.GetFunctionArgs();

                Utils.CheckArgs(args.Count, 3, m_name);

                var reportHandle = Utils.GetSafeInt(args, 0);
                var nazivKontrole = Utils.GetSafeString(args, 1);
                var nazivParametra = Utils.GetSafeString(args, 2);

                //MessageBox.Show("EXE AURA_GetParamReportFunction\n" + nazivKontrole + "\n" + nazivParametra + "\n" + tip);

                string tip;
                string nazivParametra2 = nazivParametra.ToLower();
                if (nazivParametra2 == "visible")
                {
                    tip = "BOOLEAN";
                }
                else if (nazivParametra2 == "autosize")
                {
                    tip = "BOOLEAN";
                }
                else if (nazivParametra2 == "left")
                {
                    tip = "DOUBLE";
                }
                else if (nazivParametra2 == "top")
                {
                    tip = "DOUBLE";
                }
                else if (nazivParametra2 == "width")
                {
                    tip = "DOUBLE";
                }
                else if (nazivParametra2 == "height")
                {
                    tip = "DOUBLE";
                }
                else if (nazivParametra2 == "backgroundcolor")
                {
                    tip = "STRING";
                }
                else if (nazivParametra2 == "fontcolor")
                {
                    tip = "STRING";
                }
                else if (nazivParametra2 == "fontname")
                {
                    tip = "STRING";
                }
                else if (nazivParametra2 == "fontstyle")
                {
                    tip = "STRING";
                }
                else if (nazivParametra2 == "fontsize")
                {
                    tip = "INTEGER";
                }
                else if (nazivParametra2 == "tag")
                {
                    tip = "INTEGER";
                }
                else if (nazivParametra2 == "caption")
                {
                    tip = "STRING";
                }
                else if (nazivParametra2 == "units")
                {
                    tip = "STRING";
                }
                else
                {
                    MessageBox.Show("EXE\n" + "GetParamReport\n" + "Parametar: " + nazivParametra + " nije podržan.");
                    return null;
                }


                Variable variable = null;

                if (tip == "STRING")
                {
                    string valueString;

                    GetParamReportString(reportHandle, nazivKontrole, nazivParametra, out valueString);
                    //MessageBox.Show("EXE valueString=" + valueString);
                    variable = new Variable(valueString);
                }
                else if (tip == "INTEGER")
                {
                    int valueInteger;

                    GetParamReportInteger(reportHandle, nazivKontrole, nazivParametra, out valueInteger);

                    variable = new Variable(valueInteger);
                }
                else if (tip == "DOUBLE")
                {
                    double valueDouble;

                    GetParamReportDouble(reportHandle, nazivKontrole, nazivParametra, out valueDouble);

                    variable = new Variable(valueDouble);
                }
                else if (tip == "BOOLEAN")
                {
                    bool valueBoolean;

                    GetParamReportBoolean(reportHandle, nazivKontrole, nazivParametra, out valueBoolean);

                    variable = new Variable(valueBoolean);
                }



                //variable.va = "ABCtest";

                //return Variable.EmptyInstance;
                //return null;
                return variable;
            }
        }
    }
}
