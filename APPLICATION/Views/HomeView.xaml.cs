using DATA.UTILITIES.Accessories;
using FaceSysByMvvm.Views.ChannelManager;
using GalaSoft.MvvmLight.Threading;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using xiaowen.codestacks.popwindow;
using tr = SENSING.Plugin.Trace.Views;
using bi = SENSING.Plugin.Intelligent.Views;
using biviewmodel = SENSING.Plugin.Intelligent.ViewModels;
using sc = SC_MODULES.Views;
using SC_MODULES.ViewModels;
using xiaowen.codestacks.data;

namespace SENSING.APPLICATION.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : Window
    {
        public HomeView()
        {
            try
            {
                InitializeComponent();
                Application.Current.MainWindow = this;
                this.Name = "HomeView";
                FuncationArea.Content = moduleChannel;
                this.KeyDown += KeyOpter.OnClose;
                this.WindowState = WindowState.Maximized;
                //心跳
                timer.Tick += new EventHandler(timer_Tick);
                timer.Interval = TimeSpan.FromSeconds(1);   //设置刷新的间隔时间
                timer.Start();
            }
            catch (Exception ex)
            {
                CodeStacksWindow.MessageBox.Invoke(true, false, 2, string.Format("请联系技术人员\n{0}", ex));
            }
        }

        public ChannelManage moduleChannel = new ChannelManage();
        DispatcherTimer timer = new DispatcherTimer();//心跳

        #region 临时解决方案
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void SetTemplatePopWindow(byte[] image)
        {
            TempleteInfoPop tip = new TempleteInfoPop();
            tip.SetTempleteInfo(null, 3, image);
            tip.ShowDialog();
        }
        #endregion

        public void ClearMainWindow()
        {
            ChannelManageToggleButton.IsChecked =
            CompOfRecordsToggleButton.IsChecked =
            CaptureRecordQueryToggleButton.IsChecked =
            TemplateManagerToggleButton.IsChecked =
            StaticCompareToggleBtn.IsChecked =
            BusinessIntelligentToggleBtn.IsChecked =
            LocusAnalyzeToggleBtn.IsChecked = false;

            ChannelManagePolyline.Visibility =
            CompOfRecordsPolyline.Visibility =
            CaptureRecordQueryPolyline.Visibility =
            TemplateManagerPolyline.Visibility =
            StaticComparePolyline.Visibility =
            BusinessIntelligentPolyline.Visibility =
            LocusAnalyzePolyline.Visibility = Visibility.Collapsed;
        }

        public void run(object obj)
        {
            FuncationToggleButton_Checked(BusinessIntelligentToggleBtn, new RoutedEventArgs(ToggleButton.ClickEvent) { Source = obj });
        }

        /// <summary>
        /// 切换功能区按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void FuncationToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ClearMainWindow();
            await Task.Run(() =>
            {
                try
                {
                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        ToggleButton tb = sender as ToggleButton;
                        switch (tb.Tag.ToString())
                        {
                            case "0":
                                ChannelManagePolyline.Visibility = Visibility.Visible;
                                FuncationArea.Content = moduleChannel;
                                //增加地图锚点显示功能
                                break;
                            case "1":
                                CompOfRecordsPolyline.Visibility = Visibility.Visible;
                                CompOfRecords moduleCompare = new CompOfRecords();//comparerecord instance
                                moduleCompare.RefreshChannelComboBox();
                                FuncationArea.Content = moduleCompare;
                                break;
                            case "2":
                                CaptureRecordQueryPolyline.Visibility = Visibility.Visible;
                                CaptureRecordQuery moduleSnap = new CaptureRecordQuery();//sanprecord instance
                                moduleSnap.RefreshChannelComboBox();
                                FuncationArea.Content = moduleSnap;
                                break;
                            case "3":
                                TemplateManagerPolyline.Visibility = Visibility.Visible;
                                TemplateManager moduleTemplate = new TemplateManager();
                                FuncationArea.Content = moduleTemplate;
                                break;
                            case "4":

                                StaticComparePolyline.Visibility = Visibility.Visible;
                                sc.MainControl staticAnalysis = new sc.MainControl();
                                try
                                {
                                    //if (!(e.Source is ToggleButton))
                                    //{
                                    //staticAnalysis.SetImg(e.Source);
                                    MainControlViewModel.ScMainObj.SearchSchemaDatas.ImportImageByteArray = (byte[])e.Source;
                                    MainControlViewModel.ScMainObj.SearchSchemaDatas.ImportImage = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1((byte[])e.Source);
                                    //}
                                }
                                catch (Exception)
                                {
                                }
                                finally
                                {
                                    FuncationArea.Content = staticAnalysis;
                                }
                                break;
                            case "5":
                                LocusAnalyzePolyline.Visibility = Visibility.Visible;
                                tr.MainTraceAnalysisView traceAnalysis = new tr.MainTraceAnalysisView();
                                if (!(e.Source is ToggleButton))
                                {
                                    traceAnalysis.GetPersonInfo(e.Source);
                                }
                                FuncationArea.Content = traceAnalysis;
                                break;
                            case "6":
                                BusinessIntelligentPolyline.Visibility = Visibility.Visible;
                                bi.MainControl businessIntelligate = new bi.MainControl();
                                if (!(e.Source is ToggleButton))
                                {
                                    biviewmodel.MainControlViewModel.ReceivedObj = e.Source;
                                }
                                FuncationArea.Content = businessIntelligate;
                                break;
                        }
                        GC.Collect();
                    });
                }
                catch (Exception)
                {
                }
            });
        }


        ThriftServiceNameSpace.ThriftService thirftTick = new ThriftServiceNameSpace.ThriftService();
        /// <summary>
        /// 心跳
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Tick(object sender, EventArgs e)
        {
            thirftTick.HearBeat();
        }
        
    }
}
