using DATA.MODELS.GlobalModels;
using System.Windows;

namespace SETTINGS_MODULES.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            GlobalCache.ChildWindowStatus = 1;
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            GlobalCache.ChildWindowStatus = -1;
        }
    }
}
