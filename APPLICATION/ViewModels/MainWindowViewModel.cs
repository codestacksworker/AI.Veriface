using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Input;

namespace SENSING.APPLICATION.ViewModels
{
    public partial class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            init();
        }
        
        #region CMD

        public ICommand CmdCloseWindow { get; private set; }

        void init()
        {
            CmdCloseWindow = new DelegateCommand<object>(CloseWindowFun);
        }

        private void CloseWindowFun(object obj)
        {
            Application.Current.Shutdown();
        }

        #endregion
    }
}
