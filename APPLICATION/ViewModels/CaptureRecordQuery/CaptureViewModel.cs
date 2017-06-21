using FaceSysByMvvm.Model;
using Prism.Commands;
using Prism.Mvvm;
using SENSING.ClassPool;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using xiaowen.codestacks.data;
using xiaowen.codestacks.wpf.Utilities;

namespace FaceSysByMvvm.ViewModels.CaptureRecordQuery
{
    /// <summary>
    /// 抓拍记录
    /// </summary>
    public partial class CaptureRecordQueryViewModel : BindableBase
    {
        /// <summary>
        /// 查询数据
        /// </summary>
        public ICommand SearchCommand { get; private set; }
        /// <summary>
        /// 首页
        /// </summary>
        public ICommand FirstPageCommand { get; private set; }
        /// <summary>
        /// 上一页
        /// </summary>
        public ICommand PrevPageCommand { get; private set; }
        /// <summary>
        /// 下一页
        /// </summary>
        public ICommand NextPageCommand { get; private set; }
        /// <summary>
        /// 尾页
        /// </summary>
        public ICommand LastPageCommand { get; private set; }
        /// <summary>
        /// 跳转到第n页
        /// </summary>
        public ICommand JumpToPageIndexCommand { get; private set; }
        /// <summary>
        /// 进入静态比对
        /// </summary>
        public ICommand GotoMCCommand { get; private set; }
        public ICommand GotoTRCommand { get; set; }

        public ICommand SnapImageSaveAs { get; private set; }

        void InitCmd()
        {
            SearchCommand = new DelegateCommand<Object>(SearchCommandFunc);
            FirstPageCommand = new DelegateCommand<object>(FirstPageCommandFunc);
            PrevPageCommand = new DelegateCommand<object>(PrevPageCommandFunc);
            NextPageCommand = new DelegateCommand<object>(NextPageCommandFunc);
            LastPageCommand = new DelegateCommand<object>(LastPageCommandFunc);
            JumpToPageIndexCommand = new DelegateCommand<object>(JumpToPageIndexCommandFunc);
            GotoMCCommand = new DelegateCommand<object>(GotoMCCommandFunc);
            GotoTRCommand = new DelegateCommand<object>(GotoTRCommandFunc);
            SnapImageSaveAs = new DelegateCommand<object>((obj) =>
            {
                DataStorage.ImageSaveAs((BitmapImage)obj);
            });
            getCaptureRecordsDelegate = GetAllInfo;
        }

        private void GotoTRCommandFunc(object obj)
        {
            MyCapFaceLogWithImg myCap = obj as MyCapFaceLogWithImg;
            List<byte[]> listImageBytes = new List<byte[]>();
            listImageBytes = thirft.QueryCapLogImageH(myCap.ID, DateTime.Parse(myCap.time).ToString("yyyyMMdd"));
            object newObj = null;
            //得到图片
            if (listImageBytes[0].Length > 0)
            {
                List<byte[]> senceImg = thirft.QuerySenceImg(myCap.ID, myCap.time.Split(' ')[0].Replace(@"/", "").Replace(@"/", ""));
                newObj = listImageBytes[0];
            }

            MyCmpFaceLogWidthImgModel model = new MyCmpFaceLogWidthImgModel();
            model.T1 = new CompOfRecordTemplate();
            model.T1.UserName = "通 道：" + myCap.ChannelName;
            model.SnapImageBuffer = (byte[])newObj;
            model.SnapImage = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(model.SnapImageBuffer);
            model.date = Convert.ToDateTime(myCap.time).ToShortDateString();
            model.time = Convert.ToDateTime(myCap.time).ToShortTimeString();

            ReflactionView.GoTo1(model, 5, "HomeView", "FuncationToggleButton_Checked");
        }
    }
}
