using SENSING.ClassPool;
using Prism.Mvvm;
using System.Collections.Generic;
using FaceSysByMvvm.Model;
using System;
using System.Windows;
using DATA.MODELS.GlobalModels;

namespace FaceSysByMvvm.ViewModels.CompOfRecords
{
    public partial class CompOfRecordsViewModel : BindableBase
    {
        ThriftServiceNameSpace.ThriftService thirft = new ThriftServiceNameSpace.ThriftService();

        #region 初始化和命令的执行和执行条件

        /// <summary>
        /// 初始化
        /// </summary>
        public CompOfRecordsViewModel()
        {
            InitCmd();//初始化

            //初始化条件的初值
            this.CompOfRecordsValueObj = new CompOfRecordsValue();

            //初始化抓拍数据
            CompareResultItems = new List<MyCmpFaceLogWidthImgModel>();

            //初始化通道
            Channel = new List<string>();
            ChannelId = new List<string>();
            //RefreshChannelList();
            SelectedChannel = 0;

            //初始化模版姓名 //初始化年龄的下限 //初始化年龄的上限
            Name = MaxAge = MinAge = string.Empty;

            //初始化模版类型
            Type = thirft.QueryDefFaceObjType();
            SelectedType = 0;

            //初始化模版性别
            SelectedSex = 0;
            Sex = new List<string> { "全部", "男", "女" };

            //初始化开始日期 //初始化结束日期
            StartDay = EndDay = DateTime.Today.ToShortDateString();

            //初始化开始时辰和结束时辰和选择时间
            StartHour = new List<string>();
            EndHour = new List<string>();
            for (int i = 0; i <= 23; i++)
            {
                StartHour.Add(i + ":00");
                EndHour.Add(i + 1 + ":00");
            }
            SelectedStartHour = 0;
            SelectedEndHour = 22;

            //初始化开始和结束分钟
            StartMinutes = new List<string>();
            EndMinutes = new List<string>();
            for (int i = 0; i <= 59; i++)
            {
                StartMinutes.Add(i + "");
                EndMinutes.Add(i + "");
            }

            SelectedStartMinutes = 0;
            SelectedEndMinutes = 59;

            //初始化loading图片 Hidden Visible
            LoadingVisiblity = Visibility.Collapsed;
            if (GlobalCache.AppType == 0)
                IsFilterClient = Visibility.Visible;
            else
                IsFilterClient = Visibility.Collapsed;

            //初始化每页行数
            //SelectedPageRow = 2;
            CompOfRecordsValueObj.PageSize = 15;
            PageRow = new List<int>() { 5, 10, 15, 30, 60 };

            //初始化当前页 //初始化最大页
            CurrPage = MaxPage = 0;
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
