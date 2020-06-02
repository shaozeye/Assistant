using Assistant.ViewModel;
using GalaSoft.MvvmLight.Ioc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;


namespace Assistant.Services
{
    interface IExchangeData
    {
        DataTable ExcelToDatatable(string filePath, out string msg);
        void DatatableToExcel(DataTable table,string path);
        string ReadConfigXml(string key);
        void WriteConfigXml(string key, string value);
    }
    public class ExchangeData : IExchangeData
    {
       

        public ExchangeData()
        {
           
        }
        private IWorkbook workbook = null;
        private FileStream fs = null;
        public DataTable ExcelToDatatable(string filePath, out string msg)
        {
            msg = "";
            DataTable table = new DataTable();
            ISheet sheet = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                switch (Path.GetExtension(filePath).ToLower())
                {
                    case ".xlsx":
                        workbook = new XSSFWorkbook(fs);
                        break;
                    case ".xls":
                        workbook = new HSSFWorkbook(fs);
                        break;
                    default:
                        MessageBox.Show(ReadConfigXml("alarm01"));
                        break;
                }
                sheet = workbook.GetSheetAt(0);
                int first = 0;
                ICell cell = sheet.GetRow(first).GetCell(0);
                while (cell == null || cell.IsMergedCell)
                {
                    first++;
                    cell = sheet.GetRow(first).GetCell(0);
                }
                IRow header = sheet.GetRow(first);
                if (header.LastCellNum > 20) //表头超过20个
                {
                    MessageBox.Show(ReadConfigXml("alarm01")+":LastCellNum="+header.LastCellNum);
                    return table;
                }
                for (int i = 0; i < header.LastCellNum; i++)
                {
                    table.Columns.Add(header.Cells[i].ToString().Replace("/", "").Replace("\n", "").Replace(".", ""));
                }
                IRow npoiRow;
                for (int i = first + 1; i < sheet.LastRowNum; i++)
                {
                    DataRow dataRow = table.NewRow();
                    npoiRow = sheet.GetRow(i);
                    if (npoiRow.Cells[0].CellType == CellType.Blank&& npoiRow.Cells[1].CellType == CellType.Blank&&
                        npoiRow.Cells[2].CellType == CellType.Blank&& npoiRow.Cells[3].CellType == CellType.Blank) break;
                    for (int j = 0; j < header.LastCellNum; j++)
                    {
                        dataRow[j] = GetValueType(npoiRow.GetCell(j));
                    }
                    table.Rows.Add(dataRow);
                }
        }
            catch (Exception e)
            {
                msg = e.Message;
            }
            return table;
        }
        private static object GetValueType(ICell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.Blank:
                    return " ";
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                case CellType.Numeric:
                    return cell.NumericCellValue;
                case CellType.String:
                    return cell.StringCellValue.Trim();
                case CellType.Error:
                    return cell.ErrorCellValue;
                case CellType.Formula:
                    return cell.StringCellValue;
                default:
                    return "=" + cell.CellFormula;
            }
        }
        public delegate void LogError(string msg);
        //Action action;

        public void DatatableToExcel(DataTable table,string path)
        {
            if (table==null||string.IsNullOrEmpty(path))
            {
                MessageBox.Show(ReadConfigXml("alarm08"));
                return;
            }
            IWorkbook workbook = null;
            if (Path.GetExtension(path).ToLower() == ".xls")
                workbook = new HSSFWorkbook();
            else if (Path.GetExtension(path) == ".xlsx")
                workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("sheet1");
            sheet.SetAutoFilter(new CellRangeAddress(0, 0, 0, table.Columns.Count - 1));
            sheet.CreateFreezePane(table.Columns.Count, 1);
            IRow cells = sheet.CreateRow(0);
            for (int i = 0; i < table.Columns.Count; i++)
            {
                sheet.SetColumnWidth(i, 20 * 256);
                cells.CreateCell(i).SetCellValue(table.Columns[i].ColumnName);
            }
            for (int i = 0; i < table.Rows.Count; i++)
            {
                cells = sheet.CreateRow(i + 1);
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    cells.CreateCell(j).SetCellValue(table.Rows[i][j].ToString().Trim());
                }
            }
            FileStream fs;
            try
            {
               // File.Delete(path);
                fs = File.Create(path);
                fs.Close();
                fs = new FileStream(path, FileMode.Open, FileAccess.Write);
                workbook.Write(fs);
                //fs.Flush();
                fs.Close();
                workbook.Close();
                //action = new Action(SimpleIoc.Default.GetInstance<EncodingViewModel>().SaveSuccess);
                //action.Invoke();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }

        public string ReadConfigXml(string key)
        {
            try
            {
                var config = ConfigurationManager.AppSettings;
                string resurt = config[key] ?? "无配置文件";
                return resurt;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public void WriteConfigXml(string key, string value)
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                if (ConfigurationManager.AppSettings[key] == null)
                {
                    config.AppSettings.Settings.Add(key, value);
                }
                config.AppSettings.Settings[key].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

    }
}
