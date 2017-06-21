using DATA.MODELS.GlobalModels;
using DATA.UTILITIES.Log4Net;
using System;
using Thrift.Protocol;
using Thrift.Transport;
using xiaowen.codestacks.popwindow;

namespace SENSING.THRIFT.CommonServices
{
    public class SocketOpter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="host">host ip</param>
        /// <param name="port">import port</param>
        /// <param name="timeout">timeout</param>
        /// <param name="bServerClient">ref </param>
        /// <returns></returns>
        public static TTransport Init(string host, int port, int timeout, ref BusinessServer.Client bServerClient)
        {
            TTransport transport = null;
            try
            {
                if (timeout > 0)
                    transport = new TSocket(host, port, timeout);
                else
                    transport = new TSocket(host, port, GlobalCache.TSocketTimeout * 1000);
                TProtocol protocol = new TBinaryProtocol(transport);
                bServerClient = new BusinessServer.Client(protocol);
            }
            catch (Exception ex)
            {
                Logger<SocketOpter>.Log.Error("Init", ex);
            }
            return transport;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ErrObj">Exception Class</typeparam>
        /// <typeparam name="TResult">Return Object</typeparam>
        /// <param name="transport">socket</param>
        /// <param name="thriftOpt">thriftservice interface</param>
        /// <param name="methodName">exception method</param>
        /// <returns></returns>
        public static TResult GetResult<ErrObj, TResult>(TTransport transport, Func<TResult> thriftOpt, string methodName, bool isAppearErr)
            where TResult : new()
            where ErrObj : class
        {
            TResult t = new TResult();

            try
            {
                if (!transport.IsOpen) transport.Open();
                if (thriftOpt != null) t = thriftOpt.Invoke();
            }
            catch (Exception ex)
            {
                Logger<ErrObj>.Log.Error(methodName, ex);
                //测试环境下给予提示
                //methodName heart
                if (isAppearErr)
                {
                    if ("DEBUG".Equals(GlobalCache.AppMode))
                    {
                        if (!"HearBeat".Equals(methodName))
                        {
                            CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                        }
                    }
                    else
                    {
                        if (!"HearBeat".Equals(methodName))
                        {
                            CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                        }
                    }
                }
            }
            finally
            {
                transport.Close();
                transport.Dispose();
            }

            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ErrObj"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="transport"></param>
        /// <param name="thriftOpt"></param>
        /// <param name="methodName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static TResult GetResult<ErrObj, TResult, T1>(TTransport transport, Func<T1, TResult> thriftOpt, string methodName, bool isAppearErr, params object[] param)
            where TResult : new()
            where ErrObj : class
        {
            TResult t = new TResult();

            try
            {
                if (!transport.IsOpen) transport.Open();
                t = thriftOpt.Invoke((T1)param[0]);
            }
            catch (Exception ex)
            {
                Logger<ErrObj>.Log.Error(methodName, ex);
                //测试环境下给予提示
                if (isAppearErr)
                {
                    if ("DEBUG".Equals(GlobalCache.AppMode))
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                    else
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                }
            }
            finally
            {
                transport.Close();
            }

            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ErrObj"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="transport"></param>
        /// <param name="thriftOpt"></param>
        /// <param name="methodName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static TResult GetResult<ErrObj, TResult, T1, T2>(TTransport transport, Func<T1, T2, TResult> thriftOpt, string methodName, bool isAppearErr, params object[] param)
            where TResult : new()
            where ErrObj : class
        {
            TResult t = new TResult();

            try
            {
                if (!transport.IsOpen) transport.Open();
                t = thriftOpt.Invoke(
                     (T1)param[0],
                     (T2)param[1]);
            }
            catch (Exception ex)
            {
                Logger<ErrObj>.Log.Error(methodName, ex);
                //测试环境下给予提示
                if (isAppearErr)
                {
                    if ("DEBUG".Equals(GlobalCache.AppMode))
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                    else
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                }
            }
            finally
            {
                transport.Close();
            }

            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ErrObj"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="transport"></param>
        /// <param name="thriftOpt"></param>
        /// <param name="methodName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static TResult GetResult<ErrObj, TResult, T1, T2, T3>(TTransport transport, Func<T1, T2, T3, TResult> thriftOpt, string methodName, bool isAppearErr, params object[] param)
            where TResult : new()
            where ErrObj : class
        {
            TResult t = new TResult();

            try
            {
                if (!transport.IsOpen) transport.Open();
                t = thriftOpt.Invoke(
                    (T1)param[0],
                    (T2)param[1],
                    (T3)param[2]);
            }
            catch (Exception ex)
            {
                Logger<ErrObj>.Log.Error(methodName, ex);
                //测试环境下给予提示
                if (isAppearErr)
                {
                    if ("DEBUG".Equals(GlobalCache.AppMode))
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                    else
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                }
            }
            finally
            {
                transport.Close();
            }

            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ErrObj"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="transport"></param>
        /// <param name="thriftOpt"></param>
        /// <param name="methodName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static TResult GetResult<ErrObj, TResult, T1, T2, T3, T4>(TTransport transport, Func<T1, T2, T3, T4, TResult> thriftOpt, string methodName, bool isAppearErr, params object[] param)
            where TResult : new()
            where ErrObj : class
        {
            TResult t = new TResult();

            try
            {
                if (!transport.IsOpen) transport.Open();
                t = thriftOpt.Invoke(
                    (T1)param[0],
                    (T2)param[1],
                    (T3)param[2],
                    (T4)param[3]);
            }
            catch (Exception ex)
            {
                Logger<ErrObj>.Log.Error(methodName, ex);
                //测试环境下给予提示
                if (isAppearErr)
                {
                    if ("DEBUG".Equals(GlobalCache.AppMode))
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                    else
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                }
            }
            finally
            {
                transport.Close();
            }

            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ErrObj"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="transport"></param>
        /// <param name="thriftOpt"></param>
        /// <param name="methodName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static TResult GetResult<ErrObj, TResult, T1, T2, T3, T4, T5>(TTransport transport, Func<T1, T2, T3, T4, T5, TResult> thriftOpt, string methodName, bool isAppearErr, params object[] param)
           where TResult : new()
           where ErrObj : class
        {
            TResult t = new TResult();

            try
            {
                if (!transport.IsOpen) transport.Open();
                t = thriftOpt.Invoke(
                     (T1)param[0],
                     (T2)param[1],
                     (T3)param[2],
                     (T4)param[3],
                     (T5)param[4]);
            }
            catch (Exception ex)
            {
                Logger<ErrObj>.Log.Error(methodName, ex);
                //测试环境下给予提示
                if (isAppearErr)
                {
                    if ("DEBUG".Equals(GlobalCache.AppMode))
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                    else
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                }
            }
            finally
            {
                transport.Close();
            }

            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ErrObj"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <param name="transport"></param>
        /// <param name="thriftOpt"></param>
        /// <param name="methodName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static TResult GetResult<ErrObj, TResult, T1, T2, T3, T4, T5, T6>(TTransport transport, Func<T1, T2, T3, T4, T5, T6, TResult> thriftOpt, string methodName, bool isAppearErr, params object[] param)
           where TResult : new()
           where ErrObj : class
        {
            TResult t = new TResult();

            try
            {
                if (!transport.IsOpen) transport.Open();
                t = thriftOpt.Invoke(
                    (T1)param[0],
                    (T2)param[1],
                    (T3)param[2],
                    (T4)param[3],
                    (T5)param[4],
                    (T6)param[5]);
            }
            catch (Exception ex)
            {
                Logger<ErrObj>.Log.Error(methodName, ex);
                //测试环境下给予提示
                if (isAppearErr)
                {
                    if ("DEBUG".Equals(GlobalCache.AppMode))
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                    else
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                }
            }
            finally
            {
                transport.Close();
            }

            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ErrObj"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <param name="transport"></param>
        /// <param name="thriftOpt"></param>
        /// <param name="methodName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static TResult GetResult<ErrObj, TResult, T1, T2, T3, T4, T5, T6, T7>(TTransport transport, Func<T1, T2, T3, T4, T5, T6, T7, TResult> thriftOpt, string methodName, bool isAppearErr, params object[] param)
           where TResult : new()
           where ErrObj : class
        {
            TResult t = new TResult();

            try
            {
                if (!transport.IsOpen) transport.Open();
                t = thriftOpt.Invoke(
                    (T1)param[0],
                    (T2)param[1],
                    (T3)param[2],
                    (T4)param[3],
                    (T5)param[4],
                    (T6)param[5],
                    (T7)param[6]);
            }
            catch (Exception ex)
            {
                Logger<ErrObj>.Log.Error(methodName, ex);
                //测试环境下给予提示
                if (isAppearErr)
                {
                    if ("DEBUG".Equals(GlobalCache.AppMode))
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                    else
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                }
            }
            finally
            {
                transport.Close();
            }

            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ErrObj"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <param name="transport"></param>
        /// <param name="methodName"></param>
        /// <param name="thriftOpt"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static TResult GetResult<ErrObj, TResult, T1, T2, T3, T4, T5, T6, T7, T8>(TTransport transport, Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> thriftOpt, string methodName, bool isAppearErr, params object[] param)
          where TResult : new()
          where ErrObj : class
        {
            TResult t = new TResult();

            try
            {
                if (!transport.IsOpen) transport.Open();
                t = thriftOpt.Invoke(
                    (T1)param[0],
                    (T2)param[1],
                    (T3)param[2],
                    (T4)param[3],
                    (T5)param[4],
                    (T6)param[5],
                    (T7)param[6],
                    (T8)param[7]);
            }
            catch (Exception ex)
            {
                Logger<ErrObj>.Log.Error(methodName, ex);
                //测试环境下给予提示
                if (isAppearErr)
                {
                    if ("DEBUG".Equals(GlobalCache.AppMode))
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                    else
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                }
            }
            finally
            {
                transport.Close();
            }

            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ErrObj"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <param name="transport"></param>
        /// <param name="methodName"></param>
        /// <param name="thriftOpt"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static TResult GetResult<ErrObj, TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9>(TTransport transport, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> thriftOpt, string methodName, bool isAppearErr, params object[] param)
          where TResult : new()
          where ErrObj : class
        {
            TResult t = new TResult();

            try
            {
                if (!transport.IsOpen) transport.Open();
                t = thriftOpt.Invoke(
                    (T1)param[0],
                    (T2)param[1],
                    (T3)param[2],
                    (T4)param[3],
                    (T5)param[4],
                    (T6)param[5],
                    (T7)param[6],
                    (T8)param[7],
                    (T9)param[8]);
            }
            catch (Exception ex)
            {
                Logger<ErrObj>.Log.Error(methodName, ex);
                //测试环境下给予提示
                if (isAppearErr)
                {
                    if ("DEBUG".Equals(GlobalCache.AppMode))
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                    else
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                }
            }
            finally
            {
                transport.Close();
            }

            return t;
        }

        public static TResult GetResult<ErrObj, TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(TTransport transport, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> thriftOpt, string methodName, bool isAppearErr, params object[] param)
          where TResult : new()
          where ErrObj : class
        {
            TResult t = new TResult();

            try
            {
                if (!transport.IsOpen) transport.Open();
                t = thriftOpt.Invoke(
                    (T1)param[0],
                    (T2)param[1],
                    (T3)param[2],
                    (T4)param[3],
                    (T5)param[4],
                    (T6)param[5],
                    (T7)param[6],
                    (T8)param[7],
                    (T9)param[8],
                    (T10)param[9]);
            }
            catch (Exception ex)
            {
                Logger<ErrObj>.Log.Error(methodName, ex);
                //测试环境下给予提示
                if (isAppearErr)
                {
                    if ("DEBUG".Equals(GlobalCache.AppMode))
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                    else
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                }
            }
            finally
            {
                transport.Close();
            }

            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ErrObj"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <typeparam name="T6"></typeparam>
        /// <typeparam name="T7"></typeparam>
        /// <typeparam name="T8"></typeparam>
        /// <typeparam name="T9"></typeparam>
        /// <typeparam name="T10"></typeparam>
        /// <typeparam name="T11"></typeparam>
        /// <param name="transport"></param>
        /// <param name="methodName"></param>
        /// <param name="thriftOpt"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static TResult GetResult<ErrObj, TResult, T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(TTransport transport, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> thriftOpt, string methodName, bool isAppearErr, params object[] param)
          where TResult : new()
          where ErrObj : class
        {
            TResult t = new TResult();

            try
            {
                if (!transport.IsOpen) transport.Open();
                t = thriftOpt.Invoke(
                    (T1)param[0],
                    (T2)param[1],
                    (T3)param[2],
                    (T4)param[3],
                    (T5)param[4],
                    (T6)param[5],
                    (T7)param[6],
                    (T8)param[7],
                    (T9)param[8],
                    (T10)param[9],
                    (T11)param[10]);
            }
            catch (Exception ex)
            {
                Logger<ErrObj>.Log.Error(methodName, ex);
                //测试环境下给予提示
                if (isAppearErr)
                {
                    if ("DEBUG".Equals(GlobalCache.AppMode))
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                    else
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, methodName);
                }
            }
            finally
            {
                transport.Close();
            }

            return t;
        }
    }
}
