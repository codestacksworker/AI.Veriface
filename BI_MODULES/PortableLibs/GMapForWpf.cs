using DATA.MODELS.GlobalModels;
using GMap.NET;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using xiaowen.codestacks.data;
using xiaowen.codestacks.gmap.demo.Models;

namespace BI_MODULES.PortableLibs
{
    public class GMapForWpf
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datasource"></param>
        /// <param name="photoBuffer"></param>
        /// <param name="str">不许为空</param>
        /// <returns></returns>
        public static ObservableCollection<PointLatLng> SetMapAnchor<T>(IEnumerable datasource, byte[] photoBuffer, params string[] str)
            where T : class, new()
        {
            T t = new T();
            ObservableCollection<PointLatLng> Points = new ObservableCollection<PointLatLng>();

            try
            {
                if (t is xiaowen.codestacks.data.SenSingModels.Camera)
                {
                    string _nametitle = string.Empty;
                    foreach (xiaowen.codestacks.data.SenSingModels.Camera item in datasource)
                    {
                        double lat = GlobalCache.Latitude;
                        double lng = GlobalCache.Longitude;
                        bool isLat = Double.TryParse(item.Latitude.ToString(), out lat);
                        bool isLng = Double.TryParse(item.Longitude.ToString(), out lng);

                        Points.Add(new PointLatLng(
                             lat: isLat ? lat : GlobalCache.Latitude,
                             lng: isLng ? lng : GlobalCache.Longitude,
                            cameraOrPhoto: "Photo",
                            photo: CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1.Invoke(photoBuffer),
                            geoTitle: new GeoTitle()
                            {
                                IsVisible = Visibility.Visible,
                                Header = "人员信息",
                                Content1 = str[0],
                                Content1Value = string.IsNullOrEmpty(item.Name) ? item.Name : item.Name,
                                Content1Visible = Visibility.Visible,
                                Content2 = "地址：",
                                Content2Value = item.Location,
                                Content2Visible = Visibility.Visible,
                                Content3 = "抓拍人数：",
                                Content3Value = item.SnapPeopleCount.ToString(),
                                Content3Visible = Visibility.Visible
                            }
                            ));
                    }
                }
            }
            catch (Exception)
            {
            }
            return Points;
        }


        /// <summary>
        /// new Uri("pack://application:,,,/Images/home-icon-bluecamera.png")
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="datasource"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static ObservableCollection<PointLatLng> SetMapAnchor<T>(IEnumerable datasource, Uri uri)
            where T : class, new()
        {
            T t = new T();
            ObservableCollection<PointLatLng> Points = new ObservableCollection<PointLatLng>();

            if (t is TargetedAnalysis)
            {
                try
                {
                    foreach (TargetedAnalysis item in datasource)
                    {
                        double lat = GlobalCache.Latitude;
                        double lng = GlobalCache.Longitude;
                        bool isLat = Double.TryParse(item.Latitude, out lat);
                        bool isLng = Double.TryParse(item.Longitude, out lng);

                        Points.Add(new PointLatLng(
                             lat: isLat ? lat : GlobalCache.Latitude,
                             lng: isLng ? lng : GlobalCache.Longitude,
                             cameraOrPhoto: "Camera",
                             photo: CodeStacksDataHandler.ImageData.ConvertToImageSourceDelegate1.Invoke(uri.ToString()),
                             geoTitle: new GeoTitle()
                             {
                                 IsVisible = Visibility.Visible,
                                 Header = "人员信息",
                                 Content1 = "通道：",
                                 Content1Value = item.Name,
                                 Content1Visible = Visibility.Visible,
                                 Content2 = "地址：",
                                 Content2Value = item.Channel_address,
                                 Content2Visible = Visibility.Visible,
                                 Content3 = "抓拍人数：",
                                 Content3Value = item.Count.ToString(),
                                 Content3Visible = Visibility.Visible
                             }
                             ));
                    }
                }
                catch (Exception)
                {
                }
            }
            return Points;
        }
    }
}
