using Saxon.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SchematronValidation
{
    public class SchematronValidation
    {
        public bool Validate(string xmlContent, List<string> schFiles, string transpilerPath, string logFilePath)
        {
            try
            {
                StringBuilder allErrors = new StringBuilder();

                var processor = new Processor();
                var compiler = processor.NewXsltCompiler();

                foreach (string schFile in schFiles)
                {
                    if (!File.Exists(schFile))
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
                    if (File.Exists(compiledXslPath))
                        File.Delete(compiledXslPath);
                    if (File.Exists(resultPath))
                        File.Delete(resultPath);
                }

                if (allErrors.Length > 0)
                {
                    File.WriteAllText(logFilePath, allErrors.ToString());
                }
                return allErrors.Length == 0 ? true : false;
            }
            catch (Exception ex)
            {
                File.WriteAllText(logFilePath, "ex.Message: " + ex.Message);
                return false;
            }
        }
    }

}
