using Prism.Mvvm;
using SENSING.THRIFT.Services;
using System.Collections.Generic;

//namespace SENSING.APPLICATION.ViewModels.ChannelManage
namespace FaceSysByMvvm.ViewModels
{
    public partial class ChannelManageViewModel:BindableBase
    {
        public LastRecordInfo initPastReuslt()
        {
            return ThriftServiceBasic.initPastResult();
        }

        private ChannelListItemViewModel _currentPointChannelListItem;

        public ChannelListItemViewModel CurrentPointChannelListItem
        {
            get { return _currentPointChannelListItem; }
            set
            {
                SetProperty(ref _currentPointChannelListItem, value);
            }
        }


        private int capImageCount;
        /// <summary>
        /// 抓拍照片数量
        /// </summary>
        public int CapImageCount
        {
            get { return capImageCount; }
            set
            {
                SetProperty(ref capImageCount, value);
            }
        }

        private int comImageCount;
        /// <summary>
        /// 比对照片数量
        /// </summary>
        public int ComImageCount
        {
            get { return comImageCount; }
            set
            {
                SetProperty(ref comImageCount, value);
            }
        }

        private List<ChannelListItemViewModel> channelList;

        public List<ChannelListItemViewModel> ChannelList
        {
            get { return channelList; }
            set
            {
                SetProperty(ref channelList, value);
            }
        }

        private int selectedThreshold;
        /// <summary>
        /// 阈值选中项
        /// </summary>
        public int SelectedThreshold
        {
            get { return selectedThreshold; }
            set
            {
                SetProperty(ref selectedThreshold, value);
            }
        }

        private List<string> threshold;
        /// <summary>
        /// 阈值
        /// </summary>
        public List<string> Threshold
        {
            get { return threshold; }
            set
            {
                SetProperty(ref threshold, value);
            }
        }
    }
}
