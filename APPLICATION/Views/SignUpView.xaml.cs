using DATA.MODELS.GlobalModels;
using DATA.UTILITIES.Log4Net;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace SENSING.APPLICATION.Views
{
    /// <summary>
    /// Interaction logic for SignUpView.xaml
    /// </summary>
    public partial class SignUpView : Window
    {
        public SignUpView()
        {
            InitializeComponent();
            this.Name = "SignUpView";
            version.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        private void Opt_EditSettings(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GlobalCache.ChildWindowStatus == -1)
                {
                    SETTINGS_MODULES.Views.MainWindow settings = new SETTINGS_MODULES.Views.MainWindow();
                    settings.Show();
                }
            }
            catch (Exception ex)
            {
                Logger<SignUpView>.Log.Error("Opt_EditSettings", ex);
            }
        }
        
        private void Opt_CloseWindow(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (Key.Enter == e.Key)
            {
                this.port.Focus();
                this.ip.Focus();
                isEnterKeyUp.IsEnabled = true;
                this.ip.Focus();
            }
        }
    }
}
