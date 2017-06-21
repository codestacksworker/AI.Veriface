using Prism.Mvvm;

namespace SENSING.Plugin.Intelligent.ViewModels
{
    public partial class MainControlViewModel : BindableBase
    {
        public MainControlViewModel()
        {
            initSearchCommand();
            BiMainObj = this;
            StaticMainViewModel = this;
            initPropertyValue();
        }
    }
}
