using AppConfigModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SETTINGS_MODULES.Views
{
    /// <summary>
    /// Interaction logic for AppConfigControl.xaml
    /// </summary>
    public partial class AppConfigControl : UserControl
    {
        public AppConfigControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CreateControl();
        }

        private void CreateControl()
        {
            //AppConfig.JsonOperation
            if (AppConfig.JsonOperation != null)
            {
                for (int i=0;i< AppConfig.JsonOperation.Count;i++)
                {
                    ConfigJsonItem item = AppConfig.JsonOperation[i];

                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(30) });

                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength() });

                    Label lbl = new Label();
                    lbl.Content = item.Title;
                    Grid.SetColumn(lbl, 0);
                    Grid.SetRow(lbl, i);

                    TextBox txt = new TextBox();
                    Binding bind = new Binding("AppConfigs."+item.Key);
                    //设置数据流的方向
                    bind.Mode = BindingMode.TwoWay;
                    //设置源属性
                    //binding.Path = new PropertyPath("Value");
                    //进行数据绑定
                    //txt.SetBinding(TextBlock.FontSizeProperty, binding);
                    //第二中绑定方式
                    BindingOperations.SetBinding(txt, TextBox.TextProperty, bind);
                    Grid.SetColumn(txt, 1);
                    Grid.SetRow(txt, i);

                    Label lbl2 = new Label();
                    if(item.Describe!=string.Empty)
                    {
                        lbl2.Content = "(" + item.Describe +")";
                    }
                    Grid.SetColumn(lbl2, 2);
                    Grid.SetRow(lbl2, i);

                    grid.Children.Add(lbl);
                    grid.Children.Add(txt);
                    grid.Children.Add(lbl2);
                }
            }
        }

    }
}
