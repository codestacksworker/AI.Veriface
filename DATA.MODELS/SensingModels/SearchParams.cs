using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.MODELS.SensingModels
{
    public class SearchParams
    {

    }

    /// <summary>
    /// Search Conditions for Date
    /// </summary>
    public class SearchParamsDateTime
    {
        public string Date { get; set; }
        public List<string> Hours { get; set; }
        public List<int> Minutes { get; set; }
        int _hourKey;
        public int HourKey
        {
            get { return _hourKey; }
            set
            {
                Minute = 0;
                _hourKey = value;
            }
        }
        string _hourVal;
        public string HourVal
        {
            get { return _hourVal; }
            set
            {
                Minute = 0;
                _hourVal = value;
            }
        }
        public int Minute { get; set; }

        /// <summary>
        /// 初始化日期
        /// 初始化时间
        /// 初始化分钟
        /// </summary>
        public SearchParamsDateTime()
        {
            Date = new DateTime().ToString();
            for (int i = 0; i <= 59; i++)
            {
                if (i >= 1 && i <= 24)
                {
                    Hours.Add(i + ":00");
                }
                Minutes.Add(i);
            }
        }
    }

}
