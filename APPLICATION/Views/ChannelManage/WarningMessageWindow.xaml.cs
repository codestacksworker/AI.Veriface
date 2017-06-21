using model = SINGLEUSER.Models;
using System.Windows;
using System.Windows.Controls;
using DATA.MODELS.GlobalModels;
using System.Threading;
using System.Windows.Threading;

namespace FaceSysByMvvm.Views.ChannelManager
{
    /// <summary>
    /// Interaction logic for WarningMessageWindow.xaml
    /// </summary>
    public partial class WarningMessageWindow : Window
    {
        public WarningMessageWindow()
        {
            InitializeComponent();
            this.Name = "AlarmWindow";
            this.DataContext = model.ViewDataModel.WarningData;
            //新建线程
            Thread thread = new Thread(UpdateDataContext);
            thread.Start();
        }
        private void UpdateDataContext()
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                (ThreadStart)delegate ()
                {
                    this.DataContext = model.ViewDataModel.WarningData;
                }
                );
        }
        private void Window_Closed(object sender, System.EventArgs e)
        {
            model.ViewDataModel.WarningData.Property.Flag = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClosedThisWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        

        private void leavebtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Content.Equals("离开"))
            {
                btn.Content = "已回来";
                GlobalCache.LeaveTime = 1;
            }
            else
            {
                btn.Content = "离开";
                GlobalCache.LeaveTime = 0;
            }
        }
    }
}
