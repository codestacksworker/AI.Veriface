using DATA.MODELS.GlobalModels;
using DATA.UTILITIES.Log4Net;
using PeopleModel;
using Prism.Commands;
using Prism.Mvvm;
using SENSING.THRIFT.CommonServices;
using SENSING.THRIFT.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Thrift.Transport;
using xiaowen.codestacks.data;
using xiaowen.codestacks.popwindow;
using xiaowen.codestacks.wpf.Utilities;

namespace SC_MODULES.ViewModels
{
    public partial class MainControlViewModel : BindableBase
    {
        CameraSnapPerson _templatePersonItem;
        public CameraSnapPerson TemplatePersonItem
        {
            get { return _templatePersonItem; }
            set { SetProperty(ref _templatePersonItem, value); }
        }

        IList<CameraSnapPerson> _templatePersonItems;
        public IList<CameraSnapPerson> TemplatePersonItems
        {
            get { return _templatePersonItems; }
            set { SetProperty(ref _templatePersonItems, value); }
        }

        /// <summary>
        /// 功能是否可用
        /// </summary>
        Visibility _isVisibleMarkUp;
        public Visibility IsVisibleMarkUp
        {
            get { return _isVisibleMarkUp; }
            set { SetProperty(ref _isVisibleMarkUp, value); }
        }

        void initSearchResult(object receivedData)
        {
            initSearchCmd();
            if (GlobalCache.AppType == 0)
                IsVisibleMarkUp = Visibility.Visible;
            else
                IsVisibleMarkUp = Visibility.Collapsed;
        }

        public ICommand GoToBICommand { get; set; }
        public ICommand GoToTRCommand { get; set; }

        public ICommand InToTemplate { get; set; }

        void initSearchCmd()
        {
            GoToBICommand = new DelegateCommand<object>((obj) =>
            {
                CameraSnapPerson model = obj as CameraSnapPerson;
                ReflactionView.GoTo1(model, 6, "HomeView", "FuncationToggleButton_Checked");
            });
            GoToTRCommand = new DelegateCommand<object>((obj) =>
            {
                CameraSnapPerson model = obj as CameraSnapPerson;
                ReflactionView.GoTo1(model, 5, "HomeView", "FuncationToggleButton_Checked");
            });
            InToTemplate = new DelegateCommand<object>(InToTemplateFunc);
        }


        /// <summary>
        /// 添加到模版
        /// </summary>
        /// <param name="obj"></param>
        private void InToTemplateFunc(object obj)
        {
            CameraSnapPerson model = obj as CameraSnapPerson;

            List<ErrorInfo> ListErrorInfo;
            //人员模版信息
            FaceObj faceObjnew = new FaceObj();
            faceObjnew = GetFace(model);
            faceObjnew.Tmplate = new List<FaceObjTemplate>();
            FaceObjTemplate faceTemp = new FaceObjTemplate();

            List<FaceObj> ListFaceObj = QueryFaceObj(model.SnapId);
            foreach (FaceObj faceobj in ListFaceObj)
            {
                if (faceobj != null)
                {
                    List<FaceObjTemplate> templist = faceobj.Tmplate;
                    for (int i = 0; i < 5; i++)
                    {
                        if (templist.Count > i && templist[i].Img.Length > 0)
                        {
                            faceTemp = new FaceObjTemplate();
                            faceTemp.TcUuid = templist[i].TcUuid;
                            faceTemp.TcObjid = faceObjnew.TcUuid;
                            //时间 
                            long addDateTime =
                                CodeStacksDataHandler.DateTimeData.ConvertToLongBySubstractDelegate.Invoke(DateTime.Now, new DateTime(1970, 1, 1));
                            faceTemp.DTm = addDateTime;//模板时间
                            faceTemp.Img = templist[i].Img;
                            faceTemp.NIndex = templist[i].NIndex;
                            faceObjnew.Tmplate.Add(faceTemp);
                        }
                    }
                    if (templist.Count >= 1 && templist.Count <= 4)
                    {
                        faceTemp = new FaceObjTemplate();
                        //时间 
                        long addDateTime =
                            CodeStacksDataHandler.DateTimeData.ConvertToLongBySubstractDelegate.Invoke(DateTime.Now, new DateTime(1970, 1, 1));

                        faceTemp.TcUuid = System.Guid.NewGuid().ToString().Replace("-", "");
                        faceTemp.TcObjid = faceObjnew.TcUuid;
                        faceTemp.DTm = addDateTime;//模板时间
                        faceTemp.Img = model.SourcePhotoByteArray;
                        faceTemp.NIndex = templist.Count;
                        faceObjnew.Tmplate.Add(faceTemp);

                    }
                    if (templist.Count == 5)
                    {
                        faceObjnew.Tmplate.RemoveAt(4);
                        faceTemp = new FaceObjTemplate();
                        faceTemp.TcUuid = System.Guid.NewGuid().ToString().Replace("-", ""); ;
                        faceTemp.TcObjid = faceObjnew.TcUuid;
                        //时间 
                        long addDateTime =
                            CodeStacksDataHandler.DateTimeData.ConvertToLongBySubstractDelegate.Invoke(DateTime.Now, new DateTime(1970, 1, 1));
                        faceTemp.DTm = addDateTime;//模板时间
                        faceTemp.Img = model.SourcePhotoByteArray;
                        faceTemp.NIndex = 4;
                        faceObjnew.Tmplate.Add(faceTemp);
                    }
                }
            }

            ListErrorInfo = ModifyFaceObj(faceTemp.TcObjid, faceObjnew);

            //判断是否成功
            if (ListErrorInfo.Count == 0)//判断成功，提示成功
            {
                CodeStacksWindow.MessageBox.Invoke(false, false, 2, "成功！");
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

        /// <summary>
        /// 根据ID获取模版照片信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private byte[][] GetTempImg(string id)
        {
            byte[][] tempImg = new byte[5][];
            List<FaceObj> ListFaceObj = QueryFaceObj(id);
            foreach (FaceObj faceobj in ListFaceObj)
            {
                if (faceobj != null)
                {
                    List<FaceObjTemplate> templist = faceobj.Tmplate;
                    for (int i = 0; i < 5; i++)
                    {
                        if (templist.Count > i && templist[i].Img.Length > 0)
                        {
                            var tmpObj = templist.FirstOrDefault(t => t.NIndex == i);
                            tempImg[i] = tmpObj.Img;
                            //tmpObj.TcUuid
                        }
                    }
                }
            }
            return tempImg;
        }

        /// <summary>
        /// 查询模版
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public List<FaceObj> QueryFaceObj(string id)
        {
            List<FaceObj> _ListFaceObj = new List<FaceObj>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                _ListFaceObj = SocketOpter.GetResult<MainControlViewModel, List<FaceObj>, string, string, int, int, int, int, long, long, int, int>(
                    transport,
                    bServerClient.QueryFaceObj,
                    "QueryFaceObj 查询模版", true,
                     id, string.Empty, -1, -1, -1, -1, long.MinValue, long.MinValue, -1, -1
                    );
            }
            catch (Exception ex)
            {
                Logger<MainControlViewModel>.Log.Error("QueryFaceObjTotalCount", ex);
            }
            return _ListFaceObj;
        }

        /// <summary>
        /// 修改模版
        /// </summary>
        /// <param name="id"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<ErrorInfo> ModifyFaceObj(string id, FaceObj obj)
        {
            List<ErrorInfo> result = new List<ErrorInfo>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<MainControlViewModel, List<ErrorInfo>, string, FaceObj>(
                    transport,
                    bServerClient.ModifyFaceObj,
                    "ModifyFaceObj", true,
                    id, obj
                    );
            }
            catch (Exception)
            {
            }
            return result;
        }

        private FaceObj GetFace(CameraSnapPerson snap)
        {
            FaceObj face = new FaceObj();
            face.TcUuid = snap.SnapId;
            face.TcName = snap.Name;
            face.NExten = snap.Exten;
            //face.Tmplate[0].Img = snap.PhotoByteArray;
            face.NMain_ftID = snap.Main_ftID;
            face.NType = snap.Type;
            face.NSST = snap.SST;
            face.NSex = snap.Sex;
            face.NAge = snap.Age;
            face.DTm = snap.Tm;
            face.TcRemarks = snap.Remarks;
            return face;
        }
    }
}
