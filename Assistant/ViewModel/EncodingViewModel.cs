using Assistant.Models;
using Assistant.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;

namespace Assistant.ViewModel
{
    public class EncodingViewModel : ViewModelBase
    {
        public string FilePath { get; set; }
        public string TempPath { get; set; }
        public ExchangeData exchangeData { get; set; }
        public EncodingModel encodingModel { get; set; }
        #region Command
        public RelayCommand<DragEventArgs> DropFileCommand { get; set; }
        public RelayCommand LoadCommand { get; set; }
        public RelayCommand LoadTempCommand { get; set; }
        #endregion

        public EncodingViewModel()
        {
            exchangeData = new ExchangeData();
            encodingModel = new EncodingModel();
            LoadCommand = new RelayCommand(LoadData);
            LoadTempCommand = new RelayCommand(LoadTemp);
            DropFileCommand = new RelayCommand<DragEventArgs>(DropFile);
        }

        private void LoadTemp()
        {
            if(string.IsNullOrEmpty(TempPath))
            {
                encodingModel.State = exchangeData.ReadConfigXml("alarm02");
                return;
            }
            encodingModel.IdentifyTable= LoadAllData(TempPath);
            switch (encodingModel.IdentifyTable.Columns[1].ColumnName)
            {
                case "项目简称":
                    encodingModel.State = "请购单";
                    break;
                case "工单单别":
                    encodingModel.State = "BOM表";
                    break;
                default:
                    break;
            }
            encodingModel.IdentifyCount = encodingModel.DtSources.Rows.Count;


        }

        private void DropFile(DragEventArgs e)
        {
            TempPath = ((System.Array)e.Data.GetData(System.Windows.DataFormats.FileDrop)).GetValue(0).ToString();
            encodingModel.IdentifyTable= LoadAllData(TempPath);
            encodingModel.IdentifyCount= encodingModel.DtSources.Rows.Count;
        }

        private void LoadData()
        {
            FilePath = exchangeData.ReadConfigXml("totalDataPath");
            encodingModel.TotalTable= LoadAllData(FilePath);
            encodingModel.TotalCount = encodingModel.DtSources.Rows.Count;
        }

        public void ExportData(DataTable table)
        {

        }

        public DataTable GetMaxSerialNum(DataTable table)
        {
            return table;
        }

        public DataTable LoadAllData(string filePath)
        {
            DataTable table = new DataTable();
            string state;
            table = exchangeData.ExcelToDatatable(filePath, out state);
            encodingModel.State = state;
            encodingModel.DtSources = table;
            return table;
        }

       
        public void Log(string filePath, string Msg)
        {
            throw new NotImplementedException();
        }
    }
   
}
