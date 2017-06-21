using SENSING.ClassPool;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using DATA.MODELS.GlobalModels;
using SENSING.THRIFT.Services;
using System.Windows.Input;
using Prism.Commands;
using System.Windows.Media.Imaging;
using xiaowen.codestacks.wpf.Utilities;

namespace FaceSysByMvvm.ViewModels
{
    public partial class ChannelManageViewModel : BindableBase
    {
        /// <summary>
        /// 
        /// </summary>
        public ChannelManageViewModel()
        {
            ChannelList = new List<ChannelListItemViewModel>();

            var channelList = GlobalCache.ChannelList = ThriftServiceBasic.SelectChannelList();
            if (GlobalCache.AppType == 0)
                foreach (MyChannelCfg mcc in channelList)
                {
                    ChannelListItemViewModel channel = new ChannelListItemViewModel();
                    channel.MyChannelCfg = mcc;
                    channel.IsOpened = false;
                    ChannelList.Add(channel);
                }

            CapImageCount = 0;
            ComImageCount = 0;

            //初始化阈值
            Threshold = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                Threshold.Add(i.ToString());
            }

            CmdSaveToLocalDir = new DelegateCommand<object>(CmdSaveToLocalDirFunc);
        }

        /// <summary>
        /// 保存图片至本地
        /// </summary>
        /// <param name="obj"></param>
        private void CmdSaveToLocalDirFunc(object obj)
        {
            MyCapFaceLogWithImg img = obj as MyCapFaceLogWithImg;
            DataStorage.ImageSaveAs((BitmapImage)img.img);
        }

        /// <summary>
        /// 
        /// </summary>
        public void RefreshChannelList()
        {
            List<ChannelListItemViewModel> ChannelListTemp = new List<ChannelListItemViewModel>();
            try
            {
                GlobalCache.ChannelList = ThriftServiceBasic.SelectChannelList();

                if (GlobalCache.AppType == 0)
                {
                    foreach (MyChannelCfg mcc in GlobalCache.ChannelList)
                    {
                        ChannelListItemViewModel channel = new ChannelListItemViewModel();
                        channel.MyChannelCfg = mcc;
                        channel.IsOpened = false;
                        ChannelListTemp.Add(channel);
                    }
                    //开启已经启动的摄像头
                    foreach (ChannelListItemViewModel cLI in ChannelList)
                    {
                        if (cLI.IsOpened == true)
                        {
                            ChannelListTemp.Where(p => p.MyChannelCfg.TcChaneelID == cLI.MyChannelCfg.TcChaneelID).ToList()[0].IsOpened = true;
                        }
                    }
                }
                else
                {
                    foreach (var item in GlobalCache.AreaInfoCollection)
                    {
                        /**
                         * 配置文件中可以填入简写通道，
                         * 无论筛选端或接收端，均填充数据库中的资料，而非配置文件中的名称
                         * **/
                        var myChannelContains = GlobalCache.ChannelList.Cast<MyChannelCfg>().Where(x => x.Name == item.RegionName || x.Name.StartsWith(item.RegionName));
                        foreach (var mc in myChannelContains)
                        {
                            ChannelListItemViewModel channel = new ChannelListItemViewModel();
                            channel.MyChannelCfg = mc;
                            channel.IsOpened = false;
                            ChannelListTemp.Add(channel);
                        }
                    }
                }
            }
            catch (System.Exception)
            {
            }
            finally
            {
                ChannelList = ChannelListTemp;
                GlobalCache.ChannelList = new List<MyChannelCfg>();
                List<MyChannelCfg> cfg = new List<MyChannelCfg>();
                foreach (var item in ChannelList)
                {
                    cfg.Add(item.MyChannelCfg);
                }
                GlobalCache.ChannelList = cfg;
            }
        }

        public ICommand CmdSaveToLocalDir { get; set; }

        object _snapImage;
        public object SnapImage
        {
            get { return _snapImage; }
            set { SetProperty(ref _snapImage, value); }
        }

    }

}
