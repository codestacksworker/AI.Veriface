using FaceSysByMvvm.Model;
using Prism.Mvvm;
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace FaceSysByMvvm.ViewModels.CompOfRecords
{
    /// <summary>
    /// PropertiesViewModel
    /// </summary>
    public partial class CompOfRecordsViewModel : BindableBase
    {
        CompOfRecordsViewModel _cmpDataContext;
        public CompOfRecordsViewModel CmpDataContext
        {
            get { return _cmpDataContext; }
            set { SetProperty(ref _cmpDataContext, value); }
        }

        public class CompOfRecordsValue
        {
            public string ChannelValue { get; set; }
            public string NameValue { get; set; }
            public int TypeValue { get; set; }
            public int SexValue { get; set; }
            public int MinAgeValue { get; set; }
            public int MaxAgeValue { get; set; }
            public long StartDayValue { get; set; }
            public long EndDayValue { get; set; }
            public int PageSize { get; set; }
            public int PageStartRow { get; set; }
            public int MaxDataCount { get; set; }
        }

        MyCmpFaceLogWidthImgModel _compareResultItem;
        public MyCmpFaceLogWidthImgModel CompareResultItem
        {
            get { return _compareResultItem; }
            set { SetProperty(ref _compareResultItem, value); }
        }

        //比对结果
        private IList _compareResultItems;
        public IList CompareResultItems
        {
            get { return _compareResultItems; }
            set
            {
                SetProperty(ref _compareResultItems, value);
            }
        }

        //通道      
        private int selectedChannel;
        public int SelectedChannel
        {
            get { return selectedChannel; }
            set
            {
                SetProperty(ref selectedChannel, value);
            }
        }
        private List<string> channel;
        public List<string> Channel
        {
            get { return channel; }
            set
            {
                SetProperty(ref channel, value);
            }
        }
        public List<string> ChannelId;
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
        private string minAge;
        public string MinAge
        {
            get { return minAge; }
            set
            {
                SetProperty(ref minAge, value);
            }
        }
        //年龄的上限
        private string maxAge;
        public string MaxAge
        {
            get { return maxAge; }
            set
            {
                SetProperty(ref maxAge, value);
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
                SelectedStartMinutes = 0;
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

        int _selectedStartMinutes;
        public int SelectedStartMinutes
        {
            get { return _selectedStartMinutes; }
            set
            {
                SetProperty(ref _selectedStartMinutes, value);
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
                SelectedEndMinutes = 0;
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

        int _selectedEndMinutes;
        public int SelectedEndMinutes
        {
            get { return _selectedEndMinutes; }
            set
            {
                SetProperty(ref _selectedEndMinutes, value);
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
        private int maxDataCount;
        public int MaxDataCount
        {
            get { return maxDataCount; }
            set
            {
                SetProperty(ref maxDataCount, value);
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

        private List<int> pageRow;
        public List<int> PageRow
        {
            get { return pageRow; }
            set
            {
                SetProperty(ref pageRow, value);
            }
        }

        public int InputPageIndex
        {
            get { return _inputPageIndex; }
            set { SetProperty(ref _inputPageIndex, value); }
        }

        int _inputPageIndex = 1;

        CompOfRecordsValue _compOfRecordsValueObj;
        public CompOfRecordsValue CompOfRecordsValueObj
        {
            get { return _compOfRecordsValueObj; }
            set { SetProperty(ref _compOfRecordsValueObj, value); }
        }

        bool _isSearchPublished;
        public bool IsSearchPublished
        {
            get { return _isSearchPublished; }
            set { SetProperty(ref _isSearchPublished, value); }
        }

        Visibility _isFilterClient;
        public Visibility IsFilterClient
        {
            get { return _isFilterClient; }
            set { SetProperty(ref _isFilterClient, value); }
        }

    }
}
