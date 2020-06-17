using GalaSoft.MvvmLight;
using System.Data;

namespace Assistant.Models
{
	public class EncodingModel : ObservableObject
	{
		private DataTable dtSources;

		public DataTable DtSources
		{
			get { return dtSources; }
			set
			{
				dtSources = value;
				RaisePropertyChanged(nameof(DtSources));
			}
		}

		#region 成员字段
		private DataTable templateTable;

		public DataTable TemplateTable
		{
			get { return templateTable; }
			set
			{
				templateTable = value;
				RaisePropertyChanged(nameof(TemplateTable));
			}
		}

		private string state;

		public string State
		{
			get { return state; }
			set { state = value; RaisePropertyChanged("State"); }
		}

		private DataTable totalTable;

		public DataTable TotalTable
		{
			get { return totalTable; }
			set
			{
				totalTable = value;
				RaisePropertyChanged(nameof(TotalTable));
			}
		}
		private DataTable tempTable;

		public DataTable TempTable
		{
			get { return tempTable; }
			set
			{
				tempTable = value;
				RaisePropertyChanged(nameof(TempTable));
			}
		}
		private DataTable identifyTable;

		public DataTable IdentifyTable
		{
			get { return identifyTable; }
			set
			{
				identifyTable = value;
				RaisePropertyChanged(nameof(IdentifyTable));
			}
		}
		private int totalCount;

		public int TotalCount
		{
			get { return totalCount; }
			set
			{
				totalCount = value;
				RaisePropertyChanged(nameof(TotalCount));
			}
		}
		private int identifyCount;

		public int IdentifyCount
		{
			get { return identifyCount; }
			set
			{
				identifyCount = value;
				RaisePropertyChanged(nameof(IdentifyCount));
			}
		}
		#endregion

		private int weekOfYear;

		public int WeekOfYear
		{
			get { return weekOfYear; }
			set
			{
				weekOfYear = value;
				RaisePropertyChanged(nameof(WeekOfYear));
			}
		}



	}
}
