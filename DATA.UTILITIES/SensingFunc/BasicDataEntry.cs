using DATA.MODELS.GlobalModels;
using System;
using System.Linq;

namespace DATA.UTILITIES.SensingFunc
{
    public class BasicDataEntry
    {
        public static string GetTemplateType(int type, params string[] typeStr)
        {
            string res = string.Empty;
            try
            {
                res = GlobalCache.FaceTypeList.Cast<string>().ToList()[type];
            }
            catch (Exception)
            {
            }
            return res;
        }

        public static int GetTemplateTypeIndex(string name, params string[] typeStr)
        {
            int res = -1;
            try
            {
                res = GlobalCache.FaceTypeList.Cast<string>().ToList().IndexOf(name.Replace("@", ""));
            }
            catch (Exception)
            {
            }
            return res;
        }
    }
}
