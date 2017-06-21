using DATA.UTILITIES.Log4Net;
using FaceSysByMvvm.ViewModels.TemplateManager;
using SENSING.ClassPool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using xiaowen.codestacks.data;
using xiaowen.codestacks.popwindow;

namespace FaceSysByMvvm.Views.ChannelManager
{
    /// <summary>
    /// TempleteInfoPop.xaml 的交互逻辑
    /// </summary>
    public partial class TempleteInfoPop : Window
    {
        validationRule _validationRule = new validationRule();
        TempleteInfoPopViewModel tIPViewModel;
        string[] initPhotoContainer = new string[5] { "空", "空", "空", "空", "空" };//保存五张照片的地址
        byte[] _mainTemplateStream;//保存主照片
        byte[][] templateData = new byte[5][];//5张照片的数据
        int currentSelectedPhoto = -1;//当前选中的要删除的照片的索引

        public TempleteInfoPop()
        {
            try
            {
                InitializeComponent();
                tIPViewModel = new TempleteInfoPopViewModel();
                this.DataContext = tIPViewModel;
                this.Loaded += TempleteInfoPop_Loaded;
            }
            catch (Exception ex)
            {
                Logger<TempleteInfoPop>.Log.Error("AddTemplate：AddTemplate", ex);
            }
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TempleteInfoPop_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                BrushTemplatePhoto();
            }
            catch (Exception ex)
            {
                Logger<TempleteInfoPop>.Log.Error("AddTemplate:Window_Loaded_1", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        void BrushTemplatePhoto()
        {
            //await Task.Run(() =>
            //{
            //    DispatcherHelper.CheckBeginInvokeOnUI(() =>
            //    {
            //    });
            //});

            try
            {
                //判断人脸对象不为空，那么显示具体信息
                if (tIPViewModel._FaceObj != null && tIPViewModel._FaceObj.Tmplate != null)
                {
                    //根据其之前的设计，肯定有一张为主模板
                    var mainTemplate = tIPViewModel._FaceObj.Tmplate.FirstOrDefault(t => t.NIndex == tIPViewModel._FaceObj.NMain_ftID);
                    _mainTemplateStream = mainTemplate.Img;
                    this.btnPicMain.Source = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(mainTemplate.Img);
                    this.btnPicMain.IsEnabled = true;
                    for (int i = 0; i < 5; i++)
                    {
                        if (tIPViewModel._FaceObj.Tmplate.Count > i && tIPViewModel._FaceObj.Tmplate[i].Img.Length > 0)
                        {
                            //var tmpObj = tIPViewModel._FaceObj.Tmplate.FirstOrDefault(t => t.NIndex == i);
                            var tmpObj = tIPViewModel._FaceObj.Tmplate[i];

                            Grid gallery = templategallery.Children[i] as Grid;
                            Image image = gallery.Children[0] as Image;
                            image.Source = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(tmpObj.Img);
                            templateData[i] = tmpObj.Img;
                            initPhotoContainer[i] = "1";
                            image.IsEnabled = true;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (templateData[i] != null)
                        {
                            templateData[i] = System.IO.File.ReadAllBytes(initPhotoContainer[i]);
                            Grid gallery = templategallery.Children[i] as Grid;
                            Image image = gallery.Children[0] as Image;
                            image.Source = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(templateData[i]);
                            image.IsEnabled = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger<TempleteInfoPop>.Log.Error("AddTemplate:Window_Loaded_1", ex);
            }
        }

        /// <summary>
        /// 移除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                switch (currentSelectedPhoto)
                {
                    case 0:
                        //将第一张图片删除
                        template0.Source = null;
                        if (_mainTemplateStream != null && _mainTemplateStream.Equals(templateData[0]))
                        {
                            tIPViewModel._FaceObj.NMain_ftID = 0;//主照片ID
                        }
                        initPhotoContainer[0] = "空";
                        templateData[0] = null;
                        break;
                    case 1:
                        template1.Source = null;
                        if (_mainTemplateStream != null && _mainTemplateStream.Equals(templateData[1]))
                        {
                            tIPViewModel._FaceObj.NMain_ftID = 0;//主照片ID
                        }
                        initPhotoContainer[1] = "空";
                        templateData[1] = null;
                        break;
                    case 2:
                        template2.Source = null;
                        if (_mainTemplateStream != null && _mainTemplateStream.Equals(templateData[2]))
                        {
                            tIPViewModel._FaceObj.NMain_ftID = 0;//主照片ID
                        }
                        initPhotoContainer[2] = "空";
                        templateData[2] = null;
                        break;
                    case 3:
                        template3.Source = null;
                        if (_mainTemplateStream != null && _mainTemplateStream.Equals(templateData[3]))
                        {
                            tIPViewModel._FaceObj.NMain_ftID = 0;//主照片ID
                        }
                        initPhotoContainer[3] = "空";
                        templateData[3] = null;
                        break;
                    case 4:
                        template4.Source = null;
                        if (_mainTemplateStream != null && _mainTemplateStream.Equals(templateData[4]))
                            tIPViewModel._FaceObj.NMain_ftID = 0;//主照片ID

                        initPhotoContainer[4] = "空";
                        templateData[4] = null;
                        break;
                    default:
                        currentSelectedPhoto = -1;
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger<TempleteInfoPop>.Log.Error("AddTemplate：btnRemove_Click", ex);
            }
        }

        string guid = string.Empty;
        /// <summary>
        /// 设置初始值
        /// </summary>
        /// <param name="myFaceObj"></param>
        /// <param name="i">1:添加模版 2:修改模版 3:抓拍修改模版</param>
        /// <param name="image">从抓拍处添加模版,传入的图片</param>
        public void SetTempleteInfo(MyFaceObj myFaceObj, int i, byte[] image)
        {
            try
            {
                if (i == 2)
                {
                    tIPViewModel.Id = guid = myFaceObj.fa_ob_tcUuid.ToString();
                    tIPViewModel.ImportTime = myFaceObj.fa_ob_dTm;
                    tIPViewModel.Remark = myFaceObj.fa_ob_tcRemarks;
                    tIPViewModel.Age = myFaceObj.nAge.ToString();
                    tIPViewModel.Name = myFaceObj.tcName;
                    tIPViewModel.SelectedSex = tIPViewModel.Sex.IndexOf(myFaceObj.nSex);
                    tIPViewModel.SelectedType = tIPViewModel.Type.IndexOf(myFaceObj.nType);
                    tIPViewModel._FaceObj = myFaceObj.faceObj;
                    tIPViewModel.Title = @"..\..\Images\子菜单修改模板.png";
                }
                else
                {
                    tIPViewModel.Id = guid = System.Guid.NewGuid().ToString().Replace("-", "");
                    tIPViewModel.ImportTime = DateTime.Now.ToString();
                    if (i == 3)
                    {
                        tIPViewModel._FaceObj.Tmplate = new List<FaceObjTemplate>();
                        FaceObjTemplate faceObjectTemplate = new FaceObjTemplate();
                        faceObjectTemplate.Img = image;
                        tIPViewModel._FaceObj.Tmplate.Add(faceObjectTemplate);
                    }
                    tIPViewModel.Title = @"..\..\Images\子菜单添加模板.png";
                }
            }
            catch (Exception ex)
            {
                Logger<TempleteImportPop>.Log.Error("SetTempleteInfo", ex);
            }
        }


        /// <summary>
        /// 右上角关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 浏览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                Nullable<bool> result = null;
                try
                {
                    dlg.Filter = "jpg|*.jpg|bmp|*.bmp|png|*.png";
                    dlg.Multiselect = true;
                    dlg.RestoreDirectory = true;
                    dlg.FilterIndex = 1;
                    result = dlg.ShowDialog();
                }
                catch (Exception)
                {
                }

                string[] paths = null;
                if (result == true)
                    paths = dlg.FileNames;

                if (paths.Length > 5)
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "最多添加5张照片");
                    paths = null;
                    return;
                }
                else
                {
                    int exsistCount = 0;
                    foreach (var item in initPhotoContainer)
                    {
                        if (!"空".Equals(item)) exsistCount++;
                    }

                    foreach (var photo in paths)
                    {
                        for (int i = 0; i < 5; i++)
                        {
                            if ("空".Equals(initPhotoContainer[i]))
                            {
                                initPhotoContainer[i] = photo;
                                break;
                            }
                        }
                    }

                    //IEnumerable union = initPhotoContainer.Union(paths);
                    //IEnumerable photosPath0 = fileName.Union(paths);
                    //IEnumerable photosPath = fileName.Intersect(paths);

                    ShowTemplatePhoto();
                }
            }
            catch (Exception ex)
            {
                Logger<TempleteInfoPop>.Log.Error("AddTemplate：btnBrowse_Click", ex);
            }
        }

        /// <summary>
        /// 显示5张模板图片
        /// </summary>
        private void ShowTemplatePhoto()
        {
            try
            {
                //图片1
                if (templateData[0] == null && initPhotoContainer[0] != "空")
                {
                    templateData[0] = File.ReadAllBytes(initPhotoContainer[0]);
                    this.template0.Source = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(templateData[0]);
                    this.template0.IsEnabled = true;
                }
                if (templateData[1] == null && initPhotoContainer[1] != "空")
                {
                    templateData[1] = System.IO.File.ReadAllBytes(initPhotoContainer[1]);
                    ////图片2
                    this.template1.Source = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(templateData[1]);
                    this.template1.IsEnabled = true;
                }
                //图片3
                if (templateData[2] == null && initPhotoContainer[2] != "空")
                {
                    templateData[2] = System.IO.File.ReadAllBytes(initPhotoContainer[2]);
                    this.template2.Source = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(templateData[2]);
                    this.template2.IsEnabled = true;
                }
                //图片4
                if (templateData[3] == null && initPhotoContainer[3] != "空")
                {
                    templateData[3] = System.IO.File.ReadAllBytes(initPhotoContainer[3]);
                    this.template3.Source = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(templateData[3]);
                    this.template3.IsEnabled = true;
                }
                //图片5
                if (templateData[4] == null && initPhotoContainer[4] != "空")
                {
                    templateData[4] = System.IO.File.ReadAllBytes(initPhotoContainer[4]);
                    this.template4.Source = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(templateData[4]);
                    this.template4.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Logger<TempleteInfoPop>.Log.Error("AddTemplate：btnBrowse_Click", ex);
            }
        }


        /// <summary>
        /// 取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDetermineAddTemplate_Click(object sender, RoutedEventArgs e)
        {
            List<ErrorInfo> ListErrorInfo;
            ThriftServiceNameSpace.ThriftService thirft = new ThriftServiceNameSpace.ThriftService();

            try
            {
                /**
                 * 姓名
                 * 性别
                 * 年龄
                 * 类型：普通、黑名单
                 * 备注
                 * 1张主照片
                 * 4张附属照片 均为模板照片，指向一个人
                 * **/

                #region 验证
                if (string.IsNullOrEmpty(txtName.Text.Trim()))
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "名称必填！");
                    return;
                }
                if (string.IsNullOrEmpty(txtAge.Text.Trim()))
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "年龄必填！");
                    return;
                }
                #endregion

                #region 添加输入项

                tIPViewModel._FaceObj.TcUuid = guid;
                tIPViewModel._FaceObj.TcName = txtName.Text.Trim();
                string message = _validationRule.intValidationRule(this.txtAge.Text.Trim());
                if (message != "")
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, message);
                    return;
                }
                tIPViewModel._FaceObj.NAge = int.Parse(txtAge.Text.Trim());
                tIPViewModel._FaceObj.NSex = combSex.SelectedIndex;
                //暂时没确定
                tIPViewModel._FaceObj.NType = tIPViewModel.SelectedType;
                tIPViewModel._FaceObj.TcRemarks = txtRemarks.Text.Trim();
                #endregion

                tIPViewModel._FaceObj.Tmplate = new List<FaceObjTemplate>();
                //遍历照片存放数组，必须添加至少一张照片
                var isExistPhoto = templateData.FirstOrDefault(x => x != null) ?? null;
                if (isExistPhoto == null)
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "至少要一张照片才能注册成功！");
                    return;
                }

                int nj = -1;

                for (int i = 0; i < 5; i++)
                {
                    if (templateData[i] != null)
                    {
                        FaceObjTemplate templateObj = new FaceObjTemplate();
                        templateObj.TcUuid = System.Guid.NewGuid().ToString().Replace("-", ""); ;
                        templateObj.TcObjid = tIPViewModel._FaceObj.TcUuid;

                        //判断前面是不是有空位置，有那么排到前面去
                        for (int j = 0; j < i; j++)
                        {
                            if (templateData[j] == null)
                            {
                                templateObj.NIndex = j;
                                nj = j;
                                break;
                            }
                        }
                        if (templateObj.NIndex == 0)
                        {
                            if (nj == -1)
                            {
                                templateObj.NIndex = i;
                            }
                            else
                            {
                                templateObj.NIndex = nj;
                            }
                        }
                        //时间 
                        long addDateTime =
                            CodeStacksDataHandler.DateTimeData.ConvertToLongBySubstractDelegate.Invoke(DateTime.Now, new DateTime(1970, 1, 1));

                        templateObj.DTm = addDateTime;//模板时间
                        if (tIPViewModel._FaceObj.DTm == 0) tIPViewModel._FaceObj.DTm = addDateTime;

                        if (_mainTemplateStream != null)
                        {
                            if (_mainTemplateStream.Equals(templateData[i]))
                            {
                                tIPViewModel._FaceObj.NMain_ftID = i;//主照片ID
                            }
                            else if (i == tIPViewModel._FaceObj.NMain_ftID)
                            {
                                tIPViewModel._FaceObj.NMain_ftID = templateObj.NIndex;//主照片ID
                            }
                        }
                        else
                        {
                            tIPViewModel._FaceObj.NMain_ftID = 0;//主模板ID为 0 
                        }

                        //Stream stream = new MemoryStream(templateData[i]);
                        //templateObj.Img = ImageConvert.CompressImage(stream);

                        templateObj.Img = templateData[i];
                        tIPViewModel._FaceObj.Tmplate.Add(templateObj);//Add Template to db.face_template/db.face_object

                        if (nj != -1)
                        {
                            templateData[nj] = templateData[i];
                            templateData[i] = null;
                            nj = -1;
                        }
                    }
                }
                //具体的操作(添加,修改)
                if (tIPViewModel.Title.Contains("添加"))
                {
                    ListErrorInfo = thirft.AddFaceObj(tIPViewModel._FaceObj);
                }
                else
                {
                    ListErrorInfo = thirft.ModifyFaceObj(tIPViewModel.Id, tIPViewModel._FaceObj);
                }
                //判断是否成功
                if (ListErrorInfo.Count == 0)//判断成功，提示成功
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "成功！");
                    this.Close();
                }
                else//添加不成功，显示错误
                {
                    if ("Add Face To Compare Server Failed, Because img repeat".Equals(ListErrorInfo[0].ID))
                    {
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, "模板照片已存在，请取消并重新添加模板");
                        return;
                    }
                    else
                    {
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, "添加模板ID：" + ListErrorInfo[0].ID + "失败,\n失败信息：" + ListErrorInfo[0].ErrCode);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger<TempleteInfoPop>.Log.Error("AddTemplate：btnRemove_Click", ex);
            }
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// selected main photo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void template_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                Image templateObj = sender as Image;
                currentSelectedPhoto = Convert.ToUInt16(templateObj.Tag);
                _mainTemplateStream = templateData[currentSelectedPhoto];
                btnPicMain.Source = templateObj.Source;
            }
            catch (Exception ex)
            {
                Logger<TempleteInfoPop>.Log.Error("template_MouseLeftButtonDown", ex);
            }
        }
    }
}
