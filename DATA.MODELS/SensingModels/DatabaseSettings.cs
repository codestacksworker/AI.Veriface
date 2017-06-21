using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATA.MODELS.SensingModels
{
    public class DatabaseSettings
    {
        public int Id { get; set; }
        public string Guid { get; set; }
        public TbConfig SettingsItem { get; set; }
        public ObservableCollection<TbConfig> SettingsCollection { get; set; }
    }



    public class TbConfig
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public int Type { get; set; }
        public string Mean { get; set; }
    }
}
