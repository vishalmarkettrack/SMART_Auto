using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace SMART_AUTO
{
    public static class Spreadsheet
    {
        public static string GetValueOfField(string FilePath, string fieldName, string sheetName)
        {
            FileInfo existingFile = new FileInfo(FilePath);
            string value = null;
            using (var package = new ExcelPackage(existingFile))
            {
                var workbook = package.Workbook;
                // get the first worksheet in the workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetName];
                int rowCount = worksheet.Dimension.End.Row;
                int col = 1;
                //int colCount = worksheet.Dimension.End.Column;
                //value = new string[colCount];
                bool columnfound = false;
                for (int row = 1; row <= rowCount; row++)
                {
                    if (worksheet.Cells[row, 1].Value.ToString().ToLower() == fieldName.ToString().ToLower())
                    {
                        string val = worksheet.Cells[row, (col + 2)].Value.ToString();
                        value = val;
                        columnfound = true;
                    }
                    if (columnfound == true)
                        break;
                }
            }
            return value;
        }

        public static string[] GetMultipleValueOfField(string FilePath, string fieldName, string sheetName)
        {
            FileInfo existingFile = new FileInfo(FilePath);
            string[] value;
            using (var package = new ExcelPackage(existingFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[sheetName];
                int rowCount = worksheet.Dimension.End.Row;
                int colCount = worksheet.Dimension.End.Column;
                value = new string[colCount];
                int k = 0;
                for (int row = 1; row <= rowCount; row++)
                {
                    for (int col = 1; col <= colCount; col++)
                    {
                        if (worksheet.Cells[row, 1].Value.ToString().ToLower() == fieldName.ToString().ToLower() && (col + 2 <= colCount))
                        {
                            string val = worksheet.Cells[row, (col + 2)].Value.ToString();
                            value[k] = val;
                            k++;
                            //break;
                        }
                    }
                }
            }
            return value;
        }

    }
}
