using FaceSysByMvvm.Views.ChannelManager;
using System;
using System.Collections.Generic;
using System.Text;
using DATA.UTILITIES.Log4Net;
using DATA.MODELS.GlobalModels;
using DATA.UTILITIES.SensingFunc;
using System.Linq;
using xiaowen.codestacks.popwindow;

namespace SENSING.ClassPool
{
    class UIServerInter : UIServer.Iface
    {
        public MyCapFaceLogWithImg _MyCapFaceLogWithImg = null;
        public PublishResult _IdentifyResults = null;
        List<string> listQueryDefFaceObjType = new List<string>();
        string strOldCapID = string.Empty;
        int oldScore = 0;
        //ThriftServiceNameSpace.ThriftService thirft = new ThriftServiceNameSpace.ThriftService();

        public UIServerInter() { }

        #region Old Interface
        public int UpdateRealtimeCap(RealtimeCapInfo info, string channelName)
        {
            try
            {
                if (GlobalCache.AppType == 1)
                {
                    if (!channelName.Contains(GlobalCache.AppLocation))
                    {
                        return -1;
                    }
                }
                //接收服务器附送过来的实时照片
                if (_MyCapFaceLogWithImg != null)
                    _MyCapFaceLogWithImg = null;
                _MyCapFaceLogWithImg = new MyCapFaceLogWithImg();
                _MyCapFaceLogWithImg.ID = info.Id;// 抓拍id
                _MyCapFaceLogWithImg.ChannelID = info.Channel;// 通道id
                _MyCapFaceLogWithImg.ChannelName = channelName;
                long _longtime = info.Time;
                DateTime s = new DateTime(1970, 1, 1);
                s = s.AddSeconds(_longtime);
                _MyCapFaceLogWithImg.time = s.ToString("yyyy/MM/dd HH:mm:ss");

                //ChannelManage.CapimageByteRealtimeCapInfo = info.Image;
                ChannelManage.snapStream = info.Image;
                ChannelManage._MyCapFaceLogWithImg = null;
                ChannelManage._MyCapFaceLogWithImg = _MyCapFaceLogWithImg;
                ChannelManage.ResetServerRealtimeCapInfo.Set();
                info = null;

                return 0;
            }
            catch (Exception ex)
            {
                Logger<OperaExcel>.Log.Error("UpdateRealtimeCap", ex);
                return -1;
            }
        }

        public int UpdateRealtimeCmp(RealtimeCmpInfo info, string channelName)
        {
            try
            {
                if (GlobalCache.AppType == 1)
                {
                    if (!channelName.StartsWith("##"))
                    {
                        return -1;
                    }
                }
                //显示在界面上的结果
                if (_IdentifyResults != null)
                    _IdentifyResults = null;
                _IdentifyResults = new PublishResult();
                _IdentifyResults.ID = info.CapID;
                if (info.CapID == null || info.CapID == "")
                {
                    CodeStacksWindow.MessageBox.Invoke(true, false, 2, "抓拍照片ID为空");
                }
                _IdentifyResults.RegID = info.ObjID;
                if (info.ObjID == null || info.ObjID == "")
                {
                    CodeStacksWindow.MessageBox.Invoke(true, false, 2, "抓拍照片ID为空");
                }
                long _longtime = info.Time;
                DateTime s = new DateTime(1970, 1, 1);
                s = s.AddSeconds(_longtime);
                // 抓拍照片 
                ChannelManage.snapStream = info.CapImg;
                //得到主照片
                ChannelManage.campareStream = info.ObjImg;
                //获得主照片这侧信息
                StringBuilder strRegster = new StringBuilder();
                //注册名称
                strRegster.Append(info.Name + "\r\n");
                //获得通道名称
                string ChannelName = channelName;
                _IdentifyResults.ChannelName = channelName;
                strRegster.Append(ChannelName.Replace("##", "").Replace("@", "") + "\r\n");
                //抓拍时间
                int nIndexS = s.ToString().IndexOf(" ");
                strRegster.Append(s.ToString().Substring(0, nIndexS) + "\r\n");
                int nIndexS1 = s.ToString().Length - nIndexS;
                strRegster.Append(s.ToString().Substring(nIndexS + 1, nIndexS1 - 1) + "\r\n");
                //注册类型
                string type = BasicDataEntry.GetTemplateType(info.Type);

                strRegster.Append(type + "\r\n");
                _IdentifyResults.TemplateType = type;
                //相似度。
                strRegster.Append(info.Score + "\r\n");
                _IdentifyResults.RegInfo = strRegster.ToString();
                _IdentifyResults.Info = info;
                //MainWindow.nInfoType = info.Type;
                Console.WriteLine("推送了一条比对记录");

                ChannelManage._IdentifyResults = _IdentifyResults;
                ChannelManage.ResetServerRealtimeCmpInfo.Set();

                info = null;
                return 0;
            }
            catch (Exception ex)
            {
                Logger<OperaExcel>.Log.Error("UpdateRealtimeCmp", ex);
                return -1;
            }
        }
        #endregion

        public int UpdateRealtimeCapLBS(RealtimeCapInfoLBS info)
        {
            try
            {
                if (GlobalCache.AppType == 1)
                {
                    var area =
                        GlobalCache.ChannelList.Cast<MyChannelCfg>().FirstOrDefault(n => info.Channelname.StartsWith(n.Name));
                    if (area == null)
                    {
                        return -1;
                    }
                }
                _MyCapFaceLogWithImg = new MyCapFaceLogWithImg();
                //接收服务器附送过来的实时照片
                _MyCapFaceLogWithImg.ID = info.Id;//snap id/uuid -guid
                _MyCapFaceLogWithImg.ChannelID = info.Channel;//channel id/uuid - guid
                _MyCapFaceLogWithImg.ChannelName = info.Channelname;//channel name
                _MyCapFaceLogWithImg.Longitude = info.Longitude;//channel longitude for coordinate
                _MyCapFaceLogWithImg.Latitude = info.Latitude;//
                _MyCapFaceLogWithImg.Address = string.IsNullOrEmpty(info.Address) ? info.Channelname : info.Address;
                _MyCapFaceLogWithImg.Score = info.Score;//degree of similarity 
                _MyCapFaceLogWithImg.Address = info.Address;//channel location

                DateTime datetime = new DateTime(1970, 1, 1).AddSeconds(info.Time);
                _MyCapFaceLogWithImg.time = datetime.ToString("yyyy/MM/dd HH:mm:ss");

                ChannelManage.snapStream = info.Image;
                ChannelManage._MyCapFaceLogWithImg = null;
                ChannelManage._MyCapFaceLogWithImg = _MyCapFaceLogWithImg;
                ChannelManage.ResetServerRealtimeCapInfo.Set();

                return 0;
            }
            catch (Exception ex)
            {
                Logger<OperaExcel>.Log.Error("UpdateRealtimeCapLBS", ex);
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
                if (GlobalCache.AppType == 1)
                {
                    if (!info.Channelname.StartsWith("##"))
                    {
                        return -1;
                    }
                    else
                    {
                        var area =
                     GlobalCache.ChannelList.Cast<MyChannelCfg>().FirstOrDefault(n => info.Channelname.Replace("##@", "").StartsWith(n.Name));
                        if (area == null)
                        {
                            return -1;
                        }
                    }
                }
                if ("PK".Equals(GlobalCache.AppFunction))
                {
                    if (strOldCapID == string.Empty)
                    {
                        strOldCapID = info.CapID;
                        oldScore = info.Score;
                        GetIdentifyResults(info);
                    }
                    else if (strOldCapID == info.CapID)
                    {
                        if (info.Score > oldScore)
                        {
                            strOldCapID = info.CapID;
                            oldScore = info.Score;
                            GetIdentifyResults(info);
                        }
                    }
                    else
                    {
                        strOldCapID = info.CapID;
                        oldScore = info.Score;
                        GetIdentifyResults(info);
                    }
                }
                else
                {
                    GetIdentifyResults(info);
                }
                return 0;
            }
            catch (Exception ex)
            {
                Logger<UIServerInter>.Log.Error("UpdateRealtimeCmpLBS", ex);
                return -1;
            }
        }

        public int UpdateRealtimeCmpEntertainment(CmpFaceLogDS cfg)
        {
            return 1;
        }

        private void GetIdentifyResults(RealtimeCmpInfoLBS info)
        {

            _IdentifyResults = new PublishResult();
            //显示在界面上的结果
            _IdentifyResults.ID = info.CapID;
            _IdentifyResults.RegID = info.ObjID;
            ChannelManage.snapStream = info.CapImg;//snap image               
            ChannelManage.campareStream = info.ObjImg;//template main image


            _IdentifyResults.ChannelName = info.Channelname.Replace("##", "").Replace("@", "");//channel name

            DateTime datetime = new DateTime(1970, 1, 1).AddSeconds(info.Time);

            //获得主照片这侧信息
            StringBuilder sb = new StringBuilder();
            sb.Append(info.Name + "\r\n");
            sb.Append(string.IsNullOrEmpty(_IdentifyResults.ChannelName) ? info.Address : _IdentifyResults.ChannelName + "\r\n");
            sb.Append(datetime.ToShortDateString() + "\r\n" + datetime.ToShortTimeString() + "\r\n");
            string type = BasicDataEntry.GetTemplateType(info.Type); ;//eg：白名单、黑名单、普通...
            sb.Append(type + "\r\n");
            _IdentifyResults.TemplateType = type;
            //相似度。
            sb.Append(info.Score + "\r\n");

            _IdentifyResults.RegInfo = sb.ToString();
            _IdentifyResults.NewCmpResult = info;

            Console.WriteLine("推送了一条比对记录");
            ChannelManage._IdentifyResults = _IdentifyResults;
            ChannelManage.ResetServerRealtimeCmpInfo.Set();

        }
    }
}
