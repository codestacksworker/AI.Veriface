using SENSING.ClassPool;
using System;
using System.Collections.Generic;
using Thrift.Transport;
using static FaceSysByMvvm.ViewModels.CompOfRecords.CompOfRecordsViewModel;
using static FaceSysByMvvm.ViewModels.CaptureRecordQuery.CaptureRecordQueryViewModel;
using FaceSysByMvvm.ViewModels.TemplateManager;
using System.Windows.Threading;
using FaceSysByMvvm.Model;
using DATA.UTILITIES.Log4Net;
using xiaowen.codestacks.data;
using DATA.MODELS.GlobalModels;
using System.Linq;
using DATA.UTILITIES.SensingFunc;
using SENSING.THRIFT.CommonServices;
using SENSING.THRIFT.Services;

namespace ThriftServiceNameSpace
{
    public class ThriftService : IThirtfService
    {
        /// <summary>
        /// 心跳
        /// </summary>
        /// <returns></returns>
        public int HearBeat()
        {
            BusinessServer.Client bServerClient = null;
            TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 1000, ref bServerClient);
            return SocketOpter.GetResult<ThriftService, int>(transport, bServerClient.HearBeat, "HearBeat", false);
        }

        /// <summary>
        /// 获取模版类型
        /// </summary>
        /// <returns></returns>
        public List<string> QueryDefFaceObjType()
        {
            List<string> result = new List<string>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftService, List<string>>(
                    transport, bServerClient.QueryDefFaceObjType, "QueryDefFaceObjType获取模板类型", true
                    );
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 获取模版数量
        /// </summary>
        /// <param name="channel">频道</param>
        /// <param name="name">模板名称</param>
        /// <param name="type">模板类型</param>
        /// <param name="gender">性别</param>
        /// <param name="bage">起始年龄</param>
        /// <param name="eage">终止年龄</param>
        /// <param name="btime">起始时间</param>
        /// <param name="etime">终止时间</param>
        /// <returns></returns>
        public int QueryCmpRecordTotalCount(CompOfRecordsValue compOfRecordsValue)
        {
            int dataCount = 0;
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                dataCount = SocketOpter.GetResult<ThriftService, int, string, string, int, int, int, int, long, long>(
                    transport,
                    bServerClient.QueryCmpRecordTotalCount,
                    "QueryCmpRecordTotalCount", true,
                    compOfRecordsValue.ChannelValue,
                    compOfRecordsValue.NameValue, compOfRecordsValue.TypeValue, compOfRecordsValue.SexValue, compOfRecordsValue.MinAgeValue, compOfRecordsValue.MaxAgeValue, compOfRecordsValue.StartDayValue, compOfRecordsValue.EndDayValue
                    );
            }
            catch (Exception)
            {
            }
            return dataCount;
        }

        /// <summary>
        /// 查询比对记录数量(分表)
        /// </summary>
        /// <param name="compOfRecordsValue"></param>
        /// <returns></returns>
        public List<SCountInfo> QueryCmpRecordTotalCountH(CompOfRecordsValue compOfRecordsValue)
        {
            List<SCountInfo> result = new List<SCountInfo>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                return SocketOpter.GetResult<ThriftService, List<SCountInfo>, string, string, int, int, int, int, long, long>(
                      transport,
                      bServerClient.QueryCmpRecordTotalCountHDS,
                      "QueryCmpRecordTotalCountH", true,
                      compOfRecordsValue.ChannelValue,
                      compOfRecordsValue.NameValue,
                      compOfRecordsValue.TypeValue,
                      compOfRecordsValue.SexValue,
                      compOfRecordsValue.MinAgeValue,
                      compOfRecordsValue.MaxAgeValue,
                      compOfRecordsValue.StartDayValue,
                      compOfRecordsValue.EndDayValue
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
        /// <param name="compOfRecordsValue"></param>
        /// <param name="pflag"></param>
        /// <returns></returns>
        public List<SCountInfo> QueryCmpRecordTotalCountHSX(CompOfRecordsValue compOfRecordsValue, int pflag)
        {
            List<SCountInfo> result = new List<SCountInfo>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftService, List<SCountInfo>, string, string, int, int, int, int, long, long, int>(
                    transport,
                    bServerClient.QueryCmpRecordTotalCountHSXDS,
                     "QueryCmpRecordTotalCountHSX", true,
                    compOfRecordsValue.ChannelValue, compOfRecordsValue.NameValue,
                       compOfRecordsValue.TypeValue, compOfRecordsValue.SexValue,
                       compOfRecordsValue.MinAgeValue, compOfRecordsValue.MaxAgeValue,
                       compOfRecordsValue.StartDayValue, compOfRecordsValue.EndDayValue, pflag
                    );
            }
            catch (Exception ex)
            {
                Logger<ThriftService>.Log.Error("", ex);
            }
            return result;
        }


        /// <summary>
        /// 查询比对(分区域)
        /// </summary>
        /// <param name="compOfRecordsValue"></param>
        /// <param name="pflag"></param>
        /// <returns></returns>
        public List<MyCmpFaceLogWidthImgModel> QueryCmpLogSX(CompOfRecordsValue compOfRecordsValue, int pflag)
        {
            List<CmpFaceLogDS> listCmpFaceLog = new List<CmpFaceLogDS>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                listCmpFaceLog = SocketOpter.GetResult<ThriftService, List<CmpFaceLogDS>, string, string, int, int, int, int, long, long, int, int, int>(
                    transport,
                    bServerClient.QueryCmpLogSXDS,
                    "QueryCmpLogSX ", true,
                    compOfRecordsValue.ChannelValue, compOfRecordsValue.NameValue,
                       compOfRecordsValue.TypeValue, compOfRecordsValue.SexValue,
                       compOfRecordsValue.MinAgeValue, compOfRecordsValue.MaxAgeValue,
                       compOfRecordsValue.StartDayValue, compOfRecordsValue.EndDayValue,
                       compOfRecordsValue.PageStartRow, compOfRecordsValue.PageSize, pflag
                    );
            }
            catch (Exception ex)
            {
                Logger<IThirtfService>.Log.Error("QueryCmpLogSX", ex);
            }

            List<MyCmpFaceLogWidthImgModel> listMyCmpFaceLogWidthImg = new List<MyCmpFaceLogWidthImgModel>();
            try
            {
                //比对结果            
                for (int i = listCmpFaceLog.Count - 1; i >= 0; i--)
                {
                    MyCmpFaceLogWidthImgModel _MyCmpFaceLogWidthImg = new MyCmpFaceLogWidthImgModel();

                 


                    //获得序号
                    _MyCmpFaceLogWidthImg.num = compOfRecordsValue.MaxDataCount - compOfRecordsValue.PageStartRow - i;
                    _MyCmpFaceLogWidthImg.ID = listCmpFaceLog[i].ID;// 标识ID

                    //获得通道名称 
                    var channel =
                       ThriftServiceBasic.SelectChannelList().FirstOrDefault(c => c.TcChaneelID == listCmpFaceLog[i].Channel) ?? new MyChannelCfg();
                    _MyCmpFaceLogWidthImg.channelName = channel.Name;
                    _MyCmpFaceLogWidthImg.Address = channel.Channel_address;
                    _MyCmpFaceLogWidthImg.Longitude = channel.Longitude;
                    _MyCmpFaceLogWidthImg.Latitude = channel.Latitude;

                    //_MyCmpFaceLogWidthImg.channel = listCmpFaceLog[i].Channel;// 抓拍通道
                    DateTime time = CodeStacksDataHandler.DateTimeData.ConvertToDateTimeDelegate(listCmpFaceLog[i].Time);

                    _MyCmpFaceLogWidthImg.date = time.ToShortDateString();
                    _MyCmpFaceLogWidthImg.time = time.ToShortTimeString();//抓拍时间
                    Dispatcher.CurrentDispatcher.Invoke(() => _MyCmpFaceLogWidthImg.SnapImage =
                    CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(listCmpFaceLog[i].Capimg));//抓拍照片
                    _MyCmpFaceLogWidthImg.SnapImageBuffer = listCmpFaceLog[i].Capimg;
                    Dispatcher.CurrentDispatcher.Invoke(() => _MyCmpFaceLogWidthImg.TemplatePhoto =
                    CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(listCmpFaceLog[i].Senceimg));//场景照片

                    if (listCmpFaceLog[i].Ft.Count == 0)
                    {
                        _MyCmpFaceLogWidthImg.T1 = new CompOfRecordTemplate();
                        Dispatcher.CurrentDispatcher.Invoke(() => _MyCmpFaceLogWidthImg.T1.TemplateImage =
                        CodeStacksDataHandler.ImageData.ConvertToImageSourceDelegate1("pack://application:,,,/Images/unkonw.png"));
                    };

                    for (int j = 0; j < listCmpFaceLog[i].Ft.Count; j++)
                    {
                        switch (j)
                        {
                            case 0:
                                _MyCmpFaceLogWidthImg.T1 = new CompOfRecordTemplate();
                                _MyCmpFaceLogWidthImg.T1.UserName = "姓 名：" + listCmpFaceLog[i].Ft[j].Name;
                                _MyCmpFaceLogWidthImg.T1.TemplateType = "类 型：" + BasicDataEntry.GetTemplateType(listCmpFaceLog[i].Ft[j].Type);
                                _MyCmpFaceLogWidthImg.T1.TemplateImageBuffer = listCmpFaceLog[i].Ft[j].Objimg;
                                Dispatcher.CurrentDispatcher.Invoke(() => _MyCmpFaceLogWidthImg.T1.TemplateImage =
                                CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(listCmpFaceLog[i].Ft[j].Objimg));
                                _MyCmpFaceLogWidthImg.T1.LikeScore = "相似度：" + listCmpFaceLog[i].Ft[j].Score + "%";
                                _MyCmpFaceLogWidthImg.score = listCmpFaceLog[i].Ft[j].Score;
                                break;
                            case 1:
                                _MyCmpFaceLogWidthImg.T2 = new CompOfRecordTemplate();
                                _MyCmpFaceLogWidthImg.T2.UserName = "姓 名：" + listCmpFaceLog[i].Ft[j].Name;
                                _MyCmpFaceLogWidthImg.T2.TemplateType = "类 型：" + BasicDataEntry.GetTemplateType(listCmpFaceLog[i].Ft[j].Type);
                                Dispatcher.CurrentDispatcher.Invoke(() => _MyCmpFaceLogWidthImg.T2.TemplateImage =
                                CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(listCmpFaceLog[i].Ft[j].Objimg));
                                _MyCmpFaceLogWidthImg.T2.LikeScore = "相似度：" + listCmpFaceLog[i].Ft[j].Score + "%";
                                break;
                            case 2:
                                _MyCmpFaceLogWidthImg.T3 = new CompOfRecordTemplate();
                                _MyCmpFaceLogWidthImg.T3.UserName = "姓 名：" + listCmpFaceLog[i].Ft[j].Name;
                                _MyCmpFaceLogWidthImg.T3.TemplateType = "类 型：" + BasicDataEntry.GetTemplateType(listCmpFaceLog[i].Ft[j].Type);
                                Dispatcher.CurrentDispatcher.Invoke(() => _MyCmpFaceLogWidthImg.T3.TemplateImage =
                                CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(listCmpFaceLog[i].Ft[j].Objimg));
                                _MyCmpFaceLogWidthImg.T3.LikeScore = "相似度：" + listCmpFaceLog[i].Ft[j].Score + "%";
                                break;
                            case 3:
                                _MyCmpFaceLogWidthImg.T4 = new CompOfRecordTemplate();
                                _MyCmpFaceLogWidthImg.T4.UserName = "姓 名：" + listCmpFaceLog[i].Ft[j].Name;
                                _MyCmpFaceLogWidthImg.T4.TemplateType = "类 型：" + BasicDataEntry.GetTemplateType(listCmpFaceLog[i].Ft[j].Type);
                                Dispatcher.CurrentDispatcher.Invoke(() => _MyCmpFaceLogWidthImg.T4.TemplateImage =
                                CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(listCmpFaceLog[i].Ft[j].Objimg));
                                _MyCmpFaceLogWidthImg.T4.LikeScore = "相似度：" + listCmpFaceLog[i].Ft[j].Score + "%";
                                break;
                            case 4:
                                _MyCmpFaceLogWidthImg.T5 = new CompOfRecordTemplate();
                                _MyCmpFaceLogWidthImg.T5.UserName = "姓 名：" + listCmpFaceLog[i].Ft[j].Name;
                                _MyCmpFaceLogWidthImg.T5.TemplateType = "类 型：" + BasicDataEntry.GetTemplateType(listCmpFaceLog[i].Ft[j].Type);
                                Dispatcher.CurrentDispatcher.Invoke(() => _MyCmpFaceLogWidthImg.T5.TemplateImage =
                                CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(listCmpFaceLog[i].Ft[j].Objimg));
                                _MyCmpFaceLogWidthImg.T5.LikeScore = "相似度：" + listCmpFaceLog[i].Ft[j].Score + "%";
                                break;
                            default:
                                break;
                        }
                    }
                    listMyCmpFaceLogWidthImg.Add(_MyCmpFaceLogWidthImg);
                }
            }
            catch (Exception)
            {
            }
            return listMyCmpFaceLogWidthImg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compOfRecordsValue"></param>
        /// <returns></returns>
        public List<MyCmpFaceLogWidthImgModel> QueryCmpLog(CompOfRecordsValue compOfRecordsValue)
        {
            List<CmpFaceLogDS> listCmpFaceLog = new List<CmpFaceLogDS>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                listCmpFaceLog = SocketOpter.GetResult<ThriftService, List<CmpFaceLogDS>, string, string, int, int, int, int, long, long, int, int>(
                    transport,
                    bServerClient.QueryCmpLogDS,
                    "QueryCmpLog", true,
                    compOfRecordsValue.ChannelValue, compOfRecordsValue.NameValue,
                      compOfRecordsValue.TypeValue, compOfRecordsValue.SexValue,
                      compOfRecordsValue.MinAgeValue, compOfRecordsValue.MaxAgeValue,
                      compOfRecordsValue.StartDayValue, compOfRecordsValue.EndDayValue,
                      compOfRecordsValue.PageStartRow, compOfRecordsValue.PageSize
                    );
            }
            catch (Exception)
            {
            }

            List<MyCmpFaceLogWidthImgModel> listMyCmpFaceLogWidthImg = new List<MyCmpFaceLogWidthImgModel>();
            try
            {
                //比对结果
                for (int i = listCmpFaceLog.Count - 1; i >= 0; i--)
                {
                    MyCmpFaceLogWidthImgModel _MyCmpFaceLogWidthImg = new MyCmpFaceLogWidthImgModel();

                    //获得序号
                    if (listCmpFaceLog.Count == compOfRecordsValue.PageSize)//索引等于14
                    {
                        _MyCmpFaceLogWidthImg.num = compOfRecordsValue.MaxDataCount - compOfRecordsValue.PageStartRow - i;
                    }
                    else
                    {
                        if (compOfRecordsValue.MaxDataCount < compOfRecordsValue.PageSize)
                        {
                            _MyCmpFaceLogWidthImg.num = compOfRecordsValue.MaxDataCount - compOfRecordsValue.PageStartRow - i;
                        }
                        else
                        {
                            _MyCmpFaceLogWidthImg.num =
                               compOfRecordsValue.MaxDataCount - compOfRecordsValue.PageStartRow - i -
                               (compOfRecordsValue.PageSize - listCmpFaceLog.Count);
                        }
                    }

                    //获得通道名称 
                    #region 20170531
                    //if (listCmpFaceLog[i].Channel.Contains("\\"))
                    //{
                    //    listCmpFaceLog[i].Channel = listCmpFaceLog[i].Channel.Replace("\\","\\\\");
                    //}
                    #endregion 
                    var channel = GlobalCache.ChannelList.Cast<MyChannelCfg>().FirstOrDefault(c => c.TcChaneelID == listCmpFaceLog[i].Channel) ?? new MyChannelCfg();
                    _MyCmpFaceLogWidthImg.channelName = channel.Name;
                    _MyCmpFaceLogWidthImg.Address = channel.Channel_address;
                    _MyCmpFaceLogWidthImg.Longitude = channel.Longitude;
                    _MyCmpFaceLogWidthImg.Latitude = channel.Latitude;

                    _MyCmpFaceLogWidthImg.ID = listCmpFaceLog[i].ID;// 标识ID
                    _MyCmpFaceLogWidthImg.channel = listCmpFaceLog[i].Channel;// 抓拍通道

                    DateTime time = CodeStacksDataHandler.DateTimeData.ConvertToDateTimeDelegate(listCmpFaceLog[i].Time);
                    _MyCmpFaceLogWidthImg.date = time.ToShortDateString();
                    _MyCmpFaceLogWidthImg.time = time.ToShortTimeString();// 抓拍时间
                    _MyCmpFaceLogWidthImg.SnapImageBuffer = listCmpFaceLog[i].Capimg;//

                    Dispatcher.CurrentDispatcher.Invoke(() => _MyCmpFaceLogWidthImg.SnapImage =
                    CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(listCmpFaceLog[i].Capimg));//抓拍照片
                    if (listCmpFaceLog[i].Senceimg.Length > 0)
                    {
                        Dispatcher.CurrentDispatcher.Invoke(() => _MyCmpFaceLogWidthImg.TemplatePhoto =
                   CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(listCmpFaceLog[i].Senceimg));//场景照片
                    }
                    else
                    {
                        Dispatcher.CurrentDispatcher.Invoke(() => _MyCmpFaceLogWidthImg.TemplatePhoto =
                       CodeStacksDataHandler.ImageData.ConvertToImageSourceDelegate1("pack://application:,,,/Images/unkonw.png"));
                    }

                    if (listCmpFaceLog[i].Ft.Count == 0)
                    {
                        _MyCmpFaceLogWidthImg.T1 = new CompOfRecordTemplate();
                        Dispatcher.CurrentDispatcher.Invoke(() => _MyCmpFaceLogWidthImg.T1.TemplateImage =
                        CodeStacksDataHandler.ImageData.ConvertToImageSourceDelegate1("pack://application:,,,/Images/unkonw.png"));
                    };

                    for (int j = 0; j < listCmpFaceLog[i].Ft.Count; j++)
                    {
                        switch (j)
                        {
                            case 0:
                                _MyCmpFaceLogWidthImg.score = listCmpFaceLog[i].Ft[j].Score;
                                _MyCmpFaceLogWidthImg.type = BasicDataEntry.GetTemplateType(listCmpFaceLog[i].Ft[j].Type);
                                _MyCmpFaceLogWidthImg.TypeKey = listCmpFaceLog[i].Ft[j].Type;
                                _MyCmpFaceLogWidthImg.T1 = new CompOfRecordTemplate();
                                _MyCmpFaceLogWidthImg.tcUuid = listCmpFaceLog[i].Ft[j].TcUuid;
                                _MyCmpFaceLogWidthImg.T1.UserName = "姓 名：" + listCmpFaceLog[i].Ft[j].Name;
                                _MyCmpFaceLogWidthImg.T1.TemplateType = "类 型：" + BasicDataEntry.GetTemplateType(listCmpFaceLog[i].Ft[j].Type);
                                //_MyCmpFaceLogWidthImg.T1.TemplateTypeKey = listCmpFaceLog[i].Ft[j].Type;
                                _MyCmpFaceLogWidthImg.T1.TemplateImageBuffer = listCmpFaceLog[i].Ft[j].Objimg;
                                _MyCmpFaceLogWidthImg.T1.TemplateImage =
                                CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(listCmpFaceLog[i].Ft[j].Objimg);
                                _MyCmpFaceLogWidthImg.T1.LikeScore = "相似度：" + listCmpFaceLog[i].Ft[j].Score + "%";
                                break;
                            case 1:
                                _MyCmpFaceLogWidthImg.T2 = new CompOfRecordTemplate();
                                _MyCmpFaceLogWidthImg.T2.UserName = "姓 名：" + listCmpFaceLog[i].Ft[j].Name;
                                _MyCmpFaceLogWidthImg.T2.TemplateType = "类 型：" + BasicDataEntry.GetTemplateType(listCmpFaceLog[i].Ft[j].Type);
                                //_MyCmpFaceLogWidthImg.T2.TemplateTypeKey = listCmpFaceLog[i].Ft[j].Type;
                                _MyCmpFaceLogWidthImg.T2.TemplateImage =
                                CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(listCmpFaceLog[i].Ft[j].Objimg);
                                _MyCmpFaceLogWidthImg.T2.LikeScore = "相似度：" + listCmpFaceLog[i].Ft[j].Score + "%";
                                break;
                            case 2:
                                _MyCmpFaceLogWidthImg.T3 = new CompOfRecordTemplate();
                                _MyCmpFaceLogWidthImg.T3.UserName = "姓 名：" + listCmpFaceLog[i].Ft[j].Name;
                                _MyCmpFaceLogWidthImg.T3.TemplateType = "类 型：" + BasicDataEntry.GetTemplateType(listCmpFaceLog[i].Ft[j].Type);
                                //_MyCmpFaceLogWidthImg.T3.TemplateTypeKey = listCmpFaceLog[i].Ft[j].Type;
                                _MyCmpFaceLogWidthImg.T3.TemplateImage =
                                CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(listCmpFaceLog[i].Ft[j].Objimg);
                                _MyCmpFaceLogWidthImg.T3.LikeScore = "相似度：" + listCmpFaceLog[i].Ft[j].Score + "%";
                                break;
                            case 3:
                                _MyCmpFaceLogWidthImg.T4 = new CompOfRecordTemplate();
                                _MyCmpFaceLogWidthImg.T4.UserName = "姓 名：" + listCmpFaceLog[i].Ft[j].Name;
                                _MyCmpFaceLogWidthImg.T4.TemplateType = "类 型：" + BasicDataEntry.GetTemplateType(listCmpFaceLog[i].Ft[j].Type);
                                //_MyCmpFaceLogWidthImg.T4.TemplateTypeKey = listCmpFaceLog[i].Ft[j].Type;
                                _MyCmpFaceLogWidthImg.T4.TemplateImage =
                                CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(listCmpFaceLog[i].Ft[j].Objimg);
                                _MyCmpFaceLogWidthImg.T4.LikeScore = "相似度：" + listCmpFaceLog[i].Ft[j].Score + "%";
                                break;
                            case 4:
                                _MyCmpFaceLogWidthImg.T5 = new CompOfRecordTemplate();
                                _MyCmpFaceLogWidthImg.T5.UserName = "姓 名：" + listCmpFaceLog[i].Ft[j].Name;
                                _MyCmpFaceLogWidthImg.T5.TemplateType = "类 型：" + BasicDataEntry.GetTemplateType(listCmpFaceLog[i].Ft[j].Type);
                                //_MyCmpFaceLogWidthImg.T5.TemplateTypeKey = listCmpFaceLog[i].Ft[j].Type;
                                _MyCmpFaceLogWidthImg.T5.TemplateImage =
                                CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(listCmpFaceLog[i].Ft[j].Objimg);
                                _MyCmpFaceLogWidthImg.T5.LikeScore = "相似度：" + listCmpFaceLog[i].Ft[j].Score + "%";
                                break;
                            default:
                                break;
                        }
                    }

                    listMyCmpFaceLogWidthImg.Add(_MyCmpFaceLogWidthImg);
                }
            }
            catch (Exception ex)
            {
                Logger<ThriftService>.Log.Error("QueryCmpLog", ex);
            }
            return listMyCmpFaceLogWidthImg;
        }

        /// <summary>
        /// 查询比对记录
        /// </summary>
        /// <returns></returns>
        public List<MyCmpFaceLogWidthImgModel> QueryCmpLogOld(CompOfRecordsValue compOfRecordsValue)
        {
            List<CmpFaceLog> listCmpFaceLog = new List<CmpFaceLog>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                listCmpFaceLog = SocketOpter.GetResult<ThriftService, List<CmpFaceLog>, string, string, int, int, int, int, long, long, int, int>(
                    transport,
                    bServerClient.QueryCmpLog,
                    "QueryCmpLogOld ", true,
                    compOfRecordsValue.ChannelValue, compOfRecordsValue.NameValue,
                       compOfRecordsValue.TypeValue, compOfRecordsValue.SexValue, compOfRecordsValue.MinAgeValue, compOfRecordsValue.MaxAgeValue, compOfRecordsValue.StartDayValue, compOfRecordsValue.EndDayValue, compOfRecordsValue.PageStartRow, compOfRecordsValue.PageSize
                    );
            }
            catch (Exception)
            {
            }

            List<MyCmpFaceLogWidthImgModel> listMyCmpFaceLogWidthImg = new List<MyCmpFaceLogWidthImgModel>();
            try
            {
                //比对结果            
                for (int i = listCmpFaceLog.Count - 1; i >= 0; i--)
                {
                    MyCmpFaceLogWidthImgModel _MyCmpFaceLogWidthImg = new MyCmpFaceLogWidthImgModel();
                    //获得序号
                    _MyCmpFaceLogWidthImg.num = compOfRecordsValue.MaxDataCount - compOfRecordsValue.PageStartRow - i;
                    _MyCmpFaceLogWidthImg.ID = listCmpFaceLog[i].ID;// 标识ID
                    _MyCmpFaceLogWidthImg.name = listCmpFaceLog[i].Name;// 姓名

                    //获得通道名称 
                    var channel = GlobalCache.ChannelList.Cast<MyChannelCfg>().FirstOrDefault(c => c.TcChaneelID == listCmpFaceLog[i].Channel) ?? new MyChannelCfg();
                    _MyCmpFaceLogWidthImg.channelName = channel.Name;
                    //_MyCmpFaceLogWidthImg.channel = listCmpFaceLog[i].Channel;// 抓拍通道
                    DateTime time = CodeStacksDataHandler.DateTimeData.ConvertToDateTimeDelegate(listCmpFaceLog[i].Time);
                    _MyCmpFaceLogWidthImg.time = time.ToString("yyyy/MM/dd HH:mm:ss");// 抓拍时间
                    _MyCmpFaceLogWidthImg.type = BasicDataEntry.GetTemplateType(listCmpFaceLog[i].Type);

                    _MyCmpFaceLogWidthImg.score = listCmpFaceLog[i].Score;// 相似度
                    _MyCmpFaceLogWidthImg.tcUuid = listCmpFaceLog[i].TcUuid; // uuid，模板ID

                    listMyCmpFaceLogWidthImg.Add(_MyCmpFaceLogWidthImg);
                }
            }
            catch (Exception ex)
            {
                Logger<ThriftService>.Log.Error("QueryCmpLogOld", ex);
            }
            return listMyCmpFaceLogWidthImg;
        }

        /// <summary>
        /// 查询比对(分区域)
        /// </summary>
        /// <param name="compOfRecordsValue"></param>
        /// <param name="pflag"></param>
        /// <returns></returns>
        public List<MyCmpFaceLogWidthImgModel> QueryCmpLogSXOld(CompOfRecordsValue compOfRecordsValue, int pflag)
        {
            List<CmpFaceLog> listCmpFaceLog = new List<CmpFaceLog>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                listCmpFaceLog = SocketOpter.GetResult<ThriftService, List<CmpFaceLog>, string, string, int, int, int, int, long, long, int, int, int>(
                     transport,
                     bServerClient.QueryCmpLogSX,
                     "QueryCmpLogSXOld ", true,
                     compOfRecordsValue.ChannelValue, compOfRecordsValue.NameValue,
                        compOfRecordsValue.TypeValue, compOfRecordsValue.SexValue, compOfRecordsValue.MinAgeValue, compOfRecordsValue.MaxAgeValue, compOfRecordsValue.StartDayValue, compOfRecordsValue.EndDayValue, compOfRecordsValue.PageStartRow, compOfRecordsValue.PageSize, pflag
                     );
            }
            catch (Exception)
            {
            }

            List<MyCmpFaceLogWidthImgModel> listMyCmpFaceLogWidthImg = new List<MyCmpFaceLogWidthImgModel>();
            try
            {
                //比对结果            
                for (int i = listCmpFaceLog.Count - 1; i >= 0; i--)
                {
                    MyCmpFaceLogWidthImgModel _MyCmpFaceLogWidthImg = new MyCmpFaceLogWidthImgModel();
                    //获得序号
                    _MyCmpFaceLogWidthImg.num = compOfRecordsValue.MaxDataCount - compOfRecordsValue.PageStartRow - i;
                    _MyCmpFaceLogWidthImg.ID = listCmpFaceLog[i].ID;// 标识ID
                    _MyCmpFaceLogWidthImg.name = listCmpFaceLog[i].Name;// 姓名

                    //获得通道名称
                    var channel = GlobalCache.ChannelList.Cast<MyChannelCfg>().FirstOrDefault(c => c.TcChaneelID == listCmpFaceLog[i].Channel) ?? new MyChannelCfg();
                    _MyCmpFaceLogWidthImg.channelName = channel.Name;

                    //_MyCmpFaceLogWidthImg.channel = listCmpFaceLog[i].Channel;// 抓拍通道
                    long longTime = listCmpFaceLog[i].Time;
                    DateTime time = new DateTime(1970, 1, 1);
                    time = time.AddSeconds(longTime);
                    _MyCmpFaceLogWidthImg.time = time.ToString("yyyy/MM/dd HH:mm:ss");// 抓拍时间

                    _MyCmpFaceLogWidthImg.type = BasicDataEntry.GetTemplateType(listCmpFaceLog[i].Type);

                    _MyCmpFaceLogWidthImg.score = listCmpFaceLog[i].Score;// 相似度
                    _MyCmpFaceLogWidthImg.tcUuid = listCmpFaceLog[i].TcUuid; // uuid，模板ID

                    listMyCmpFaceLogWidthImg.Add(_MyCmpFaceLogWidthImg);
                }
            }
            catch (Exception ex)
            {
                Logger<ThriftService>.Log.Error("QueryCmpLogSXOld", ex);
            }
            return listMyCmpFaceLogWidthImg;
        }


        /// <summary>
        /// 查询比对记录
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<byte[]> QueryCmpLogImage(string ID)
        {
            List<byte[]> result = new List<byte[]>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftService, List<byte[]>, string>(
                    transport,
                    bServerClient.QueryCmpLogImage,
                    "QueryCmpLogImage", true,
                    ID
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
        /// <param name="ID"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public List<byte[]> QueryCmpLogImageH(string ID, string day)
        {
            List<byte[]> result = new List<byte[]>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftService, List<byte[]>, string, string>(
                    transport,
                    bServerClient.QueryCmpLogImageH,
                    "QueryCmpLogImage", true,
                    ID, day
                    );
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 查询模版照片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<FaceObj> QueryFaceObj(string id)
        {
            List<FaceObj> result = new List<FaceObj>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftService, List<FaceObj>, string, string, int, int, int, int, long, long, int, int>(
                    transport,
                    bServerClient.QueryFaceObj,
                    "QueryFaceObj 查询模版照片", true,
                    id, string.Empty, -1, -1, -1, -1, long.MinValue, long.MinValue, -1, -1
                    );
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 抓拍记录数量
        /// </summary>
        /// <param name="captureRecordQueryValue"></param>
        /// <returns></returns>
        public int QueryCapRecordTotalCount(CaptureRecordQueryValue captureRecordQueryValue)
        {
            int count = 0;
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                count = SocketOpter.GetResult<ThriftService, int, string, long, long>(
                    transport,
                    bServerClient.QueryCapRecordTotalCount,
                    "QueryCapRecordTotalCount 抓拍记录数量", true,
                    captureRecordQueryValue.ChannelValue, captureRecordQueryValue.StartDayValue, captureRecordQueryValue.EndDayValue
                    );
            }
            catch (Exception)
            {
            }
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="captureRecordQueryValue"></param>
        /// <returns></returns>
        public List<SCountInfo> QueryCapRecordTotalCountH(CaptureRecordQueryValue captureRecordQueryValue)
        {
            List<SCountInfo> result = new List<SCountInfo>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftService, List<SCountInfo>, string, long, long>(
                    transport,
                    bServerClient.QueryCapRecordTotalCountH,
                    "QueryCapRecordTotalCountH", true,
                    captureRecordQueryValue.ChannelValue, captureRecordQueryValue.StartDayValue, captureRecordQueryValue.EndDayValue
                    );
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 查询比对记录数(筛选)
        /// </summary>
        /// <param name="captureRecordQueryValue"></param>
        /// <returns></returns>
        public List<SCountInfo> QueryCapRecordTotalCountHSXC(CaptureRecordQueryValue captureRecordQueryValue, List<string> channelName)
        {
            List<SCountInfo> result = new List<SCountInfo>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftService, List<SCountInfo>, List<string>, long, long>(
                    transport,
                    bServerClient.QueryCapRecordTotalCountHSXC,
                    "QueryCapRecordTotalCountHSXC", true,
                    channelName, captureRecordQueryValue.StartDayValue, captureRecordQueryValue.EndDayValue
                    );
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 抓拍记录数据
        /// </summary>
        /// <param name="captureRecordQueryValue"></param>
        /// <returns></returns>
        public List<MyCapFaceLogWithImg> QueryCapLog(CaptureRecordQueryValue captureRecordQueryValue)
        {
            List<CapFaceLog> listCapFaceLog = new List<CapFaceLog>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                listCapFaceLog = SocketOpter.GetResult<ThriftService, List<CapFaceLog>, string, long, long, int, int>(
                    transport,
                    bServerClient.QueryCapLog,
                    "QueryCapLog 抓拍记录数据", true,
                    captureRecordQueryValue.ChannelValue, captureRecordQueryValue.StartDayValue,
                    captureRecordQueryValue.EndDayValue, captureRecordQueryValue.StartRowValue,
                    captureRecordQueryValue.PageRowValue
                    );
            }
            catch (Exception)
            {
            }

            List<MyCapFaceLogWithImg> listMyCapFaceLogWithImg = new List<MyCapFaceLogWithImg>();
            try
            {
                for (int i = listCapFaceLog.Count - 1; i >= 0; i--)
                {
                    MyCapFaceLogWithImg _MyCapFaceLogWithImg = new MyCapFaceLogWithImg();
                    _MyCapFaceLogWithImg.Id = captureRecordQueryValue.MaxCount - captureRecordQueryValue.StartRowValue - i;
                    _MyCapFaceLogWithImg.ID = listCapFaceLog[i].ID;// 获得抓拍id
                    _MyCapFaceLogWithImg.ChannelID = listCapFaceLog[i].ChannelID;// 获得通道id
                    _MyCapFaceLogWithImg.ChannelName = listCapFaceLog[i].Channelname;

                    //获得通道名称 
                    //foreach (MyChannelCfg mcc in GlobalCache.ChannelList)
                    //{
                    //    if (listCapFaceLog[i].ChannelID == mcc.TcChaneelID)
                    //    {
                    //        _MyCapFaceLogWithImg.ChannelName = mcc.Name;
                    //    }
                    //}

                    long longTime = listCapFaceLog[i].Time;
                    DateTime time = new DateTime(1970, 1, 1);
                    time = time.AddSeconds(longTime);
                    _MyCapFaceLogWithImg.time = time.ToString("yyyy/MM/dd HH:mm:ss");// 获得抓拍时间

                    listMyCapFaceLogWithImg.Add(_MyCapFaceLogWithImg);
                }
            }
            catch (Exception ex)
            {
                Logger<ThriftService>.Log.Error("QueryCapLog InternalErr", ex);
            }
            return listMyCapFaceLogWithImg;
        }

        /// <summary>
        /// 查询抓拍记录(筛选)
        /// </summary>
        /// <param name="captureRecordQueryValue"></param>
        /// <param name="pflag"></param>
        /// <returns></returns>
        public List<MyCapFaceLogWithImg> QueryCapLogSXC(CaptureRecordQueryValue captureRecordQueryValue, List<string> channelList)
        {
            List<CapFaceLog> listCapFaceLog = new List<CapFaceLog>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                listCapFaceLog = SocketOpter.GetResult<ThriftService, List<CapFaceLog>, List<string>, long, long, int, int>(
                    transport,
                    bServerClient.QueryCapLogSXC,
                    "QueryCapLogSXC查询抓拍记录(筛选)", true,
                    channelList, captureRecordQueryValue.StartDayValue, captureRecordQueryValue.EndDayValue, captureRecordQueryValue.StartRowValue, captureRecordQueryValue.PageRowValue
                    );
            }
            catch (Exception)
            {
            }

            List<MyCapFaceLogWithImg> listMyCapFaceLogWithImg = new List<MyCapFaceLogWithImg>();
            try
            {
                for (int i = listCapFaceLog.Count - 1; i >= 0; i--)
                {
                    MyCapFaceLogWithImg _MyCapFaceLogWithImg = new MyCapFaceLogWithImg();
                    _MyCapFaceLogWithImg.Id = captureRecordQueryValue.MaxCount - captureRecordQueryValue.StartRowValue - i;
                    _MyCapFaceLogWithImg.ID = listCapFaceLog[i].ID;// 获得抓拍id
                    _MyCapFaceLogWithImg.ChannelID = listCapFaceLog[i].ChannelID;// 获得通道id

                    //获得通道名称 
                    foreach (MyChannelCfg mcc in GlobalCache.ChannelList)
                    {
                        if (listCapFaceLog[i].ChannelID == mcc.TcChaneelID)
                        {
                            _MyCapFaceLogWithImg.ChannelName = mcc.Name;
                        }
                    }

                    long longTime = listCapFaceLog[i].Time;
                    DateTime time = new DateTime(1970, 1, 1);
                    time = time.AddSeconds(longTime);
                    _MyCapFaceLogWithImg.time = time.ToString("yyyy/MM/dd HH:mm:ss"); ;// 获得抓拍时间

                    listMyCapFaceLogWithImg.Add(_MyCapFaceLogWithImg);
                }
            }
            catch (Exception ex)
            {
                Logger<ThriftService>.Log.Error("QueryCapLogSXC InternalErr", ex);
            }
            return listMyCapFaceLogWithImg;
        }

        /// <summary>
        /// 抓拍记录照片
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<byte[]> QueryCapLogImage(string ID)
        {
            List<byte[]> result = new List<byte[]>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftService, List<byte[]>, string>(
                    transport,
                    bServerClient.QueryCapLogImage,
                    "QueryCapLogImage 抓拍记录照片", true,
                    ID
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
        /// <param name="ID"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public List<byte[]> QueryCapLogImageH(string ID, string day)
        {
            List<byte[]> result = new List<byte[]>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftService, List<byte[]>, string, string>(
                    transport,
                    bServerClient.QueryCapLogImageH,
                    "QueryCapLogImage 抓拍记录照片", true,
                    ID, day
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
        /// <param name="ID"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public List<byte[]> QuerySenceImg(string ID, string day)
        {
            List<byte[]> result = new List<byte[]>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                return SocketOpter.GetResult<ThriftService, List<byte[]>, string, string>(
                    transport,
                    bServerClient.QuerySenceImg,
                    "QueryCapLogImage 抓拍记录照片", true,
                    ID, day
                    );
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 模板管理记录总数
        /// </summary>
        /// <param name="templateManagerValue"></param>
        /// <returns></returns>
        public int QueryFaceObjTotalCount(TemplateManagerViewModel.TemplateManagerValue template)
        {
            int count = 0;
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                count = SocketOpter.GetResult<ThriftService, int, string, string, int, int, int, int, long, long>(
                    transport,
                    bServerClient.QueryFaceObjTotalCount,
                    "QueryFaceObjTotalCount 模板管理记录总数", true,
                    null,
                        template.NameValue, template.LittleAgeValue, template.OldAgeValue, template.SexValue, template.TypeValue, template.StartDayValue, template.EndDayValue
                    );
            }
            catch (Exception)
            {
            }
            return count;
        }

        /// <summary>
        /// 查询模版
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        public List<MyFaceObj> QueryFaceObj(TemplateManagerViewModel.TemplateManagerValue template)
        {
            List<FaceObj> _ListFaceObj = new List<FaceObj>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                _ListFaceObj = SocketOpter.GetResult<ThriftService, List<FaceObj>, string, string, int, int, int, int, long, long, int, int>(
                    transport,
                    bServerClient.QueryFaceObj,
                    "QueryFaceObj 查询模版", true,
                    null, template.NameValue, template.LittleAgeValue, template.OldAgeValue, template.SexValue, template.TypeValue, template.StartDayValue, template.EndDayValue, template.StartRowValue, template.PageRowValue
                    );
            }
            catch (Exception)
            {
            }

            List<MyFaceObj> _ListMyFaceObj = new List<MyFaceObj>();
            try
            {
                List<string> sexList = new List<string>() { "未知", "男", "女" };

                for (int i = _ListFaceObj.Count - 1; i >= 0; i--)//循环得到人脸
                {
                    MyFaceObj _MyFaceObj = new MyFaceObj();
                    _MyFaceObj.faceObj = _ListFaceObj[i];
                    _MyFaceObj.ID = template.MaxCount - template.StartRowValue - i;
                    _MyFaceObj.fa_ob_tcUuid = _ListFaceObj[i].TcUuid;
                    _MyFaceObj.tcName = _ListFaceObj[i].TcName;// 姓名 
                    _MyFaceObj.nType = BasicDataEntry.GetTemplateType(_ListFaceObj[i].NType);
                    _MyFaceObj.nSex = sexList[_ListFaceObj[i].NSex];// 性别（0，未知；1，男；2，女）
                    _MyFaceObj.nMain_ftID = _ListFaceObj[i].NMain_ftID;
                    _MyFaceObj.nAge = _ListFaceObj[i].NAge;     // 年龄 
                    
                    _MyFaceObj.fa_ob_dTm = new DateTime(1970, 1, 1).AddSeconds(_ListFaceObj[i].DTm).ToString();

                    _MyFaceObj.fa_ob_tcRemarks = _ListFaceObj[i].TcRemarks;

                    _ListMyFaceObj.Add(_MyFaceObj);
                }
            }
            catch (Exception ex)
            {
                Logger<ThriftService>.Log.Error("QueryFaceObjTotalCount", ex);
            }
            return _ListMyFaceObj;
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
                result = SocketOpter.GetResult<ThriftService, List<ErrorInfo>, string, FaceObj>(
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

        /// <summary>
        /// 删除模版
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int DelFaceObj(string ID)
        {
            int result = 404;
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftService, int, string>(
                    transport,
                    bServerClient.DelFaceObj,
                    "DelFaceObj 删除模版", true,
                    ID
                    );
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 添加模版
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<ErrorInfo> AddFaceObj(FaceObj obj)
        {
            List<ErrorInfo> result = new List<ErrorInfo>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                return SocketOpter.GetResult<ThriftService, List<ErrorInfo>, FaceObj>(
                    transport,
                    bServerClient.AddFaceObj,
                    "AddFaceObj 添加模版", true,
                    obj
                    );
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 查询通道类型
        /// </summary>
        /// <returns></returns>
        public List<string> QueryDefChannelType()
        {
            List<string> result = new List<string>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                return SocketOpter.GetResult<ThriftService, List<string>>(
                    transport,
                    bServerClient.QueryDefChannelType,
                    "QueryDefChannelType 查询通道类型", true
                    );
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 查询视频源类型
        /// </summary>
        /// <returns></returns>
        public List<string> QueryDefCameraType()
        {
            List<string> result = new List<string>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                return SocketOpter.GetResult<ThriftService, List<string>>(
                    transport,
                    bServerClient.QueryDefCameraType,
                    "QueryDefCameraType 查询视频源类型", true
                    );
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 修改频道
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public int ModifyChannel(ChannelCfgLBS cfg)
        {
            int result = 404;
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                return SocketOpter.GetResult<ThriftService, int, ChannelCfgLBS>(
                    transport,
                    bServerClient.ModifyChannelLBS,
                    "ModifyChannel 修改频道", true,
                    cfg
                    );
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 添加频道
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public int AddChannel(ChannelCfgLBS cfg)
        {
            int result = 404;
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftService, int, ChannelCfgLBS>(
                    transport,
                    bServerClient.AddChannelLBS,
                    "AddChannel 添加新的通道", true,
                    cfg
                    );
            }
            catch (Exception)
            {
            }
            return result;
        }
        /// <summary>
        /// 添加频道
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public int AddChannel(ChannelCfg cfg)
        {
            int result = 404;
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                return SocketOpter.GetResult<ThriftService, int, ChannelCfg>(
                    transport,
                    bServerClient.AddChannel,
                    "AddChannel 添加新的通道", true,
                    cfg
                    );
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 修改频道
        /// </summary>
        /// <param name="cfg"></param>
        /// <returns></returns>
        public int ModifyChannel(ChannelCfg cfg)
        {
            int result = 404;
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftService, int, ChannelCfg>(
                    transport,
                    bServerClient.ModifyChannel,
                    "ModifyChannel", true,
                    cfg
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
        /// <param name="imageStrem"></param>
        /// <param name="threshold"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public List<MyFaceObj> QueryFaceObjByImg(byte[] imageStrem, int threshold, int maxCount)
        {
            List<FaceObj> _ListFaceObj = new List<FaceObj>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                _ListFaceObj = SocketOpter.GetResult<ThriftService, List<FaceObj>, byte[], int, int>(
                    transport,
                    bServerClient.QueryFaceObjByImg,
                    "QueryFaceObjByImg", true,
                    imageStrem,
                    threshold,
                    maxCount
                    );
            }
            catch (Exception)
            {
            }

            List<MyFaceObj> _ListMyFaceObj = new List<MyFaceObj>();
            try
            {
                List<string> sexList = new List<string>() { "未知", "男", "女" };
                for (int i = _ListFaceObj.Count - 1; i >= 0; i--)//循环得到人脸
                {
                    MyFaceObj _MyFaceObj = new MyFaceObj();
                    _MyFaceObj.faceObj = _ListFaceObj[i];
                    _MyFaceObj.ID = _ListFaceObj.Count - i;
                    _MyFaceObj.fa_ob_tcUuid = _ListFaceObj[i].TcUuid;
                    _MyFaceObj.tcName = _ListFaceObj[i].TcName;// 姓名                     
                    _MyFaceObj.nType = BasicDataEntry.GetTemplateType(_ListFaceObj[i].NType);// 类型 
                    _MyFaceObj.nSex = sexList[_ListFaceObj[i].NSex];// 性别（0，未知；1，男；2，女）
                    _MyFaceObj.nMain_ftID = _ListFaceObj[i].NMain_ftID;
                    _MyFaceObj.nAge = _ListFaceObj[i].NAge;		// 年龄 

                    long longTime = _ListFaceObj[i].DTm;
                    DateTime time = new DateTime(1970, 1, 1);
                    time = time.AddSeconds(longTime);
                    _MyFaceObj.fa_ob_dTm = time.ToString();

                    _MyFaceObj.fa_ob_tcRemarks = _ListFaceObj[i].TcRemarks;

                    _ListMyFaceObj.Insert(0, _MyFaceObj);
                }
            }
            catch (Exception ex)
            {
                Logger<ThriftService>.Log.Error("QueryFaceObjByImg", ex);
            }
            return _ListMyFaceObj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<CmpFaceLogWidthImg> QueryCmpByCapIdWidthImg(string ID)
        {
            List<CmpFaceLogWidthImg> reuslt = new List<CmpFaceLogWidthImg>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                reuslt = SocketOpter.GetResult<ThriftService, List<CmpFaceLogWidthImg>, string>(
                     transport,
                     bServerClient.QueryCmpByCapIdWidthImg,
                     "QueryCmpByCapIdWidthImg", true,
                     ID
                     );
            }
            catch (Exception)
            {
            }
            return reuslt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public List<CmpFaceLogWidthImg> QueryCmpByCapIdWidthImgH(string ID, string day)
        {
            List<CmpFaceLogWidthImg> result = new List<CmpFaceLogWidthImg>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftService, List<CmpFaceLogWidthImg>, string, string>(
                    transport,
                    bServerClient.QueryCmpByCapIdWidthImgH,
                    "", true,
                    ID, day
                    );
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 删除通道
        /// </summary>
        /// <param name="channelID"></param>
        /// <returns></returns>
        public int DelChannel(string channelID)
        {
            int result = 404;
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftService, int, string>(
                    transport,
                    bServerClient.DelChannel,
                    "DelChannel 删除通道", true,
                    channelID
                    );
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 设置新的域值
        /// </summary>
        /// <param name="threshold"></param>
        /// <returns></returns>
        public int SetCMPthreshold(int threshold)
        {
            int result = 404;
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                return SocketOpter.GetResult<ThriftService, int, int>(
                    transport,
                    bServerClient.SetCMPthreshold,
                    "SetCMPthreshold 设置新的阈值", true,
                    threshold
                    );
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 查询当前的域值
        /// </summary>
        /// <returns></returns>
        public int QueryThreshold()
        {
            int result = 404;
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftService, int>(
                    transport,
                    bServerClient.QueryThreshold,
                    "QueryThreshold 查询当前阈值", true
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
        /// <param name="ID"></param>
        /// <param name="uuid"></param>
        /// <param name="day"></param>
        /// <param name="pflag"></param>
        /// <returns></returns>
        public int UpdateCmpLog(string ID, string uuid, string day, int pflag)
        {
            int result = 404;
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftService, int, string, string, string, int>(
                    transport,
                    bServerClient.UpdateCmpLog,
                    "UpdateCmpLog 更新比对记录", true,
                    ID, uuid, day, pflag
                    );
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 查询性别定义
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<STypeInfo> QueryDefGenderH(int id)
        {
            List<STypeInfo> result = new List<STypeInfo>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                return SocketOpter.GetResult<ThriftService, List<STypeInfo>, int>(
                    transport,
                    bServerClient.QueryDefGenderH,
                    "QueryDefGenderH 查询性别", true,
                    id
                    );
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 查询人脸对象类型定义
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<STypeInfo> QueryDefFaceObjTypeH(int id)
        {
            List<STypeInfo> reuslt = new List<STypeInfo>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                reuslt = SocketOpter.GetResult<ThriftService, List<STypeInfo>, int>(
                    transport,
                    bServerClient.QueryDefFaceObjTypeH,
                    "QueryDefFaceObjTypeH 查询人脸对象类型定义", true,
                    id
                    );
            }
            catch (Exception)
            {
            }
            return reuslt;
        }

        /// <summary>
        /// 查询通道类型定义
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<STypeInfo> QueryDefChannelTypeH(int id)
        {
            List<STypeInfo> result = new List<STypeInfo>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftService, List<STypeInfo>, int>(
                    transport,
                    bServerClient.QueryDefChannelTypeH,
                    "QueryDefChannelTypeH 查询通道类型定义", true,
                    id
                    );
            }
            catch (Exception)
            {
            }
            return result;
        }

        /// <summary>
        /// 查询相机类型定义
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<STypeInfo> QueryDefCameraTypeH(int id)
        {
            List<STypeInfo> result = new List<STypeInfo>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftService, List<STypeInfo>, int>(
                    transport,
                    bServerClient.QueryDefChannelTypeH,
                    "QueryDefCameraTypeH 查询相机类型定义", true,
                    id
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
        /// <param name="capimg"></param>
        /// <param name="btime"></param>
        /// <param name="etime"></param>
        /// <returns></returns>
        public List<TrackInfo> QueryTrackPlayback(string capid, byte[] capimg, long btime, long etime)
        {
            List<TrackInfo> result = new List<TrackInfo>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftService, List<TrackInfo>, string, byte[], long, long>(
                    transport,
                    bServerClient.QueryTrackPlayback, "QueryTrackPlayback", true,
                    capid, capimg, btime, etime
                    );
            }
            catch (Exception)
            {
            }
            return result;
        }
        /// <summary>
        /// 打开通道
        /// </summary>
        /// <param name="strChannelID"></param>
        /// <returns></returns>
        public int OpenChannel(string strChannelID)
        {
            int res = -1;
            try
            {
                BusinessServer.Client bServer = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServer);
                res = SocketOpter.GetResult<ThriftService, int, string>(
                    transport,bServer.OpenChannel,"OpenChannel",true,strChannelID
                    );
            }
            catch (Exception)
            {
            }
            return res;
        }
        /// <summary>
        /// 关闭通道
        /// </summary>
        /// <param name="strChannelID"></param>
        /// <returns></returns>
        public int CloseChannel(string strChannelID)
        {
            int res = -1;
            try
            {
                BusinessServer.Client bServer = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServer);
                res = SocketOpter.GetResult<ThriftService, int, string>(
                    transport, bServer.CloseChannel, "CloseChannel", true, strChannelID
                    );
            }
            catch (Exception)
            {
            }
            return res;
        }
    }
}
