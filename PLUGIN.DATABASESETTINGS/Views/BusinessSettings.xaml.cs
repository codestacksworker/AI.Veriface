using System;
using System.Threading.Tasks;
using System.Windows;

namespace PLUGIN.DATABASESETTINGS.Views
{
    /// <summary>
    /// Interaction logic for BusinessSettings.xaml
    /// </summary>
    public partial class BusinessSettings : Window
    {
        public BusinessSettings()
        {
            InitializeComponent();
            MouseLeftButtonDown += Move_MouseLeftButtonDown;
        }

        private void Move_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch (Exception)
            {
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            isClose.Visibility = Visibility.Collapsed;
            this.Hide();
            DelayCloseWindow();
        }

        async void DelayCloseWindow()
        {
            await Task.Delay(TimeSpan.FromSeconds(1.5));
            this.Close();
        }
    }
}
