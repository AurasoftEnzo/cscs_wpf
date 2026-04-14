using SplitAndMerge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace WpfCSCS.XmlHelper
{
    public class XmlParseFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);

            string xmlContent = Utils.GetSafeString(args, 0);

            try
            {
                XDocument xDoc = XDocument.Parse(xmlContent);
                return new Variable(xDoc);
            }
            catch (Exception ex)
            {
                return new Variable("ERROR: " + ex.Message);
            }
        }
    }

    public class XmlGetRootNameFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);

            XDocument xDoc = args[0].Object as XDocument;
            if (xDoc?.Root != null)
            {
                return new Variable(xDoc.Root.Name.LocalName);
            }

            return Variable.EmptyInstance;
        }
    }

    public class XmlGetValueFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 2, m_name);

            XDocument xDoc = args[0].Object as XDocument;
            string xpath = Utils.GetSafeString(args, 1);
            string ns = Utils.GetSafeString(args, 2, "");

            try
            {
                XNamespace xns = ns;
                var element = xDoc.Root.XPathSelectElement(xpath, CreateNamespaceManager(xDoc, ns));
                return new Variable(element?.Value ?? "");
            }
            catch
            {
                return Variable.EmptyInstance;
            }
        }

        private static XmlNamespaceManager CreateNamespaceManager(XDocument xDoc, string defaultNs)
        {
            var nsmgr = new XmlNamespaceManager(new NameTable());
            if (!string.IsNullOrEmpty(defaultNs))
            {
                nsmgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
                nsmgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                nsmgr.AddNamespace("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
                nsmgr.AddNamespace("hrextac", "urn:mfin.gov.hr:schema:xsd:HRExtensionAggregateComponents-1");
            }
            return nsmgr;
        }
    }

    public class XmlGetElementsFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 2, m_name);

            XDocument xDoc = args[0].Object as XDocument;
            string xpath = Utils.GetSafeString(args, 1);
            string ns = Utils.GetSafeString(args, 2, "");

            try
            {
                var elements = xDoc.Root.XPathSelectElements(xpath, CreateNamespaceManager(xDoc, ns));
                Variable result = new Variable(Variable.VarType.ARRAY);

                foreach (var element in elements)
                {
                    result.Tuple.Add(new Variable(element));
                }

                return result;
            }
            catch
            {
                return new Variable(Variable.VarType.ARRAY);
            }
        }

        private static XmlNamespaceManager CreateNamespaceManager(XDocument xDoc, string defaultNs)
        {
            var nsmgr = new XmlNamespaceManager(new NameTable());
            if (!string.IsNullOrEmpty(defaultNs))
            {
                nsmgr.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
                nsmgr.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                nsmgr.AddNamespace("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
                nsmgr.AddNamespace("hrextac", "urn:mfin.gov.hr:schema:xsd:HRExtensionAggregateComponents-1");
            }
            return nsmgr;
        }
    }

    public class XmlGetChildValueFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 2, m_name);

            XElement parent = args[0].Object as XElement;
            string childName = Utils.GetSafeString(args, 1);
            string ns = Utils.GetSafeString(args, 2, "");

            if (parent != null)
            {
                XNamespace xns = ns;
                var child = parent.Element(xns + childName);
                return new Variable(child?.Value ?? "");
            }

            return Variable.EmptyInstance;
        }
    }
    
    public class XmlGetChildFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 2, m_name);

            XElement parent = args[0].Object as XElement;
            string childName = Utils.GetSafeString(args, 1);
            string ns = Utils.GetSafeString(args, 2, "");

            if (parent != null)
            {
                XNamespace xns = ns;
                var child = parent.Element(xns + childName);
                //return new Variable(child?.Value ?? "");
                return new Variable(child);
            }

            return Variable.EmptyInstance;
        }
    }

    public class XmlGetChildAttributeFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 3, m_name);

            XElement parent = args[0].Object as XElement;
            string childName = Utils.GetSafeString(args, 1);
            string attrName = Utils.GetSafeString(args, 2);
            string ns = Utils.GetSafeString(args, 3, "");

            if (parent != null)
            {
                XNamespace xns = ns;
                var child = parent.Element(xns + childName);
                return new Variable(child?.Attribute(attrName)?.Value ?? "");
            }

            return Variable.EmptyInstance;
        }
    }
    
    public class XmlGetElementValueFunction : ParserFunction
    {
        protected override Variable Evaluate(ParsingScript script)
        {
            List<Variable> args = script.GetFunctionArgs();
            Utils.CheckArgs(args.Count, 1, m_name);

            XElement element = args[0].Object as XElement;

            if (element != null)
            {   
                return new Variable(element.Value ?? "");
            }

            return Variable.EmptyInstance;
        }
    }

}
