using PeopleModel;
using Prism.Mvvm;
using SearchModel;
using SENSING.Plugin.Intelligent.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace SENSING.Plugin.Intelligent.ViewModels
{
    public partial class MainControlViewModel : BindableBase
    {
        #region STATIC PROPERTIES

        public static MainControl MainControlContent { get; set; }
        public static TargetControl TargetControlContent { get; set; }
        public static NoTargetControl NoTargetControlContent { get; set; }

        #endregion

        #region Page Appear Property

        Visibility _loadingVisiblity;
        public Visibility LoadingVisiblity
        {
            get { return _loadingVisiblity; }
            set { SetProperty(ref _loadingVisiblity, value); }
        }

        #endregion

        #region MyRegion

        SearchSchemaModel _searchSchemaDatas;
        public SearchSchemaModel SearchSchemaDatas
        {
            get
            {
                if (_searchSchemaDatas.IsTargetValue != null)
                    if (_searchSchemaDatas.IsTargetValue == "有目标分析")
                    {
                        IsTarget = System.Windows.Visibility.Visible;
                        IsNotTarget = System.Windows.Visibility.Collapsed;
                    }
                    else
                    {
                        IsTarget = System.Windows.Visibility.Collapsed;
                        IsNotTarget = System.Windows.Visibility.Visible;
                    }
                return _searchSchemaDatas;
            }
            set { SetProperty(ref _searchSchemaDatas, value); }
        }

        ObservableCollection<TmpSnapInfo> _topNumberItems;
        public ObservableCollection<TmpSnapInfo> TopNumberItems
        {
            get { return _topNumberItems; }
            set { SetProperty(ref _topNumberItems, value); }
        }

        CameraSnapPerson _suspiciousInfo;
        public CameraSnapPerson SusipciousInfo
        {
            get { return _suspiciousInfo; }
            set { SetProperty(ref _suspiciousInfo, value); }
        }

        CameraSnapPerson _forTargetPage;
        public CameraSnapPerson ForTargetPage
        {
            get
            {
                return _forTargetPage;
            }
            set { SetProperty(ref _forTargetPage, value); }
        }

        CameraSnapPerson _forNoTargetPage;
        public CameraSnapPerson ForNoTargetPage
        {
            get
            {
                return _forNoTargetPage;
            }
            set { SetProperty(ref _forNoTargetPage, value); }
        }

        ObservableCollection<xiaowen.codestacks.data.SenSingModels.Camera> _snapPersonItems;
        public ObservableCollection<xiaowen.codestacks.data.SenSingModels.Camera> SnapPersonItems
        {
            get { return _snapPersonItems; }
            set { SetProperty(ref _snapPersonItems, value); }
        }

        #endregion

        #region Temp Cache Property



        #endregion

        #region MAP PROPERTY
        
        public static MainControlViewModel StaticMainViewModel { get; private set; }

        #endregion

        void initPropertyValue()
        {
            LoadingVisiblity = Visibility.Collapsed;
        }

        #region is target properties

        Visibility _isTarget;
        public Visibility IsTarget
        {
            get { return _isTarget; }
            set { SetProperty(ref _isTarget, value); }
        }

        Visibility _isNotTarget;
        public Visibility IsNotTarget
        {
            get { return _isNotTarget; }
            set { SetProperty(ref _isNotTarget, value); }
        }

        static object _receiedObj;
        public static object ReceivedObj
        {
            set
            {
                _receiedObj = value;
                BiMainObj.SetReceivedData(_receiedObj);
            }
        }

        #endregion

        public static MainControlViewModel BiMainObj { get; set; }
    }
}
