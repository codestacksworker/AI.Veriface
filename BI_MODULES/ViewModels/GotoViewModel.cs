using DATA.MODELS.GlobalModels;
using DATA.UTILITIES.Log4Net;
using FaceSysByMvvm.Model;
using GMap.NET;
using PeopleModel;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using xiaowen.codestacks.data;
using xiaowen.codestacks.gmap.demo.Models;

namespace SENSING.Plugin.Intelligent.ViewModels
{
    public partial class MainControlViewModel : BindableBase
    {
        /// <summary>
        /// 接收跳转过来的对象
        /// 1、智能分析
        /// 2、比对记录
        /// 3、
        /// </summary>
        /// <param name="obj"></param>
        void SetReceivedData(object obj)
        {
            TargetControlContent.MainMap.Points = new ObservableCollection<GMap.NET.PointLatLng>();

            try
            {
                if (obj is MyCmpFaceLogWidthImgModel)
                {
                    MyCmpFaceLogWidthImgModel mfl = obj as MyCmpFaceLogWidthImgModel;
                    ForTargetPage = new CameraSnapPerson();
                    ForTargetPage.Name = mfl.T1.UserName;
                    ForTargetPage.Photo = mfl.SnapImage;
                    ForTargetPage.PhotoByteArray = mfl.SnapImageBuffer;
                    ForTargetPage.SnapDate = mfl.date;
                    ForTargetPage.SnapTime = mfl.time;
                    ForTargetPage.CameraInfo = new Camera();
                    ForTargetPage.CameraInfo.Name = mfl.channelName;
                    ForTargetPage.CameraInfo.Longitude = mfl.Longitude;
                    ForTargetPage.CameraInfo.Latitude = mfl.Latitude;
                    ForTargetPage.CameraInfo.Location = mfl.Address;
                    ForTargetPage.Score = mfl.Score;

                    double lat = GlobalCache.Latitude;
                    double lng = GlobalCache.Longitude;
                    bool isLat = Double.TryParse(mfl.Latitude, out lat);
                    bool isLng = Double.TryParse(mfl.Longitude, out lng);

                    TargetControlContent.MainMap.Points = new ObservableCollection<PointLatLng>();
                    if (isLat && isLng)
                    {
                        TargetControlContent.MainMap.Points.Add(new PointLatLng(
                                       lat: isLat ? lat : GlobalCache.Latitude,
                                       lng: isLng ? lng : GlobalCache.Longitude,
                                        cameraOrPhoto: "Photo",
                                        photo: CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1((mfl.SnapImageBuffer)),
                                        geoTitle: new GeoTitle()
                                        ));
                    }
                    else
                    {
                        TargetControlContent.MainMap.Points.Add(new PointLatLng(
                                       lat: isLat ? lat : GlobalCache.Latitude,
                                       lng: isLng ? lng : GlobalCache.Longitude,
                                        cameraOrPhoto: string.Empty,
                                        photo: null,
                                        geoTitle: new GeoTitle()
                                        ));
                    }
                    TargetControlContent.MainMap.MapRefresh.Invoke(null, null);
                }
                else if (obj is CameraSnapPerson)//静态分析抓拍库跳至这里
                {
                    CameraSnapPerson cor = obj as CameraSnapPerson;
                    ForTargetPage = new CameraSnapPerson();
                    if ("抓拍库".Equals(cor.Source))
                    {
                        ForTargetPage.Name = "通 道：" + cor.Name;
                    }
                    else
                    {
                        ForTargetPage.Name = "姓 名：" + cor.Name;
                    }
                    ForTargetPage.Photo = cor.Photo;
                    ForTargetPage.PhotoByteArray = cor.PhotoByteArray;
                    ForTargetPage.SnapDate = cor.SnapDate;
                    ForTargetPage.SnapTime = cor.SnapTime;
                    ForTargetPage.CameraInfo = cor.CameraInfo ?? new Camera();
                    ForTargetPage.CameraInfo.Location = cor.CameraInfo.Location;
                    ForTargetPage.CameraInfo.Longitude = cor.CameraInfo.Longitude;
                    ForTargetPage.CameraInfo.Latitude = cor.CameraInfo.Latitude;
                }
            }
            catch (Exception ex)
            {
                Logger<MainControlViewModel>.Log.Error("SetReceivedData", ex);
            }
            finally
            {
                //更新统计结果 柱状图
                initBarCharts(true, null);
                SearchSchemaDatas.IsTargetValue = "有目标分析";
                RaisePropertyChanged("SearchSchemaDatas");
                RaisePropertyChanged("SusipciousInfo");
            }
        }
    }
}
