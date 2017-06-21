using DATA.MODELS.GlobalModels;
using DATA.UTILITIES.Log4Net;
using FaceSysByMvvm.Model;
using GMap.NET;
using PeopleModel;
using SENSING.ClassPool;
using SENSING.THRIFT.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TR_MODULES.Models;
using xiaowen.codestacks.data;
using xiaowen.codestacks.gmap.demo.Models;
using xiaowen.codestacks.popwindow;

namespace SENSING.Plugin.Trace.Views
{
    /// <summary>
    /// Interaction logic for MainTraceAnalysisView.xaml
    /// </summary>
    public partial class MainTraceAnalysisView : UserControl, INotifyPropertyChanged
    {
        #region Special

        public object PersonInfo { get; set; }

        CameraSnapPerson _snapPersonInfo;
        public CameraSnapPerson SnapPersonInfo
        {
            get { return _snapPersonInfo; }
            set
            {
                _snapPersonInfo = value;
                RaisePropertyChanged("SnapPersonInfo");
            }
        }

        ObservableCollection<CameraSnapPerson> _listItemSourceObj;
        public ObservableCollection<CameraSnapPerson> ListItemSourceObj
        {
            get { return _listItemSourceObj; }
            set
            {
                _listItemSourceObj = value;
                RaisePropertyChanged("ListItemSourceObj");
            }
        }



        #endregion

        #region MyRegion
        byte[] TemplatePhoto;
        ThriftServiceUtilities thirft = new ThriftServiceUtilities();
        private ObservableCollection<TrackInfoModel> _trackInfolist;

        public ObservableCollection<TrackInfoModel> TrackInfolist
        {
            get
            {
                return _trackInfolist;
            }
            set
            {
                _trackInfolist = value;
                RaisePropertyChanged("TrackInfolist");
            }
        }

        private TrackInfoModel _currentTrackInfo;
        public TrackInfoModel CurrentTrackInfo
        {
            get { return _currentTrackInfo; }
            set
            {
                _currentTrackInfo = value;
                RaisePropertyChanged("CurrentTrackInfo");
            }
        }
        private string _startDay;
        public string StartDay
        {
            get { return _startDay; }
            set
            {
                _startDay = value;
                RaisePropertyChanged("StartDay");
            }
        }
        private string _endDay;
        public string EndDay
        {
            get { return _endDay; }
            set
            {
                _endDay = value;
                RaisePropertyChanged("EndDay");
            }
        }

        private ObservableCollection<string> _startHours;
        public ObservableCollection<string> StartHours
        {
            get { return _startHours; }
            set
            {
                _startHours = value;
                RaisePropertyChanged("StartHours");
            }
        }
        private int _startHour;
        public int StartHour
        {
            get { return _startHour; }
            set
            {
                _startHour = value;
                RaisePropertyChanged("StartHour");
            }
        }
        private ObservableCollection<string> _endHours;
        public ObservableCollection<string> EndHours
        {
            get { return _endHours; }
            set
            {
                _endHours = value;
                RaisePropertyChanged("EndHours");
            }
        }
        private int _endHour;
        public int EndHour
        {
            get { return _endHour; }
            set
            {
                _endHour = value;
                RaisePropertyChanged("EndHour");
            }
        }

        private ObservableCollection<int> _startMinutes;
        public ObservableCollection<int> StartMinutes
        {
            get { return _startMinutes; }
            set
            {
                _startMinutes = value;
                RaisePropertyChanged("StartMinute");
            }
        }
        private int _startMinute;
        public int StartMinute
        {
            get { return _startMinute; }
            set
            {
                _startMinute = value;
                RaisePropertyChanged("StartMinute");
            }
        }

        private ObservableCollection<int> _endMinutes;
        public ObservableCollection<int> EndMinutes
        {
            get { return _endMinutes; }
            set
            {
                _endMinutes = value;
                RaisePropertyChanged("EndMinutes");
            }
        }
        private int _endMinute;
        public int EndMinute
        {
            get { return _endMinute; }
            set
            {
                _endMinute = value;
                RaisePropertyChanged("EndMinute");
            }
        }

        Visibility _loadingVisiblity;
        public Visibility LoadingVisiblity
        {
            get { return _loadingVisiblity; }
            set
            {
                _loadingVisiblity = value;
                RaisePropertyChanged("LoadingVisiblity");
            }
        }
        #endregion

        #region  PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        public MainTraceAnalysisView()
        {
            InitializeComponent();

            try
            {
                LoadingVisiblity = Visibility.Collapsed;
                StartDay = DateTime.Today.ToShortDateString();
                EndDay = DateTime.Today.ToShortDateString();
                StartHours = new ObservableCollection<string>();
                EndHours = new ObservableCollection<string>();
                StartMinutes = new ObservableCollection<int>();
                EndMinutes = new ObservableCollection<int>();

                for (int i = 0; i <= 60; i++)
                {
                    if (i <= 24)
                    {
                        StartHours.Add(i + ":00");
                        EndHours.Add(i + ":00");
                    }
                    StartMinutes.Add(i);
                    EndMinutes.Add(i);
                }

                EndHour = 23;
                EndMinute = 59;
                this.DataContext = this;

                MainMap.Points = new ObservableCollection<PointLatLng>();
                MainMap.Points.Add(
                                     new PointLatLng(
                                         lat: GlobalCache.Latitude,
                                         lng: GlobalCache.Longitude,
                                         cameraOrPhoto: string.Empty,
                                         photo: null, geoTitle: new GeoTitle())
                                       );

            }
            catch (Exception ex)
            {
                Logger<MainTraceAnalysisView>.Log.Error("MainTraceAnalysisView()", ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgSpecifyQueryPhoto_Click(object sender, RoutedEventArgs e)
        {
            //捕获异常
            try
            {
                //打开选择文件对话框
                System.Windows.Forms.OpenFileDialog _openFileDialog = new System.Windows.Forms.OpenFileDialog();
                _openFileDialog.Filter = "jpg|*.jpg|bmp|*.bmp";
                System.Windows.Forms.DialogResult result = _openFileDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string StrPath = _openFileDialog.FileName;
                    TemplatePhoto = System.IO.File.ReadAllBytes(StrPath);
                    //显示选择的图片
                    Image img2 = new Image();
                    img2.Source = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(TemplatePhoto);
                }
            }
            catch (Exception ex)
            {
                Logger<MainTraceAnalysisView>.Log.Error("imgSpecifyQueryPhoto_Click", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCaptureRecordQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToDateTime(StartDay) <= Convert.ToDateTime(EndDay))
                {
                    this.SearchResult(sender, e);
                }
                else
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "结束时间不能大于开始时间");
                }
            }
            catch (Exception ex)
            {
                Logger<MainTraceAnalysisView>.Log.Error("btnCaptureRecordQuery_Click", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="Points"></param>
        async void SearchResult(object sender, RoutedEventArgs e)
        {
            LoadingVisiblity = Visibility.Visible;

            await Task.Run(() =>
            {
                long start = CodeStacksDataHandler.DateTimeData.ConvertToLongDelegate1(true, StartDay, StartHour, StartMinute);
                long end = CodeStacksDataHandler.DateTimeData.ConvertToLongDelegate1(false, EndDay, EndHour, EndMinute);

                try
                {
                    List<TrackInfo> TrackInfos = new List<TrackInfo>();
                    if (SnapPersonInfo != null)
                    {
                        TrackInfos = AsyncSelectTrack(SnapPersonInfo.SnapId, SnapPersonInfo.PhotoByteArray, start, end).Result;
                    }
                    else
                    {
                        LoadingVisiblity = Visibility.Collapsed;//
                        CodeStacksDataHandler.UIThread.Invoke(() =>
                        {
                            CodeStacksWindow.MessageBox.Invoke(false, false, 2, "请移至其他模块选取分析对象");
                        });
                        return;
                    }

                    this.LoadData(TrackInfos);//async

                    CodeStacksDataHandler.UIThread.Invoke(() =>
                     {
                         MainMap.Points = new ObservableCollection<PointLatLng>();
                         foreach (TrackInfo item in TrackInfos)
                         {
                             double lat, lng = 0;
                             lat = Convert.ToDouble(!string.IsNullOrEmpty(item.Latitude) ? item.Latitude : GlobalCache.Latitude.ToString());
                             lng = Convert.ToDouble(!string.IsNullOrEmpty(item.Longitude) ? item.Longitude : GlobalCache.Longitude.ToString());

                             MainMap.Points.Add(
                        new PointLatLng(
                            lat: lat, lng: lng, cameraOrPhoto: "Photo"
                            , photo: CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(SnapPersonInfo.PhotoByteArray),
                            geoTitle: new GeoTitle()
                            {
                                IsVisible = Visibility.Visible,
                                Header = item.Name,
                                Content1 = "经度：",
                                Content1Value = item.Longitude,
                                Content1Visible = Visibility.Visible,
                                Content2 = "纬度：",
                                Content2Value = item.Latitude,
                                Content2Visible = Visibility.Visible,
                                Content3 = "地址：",
                                Content3Value = item.Latitude,
                                Content3Visible = Visibility.Visible
                            }));
                         }
                         MainMap.Route.IsRoute = true;
                         MainMap.MapRefresh.Invoke(null, null);
                     });
                }
                catch (Exception ex)
                {
                    Logger<MainTraceAnalysisView>.Log.Error("SearchResult Funcation [轨迹分析主查询方法中出现异常]", ex);
                }
                finally
                {
                    this.SleepFiveSeconds();//application sleep five seconds
                }
            });
        }


        /// <summary>
        /// 
        /// </summary>
        async void SleepFiveSeconds()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            LoadingVisiblity = Visibility.Collapsed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capid"></param>
        /// <param name="capimg"></param>
        /// <param name="btime"></param>
        /// <param name="etime"></param>
        /// <returns></returns>
        async Task<List<TrackInfo>> AsyncSelectTrack(string capid, byte[] capimg, long btime, long etime)
        {
            return await Task<List<TrackInfo>>.Run(() =>
             {
                 List<TrackInfo> result = new List<TrackInfo>();
                 try
                 {
                     result = thirft.QueryTrackPlayback(SnapPersonInfo.SnapId, SnapPersonInfo.PhotoByteArray, btime, etime);
                 }
                 catch (Exception ex)
                 {
                     throw new Exception(ex.Message, ex);
                 }
                 return result;
             }).ConfigureAwait(false);
        }


        async void LoadData(List<TrackInfo> TrackInfos)
        {
            await Task.Run(() =>
            {
                try
                {
                    CodeStacksDataHandler.UIThread.Invoke(() =>
                    {
                        ListItemSourceObj = new ObservableCollection<CameraSnapPerson>();
                        int row = 0;
                        foreach (var item in TrackInfos)
                        {
                            CameraSnapPerson csp = new CameraSnapPerson();

                            csp.RowNumber = ++row;
                            csp.CameraInfo = new Camera();
                            csp.CameraInfo.Name = item.Name;
                            csp.Datetime = item.Capimg != null ? CodeStacksDataHandler.DateTimeData.ConvertToStringDelegate(item.Capimg[0].Time) : "";
                            csp.CameraInfo.Location = item.Channel_address;
                            csp.CameraInfo.Longitude = item.Longitude;
                            csp.CameraInfo.Latitude = item.Latitude;
                            ListItemSourceObj.Add(csp);
                        }
                        ListItemSourceObj.Add(null);
                    });
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex);
                }
                finally
                {
                }
            }).ConfigureAwait(false);
        }


        public void GetPersonInfo(object obj)
        {
            Type type = obj.GetType();
            SnapPersonInfo = new CameraSnapPerson();

            try
            {
                if (obj is MyCmpFaceLogWidthImgModel)//对比记录至此
                {
                    MyCmpFaceLogWidthImgModel mfl = obj as MyCmpFaceLogWidthImgModel;
                    SnapPersonInfo.Name = mfl.T1.UserName;
                    SnapPersonInfo.Photo = mfl.SnapImage;
                    SnapPersonInfo.PhotoByteArray = mfl.SnapImageBuffer;
                    SnapPersonInfo.Datetime = mfl.date + " " + mfl.time;
                    SnapPersonInfo.CameraInfo = new Camera();
                    SnapPersonInfo.CameraInfo.Location = mfl.Address;
                    SnapPersonInfo.CameraInfo.Longitude = mfl.Longitude;
                    SnapPersonInfo.CameraInfo.Latitude = mfl.Latitude;
                    SnapPersonInfo.Score = mfl.Score;
                    SnapPersonInfo.Source = "抓拍库";
                }
                else if (obj is CameraSnapPerson)//静态分析至此
                {
                    SnapPersonInfo = obj as CameraSnapPerson;
                    SnapPersonInfo.Name = SnapPersonInfo.NameTitle + SnapPersonInfo.Name;
                    //SnapPersonInfo.Source = "抓拍库";
                }
                else if (obj is MyFaceObj)//模板库至此
                {
                    MyFaceObj mfo = obj as MyFaceObj;
                    SnapPersonInfo.Name = "姓 名：" + mfo.tcName;
                    SnapPersonInfo.Photo = mfo.img;
                    //SnapPersonInfo.Source = "模板库";
                    SnapPersonInfo.PhotoByteArray = mfo.imgStream; //ImageConvert.ToImageStream(mfo.img as BitmapImage);
                    SnapPersonInfo.Datetime = mfo.fa_ob_dTm;
                }
            }
            catch (Exception ex)
            {
                Logger<MainTraceAnalysisView>.Log.Error(ex);
            }
            finally
            {
                RaisePropertyChanged("SnapPersonInfo");
            }
        }

        public static TrackInfoModel ConvertModel(TrackInfo info, int i)
        {
            TrackInfoModel model = new TrackInfoModel();
            model.ID = i;
            model.TcChannelID = info.TcChannelID;
            model.Name = info.Name;
            model.Channel_type = info.Channel_type;
            model.Typestr = info.Typestr;
            model.Channel_address = info.Channel_address;
            model.Longitude = info.Longitude;
            model.Latitude = info.Latitude;
            model.Capimg = info.Capimg;
            {
                CapCount cap = new CapCount();
                for (int j = 0; j < info.Capimg.Count; j++)
                {
                    cap = info.Capimg[j];
                    if (j == Convert.ToInt32(cap.Feaid))
                    {
                        break;
                    }
                }
                model.CurrentCapimg = cap.Capimg;
                DateTime time = CodeStacksDataHandler.DateTimeData.ConvertToDateTimeDelegate(cap.Time);
                model.Time = time.ToString("yyyyMMdd");
                model.InstallDate = time.ToString("HH:mm:dd");
            }
            return model;
        }
        private void listViewCaptureRecord_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                TrackInfoModel model = listViewCaptureRecord.SelectedItem as TrackInfoModel;
                if (model != null)
                {
                    CurrentTrackInfo = model;
                }
            }
            catch (Exception)
            {
            }
        }

        private void ClearDatePickerTime(object sender, MouseButtonEventArgs e)
        {
            DatePicker dp = sender as DatePicker;
            dp.Text = "";
        }
    }
}
