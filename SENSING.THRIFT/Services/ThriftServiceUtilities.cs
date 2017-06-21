using DATA.MODELS.GlobalModels;
using DATA.UTILITIES.Log4Net;
using PeopleModel;
using SENSING.ClassPool;
using SENSING.THRIFT.CommonServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Thrift.Transport;
using xiaowen.codestacks.data;
using xiaowen.codestacks.popwindow;
using System.Linq;

namespace SENSING.THRIFT.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ThriftServiceUtilities
    {
        UIServerInterForWifi wifi = new UIServerInterForWifi();

        /// <summary>
        /// init
        /// </summary>
        public ThriftServiceUtilities(ManualResetEvent snap, ManualResetEvent snapCompare, PublishResult result, byte[] snapStream, byte[] compareStream)
        {
            if ("WIFI".Equals(GlobalCache.NetworkMode))
            {
                wifi.Get(snap, snapCompare, result, snapStream, compareStream);
            }
        }

        public ThriftServiceUtilities() { }

        /// <summary>
        /// public funcation
        /// </summary>
        /// <param name="snapDueTime"></param>
        /// <param name="compareDueTime"></param>
        /// <param name="publishFlag"></param>
        public void SelectSnapAndSnapCompareResultWithPublicFunc(int snapDueTime, int compareDueTime, int publishFlag)
        {
            this.ASelectSnapDataFromDb(snapDueTime);
            this.ASelectSnapCompareDataFromDb(compareDueTime, publishFlag);
        }
        
        async void ASelectSnapDataFromDb(int snapDueTime)
        {
            await Task.Run(() =>
            {
                foreach (var item in SelectSnapDataFromDb(snapDueTime))
                {
                    wifi.UpdateRealtimeCapLBS(item);
                }
            }).ConfigureAwait(false);
        }
        
        public async void ASelectSnapCompareDataFromDb(int compareDueTime, int publishFlag)
        {
            await Task.Run(() =>
            {
                foreach (var item in SelectSnapCompareDataFromDb(compareDueTime, publishFlag))
                {
                    wifi.UpdateRealtimeCmpLBS(item);
                }
            }).ConfigureAwait(false);
        }
        
        /// <summary>
        /// Snap
        /// </summary>
        /// <param name="duetime"></param>
        List<RealtimeCapInfoLBS> SelectSnapDataFromDb(int duetime)
        {
            //duetime
            List<RealtimeCapInfoLBS> result = new List<RealtimeCapInfoLBS>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftServiceUtilities, List<RealtimeCapInfoLBS>, int>(
                       transport,
                       bServerClient.QueryRealtimeCapInfoQLBS,
                       "SelectSnapDataFromDb", false,
                       duetime);
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// Snap Compare
        /// </summary>
        /// <param name="duetime"></param>
        /// <param name="publishFlag">publish flag bit</param>
        List<RealtimeCmpInfoLBS> SelectSnapCompareDataFromDb(int duetime, int publishFlag)
        {
            List<RealtimeCmpInfoLBS> result = new List<RealtimeCmpInfoLBS>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftServiceUtilities, List<RealtimeCmpInfoLBS>, int, int>(
                     transport,
                     bServerClient.QueryRealtimeCmpInfoQLBS,
                     "SelectSnapCompareDataFromDb", false,
                     duetime, publishFlag
                     );
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capid"></param>
        /// <param name="qflag"></param>
        /// <returns>0-成功   1-失败</returns>
        public int UpdateRealtimeCmpToDbFlag(List<string> capid, int qflag)
        {
            int result = 404;
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftServiceUtilities, int, List<string>, int>(
                     transport,
                     bServerClient.UpdateRealtimeCmpQ,
                     "UpdateRealtimeCmpToDbFlag", true,
                     capid,
                     qflag
                     );
            }
            catch (Exception)
            {
            }
            finally
            {
                if (result != 0) CodeStacksWindow.MessageBox.Invoke(false, false, 2, "推送失败");
            }
            return result;
        }

        #region 20170505 new request
        /// <summary>
        /// Update Config Content
        /// </summary>
        /// <param name="server">20170508 暂时可空</param>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task<List<ErrorInfo>> ConfigBySet(List<SServerInfo> server, List<SConfigInfo> config)
        {
            List<ErrorInfo> result = new List<ErrorInfo>();
            try
            {
                return await Task<List<ErrorInfo>>.Run(() =>
                 {
                     BusinessServer.Client bServerClient = null;
                     TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                     result = SocketOpter.GetResult<ThriftServiceUtilities, List<ErrorInfo>, List<SServerInfo>, List<SConfigInfo>>(
                          transport,
                          bServerClient.SetConfig,
                          "ConfigBySet", true,
                          server,
                          config
                          );

                     return result;
                 });
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// Get Config Content
        /// </summary>
        /// <param name="stype"></param>
        /// <returns></returns>
        public List<SConfigInfo> ConfigByGet(int stype)
        {
            List<SConfigInfo> result = new List<SConfigInfo>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftServiceUtilities, List<SConfigInfo>, int>(
                     transport,
                     bServerClient.GetConfig,
                     "ConfigByGet", true,
                     stype
                     );
            }
            catch (Exception)
            {
            }
            return result;
        }

        #endregion
        
        #region 20170522 增加重点标记

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid">uuid</param>
        /// <param name="flagValue">-1普通 1重点</param>
        /// <returns></returns>
        public int MarkUpKeyObject(string guid, int flagValue)
        {
            int result = -1;
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftServiceUtilities, int, string, int>(
                    transport,
                    bServerClient.ChangeTemplateImark,
                    "MarkUpKeyObject",
                    false,
                    guid,
                    flagValue
                    );
            }
            catch (Exception ex)
            {
                Logger<ThriftServiceUtilities>.Log.Error("MarkUpKeyObjec", ex);
            }
            return result;
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="capid"></param>
        /// <param name="capimg"></param>
        /// <param name="threshold"></param>
        /// <param name="btime"></param>
        /// <param name="etime"></param>
        /// <returns></returns>
        public ObservableCollection<xiaowen.codestacks.data.SenSingModels.Camera> GetTargetAnalysisResult(string capid, byte[] capimg, int threshold, long btime, long etime)
        {
            BusinessServer.Client bServerClient = null;
            TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
            List<TargetedAnalysis> result = SocketOpter.GetResult<ThriftServiceUtilities, List<TargetedAnalysis>, string, byte[], int, long, long>(
                transport,
                bServerClient.QueryTargetedAnalysis,
                "GetTargetAnalysisResult", true,
                capid, capimg, threshold, btime, etime
                );

            ObservableCollection<xiaowen.codestacks.data.SenSingModels.Camera> SnapPersonItems = new ObservableCollection<xiaowen.codestacks.data.SenSingModels.Camera>();

            try
            {
                IEnumerable<TargetedAnalysis> sortResult = from r in result
                                                           orderby r.Count descending
                                                           select r;

                foreach (var item in sortResult)
                {
                    SnapPersonItems.Add(new xiaowen.codestacks.data.SenSingModels.Camera
                    {
                        Name = item.Name,
                        Location = item.Channel_address,
                        Longitude = Convert.ToDouble(item.Longitude),
                        Latitude = Convert.ToDouble(item.Latitude),
                        SnapPeopleCount = item.Count
                    });
                }
            }
            catch (Exception ex)
            {
                Logger<ThriftServiceUtilities>.Log.Error("GetTargetAnalysisResult", ex);
            }
            return SnapPersonItems;
        }

        public ObservableCollection<TmpSnapInfo> GetNoTargetAnalysisResult(ref CameraSnapPerson suspectObj, long btime, long etime)
        {
            BusinessServer.Client bServerClient = null;
            TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
            SuspectAnalysisInfo results = SocketOpter.GetResult<ThriftServiceUtilities, SuspectAnalysisInfo, long, long>(
                transport,
                bServerClient.QueryKeyAreaSuspectAnalysis,
                "GetNoTargetAnalysisResult[QueryKeyAreaSuspectAnalysis]", true,
                btime, etime
                );

            ObservableCollection<TmpSnapInfo> result = new ObservableCollection<TmpSnapInfo>();
            try
            {
                suspectObj = new CameraSnapPerson();
                suspectObj.Photo = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(results.Capcfg.Image);
                suspectObj.PhotoByteArray = results.Capcfg.Image;
                suspectObj.CameraInfo = new Camera();
                suspectObj.CameraInfo.Id = results.Capcfg.Id;
                suspectObj.CameraInfo.Name = results.Capcfg.Channelname;
                suspectObj.CameraInfo.Location = results.Capcfg.Address;
                if (results.Capcfg.Time > 0)
                {
                    string tmp = CodeStacksDataHandler.DateTimeData.ConvertToStringDelegate(results.Capcfg.Time);
                    suspectObj.SnapDate = Convert.ToDateTime(tmp).ToShortDateString();
                    suspectObj.SnapTime = Convert.ToDateTime(tmp).ToShortTimeString();
                }
                else
                {
                    suspectObj.SnapDate = string.Empty;
                    suspectObj.SnapTime = string.Empty;
                }

                foreach (SuspectAnalysis item in results.Sulist)
                {
                    TmpSnapInfo ts = new TmpSnapInfo();
                    ts.CameraChannelId = item.TcChannelID;
                    ts.CameraChannel = item.Name;
                    ts.Longitude = item.Longitude;
                    ts.Latitude = item.Latitude;
                    ts.Location = item.Channel_address;

                    #region TopFive
                    for (int i = 0; i < item.Capimg.Count; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                ts.CameraTopOne = new Camera
                                {
                                    Name = item.Name,
                                    SnapPersonCount = item.Capimg[i].Count,
                                    SnapPersonCountStr = item.Capimg[i].Count + " 次",
                                    SnapPhoto = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(item.Capimg[i].Capimg),
                                    Location = item.Channel_address,
                                };
                                break;
                            case 1:
                                ts.CameraTopTwo = new Camera
                                {
                                    Name = item.Name,
                                    SnapPersonCount = item.Capimg[i].Count,
                                    SnapPersonCountStr = item.Capimg[i].Count + " 次",
                                    SnapPhoto = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(item.Capimg[i].Capimg),
                                    Location = item.Channel_address,
                                };
                                break;
                            case 2:
                                ts.CameraTopThree = new Camera
                                {
                                    Name = item.Name,
                                    SnapPersonCount = item.Capimg[i].Count,
                                    SnapPersonCountStr = item.Capimg[i].Count + " 次",
                                    SnapPhoto = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(item.Capimg[i].Capimg),
                                    Location = item.Channel_address,
                                };
                                break;
                            case 3:
                                ts.CameraTopFour = new Camera
                                {
                                    Name = item.Name,
                                    SnapPersonCount = item.Capimg[i].Count,
                                    SnapPersonCountStr = item.Capimg[i].Count + " 次",
                                    SnapPhoto = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(item.Capimg[i].Capimg),
                                    Location = item.Channel_address,
                                };
                                break;
                            case 4:
                                ts.CameraTopFive = new Camera
                                {
                                    Name = item.Name,
                                    SnapPersonCount = item.Capimg[i].Count,
                                    SnapPersonCountStr = item.Capimg[i].Count + " 次",
                                    SnapPhoto = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(item.Capimg[i].Capimg),
                                    Location = item.Channel_address,
                                };
                                break;
                        }
                    }
                    #endregion

                    result.Add(ts);
                }
            }
            catch (Exception ex)
            {
                Logger<ThriftServiceUtilities>.Log.Error("GetNoTargetAnalysisResult", ex);
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="btiem"></param>
        /// <param name="etime"></param>
        public List<TargetedAnalysis> GetNoTargetAnalysisResultForMap(long btime, long etime)
        {
            BusinessServer.Client bServerClient = null;
            TTransport transport2 = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
            return SocketOpter.GetResult<ThriftServiceUtilities, List<TargetedAnalysis>, long, long>(
                transport2,
                bServerClient.QueryCrowdedAnalysis,
                "GetNoTargetAnalysisResult[QueryCrowdedAnalysis]", true,
                btime, etime
                );
        }



        #region static analysis
        /// <summary>
        /// Get analysis result
        /// </summary>
        /// <param name="capId"></param>
        /// <param name="capImage"></param>
        /// <param name="btime"></param>
        /// <param name="etime"></param>
        /// <param name="threshold"></param>
        /// <param name="maxcount"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public IList<CameraSnapPerson> SC_GetAnalysisResult(string capId, byte[] capImage, long btime, long etime, int threshold, int maxcount, string template)
        {
            BusinessServer.Client bServerClient = null;
            TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
            List<RealtimeCapInfoLBS> caplist = SocketOpter.GetResult<ThriftServiceUtilities, List<RealtimeCapInfoLBS>, string, byte[], long, long, int, int>(
                transport,
                bServerClient.QueryStaticAnalysis,
                "SC_GetAnalysisResult", true,
                capId, capImage, btime, etime, threshold, maxcount
                );

            IList<CameraSnapPerson> result = new List<CameraSnapPerson>();
            try
            {
                #region Data Convert

                int row = 0;
                foreach (var item in caplist)
                {
                    DateTime time = new DateTime(1970, 1, 1);
                    time = time.AddSeconds(item.Time);
                    result.Add(new CameraSnapPerson
                    {

                        DataIndex = ++row,
                        NameTitle = "通 道：",
                        PhotoByteArray = item.Image,
                        Photo = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(item.Image),
                        Name = item.Channelname,
                        Datetime = time.ToString(),
                        SnapDate = time.ToShortDateString(),
                        SnapTime = time.ToShortTimeString(),
                        Source = template,
                        Score = item.Score,
                        CameraInfo = new Camera
                        {
                            Id = item.Id,
                            Name = item.Channelname,
                            Longitude = item.Longitude,
                            Latitude = item.Latitude,
                            Location = item.Address
                        }
                    });
                }

                #endregion
            }
            catch (Exception ex)
            {
                Logger<ThriftServiceUtilities>.Log.Error("SC_GetAnalysisResult", ex);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> SC_GetAnalysisStoreFromSanpStore()
        {
            BusinessServer.Client bServerClient = null;
            TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
            return SocketOpter.GetResult<ThriftServiceUtilities, List<string>>(
                 transport,
                 bServerClient.QueryDefFaceObjType,
                 "SC_GetAnalysisStoreFromSanpStore", true
                 );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="nThreshold"></param>
        /// <param name="nMaxCount"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public IList<CameraSnapPerson> SC_GetAnalysisResultFromTemplateStore(byte[] image, int nThreshold, int nMaxCount, string template)
        {
            BusinessServer.Client bServerClient = null;
            TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
            List<FaceObj> _ListFaceObj = SocketOpter.GetResult<ThriftServiceUtilities, List<FaceObj>, byte[], int, int>(
                transport,
                bServerClient.QueryFaceObjByImg,
                "SC_GetAnalysisResultFromTemplateStore", true,
                image, nThreshold, nMaxCount
                );

            List<CameraSnapPerson> result = new List<CameraSnapPerson>();
            try
            {
                int row = 0;
                foreach (var item in _ListFaceObj)
                {
                    //这个项目已19700101作为起点，计算时间
                    DateTime time = new DateTime(1970, 1, 1).AddSeconds(item.DTm);


                    result.Add(new CameraSnapPerson
                    {
                        SnapId = item.TcUuid,
                        DataIndex = ++row,
                        NameTitle = "姓 名：",
                        Name = item.TcName,
                        Datetime = time.ToString(),
                        SnapDate = time.ToShortDateString(),
                        SnapTime = time.ToShortTimeString(),
                        Score = item.NExten,
                        PhotoByteArray = item.Tmplate[0].Img,
                        Photo = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(item.Tmplate[0].Img) // item.Tmplate[0].Img
                        ,
                        Source = template,
                        SourcePhotoByteArray = image,
                        Main_ftID = item.NMain_ftID,
                        Type = item.NType,
                        SST = item.NSST,
                        Exten = item.NExten,
                        Sex = item.NSex,
                        Age = item.NAge,
                        Tm = item.DTm,
                        Remarks = item.TcRemarks
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            return result;
        }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="capid"></param>
        /// <param name="capimg"></param>
        /// <param name="btime"></param>
        /// <param name="etime"></param>
        /// <returns></returns>
        public List<TrackInfo> QueryTrackPlayback(string capid, byte[] capimg, long btime, long etime)
        {
            List<TrackInfo> result = null;
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftServiceUtilities, List<TrackInfo>, string, byte[], long, long>(transport, bServerClient.QueryTrackPlayback, "QueryTrackPlayback", true, capid, capimg, btime, etime);
            }
            catch (Exception ex)
            {
                Logger<ThriftServiceUtilities>.Log.Error("QueryTrackPlayback", ex);
            }
            return result;
        }
    }
}
