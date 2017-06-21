using Prism.Mvvm;

namespace SETTINGS_MODULES.ViewModels
{
    public partial class MainWindowViewModel : BindableBase
    {
        AppConfigModel.ConfigContent _portConfigContent;
        public AppConfigModel.ConfigContent PortConfigContent
        {
            get { return _portConfigContent; }
            set { SetProperty(ref _portConfigContent, value); }
        }
    }
}
