using DATA.MODELS.GlobalModels;
using DATA.MODELS.SensingModels;
using FaceSysByMvvm.Model;
using SINGLEUSER.Models;
using System;
using System.Linq;
using System.Windows.Controls;
using xiaowen.codestacks.popwindow;

namespace Function
{
    public class ClearPushedWaringData
    {
        public static string ClearPushedData(ref string err, object isPushSuccessed)
        {
            try
            {
                if ((int)isPushSuccessed == 0)
                {
                    foreach (MyCmpFaceLogWidthImgModel item in ViewDataModel.WarningData.Property.CurCompareLogDatas)
                    {
                        if (GlobalCache.AreaInfoCollection.Cast<ConfigRegion>().SingleOrDefault(x => item.channelName.StartsWith(x.RegionName)) != null)
                        {
                            ViewDataModel.WarningData.Property.CompareLogDatas.Remove(item);
                            ViewDataModel.WarningData.Property.CurCompareLogDatas.Remove(item);
                        }
                    }
                    ViewDataModel.WarningData.RefreshProperty();
                }
            }
            catch (Exception)
            {
            }
            return err;
        }

        public static string ClearPushedData(ref string err, object isPushSuccessed, MyCmpFaceLogWidthImgModel selectedData)
        {
            try
            {
                if ((int)isPushSuccessed == 0)
                {
                    if (ViewDataModel.WarningData.Property.CurCompareLogDatas.Count > 0 && ViewDataModel.WarningData.Property.CurCompareLogDatas.Contains(selectedData))
                    {
                        ViewDataModel.WarningData.Property.CurCompareLogDatas.Remove(selectedData);
                    }
                    ViewDataModel.WarningData.Property.CompareLogDatas.Remove(selectedData);
                    ViewDataModel.WarningData.RefreshProperty();
                }
            }
            catch (Exception)
            {
            }
            return err;
        }


        /// <summary>
        /// 单个清理已推送结果
        /// </summary>
        /// <param name="selectedData"></param>
        /// <returns></returns>
        public static void ClearSinglePushedData(MyCmpFaceLogWidthImgModel selectedData)
        {
            try
            {
                if (ViewDataModel.WarningData.Property.CurCompareLogDatas.Count > 0 && ViewDataModel.WarningData.Property.CurCompareLogDatas.Contains(selectedData))
                {
                    ViewDataModel.WarningData.Property.CurCompareLogDatas.Remove(selectedData);
                }
                ViewDataModel.WarningData.Property.CompareLogDatas.Remove(selectedData);
                ViewDataModel.WarningData.RefreshProperty();
            }
            catch (Exception)
            {
            }
        }


        public static void ClearAll()
        {
            try
            {
                ViewDataModel.WarningData.Property.CompareLogDatas.Clear();
                ViewDataModel.WarningData.Property.CurCompareLogDatas.Clear();
                ViewDataModel.WarningData.RefreshProperty();
            }
            catch (Exception)
            {
            }
        }


        public static string ClearPushedDataFormCodeBehind(ref string err, Button btn)
        {
            try
            {
                MyCmpFaceLogWidthImgModel selectedData = new MyCmpFaceLogWidthImgModel();
                foreach (MyCmpFaceLogWidthImgModel obj in ViewDataModel.WarningData.Property.CompareLogDatas)
                {
                    if (obj.ID == btn.CommandParameter.ToString())
                    {
                        selectedData = obj;
                        break;
                    }
                }

                if (ViewDataModel.WarningData.Property.CurCompareLogDatas.Count > 0 && ViewDataModel.WarningData.Property.CurCompareLogDatas.Contains(selectedData))
                {
                    ViewDataModel.WarningData.Property.CurCompareLogDatas.Remove(selectedData);
                }
                ViewDataModel.WarningData.Property.CompareLogDatas.Remove(selectedData);
                ViewDataModel.WarningData.RefreshProperty();
            }
            catch (System.Exception ex)
            {
                err = ex.Message;
                CodeStacksWindow.MessageBox.Invoke(true, false, 2, ex.Message);
            }

            return err;
        }
        
    }
}
