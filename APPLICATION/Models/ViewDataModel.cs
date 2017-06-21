using FaceSysByMvvm.ViewModels.ChannelManage;

namespace SINGLEUSER.Models
{
    public class ViewDataModel
    {
        //static WarningMessageWindowViewModel _warningData = new WarningMessageWindowViewModel();
        static WarningMessageWindowViewModel _warningData = WarningMessageWindowViewModel.WarnModel;
        public static WarningMessageWindowViewModel WarningData
        {
            get { return _warningData; }
            set { _warningData = value; }
        }
    }
}
