using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.UTILITIES.FileHandler
{
    public class ReadJson
    {
        /// <summary>
        /// 获取Json字符串
        /// </summary>
        public static string GetAppConfigJson(string strPath)
        {
            string strJson = string.Empty;
            try
            {
                StreamReader sr = new StreamReader(strPath, Encoding.GetEncoding("utf-8"));
                while (!sr.EndOfStream)
                {
                    strJson += sr.ReadLine();
                    //文本处理
                }
                sr.Close();
            }
            catch (Exception)
            {
            }
            return strJson;
        }

        public static void SetAppConfigJson(string strPath, string strJson)
        {
            try
            {
                StreamWriter sw = new StreamWriter(strPath,false, Encoding.UTF8);
                sw.WriteLine(strJson);
                sw.Close();
            }
            catch (Exception)
            {
            }
        }
    }
}
