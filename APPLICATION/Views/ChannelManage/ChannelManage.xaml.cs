using AxReadFileActiveXControlLib;
using DATA.MODELS.GlobalModels;
using DATA.UTILITIES.Log4Net;
using DATA.UTILITIES.SensingFunc;
using DZVideoWpf;
using FaceSysByMvvm.Model;
using FaceSysByMvvm.ViewModels;
using FaceSysByMvvm.ViewModels.ChannelManage;
using GMap.NET;
using SENSING.ClassPool;
using SENSING.THRIFT.Services;
using SENSING_SINGLEUSER.Views.ChannelManage;
using SINGLEUSER.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Xml;
using Thrift.Server;
using Thrift.Transport;
using xiaowen.codestacks.data;
using xiaowen.codestacks.gmap.demo.Models;
using xiaowen.codestacks.popwindow;
using xiaowen.codestacks.popwindow.Views;
using xiaowen.codestacks.wpf.Utilities;
using sensing = xiaowen.codestacks.data.SenSingModels;

namespace FaceSysByMvvm.Views.ChannelManager
{
    /// <summary>
    /// ChannelManage.xaml 的交互逻辑
    /// </summary>
    public partial class ChannelManage : UserControl
    {
        ThriftServiceNameSpace.ThriftService thrift = new ThriftServiceNameSpace.ThriftService();

        UIServerInter _UIServerInter = new UIServerInter();//特定ip
        public TThreadPoolServer server;

        public static ManualResetEvent ResetServerRealtimeCapInfo;          //监听服务器实时上传的抓拍信息
        public static ManualResetEvent ResetServerRealtimeCmpInfo;          //监听服务器实时上传的比对信息 
        public static MyCapFaceLogWithImg _MyCapFaceLogWithImg = new MyCapFaceLogWithImg();//保存服务器上传的抓拍信息
        public static List<MyCapFaceLogWithImg> _ListMyCapFaceLogWithImg = new List<MyCapFaceLogWithImg>();
        //public static byte[] CapimageByteRealtimeCapInfo;                   //传递抓拍实时图片
        public static PublishResult _IdentifyResults = new PublishResult();//保存服务器上传的识别信息
        public static ObservableCollection<PublishResult> _ListIdentifyResults = new ObservableCollection<PublishResult>();
        public static byte[] snapStream;                   //传递实时识别抓拍图片
        public static byte[] campareStream;                   //传递实时识别注册图片
        ChannelManageViewModel _ChannelManageViewModel;
        List<WindowsFormsHost> wFHList = new List<WindowsFormsHost>();
        int currentVideo = 0;                                               //当前正在播放的视频数量
        int currentMaxvideo = 0;                                            //当前分屏允许的最大分屏数量

        Dictionary<string, long> cmpInfoDictionary = new Dictionary<string, long>();

        #region MyRegion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        void SetTemplatePopWindow(byte[] image)
        {
            TempleteInfoPop tIP = new TempleteInfoPop();
            tIP.SetTempleteInfo(null, 3, image);
        }

        #endregion

        ThriftServiceUtilities thriftWifi = null;
        Timer timerWifi = null;
        Timer timerWifiReceive = null;

        string strCmpOldCapID = string.Empty; //上条抓拍ID

        string strClientOldCapID = string.Empty;
        public ChannelManage()
        {
            InitializeComponent();
            _ChannelManageViewModel = new ChannelManageViewModel();
            this.DataContext = _ChannelManageViewModel;
            sanpResultCollection.ItemsSource = _ListMyCapFaceLogWithImg;
            compareResultCollection.ItemsSource = _ListIdentifyResults;
            ResetServerRealtimeCapInfo = new ManualResetEvent(false);
            ResetServerRealtimeCmpInfo = new ManualResetEvent(false);
            //初始化OCX控件载体
            for (int i = 0; i < 16; i++)
            {
                WindowsFormsHost wfh = new WindowsFormsHost();
                wfh.Tag = null;
                wFHList.Add(wfh);
            }
            //初始化默认分屏
            SetVideoGridScreen(1);

            //域值设置
            Parallel.Invoke(
                () =>
                {
                    GlobalCache.ChannelList = ThriftServiceBasic.SelectChannelList();
                },
                () =>
                {
                    int threshold = thrift.QueryThreshold();
                    //if (GlobalCache.AppType == 0)
                    //    _ChannelManageViewModel.SelectedThreshold = threshold < 0 ? 1 : threshold;
                    //else
                    //{
                    //    if (threshold * 1.1176 >= 100)
                    //        _ChannelManageViewModel.SelectedThreshold = 99;
                    //    else
                    //        _ChannelManageViewModel.SelectedThreshold = (int)(threshold * 1.176);
                    //}
                    _ChannelManageViewModel.SelectedThreshold = threshold < 0 ? 1 : threshold;
                });

            if (GlobalCache.AppType == 1)
            {
                comboThreshold.IsEnabled = false;
                _ChannelManageViewModel.RefreshChannelList();
            }

            DrawingAnchorOnMap(false, string.Empty);//01

            #region 临时解决方案

            GlobalCache.ChannelMgrObj = this;

            //打开客户端服务，接收业务服务器上传的实时抓拍和实时识别结果
            Thread ThreadStarServer = new System.Threading.Thread(new ParameterizedThreadStart(StartServer));
            ThreadStarServer.SetApartmentState(ApartmentState.STA);
            ThreadStarServer.Start();

            //#if DEBUG

            //            UnitTest1 test = new UnitTest1();
            //            test.TestMethod1();

            //#endif

            #endregion
            if (GlobalCache.AppType == 0)
                initPastResult();
        }

        
        /// <summary>
        /// 双击通道打开视频
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChannelManageListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ChannelListItemViewModel lbi = null;
                if (sender is ListBox)
                    lbi = (sender as ListBox).SelectedItem as ChannelListItemViewModel;
                else if (sender is ChannelListItemViewModel)
                    lbi = sender as ChannelListItemViewModel;

                if (lbi == null) return;

                string ip = lbi.MyChannelCfg.CaptureCfg.TcAddr;
                if (ip == string.Empty)
                {
                    System.Windows.MessageBox.Show("摄像机IP为空！");
                    return;
                }

                if (lbi.IsOpened == false)
                {
                    OpenOrCloseChannel(lbi.MyChannelCfg.TcChaneelID, true);

                    lbi.IsOpened = true;
                    currentVideo++;
                    UserControl1 usercontrol = new UserControl1();
                    usercontrol.opencamera(lbi.MyChannelCfg.CaptureCfg.NCaptureType, lbi.MyChannelCfg.CaptureCfg.TcAddr + "|" + lbi.MyChannelCfg.TcDescription, (uint)lbi.MyChannelCfg.CaptureCfg.NPort, lbi.MyChannelCfg.CaptureCfg.TcUID, lbi.MyChannelCfg.CaptureCfg.TcPSW, 1, 1);
                    foreach (WindowsFormsHost wfh in wFHList)
                    {
                        if (wfh.Tag == null)
                        {
                            wfh.Child = usercontrol;
                            wfh.Tag = lbi.MyChannelCfg.TcChaneelID;
                            break;
                        }
                    }
                    if (currentVideo > currentMaxvideo)
                    {
                        SetVideoGridScreen(currentVideo);
                    }
                    this.UpdateLayout();
                }
                else
                {
                    OpenOrCloseChannel(lbi.MyChannelCfg.TcChaneelID, false);

                    currentVideo--;
                    foreach (WindowsFormsHost wfh in wFHList)
                    {
                        if (wfh.Tag != null && wfh.Tag.ToString() == lbi.MyChannelCfg.TcChaneelID)
                        {
                            (wfh.Child as UserControl1).closecamera();
                            wfh.Child = null;
                            wfh.Tag = null;
                            break;
                        }
                    }
                    lbi.IsOpened = false;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Interop.DZVideoActiveXLib"))
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "缺少Interop.DZVideoActiveXLib.dll文件，\n请添加该文件到程序目录");
                }
                else
                {
                    Logger<ChannelManage>.Log.Error("ChannelManageListBox_MouseDoubleClick", ex);
                }
            }
        }

        /// <summary>
        /// 测试通道IP是否能连通
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public bool Ping(string ip)
        {
            System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
            System.Net.NetworkInformation.PingOptions options = new System.Net.NetworkInformation.PingOptions();
            options.DontFragment = true;
            string data = "Test Data!";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 1000; // Timeout 时间，单位：毫秒
            System.Net.NetworkInformation.PingReply reply = p.Send(ip, timeout, buffer, options);
            if (reply.Status == System.Net.NetworkInformation.IPStatus.Success)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 开始抓拍
        /// </summary>
        /// <param name="ob"></param>
        public void StartServer(object ob)
        {
            try
            {
                //在新线程中创建监听，接收服务器实时抓拍数据
                ConcurrencyHandler.SetSTAThread.Invoke(new ParameterizedThreadStart(RealtimeSanpListener));
                ConcurrencyHandler.SetSTAThread.Invoke(new ParameterizedThreadStart(RealtimeCmpListener));

                if ("WIFI".Equals(GlobalCache.NetworkMode))
                {
                    thriftWifi = new ThriftServiceUtilities(
                        ResetServerRealtimeCapInfo, ResetServerRealtimeCmpInfo, _IdentifyResults
                        , snapStream, campareStream);
                    //这里我不需要知道端口，我只要去数据库查询数据
                    //然后条用Wifi模块下的两个方法
                    if (GlobalCache.AppType == 0)
                    {
                        timerWifi = new Timer((obj) =>
                    {
                        thriftWifi.SelectSnapAndSnapCompareResultWithPublicFunc(
                           Convert.ToInt32(GlobalCache.SelectedTimeout),
                          Convert.ToInt32(GlobalCache.SelectedTimeout), -1);
                    }, null, GlobalCache.SelectTimeout * 1000, GlobalCache.SelectTimeout * 1000);
                    }
                    else if (GlobalCache.AppType == 1)
                    {
                        timerWifiReceive = new Timer((obj) =>
                        {
                            thriftWifi.SelectSnapAndSnapCompareResultWithPublicFunc(
                           Convert.ToInt32(GlobalCache.SelectedTimeout),
                          Convert.ToInt32(GlobalCache.SelectedTimeout), GlobalCache.AppRegion);
                        }, null, GlobalCache.SelectTimeout * 1000, GlobalCache.SelectTimeout * 1000);
                    }
                }
                else//UIServerInter
                {
                    try//捕获异常
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(System.Environment.CurrentDirectory + @"\AppConfig\userinfo.xml");
                        XmlNodeList startupPortNode = xmlDoc.GetElementsByTagName("StartupPort");
                        XmlNodeList nodes = startupPortNode[0].ChildNodes;

                        XmlNode portNode = nodes[0];
                        int port = int.Parse(portNode.InnerText);
                        TServerSocket serverTransport = new TServerSocket(port, 0, false);
                        UIServer.Processor processor = new UIServer.Processor(_UIServerInter);
                        server = new TThreadPoolServer(processor, serverTransport);
                        server.Serve();
                    }
                    catch (Exception ex)
                    {
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger<ChannelManage>.Log.Error("StartServer", ex);
            }
        }

        /// <summary>
        /// 抓拍监听
        /// </summary>
        /// <param name="obj"></param>
        public void RealtimeSanpListener(object obj)
        {
            try
            {
                while (true)
                {
                    if (ResetServerRealtimeCapInfo.WaitOne())//设置监听等待
                    {
                        //刷新抓拍列表
                        sanpResultCollection.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            this.PushSanpPhotoFromWifiModule();//WIFI module

                            if (!"WIFI".Equals(GlobalCache.NetworkMode))//LAN module
                            {
                                try
                                {
                                    #region Basic
                                    if (snapStream.Length > 0)
                                    {
                                        //读入MemoryStream对象
                                        _MyCapFaceLogWithImg.img = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(snapStream);
                                        _ListMyCapFaceLogWithImg.Insert(0, _MyCapFaceLogWithImg);
                                        _ChannelManageViewModel.CapImageCount++;
                                        if (_ListMyCapFaceLogWithImg.Count > 100)
                                        {
                                            _ListMyCapFaceLogWithImg.RemoveRange(9, 99);
                                        }
                                    }
                                    else
                                    {
                                        return;
                                    }
                                    sanpResultCollection.Items.Refresh();
                                    #endregion
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }));
                        ResetServerRealtimeCapInfo.Reset();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger<ChannelManage>.Log.Error("listViewCaptureResultsDispatcher", ex);
            }
        }

        WarningMessageWindowViewModel viewModel = WarningMessageWindowViewModel.WarnModel;
        public WarningMessageWindow wmw = null;
        List<Window> windows = new List<Window>();
        bool warningWindowIsOpen = false;

        MyCmpFaceLogWidthImgModel willSendObj = new MyCmpFaceLogWidthImgModel();
        List<MyCmpFaceLogWidthImgModel> willDeletedWaringData = new List<MyCmpFaceLogWidthImgModel>();//要删除的告警数据
        /// <summary>
        /// 比对监听
        /// 20170301 取消自动推送功能
        /// </summary>
        /// <param name="obj"></param>
        public void RealtimeCmpListener(object obj)
        {
            bool isCmpSend = false;
            try
            {
                while (true)
                {
                    if (ResetServerRealtimeCmpInfo.WaitOne())//设置监听等待
                    {
                        byte[] tempSnapStream, tempCompareStream = { };

                        if ("WIFI".Equals(GlobalCache.NetworkMode))
                            _IdentifyResults = GlobalCache.IdentifyResult as PublishResult;

                        lock (_IdentifyResults)
                        {
                            PublishResult tempIdentifyResults = _IdentifyResults;
                            tempIdentifyResults.NewCmpResult = tempIdentifyResults.NewCmpResult ?? new RealtimeCmpInfoLBS();

                            if ("WIFI".Equals(GlobalCache.NetworkMode))
                            {
                                tempSnapStream = GlobalCache.SnapStream;
                                tempCompareStream = GlobalCache.SnapCompareStream;
                            }
                            else
                            {
                                tempSnapStream = snapStream;
                                tempCompareStream = campareStream;
                            }

                            //异步线程加载比对记录
                            compareResultCollection.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                //刷新识别结果列表
                                try
                                {
                                    if (tempSnapStream.Length > 0 && tempCompareStream.Length > 0)
                                    {
                                        BitmapImage snapPhoto = new BitmapImage();
                                        BitmapImage scenePhoto = new BitmapImage();

                                        if ("PK".Equals(GlobalCache.AppFunction))
                                        {
                                            if (strCmpOldCapID == string.Empty)
                                            {
                                                strCmpOldCapID = tempIdentifyResults.ID;
                                                _ListIdentifyResults.Insert(0, tempIdentifyResults);
                                            }
                                            else if (strCmpOldCapID == tempIdentifyResults.ID)
                                            {
                                                strCmpOldCapID = tempIdentifyResults.ID;
                                                for (int i = 0; i < _ListIdentifyResults.Count; i++)
                                                {
                                                    if (_ListIdentifyResults[i].ID == tempIdentifyResults.ID)
                                                    {
                                                        _ListIdentifyResults.RemoveAt(i);
                                                    }
                                                }
                                                _ListIdentifyResults.Insert(0, tempIdentifyResults);
                                            }
                                            else
                                            {
                                                strCmpOldCapID = tempIdentifyResults.ID;
                                                _ListIdentifyResults.Insert(0, tempIdentifyResults);
                                            }
                                        }
                                        else
                                        {
                                            _ListIdentifyResults.Insert(0, tempIdentifyResults);
                                        }
                                        
                                        try
                                        {
                                            //Snap Photo
                                            tempIdentifyResults.SanpImage = snapPhoto =
                                               CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(tempSnapStream);
                                            //Template Photo
                                            tempIdentifyResults.TemplateImage = scenePhoto =
                                            CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(tempCompareStream);

                                            tempIdentifyResults.SnapImageBuffer = tempSnapStream;
                                            tempIdentifyResults.TemplateImageBuffer = tempCompareStream;
                                        }
                                        catch (Exception)
                                        {
                                        }

                                        if (tempIdentifyResults.ChannelName.StartsWith("##@")) isCmpSend = true;

                                        _ChannelManageViewModel.ComImageCount++;
                                        if (_ListIdentifyResults.Count > 100)
                                        {
                                            _ListIdentifyResults.Clear();
                                        }
                                        #region 报警弹窗

                                        switch (GlobalCache.AppType)
                                        {
                                            case 0://0-筛选端
                                                this.CatchWaringDataForSendClient(isCmpSend, tempIdentifyResults);
                                                break;
                                            case 1://1-接收端
                                                if ("DEBUG".Equals(GlobalCache.AppMode))
                                                {
                                                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "接收到推送来的告警信息");
                                                }
                                                this.CatchWaringDataForReceivedClient(isCmpSend, tempIdentifyResults);
                                                break;
                                        }

                                        #endregion
                                    }
                                    else
                                    {
                                        Logger<ChannelManage>.Log.Error("【Error】",
                                            new Exception(string.Format("tempSnapStream.Length = {0},tempCompareStream.Length = {1}", tempSnapStream.Length, tempCompareStream.Length)));
                                    }

                                    Thread thread = new Thread(new ThreadStart(Play));
                                    thread.Start();
                                }
                                catch (Exception ex)
                                {
                                    if ("DEBUG".Equals(GlobalCache.AppMode))
                                    {
                                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, "ERROR-0:RealtimeCmpListener\r\n" + ex.Message);
                                    }
                                    Logger<ChannelManage>.Log.Error("ListenWaitServerRealtimeCmpInfo", ex);
                                }
                            }));
                            ResetServerRealtimeCmpInfo.Reset();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if ("DEBUG".Equals(GlobalCache.AppMode))
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "ERROR-1:RealtimeCmpListener\r\n" + ex.Message);
                }
                Logger<OperaExcel>.Log.Error("ListenWaitServerRealtimeCmpInfo", ex);
            }
        }

        #region Publish Warning Data

        /// <summary>
        /// 筛选端报警弹框
        /// </summary>
        /// <param name="isCmpSend"></param>
        /// <param name="tempIRes"></param>
        /// <param name="snapPhoto"></param>
        /// <param name="scenePhoto"></param>
        void CatchWaringDataForSendClient(bool isCmpSend, PublishResult tempIRes)
        {
            #region 01
            //弹窗
            compareResultCollection.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (!"DEMO".Equals(GlobalCache.AppMode))
                    try
                    {
                        warningWindowIsOpen = false;
                        List<Window> windows = new List<Window>();
                        #region 判断窗体是否打开
                        if (!warningWindowIsOpen)
                        {
                            foreach (Window window in Application.Current.Windows)
                            {
                                windows.Add(window);
                            }
                            if (windows.FirstOrDefault(x => x.Name == "AlarmWindow") == null)
                            {
                                warningWindowIsOpen = false;
                            }
                            else
                            {
                                warningWindowIsOpen = true;
                            }
                        }
                        #endregion

                        MyCmpFaceLogWidthImgModel compareResult = new MyCmpFaceLogWidthImgModel();
                        #region 告警Model赋值
                        if (!isCmpSend)
                        {
                            //viewModel.Property.CompareLogData.num = 0;
                            compareResult.ID = tempIRes.ID;//抓拍记录ID
                            compareResult.name = tempIRes.NewCmpResult.Name;
                            compareResult.TypeKey = tempIRes.NewCmpResult.Type;
                            compareResult.type = tempIRes.TemplateType;
                            compareResult.score = tempIRes.NewCmpResult.Score;
                            compareResult.tcUuid = tempIRes.NewCmpResult.ObjID;//模板对象ID
                            compareResult.time = new DateTime(1970, 1, 1).AddSeconds(tempIRes.NewCmpResult.Time).ToString("yyyy/MM/dd HH:mm:ss");
                            compareResult.channelName = tempIRes.ChannelName.Replace("##", "").Replace("@", "");
                            compareResult.channel = tempIRes.NewCmpResult.Channel;
                            compareResult.Address = tempIRes.NewCmpResult.Address;
                        }

                        compareResult.SnapImage = tempIRes.SanpImage;
                        compareResult.TemplatePhoto = tempIRes.TemplateImage;
                        compareResult.SnapImageBuffer = tempIRes.SnapImageBuffer;
                        compareResult.T1 = new CompOfRecordTemplate();
                        compareResult.T1.UserName = tempIRes.NewCmpResult.Name;
                        compareResult.T1.TemplateImageBuffer = tempIRes.TemplateImageBuffer;

                        //viewModel.Property.CompareLogData.IsChecked = true;

                        //viewModel.IsChecked = false;

                        int typeKeyInt = compareResult.TypeKey;
                        string strPath = System.Windows.Forms.Application.StartupPath;

                        switch (typeKeyInt)
                        {
                            case 0:
                                compareResult.Background =
                           new BitmapImage(new Uri(strPath + System.Configuration.ConfigurationManager.AppSettings["Warn_bg_1"]));
                                break;
                            case 1:
                                compareResult.Background =
                           new BitmapImage(new Uri(strPath + System.Configuration.ConfigurationManager.AppSettings["Warn_bg_1"]));
                                break;
                            case 2:
                                compareResult.Background =
                           new BitmapImage(new Uri(strPath + System.Configuration.ConfigurationManager.AppSettings["Warn_bg_2"]));
                                break;
                            case 3:
                                compareResult.Background =
                            new BitmapImage(new Uri(strPath + System.Configuration.ConfigurationManager.AppSettings["Warn_bg_3"]));
                                break;
                            case 4:
                                compareResult.Background =
                               new BitmapImage(new Uri(strPath + System.Configuration.ConfigurationManager.AppSettings["Warn_bg_4"]));
                                break;
                            default:
                                compareResult.Background =
                           new BitmapImage(new Uri(strPath + System.Configuration.ConfigurationManager.AppSettings["Warn_bg_1"]));
                                break;
                        }
                        #endregion

                        if (!"PK".Equals(GlobalCache.AppFunction))
                        {
                            #region 窗体打开或关闭计算出现次数并附加到窗体
                            if (!warningWindowIsOpen)
                            {//窗体关闭时计算出现次数
                                compareResult.AppearCount += 1;
                                compareResult.AppearCount = CountAppearCount(compareResult.AppearCount);
                                viewModel.Property.CompareLogDatas.Insert(0, compareResult);
                            }
                            else
                            {//窗体打开时计算出现次数
                                if (viewModel.Property.CompareLogDatas.Where(x => x.name == compareResult.name && x.channelName == compareResult.channelName).ToList().Count > 0)
                                {//窗体打开时存在 只计算出现次数
                                    for (int i = 0; i < viewModel.Property.CompareLogDatas.Count; i++)
                                    {
                                        var historyObj = viewModel.Property.CompareLogDatas[i];
                                        if (historyObj.tcUuid == compareResult.tcUuid && historyObj.channelName == compareResult.channelName)
                                        {
                                            compareResult.AppearCount = historyObj.AppearCount + 1;
                                            if (viewModel.Property.CompareLogDatas.Remove(historyObj))
                                                break;
                                            //else
                                            //    Logger<ChannelManage>.Log.Error("移除数据失败！123");
                                        }
                                    }
                                    compareResult.AppearCount = CountAppearCount(compareResult.AppearCount);
                                    viewModel.Property.CompareLogDatas.Insert(0, compareResult);
                                }
                                else
                                {//窗体打开时不存在 计算出现次数并附加
                                    compareResult.AppearCount += 1;
                                    compareResult.AppearCount = CountAppearCount(compareResult.AppearCount);
                                    viewModel.Property.CompareLogDatas.Insert(0, compareResult);
                                }
                            }
                            #endregion

                            //viewModel.Property.CompareLogData = compareResult;

                            #region 自动推送
                            if (viewModel.IsChecked)
                            {
                                int res = WarningMessageCmd.SendOneResultInfo(compareResult);
                                if (res != -1)
                                {
                                    return;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region PK
                            compareResult.AppearCount = CountAppearCount(compareResult.AppearCount);
                            if (strClientOldCapID == string.Empty)
                            {
                                strClientOldCapID = compareResult.ID;
                                viewModel.Property.CompareLogDatas.Insert(0, compareResult);
                            }
                            if (strClientOldCapID == compareResult.ID)
                            {
                                strClientOldCapID = compareResult.ID;
                                for (int i = 0; i < viewModel.Property.CompareLogDatas.Count; i++)
                                {
                                    if (viewModel.Property.CompareLogDatas[i].ID == compareResult.ID)
                                    {
                                        viewModel.Property.CompareLogDatas.RemoveAt(i);
                                    }
                                }
                                viewModel.Property.CompareLogDatas.Insert(0, compareResult);
                            }
                            else
                            {
                                strClientOldCapID = compareResult.ID;
                                viewModel.Property.CompareLogDatas.Insert(0, compareResult);
                            }
                            #endregion 
                        }
                        #region 附加告警信息
                        if (!warningWindowIsOpen)
                        {//窗体关闭时附加告警信息 并打开窗体
                            try
                            {
                                ViewDataModel.WarningData = viewModel;
                                viewModel.Wmv = viewModel;
                                wmw = new WarningMessageWindow();
                                wmw.Show();
                                viewModel.RefreshProperty();

                                if (!GlobalCache.AlarmCamera.Contains(compareResult.channel))
                                    GlobalCache.AlarmCamera.Add(compareResult.channel);

                                DrawingAnchorOnMap(true, compareResult.channel);//根据报警情况标红摄像头
                            }
                            catch (Exception)
                            {
                            }
                        }
                        else
                        {//窗体打开时附加告警信息
                            ViewDataModel.WarningData = viewModel;
                            viewModel.Wmv = viewModel;
                            wmw = windows.FirstOrDefault(x => x.Name == "AlarmWindow") as WarningMessageWindow;
                            wmw.Show();

                            wmw.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
                            (ThreadStart)delegate ()
                            {
                                wmw.DataContext = viewModel;
                            });
                            //wmw = windows.FirstOrDefault(x => x.Name == "AlarmWindow") as WarningMessageWindow;
                            //wmw.Activate();
                            if (!GlobalCache.AlarmCamera.Contains(compareResult.channel))
                            {
                                GlobalCache.AlarmCamera.Add(compareResult.channel);
                            }

                            if (!warningWindowIsOpen)
                            {
                                DrawingAnchorOnMap(true, compareResult.channel);//根据报警情况标红摄像头
                            }


                            if (ViewDataModel.WarningData.Property.Flag < 0)
                            {
                                warningWindowIsOpen = false;
                                ViewDataModel.WarningData.Property.Flag = 0;
                            }
                            viewModel.RefreshProperty();
                        }
                        #endregion

                    }
                    catch (Exception ex)
                    {
                        if ("DEBUG".Equals(GlobalCache.AppMode))
                        {
                            CodeStacksWindow.MessageBox.Invoke(false, false, 2, "ERROR-alarm:\r\n" + ex.Message);
                        }
                        Logger<ChannelManage>.Log.Error("CatchWaringDataForSendClient", ex);
                    }
            }));
            #endregion
        }

        /// <summary>
        /// 接受端报警弹框
        /// </summary>
        /// <param name="isCmpSend"></param>
        /// <param name="tempIRes"></param>
        void CatchWaringDataForReceivedClient(bool isCmpSend, PublishResult tempIRes)
        {
            try
            {
                if (!isCmpSend)
                {
                    if (cmpInfoDictionary.ContainsKey(tempIRes.NewCmpResult.Name))
                    {
                        if (tempIRes.NewCmpResult.Time - cmpInfoDictionary[tempIRes.NewCmpResult.Name] <= 120)
                        {
                            cmpInfoDictionary[tempIRes.NewCmpResult.Name] = tempIRes.NewCmpResult.Time;
                        }
                        cmpInfoDictionary[tempIRes.NewCmpResult.Name] = tempIRes.NewCmpResult.Time;
                    }
                    else
                    {
                        cmpInfoDictionary.Add(tempIRes.NewCmpResult.Name, tempIRes.NewCmpResult.Time);
                    }
                }

                sensing.Compare compare = new sensing.Compare();
                compare.Score = tempIRes.NewCmpResult.Score;
                compare.Snap = new sensing.Snap();
                compare.Snap.Guid = tempIRes.ID;
                compare.Snap.DateTime = CodeStacksDataHandler.DateTimeData.ConvertToDateTimeDelegate(tempIRes.NewCmpResult.Time).ToString();
                compare.Snap.Photo = tempIRes.SanpImage;
                compare.Camera = new sensing.Camera();
                compare.Camera.Location = tempIRes.NewCmpResult.Channelname.Replace("##@", "");
                compare.Template = new sensing.Template();
                compare.Template.TypeValue = tempIRes.TemplateType;
                compare.Template.Guid = tempIRes.NewCmpResult.ObjID;
                compare.Template.PersonInfo = new sensing.Person();
                compare.Template.PersonInfo.Name = tempIRes.NewCmpResult.Name.Replace("|", "").Replace("姓 名：", "");
                compare.Template.PersonInfo.Photo = tempIRes.TemplateImage;
                compare.Template.TypeKey = tempIRes.NewCmpResult.Type;
                compare.Captype = "发 现 可 疑 人 员";

                ThriftServiceNameSpace.ThriftService thirft = new ThriftServiceNameSpace.ThriftService();
                List<byte[]> senceImg = thirft.QuerySenceImg(tempIRes.ID, DateTime.Parse(compare.Snap.DateTime).ToString("yyyyMMdd"));
                if (senceImg != null && senceImg.Count > 0 && senceImg[0].Length > 0)
                {
                    compare.Snap.EnvironmentPhoto = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(senceImg[0]);
                }
                string strPath = System.Windows.Forms.Application.StartupPath;

                switch (compare.Template.TypeKey)
                {
                    case 0:
                        compare.Template.TypePhotoPath = strPath + System.Configuration.ConfigurationManager.AppSettings["accept_Warn_bg_1"];
                        break;
                    case 1:
                        compare.Template.TypePhotoPath = strPath + System.Configuration.ConfigurationManager.AppSettings["accept_Warn_bg_1"];
                        break;
                    case 2:
                        compare.Template.TypePhotoPath = strPath + System.Configuration.ConfigurationManager.AppSettings["accept_Warn_bg_2"];
                        break;
                    case 3:
                        compare.Template.TypePhotoPath = strPath + System.Configuration.ConfigurationManager.AppSettings["accept_Warn_bg_3"];
                        break;
                    case 4:
                        compare.Template.TypePhotoPath = strPath + System.Configuration.ConfigurationManager.AppSettings["accept_Warn_bg_4"];
                        break;
                    default:
                        compare.Template.TypePhotoPath = strPath + System.Configuration.ConfigurationManager.AppSettings["accept_Warn_bg_1"];
                        break;
                }
                CodeStacksComparePopInfo pop = new CodeStacksComparePopInfo();
                pop.SetResultValue(compare);
                pop.ShowDialog();
            }
            catch (Exception ex)
            {
                if ("DEBUG".Equals(GlobalCache.AppMode))
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "\r\n接收端，在接收到推送的时候出现异常");
                }
                Logger<ChannelManage>.Log.Error("listViewContIdentifyResultsDispatcher", ex);
            }
        }

        #endregion

        #region 20170420 old

        ///// <summary>
        ///// 推送比对记录
        ///// </summary>
        ///// <param name="info"></param>
        ///// <param name="IP"></param>
        ///// <returns></returns>
        //public int UpdateCmp(RealtimeCmpInfo info, string IP)
        //{
        //    try
        //    {
        //        TSocket tsocket = new TSocket(IP, 6000);
        //        tsocket.Timeout = 3000;
        //        TTransport transport = tsocket;
        //        Thrift.Protocol.TProtocol protocol = new Thrift.Protocol.TBinaryProtocol(transport);
        //        UIServer.Client _BusinessServerClient = new UIServer.Client(protocol);
        //        transport.Open();
        //        _BusinessServerClient.UpdateRealtimeCmp(info, "##@" + info.Channel);
        //        transport.Close();
        //        return 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger<OperaExcel>.Log.Error("UpdateCmp", ex);
        //        return -1;
        //    }
        //} 
        #endregion

        public SoundPlayer soundPlayer;
        /// <summary>
        /// 播放告警声音
        /// </summary>
        /// <param name="obj"></param>
        private void Play()
        {
            try
            {
                if (!string.IsNullOrEmpty(GlobalCache.AudioName))
                {
                    string path = System.Environment.CurrentDirectory + @"\Resources\Audio\" + GlobalCache.AudioName.Trim();
                    if (File.Exists(path))
                    {
                        soundPlayer = new SoundPlayer(path);
                        soundPlayer.Play();
                    }
                    else
                    {
                        CodeStacksWindow.MessageBox(false, false, 2, @"\Resources\Audio\目录下\r\n缺少音频文件");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger<ChannelManage>.Log.Error("Play Audio Error", ex);
            }
        }

        /// <summary>
        /// 设置分屏,并添加子项
        /// </summary>
        /// <param name="i">几分屏</param>
        public void SetVideoGridScreen(int sceenCount)
        {
            currentMaxvideo = sceenCount;
            try
            {
                //设置分屏,添加行列
                int rowCount = 0;
                int ColCount = 0;
                foreach (Grid thing in VideoPartGrid.Children)
                {
                    thing.Children.Clear();
                }
                VideoPartGrid.Children.Clear();
                VideoPartGrid.RowDefinitions.Clear();
                VideoPartGrid.ColumnDefinitions.Clear();
                switch (sceenCount)
                {
                    case 1:
                        rowCount = 1;
                        ColCount = 1;
                        break;
                    case 2:
                        rowCount = 2;
                        ColCount = 1;
                        break;
                    case 3:
                        rowCount = 2;
                        ColCount = 2;
                        break;
                    case 4:
                        rowCount = 2;
                        ColCount = 2;
                        break;
                    case 5:
                    case 6:
                        rowCount = 3;
                        ColCount = 2;
                        break;
                    case 7:
                    case 8:
                    case 9:
                        rowCount = 3;
                        ColCount = 3;
                        break;
                    case 10:
                    case 11:
                    case 12:
                        rowCount = 4;
                        ColCount = 3;
                        break;
                    case 13:
                    case 14:
                    case 15:
                    case 16:
                        rowCount = 4;
                        ColCount = 4;
                        break;
                }
                for (int i = 1; i <= rowCount; i++)
                {
                    VideoPartGrid.RowDefinitions.Add(new RowDefinition());
                }
                for (int i = 1; i <= ColCount; i++)
                {
                    VideoPartGrid.ColumnDefinitions.Add(new ColumnDefinition());
                }

                //添加子项
                for (int i = 0; i < sceenCount; i++)
                {
                    Grid screenGrid = new Grid();
                    screenGrid.Background = this.FindResource("ViedoBackground") as ImageBrush;
                    if (sceenCount == 3 && i == 2)
                    {
                        screenGrid.SetValue(Grid.RowProperty, i / ColCount);
                        screenGrid.SetValue(Grid.ColumnProperty, i % ColCount);
                        screenGrid.SetValue(Grid.ColumnSpanProperty, 2);
                    }
                    else
                    {
                        screenGrid.SetValue(Grid.RowProperty, i / ColCount);
                        screenGrid.SetValue(Grid.ColumnProperty, i % ColCount);
                    }
                    screenGrid.Children.Add(wFHList[i]);
                    VideoPartGrid.Children.Add(screenGrid);
                }
            }
            catch (Exception ex)
            {
                Logger<ChannelManage>.Log.Error("SetVideoGridScreen", ex);
            }
        }

        /// <summary>
        /// 设置分屏按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetScreen_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Button bt = sender as Button;
                string strBtnName = bt.Name;
                //判断按钮名称
                switch (strBtnName)
                {
                    case "btnOneScreen":
                        SetVideoGridScreen(1);
                        break;
                    case "btnTowScreen":
                        SetVideoGridScreen(2);
                        break;
                    case "btnThreeScreen":
                        SetVideoGridScreen(3);
                        break;
                    case "btnFourScreen":
                        SetVideoGridScreen(4);
                        break;
                    case "btnSixScreen":
                        SetVideoGridScreen(6);
                        break;
                    case "btnNineScreen":
                        SetVideoGridScreen(9);
                        break;
                    case "btnTwelveScreen":
                        SetVideoGridScreen(12);
                        break;
                    case "btnSixteenScreen":
                        SetVideoGridScreen(16);
                        break;
                }
                Pop.IsOpen = false;

            }
            catch (Exception ex)
            {
                Logger<OperaExcel>.Log.Error("btnSetScreen_Click", ex);
            }
        }

        /// <summary>
        /// 增加频道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPassageWay_Click(object sender, RoutedEventArgs e)
        {
            ChannelInfo channelInfo = new ChannelInfo();
            channelInfo.RefreshChannelDelegate = _ChannelManageViewModel.RefreshChannelList;
            bool? b = channelInfo.ShowDialog();
            if (b != null && b == false)
                DrawingAnchorOnMap(false, null);
        }

        /// <summary>
        /// 修改频道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifyPassageWay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //todo(已经完成未测试) 先关闭视频 再进行修改
                if (ChannelManageListBox.SelectedIndex < 0)
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "请选中需要修改的通道!");
                    return;
                }

                ChannelListItemViewModel lbi = ChannelManageListBox.SelectedItem as ChannelListItemViewModel;
                ChannelInfo channelInfo = new ChannelInfo();
                channelInfo.RefreshChannelDelegate = _ChannelManageViewModel.RefreshChannelList;
                channelInfo.CloseVideoDelegate = CloseVideo;
                channelInfo.SetChannelInfo(lbi.MyChannelCfg);
                channelInfo.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger<ChannelManage>.Log.Error("btnModifyPassageWay_Click", ex);
            }
        }

        /// <summary>
        /// listview宽度自动变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewContIdentifyResults_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            try
            {
                //获得识别结果listview
                GridView gv = compareResultCollection.View as GridView;
                if (gv != null)//listview存在
                {
                    if (compareResultCollection.ActualWidth <= 0)
                    {
                        return;
                    }
                    //获得第一列
                    GridViewColumn gvc = gv.Columns[0];
                    //设置第一列列宽
                    gvc.Width = (compareResultCollection.ActualWidth - 4) / 3;

                    //获得第二列
                    gvc = gv.Columns[1];
                    //设置第二列列宽
                    gvc.Width = (compareResultCollection.ActualWidth - 4) / 3;

                    //获得第三列
                    gvc = gv.Columns[2];
                    //设置第三列列宽
                    gvc.Width = (compareResultCollection.ActualWidth - 4) / 3;
                }
            }
            catch (Exception ex)
            {
                Logger<OperaExcel>.Log.Error("listViewContIdentifyResults_SizeChanged_1", ex);
            }
        }

        /// <summary>
        /// 比对双击 显示比对详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewContIdentifyResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                sensing.Compare compare = new sensing.Compare();
                if (compareResultCollection.SelectedItem is PublishResult)
                {
                    PublishResult Results = compareResultCollection.SelectedItem as PublishResult;
                    if (Results == null) return;

                    compare.Score = Results.NewCmpResult.Score;
                    compare.Snap = new sensing.Snap();
                    compare.Snap.Photo = Results.SanpImage;
                    compare.Template = new sensing.Template();
                    compare.Template.TypeValue = Results.TemplateType;
                    compare.Template.PersonInfo = new sensing.Person();
                    compare.Template.PersonInfo.Name = Results.NewCmpResult.Name.Replace("|", "").Replace("姓 名：", "");
                    compare.Template.TypeValue = Results.TemplateType;
                    compare.Template.PersonInfo.Photo = Results.TemplateImage;
                    compare.Snap.DateTime = CodeStacksDataHandler.DateTimeData.ConvertToDateTimeDelegate.Invoke(Results.NewCmpResult.Time).ToString();
                    compare.Camera = new sensing.Camera();
                    compare.Camera.Location = Results.NewCmpResult.Channelname.Replace("##@", "");
                    compare.Captype = "场 景 留 存";

                    ThriftServiceNameSpace.ThriftService thirft = new ThriftServiceNameSpace.ThriftService();
                    List<byte[]> senceImg = thirft.QuerySenceImg(Results.ID, DateTime.Parse(compare.Snap.DateTime).ToString("yyyyMMdd"));
                    if (senceImg != null && senceImg.Count > 0 && senceImg[0].Length > 0)
                    {
                        compare.Snap.EnvironmentPhoto = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(senceImg[0]);
                    }

                    string strPath = System.Windows.Forms.Application.StartupPath;
                    compare.Template.TypePhotoPath = strPath + System.Configuration.ConfigurationManager.AppSettings["accept_Warn_bg_1"];

                    CodeStacksComparePopInfo pop = new CodeStacksComparePopInfo();
                    pop.SetResultValue(compare);
                    pop.ShowDialog();
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 删除频道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeletePassageWay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int success = -1;
                if (ChannelManageListBox.SelectedIndex < 0)
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "请选中需要删除的通道!");
                    return;
                }
                ChannelListItemViewModel lbi = ChannelManageListBox.SelectedItem as ChannelListItemViewModel;
                if (CodeStacksWindow.MessageBox.Invoke(true, false, 2, "是否删除通道?") == true)
                {
                    #region 20170531 薛保波
                    string strID = lbi.MyChannelCfg.TcChaneelID;
                    if (strID.Contains("\\") && !strID.Contains("\\\\"))
                    {
                        strID = strID.Replace("\\", "\\\\");
                    }
                    success = thrift.DelChannel(strID);
                    #endregion 
                    if (success == 0)
                    {
                        CloseVideo();
                        _ChannelManageViewModel.RefreshChannelList();
                        DrawingAnchorOnMap(false, null);
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, "删除通道成功！");
                    }
                    else
                    {
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, "删除通道失败！");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger<ChannelManage>.Log.Error("btnDeletePassageWay_Click", ex);
            }
        }

        /// <summary>
        /// 关闭摄像头
        /// </summary>
        private void CloseVideo()
        {
            try
            {
                ChannelListItemViewModel lbi = ChannelManageListBox.SelectedItem as ChannelListItemViewModel;
                if (lbi.IsOpened == true)
                {
                    currentVideo--;
                    foreach (WindowsFormsHost wfh in wFHList)
                    {
                        if (wfh.Tag != null && wfh.Tag.ToString() == lbi.MyChannelCfg.TcChaneelID)
                        {
                            (wfh.Child as UserControl1).closecamera();
                            wfh.Child = null;
                            wfh.Tag = null;
                        }
                    }
                    lbi.IsOpened = false;
                }
            }
            catch (Exception ex)
            {
                Logger<ChannelManage>.Log.Error("CloseVideo", ex);
            }
        }


        /// <summary>
        /// 双击抓拍照片 添加模版
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewCaptureResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                MyCapFaceLogWithImg cmpFaceLogWidthImg = sanpResultCollection.SelectedItem as MyCapFaceLogWithImg;
                if (cmpFaceLogWidthImg == null)
                {
                    return;
                }
                List<byte[]> listImageBytes = new List<byte[]>();
                listImageBytes = thrift.QueryCapLogImageH(cmpFaceLogWidthImg.ID, DateTime.Parse(cmpFaceLogWidthImg.time).ToString("yyyyMMdd"));
                TempleteInfoPop tIP = new TempleteInfoPop();
                if (listImageBytes.Count <= 0)
                {
                    return;
                }
                else
                {
                    tIP.SetTempleteInfo(null, 3, listImageBytes[0]);
                }
                tIP.ShowDialog();
            }
            catch (Exception ex)
            {
                Logger<ChannelManage>.Log.Error("listViewCaptureResults_MouseDoubleClick", ex);
            }
        }
        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            return t;
        }

        /// <summary>
        /// 刷新通道列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshChannel_Click(object sender, RoutedEventArgs e)
        {
            _ChannelManageViewModel.RefreshChannelList();
            DrawingAnchorOnMap(false, null);
        }

        /// <summary>
        /// 重新设置阈值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GlobalCache.AppType == 1) return;
            ComboBox cb = sender as ComboBox;
            thrift.SetCMPthreshold(_ChannelManageViewModel.SelectedThreshold);
        }


        private bool VisibilityState = false;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!VisibilityState)
            {
                mapBar.Visibility = Visibility.Visible;
                GMap.Visibility = Visibility.Visible;
                VideoPartGrid.Visibility = Visibility.Collapsed;
                VisibilityState = true;
                videoplayer.Visibility = Visibility.Collapsed;
            }
            else
            {
                mapBar.Visibility = Visibility.Collapsed;
                GMap.Visibility = Visibility.Collapsed;
                VideoPartGrid.Visibility = Visibility.Visible;
                VisibilityState = false;
                videoplayer.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isAlarm"></param>
        void DrawingAnchorOnMap(bool isAlarm, string channel)
        {
            try
            {
                MainMap.Points = new ObservableCollection<PointLatLng>();

                foreach (MyChannelCfg mcc in GlobalCache.ChannelList)
                {
                    double lat = GlobalCache.Latitude;
                    double lng = GlobalCache.Longitude;
                    bool isLat = Double.TryParse(mcc.Latitude, out lat);
                    bool isLng = Double.TryParse(mcc.Longitude, out lng);

                    if (isLat && isLng)
                    {
                        MainMap.MouseLeftBtnUp = MainMap_AnchorMouseLeftButtonUp;
                        MainMap.MouseLeftBtnUp_Args = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                        {
                            RoutedEvent = UIElement.MouseLeftButtonUpEvent,
                            Source = mcc.TcChaneelID
                        };

                        MainMap.Points.Add(new PointLatLng(
                           lat, lng,
                           "Camera",
                           GlobalCache.AlarmCamera.Contains(mcc.TcChaneelID) ? CodeStacksDataHandler.ImageData.ConvertToImageSourceDelegate1("pack://application:,,,/Images/home-icon-redcamera.png") : CodeStacksDataHandler.ImageData.ConvertToImageSourceDelegate1.Invoke("pack://application:,,,/Images/home-icon-bluecamera.png"),
                           new GeoTitle()
                           {
                               IsVisible = Visibility.Visible,
                               Header = "通道信息",
                               Content1 = "通道：",
                               Content1Value = mcc.Name,
                               Content1Visible = Visibility.Visible,
                               Content2 = "经度：",
                               Content2Value = mcc.Longitude,
                               Content2Visible = Visibility.Visible,
                               Content3 = "纬度：",
                               Content3Value = mcc.Latitude,
                               Content3Visible = Visibility.Visible,
                               Content4 = "地址：",
                               Content4Value = mcc.Channel_address,
                               Content4Visible = Visibility.Visible
                           }));
                    }
                    else
                    {
                        MainMap.Points.Add(new PointLatLng(GlobalCache.Latitude, GlobalCache.Longitude, string.Empty, null, new GeoTitle()));
                    }
                }
                MainMap.IsMapCtrlVisibale = Visibility.Collapsed;
                MainMap.MapRefresh.Invoke(null, null);
            }
            catch (Exception ex)
            {
                Logger<ChannelManage>.Log.Error("DrawingAnchorOnMap", ex);
            }
        }

        #region MainMap_AnchorMouseLeftButtonUp
        private void MainMap_AnchorMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (_ChannelManageViewModel.ChannelList != null && _ChannelManageViewModel.ChannelList.Count > 0)
                {
                    foreach (ChannelListItemViewModel channel in _ChannelManageViewModel.ChannelList)
                    {
                        if (channel.MyChannelCfg.TcChaneelID == e.Source.ToString())
                        {
                            _ChannelManageViewModel.CurrentPointChannelListItem = channel;
                            break;
                        }
                    }
                }
                if (_ChannelManageViewModel.CurrentPointChannelListItem != null)
                {
                    var video1 = Application.Current.Windows.Cast<Window>().FirstOrDefault(x => x is VideoPreview);
                    if (video1 == null)
                    {
                        VideoPreview video = new VideoPreview(_ChannelManageViewModel);
                        video.Show();
                    }
                    else
                    {
                        VideoPreview video2 = video1 as VideoPreview;
                        video2.Close();
                        if (video2.wfh.Tag != null && video2.wfh.Tag.ToString() == video2.lbi.MyChannelCfg.TcChaneelID)
                        {
                            (video2.wfh.Child as UserControl1).closecamera();
                            video2.wfh.Child = null;
                            video2.wfh.Tag = null;
                        }

                        VideoPreview video = new VideoPreview(_ChannelManageViewModel);
                        video.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger<ChannelManage>.Log.Error("ChannelManageListBox_MouseDoubleClick", ex);
            }
        }
        #endregion

        #region INIT SNAP、COMPARE RESULT FOR ＳＴＡＲＴＵＰ　ＰＡＧＥ

        //listViewContIdentifyResults Compareresult
        //listViewCaptureResults snapresult
        public void initPastResult()
        {
            try
            {
                var initResult = _ChannelManageViewModel.initPastReuslt(); //task.Result;
                var snapResult = initResult.Caplist;
                var compareReuslt = initResult.Cmplist;
                IList<PublishResult> cmpResult = new List<PublishResult>();

                foreach (var item in snapResult)
                {
                    PublishResult r = new PublishResult();
                    _ListMyCapFaceLogWithImg.Add(new MyCapFaceLogWithImg
                    {
                        ID = item.Id,
                        time = CodeStacksDataHandler.DateTimeData.ConvertToDateTimeDelegate(item.Time).ToString(),
                        img = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(item.Image),
                        ChannelName = item.Channelname
                    });
                }

                foreach (var item in compareReuslt)
                {
                    PublishResult r = new PublishResult();
                    r.ID = item.CapID;
                    r.NewCmpResult = item;
                    r.TemplateType = BasicDataEntry.GetTemplateType(item.Type);
                    StringBuilder info = new StringBuilder();
                    info.Append(item.Name + "\r\n");//Template Name
                    info.Append(item.Channelname + "\r\n");//Channel
                    info.Append(CodeStacksDataHandler.DateTimeData.ConvertToDateTimeDelegate(item.Time).ToShortDateString() + "\r\n");//Date
                    info.Append(CodeStacksDataHandler.DateTimeData.ConvertToDateTimeDelegate(item.Time).ToShortTimeString() + "\r\n");//Time
                    info.Append(item.Score + "\r\n");//Score
                    r.SanpImage = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(item.CapImg);
                    r.SanpImage.Freeze();
                    if (item.ObjImg.Length > 0)
                    {
                        r.TemplateImage = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(item.ObjImg);
                        r.TemplateImage.Freeze();
                    }
                    else
                    {
                        r.TemplateImage = CodeStacksDataHandler.ImageData.ConvertToImageSourceDelegate1("pack://application:,,,/Images/unkonw.png");
                    }

                    r.RegInfo = info.ToString();
                    _ListIdentifyResults.Add(r);
                }
            }
            catch (Exception ex)
            {
                Logger<ChannelManage>.Log.Error("initPastResult", ex);
            }
        }

        async Task<LastRecordInfo> SelectPastSnapCompareInfo()
        {
            return await Task<LastRecordInfo>.Run(() =>
             {
                 return _ChannelManageViewModel.initPastReuslt();
             }).ConfigureAwait(false);
        }

        #endregion

        private void ChannelManageListBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ChannelListItemViewModel item = ChannelManageListBox.SelectedItem as ChannelListItemViewModel;

                double lat = GlobalCache.Latitude;
                double lng = GlobalCache.Longitude;
                if (double.TryParse(item.MyChannelCfg.Latitude, out lat) && double.TryParse(item.MyChannelCfg.Longitude, out lng))
                    MainMap.Position.Invoke(new PointLatLng(lat, lng));
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rfaxControl != null)
                {
                    rfaxControl.Pause();
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void close_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (rfaxControl != null)
                {
                    this.stop.IsChecked = false;
                    if ("DEBUG".Equals(GlobalCache.AppMode))
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, "close video file !");

                    rfaxControl.CloseVideoFile();
                    wFHList[0] = new WindowsFormsHost();
                }
            }
            catch (Exception)
            {
            }
        }


        private AxReadFileActiveXControl rfaxControl = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void opener_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.stop.IsChecked = false;
                if (VideoPartGrid != null && VideoPartGrid.Children.Count > 0)
                {
                    List<ChannelListItemViewModel> channelIsOpend =
                        ChannelManageListBox.Items.Cast<ChannelListItemViewModel>().Where(x => x.IsOpened == true).ToList();
                    foreach (var item in channelIsOpend)
                    {
                        ChannelManageListBox_MouseDoubleClick(item, null);
                    }
                }

                System.Windows.Forms.OpenFileDialog _openFileDialog = new System.Windows.Forms.OpenFileDialog();
                _openFileDialog.Filter = "音频文|*.mp3;*.mp4;*.wma;*.aac;*.midi;*.wav;*.ts";
                System.Windows.Forms.DialogResult result = _openFileDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string StrPath = _openFileDialog.FileName;

                    if (HasChinese(StrPath))
                    {
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, "视频名称和路径不能包含中文字符！");
                        return;
                    }
                    FileVideoPlayer(StrPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="StrPath"></param>
        void FileVideoPlayer(object StrPath)
        {
            if (rfaxControl == null) rfaxControl = new AxReadFileActiveXControl();

            if ("DEBUG".Equals(GlobalCache.AppMode))
            {
                if (rfaxControl == null)
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "rfaxControl not NULL");
                else
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "rfaxControl is not NULL");
            }

            rfaxControl.BeginInit();

            try
            {
                wFHList[0].Child = rfaxControl;

                SetVideoGridScreen(1);

                rfaxControl.EndInit();
                rfaxControl.CloseVideoFile();
                rfaxControl.OpenVideoFile(StrPath.ToString());
                rfaxControl.SetServerInfo(GlobalCache.Host, GlobalCache.Port);

                int playResult = rfaxControl.Play();

                //insert virtual channel
                if (playResult == 0)
                {
                    InsetVirtualChannel(StrPath.ToString());
                }
            }
            catch (Exception)
            {
            }
        }


        /// <summary>
        /// building to channel
        /// </summary>
        /// <param name="channelName"></param>
        async void InsetVirtualChannel(string channelName)
        {
            await Task.Run(() =>
            {
                var res = GlobalCache.ChannelList.Cast<MyChannelCfg>().FirstOrDefault(t => t.TcChaneelID == channelName);
                if (res == null)
                {
                    try
                    {
                        string fileName = Path.GetFileName(channelName);
                        ChannelCfgLBS channel = new ChannelCfgLBS();
                        ThriftServiceNameSpace.ThriftService thrift = new ThriftServiceNameSpace.ThriftService();

                        channel.TcChannelID = channelName; //Guid.NewGuid().ToString().Replace("-", "");
                        channel.TcUID = channelName;
                        channel.Name = fileName;
                        channel.Channel_type = 2000;
                        channel.Latitude = GlobalCache.Latitude.ToString();
                        channel.Longitude = GlobalCache.Longitude.ToString();
                        int result = thrift.AddChannel(channel);
                        if (result != 0)
                        {
                            CodeStacksWindow.MessageBox.Invoke(false, false, 2, "创建通道失败，请联系技术人员检查关联内容");
                        }
                        else
                        {
                            GlobalCache.ChannelList = ThriftServiceBasic.SelectChannelList();
                        }
                    }
                    catch (Exception)
                    {
                        if ("DEBUG".Equals(GlobalCache.AppMode))
                        {
                            CodeStacksWindow.MessageBox.Invoke(false, false, 2, "创建虚拟通道出错");
                        }
                    }
                }
            });
        }


        public static bool HasChinese(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }

        private void ClearwFHList()
        {
            try
            {
                for (int i = 0; i < VideoPartGrid.Children.Count; i++)
                {
                    if (VideoPartGrid.Children[i] is Grid)
                    {
                        ((Grid)VideoPartGrid.Children[i]).Children.Clear();
                    }
                }
                VideoPartGrid.Children.Clear();
                VideoPartGrid.RowDefinitions.Clear();
                VideoPartGrid.ColumnDefinitions.Clear();

                Grid screenGrid = new Grid();
                screenGrid.Background = this.FindResource("ViedoBackground") as ImageBrush;
                VideoPartGrid.Children.Add(screenGrid);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void video_Checked(object sender, RoutedEventArgs e)
        {
            if (this.stop.IsChecked == true)
            {
                this.stop.Content = "播 放";
                this.opener.IsChecked = false;
                this.close.IsChecked = false;
                //this.stop.IsChecked = false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void video_UnChecked(object sender, RoutedEventArgs e)
        {
            if (this.stop.IsChecked == false)
            {
                this.stop.Content = "暂 停";
                this.opener.IsChecked = false;
                this.close.IsChecked = false;
                //this.stop.IsChecked = true;
            }
        }


        private void sanpResultCollection_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            _ChannelManageViewModel.SnapImage = sanpResultCollection.SelectedItem;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(stopBtnClick), null);
        }

        private async void stopBtnClick()
        {
            stop_Click(null, null);
            await Task.Run(() => Thread.Sleep(1000));
            stop_Click(null, null);
            await Task.Delay(200);
        }

        /// <summary>
        /// 监听AppearCount是否下雨等于0
        /// </summary>
        /// <param name="AppearCount"></param>
        /// <returns></returns>
        private long CountAppearCount(long AppearCount)
        {
            if (AppearCount <= 0)
            {
                AppearCount = 1;
            }
            return AppearCount;
        }
        /// <summary>
        /// 打开关闭通道
        /// </summary>
        /// <param name="strChannelID">通道ID</param>
        /// <param name="flag">true 打开通道 false 关闭通道</param>
        /// <returns></returns>
        private int OpenOrCloseChannel(string strChannelID, bool flag)
        {
            int res = -1;
            if (strChannelID.Contains("\\") && !strChannelID.Contains("\\\\"))
            {
                strChannelID = strChannelID.Replace("\\", "\\\\");
            }

            if (flag)
            {
                res = thrift.OpenChannel(strChannelID);
            }
            else
            {
                res = thrift.CloseChannel(strChannelID);
            }
            return res;
        }


    }
}
