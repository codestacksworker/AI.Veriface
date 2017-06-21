using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace SC_MODULES.Views
{
    /// <summary>
    /// Interaction logic for SearchSchemaControl.xaml
    /// </summary>
    public partial class SearchSchemaControl : UserControl
    {
        public SearchSchemaControl()
        {
            InitializeComponent();
        }

        private void ClearDatePickerTime(object sender, MouseButtonEventArgs e)
        {

        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "图片|*.jpg;*.png;*.bmp;*.jpeg";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                image.Source = new BitmapImage(new Uri(filename));
                MessageBox.Show("111111");
            }
        }

        private void selectDatas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.selectDatas.SelectedIndex == 0)
            {
                this.dStar.Visibility = Visibility.Collapsed;
                this.hStar.Visibility = Visibility.Collapsed;
                this.mStar.Visibility = Visibility.Collapsed;
                this.dEnd.Visibility = Visibility.Collapsed;
                this.hEnd.Visibility = Visibility.Collapsed;
                this.mEnd.Visibility = Visibility.Collapsed;
            }
            else
            {
                this.dStar.Visibility = Visibility.Visible;
                this.hStar.Visibility = Visibility.Visible;
                this.mStar.Visibility = Visibility.Visible;
                this.dEnd.Visibility = Visibility.Visible;
                this.hEnd.Visibility = Visibility.Visible;
                this.mEnd.Visibility = Visibility.Visible;
            }
        }
    }
}
