using SINGLEUSER.Models;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using FaceSysByMvvm.Model;

namespace FaceSysByMvvm.ViewModels.ChannelManage
{
    public class WarningMessageWindowViewModel : BindableBase
    {
        private WarningMessageCmd _cmd = new WarningMessageCmd();
        private WarningMessageModel _property;
        WarningMessageWindowViewModel _wmv;
        public WarningMessageCmd Cmd
        {
            get { return _cmd; }
            set { SetProperty(ref _cmd, value); }
        }

        public WarningMessageModel Property
        {
            get { return _property; }
            set { SetProperty(ref _property, value); }
        }

        public WarningMessageWindowViewModel Wmv
        {
            get { return _wmv; }
            set
            {
                _wmv = value;
                SetProperty(ref _wmv, value);
            }
        }

        bool _isChecked;

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = value;
            }
        }

        public WarningMessageWindowViewModel()
        {
            WarningMessageCmd.InitCmd()();
            Property = new WarningMessageModel();
            Property.CompareLogDatas = new ObservableCollection<MyCmpFaceLogWidthImgModel>();
            Property.Flag = 0;
        }
        private static volatile WarningMessageWindowViewModel warnModel;
        private static readonly object obj = new object();
        public static WarningMessageWindowViewModel WarnModel
        {
            get
            {
                if (null == warnModel)
                {
                    lock (obj)
                    {
                        if (null == warnModel)
                        {
                            warnModel = new WarningMessageWindowViewModel();
                        }
                    }
                }
                return warnModel;
            }
        }
        public void RefreshProperty()
        {
            RaisePropertyChanged("Property");
        }
    }
}
