using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCSCS.Reporting.ReportBuilderReporting
{
    public static class MyDictionary
    {
        public static string RTMFolderFilename = "";// N:\\RTM\\MyDLLReport7.RTM - Za MAINREPORT i SUBREPORT

        //Reports<ReportHandle, Report>
        public static Dictionary<int, Report> Reports = new Dictionary<int, Report>();

    }

    public class Report
    {
        public Report()
        {
            //Napravi kolone za TableOstaliParametri
            DataColumn column;

            column = new DataColumn();
            column.ColumnName = "IDRow";
            column.DataType = typeof(System.Int32);
            column.MaxLength = -1;
            this.TableOstaliParametri.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "IDParentRow";
            column.DataType = typeof(System.Int32);
            column.MaxLength = -1;
            this.TableOstaliParametri.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "NazivKontrole";
            column.DataType = typeof(System.String);
            column.MaxLength = 1000;
            this.TableOstaliParametri.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "NazivParametra";
            column.DataType = typeof(System.String);
            column.MaxLength = 1000;
            this.TableOstaliParametri.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "ValueString";
            column.DataType = typeof(System.String);
            column.MaxLength = 1000;
            this.TableOstaliParametri.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "ValueInt";
            column.DataType = typeof(System.Int32);
            column.MaxLength = -1;
            this.TableOstaliParametri.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "ValueDouble";
            column.DataType = typeof(System.Double);
            column.MaxLength = -1;
            this.TableOstaliParametri.Columns.Add(column);

            column = new DataColumn();
            column.ColumnName = "ValueBoolean";
            column.DataType = typeof(System.Boolean);
            column.MaxLength = -1;
            this.TableOstaliParametri.Columns.Add(column);

        }
        public List<Komponenta> MyListKomponente = new List<Komponenta>();

        public DataTable TablePodaci = new DataTable();
        public DataTable TableOstaliParametri = new DataTable();

        public string ReportName = ""; // MyDLLReport5 tj.MyDLLSUBReport5
        public bool IsMainReport = false;
        public int parentHandle = 0;

        public Komponenta GetKomponentaByName(string name)
        {
            Report report = this;

            Komponenta komponenta;
            int i;
            for (i = 0; i < report.MyListKomponente.Count; i++)
            {
                komponenta = report.MyListKomponente[i];

                if (komponenta.Name == name)
                {
                    return komponenta;
                }
            }
            return null;
        }
    }


    public class Komponenta
    {
        public Komponenta()
        {
        }

        public Komponenta(string name, string className)
        {
            this.Name = name;
            this.ClassName = className;
        }

        public string Name = "";
        public string ClassName = "";

        public OstaliParametri ostaliParametri = new OstaliParametri();//left, top, fontColor...
        public Object parametriKomponente = new Object();//LabelParametri, DBTextParametri...
    }


    public class TipVarijable
    {
        public TipVarijable()
        {
        }
        public TipVarijable(string tip, int size)
        {
            this.Tip = tip;
            this.Size = size;
        }

        public string Tip = "";
        public int Size = 0;
    }

    public class OstaliParametri
    {
        public OstaliParametri()
        {
        }


        private int left = 0;
        private int top = 0;
        private string backgroundColor = "RED";
        private int width = 0;
        private int height = 0;
        private string font = "";
        private string fontColor = "BLUE";
        private bool visible = true;

        public bool LeftChanged = false;
        public bool TopChanged = false;
        public bool BackgroundColorChanged = false;
        public bool WidthChanged = false;
        public bool HeightChanged = false;
        public bool FontChanged = false;
        public bool FontColorChanged = false;
        public bool VisibleChanged = false;



        /// <summary>
        /// Left 
        /// Extenzija LT
        /// </summary>
        public int Left
        {
            get
            {
                return left;
            }
            set
            {
                left = value;
            }
        }

        /// <summary>
        /// Top 
        /// Extenzija TOP        
        /// </summary>
        public int Top
        {
            get
            {
                return top;
            }
            set
            {
                top = value;
            }
        }

        /// <summary>
        /// BackgroundColor 
        /// Extenzija CL
        /// Moguće vrijednosti "[RED]", "[GREEN]", "[BLUE]"...
        /// </summary>
        public string BackgroundColor
        {
            get
            {
                return backgroundColor;
            }
            set
            {
                backgroundColor = value;
            }
        }

        /// <summary>
        /// Width 
        /// Extenzija WD        
        /// </summary>
        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        /// <summary>
        /// Height 
        /// Extenzija HT        
        /// </summary>
        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        /// <summary>
        /// Font 
        /// Extenzija FN.
        /// Moguće vrijednosti "Arial 16".
        /// </summary>
        public string Font
        {
            get
            {
                return font;
            }
            set
            {
                font = value;
            }
        }

        /// <summary>
        /// Font Color 
        /// Extenzija FC        
        /// </summary>
        public string FontColor
        {
            get
            {
                return fontColor;
            }
            set
            {
                fontColor = value;
            }
        }

        /// <summary>
        /// Visible 
        /// Extenzije nema.
        /// Moguće vrijednosti true,false.
        /// </summary>
        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
            }
        }

    }

    public class LabelParametri
    {
        public LabelParametri()
        {
        }

        public string Caption = "";
        public bool CaptionChanged = false;

    }

    public class MemoParametri
    {
        public MemoParametri()
        {
        }

        public string Caption = "";
        public bool CaptionChanged = false;
    }

    public class RichTextParametri
    {
        public RichTextParametri()
        {
        }

        public string RichText = "";
        public bool RichTextChanged = false;
    }

    public class ImageParametri
    {
        public ImageParametri()
        {
        }

        public string Picture = "";
        public bool PictureChanged = false;
    }

    public class DBTextParametri
    {
        public DBTextParametri()
        {
        }

        public string DataField = "";
        public bool DataFieldChanged = false;
        public string Tip = "";
        public bool TipChanged = false;
        public int Size = 0;
        public bool SizeChanged = false;
    }

    public class DBMemoParametri
    {
        public DBMemoParametri()
        {
        }

        public string DataField = "";
        public bool DataFieldChanged = false;
        public string Tip = "";
        public bool TipChanged = false;
        public int Size = 0;
        public bool SizeChanged = false;
    }

    public class DBRichTextParametri
    {
        public DBRichTextParametri()
        {
        }

        public string DataField = "";
        public bool DataFieldChanged = false;
        public string Tip = "";
        public bool TipChanged = false;
        public int Size = 0;
        public bool SizeChanged = false;
    }

    public class DBCalcParametri
    {
        public DBCalcParametri()
        {
        }

        public string DataField = "";
        public bool DataFieldChanged = false;
    }

    public class DBImageParametri
    {
        public DBImageParametri()
        {
        }

        public string DataField = "";
        public bool DataFieldChanged = false;
        public string Tip = "";
        public bool TipChanged = false;
        public int Size = 0;
        public bool SizeChanged = false;
    }

    public class DBCheckBoxParametri
    {
        public DBCheckBoxParametri()
        {
        }

        public string DataField = "";
        public bool DataFieldChanged = false;
        public string Tip = "";
        public bool TipChanged = false;
        public int Size = 0;
        public bool SizeChanged = false;
    }

    public class RegionParametri
    {
        public RegionParametri()
        {
        }

        public string Caption = "";
        public bool CaptionChanged = false;
    }

    public class UpdateVariable
    {
        public UpdateVariable()
        {
        }

        public string Name;
        public string Tip;
        public int Size;

        public string ValueString = "";
        public int ValueInt = 0;
        public Double ValueDouble = 0.0;
        public DateTime ValueDateTime = new DateTime(1800, 1, 1);
        public bool ValueBool = false;
    }
}
