using DATA.MODELS.GlobalModels;
using FaceSysByMvvm.Model;
using SINGLEUSER.Models;
using System.Windows.Controls;
using System;
using DATA.UTILITIES.Log4Net;
using System.Linq;
using DATA.MODELS.SensingModels;
using xiaowen.codestacks.popwindow.Views;
using sensing = xiaowen.codestacks.data.SenSingModels;
using xiaowen.codestacks.popwindow;
using System.Collections.Generic;
using xiaowen.codestacks.data;

namespace FaceSysByMvvm.Views.ChannelManager.WarningMessageControls
{
    /// <summary>
    /// Interaction logic for WarningDataControl.xaml
    /// </summary>
    public partial class WarningDataControl : UserControl
    {
        public WarningDataControl()
        {
            InitializeComponent();
            cameraList.MouseDoubleClick += CameraList_MouseDoubleClick;
            cameraList.MouseEnter += CameraList_MouseEnter;
        }

        private void CameraList_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        /**
         * doubleclick 1->btnUp 2->doubleClick 3->btnUp
         * **/
        object selectedItem = null;
        private void CameraList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                sensing.Compare compare = new sensing.Compare();
                if (GlobalCache.Func_AlarmDoubleClick)
                {
                    if (selectedItem is MyCmpFaceLogWidthImgModel)
                    {
                        MyCmpFaceLogWidthImgModel _obj = selectedItem as MyCmpFaceLogWidthImgModel;
                        //compare = MyCmpFaceLogWidthImgModel.DataConvertToCmpare(_obj);
                        
                        compare.Row = _obj.num;
                        compare.Score = _obj.Score == 0 ? _obj.score : _obj.Score;
                        compare.Snap = new sensing.Snap();
                        compare.Snap.Guid = _obj.ID;
                        compare.Snap.Photo = _obj.SnapImage;
                        //compare.Snap.EnvironmentPhoto = thisObj.EnvironmentPhoto;
                        compare.Template = new sensing.Template();
                        compare.Template.Guid = _obj.tcUuid;
                        compare.Template.TypeKey = _obj.TypeKey;
                        compare.Template.TypeValue = _obj.type;
                        compare.Template.PersonInfo = new sensing.Person();
                        compare.Template.PersonInfo.Name = _obj.name;
                        compare.Template.PersonInfo.Photo = _obj.TemplatePhoto;
                        compare.Snap.DateTime = _obj.date + " " + _obj.time;
                        compare.Camera = new sensing.Camera();
                        compare.Camera.Guid = _obj.channel;
                        compare.Camera.Location = _obj.channelName;
                        compare.Captype = "发 现 可 疑 人 员";

                        ThriftServiceNameSpace.ThriftService thirft = new ThriftServiceNameSpace.ThriftService();
                        List<byte[]> senceImg = thirft.QuerySenceImg(_obj.ID, DateTime.Parse(compare.Snap.DateTime).ToString("yyyyMMdd"));
                        if (senceImg != null && senceImg.Count > 0 && senceImg[0].Length > 0)
                        {
                            compare.Snap.EnvironmentPhoto = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(senceImg[0]);
                        }

                

                        string strPath = System.Windows.Forms.Application.StartupPath;

                        switch (compare.Template.TypeKey)
                        {
                            case 0:
                                compare.Template.TypePhotoPath = strPath + System.Configuration.ConfigurationManager.AppSettings["accept_Warn_bg_1"];
                                break;
                            case 1:
                                compare.Template.TypePhotoPath = strPath + System.Configuration.ConfigurationManager.AppSettings["accept_Warn_bg_1"];
                                break;
                            case 2:
                                compare.Template.TypePhotoPath = strPath + System.Configuration.ConfigurationManager.AppSettings["accept_Warn_bg_2"];
                                break;
                            case 3:
                                compare.Template.TypePhotoPath = strPath + System.Configuration.ConfigurationManager.AppSettings["accept_Warn_bg_3"];
                                break;
                            case 4:
                                compare.Template.TypePhotoPath = strPath + System.Configuration.ConfigurationManager.AppSettings["accept_Warn_bg_4"];
                                break;
                            default:
                                compare.Template.TypePhotoPath = strPath + System.Configuration.ConfigurationManager.AppSettings["accept_Warn_bg_1"];
                                break;
                        }
                        CodeStacksComparePopInfo pop = new CodeStacksComparePopInfo();
                        pop.SetResultValue(compare);
                        pop.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
        }

        private void cameraList_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            selectedItem = (sender as ListBox).SelectedItem;
            ViewDataModel.WarningData.Property.CurCompareLogDatas = (sender as ListBox).SelectedItems;
        }

        private void BtnDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewDataModel.WarningData.Property.CurCompareLogDatas.Clear();
            this.Focus();
            string err = string.Empty;
            Button btn = sender as Button;
            btn.Focus();
            Function.ClearPushedWaringData.ClearPushedDataFormCodeBehind(ref err, btn);
        }

        /// <summary>
        /// publish btn click event
        /// 
        /// 推送只会推送当前该区域下的，即当前运行程序所属区域Number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSend_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ViewDataModel.WarningData.Property.CurCompareLogDatas.Clear();
            this.Focus();
            string err = string.Empty;
            Button btn = sender as Button;
            MyCmpFaceLogWidthImgModel selectedData = new MyCmpFaceLogWidthImgModel();
            try
            {
                foreach (MyCmpFaceLogWidthImgModel obj in ViewDataModel.WarningData.Property.CompareLogDatas)
                {
                    if (obj.ID == btn.CommandParameter.ToString())
                    {
                        selectedData = obj;
                        break;
                    }
                }

                var area = GlobalCache.AreaInfoCollection.Cast<ConfigRegion>().Where(x => selectedData.channelName.StartsWith(x.RegionName));

                if (area.Count() > 0)
                {
                    int res = WarningMessageCmd.SendOneResultInfo(selectedData);
                    if (res == 0)
                    {
                        CodeStacksWindow.MessageBox.Invoke(false, false, 1, "推送成功");
                        Function.ClearPushedWaringData.ClearPushedData(ref err, res, selectedData);
                    }
                }
                else
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 1, "无可推送通道，请检查配置");
                }
            }
            catch (System.Exception ex)
            {
                Logger<WarningDataControl>.Log.Error("【Error】", ex);
            }
        }

        private void cameraList_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void Button_LostMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is Button)
            {
                BtnSend_Click(null, null);
            }
            else
            {
                CameraList_MouseDoubleClick(null, null);
            }
        }
        
    }
}
