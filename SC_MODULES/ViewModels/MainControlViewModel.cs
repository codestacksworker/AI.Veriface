using Prism.Mvvm;
using System.Windows;

namespace SC_MODULES.ViewModels
{
    public partial class MainControlViewModel : BindableBase
    {
        public MainControlViewModel()
        {
            initSearchResult(ReceivedObj);
            initCmd();
            ScMainObj = this;
            LoadingVisiblity = Visibility.Collapsed;
        }

        static object _receiedObj;
        public static object ReceivedObj
        {
            get { return _receiedObj; }
            set { _receiedObj = value; }
        }

        Visibility _isSnapStore;
        public Visibility IsSnapStore
        {
            get { return _isSnapStore; }
            set { SetProperty(ref _isSnapStore, value); }
        }

        Visibility _loadingVisiblity;
        public Visibility LoadingVisiblity
        {
            get { return _loadingVisiblity; }
            set { SetProperty(ref _loadingVisiblity, value); }
        }

        public static MainControlViewModel ScMainObj { get; set; }
        
    }
}
