
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Windows.Input;
using AppConfigModel;
using DATA.UTILITIES.FileHandler;
using System;
using System.IO;

namespace SETTINGS_MODULES.ViewModels
{
    public partial class MainWindowViewModel : BindableBase
    {
        public ICommand SaveAppconfigCommand { get; private set; }

        public MainWindowViewModel()
        {
            SaveAppconfigCommand = new DelegateCommand<object>(SaveAppconfigCommandFunc);
            initJsonList();
            initAppConfigContent();

        }

        private void SaveAppconfigCommandFunc(object obj)
        {
            SaveJson();
            //AppConfigModel.AppConfig.SetAppSettings(AppConfigs);
        }

        string _title = "应用程序配置123";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        AppConfigModel.AppSettings _appSettings;
        public AppConfigModel.AppSettings AppConfigs
        {
            get { return _appSettings; }
            set { SetProperty(ref _appSettings, value); }
        }


        List<AppConfigModel.ConfigJsonItem> _jsonOperation;
        public List<ConfigJsonItem> JsonOperationList
        {
            get
            {
                return _jsonOperation;
            }

            set
            {
                _jsonOperation = value;
            }
        }
        void initJsonList()
        {
            string path = Path.Combine(Environment.CurrentDirectory + @"\AppConfig\appsettings.json");

            string srtjson = ReadJson.GetAppConfigJson(path);

            var json = (Root)Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(srtjson);
            JsonOperationList = json.ConfigJson;

            AppConfig.JsonOperation = JsonOperationList;
        }

        void SaveJson()
        {
            string path = Path.Combine(Environment.CurrentDirectory + @"\AppConfig\appsettings.json");
            JsonOperationList = AppConfig.SetJsonOperateListFromAppSetting(AppConfigs);
            Root jsonRoot = new Root();
            jsonRoot.ConfigJson = JsonOperationList;
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(jsonRoot);

            ReadJson.SetAppConfigJson(path, json.ToString());
        }
        void initAppConfigContent()
        {
            AppConfigs = new AppConfigModel.AppSettings();
            AppConfigs.AppConfig = AppConfigModel.AppConfig.Instance.AppConfig;
            AppConfigs.AppTitle = AppConfigModel.AppConfig.Instance.AppTitle;
            AppConfigs.AppVersion = AppConfigModel.AppConfig.Instance.AppVersion;
            AppConfigs.Threshold = AppConfigModel.AppConfig.Instance.Threshold;
            AppConfigs.AutoSingin = AppConfigModel.AppConfig.Instance.AutoSingin;
            AppConfigs.AppFuncation = AppConfigModel.AppConfig.Instance.AppFuncation;
            AppConfigs.Region = AppConfigModel.AppConfig.Instance.Region;
            AppConfigs.RevicedClientIP = AppConfigModel.AppConfig.Instance.RevicedClientIP;
            AppConfigs.ExcelSavePath = AppConfigModel.AppConfig.Instance.ExcelSavePath;
            AppConfigs.FaceUUID = AppConfigModel.AppConfig.Instance.FaceUUID;
            AppConfigs.Name = AppConfigModel.AppConfig.Instance.Name;
            AppConfigs.Type = AppConfigModel.AppConfig.Instance.Type;

            AppConfigs.Areaconfig = AppConfigModel.AppConfig.Instance.Areaconfig;

            AreaConfigContent = new AppConfigModel.ConfigContent();
            AreaConfigContent.AreaJsonText = new AppConfigModel.ConfigText();
            AreaConfigContent.AreaJsonText.JsonConfigText = AppConfigModel.EasyConfig.AreaConfigContent;

            //AreaConfigContent.PortJsonText.JsonConfigText = AppConfigModel.CameraArea.p
        }
    }
}
