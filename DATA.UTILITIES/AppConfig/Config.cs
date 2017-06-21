using DATA.MODELS.GlobalModels;
using DATA.MODELS.SensingModels;
using DATA.UTILITIES.Log4Net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using xiaowen.codestacks.popwindow;

namespace DATA.UTILITIES.AppConfig
{
    public class Config
    {
        /// <summary>
        /// read app.config file
        /// setting basic data
        /// </summary>
        public static int ReadAppDotConfig
        {
            get { return ReadAppCoinfigFromAppSettings(); }
        }

        /// <summary>
        /// App Startup Load Content
        /// </summary>
        /// <returns></returns>
        static int ReadAppCoinfigFromAppSettings()
        {
            int res = -1;
            try
            {
                GlobalCache.AppTitle =
               ConfigurationManager.AppSettings["AppTitle"] ?? "深 醒 动 态 人 脸 识 别 系 统";
                GlobalCache.AppType =
                    Convert.ToUInt16(ConfigurationManager.AppSettings["AppFuncation"] ?? "0");
                GlobalCache.AppRegion =
                    Convert.ToUInt16(ConfigurationManager.AppSettings["Region"] ?? "5");
                GlobalCache.AppVersion =
                    ConfigurationManager.AppSettings["AppVersion"];
                GlobalCache.AppMode =
                    ConfigurationManager.AppSettings["AppMode"];
                GlobalCache.Func_AlarmDoubleClick =
                    Convert.ToBoolean(ConfigurationManager.AppSettings["Func-AlarmDoubleClick"]);
                GlobalCache.Func_AutoSignin =
                    Convert.ToBoolean(ConfigurationManager.AppSettings["Func-AutoSignin"]);
                GlobalCache.NetworkMode =
                    ConfigurationManager.AppSettings["NetworkMode"] ?? "LAN";
                GlobalCache.SelectedTimeout =
                    Convert.ToInt64(ConfigurationManager.AppSettings["SelectedTimeout"] ?? "15");
                GlobalCache.SelectTimeout =
                    Convert.ToInt64(ConfigurationManager.AppSettings["SelectTimeout"] ?? "12");
                GlobalCache.AppearLimited =
                    Convert.ToInt32(ConfigurationManager.AppSettings["AutoPushTheLimited"] ?? "1");
                GlobalCache.Longitude =
                    Convert.ToDouble(ConfigurationManager.AppSettings["Longitude"] ?? "116.398086547852");
                GlobalCache.Latitude =
                    Convert.ToDouble(ConfigurationManager.AppSettings["Latitude"] ?? "39.9163196138081");
                GlobalCache.AudioName = ConfigurationManager.AppSettings["AudioName"];
                GlobalCache.TSocketTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["TSocketTimeout"] ?? "5");
                GlobalCache.AppFunction =
                   ConfigurationManager.AppSettings["AppFunction"];


                List<ConfigRegion> regionList = new List<ConfigRegion>();
                if (ReadAreaInfo(ref regionList) == 200)
                {
                    GlobalCache.AreaInfoCollection = regionList;
                }

                res = 1;
            }
            catch (Exception ex)
            {
                Logger<Config>.Log.Error("ReadAppCoinfigFromAppSettings", ex);
            }

            return res;
        }

        /// <summary>
        /// Read userInfo
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns>200正常返回 404异常</returns>
        public static int ReadUserInfo(ref string ip, ref string port)
        {
            int res;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + @"\AppConfig\userinfo.xml");
                }
                catch (Exception)
                {
                    return 404;
                }
                XmlNodeList ipAddressNode = xmlDoc.GetElementsByTagName("IpAdress");
                XmlNodeList ipNode = ipAddressNode[0].ChildNodes;

                //获得第一个子节点,得到抓拍ID
                ip = ipNode[0].InnerText.ToString();
                port = ipNode[1].InnerText.ToString();
                res = 200;
            }
            catch (Exception)
            {
                res = 404;
                CodeStacksWindow.MessageBox.Invoke(false, false, 2, "读userinof.xml文件出错，\n请检查各节点是否书写正确");
            }
            return res;
        }

        /// <summary>
        /// save userinfo 
        /// 200-success 404-error
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <returns>200-success 404-error</returns>
        public static int SaveUserInfo(string ip, string port)
        {
            int res;
            try
            {
                string path = Path.Combine(Environment.CurrentDirectory + @"\AppConfig\userinfo.xml");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                XmlNode root = xmlDoc.SelectSingleNode("Root");
                XmlNodeList ipAddressNode = xmlDoc.GetElementsByTagName("IpAdress");
                for (int i = 0; i < ipAddressNode.Count; i++)
                {
                    root.RemoveChild(ipAddressNode[i]);
                }

                XmlElement nodeIpRoot = xmlDoc.CreateElement("IpAdress");

                XmlElement ipNode = xmlDoc.CreateElement("IP");
                ipNode.InnerText = ip.Trim();
                nodeIpRoot.AppendChild(ipNode);

                XmlElement portNode = xmlDoc.CreateElement("Port");
                portNode.InnerText = port.Trim();
                nodeIpRoot.AppendChild(portNode);

                root.AppendChild(nodeIpRoot);

                xmlDoc.Save(path);

                res = 200;
            }
            catch (Exception ex)
            {
                res = 404;
                Logger<Config>.Log.Error("DATA.UTILITIES.AppConfig.Config.【SaveUserInfo】", ex);
            }

            return res;
        }


        /// <summary>
        /// 505-xml is not found
        /// 404 error exception
        /// 200 success execute
        /// </summary>
        /// <returns></returns>
        static int ReadAreaInfo(ref List<ConfigRegion> regionList)
        {
            int res;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                try
                {
                    string path = AppDomain.CurrentDomain.BaseDirectory + @"\AppConfig\AreaInfo.xml";
                    xmlDoc.Load(path);
                }
                catch (Exception)
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "AreaInfo.xml中出现语法错误，请检查后重启");
                    return 505;
                }

                XmlNode rootNode = xmlDoc.SelectSingleNode("xml");
                XmlNodeList dataObject = rootNode.ChildNodes;
                foreach (XmlNode node in dataObject)
                {
                    ConfigRegion region = new ConfigRegion();
                    region.Hosts = new List<string>();
                    XmlElement xe = (XmlElement)node;
                    XmlNodeList xnl0 = xe.ChildNodes;
                    region.RegionNO = Convert.ToInt32(xnl0.Item(0).InnerText);
                    if (region.RegionNO.Equals(GlobalCache.AppRegion))//
                    {
                        region.RegionName = xnl0.Item(1).InnerText;
                        XmlNodeList xml02 = xnl0.Item(2).ChildNodes;

                        foreach (XmlNode receiveIP in xml02)
                        {
                            string IPAddressFormartRegex =
                                @"^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$";
                            //检查输入的字符串是否符合IP地址格式
                            if (Regex.IsMatch(receiveIP.InnerText, IPAddressFormartRegex))
                            {
                                region.Hosts.Add(receiveIP.InnerText);
                            }
                        }
                        region.AlarmNO = Convert.ToInt32(xnl0.Item(3).InnerText);
                        region.Threshold = Convert.ToInt32(xnl0.Item(4).InnerText);
                        regionList.Add(region);
                    }
                }
                res = 200;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                res = 404;
            }
            return res;
        }

    }
}
