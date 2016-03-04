using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DigitasLbi.RedirectTool.Extension;
using OfficeOpenXml;

namespace DigitasLbi.RedirectTool.Helper
{
    public class Helper
    {
        public static string CreateFile(string xmlPathToSave, string excelSourcePath)
        {
            try
            {
                FileInfo file = new FileInfo(excelSourcePath);

                ExcelPackage package = new ExcelPackage(file);
                var dt = package.ToDataTable();
                List<rewriteRule> rules = new List<rewriteRule>();

                for (int i = 0; i < 10; i++)
                {
                    rules.Add(new rewriteRule
                    {
                        stopProcessing = "aa" + i,
                        name = "rule" + i,
                        patternSyntax = "sdds" + i
                    });
                }

                var data = new rewrite { rules = rules.ToArray() };

                var serializer = new XmlSerializer(typeof(rewrite));

                using (var stream = new StreamWriter(xmlPathToSave))
                    serializer.Serialize(stream, data);

                return Constant.Constant.Mesasge.Success.ToString();
            }
            catch (Exception ex)
            {
                return $"{Constant.Constant.Mesasge.Fail} : Message :{ex.Message}\nSource :{ex.Source} \nStackTrace : {ex.StackTrace}";
            }
        }
    }
}
