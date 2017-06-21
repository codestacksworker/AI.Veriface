using Prism.Mvvm;

namespace SETTINGS_MODULES.ViewModels
{
    public partial class MainWindowViewModel : BindableBase
    {
        AppConfigModel.ConfigContent _areaConfigContent;
        public AppConfigModel.ConfigContent AreaConfigContent
        {
            get { return _areaConfigContent; }
            set { SetProperty(ref _areaConfigContent, value); }
        }


    }
}
