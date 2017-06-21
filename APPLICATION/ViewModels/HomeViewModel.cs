using DATA.MODELS.GlobalModels;
using FaceSysByMvvm.Views.ChannelManager;
using PLUGIN.DATABASESETTINGS.Views;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System;

namespace SENSING.APPLICATION.ViewModels
{
    public partial class HomeViewModel : BindableBase
    {
        public HomeViewModel()
        {
            initCmd();
        }

        #region CMD

        public ICommand CmdBusinessSettings { get; private set; }
        public ICommand CmdOpenAlarm { get; set; }

       
        void initCmd()
        {
            CmdBusinessSettings = new DelegateCommand<object>(CmdBusinessSettingsFunc);
            CmdOpenAlarm = new DelegateCommand<object>(CmdOpenAlarmFunc);
            
        }


        private void CmdOpenAlarmFunc(object obj)
        {
            List<Window> windows = new List<Window>();
            foreach (Window window in Application.Current.Windows)
            {
                windows.Add(window);
            }

            if (windows.FirstOrDefault(x => x.Name == "AlarmWindow") == null)
            {
                WarningMessageWindow alarm = new WarningMessageWindow();                
                alarm.Show();
            }
            else
            {
                windows.FirstOrDefault(x => x.Name == "AlarmWindow").Show();
            }
        }

        private void CmdBusinessSettingsFunc(object obj)
        {
            BusinessSettings settings = new BusinessSettings();
            settings.Show();
        }

        #endregion

        #region Properties

        string _appName;
        public string AppName
        {
            get { return _appName = GlobalCache.AppTitle; }
            private set { SetProperty(ref _appName, value); }
        }

        Visibility _isVisibility = Visibility.Visible;
        public Visibility IsVisibility
        {
            get
            {
                if (GlobalCache.AppType == 1)
                {
                    _isVisibility = Visibility.Collapsed;
                }
                return
                  _isVisibility;
            }
            set { SetProperty(ref _isVisibility, value); }
        }


        #endregion
    }
}
