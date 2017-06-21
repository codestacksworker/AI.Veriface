using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;

namespace AppConfigModel
{
    public class AppConfig
    {
        static List<ConfigJsonItem> jsonOperation;
        public static List<ConfigJsonItem> JsonOperation
        {
            get
            {
                return jsonOperation;
            }

            set
            {
                jsonOperation = value;
            }
        }

        static AppSettings _appSettings;
        public static AppSettings Instance
        {
            get
            {
                _appSettings = new AppSettings();
                GetAppSettingsFromJsonOperateList(JsonOperation);
                //GetAppSettings();
                return _appSettings;
            }
            private set { _appSettings = value; }
        }

        public static void GetAppSettingsFromJsonOperateList(List<ConfigJsonItem> list)
        {
            if (list != null)
            {
                foreach (ConfigJsonItem item in list)
                {
                    if (item.Key == "AppConfig")
                        _appSettings.AppConfig = item.Value;
                    if (item.Key == "AppTitle")
                        _appSettings.AppTitle = item.Value;
                    if (item.Key == "AppVersion")
                        _appSettings.AppVersion = item.Value;
                    if (item.Key == "Threshold")
                        _appSettings.Threshold = item.Value;
                    if (item.Key == "AutoSingin")
                        _appSettings.AutoSingin = item.Value;
                    if (item.Key == "AppFuncation")
                        _appSettings.AppFuncation = item.Value;
                    if (item.Key == "Region")
                        _appSettings.Region = item.Value;
                    if (item.Key == "RevicedClientIP")
                        _appSettings.RevicedClientIP = item.Value;
                    if (item.Key == "ExcelSavePath")
                        _appSettings.ExcelSavePath = item.Value;
                    if (item.Key == "FaceUUID")
                        _appSettings.FaceUUID = item.Value;
                    if (item.Key == "Name")
                        _appSettings.Name = item.Value;
                    if (item.Key == "Type")
                        _appSettings.Type = item.Value;
                    if (item.Key == "Areaconfig")
                        _appSettings.Areaconfig = item.Value;
                    if (item.Key == "Portconfig")
                        _appSettings.Portconfig = item.Value;
                    if (item.Key == "RealTimeCaptureconfig")
                        _appSettings.RealTimeCaptureconfig = item.Value;
                }
            }
        }

        public static List<ConfigJsonItem> SetJsonOperateListFromAppSetting(AppSettings app)
        {
            List<ConfigJsonItem> list = new List<ConfigJsonItem>();
            if (app != null)
            {
                foreach (ConfigJsonItem item in JsonOperation)
                {
                    if (item.Key == "AppConfig")
                        item.Value = app.AppConfig;
                    if (item.Key == "AppTitle")
                        item.Value = app.AppTitle;
                    if (item.Key == "AppVersion")
                        item.Value = app.AppVersion;
                    if (item.Key == "Threshold")
                        item.Value = app.Threshold;
                    if (item.Key == "AutoSingin")
                        item.Value = app.AutoSingin;
                    if (item.Key == "AppFuncation")
                        item.Value = app.AppFuncation;
                    if (item.Key == "Region")
                        item.Value = app.Region;
                    if (item.Key == "RevicedClientIP")
                        item.Value = app.RevicedClientIP;
                    if (item.Key == "ExcelSavePath")
                        item.Value = app.ExcelSavePath;
                    if (item.Key == "FaceUUID")
                        item.Value = app.FaceUUID;
                    if (item.Key == "Name")
                        item.Value = app.Name;
                    if (item.Key == "Type")
                        item.Value = app.Type;
                    if (item.Key == "Areaconfig")
                        item.Value = app.Areaconfig;
                    if (item.Key == "Portconfig")
                        item.Value = app.Portconfig;
                    if (item.Key == "RealTimeCaptureconfig")
                        item.Value = app.RealTimeCaptureconfig;
                    list.Add(item);
                }
            }
            return list;
        }

        public static void GetAppSettings()
        {
            NameValueCollection nameValueCollection = System.Configuration.ConfigurationManager.AppSettings;

            _appSettings.AppConfig = nameValueCollection.Get("AppConfig");
            _appSettings.AppTitle = nameValueCollection.Get("AppTitle");
            _appSettings.AppVersion = nameValueCollection.Get("AppVersion");
            _appSettings.Threshold = nameValueCollection.Get("Threshold");
            _appSettings.AutoSingin = nameValueCollection.Get("AutoSingin");
            _appSettings.AppFuncation = nameValueCollection.Get("AppFuncation");
            _appSettings.Region = nameValueCollection.Get("Region");
            _appSettings.RevicedClientIP = nameValueCollection.Get("RevicedClientIP");
            _appSettings.ExcelSavePath = nameValueCollection.Get("ExcelSavePath");
            _appSettings.FaceUUID = nameValueCollection.Get("FaceUUID");
            _appSettings.Name = nameValueCollection.Get("Name");
            _appSettings.Type = nameValueCollection.Get("Type");

            _appSettings.Areaconfig = nameValueCollection.Get("Areaconfig");
        }

        public static int SetAppSettings(AppSettings settings)
        {
            int res = 1;
            try
            {
                Type type = settings.GetType();
                System.Configuration.Configuration cfg = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                foreach (PropertyInfo prop in type.GetProperties())
                {
                    cfg.AppSettings.Settings[prop.Name].Value = prop.ToString();
                }
            }
            catch (Exception ex)
            {
                res = -1;
                string err = ex.Message;
            }
            return res;
        }
    }
}
