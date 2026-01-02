using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting.Caching;
using DevExpress.XtraReports.UI;
using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


//using WpfCSCS.Reporting.DevExpressReporting;
using WpfCSCS.Reporting.DevExpressReportingNonDEFINEVariables;
//using WpfCSCS.Reporting.ReportBuilderReporting;


namespace WpfCSCS.Reporting
{
    public static class Reporting
    {
        public static void Init(Interpreter interpreter)
        {
            ReportFunction.Init(interpreter);
        }
    }  
    
}
