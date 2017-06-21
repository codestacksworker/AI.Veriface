using SENSING.Plugin.Intelligent.ViewModels;
using System.Windows.Controls;

namespace SENSING.Plugin.Intelligent.Views
{
    /// <summary>
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        public MainControl()
        {
            InitializeComponent();

            MainControlViewModel.MainControlContent = this;

        }
    }
}
