using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Newtonsoft.Json;

namespace AppConfigModel
{
    public class EasyConfig
    {
        public static IEnumerable AreaInfoConfig
        {
            get
            {
                return GetConfigFile();
            }
        }

        public static string AreaConfigContent
        {
            get { return GetAreaContent(); }
        }

        static IEnumerable GetConfigFile()
        {
            IEnumerable area = null;
            try
            {
                string appDomain = AppDomain.CurrentDomain.BaseDirectory;
                string areaFile = AppConfig.Instance.Areaconfig;
                string path = appDomain + areaFile;

                if (File.Exists(path))
                {
                    string jsonText = File.ReadAllText(path);
                    area = JsonConvert.DeserializeObject<List<AreaInfo>>(jsonText);
                }
            }
            catch (Exception ex)
            {
                area = new List<AreaInfo>();
                string err = ex.Message;
            }
            return area;
        }

        static string GetAreaContent()
        {
            string content = string.Empty;
            try
            {
                string appDomain = AppDomain.CurrentDomain.BaseDirectory;
                string areaFile = AppConfig.Instance.Areaconfig;
                string path = appDomain + areaFile;

                if (File.Exists(path))
                {
                    content = File.ReadAllText(path);
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
            return content;
        }


        static string GetPortContent()
        {
            string content = string.Empty;
            try
            {
                string appDomain = AppDomain.CurrentDomain.BaseDirectory;
                string areaFile = AppConfig.Instance.Portconfig;
                string path = appDomain + areaFile;

                if (File.Exists(path))
                {
                    content = File.ReadAllText(path);
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
            return content;
        }

    }
}
