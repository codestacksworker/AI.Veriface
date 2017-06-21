using SENSING.ClassPool;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Windows;
using DATA.MODELS.GlobalModels;

namespace FaceSysByMvvm.ViewModels.TemplateManager
{
    public partial class TemplateManagerViewModel : BindableBase
    {
        ThriftServiceNameSpace.ThriftService thirft = new ThriftServiceNameSpace.ThriftService();
        #region 属性的声明 

        public class TemplateManagerValue
        {
            public string NameValue { get; set; }
            public int TypeValue { get; set; }
            public int SexValue { get; set; }
            public int LittleAgeValue { get; set; }
            public int OldAgeValue { get; set; }
            public long StartDayValue { get; set; }
            public long EndDayValue { get; set; }
            public int PageRowValue { get; set; }
            public int StartRowValue { get; set; }
            public int MaxCount { get; set; }
        }

        //模版姓名     
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                SetProperty(ref name, value);
            }
        }

        //模版类型      
        private int selectedType;
        public int SelectedType
        {
            get { return selectedType; }
            set
            {
                SetProperty(ref selectedType, value);
            }
        }
        private List<string> type;
        public List<string> Type
        {
            get { return type; }
            set
            {
                SetProperty(ref type, value);
            }
        }

        //模版性别
        private int selectedSex;
        public int SelectedSex
        {
            get { return selectedSex; }
            set
            {
                SetProperty(ref selectedSex, value);
            }
        }
        private List<string> sex;
        public List<string> Sex
        {
            get { return sex; }
            set
            {
                SetProperty(ref sex, value);
            }
        }

        //模版年龄
        //年龄的下限
        private int littleAge;
        public int LittleAge
        {
            get { return littleAge; }
            set
            {
                SetProperty(ref littleAge, value);
            }
        }
        //年龄的上限
        private int oldAge;
        public int OldAge
        {
            get { return oldAge; }
            set
            {
                SetProperty(ref oldAge, value);
            }
        }

        //开始日期
        private string startDay;
        public string StartDay
        {
            get { return startDay; }
            set
            {
                SetProperty(ref startDay, value);
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

        //相似度correlate
        private int selectedCorrelate;
        public int SelectedCorrelate
        {
            get { return selectedCorrelate; }
            set
            {
                SetProperty(ref selectedCorrelate, value);
            }
        }
        private List<string> correlate;
        public List<string> Correlate
        {
            get { return correlate; }
            set
            {
                SetProperty(ref correlate, value);
            }
        }
        //返回数 returnCount
        private int selectedReturnCount;
        public int SelectedReturnCount
        {
            get { return selectedReturnCount; }
            set
            {
                SetProperty(ref selectedReturnCount, value);
            }
        }
        private List<string> returnCount;
        public List<string> ReturnCount
        {
            get { return returnCount; }
            set
            {
                SetProperty(ref returnCount, value);
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

        //Scan动画显示
        private Visibility scanVisiblity;
        public Visibility ScanVisiblity
        {
            get { return scanVisiblity; }
            set
            {
                SetProperty(ref scanVisiblity, value);
            }
        }

        //条件查询显示
        private Visibility tiaojianQueryVisiblity;
        public Visibility TiaojianQueryVisiblity
        {
            get { return tiaojianQueryVisiblity; }
            set
            {
                SetProperty(ref tiaojianQueryVisiblity, value);
            }
        }
        //图片查询显示
        private Visibility picQueryVisiblity;
        public Visibility PicQueryVisiblity
        {
            get { return picQueryVisiblity; }
            set
            {
                SetProperty(ref picQueryVisiblity, value);
            }
        }

        private List<string> templateScroe;
        /// <summary>
        /// 图片查询相似度
        /// </summary>
        public List<string> TemplateScroe
        {
            get { return templateScroe; }
            set
            {
                SetProperty(ref templateScroe, value);
            }
        }

        private List<string> maxPictureCount;
        /// <summary>
        /// 图片查询返回照片数
        /// </summary>
        public List<string> MaxPictureCount
        {
            get { return maxPictureCount; }
            set
            {
                SetProperty(ref maxPictureCount, value);
            }
        }
        //表格数据 List<MyFaceObj> ListMyFaceObj
        private List<MyFaceObj> listMyFaceObj;
        public List<MyFaceObj> ListMyFaceObj
        {
            get { return listMyFaceObj; }
            set
            {
                SetProperty(ref listMyFaceObj, value);
            }
        }

        MyFaceObj _myFaceObj;
        public MyFaceObj MyFaceObj
        {
            get { return _myFaceObj; }
            set { SetProperty(ref _myFaceObj, value); }
        }

        public TemplateManagerValue templateManagerValue;
        #endregion

        #region Properties

        Visibility _isVisibleMarkUp;
        public Visibility IsVisibleMarkUp
        {
            get { return _isVisibleMarkUp; }
            set { SetProperty(ref _isVisibleMarkUp, value); }
        }

        #endregion

        #region 初始化
        public TemplateManagerViewModel()
        {
            this.initCmd();

            //初始化条件的初值
            templateManagerValue = new TemplateManagerValue();
            //初始化表格数据
            ListMyFaceObj = new List<MyFaceObj>();

            //初始化年龄的下限 初始化年龄的上限 初始化开始日期 初始化结束日期
            Name = StartDay = EndDay = string.Empty;
            OldAge = 100;
            //初始化模版类型
            GlobalCache.FaceTypeList = Type = thirft.QueryDefFaceObjType();
            SelectedType = 0;

            //初始化模版性别
            SelectedSex = 0;
            Sex = new List<string> { "全部", "男", "女" };

            //初始化开始时辰和结束时辰和选择时间
            StartHour = new List<string>();
            EndHour = new List<string>();
            for (int i = 0; i <= 23; i++)
            {
                StartHour.Add(i + ":00");
                EndHour.Add(i + 1 + ":00");
            }
            SelectedEndHour = 23;
            SelectedStartHour = 0;

            //初始化loading图片 Hidden Visible
            LoadingVisiblity = Visibility.Collapsed;
            ScanVisiblity = Visibility.Collapsed;
            TiaojianQueryVisiblity = Visibility.Visible;
            PicQueryVisiblity = Visibility.Collapsed;
            
            if (GlobalCache.AppType == 0)
                IsVisibleMarkUp = Visibility.Visible;
            else
                IsVisibleMarkUp = Visibility.Collapsed;
            //初始化每页行数
            SelectedPageRow = 2;
            PageRow = new List<string>() { "5", "10", "15", "30", "60" };

            //初始化当前页 初始化最大页
            CurrPage = MaxPage = 0;

            //初始化图片相似度
            TemplateScroe = new List<string>();
            int score = 5;
            do
            {
                score += 5;
                TemplateScroe.Add(score.ToString());
            } while (score < 100);

            //初始化图片查询最大返回数量
            MaxPictureCount = new List<string>();
            MaxPictureCount.Add("5");
            MaxPictureCount.Add("10");
            MaxPictureCount.Add("15");
            MaxPictureCount.Add("35");
            MaxPictureCount.Add("75");
            MaxPictureCount.Add("105");
            MaxPictureCount.Add("135");
            MaxPictureCount.Add("165");
            MaxPictureCount.Add("200");
            MaxPictureCount.Add("250");
            MaxPictureCount.Add("300");
            MaxPictureCount.Add("350");
            MaxPictureCount.Add("400");
            MaxPictureCount.Add("450");
            MaxPictureCount.Add("500");
        }
        #endregion




    }
}
