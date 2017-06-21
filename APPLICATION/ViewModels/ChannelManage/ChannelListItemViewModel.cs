using SENSING.ClassPool;
using Prism.Mvvm;

namespace FaceSysByMvvm.ViewModels
{
    public class ChannelListItemViewModel : BindableBase
    {
        public MyChannelCfg MyChannelCfg { get; set; }

        private bool isOpened;
        public bool IsOpened
        {
            get { return isOpened; }
            set
            {
                SetProperty(ref isOpened, value);
            }
        }
    }
}

