using DATA.MODELS.SensingModels;
using GalaSoft.MvvmLight.Threading;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CONTROL.UTILITIES.ViewModels
{
    public class BusinessSettingsViewModel : BindableBase
    {
        /**
         * 设计方案：
         * 无按钮操作保存
         * 设计缺陷：无Cache
         * 
         * 优点：即时存储更新内容
         * 
         * **/

        /// <summary>
        /// /
        /// </summary>
        public BusinessSettingsViewModel()
        {
            TbConfigInfo = new DatabaseSettings();
            GetSettingsList(1);

            SelectedItemCmd = new DelegateCommand<object>(SelectedItemCmdFunc);
            SaveCmd = new DelegateCommand<object>(SaveCmdFunc);
            AvailableSettingsContentCmd = new DelegateCommand<object>(AvailableSettingsContentCmdFunc);
            AllSettingsContentCmd = new DelegateCommand<object>(AllSettingsContentCmdFunc);
        }

        /// <summary>
        /// display all
        /// </summary>
        /// <param name="obj"></param>
        private void AllSettingsContentCmdFunc(object obj)
        {
            GetSettingsList(-1);
        }

        /// <summary>
        /// a few
        /// </summary>
        /// <param name="obj"></param>
        private void AvailableSettingsContentCmdFunc(object obj)
        {
            GetSettingsList(1);//
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void SaveCmdFunc(object obj)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void SelectedItemCmdFunc(object obj)
        {
            TbConfig config = obj as TbConfig;

            var tbconfig = TbConfigInfo.SettingsCollection.FirstOrDefault(x => x.Key.Equals(this.Key));
            if (!string.IsNullOrEmpty(this.Value) && !tbconfig.Value.Equals(this.Value))
            {
                SetSettingsValue(new TbConfig { Key = this.Key, Value = this.Value, Mean = tbconfig.Mean });
            }

            this.Key = config.Key;
            this.Value = config.Value;
        }

        #region CMD

        public ICommand SelectedItemCmd { get; private set; }
        public ICommand SaveCmd { get; private set; }

        public ICommand AvailableSettingsContentCmd { get; set; }
        public ICommand AllSettingsContentCmd { get; set; }

        #endregion

        #region Properties

        string _key;
        public string Key
        {
            get { return _key; }
            set { SetProperty(ref _key, value); }
        }

        string _value;
        public string Value
        {
            get
            {
                if (string.IsNullOrEmpty(_value))
                    return _value;
                else
                    return _value.Trim();
            }
            set { SetProperty(ref _value, value); }
        }


        Visibility _isClose;
        public Visibility IsClose
        {
            get
            {
                if (_isClose == Visibility.Collapsed)
                {
                    SelectedItemCmdFunc(TbConfigInfo.SettingsItem);
                }
                return _isClose;
            }
            set { SetProperty(ref _isClose, value); }
        }

        DatabaseSettings _tbConfigInfo;
        public DatabaseSettings TbConfigInfo
        {
            get { return _tbConfigInfo; }
            set { SetProperty(ref _tbConfigInfo, value); }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        async void GetSettingsList(int i)
        {
            TbConfigInfo.SettingsCollection = new List<TbConfig>();
            TbConfigInfo.SettingsItem = new TbConfig();
            List<SConfigInfo> result = null;
            THRIFTSERVICES.Services.ThriftServiceUtilities thrift = new THRIFTSERVICES.Services.ThriftServiceUtilities();
            await Task.Run(() =>
            {
                try
                {
                    result = thrift.ConfigByGet(i);//数据库表中stype字段为1，表示可以配置的内容
                    foreach (var item in result)
                    {
                        TbConfigInfo.SettingsCollection.Add(new TbConfig
                        {
                            Key = item.Key,
                            Value = item.Value,
                            Mean = item.Describe
                        });
                    }
                    try
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            if (TbConfigInfo.SettingsCollection.Count > 0)
                            {
                                this.Key = TbConfigInfo.SettingsCollection[0].Key;
                                this.Value = TbConfigInfo.SettingsCollection[0].Value;
                            }
                        });
                    }
                    catch (Exception)
                    {
                    }

                }
                catch (Exception)
                {
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="updateContent"></param>
        async void SetSettingsValue(TbConfig updateContent)
        {
            List<ErrorInfo> result = null;
            List<SConfigInfo> content = new List<SConfigInfo>();
            THRIFTSERVICES.Services.ThriftServiceUtilities thrift = new THRIFTSERVICES.Services.ThriftServiceUtilities();
            await Task.Run(() =>
            {
                content.Add(new SConfigInfo
                {
                    Key = updateContent.Key,
                    Value = updateContent.Value,
                    Describe = updateContent.Mean
                });
                result = thrift.ConfigBySet(null, content).Result;
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    TbConfigInfo.SettingsCollection.FirstOrDefault(x => x.Key == updateContent.Key).Value = updateContent.Value;
                });
            });
        }

    }
}
