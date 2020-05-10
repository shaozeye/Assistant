using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
            FrameSources = "Views/EncodingPage.xaml";
        }

        private void ChangePage(RadioButton obj)
        {
            var select = obj as RadioButton;
            FrameSources = @"Views/" + select.Tag + "Page.xaml";
        }
    }

}