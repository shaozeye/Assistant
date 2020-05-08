﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Assistant.Services
{
    interface IExchangeData
    {
        DataTable ExcelToDatatable(string filePath);
        void DatatableToExcel(DataTable table);
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
        public DataTable ExcelToDatatable(string filePath)
        {
            DataTable table = new DataTable();
            ISheet sheet = null;
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
            for (int i = 0; i < header.LastCellNum; i++)
            {
                table.Columns.Add(header.Cells[i].ToString().Replace("/", "").Replace("\n", "").Replace(".", ""));
            }
            IRow cells;
            for (int i = first + 1; i <= sheet.LastRowNum - first; i++)
            {
                DataRow dataRow = table.NewRow();
                cells = sheet.GetRow(i);
                for (int j = 0; j < cells.LastCellNum; j++)
                {
                    if(cells.Cells[j]!=null)
                    dataRow[j] =cells.Cells[j].StringCellValue;
                }
                table.Rows.Add(dataRow);
            }
            return table;
        }
        //    private static object GetValueType(ICell cell)
        //    {
        //        if (cell == null)
        //            return null;
        //        switch (cell.CellType)
        //        {
        //            case CellType.Blank:
        //                return "";
        //            case CellType.Boolean:
        //                return cell.BooleanCellValue;
        //            case CellType.Numeric:
        //                return cell.NumericCellValue;
        //            case CellType.String:
        //                return cell.StringCellValue.Trim();
        //            case CellType.Error:
        //                return cell.ErrorCellValue;
        //            case CellType.Formula:
        //                return cell.StringCellValue;
        //            default:
        //                return "=" + cell.CellFormula;
        //        }

        public void DatatableToExcel(DataTable table)
        {

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