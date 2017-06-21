using SENSING.ClassPool;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Windows;
using DATA.MODELS.GlobalModels;
using System.Collections;
using System.Windows.Media.Imaging;

namespace FaceSysByMvvm.ViewModels.CaptureRecordQuery
{
    public partial class CaptureRecordQueryViewModel : BindableBase
    {
        ThriftServiceNameSpace.ThriftService thirft = new ThriftServiceNameSpace.ThriftService();
        #region 属性和命令的声明
        CaptureRecordQueryViewModel capDataContext;
        public CaptureRecordQueryViewModel CapDataContext
        {
            get { return capDataContext; }
            set { SetProperty(ref capDataContext, value); }
        }
        //抓拍结果
        private MyCapFaceLogWithImg captureResultItem;
        public MyCapFaceLogWithImg CaptureResultItem
        {
            get { return captureResultItem; }
            set
            {
                SetProperty(ref captureResultItem, value);
            }
        }

        private IList captureResultItems;
        public IList CaptureResultItems
        {
            get { return captureResultItems; }
            set
            {
                SetProperty(ref captureResultItems, value);
            }
        }
        public class CaptureRecordQueryValue
        {
            /// <summary>
            /// 选择的通道
            /// </summary>
            public string ChannelValue { get; set; }
            /// <summary>
            /// 开始日期
            /// </summary>
            public long StartDayValue { get; set; }
            /// <summary>
            /// 结束日期
            /// </summary>
            public long EndDayValue { get; set; }
            /// <summary>this.SetProperty(ref id, value);
            /// 每页现实的行数
            /// </summary>
            public int PageRowValue { get; set; }
            /// <summary>
            /// 从第几行开始获取
            /// </summary>
            public int StartRowValue { get; set; }

            public int MaxCount { get; set; }
        }

        //通道      
        private int selectedChannel;
        public int SelectedChannel
        {
            get { return selectedChannel; }
            set
            {
                this.SetProperty(ref selectedChannel, value);
            }
        }
        private List<string> channel;
        public List<string> Channel
        {
            get { return channel; }
            set
            {
                this.SetProperty(ref channel, value);
            }
        }

        public List<string> ChannelId;
        //开始日期
        private string startDay;
        public string StartDay
        {
            get { return startDay; }
            set
            {
                this.SetProperty(ref startDay, value);
            }
        }

        //开始时辰
        private int selectedStartHour;
        public int SelectedStartHour
        {
            get { return selectedStartHour; }
            set
            {
                SetProperty(ref selectedStartHour, value);
            }
        }
        private List<string> startHour;
        public List<string> StartHour
        {
            get { return startHour; }
            set
            {
                SetProperty(ref startHour, value);
            }
        }


        private List<string> startMinutes;
        /// <summary>
        /// 开始分钟
        /// </summary>
        public List<string> StartMinutes
        {
            get { return startMinutes; }
            set
            {
                SetProperty(ref startMinutes, value);
            }
        }
        /// <summary>
        /// 选择开始分钟
        /// </summary>
        private int selectedStartMinutes;
        public int SelectedStartMinutes
        {
            get { return selectedStartMinutes; }
            set
            {
                SetProperty(ref selectedStartMinutes, value);
            }
        }

        //结束日期
        private string endDay;
        public string EndDay
        {
            get { return endDay; }
            set
            {
                SetProperty(ref endDay, value);
            }
        }

        //结束时辰
        private int selectedEndHour;
        public int SelectedEndHour
        {
            get { return selectedEndHour; }
            set
            {

                SetProperty(ref selectedEndHour, value);
            }
        }
        private List<string> endHour;
        public List<string> EndHour
        {
            get { return endHour; }
            set
            {
                SetProperty(ref endHour, value);
            }
        }

        private List<string> endMinutes;
        /// <summary>
        /// 结束分钟
        /// </summary>
        public List<string> EndMinutes
        {
            get { return endMinutes; }
            set
            {
                SetProperty(ref endMinutes, value);
            }
        }
        /// <summary>
        /// 选择结束分钟
        /// </summary>
        private int selectedEndMinutes;
        public int SelectedEndMinutes
        {
            get { return selectedEndMinutes; }
            set
            {
                SetProperty(ref selectedEndMinutes, value);
            }
        }
        //loading动画显示
        private Visibility loadingVisiblity;
        public Visibility LoadingVisiblity
        {
            get { return loadingVisiblity; }
            set
            {
                SetProperty(ref loadingVisiblity, value);
            }
        }

        //查询结果总数量
        private int maxCount;
        public int MaxCount
        {
            get { return maxCount; }
            set
            {
                SetProperty(ref maxCount, value);
            }
        }

        //当前页码
        private int currPage;
        public int CurrPage
        {
            get { return currPage; }
            set
            {
                SetProperty(ref currPage, value);
            }
        }

        //总的页码数
        private int maxPage;
        public int MaxPage
        {
            get { return maxPage; }
            set
            {
                SetProperty(ref maxPage, value);
            }
        }

        //每页显示的行数
        private int selectedPageRow;
        public int SelectedPageRow
        {
            get { return selectedPageRow; }
            set
            {
                SetProperty(ref selectedPageRow, value);
            }
        }
        private List<string> pageRow;
        public List<string> PageRow
        {
            get { return pageRow; }
            set
            {
                SetProperty(ref pageRow, value);
            }
        }
        /// <summary>
        /// 跳转到第几页
        /// </summary>
        private int inputPageIndex = 1;
        public int InputPageIndex
        {
            get { return inputPageIndex; }
            set { SetProperty(ref inputPageIndex, value); }
        }


        private string selectCurrDay;
        public string SelectCurrDay
        {
            get
            {
                return selectCurrDay;
            }

            set
            {
                selectCurrDay = value;
            }
        }


        CaptureRecordQueryValue captureRecordsValueObj;
        public CaptureRecordQueryValue CaptureRecordsValueObj
        {
            get { return captureRecordsValueObj; }
            set { SetProperty(ref captureRecordsValueObj, value); }
        }

        //抓拍记录结果
        private List<MyCapFaceLogWithImg> listMyCapFaceLogWithImg;
        public List<MyCapFaceLogWithImg> ListMyCapFaceLogWithImg
        {
            get { return listMyCapFaceLogWithImg; }
            set
            {
                SetProperty(ref listMyCapFaceLogWithImg, value);
            }
        }

        BitmapImage _snapImage;
        public BitmapImage SnapIamge
        {
            get { return _snapImage; }
            set { SetProperty(ref _snapImage, value); }
        }


        //查询时条件值
        public CaptureRecordQueryValue captureRecordQueryValue;
        #endregion

        #region 初始化和命令的执行和执行条件
        public CaptureRecordQueryViewModel()
        {
            InitCmd();
            //初始化条件的初值
            //captureRecordQueryValue = new CaptureRecordQueryValue();

            this.CaptureRecordsValueObj = new CaptureRecordQueryValue();

            //初始化抓拍数据
            CaptureResultItems = new List<MyCapFaceLogWithImg>();

            //初始化通道
            Channel = new List<string>();
            ChannelId = new List<string>();
            RefreshChannelList();
            SelectedChannel = 0;

            //初始化开始日期  //初始化结束日期
            StartDay = EndDay = System.DateTime.Today.ToShortDateString();

            //初始化开始时辰和结束时辰和选择时间
            StartHour = new List<string>();
            EndHour = new List<string>();
            for (int i = 0; i <= 23; i++)
            {
                StartHour.Add(i + ":00");
                EndHour.Add(i + 1 + ":00");
            }
            SelectedEndHour = 22;
            SelectedStartHour = 0;
            //初始化开始和结束分钟
            StartMinutes = new List<string>();
            EndMinutes = new List<string>();
            for (int i = 0; i <= 59; i++)
            {
                StartMinutes.Add(i.ToString());
                EndMinutes.Add(i.ToString());
            }


            //初始化loading图片 Hidden Visible
            LoadingVisiblity = Visibility.Collapsed;

            CaptureRecordsValueObj.PageRowValue = 15;
            PageRow = new List<string>() { "5", "10", "15", "30", "60" };
            //初始化每页行数
            //SelectedPageRow = 2;

            //初始化当前页 //初始化最大页 //初始化最大查询数
            CurrPage = MaxPage = MaxCount = 0;
        }

        internal void RefreshChannelList()
        {
            var ChannelTemp = new List<string>();
            var ChannelIdTemp = new List<string>();
            foreach (MyChannelCfg mcc in GlobalCache.ChannelList)
            {
                if (GlobalCache.AppType == 1)
                {
                    if (mcc.Name.Contains(GlobalCache.AppLocation))
                    {
                        ChannelTemp.Add(mcc.Name);
                        ChannelIdTemp.Add(mcc.TcChaneelID);
                    }
                }
                else
                {
                    ChannelTemp.Add(mcc.Name);
                    ChannelIdTemp.Add(mcc.TcChaneelID);
                }
            }
            ChannelTemp.Insert(0, "全部");
            Channel = ChannelTemp;
            ChannelId = ChannelIdTemp;
        }

        #endregion

    }
}
