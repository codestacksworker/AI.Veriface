using DATA.MODELS.GlobalModels;
using DATA.UTILITIES.AppConfig;
using DATA.UTILITIES.Log4Net;
using FaceSysByMvvm.Common;
using GalaSoft.MvvmLight.Threading;
using Prism.Commands;
using Prism.Mvvm;
using SENSING.APPLICATION.Views;
using SENSING.THRIFT.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using xiaowen.codestacks.popwindow;

namespace SENSING.APPLICATION.ViewModels
{
    public partial class SignUpViewModel : BindableBase
    {
        public SignUpViewModel()
        {
            LoadingVisiblity = Visibility.Collapsed;
            initCmd();
            initUserInfo();
        }

        #region CMD

        public ICommand SignUpCmd { get; set; }

        void initCmd()
        {
            SignUpCmd = new DelegateCommand<object>(SignUpCmdFunc);
        }

        void initUserInfo()
        {
            string ip = string.Empty, port = string.Empty;
            Config.ReadUserInfo(ref ip, ref port);
            Host = ip;
            Port = port;

            IsEnterKeyUp = false;//允许EnterKey进行登录操作
        }

        DispatcherTimer autoLogin = new DispatcherTimer();//自动登录
        private void SignUpCmdFunc(object obj)
        {
            AsyncSignUpFunc();
        }
        #endregion

        #region Async Funcation

        void AsyncSignUpFunc()
        {
            try
            {
                if (string.IsNullOrEmpty(Host) || string.IsNullOrEmpty(Port))
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "IP和端口不能为空");
                    return;
                }

                string ip = Host, port = Port;

                int result = ThriftServiceBasic.Singin(ref ip, ref port);
                if (result == 200)
                {
                    if (GlobalCache.Func_AutoSignin) autoLogin.Stop();//auto singin funcation

                    if (Config.SaveUserInfo(Host, Port) == 200)//save ip&port
                    {
                        AppConfigs.AsyncSelectFaceType();
                        var config = GlobalCache.AreaInfoCollection.FirstOrDefault(region => region.RegionNO == GlobalCache.AppRegion);
                        if (config != null)
                        {
                            try
                            {
                                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                                {
                                    AsyncQueryTemplateProperty();//to query template type
                                    GlobalCache.AppLocation = config.RegionName;
                                    HomeView home = new HomeView();
                                    CurrentWindow.Close();
                                    home.Show();
                                });
                            }
                            catch (Exception ex)
                            {
                                string err = ex.Message;
                                Logger<SignUpViewModel>.Log.Error("SignUp_Click", ex);
                            }
                        }
                        else
                        {
                            CodeStacksWindow.MessageBox.Invoke(false, false, 2, "exe.config文件与AreaInfo.xml文件中的区域类型不一致");
                            return;
                        }
                    }
                    else
                    {
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, "请您先检查userinfo.xml文件是否存在，若存在，\n请联系技术人员处理");
                    }
                }
                else if (result == 404)
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, string.Format("无法连接到 {0}:{1}", ip, port));
                }
            }
            catch (Exception ex)
            {
                CodeStacksWindow.MessageBox.Invoke(false, false, 2, "应用程序异常，请联系技术人员");
                Logger<SignUpViewModel>.Log.Error("SignUp_Click", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        async void AsyncQueryTemplateProperty()
        {
            await Task.Run(() =>
            {
                GlobalCache.ChannePropertiesList = ThriftServiceBasic.SelectChannelPropertiesList();
            });
        }

        #endregion

        #region Properties

        Window _currentWindow;
        public Window CurrentWindow
        {
            get
            {
                _currentWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(x => x.Name == "SignUpView");
                return _currentWindow;
            }
            set { SetProperty(ref _currentWindow, value); }
        }

        Visibility _loadingVisiblity;
        public Visibility LoadingVisiblity
        {
            get { return _loadingVisiblity; }
            set { SetProperty(ref _loadingVisiblity, value); }
        }

        string _host;
        public string Host
        {
            get { return _host; }
            set { SetProperty(ref _host, value); }
        }

        string _port;
        public string Port
        {
            get { return _port; }
            set { SetProperty(ref _port, value); }
        }

        bool _isEnterKeyUp;
        public bool IsEnterKeyUp
        {
            get
            {
                if (_isEnterKeyUp)
                    AsyncSignUpFunc();

                return
                  _isEnterKeyUp;
            }
            set { SetProperty(ref _isEnterKeyUp, value); }
        }

        #endregion
    }
}
