using Assistant.Services;
using Assistant.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Windows.Controls;

namespace Assistant.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// 

        public RelayCommand<RadioButton> ChangePageCommand { get; set; }
        public RelayCommand SetConfigCommand { get; set; }


        private string frameSources;

        public string FrameSources
        {
            get { return frameSources; }
            set
            {
                frameSources = value;
                RaisePropertyChanged(nameof(FrameSources));
            }
        }
        private string version;

        public string Version
        {
            get { return version; }
            set
            {
                version = value;
                RaisePropertyChanged(nameof(Version));
            }
        }


        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
            ChangePageCommand = new RelayCommand<RadioButton>(ChangePage);
            SetConfigCommand = new RelayCommand(SetConfig);
            FrameSources = "Views/EncodingPage.xaml";
            version = new ExchangeData().ReadConfigXml("version");
        }

        private void SetConfig()
        {
            SetWindow setWindow = new SetWindow();
            setWindow.ShowDialog();
        }

        private void ChangePage(RadioButton obj)
        {
            var select = obj as RadioButton;
            FrameSources = @"Views/" + select.Tag + "Page.xaml";
        }
    }

}