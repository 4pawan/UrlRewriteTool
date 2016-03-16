using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Xml.Serialization;
using DigitasLbi.RedirectTool.Extension;
using OfficeOpenXml;
using OfficeOpenXml.Style;

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

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var existingUrl = new Uri(dt.Rows[i][1].ToString());  // existing url

                    rules.Add(new rewriteRule
                    {
                        stopProcessing = true,
                        name = "rule" + i,
                        patternSyntax = "Wildcard",
                        match = new rewriteRuleMatch
                        {
                            url = "*"
                        },
                        conditions = new rewriteRuleConditions
                        {
                            //trackAllCaptures = true,
                            add = new List<rewriteRuleConditionsAdd>
                            {
                                 //new rewriteRuleConditionsAdd
                                 //{
                                 //   input = "{HTTP_HOST}",
                                 //   pattern = existingUrl.Host
                                 //},
                                 new rewriteRuleConditionsAdd
                                 {
                                    input = "{URL}",
                                    pattern = existingUrl.AbsolutePath
                                 }
                          }.ToArray()
                        },
                        action = new rewriteRuleAction
                        {
                            type = "Redirect",
                            url = dt.Rows[i][3].ToString()  // to be replaced
                        }
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

        public static async Task ValidateRewriteRulesAsync(string xmlPathToSave)
        {
            rewrite output = (rewrite)new XmlSerializer(typeof(rewrite)).Deserialize(new StreamReader(xmlPathToSave));

            var dt = output.ToDataTable();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][2] = await ValidateRuleAsync(dt.Rows[i][0].ToString());
            }
            DataTableToExcel(xmlPathToSave.Replace(".xml", ".xlsx"), dt);

        }

        public static void DataTableToExcel(string excelDestinationPath, DataTable dt)
        {
            FileInfo fileInfo = new FileInfo(excelDestinationPath);
            if (fileInfo.Exists) fileInfo.Delete();
            using (ExcelPackage pck = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");
                ws.Cells["A1"].LoadFromDataTable(dt, true);

                //Format the header for column 1-3
                using (ExcelRange rng = ws.Cells["A1:C1"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    rng.Style.Fill.BackgroundColor.SetColor((System.Drawing.Color.Gray));
                    rng.Style.Font.Color.SetColor(System.Drawing.Color.Green);
                }

                ExcelAddress formatRangeAddress = new ExcelAddress("C2:C" + (dt.Rows.Count + 1));

                string statement1 = "ISNUMBER(FIND(OK,ROW()))";   
                var cond1 = ws.ConditionalFormatting.AddExpression(formatRangeAddress);
                cond1.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                cond1.Style.Fill.BackgroundColor.Color = System.Drawing.Color.Green;
                cond1.Formula = statement1;

                string statement2 = "ISNUMBER(FIND(Error,ROW()))";     //"MOD(ROW(),2)<>0";
                var cond2 = ws.ConditionalFormatting.AddExpression(formatRangeAddress);
                cond2.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                cond2.Style.Fill.BackgroundColor.Color = System.Drawing.Color.Red;
                cond2.Formula = statement2;

                pck.Save();
            }
        }


        private static async Task<string> ValidateRuleAsync(string url)
        {
            try
            {
                url = "http://" + url;
                var httpClient = new HttpClient();
                var response = await httpClient.GetAsync(url);
                return response.IsSuccessStatusCode ? "OK" : "Error";
            }
            catch (Exception ex)
            {
                return ex.InnerException.Message.Contains("'static.halifax.co.uk") ? "OK" : "Error";
            }
        }
    }
}
