using Assistant.Services;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assistant.ViewModel
{
    public class SetWindowViewModel:ViewModelBase
    {
        public RelayCommand<object>  OpenFileCommand { get; set; }
		ExchangeData exchangeData { get; set; }


		public SetWindowViewModel()
        {
            OpenFileCommand = new RelayCommand<object>(OpenFile);
			exchangeData = new ExchangeData();
			TemplateDataPath = exchangeData.ReadConfigXml("templatePath");
			TotalDataPath = exchangeData.ReadConfigXml("totalDataPath");
		}
		private bool? autoStart;

		public bool? AutoStart
		{
			get 
			{
				//autoLoad = exchangeData.ReadConfigXml("autoLoad");
				return autoStart; 
			}
			set
			{
				autoStart = value;
				//exchangeData.WriteConfigXml();
				RaisePropertyChanged(nameof(AutoStart));
			}
		}

		private bool? autoLoad;

		public bool? AutoLoad
		{
			get { return autoLoad; }
			set
			{
				autoLoad = value;
				RaisePropertyChanged(nameof(AutoLoad));
			}
		}
		private string safeCount;

		public string SafeCount
		{
			get {
				safeCount = exchangeData.ReadConfigXml("safeColumnCount");
				return safeCount; }
			set
			{
				safeCount = value;
				exchangeData.WriteConfigXml("safeColumnCount", safeCount);
				RaisePropertyChanged(nameof(SafeCount));
			}
		}

		private string totalDataPath;

		public string TotalDataPath
		{
			get { return totalDataPath; }
			set
			{
				totalDataPath = value;
				RaisePropertyChanged(nameof(TotalDataPath));
			}
		}
		private string templateDataPath;

		public string TemplateDataPath
		{
			get { return templateDataPath; }
			set
			{
				templateDataPath = value;
				RaisePropertyChanged(nameof(TemplateDataPath));
			}
		}


		private void OpenFile(object obj)
        {
			System.Windows.Controls.Button button = obj as System.Windows.Controls.Button;
			var ofd = new System.Windows.Forms.OpenFileDialog();
			ofd.Filter = "Excel文件(*.xls;*.xlsx)|*.xls;*.xlsx|所有文件|*.*";
			ofd.ValidateNames = true;
			ofd.CheckPathExists = true;
			ofd.CheckFileExists = true;
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				string strFileName = ofd.FileName;

				if (button.Tag.ToString() == "template")
				{
					TemplateDataPath = strFileName;
					exchangeData.WriteConfigXml("templatePath", strFileName);

				}
				else
				{
					TotalDataPath = strFileName;
					exchangeData.WriteConfigXml("totalDataPath", strFileName);

				}
			}
		}
    }
}
