using xiaowen.codestacks.data;
using DATA.MODELS.GlobalModels;
using SENSING.ClassPool;
using System;
using System.Windows.Controls;

namespace FaceSysByMvvm.Views.ChannelManager  //SENSING.APPLICATION.Views.ChannelManage
{
    public partial class ChannelManage : UserControl
    {
        public void PushSanpPhotoFromWifiModule()
        {
            if ("WIFI".Equals(GlobalCache.NetworkMode))
            {
                try
                {
                    _MyCapFaceLogWithImg = GlobalCache.MySnapFaceLogWithImgObj as MyCapFaceLogWithImg;
                    if (GlobalCache.SnapStream == null) return;

                    if (GlobalCache.SnapStream.Length > 0)
                    {
                        //读入MemoryStream对象
                        _MyCapFaceLogWithImg.img =
                        CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(GlobalCache.SnapStream);
                        _ListMyCapFaceLogWithImg.Insert(0, _MyCapFaceLogWithImg);
                        _ChannelManageViewModel.CapImageCount++;
                        if (_ListMyCapFaceLogWithImg.Count > 100)
                        {
                            _ListMyCapFaceLogWithImg.RemoveRange(9, 90);
                        }
                    }
                    sanpResultCollection.Items.Refresh();
                }
                catch (Exception)
                {
                }
            }
        }
    }
}
