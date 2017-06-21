using DATA.MODELS.GlobalModels;
using DATA.UTILITIES.Log4Net;
using SENSING.ClassPool;
using SENSING.THRIFT.CommonServices;
using System;
using System.Collections.Generic;
using Thrift.Transport;

namespace SENSING.THRIFT.Services
{
    public class ThriftServiceBasic
    {
        #region Generic
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<MyChannelCfg> SelectChannelList()
        {
            List<MyChannelCfg> result = new List<MyChannelCfg>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                List<ChannelCfgLBS> ListChannelCfg =
                SocketOpter.GetResult<ThriftServiceBasic, List<ChannelCfgLBS>>(transport, bServerClient.QueryAllChannelLBS, "QueryAllChannel检出所有通道", true);

                //todo(暂时不需要) 包装返回类 使其返回需要的类
                foreach (ChannelCfgLBS cc in ListChannelCfg)
                {
                    if (cc.Channel_type > -1)
                    {
                        result.Add(new MyChannelCfg().ChannelCfgToMyChannelCfgLBS(cc));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger<ThriftServiceBasic>.Log.Error("SelectChannelList", ex);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<string> SelectChannelPropertiesList()
        {
            List<string> result = new List<string>();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                result = SocketOpter.GetResult<ThriftServiceBasic, List<string>>(
                    transport,
                    bServerClient.QueryDefChannelType,
                    "QueryDefChannelType 查询通道类型", true
                    );
            }
            catch (Exception ex)
            {
                Logger<ThriftServiceBasic>.Log.Error("SelectChannelPropertiesList", ex);
            }
            return result;
        }

        /// <summary>
        /// user singin
        /// 为什么采用整形返回设计：1、必须有回执 2、便于处理异常并准确提示错误
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public static int Singin(ref string ip, ref string port)
        {
            int res = 505;
            int _port = Convert.ToInt32(port);
            BusinessServer.Client bServerClient = null;
            TTransport transport = SocketOpter.Init(ip, _port, 800, ref bServerClient);

            if (!transport.IsOpen)
            {
                try
                {
                    transport.Open();
                    if (transport.IsOpen)
                    {
                        GlobalCache.Host = ip;
                        GlobalCache.Port = _port;
                        res = 200;
                    }
                }
                catch (System.Exception ex)
                {
                    Logger<ThriftServiceBasic>.Log.Error("Singin", ex);
                    res = 404;
                }
                finally
                {
                    transport.Close();
                }
            }
            return res;
        }
        #endregion



        #region 20170410

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static LastRecordInfo initPastResult()
        {
            LastRecordInfo res = new LastRecordInfo();
            try
            {
                BusinessServer.Client bServerClient = null;
                TTransport transport = SocketOpter.Init(GlobalCache.Host, GlobalCache.Port, 0, ref bServerClient);
                res = SocketOpter.GetResult<ThriftServiceBasic, LastRecordInfo>(transport, bServerClient.QueryLastRecordInfo, "initPastResult", true) ?? new LastRecordInfo();
            }
            catch (Exception ex)
            {
                Logger<ThriftServiceBasic>.Log.Error("initPastResult", ex);
            }
            return res;
        }

        #endregion


    }
}
