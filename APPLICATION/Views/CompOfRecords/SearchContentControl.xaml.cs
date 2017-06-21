using SENSING.ClassPool;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using FaceSysByMvvm.Model;
using DATA.UTILITIES.Log4Net;
using GalaSoft.MvvmLight.Threading;
using sensing = xiaowen.codestacks.data.SenSingModels;
using xiaowen.codestacks.popwindow.Views;

namespace FaceSysByMvvm.Views.ChannelManager
{
    /// <summary>
    /// Interaction logic for SearchContentControl.xaml
    /// </summary>
    public partial class SearchContentControl : UserControl
    {
        public SearchContentControl()
        {
            InitializeComponent();
            listComparisonRecord.MouseDoubleClick += listComparisonRecord_MouseDoubleClick;
        }

        BitmapImage cmpTemp = new BitmapImage();
        BitmapImage capTemp = new BitmapImage();

        bool isHeader = false;
        private void listComparisonRecord_MouseDoubleClick(object sender, RoutedEventArgs e)
        {
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                MyCmpFaceLogWidthImgModel cmpFaceLogWidthImg = listComparisonRecord.SelectedItem as MyCmpFaceLogWidthImgModel;
                if (cmpFaceLogWidthImg != null)
                    if (!isHeader)
                        try
                        {
                            sensing.Compare compare = new sensing.Compare();
                            compare.Score = cmpFaceLogWidthImg.score;
                            compare.Snap = new sensing.Snap();
                            compare.Snap.Photo = cmpFaceLogWidthImg.SnapImage;
                            compare.Template = new sensing.Template();
                            compare.Template.TypeValue = cmpFaceLogWidthImg.T1.TemplateType.Replace("类 型", "").Replace(":", "").Replace("：", "");
                            compare.Template.PersonInfo = new sensing.Person();
                            compare.Template.PersonInfo.Name = cmpFaceLogWidthImg.T1.UserName.Replace("姓 名", "").Replace("：", "");
                            compare.Template.PersonInfo.Photo = cmpFaceLogWidthImg.T1.TemplateImage;
                            compare.Snap.DateTime = cmpFaceLogWidthImg.date + " " + cmpFaceLogWidthImg.time;
                            compare.Camera = new sensing.Camera();
                            compare.Camera.Location = cmpFaceLogWidthImg.channelName;
                            compare.Snap.EnvironmentPhoto = cmpFaceLogWidthImg.TemplatePhoto;
                            compare.Captype = "场 景 留 存";

                            string strPath = System.Windows.Forms.Application.StartupPath;
                            compare.Template.TypePhotoPath = strPath + System.Configuration.ConfigurationManager.AppSettings["accept_Warn_bg_1"];

                            CodeStacksComparePopInfo pop = new CodeStacksComparePopInfo();
                            pop.SetResultValue(compare);
                            pop.ShowDialog();
                        }
                        catch (Exception ex)
                        {
                            Logger<OperaExcel>.Log.Error("listComparisonRecord_MouseDoubleClick", ex);
                        }
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listComparisonRecord_LostMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            GridViewColumnHeader header = e.OriginalSource as GridViewColumnHeader;

            if (header != null && !string.IsNullOrEmpty(header.Content.ToString()))
            {
                isHeader = true;
            }
            else
            {
                isHeader = false;
            }
        }

    }
}
