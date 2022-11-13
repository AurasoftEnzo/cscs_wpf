﻿using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfControlsLibrary;

namespace WpfCSCS
{
    public class NavigatorClass
    {
        public void Init(Interpreter intepreter)
        {
            intepreter.RegisterFunction(Constants.NAVIGATOR, new NavigatorFunction()); 
        }

        public static Dictionary<string, int> navigatorOfTable = new Dictionary<string, int>();
        public static Dictionary<string, string> navigatorKey = new Dictionary<string, string>();
        public static Dictionary<string, ParsingScript> navigatorScript = new Dictionary<string, ParsingScript>();

        public class NavigatorFunction : ParserFunction
        {
            string navigatorName;
            int tableHndlNum;
            static string tableKey;
            
            static NavigatorButton navigatorButton;

            protected override Variable Evaluate(ParsingScript script)
            {
                List<Variable> args = script.GetFunctionArgs();
                Utils.CheckArgs(args.Count, 3, m_name);
                navigatorName = Utils.GetSafeString(args, 0);
                tableHndlNum = Utils.GetSafeInt(args, 1);
                tableKey = Utils.GetSafeString(args, 2).ToLower();

                navigatorOfTable[navigatorName] = tableHndlNum;
                navigatorKey[navigatorName] = tableKey;
                navigatorScript[navigatorName] = script;

                return Variable.EmptyInstance;
            }
        }

        public void NavigateFirst(string navigatorName)
        {
            new Btrieve.FINDVClass(navigatorScript[navigatorName], navigatorOfTable[navigatorName], "f", navigatorKey[navigatorName]).FINDV();
        }
        public void NavigateLast(string navigatorName)
        {
            new Btrieve.FINDVClass(navigatorScript[navigatorName], navigatorOfTable[navigatorName], "l", navigatorKey[navigatorName]).FINDV();
        }

        public void NavigateNext(string navigatorName)
        {
            if(Btrieve.OPENVs[navigatorOfTable[navigatorName]].currentRow == 0)
            {
                NavigateFirst(navigatorName);
            }
            else
            {
                new Btrieve.FINDVClass(navigatorScript[navigatorName], navigatorOfTable[navigatorName], "n").FINDV();
            }
        }

        public void NavigatePrevious(string navigatorName)
        {
            if (Btrieve.OPENVs[navigatorOfTable[navigatorName]].currentRow == 0)
            {
                NavigateFirst(navigatorName);
            }
            else
            {
                new Btrieve.FINDVClass(navigatorScript[navigatorName], navigatorOfTable[navigatorName], "p").FINDV();
            }
        }

    }
}
