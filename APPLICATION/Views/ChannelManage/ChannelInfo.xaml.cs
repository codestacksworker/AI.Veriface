using FaceSysByMvvm.ViewModels.ChannelManage;
using SENSING.ClassPool;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GMap.NET;
using DATA.MODELS.GlobalModels;
using DATA.UTILITIES.Log4Net;
using xiaowen.codestacks.popwindow;
using SENSING.THRIFT.Services;
using xiaowen.codestacks.gmap.demo.Models;

namespace FaceSysByMvvm.Views.ChannelManager
{
    /// <summary>
    /// ChannelInfo.xaml 的交互逻辑
    /// </summary>
    public partial class ChannelInfo : Window
    {
        MyChannelCfg _ChannelCfg = new MyChannelCfg();
        ChannelInfoViewModel cIViewModel;
        public System.IO.FileInfo[] filesPic;
        ThriftServiceNameSpace.ThriftService thirft = new ThriftServiceNameSpace.ThriftService();
        validationRule _validationRule = new validationRule();
        public Action RefreshChannelDelegate;
        public Action CloseVideoDelegate;

        public Action<bool, string> RefreshChannelAction;

        public ChannelInfo()
        {
            InitializeComponent();
            GlobalCache.ChannePropertiesList = ThriftServiceBasic.SelectChannelPropertiesList();
            cIViewModel = new ChannelInfoViewModel();
            this.DataContext = cIViewModel;
            btnChannelNumGeneration.IsEnabled = true;
            try
            {
                MainMap.Points = new System.Collections.ObjectModel.ObservableCollection<PointLatLng>();
                MainMap.IsMapCtrlVisibale = Visibility.Collapsed;
                MainMap.Points.Add(new PointLatLng(GlobalCache.Latitude, GlobalCache.Longitude, "Red", null, new GeoTitle()));
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 重新生成编号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChannelNumGeneration_Click(object sender, RoutedEventArgs e)
        {
            cIViewModel.ChannelId = System.Guid.NewGuid().ToString().Replace("-", "");
        }

        /// <summary>
        /// 确认
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirmAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int nSucess = -1;
                //判断是添加还是修改
                if (!CheckInfo()) return;

                ChannelCfgLBS channelCfg = new ChannelCfgLBS();
                channelCfg = _ChannelCfg.MyChannelCfgToChannelCfgLBS(_ChannelCfg);
                channelCfg.Channel_type = cIViewModel.SelectedChannelType;
                channelCfg.NState = 0;
                double x = 0;
                double y = 0;

                if (string.IsNullOrWhiteSpace(channelCfg.Latitude) || string.IsNullOrWhiteSpace(channelCfg.Longitude) || !double.TryParse(channelCfg.Latitude, out x) || !double.TryParse(channelCfg.Longitude, out y))
                {
                    CodeStacksWindow.MessageBox(false, false, 1, "通道坐标不能为空，请正确填写通道坐标");
                    return;
                }

                if (cIViewModel.Title.Equals("添加通道"))
                {
                    nSucess = thirft.AddChannel(channelCfg);
                }
                else
                {
                    nSucess = thirft.ModifyChannel(channelCfg);
                }

                if (nSucess == 0)
                {
                    CodeStacksWindow.MessageBox.Invoke(true, false, 2, "操作通道成功！");
                    //修改通道 如果通道已经被打开则关闭
                    if (!cIViewModel.Title.Equals("添加通道"))
                    {
                        CloseVideoDelegate();
                    }
                    this.Close();
                }
                else
                {
                    CodeStacksWindow.MessageBox.Invoke(true, false, 2, "操作失败！");
                    return;
                }
                RefreshChannelDelegate();
            }
            catch (Exception ex)
            {
                Logger<ChannelInfo>.Log.Error("btnConfirmAdd_Click", ex);
            }
        }

        /// <summary>
        /// 检测各项的值
        /// </summary>
        private bool CheckInfo()
        {
            try
            {
                //通道uuid，默认生成
                if (!string.IsNullOrEmpty(cIViewModel.ChannelId))
                {
                    _ChannelCfg.TcChaneelID = cIViewModel.ChannelId;
                }
                //通道名称，必填项
                if (!string.IsNullOrEmpty(cIViewModel.ChannelName))
                {
                    _ChannelCfg.Name = cIViewModel.ChannelName;
                }
                else
                {
                    CodeStacksWindow.MessageBox.Invoke(true, false, 2, "通道名称必填");
                    return false;
                }
                //备注
                if (!string.IsNullOrEmpty(cIViewModel.Remark))
                {
                    _ChannelCfg.TcDescription = cIViewModel.Remark;
                }
                //抓拍服务器IP，必须项
                if (!string.IsNullOrEmpty(cIViewModel.CaptureAddr))
                {
                    string message = _validationRule.ipValidationRule(cIViewModel.CaptureAddr);
                    if (message != "")
                    {
                        CodeStacksWindow.MessageBox.Invoke(true, false, 2, message);
                        return false;
                    }
                    _ChannelCfg.Addr = cIViewModel.CaptureAddr;
                }
                else
                {
                    CodeStacksWindow.MessageBox.Invoke(true, false, 2, "抓拍服务器IP必填！");
                    return false;
                }
                //抓拍服务器端口必输项
                if (!string.IsNullOrEmpty(cIViewModel.CapturePort))
                {
                    string message = _validationRule.intValidationRule(cIViewModel.CapturePort);
                    if (message != "")
                    {
                        CodeStacksWindow.MessageBox.Invoke(true, false, 2, message);
                        return false;
                    }
                    _ChannelCfg.Port = int.Parse(cIViewModel.CapturePort);
                }
                else
                {
                    CodeStacksWindow.MessageBox.Invoke(true, false, 2, "抓拍服务器端口必填！");
                    return false;
                }
                _ChannelCfg.Latitude = cIViewModel.Latitude;
                _ChannelCfg.Longitude = cIViewModel.Longitude;
                _ChannelCfg.Channel_address = cIViewModel.Channel_address;
                //视频源类型
                _ChannelCfg.CaptureCfg = new CaptureCfg();
                _ChannelCfg.CaptureCfg.NCaptureType = cIViewModel.SelectedType;
                //视频源地址
                if (!string.IsNullOrEmpty(cIViewModel.VideoAddr))
                {
                    string message = _validationRule.ipValidationRule(this.txttcAddr.Text.Trim());
                    if (message != "")
                    {
                        CodeStacksWindow.MessageBox.Invoke(true, false, 2, message);
                        return false;
                    }
                    _ChannelCfg.CaptureCfg.TcAddr = cIViewModel.VideoAddr;
                }
                else
                {
                    if (cIViewModel.SelectedType != 2)
                    {
                        CodeStacksWindow.MessageBox.Invoke(true, false, 2, "视频源地址必输项");
                        return false;
                    }
                }

                //登录相机用户名
                if (!string.IsNullOrEmpty(cIViewModel.UID))
                {
                    _ChannelCfg.CaptureCfg.TcUID = cIViewModel.UID;
                }
                else
                {
                    if (cIViewModel.SelectedType != 2)
                    {
                        CodeStacksWindow.MessageBox.Invoke(true, false, 2, "登录相机用户名必输项");
                        return false;
                    }
                }
                //登录相机端密码
                if (!string.IsNullOrEmpty(cIViewModel.PSW))
                {
                    _ChannelCfg.CaptureCfg.TcPSW = cIViewModel.PSW;
                }
                else
                {
                    if (cIViewModel.SelectedType != 2)
                    {
                        CodeStacksWindow.MessageBox.Invoke(true, false, 2, "登录相机端密码必输项");
                        return false;
                    }
                }
                //视频源端口
                if (!string.IsNullOrEmpty(cIViewModel.VideoPort))
                {
                    string message = _validationRule.intValidationRule(this.txtnPort.Text.Trim());
                    if (message != "")
                    {
                        CodeStacksWindow.MessageBox.Invoke(true, false, 2, message);
                        return false;
                    }
                    _ChannelCfg.CaptureCfg.NPort = int.Parse(cIViewModel.VideoPort);
                }
                else
                {
                    CodeStacksWindow.MessageBox.Invoke(true, false, 2, "视频源端口必输项");
                    return false;
                }

                //人脸参数判断
                _ChannelCfg.CatchFaceCfg = new CatchFaceCfg();
                _ChannelCfg.CatchFaceCfg.NMinFace = Convert.ToInt32(cIViewModel.MinFace);
                _ChannelCfg.CatchFaceCfg.NMinQuality = Convert.ToInt32(cIViewModel.MinQuality);
                _ChannelCfg.CatchFaceCfg.NMinCapDistance = Convert.ToInt32(cIViewModel.MinCapDistance);
                _ChannelCfg.CatchFaceCfg.NMaxFaceSaveDistance = Convert.ToInt32(cIViewModel.MaxFaceSaveDistance);
                _ChannelCfg.CatchFaceCfg.NYaw = Convert.ToInt32(cIViewModel.Yaw);
                _ChannelCfg.CatchFaceCfg.NPitch = Convert.ToInt32(cIViewModel.Pitch);
                _ChannelCfg.CatchFaceCfg.NYoll = Convert.ToInt32(cIViewModel.Yoll);
            }
            catch (Exception ex)
            {
                Logger<OperaExcel>.Log.Error("ChannelInfo:btnConfirmAdd_Click", ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btmPhotoSource_Click(object sender, RoutedEventArgs e)
        {
            if (ImageSetGrid.Visibility == Visibility.Visible)
            {
                ImageSetGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                ImageSetGrid.Visibility = Visibility.Visible;
            }
        }

        private void btnSetFaceCaptureParameters_Click(object sender, RoutedEventArgs e)
        {
            if (CapSetGrid.Visibility == Visibility.Visible)
            {
                CapSetGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                CapSetGrid.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 初始化修改通道信息
        /// </summary>
        /// <param name="_ChannelCfg"></param>
        public void SetChannelInfo(MyChannelCfg _ChannelCfg)
        {
            try
            {
                btnChannelNumGeneration.IsEnabled = false;
                cIViewModel.Title = "修改通道";
                cIViewModel.ChannelId = _ChannelCfg.TcChaneelID;
                cIViewModel.ChannelName = _ChannelCfg.Name;
                cIViewModel.SelectedChannelType = _ChannelCfg.ChannelType;
                cIViewModel.Remark = _ChannelCfg.TcDescription;

                cIViewModel.CaptureAddr = _ChannelCfg.Addr;
                cIViewModel.CapturePort = "" + _ChannelCfg.Port;
                cIViewModel.SelectedType = _ChannelCfg.CaptureCfg.NCaptureType;
                cIViewModel.VideoAddr = _ChannelCfg.CaptureCfg.TcAddr;
                cIViewModel.UID = _ChannelCfg.CaptureCfg.TcUID;
                cIViewModel.PSW = _ChannelCfg.CaptureCfg.TcPSW;
                cIViewModel.VideoPort = "" + _ChannelCfg.CaptureCfg.NPort;
                cIViewModel.MinFace = "" + _ChannelCfg.CatchFaceCfg.NMinFace;
                cIViewModel.MinQuality = "" + _ChannelCfg.CatchFaceCfg.NMinQuality;
                cIViewModel.MinCapDistance = "" + _ChannelCfg.CatchFaceCfg.NMinCapDistance;
                cIViewModel.MaxFaceSaveDistance = "" + _ChannelCfg.CatchFaceCfg.NMaxFaceSaveDistance;
                cIViewModel.Yaw = "" + _ChannelCfg.CatchFaceCfg.NYaw;
                cIViewModel.Pitch = "" + _ChannelCfg.CatchFaceCfg.NPitch;
                cIViewModel.Yoll = "" + _ChannelCfg.CatchFaceCfg.NYoll;
                cIViewModel.Channel_address = _ChannelCfg.Channel_address;
                cIViewModel.Latitude = _ChannelCfg.Latitude;
                cIViewModel.Longitude = _ChannelCfg.Longitude;

                double lat = GlobalCache.Latitude;
                double lng = GlobalCache.Longitude;
                bool isLat = Double.TryParse(_ChannelCfg.Latitude, out lat);
                bool isLng = Double.TryParse(_ChannelCfg.Longitude, out lng);

                cIViewModel.Latitude = lat.ToString();
                cIViewModel.Longitude = lng.ToString();

                MainMap.Points = new System.Collections.ObjectModel.ObservableCollection<PointLatLng>();
                if (isLat && isLng)
                {
                    MainMap.Points.Add(new PointLatLng(lat, lng, "Red", null, new GeoTitle()));
                }
                else
                {
                    MainMap.Points.Add(new PointLatLng(GlobalCache.Latitude, GlobalCache.Longitude, "Red", null, new GeoTitle()));
                }
            }
            catch (Exception ex)
            {
                Logger<ChannelInfo>.Log.Error("SetChannelInfo", ex);
            }
        }

        private void btnResetToDefault_Click(object sender, RoutedEventArgs e)
        {
            cIViewModel.ResetFaceCap();
        }

        /// <summary>
        /// 切换视频源类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void combCaptureType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cIViewModel.SelectedType == 2)
            {
                txttcAddr.IsReadOnly = true;
                txttcUID.IsReadOnly = true;
                txttcPSW.IsReadOnly = true;
            }
            else
            {
                txttcAddr.IsReadOnly = false;
                txttcUID.IsReadOnly = false;
                txttcPSW.IsReadOnly = false;
            }
        }

        private void MainMap_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            txtlat.Text = MainMap.Latitude.ToString();
            txtLng.Text = MainMap.Longtitude.ToString();
        }

        private void btnmap_Click(object sender, RoutedEventArgs e)
        {
            if (GridMap.Visibility == Visibility.Visible)
            {
                GridMap.Visibility = Visibility.Collapsed;
            }
            else
            {
                GridMap.Visibility = Visibility.Visible;
            }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
