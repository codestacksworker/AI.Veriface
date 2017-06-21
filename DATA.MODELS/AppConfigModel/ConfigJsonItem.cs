using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConfigModel
{
    public class ConfigJsonItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 应用程序标题：
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Describe { get; set; }
        /// <summary>
        /// 深 醒 动 态 人 脸 识 别 系 统
        /// </summary>
        public string Value { get; set; }
    }

    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ConfigJsonItem> ConfigJson { get; set; }
    }

}
