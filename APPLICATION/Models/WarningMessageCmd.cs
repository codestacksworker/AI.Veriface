using xiaowen.codestacks.data;
using DATA.MODELS.GlobalModels;
using DATA.MODELS.SensingModels;
using DATA.UTILITIES.Log4Net;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using xiaowen.codestacks.popwindow;
using FaceSysByMvvm.Model;
using SENSING.THRIFT.Services;
using FaceSysByMvvm.ViewModels.ChannelManage;

namespace SINGLEUSER.Models
{
    public class WarningMessageCmd
    {
        public static ICommand ClearAllBtnCommand { get; private set; }
        public static ICommand BatchSendBtnCommand { get; private set; }
        public static ICommand ClosedWindowCommand { get; private set; }
        public static ICommand AutoSendBtnCommand { get; private set; }

        public static Action InitCmd()
        {
            Action act = () =>
            {
                ClearAllBtnCommand = new DelegateCommand<object>(ClearAllBtnCommandFunc);
                BatchSendBtnCommand = new DelegateCommand<object>(BatchSendBtnCommandFunc);
                ClosedWindowCommand = new DelegateCommand<object>(ClosedWindowCommandFunc);
                AutoSendBtnCommand = new DelegateCommand<object>(AutoSendBtnCommandFunc);
            };
            return act;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private static void ClosedWindowCommandFunc(object obj)
        {
            Window warningWindow = obj as Window;
            warningWindow.Close();
        }

        /// <summary>
        /// Send Batch
        /// </summary>
        /// <param name="obj"></param>
        private static void BatchSendBtnCommandFunc(object obj)
        {
            try
            {
                int res = SendInfo();
            }
            catch (Exception)
            {
                CodeStacksWindow.MessageBox.Invoke(false, false, 1, "【error】软件运行出现异常，请联系技术人员！");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int SendInfo()
        {
            int res = -1;
            ThriftServiceNameSpace.ThriftService thirft = new ThriftServiceNameSpace.ThriftService();
            try
            {
                if (ViewDataModel.WarningData.Property.CurCompareLogDatas.Count == 0) return res;

                int i = 0;
                do
                {
                    i = ViewDataModel.WarningData.Property.CurCompareLogDatas.Count - 1;
                    var cmpface = ViewDataModel.WarningData.Property.CurCompareLogDatas.Cast<MyCmpFaceLogWidthImgModel>().ToList();

                    //过滤
                    if (GlobalCache.AreaInfoCollection.Cast<ConfigRegion>().SingleOrDefault(x => cmpface[i].channelName.StartsWith(x.RegionName)) != null)
                    {
                        res = SendOneResultInfo(cmpface[i]);
                        if (res == 0)
                        {
                            string err = string.Empty;
                            Function.ClearPushedWaringData.ClearPushedData(ref err, res, cmpface[i]);
                        }
                        else
                        {
                            i = -1;
                        }
                    }
                    else
                    {
                        CodeStacksWindow.MessageBox.Invoke(false, false, 1, "请取消选中不可推送的告警");
                        i = -1;
                    }

                } while (i > 0);
            }
            catch (Exception ex)
            {
                CodeStacksWindow.MessageBox.Invoke(false, false, 2, ex.Message);
            }
            return res;
        }
        /// <summary>
        /// Cleanning all
        /// </summary>
        /// <param name="obj"></param>
        private static void ClearAllBtnCommandFunc(object obj)
        {
            Function.ClearPushedWaringData.ClearAll();
        }

        private static void AutoSendBtnCommandFunc(object obj)
        {
            try
            {
                int res = SendInfo();
            }
            catch (Exception)
            {
                CodeStacksWindow.MessageBox.Invoke(false, false, 2, "【error】软件运行出现异常，请联系技术人员！");
            }
        }


        /// <summary>
        /// 自动推送 推送全部 代码暂代完善
        /// </summary>
        /// <returns></returns>
        public static int AutoSendInfo()
        {
            int res = -1;
            ThriftServiceNameSpace.ThriftService thirft = new ThriftServiceNameSpace.ThriftService();
            try
            {
                if (ViewDataModel.WarningData.Property.CompareLogDatas.Count == 0) return res;

                int i = 0;
                do
                {
                    i = ViewDataModel.WarningData.Property.CompareLogDatas.Count - 1;
                    var cmpface = ViewDataModel.WarningData.Property.CompareLogDatas.Cast<MyCmpFaceLogWidthImgModel>().ToList();

                    //过滤
                    if (GlobalCache.AreaInfoCollection.Cast<ConfigRegion>().SingleOrDefault(x => cmpface[i].channelName.StartsWith(x.RegionName)) != null)
                    {
                        res = SendOneResultInfo(cmpface[i]);
                        if (res == 0)
                        {
                            string err = string.Empty;
                            Function.ClearPushedWaringData.ClearPushedData(ref err, res, cmpface[i]);
                        }
                        else
                        {
                            i = -1;
                        }
                    }
                    else
                    {
                        CodeStacksWindow.MessageBox.Invoke(false, false, 1, "请取消选中不可推送的告警");
                        i = -1;
                    }

                } while (i > 0);
            }
            catch (Exception ex)
            {
                CodeStacksWindow.MessageBox.Invoke(false, false, 2, ex.Message);
            }
            return res;
        }

        /// <summary>
        /// 推送一条
        /// </summary>
        /// <param name="signleObj"></param>
        public static int SendOneResultInfo(MyCmpFaceLogWidthImgModel signleObj)
        {
            int res = -1;
            ThriftServiceNameSpace.ThriftService thirft = new ThriftServiceNameSpace.ThriftService();
            try
            {
                RealtimeCmpInfoLBS newRci = new RealtimeCmpInfoLBS();
                newRci.CapID = signleObj.ID;
                newRci.ObjID = signleObj.tcUuid;
                newRci.Name = signleObj.T1.UserName;
                newRci.Channel = signleObj.channel;
                newRci.Channelname = signleObj.channelName;
                newRci.Longitude = signleObj.Longitude;
                newRci.Latitude = signleObj.Latitude;
                newRci.Address = signleObj.Address;
                newRci.CapImg = signleObj.SnapImageBuffer;
                newRci.ObjImg = signleObj.T1.TemplateImageBuffer;
                newRci.Time =
                    CodeStacksDataHandler.DateTimeData.ConvertToLongBySubstractDelegate.Invoke(Convert.ToDateTime(signleObj.time), new DateTime(1970, 1, 1));
                newRci.Type = signleObj.TypeKey;
                newRci.Score = signleObj.score == 0 ? signleObj.Score : signleObj.score;
                res = SendCmpToClient(newRci);
            }
            catch (Exception ex)
            {
                Logger<WarningMessageCmd>.Log.Error("【Error】", ex);
            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        private static int SendCmpToClient(RealtimeCmpInfoLBS info)
        {
            int res = -1;
            ThriftServiceNameSpace.ThriftService thirft = new ThriftServiceNameSpace.ThriftService();
            ThriftServiceUtilities thrift = new ThriftServiceUtilities();
            try
            {
                if ("WIFI".Equals(GlobalCache.NetworkMode))
                {
                    thrift.UpdateRealtimeCmpToDbFlag(new List<string> { info.CapID }, GlobalCache.AppRegion);
                }
                else
                {
                    var regionCollection = GlobalCache.AreaInfoCollection.Where(x => info.Channelname.Contains(x.RegionName));
                    foreach (var area in regionCollection)
                    {
                        //推送的区域Number
                        foreach (var ip in area.Hosts)
                        {
                            if (UpdateCmp(info, ip) != 0)
                            {
                                CodeStacksWindow.MessageBox.Invoke(false, false, 1, "往 " + ip + " 推送失败!");
                                res = -1;
                            }
                            else
                                res = thirft.UpdateCmpLog(
                                    info.CapID, info.ObjID, System.DateTime.Now.ToString("yyyyMMdd"),
                                    GlobalCache.AppRegion);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger<WarningMessageCmd>.Log.Error("SendCmpToClient", ex);
            }
            return res;
        }

        /// <summary>
        /// 更新比对记录标识
        /// </summary>
        /// <param name="info"></param>
        /// <param name="IP"></param>
        /// <returns></returns>
        public static int UpdateCmp(RealtimeCmpInfoLBS info, string IP)
        {
            try
            {
                // 生成socket套接字；
                Thrift.Transport.TSocket tsocket = new Thrift.Transport.TSocket(IP, 6000);
                //设置连接超时为100；
                tsocket.Timeout = 3000;
                //生成客户端对象
                Thrift.Transport.TTransport transport = tsocket;
                Thrift.Protocol.TProtocol protocol = new Thrift.Protocol.TBinaryProtocol(transport);
                UIServer.Client _BusinessServerClient = new UIServer.Client(protocol);
                transport.Open();
                //_BusinessServerClient.UpdateRealtimeCmp(info, "##@" + info.Channel);
                info.Channelname = "##@" + info.Channelname;
                _BusinessServerClient.UpdateRealtimeCmpLBS(info);
                transport.Close();
                return 0;
            }
            catch (Exception ex)
            {
                if ("DEBUG".Equals(GlobalCache.AppMode))
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, ex.Message);
                }
                Logger<WarningMessageCmd>.Log.Error("UpdateCmp", ex);
                return -1;
            }
            finally
            {

            }
        }
    }
}
