using System;
using System.Linq;
using System.Windows;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using DZVideoWpf;
using FaceSysByMvvm.ViewModels;
using SENSING.ClassPool;
using DATA.UTILITIES.Log4Net;
using System.Threading;
using System.Threading.Tasks;
using xiaowen.codestacks.data;

namespace SENSING_SINGLEUSER.Views.ChannelManage
{
    /// <summary>
    /// VideoPreview.xaml 的交互逻辑
    /// </summary>
    public partial class VideoPreview : Window
    {
        Timer timer = null;
        public VideoPreview()
        {
            InitializeComponent();
        }
        public VideoPreview(ChannelManageViewModel _viewModel) : this()
        {
            viewModel = _viewModel;
            IniVideo();

            timer = new Timer((obj) =>
            {
                Dispatcher.Invoke(() =>
                {
                    listViewContIdentifyResults.ItemsSource =
                FaceSysByMvvm.Views.ChannelManager.ChannelManage._ListIdentifyResults.Where(c => c.ChannelName == _viewModel.CurrentPointChannelListItem.MyChannelCfg.Name);
                });
            }, null, TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(0.5));

            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
        }
        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        public ChannelManageViewModel viewModel
        {
            get { return this.DataContext as ChannelManageViewModel; }
            set { this.DataContext = value; }
        }

        public WindowsFormsHost wfh = new WindowsFormsHost();
        public ChannelListItemViewModel lbi = new ChannelListItemViewModel();
        private async void IniVideo()
        {
            await Task.Run(() =>
            {
                CodeStacksDataHandler.UIThread.Invoke(() =>
                {

                    try
                    {

                        lbi = viewModel.CurrentPointChannelListItem;
                        if (lbi == null)
                        {
                            return;
                        }
                        //txtChannelname.Text = lbi.MyChannelCfg.Name;
                        string ip = lbi.MyChannelCfg.CaptureCfg.TcAddr;
                        if (ip == string.Empty)
                        {
                            System.Windows.MessageBox.Show("摄像机IP为空！");
                            return;
                        }

                        lbi.IsOpened = true;

                        UserControl1 usercontrol = new UserControl1();
                        usercontrol.opencamera(lbi.MyChannelCfg.CaptureCfg.NCaptureType,
                            lbi.MyChannelCfg.CaptureCfg.TcAddr + "|" + lbi.MyChannelCfg.TcDescription,
                            (uint)lbi.MyChannelCfg.CaptureCfg.NPort, lbi.MyChannelCfg.CaptureCfg.TcUID,
                            lbi.MyChannelCfg.CaptureCfg.TcPSW, 1, 1);

                        if (wfh.Tag == null)
                        {
                            wfh.Child = usercontrol;
                            wfh.Tag = lbi.MyChannelCfg.TcChaneelID;
                        }
                        this.VideoPartGrid.Children.Add(wfh);

                        this.UpdateLayout();
                    }
                    catch (Exception ex)
                    {
                        Logger<OperaExcel>.Log.Error("IniVideo", ex);
                    }

                });
            });
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            timer.Dispose();

            if (wfh.Tag != null && wfh.Tag.ToString() == lbi.MyChannelCfg.TcChaneelID)
            {
                (wfh.Child as UserControl1).closecamera();
                wfh = new WindowsFormsHost();
            }
            this.Close();
        }
    }
}
