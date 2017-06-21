using DATA.MODELS.GlobalModels;
using GMap.NET;
using SENSING.Plugin.Intelligent.ViewModels;
using System.Windows.Controls;
using xiaowen.codestacks.gmap.demo.Models;

namespace SENSING.Plugin.Intelligent.Views
{
    /// <summary>
    /// Interaction logic for NoTarget.xaml
    /// </summary>
    public partial class NoTargetControl : UserControl
    {
        public NoTargetControl()
        {
            InitializeComponent();
            MainControlViewModel.NoTargetControlContent = this;

            MainMap.Points = new System.Collections.ObjectModel.ObservableCollection<PointLatLng>();
            MainMap.Points.Add(new PointLatLng(
                lat: GlobalCache.Latitude, lng: GlobalCache.Longitude,
                cameraOrPhoto: string.Empty,
                photo: null, geoTitle: new GeoTitle()
                ));
        }
    }
}
