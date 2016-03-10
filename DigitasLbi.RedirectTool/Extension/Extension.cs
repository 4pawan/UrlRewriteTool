using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace DigitasLbi.RedirectTool.Extension
{
    public static class Extension
    {
        public static DataTable ToDataTable(this ExcelPackage package)
        {
            ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
            DataTable table = new DataTable();
            foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
            {
                table.Columns.Add(firstRowCell.Text);
            }

            for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
            {
                var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
                var newRow = table.NewRow();
                foreach (var cell in row)
                {
                    newRow[cell.Start.Column - 1] = cell.Text;
                }
                table.Rows.Add(newRow);
            }
            return table;
        }

        public static DataTable ToDataTable(this rewrite xml)
        {
            string existingUrl = "ExistingUrl";
            string newUrl = "NewUrl";
            string status = "Status";

            DataTable table = new DataTable
            {
                Columns = { new DataColumn(existingUrl), new DataColumn(newUrl), new DataColumn(status) }
            };

            foreach (rewriteRule rule in xml.rules)
            {
                var newRow = table.NewRow();
                newRow[newUrl] = rule.action.url;
                newRow[existingUrl] = string.Join("", rule.conditions.add.Select(i => i.pattern));
                newRow[status] = "Ok";
                table.Rows.Add(newRow);
            }
            return table;
        }


        public static List<T> GetClassFromExcel<T>(this ExcelPackage package, int fromRow = 2, int fromColumn = 1, int toColumn = 0)
        {
            List<T> retList = new List<T>();

            //Retrieve first Worksheet
            var ws = package.Workbook.Worksheets.First();
            //If the to column is empty or 0, then make the tocolumn to the count of the properties
            //Of the class object inserted
            toColumn = toColumn == 0 ? typeof(T).GetProperties().Count() : toColumn;

            //Read the first Row for the column names and place into a list so that
            //it can be used as reference to properties
            List<string> columnNames = new List<string>();
            // wsRow = ws.Row(0);
            foreach (var cell in ws.Cells[1, 1, 1, ws.Cells.Count()])
            {
                columnNames.Add(cell.Value.ToString());
            }
            //Loop through the rows of the excel sheet
            for (var rowNum = fromRow; rowNum <= ws.Dimension.End.Row; rowNum++)
            {
                //create a instance of T
                T objT = Activator.CreateInstance<T>();
                //Retrieve the type of T
                Type myType = typeof(T);
                //Get all the properties associated with T
                PropertyInfo[] myProp = myType.GetProperties();

                var wsRow = ws.Cells[rowNum, fromColumn, rowNum, ws.Cells.Count()];

                foreach (var propertyInfo in myProp)
                {
                    if (columnNames.Contains(propertyInfo.Name))
                    {
                        int position = columnNames.IndexOf(propertyInfo.Name);
                        //To prevent an exception cast the value to the type of the property.
                        propertyInfo.SetValue(objT, Convert.ChangeType(wsRow[rowNum, position + 1].Value, propertyInfo.PropertyType));
                    }
                }

                retList.Add(objT);
            }


            return retList;
        }







    }
}
