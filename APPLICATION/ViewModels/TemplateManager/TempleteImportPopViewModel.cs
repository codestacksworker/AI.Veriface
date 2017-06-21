using ThriftServiceNameSpace;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using SENSING.ClassPool;
using System.IO;
using DATA.UTILITIES.Log4Net;
using xiaowen.codestacks.popwindow;
using SENSING.APPLICATION.Common;
using xiaowen.codestacks.data;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace FaceSysByMvvm.ViewModels.TemplateManager
{
    public class TempleteImportPopViewModel : BindableBase
    {
        ThriftServiceNameSpace.ThriftService thirft = new ThriftServiceNameSpace.ThriftService();
        #region 属性的定义

        TempleteImportPopViewModel templeteViewModel;
        public TempleteImportPopViewModel TempleteViewModel
        {
            get
            {
                return templeteViewModel;
            }

            set
            {
                SetProperty(ref templeteViewModel, value);
            }
        }

        //是否包含子文件夹      
        private int selectedAllDocu;
        public int SelectedAllDocu
        {
            get { return selectedAllDocu; }
            set
            {
                SetProperty(ref selectedAllDocu, value);
            }
        }
        private List<string> allDocu;
        public List<string> AllDocu
        {
            get { return allDocu; }
            set
            {
                SetProperty(ref allDocu, value);
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

        //进度条当前进度
        private int currentLength;
        public int CurrentLength
        {
            get { return currentLength; }
            set
            {
                SetProperty(ref currentLength, value);
            }
        }

        //进度条最大刻度
        private int maxLength;
        public int MaxLength
        {
            get { return maxLength; }
            set
            {
                SetProperty(ref maxLength, value);
            }
        }

        //错误信息
        private string errorInfo;
        public string ErrorInfo
        {
            get { return errorInfo; }
            set
            {
                SetProperty(ref errorInfo, value);
            }
        }
        //成功数量
        private int successCount;
        public int SuccessCount
        {
            get { return successCount; }
            set
            {
                SetProperty(ref successCount, value);
            }
        }


        //错误数量
        private int errorCount;
        public int ErrorCount
        {
            get { return errorCount; }
            set
            {
                SetProperty(ref errorCount, value);
            }
        }

        //错误照片存放地址
        private string errorAddress;
        public string ErrorAddress
        {
            get { return errorAddress; }
            set
            {
                SetProperty(ref errorAddress, value);
            }
        }

        //导入进度描述
        private string importInfo;
        public string ImportInfo
        {
            get { return importInfo; }
            set
            {
                SetProperty(ref importInfo, value);
            }
        }

        #endregion

        #region 属性初始化
        public delegate List<ErrorInfo> ThreadImportPicIntoDbDelegate(FaceObj _FaceObj);
        ThreadImportPicIntoDbDelegate AddFaceObjDelegate;

        public TempleteImportPopViewModel()
        {
            this.initCmd();
            //this.templeteViewModel = new TempleteImportPopViewModel();

            //初始化是否包含子文件夹
            AllDocu = new List<string> { "是", "否" };
            SelectedAllDocu = 0;

            //初始化模版类型
            Type = thirft.QueryDefFaceObjType();
            SelectedType = 0;

            //初始化模版性别
            SelectedSex = 0;
            Sex = new List<string> { "全部", "男", "女" };

            //初始化进度条当前进度
            CurrentLength = 0;
            //初始化进度条最大进度
            MaxLength = 100;
            //初始化错误照片数量
            errorCount = 0;
            //初始化错误信息
            errorInfo = "";
            //初始化错误照片保存地址
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            const string strDirDest = "\\ImageError";
            errorAddress = dir + strDirDest;
            //初始化导入进度描述
            ImportInfo = "点击导入模板文件按钮，导入模板";
            AddFaceObjDelegate = new ThreadImportPicIntoDbDelegate(delegate (FaceObj _FaceObj)
            {
                return thirft.AddFaceObj(_FaceObj);
            });
        }
        #endregion

        #region  Command 事件

        public ICommand ImportTemplateFileCommand { get; private set; }
        public ICommand SelectWrongPhotoPathCommand { get; private set; }

        void initCmd()
        {
            ImportTemplateFileCommand = new DelegateCommand<object>(ImportTemplateFileCommandFunc);
            SelectWrongPhotoPathCommand = new DelegateCommand<object>(SelectWrongPhotoPathCommandFunc);
        }
        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="obj"></param>
        private void ImportTemplateFileCommandFunc(object obj)
        {
            try
            {
                if (obj == null) return;
                TempleteImportPopViewModel viewModel = obj as TempleteImportPopViewModel;
                bool bIsContainsSubFold = viewModel.SelectedAllDocu == 0 ? true : false;

                string strSex = viewModel.Sex[viewModel.SelectedSex];//sex
                string strType = viewModel.Type[viewModel.SelectedType];//direct type by utilities
                string sourcePath = string.Empty;
                System.Windows.Forms.FolderBrowserDialog _FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                if (_FolderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    sourcePath = _FolderBrowserDialog.SelectedPath;
                    string dir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    if (dir.Equals(sourcePath))
                    {
                        CodeStacksWindow.MessageBox.Invoke(true, false, 2, "未选择"); return;
                    }
                }
                else return;

                FileInfo[] fileInfo = OperateFiles.GetFilesPic(sourcePath, bIsContainsSubFold);

                int maxCount = 0;
                OperateFiles.GetNumReadPic(fileInfo, ref maxCount);
                MaxLength = maxCount;
                CurrentLength = 0;
                SuccessCount = 0;
                ErrorCount = 0;

                #region 人脸对象操作
                for (int i = 0; i < fileInfo.Length; i++)
                {
                    string sourceName = (fileInfo[i].DirectoryName + "\\" + fileInfo[i].Name);

                    if (File.Exists(sourceName))
                    {
                        #region 人脸对象
                        FaceObj faceObj = new FaceObj();//实例OrderInf，添加到新的窗口的list中
                                                        //人脸对象添加时间
                        faceObj.DTm = CodeStacksDataHandler.DateTimeData.ConvertToLongBySubstractDelegate.Invoke(DateTime.Now, new DateTime(1970, 1, 1));
                        faceObj.TcUuid = Guid.NewGuid().ToString().Replace("-", ""); //生成人脸对象uuid
                        faceObj.TcName = fileInfo[i].Name.Substring(0, fileInfo[i].Name.Length - 4);  //人脸名称

                        faceObj.Tmplate = new List<FaceObjTemplate>();
                        FaceObjTemplate faceObjTemplate = new FaceObjTemplate();
                        faceObj.Tmplate.Add(faceObjTemplate);

                        faceObj.NMain_ftID = 0;//设置主模板ID
                        faceObj.Tmplate[0].Img = File.ReadAllBytes(sourceName);
                        //判断统一指定类型
                        if ((strSex == "全部" && strType == "请选择"))
                        {
                            faceObj.NSex = 0;
                            faceObj.NType = 1;
                        }
                        else
                        {
                            if (strSex != "" && strType != "")
                            {
                                faceObj.NSex = viewModel.SelectedSex;
                                faceObj.NType = viewModel.SelectedType;
                            }
                            else
                            {
                                if (strSex != "")//如果指定了性别
                                {
                                    faceObj.NSex = viewModel.SelectedSex;
                                    faceObj.NType = 1;
                                }
                                if (strType != "")//如果指定了类型
                                {
                                    faceObj.NType = viewModel.SelectedType;
                                    faceObj.NSex = 0;
                                }
                            }
                        }
                        //补足模板中不足的字段
                        for (int k = 0; k < faceObj.Tmplate.Count; k++)
                        {
                            if (faceObj.Tmplate[k].Img != null)//判断模板照片存在
                            {
                                faceObj.Tmplate[k].TcUuid = Guid.NewGuid().ToString().Replace("-", "");  //生成uuid                               
                                faceObj.Tmplate[k].TcObjid = faceObj.TcUuid;//所属FaceObj的uuid                     
                                faceObj.Tmplate[k].NIndex = k;//模板序号                            
                                faceObj.Tmplate[k].DTm = CodeStacksDataHandler.DateTimeData.ConvertToLongBySubstractDelegate.Invoke(DateTime.Now, new DateTime(1970, 1, 1)); // 模板添加时间
                            }
                        }
                        #endregion

                        #region  将人脸对象添加到数据库
                        while (true)
                        {
                            //thirft.AddFaceObj(faceObj);
                            IAsyncResult result = AddFaceObjDelegate.BeginInvoke(faceObj, new AsyncCallback(PicHandle), faceObj);

                            break;
                        }
                        #endregion
                    }
                }
                #endregion 

            }
            catch (Exception ex)
            {
                Logger<TempleteImportPopViewModel>.Log.Error("ImportTemplateFileCommandFunc", ex);
            }
        }

        /// <summary>
        /// 将人脸对象批量上传的回调函数(错误的保存)
        /// </summary>
        /// <param name="ar"></param>
        private void PicHandle(IAsyncResult ar)
        {
            try
            {
                FaceObj _FaceObj = ar.AsyncState as FaceObj;
                AsyncResult a = (AsyncResult)ar;
                ThreadImportPicIntoDbDelegate trys = (ThreadImportPicIntoDbDelegate)a.AsyncDelegate;
                List<ErrorInfo> ListErrorInfo = trys.EndInvoke(ar);
                if (ListErrorInfo.Count > 0)
                {
                    for (int l = 0; l < ListErrorInfo.Count; l++)
                    {
                        Console.WriteLine(ListErrorInfo[l].ErrCode + "");
                        if (ListErrorInfo[l].ErrCode == -1)//小于0，注册失败
                        {

                            byte[] photo = _FaceObj.Tmplate[0].Img;
                            MemoryStream stream = new MemoryStream(photo);
                            System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                            //判断文件夹存在 
                            DirectoryInfo directory = new DirectoryInfo(templeteViewModel.ErrorAddress);
                            if (!directory.Exists)
                            {
                                //重新生成文件夹
                                Directory.CreateDirectory(templeteViewModel.ErrorAddress);
                            }

                            string strPath = "";

                            strPath = templeteViewModel.ErrorAddress + @"\" + _FaceObj.TcName + @".jpg";
                            img.Save(strPath, img.RawFormat);
                            templeteViewModel.ErrorCount++;
                        }
                        if (ListErrorInfo[l].ErrCode == -3 || ListErrorInfo[l].ErrCode == -2)//图片不存在
                        {
                            templeteViewModel.ErrorCount++;
                        }
                    }
                }
                else
                {
                    ++templeteViewModel.SuccessCount;
                }

            }
            catch (Exception ex)
            {
                Logger<TempleteImportPopViewModel>.Log.Error("PicHandle", ex);
                templeteViewModel.ErrorCount++;
            }
            finally
            {
                ++templeteViewModel.CurrentLength;
                templeteViewModel.ImportInfo = "总数量:" + templeteViewModel.MaxLength +
                    "  上传数量:" + templeteViewModel.CurrentLength + "成功数量:" + templeteViewModel.SuccessCount + "";
                if (templeteViewModel.MaxLength == templeteViewModel.CurrentLength)
                {
                    CodeStacksDataHandler.UIThread.Invoke(() =>
                    {
                        CodeStacksWindow.MessageBox.Invoke(false, false, 1, "模版已全部导入完成！");
                    });
                }
            }
        }
        /// <summary>
        /// 选择错误图片保存地址
        /// </summary>
        /// <param name="obj"></param>
        private void SelectWrongPhotoPathCommandFunc(object obj)
        {
            try
            {
                System.Windows.Forms.FolderBrowserDialog _FolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
                if (_FolderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    const string strDirDest = "\\ImageError";
                    string strPath = _FolderBrowserDialog.SelectedPath + strDirDest;
                    if (!String.IsNullOrEmpty(strPath))
                    {
                        if (!Directory.Exists(strPath))
                            Directory.CreateDirectory(strPath);
                        ErrorAddress = strPath;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger<TempleteImportPopViewModel>.Log.Error("SelectWrongPhotoPathCommandFunc", ex);
            }
        }
        #endregion 
    }
}
