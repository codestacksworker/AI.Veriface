using DATA.MODELS.GlobalModels;
using DATA.UTILITIES.Log4Net;
using FaceSysByMvvm.ViewModels.TemplateManager;
using SENSING.ClassPool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using xiaowen.codestacks.data;
using xiaowen.codestacks.popwindow;

namespace FaceSysByMvvm.Views.ChannelManager
{
    /// <summary>
    /// TemplateManager.xaml 的交互逻辑
    /// </summary>
    public partial class TemplateManager : UserControl
    {
        TemplateManagerViewModel tMViewModel;
        ThriftServiceNameSpace.ThriftService thirft = new ThriftServiceNameSpace.ThriftService();
        Action<int> cetTemplateManagerDelegate;
        delegate void GetTemplateByImageDelegate(int threshold, int count);
        GetTemplateByImageDelegate getTemplateByImageDelegate;

        public Action<string> QueryTemplateCmpDelegate;

        byte[] TemplatePhoto;

        public TemplateManager()
        {
            InitializeComponent();
            tMViewModel = new TemplateManagerViewModel();
            this.DataContext = tMViewModel;
            cetTemplateManagerDelegate = GetAllInfo;
            getTemplateByImageDelegate = GetTemplateByImage;
            if (GlobalCache.AppType == 1)
            {
                gridTempleteControl.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 根据图片查询模版
        /// </summary>
        private void GetTemplateByImage(int threshold, int count)
        {
            tMViewModel.LoadingVisiblity = Visibility.Visible;
            tMViewModel.ScanVisiblity = Visibility.Visible;
            tMViewModel.ListMyFaceObj = thirft.QueryFaceObjByImg(TemplatePhoto, threshold, count);
            tMViewModel.CurrPage = 1;
            tMViewModel.MaxPage = 1;
            tMViewModel.MaxCount = tMViewModel.ListMyFaceObj.Count;
            tMViewModel.LoadingVisiblity = Visibility.Collapsed;
            tMViewModel.ScanVisiblity = Visibility.Collapsed;
        }

        int currPage = 0;

        /// <summary>
        /// 查询模版的具体实现
        /// </summary>
        /// <param name="page"></param>
        private void GetAllInfo(int page)
        {
            try
            {
                currPage = page;
                //page = page - 1;
                tMViewModel.LoadingVisiblity = Visibility.Visible;

                List<MyCapFaceLogWithImg> listMyCapFaceLogWithImg = new List<MyCapFaceLogWithImg>();
                tMViewModel.MaxCount = thirft.QueryFaceObjTotalCount(tMViewModel.templateManagerValue);
                tMViewModel.MaxPage = tMViewModel.MaxCount % tMViewModel.templateManagerValue.PageRowValue != 0 ? tMViewModel.MaxCount / tMViewModel.templateManagerValue.PageRowValue + 1 : tMViewModel.MaxCount / tMViewModel.templateManagerValue.PageRowValue;
                tMViewModel.templateManagerValue.MaxCount = tMViewModel.MaxCount;
                if (page > 1)
                {
                    tMViewModel.templateManagerValue.StartRowValue = tMViewModel.MaxCount - (page * tMViewModel.templateManagerValue.PageRowValue);
                    if (tMViewModel.templateManagerValue.StartRowValue < 0)
                    {
                        tMViewModel.templateManagerValue.StartRowValue = 0;
                    }
                }
                else
                {
                    tMViewModel.templateManagerValue.StartRowValue = tMViewModel.MaxCount - (tMViewModel.templateManagerValue.PageRowValue);
                    if (tMViewModel.templateManagerValue.StartRowValue < 0)
                    {
                        tMViewModel.templateManagerValue.StartRowValue = 0;
                    }
                }
                tMViewModel.CurrPage = page;
                tMViewModel.ListMyFaceObj = thirft.QueryFaceObj(tMViewModel.templateManagerValue);
                tMViewModel.LoadingVisiblity = Visibility.Collapsed;
                this.Dispatcher.BeginInvoke(
                    new Action(() =>
                    {
                        if (listViewTemplate.Items.Count > 0)
                        {
                            listViewTemplate.ScrollIntoView(listViewTemplate.Items[0]);
                        }
                    }));
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 换页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPageChange_Click(object sender, RoutedEventArgs e)
        {
            Button pageBtn = sender as Button;
            int page = 0;
            switch (pageBtn.Name)
            {
                case "btnTemplateFirstPage":
                    page = 1;
                    break;
                case "btnTemplatePastPage":
                    page = tMViewModel.CurrPage - 1;
                    break;
                case "btnTemplateNextPage":
                    page = tMViewModel.CurrPage + 1;
                    break;
                case "btnTemplateLastPage":
                    page = tMViewModel.MaxPage;
                    break;
                case "btnGoToPage":
                    try
                    {
                        page = Convert.ToInt32(txtGoToPage.Text.ToString().Trim());
                    }
                    catch (Exception)
                    {
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, "请输入正确的跳转页码!");
                        return;
                    }
                    break;
                default:
                    break;
            }
            if (page <= 0)
            {
                CodeStacksWindow.MessageBox.Invoke(false, false, 2, "已经是首页!");
                return;
            }
            if (page > tMViewModel.MaxPage)
            {
                CodeStacksWindow.MessageBox.Invoke(false, false, 2, "已经是尾页！");
                return;
            }
            cetTemplateManagerDelegate.BeginInvoke(page, null, null);
        }

        /// <summary>
        /// 模板各列宽度自动变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewTemplate_SizeChanged_1(object sender, SizeChangedEventArgs e)
        {
            try
            {
                #region
                //获得模板listview
                GridView gv = listViewTemplate.View as GridView;
                if (gv != null)
                {
                    //循环获得类，设置列的宽度
                    foreach (GridViewColumn gvc in gv.Columns)
                    {
                        gvc.Width = (listViewTemplate.ActualWidth - 4) / 7;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Logger<OperaExcel>.Log.Error("listViewTemplate_SizeChanged_1", ex);
            }
        }

        /// <summary>
        /// 查询按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTemplateQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tMViewModel.templateManagerValue.NameValue = tMViewModel.Name;
                if (combType.SelectedIndex == -1 || combType.SelectedIndex == 0)
                {
                    tMViewModel.templateManagerValue.TypeValue = -1;
                }
                else
                {
                    tMViewModel.templateManagerValue.TypeValue = combType.SelectedIndex;
                }

                tMViewModel.templateManagerValue.SexValue = tMViewModel.SelectedSex == 0 ? -1 : tMViewModel.SelectedSex;
                tMViewModel.templateManagerValue.PageRowValue = Convert.ToInt32(tMViewModel.PageRow[tMViewModel.SelectedPageRow]);
                //todo(已完成 未测试) 修改年龄和时间
                tMViewModel.templateManagerValue.LittleAgeValue = tMViewModel.LittleAge;
                tMViewModel.templateManagerValue.OldAgeValue = tMViewModel.OldAge;

                //开始时间
                long bTime = -1, eTime = -1;
                if (!string.IsNullOrEmpty(tMViewModel.StartDay))
                {
                    bTime =
                        CodeStacksDataHandler.DateTimeData.ConvertToLongBySubstractDelegate.Invoke(Convert.ToDateTime(tMViewModel.StartDay), new DateTime(1970, 1, 1));

                    if (tMViewModel.SelectedStartHour != -1)
                    {
                        bTime = bTime + int.Parse(tMViewModel.SelectedStartHour.ToString()) * 60 * 60;
                    }
                }

                if (!string.IsNullOrEmpty(tMViewModel.EndDay))
                {
                    eTime =
                        CodeStacksDataHandler.DateTimeData.ConvertToLongBySubstractDelegate.Invoke(Convert.ToDateTime(tMViewModel.EndDay), new DateTime(1970, 1, 1));
                    if (tMViewModel.SelectedEndHour != -1)
                    {
                        eTime = eTime + (int.Parse(tMViewModel.SelectedEndHour.ToString()) + 1) * 60 * 60;
                    }
                }

                tMViewModel.templateManagerValue.StartDayValue = bTime;
                tMViewModel.templateManagerValue.EndDayValue = eTime;

                cetTemplateManagerDelegate.BeginInvoke(1, null, null);
            }
            catch (Exception ex)
            {
                Logger<TemplateManager>.Log.Error(ex);
            }
        }

        /// <summary>
        /// 切换选中项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewTemplate_SelectionChanged(object sender, SelectionChangedEventArgs eventArgs)
        {
            try
            {
                MyFaceObj myFaceObj = listViewTemplate.SelectedItem as MyFaceObj;
                if (myFaceObj == null)
                {
                    GridTemplatePic.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        GridTemplatePic.Background = new ImageBrush
                        {
                            ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/抓拍照片纯背景.png"))
                        };
                        GridAfterbtnTemplatePhoto.Visibility = Visibility.Collapsed;
                    }));
                    return;
                }
                //获得模板id和所属的人脸Id
                string nTemplateFaceID = myFaceObj.fa_ob_tcUuid;
                int nTemplateId = myFaceObj.nMain_ftID;
                List<FaceObj> ListFaceObj = new List<FaceObj>();
                ListFaceObj.Add(myFaceObj.faceObj);
                byte[] imageBytes = new byte[10];

                btnTemplatePhoto.Dispatcher.BeginInvoke(new Action(() =>
                {
                    this.btnTemplatePhoto.Content = string.Empty;
                    //遍历得到对应模板ID下的照片
                    if (ListFaceObj.Count > 0)
                    {
                        var faceobj = ListFaceObj.FirstOrDefault(x => x.NMain_ftID == nTemplateId);
                        if (faceobj != null)
                        {
                            #region 
                            try
                            {
                                var mainPhoto = faceobj.Tmplate.FirstOrDefault(x => x.NIndex == nTemplateId);
                                GridTemplatePic.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    GridTemplatePic.Background = new ImageBrush
                                    {
                                        ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/抓拍照片纯背景.png"))
                                    };
                                    GridAfterbtnTemplatePhoto.Visibility = Visibility.Visible;
                                }));

                                btnTemplatePhoto.Visibility = Visibility.Visible;
                                this.btnTemplatePhoto.Background = new ImageBrush { ImageSource = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(mainPhoto.Img) };
                                //读入MemoryStream对象
                                tMViewModel.MyFaceObj.img = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(mainPhoto.Img);
                                tMViewModel.MyFaceObj.imgStream = mainPhoto.Img;
                            }
                            catch (Exception e)
                            {
                                if (e.InnerException != null)
                                {
                                    if (e.InnerException.Message.Contains("0x88982F72"))
                                    {
                                        try
                                        {
                                            FileStream fs = new FileStream(System.Windows.Forms.Application.StartupPath + "\\转存的图片.jpg", FileMode.OpenOrCreate);
                                            fs.Write(imageBytes, 0, imageBytes.Length);

                                            fs.Flush();
                                            fs.Close();
                                            fs.Dispose();

                                            System.Drawing.Image img = new System.Drawing.Bitmap(System.Windows.Forms.Application.StartupPath + "\\转存的图片.jpg");
                                            MemoryStream stream = new MemoryStream();
                                            img.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                                            BinaryReader br = new BinaryReader(stream);
                                            byte[] photo = stream.ToArray();
                                            stream.Close();
                                            stream.Dispose();
                                            System.IO.MemoryStream ms = new System.IO.MemoryStream(photo);//img是从数据库中读取出来的字节数组

                                            this.btnTemplatePhoto.Background = new ImageBrush { ImageSource = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(imageBytes) };
                                            tMViewModel.MyFaceObj.img = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(imageBytes);
                                            //File.Delete(System.Windows.Forms.Application.StartupPath + "\\转存的图片.jpg");
                                        }
                                        catch (Exception ex)
                                        {
                                            CodeStacksWindow.MessageBox.Invoke(false, false, 2, "您当前的操作过于频繁,请稍后重试");
                                            Logger<TemplateManager>.Log.Error("listViewTemplate_SelectionChanged", ex);
                                        }

                                    }
                                    else
                                    {
                                        //imageBytes;
                                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, "获得数据库图片错误！" + e.InnerException.Message);
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            GridTemplatePic.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                GridTemplatePic.Background = new ImageBrush
                                {
                                    ImageSource = null
                                };
                                GridAfterbtnTemplatePhoto.Visibility = Visibility.Collapsed;
                            }));
                        }
                    }
                }));
            }
            catch (Exception ex)
            {
                Logger<TemplateManager>.Log.Error("listViewTemplate_SelectionChanged", ex);
            }
        }

        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddTemplate_Click(object sender, RoutedEventArgs e)
        {
            TempleteInfoPop tIP = new TempleteInfoPop();
            tIP.SetTempleteInfo(null, 1, null);
            tIP.ShowDialog();
        }

        /// <summary>
        /// 修改模板
        /// </summary>
        private void UpdateTemplate()
        {
            try
            {
                if (listViewTemplate.SelectedIndex < 0)
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "请选中需要修改的模版!");
                }
                else
                {
                    MyFaceObj myFaceObj = listViewTemplate.SelectedItem as MyFaceObj;
                    TempleteInfoPop tIP = new TempleteInfoPop();
                    tIP.SetTempleteInfo(myFaceObj, 2, null);
                    tIP.ShowDialog();
                    cetTemplateManagerDelegate = GetAllInfo;
                }
            }
            catch (Exception ex)
            {
                Logger<TemplateManager>.Log.Error("UpdateTemplate", ex);
            }
        }

        /// <summary>
        /// 双击修改模版
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewTemplate_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listViewTemplate.SelectedItem is MyFaceObj)
            {
                if (GlobalCache.AppType == 0)
                    UpdateTemplate();
            }
        }

        /// <summary>
        /// 删除模版
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteTemplate_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalCache.AppType == 0)
            {
                try
                {
                    if (listViewTemplate.SelectedIndex >= 0)
                    {
                        var selectedItem = listViewTemplate.SelectedItem;
                        MyFaceObj myFaceObj = selectedItem as MyFaceObj;
                        int isSuccess = -1;
                        if (myFaceObj != null)
                        {
                            bool? result = CodeStacksWindow.MessageBox.Invoke(true, false, 2, "是否删除模板！");
                            if (result == true)//点击确定
                            {
                                isSuccess = thirft.DelFaceObj(myFaceObj.fa_ob_tcUuid);
                                if (isSuccess == 0)
                                {
                                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "删除模板 " + myFaceObj.tcName + " 成功！");

                                    cetTemplateManagerDelegate.BeginInvoke(currPage, null, null);
                                }
                                if (isSuccess == -1)
                                {
                                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "删除模板失败！");
                                }
                            }
                        }
                    }
                    else
                    {
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, "请选择要删除的模板！");
                    }
                }
                catch (Exception ex)
                {
                    Logger<TemplateManager>.Log.Error("btnDeleteTemplate_Click", ex);
                }
            }
        }

        /// <summary>
        /// 批量导入模版
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBatchImportTemplate_Click(object sender, RoutedEventArgs e)
        {
            TempleteImportPop templeteImportPop = new TempleteImportPop();
            templeteImportPop.ShowDialog();
        }

        /// <summary>
        /// 指定查询照片按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void imgSpecifyQueryPhoto_Click(object sender, RoutedEventArgs e)
        {
            //捕获异常
            try
            {
                //打开选择文件对话框
                System.Windows.Forms.OpenFileDialog _openFileDialog = new System.Windows.Forms.OpenFileDialog();
                _openFileDialog.Filter = "jpg|*.jpg|bmp|*.bmp";
                System.Windows.Forms.DialogResult result = _openFileDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    string StrPath = _openFileDialog.FileName;
                    btnPIcPath.Content = StrPath;
                    TemplatePhoto = System.IO.File.ReadAllBytes(StrPath);
                    //显示选择的图片
                    System.Windows.Controls.Image img2 = new System.Windows.Controls.Image();
                    BitmapImage myBitmapImage = new BitmapImage();
                    myBitmapImage.BeginInit();
                    myBitmapImage.StreamSource = new System.IO.MemoryStream(TemplatePhoto);
                    myBitmapImage.EndInit();
                    img2.Source = myBitmapImage;
                    myBitmapImage = null;
                    this.imgSpecifyQueryPhoto.Content = img2;
                }
            }
            catch (Exception ex)
            {
                Logger<OperaExcel>.Log.Error("btnSpecifyQueryPhoto_Click", ex);
                return;
            }
        }

        /// <summary>
        /// 清空图片查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemoveQueryPhoto_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region
                //清空查询条件
                this.imgSpecifyQueryPhoto.Content = "";
                btnPIcPath.Content = "";
                TemplatePhoto = null;
                #endregion
            }
            catch (Exception ex)
            {
                Logger<OperaExcel>.Log.Error("btnRemoveQueryPhoto_Click", ex);
            }
        }

        /// <summary>
        /// 根据照片返回相似结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryPhotoForScroe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TemplatePhoto == null || TemplatePhoto.Length <= 0)
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "请指定照片");
                    return;
                }

                //获得相似度阈值
                int nThreshold = -1;
                if (combTemplateScroe.SelectedItem == null)
                {
                    nThreshold = 10;
                }
                else
                {
                    nThreshold = int.Parse(combTemplateScroe.SelectedItem.ToString());
                }
                //获得最大返回行数
                int nMaxCount = -1;
                if (combTemplateMaxBack.SelectedItem == null)
                {
                    nMaxCount = 5;
                }
                else
                {
                    nMaxCount = int.Parse(combTemplateMaxBack.SelectedItem.ToString());
                }

                getTemplateByImageDelegate.BeginInvoke(nThreshold, nMaxCount, null, null);
            }
            catch (Exception ex)
            {
                Logger<TemplateManager>.Log.Error("btnQueryPhotoForScroe_Click", ex);
            }
        }

        /// <summary>
        /// 切换查询方式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryTypeChange_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == btnTiaojianQuery)
            {
                tMViewModel.TiaojianQueryVisiblity = Visibility.Visible;
                tMViewModel.PicQueryVisiblity = Visibility.Collapsed;
            }
            else
            {
                tMViewModel.TiaojianQueryVisiblity = Visibility.Collapsed;
                tMViewModel.PicQueryVisiblity = Visibility.Visible;
            }
        }

        /// <summary>
        /// 点击修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifyTemplate_Click(object sender, RoutedEventArgs e)
        {
            if (GlobalCache.AppType == 0)
            {
                UpdateTemplate();
            }
        }

        /// <summary>
        /// 查询模版的比对记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryTemplateCmp_Click(object sender, RoutedEventArgs e)
        {
            if (listViewTemplate.SelectedIndex < 0)
            {
                CodeStacksWindow.MessageBox.Invoke(false, false, 2, "请选中需要查询的模版!");
            }
            else
            {
                MyFaceObj myFaceObj = listViewTemplate.SelectedItem as MyFaceObj;
                QueryTemplateCmpDelegate(myFaceObj.tcName);
            }
        }

        /// <summary>
        /// loaded this page element
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TemplateInstance_Loaded(object sender, RoutedEventArgs e)
        {
            if (tMViewModel.ListMyFaceObj.Count <= 0)
            {
                btnTemplateQuery_Click(null, null);
            }
        }

    }
}
