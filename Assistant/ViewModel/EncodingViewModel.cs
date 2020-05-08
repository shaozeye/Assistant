using Assistant.Models;
using Assistant.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assistant.ViewModel
{
    public class EncodingViewModel:ViewModelBase,IHandleData
    {
        public string FilePath { get; set; }
        public ExchangeData exchangeData { get; set; }
        public EncodingModel encodingModel { get; set; }
        #region Command
        public RelayCommand LoadCommand { get; set; }
        #endregion

        public EncodingViewModel()
        {
            exchangeData = new ExchangeData();
            encodingModel = new EncodingModel();
            LoadCommand = new RelayCommand(LoadData);
        }

        private void LoadData()
        {
            //FilePath = exchangeData.ReadConfigXml("totalDataPath");
            FilePath = @"D:\仓库物料总表标准版.xls";
            encodingModel.DtSources= LoadAllData(FilePath);
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
            table = exchangeData.ExcelToDatatable(filePath);
           
            return table;
        }

        public DataTable LoadTempData(string filePath)
        {
            throw new NotImplementedException();
        }

        public void Log(string filePath, string Msg)
        {
            throw new NotImplementedException();
        }
    }
    interface IHandleData
    {
        DataTable LoadAllData(string filePath);
        DataTable LoadTempData(string filePath);
        DataTable GetMaxSerialNum(DataTable table);
        void ExportData(DataTable table);
        void Log(string filePath,string Msg);
    }
}
