using System;
using System.Windows;
using DATA.MODELS.GlobalModels;
using DATA.UTILITIES.Log4Net;
using System.Collections.Generic;
using DATA.UTILITIES.AppConfig;
using GalaSoft.MvvmLight.Threading;
using xiaowen.codestacks.popwindow;
using xiaowen.codestacks.data;
using System.Diagnostics;

namespace APPLICATION
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                Logger<App>.InitLogger();
                Logger<App>.Log.Info("程序启动");
                GlobalCache.ChildWindowStatus = -1;

                int readAppConfigRes = Config.ReadAppDotConfig;
                GlobalCache.AlarmCamera = new List<string>();

                this.DispatcherUnhandledException += App_DispatcherUnhandledException;

                DispatcherHelper.Initialize();
                CodeStacksDataHandler.InitUIThread();
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                CodeStacksWindow.MessageBox.Invoke(false, false, 2, "exe.config配置出错，请检查配置内容");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            //Occurs when the user ends the windows session by logging off or shutting down the operating system
            string msg = string.Format("{0}.结束会话？", e.ReasonSessionEnding);
            bool result = CodeStacksWindow.MessageBox.Invoke(true, false, 0, msg);
            if (!result)
                e.Cancel = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //Prevent deault unhandled exception processing
            //remain app contiune running
            e.Handled = true;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                Process.GetCurrentProcess().Kill();
                Environment.Exit(-1);
            }
            catch (Exception)
            {
            }
        }
    }
}
