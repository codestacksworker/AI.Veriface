using System;
using System.Collections.Generic;
using System.Text;


public class TemplateTypeConvert
{
    const string General = "普通";
    const string Black = "黑名单";
    const string Null = "无";


    public static string GetTemplateType(int type, params string[] typeStr)
    {
        string res = string.Empty;
        switch (type)
        {
            case 1:
                res = Null;
                break;
            case 2:
                res = General;
                break;
            case 3:
                res = Black;
                break;
        }
        return res;
    }

}
