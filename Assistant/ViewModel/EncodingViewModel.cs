using Assistant.Models;
using Assistant.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using Org.BouncyCastle.Crypto.Modes.Gcm;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Assistant.ViewModel
{
    public class EncodingViewModel : ViewModelBase
    {
        public string[] BuyList { get; set; }   
        public string[] BOM { get; set; }
        public string FilePath { get; set; }
        public string TempPath { get; set; }
        public ExchangeData exchangeData { get; set; }
        public EncodingModel encodingModel { get; set; }
        public string TotalFilePath { get; set; }
        public string TempFilePath { get; set; }
        #region Command
        public RelayCommand<DragEventArgs> DropFileCommand { get; set; }

        public RelayCommand LoadCommand { get; set; }
        public RelayCommand LoadTempCommand { get; set; }
        public RelayCommand SaveFileCommand { get; set; }
        #endregion
        public const  string buylist = "项目简称";
        public const  string bom = "工单单别";
        public EncodingViewModel()
        {
            BuyList = new string[] {"品号","品名","品牌","规格" };
            BOM = new string[] { "半成品品号", "元件品号","品名","品牌","规格","单位","属性" };
            exchangeData = new ExchangeData();
            encodingModel = new EncodingModel();
            LoadCommand = new RelayCommand(LoadData);
            LoadTempCommand = new RelayCommand(LoadTemp);
            DropFileCommand = new RelayCommand<DragEventArgs>(DropFile);
            SaveFileCommand = new RelayCommand(SaveFile);
            encodingModel.WeekOfYear = GetWeekOfYear();
            TotalFilePath = Path.GetFullPath(exchangeData.ReadConfigXml("totalDataPath"));
            TempFilePath = Path.GetDirectoryName(exchangeData.ReadConfigXml("totalDataPath"))+"\\";
            //System.Diagnostics.Process.Start("explorer.exe", @"D:\");
        }

        private void SaveFile()
        {
            if (encodingModel.TotalTable == null||encodingModel.IdentifyTable==null)
            {
               encodingModel.State+=  exchangeData.ReadConfigXml("alarm08")+"\r\n";
                return;
            }
            TotalFilePath = Path.GetFullPath(exchangeData.ReadConfigXml("totalDataPath"));
            TempFilePath = Path.GetDirectoryName(exchangeData.ReadConfigXml("totalDataPath"))  + Path.GetFileName(TempPath);
            try
            {
                exchangeData.DatatableToExcel(encodingModel.TotalTable, TotalFilePath);
                exchangeData.DatatableToExcel(encodingModel.IdentifyTable, TempFilePath);
                if (File.Exists(TotalFilePath) && File.Exists(TempFilePath))
                {
                    SaveSuccess();
                }
            }
            catch (Exception e)
            {

                encodingModel.State += e.Message + "\r\n";
            }
           
            
            
        }
        public void SaveSuccess()
        {
            var result = MessageBox.Show("文件保存成功\r\n是否打开文件？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                System.Diagnostics.Process.Start("explorer.exe", TempFilePath);
            }
            else
            {
                return;
            }
        }

        public int GetWeekOfYear()
        {
            System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
            return gc.GetWeekOfYear(DateTime.Now, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        }
        private void LoadTemp()
        {
            DataTable table = new DataTable();
            if(string.IsNullOrEmpty(TempPath))
            {
                encodingModel.State += exchangeData.ReadConfigXml("alarm02") + "\r\n"; //没有待编码文件
                return;
            }
            encodingModel.IdentifyTable= LoadAllData(TempPath);
            switch (encodingModel.IdentifyTable.Columns[1].ColumnName)
            {
                case buylist:
                    //encodingModel.State +="\r\n" "请购单";
                    table= encodingModel.IdentifyTable.DefaultView.ToTable(false,BuyList);
                   
                    break;
                case bom:
                    //encodingModel.State +="\r\n" "BOM表";
                    table = encodingModel.TempTable.DefaultView.ToTable(false, BOM);
                    break;
                default:
                    break;
            }
            table.Columns.Add("提示");
            
            encodingModel.DtSources= encodingModel.IdentifyTable= GetMaxSerialNum(table);
            encodingModel.IdentifyCount = encodingModel.DtSources.Rows.Count;


        }

        private void DropFile(DragEventArgs e)
        {
            if(encodingModel.TotalTable==null)
            {
                encodingModel.State +=exchangeData.ReadConfigXml("alarm03") + "\r\n";  //没有加载总表
                return;
            }
            TempPath = ((System.Array)e.Data.GetData(System.Windows.DataFormats.FileDrop)).GetValue(0).ToString();
            encodingModel.TempTable= LoadAllData(TempPath);
           // encodingModel.IdentifyCount= encodingModel.DtSources.Rows.Count;
        }

        private void LoadData()
        {
            TempPath = "";
            FilePath = exchangeData.ReadConfigXml("totalDataPath");
            encodingModel.TotalTable= LoadAllData(FilePath);
            encodingModel.TotalCount = encodingModel.DtSources.Rows.Count;
        }

        public void ExportData(DataTable table)
        {

        }

        public DataTable GetMaxSerialNum(DataTable table)
        {
            DataTable totalTable = new DataTable();
            totalTable = encodingModel.TotalTable.Copy();
            totalTable.Columns.Add("品名规格",typeof(string));
            totalTable.Columns["品名规格"].Expression = "物料名称+规格图号";
            string code;
            int codeIdex = 0;

            foreach (DataRow dataRow in table.Rows)
            {
                if (dataRow[0].ToString().ToUpper() == " ")
                {
                    codeIdex = 1;
                }
                else
                {
                    codeIdex = 0;

                }
                code = dataRow[codeIdex].ToString().ToUpper();
                string combine = dataRow["品名"] + dataRow["规格"].ToString().Trim();
                DataRow[] dataRows = totalTable.Select("品名规格=" + "'" + combine + "'");
                if (dataRows.Length > 0)
                {
                    dataRow[codeIdex] = dataRows[0]["物料编码"];
                    dataRow["提示"] = exchangeData.ReadConfigXml("alarm05");
                }
                else
                {
                    if (code.Length == 9)
                    {
                        dataRows = totalTable.Select("物料编码=" + "'" + code + "'");
                        if (dataRows.Length == 0)
                        {
                            dataRow["提示"] = exchangeData.ReadConfigXml("alarm04");
                        }
                        else
                        {
                            dataRow["品名"] = dataRows[0]["物料名称"];
                            dataRow["规格"] = dataRows[0]["规格图号"];
                            dataRow["提示"] = exchangeData.ReadConfigXml("alarm06");
                        }

                    }
                    else
                    {
                        dataRows = totalTable.Select("物料编码 like" + "'" + code + "%'", "物料编码 Desc");
                        if (dataRows.Length == 0)
                        {
                            dataRow["提示"] = exchangeData.ReadConfigXml("alarm07");
                        }
                        else
                        {
                            string num;
                            num = dataRows[0]["物料编码"].ToString().Substring(code.Length, 9 - code.Length);
                            try
                            {
                                num = (int.Parse(num) + 1).ToString();
                                code = code + num;
                                dataRow[codeIdex] = code;
                                totalTable.Rows.Add(dataRow.ItemArray[0], dataRow.ItemArray[1], dataRow.ItemArray[2], dataRow.ItemArray[3]);
                            }
                            catch (Exception e)
                            {
                                encodingModel.State += e.Message + "\r\n";
                            }
                        }

                    }
                }
            }

            
            return table;
        }

        public DataTable LoadAllData(string filePath)
        {
            DataTable table = new DataTable();
            string state;
            table = exchangeData.ExcelToDatatable(filePath, out state);
            encodingModel.State = state + "\r\n";
            encodingModel.DtSources = table;
            return table;
        }

       
        public void Log(string filePath, string Msg)
        {
           //DateTime.Now.Day
        }
    }
   
}
