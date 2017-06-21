using SENSING.Plugin.Intelligent.ViewModels;
using System.Windows.Controls;

namespace SENSING.Plugin.Intelligent.Views
{
    /// <summary>
    /// Interaction logic for TargetControl.xaml
    /// </summary>
    public partial class TargetControl : UserControl
    {
        public TargetControl()
        {
            InitializeComponent();
            MainControlViewModel.TargetControlContent = this;

            //MainMap.Points = new System.Collections.ObjectModel.ObservableCollection<GMap.NET.PointLatLng>();
            //MainMap.Points.Add(new PointLatLng(
            //    lat:GlobalCache.Latitude,lng:GlobalCache.Longitude,
            //    cameraOrPhoto:string.Empty,photo:null,geoTitle:new GeoTitle()
            //    ));
        }
    }
}
