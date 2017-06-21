using System;
using System.Collections.Generic;
using System.Text;
using DATA.UTILITIES.Log4Net;
using DATA.MODELS.GlobalModels;
using System.Reflection;
using SENSING.ClassPool;
using System.Windows.Controls;
using System.Threading;
using DATA.UTILITIES.SensingFunc;
using xiaowen.codestacks.popwindow;

namespace SENSING.THRIFT.Services
{
    public class UIServerInterForWifi : UIServer.Iface
    {
        public MyCapFaceLogWithImg snapPersonObj;
        public PublishResult compareResult;
        List<string> listQueryDefFaceObjType = new List<string>();
        Control channelMgrCtrl = null;
        object snapField = null;
        ManualResetEvent resetSnapInfoField = null;
        object snapStreamField = null;
        object compareStreamField = null;
        object identifyResult = null;
        ManualResetEvent resetCompareInfoField = null;

        /// <summary>
        /// 
        /// </summary>
        public void Get(ManualResetEvent snap, ManualResetEvent snapCompare, PublishResult result, byte[] snapStream, byte[] compareStream)
        {
            try
            {
                TypeInfo channelMgrType = GlobalCache.ChannelMgrObj.GetType() as TypeInfo;
                channelMgrCtrl = GlobalCache.ChannelMgrObj as Control;

                foreach (FieldInfo item in channelMgrType.DeclaredFields)
                {
                    switch (item.Name)
                    {
                        case "CapimageByteRealtimeCapInfo":
                            snapField = item;
                            break;
                        case "_MyCapFaceLogWithImg":
                            break;
                        case "ResetServerRealtimeCapInfo":
                            //TypeInfo ti = item.FieldType as TypeInfo;
                            //ConstructorInfo cio = ti.GetConstructors().FirstOrDefault();
                            //object _obj = cio.Invoke(new object[] { false });
                            //resetSnapInfoField = _obj as ManualResetEvent;
                            resetSnapInfoField = snap;
                            break;
                        case "snapStream":
                            snapStreamField = snapStream;
                            break;
                        case "campareStream":
                            compareStreamField = compareStream;
                            break;
                        case "_IdentifyResults":
                            identifyResult = result;
                            break;
                        case "ResetServerRealtimeCmpInfo":
                            resetCompareInfoField = snapCompare;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger<UIServerInterForWifi>.Log.Error("UIServerInterForWifi.Get()", ex);
            }
        }


        public int UpdateRealtimeCapLBS(RealtimeCapInfoLBS info)
        {
            try
            {
                if (GlobalCache.AppType == 1)
                {
                    if (!info.Channelname.Contains(GlobalCache.AppLocation))
                    {
                        return -1;
                    }
                }
                //接收服务器附送过来的实时照片
                if (snapPersonObj != null)
                    snapPersonObj = null;
                snapPersonObj = new MyCapFaceLogWithImg();
                snapPersonObj.ID = info.Id;// 抓拍id
                //string strChannelId = info.Channel.Substring(0, 6) + "\r\n";
                //strChannelId += info.Channel.Substring(0, 6);
                snapPersonObj.ChannelID = info.Channel;// 通道id
                snapPersonObj.ChannelName = info.Channelname;
                snapPersonObj.Longitude = info.Longitude;
                snapPersonObj.Latitude = info.Latitude;
                snapPersonObj.Score = info.Score;
                snapPersonObj.Address = info.Address;

                DateTime s = new DateTime(1970, 1, 1);
                s = s.AddSeconds(info.Time);
                snapPersonObj.time = s.ToString("yyyy/MM/dd HH:mm:ss");

                //ChannelManage.CapimageByteRealtimeCapInfo = info.Image;
                //ChannelManage._MyCapFaceLogWithImg = snapPersonImage;
                //ChannelManage.ResetServerRealtimeCapInfo.Set();

                GlobalCache.SnapStream = info.Image;
                //snapFaceImgField = snapPersonObj;
                GlobalCache.MySnapFaceLogWithImgObj = snapPersonObj;
                resetSnapInfoField.Set();

                return 0;
            }
            catch (Exception ex)
            {
                Logger<UIServerInterForWifi>.Log.Error("UpdateRealtimeCapLBS", ex);
                return -1;
            }
        }

        /// <summary>
        /// 实时更新比对结果，显示到结果列表中
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public int UpdateRealtimeCmpLBS(RealtimeCmpInfoLBS info)
        {
            try
            {
                if ("WIFI".Equals(GlobalCache.NetworkMode))
                {
                    Active(info);
                }
                else
                {
                    if (GlobalCache.AppType == 1)
                    {
                        if (!info.Channelname.StartsWith("##"))
                        {
                            return -1;
                        }
                    }
                    Active(info);

                }
                return 0;
            }
            catch (Exception ex)
            {
                Logger<UIServerInterForWifi>.Log.Error("UpdateRealtimeCmpLBS", ex);
                return -1;
            }
        }


        void Active(RealtimeCmpInfoLBS info)
        {
            //显示在界面上的结果
            if (compareResult != null)
                compareResult = null;
            compareResult = new PublishResult();
            compareResult.ID = info.CapID;
            if (info.CapID == null || info.CapID == "")
            {
                CodeStacksWindow.MessageBox.Invoke(true, false, 2, "抓拍照片ID为空");
            }
            compareResult.RegID = info.ObjID;
            if (info.ObjID == null || info.ObjID == "")
            {
                CodeStacksWindow.MessageBox.Invoke(true, false, 2, "抓拍照片ID为空");
            }
            long _longtime = info.Time;
            DateTime s = new DateTime(1970, 1, 1);
            s = s.AddSeconds(_longtime);

            GlobalCache.SnapStream = info.CapImg;
            GlobalCache.SnapCompareStream = info.ObjImg;

            //获得主照片这侧信息
            StringBuilder strRegster = new StringBuilder();
            //注册名称
            strRegster.Append(info.Name + "\r\n");
            //获得通道名称
            string ChannelName = info.Channelname;
            compareResult.ChannelName = info.Channelname;
            strRegster.Append(ChannelName.Replace("##", "").Replace("@", "") + "\r\n");
            //抓拍时间
            int nIndexS = s.ToString().IndexOf(" ");
            strRegster.Append(s.ToString().Substring(0, nIndexS) + "\r\n");
            int nIndexS1 = s.ToString().Length - nIndexS;
            strRegster.Append(s.ToString().Substring(nIndexS + 1, nIndexS1 - 1) + "\r\n");
            //注册类型
            string type = BasicDataEntry.GetTemplateType(info.Type);//eg：白名单、黑名单、普通...

            strRegster.Append(type + "\r\n");
            compareResult.TemplateType = type;
            //相似度。
            strRegster.Append(info.Score + "\r\n");
            compareResult.RegInfo = strRegster.ToString();
            compareResult.NewCmpResult = info;
            GlobalCache.IdentifyResult = compareResult;
            resetCompareInfoField.Set();

            ////弹出报警框
            //AlarmWindow _AlarmWindow = new AlarmWindow();
            //_AlarmWindow.CapimageByteRealtimeCmpInfo = info.CapImg;
            //_AlarmWindow.CmpimageByteRealtimeCmpInfo = info.CapImg;
            //_AlarmWindow.RegInfo = strRegster.ToString();
            //_AlarmWindow.nInfoType = info.Type;
            //_AlarmWindow.Show();
        }


        public int UpdateRealtimeCmpEntertainment(CmpFaceLogDS cfg)
        {
            return 1;
        }

        int UIServer.Iface.UpdateRealtimeCap(RealtimeCapInfo info, string channelname)
        {

            return 0;
        }

        int UIServer.Iface.UpdateRealtimeCmp(RealtimeCmpInfo info, string channelname)
        {
            return 0;
        }
    }
}
