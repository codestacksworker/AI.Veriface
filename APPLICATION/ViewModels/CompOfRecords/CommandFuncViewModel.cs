using SENSING.ClassPool;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using FaceSysByMvvm.Model;
using xiaowen.codestacks.data;
using DATA.UTILITIES.Log4Net;
using System.Windows;
using System.Linq;
using DATA.MODELS.GlobalModels;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Threading;
using xiaowen.codestacks.popwindow;
using SINGLEUSER.Models;

namespace FaceSysByMvvm.ViewModels.CompOfRecords
{
    public partial class CompOfRecordsViewModel : BindableBase
    {
        Action<int, CompOfRecordsViewModel> getCompOfRecordsDelegate;
        

        ///// <summary>
        ///// 获得父窗体控件
        ///// </summary>
        ///// <typeparam name="T">要获得控件类名</typeparam>
        ///// <param name="obj">当前子控件名</param>
        ///// <param name="name">要查询父控件名</param>
        ///// <returns>要获得控件类名</returns>
        //public static T GetParentObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        //{
        //    DependencyObject parent = VisualTreeHelper.GetParent(obj);
        //    while (parent != null)
        //    {
        //        if (parent is T && (((T)parent).Name == name || string.IsNullOrEmpty(name)))
        //        {
        //            return (T)parent;
        //        }
        //        parent = VisualTreeHelper.GetParent(parent);
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// 获得子控件
        ///// </summary>
        ///// <typeparam name="T">要获得控件类名</typeparam>
        ///// <param name="obj">当前控件名</param>
        ///// <param name="name">要查询子控件名</param>
        ///// <returns>要获得控件类名</returns>
        //public static T GetChildObject<T>(DependencyObject obj, string name) where T : FrameworkElement
        //{
        //    DependencyObject child = null;
        //    T grandChild = null;
        //    for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
        //    {
        //        child = VisualTreeHelper.GetChild(obj, i);
        //        if (child is T && (((T)child).Name == name | string.IsNullOrEmpty(name)))
        //        {
        //            return (T)child;
        //        }
        //        else
        //        {
        //            grandChild = GetChildObject<T>(child, name);
        //            if (grandChild != null)
        //                return grandChild;
        //        }
        //    }
        //    return null;
        //}

        private void JumpToPageIndexCommandFunc(object obj)
        {
            try
            {
                CompOfRecordsViewModel crv = obj as CompOfRecordsViewModel;
                if (crv.MaxPage >= crv.InputPageIndex)
                {
                    getCompOfRecordsDelegate.Invoke(Convert.ToInt32(crv.InputPageIndex), crv);
                }
                else
                {
                    crv.InputPageIndex = crv.MaxPage;
                }
            }
            catch (Exception)
            {
                CodeStacksWindow.MessageBox.Invoke(false, false, 2, "请输入正确的跳转页码!");
            }
        }

        private async void LastPageCommandFunc(object obj)
        {
            await Task.Run(() =>
            {
                CompOfRecordsViewModel crv = obj as CompOfRecordsViewModel;
                if (crv.MaxPage > 0)
                {
                    IntiQueryTime(crv);
                    getCompOfRecordsDelegate.Invoke(crv.MaxPage, crv);
                }
            });
        }

        private async void NextPageCommandFunc(object obj)
        {
            await Task.Run(() =>
            {
                CompOfRecordsViewModel crv = obj as CompOfRecordsViewModel;
                if (crv.maxPage == crv.currPage)
                {
                }
                else
                {
                    IntiQueryTime(crv);
                    getCompOfRecordsDelegate.Invoke(crv.CurrPage + 1, crv);
                }
            });
        }

        private async void PrevPageCommandFunc(object obj)
        {
            await Task.Run(() =>
            {
                try
                {
                    CompOfRecordsViewModel crv = obj as CompOfRecordsViewModel;
                    if (crv.CurrPage > 1)
                    {
                        IntiQueryTime(crv);
                        getCompOfRecordsDelegate.Invoke(crv.CurrPage - 1, crv);
                    }
                }
                catch (Exception)
                {
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private async void FirstPageCommandFunc(object obj)
        {
            await Task.Run(() =>
            {
                CompOfRecordsViewModel crv = obj as CompOfRecordsViewModel;
                if (crv.CurrPage > 0)
                {
                    IntiQueryTime(crv);
                    getCompOfRecordsDelegate.Invoke(1, crv);
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void SearchCommandFunc(object obj)
        {
            CompOfRecordsViewModel crv = obj as CompOfRecordsViewModel;
            getCompOfRecordsDelegate = GetAllInfo;
            btnCompOfRecordsQuery_Click(crv);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cORViewModel"></param>
        private void btnCompOfRecordsQuery_Click(CompOfRecordsViewModel corViewModel)
        {
            try
            {
                string strChannelValue = corViewModel.SelectedChannel == 0 ? "" : corViewModel.ChannelId[corViewModel.SelectedChannel - 1];
                if (strChannelValue.Contains("\\") && !strChannelValue.Contains("\\\\"))
                {
                    strChannelValue = strChannelValue.Replace("\\", "\\\\");
                }

                corViewModel.CompOfRecordsValueObj.ChannelValue = strChannelValue;
                corViewModel.CompOfRecordsValueObj.NameValue = corViewModel.Name;

                if (corViewModel.SelectedType == -1 || corViewModel.SelectedType == 0)
                {
                    corViewModel.CompOfRecordsValueObj.TypeValue = -1;
                }
                else
                {
                    corViewModel.CompOfRecordsValueObj.TypeValue = corViewModel.SelectedType;
                }

                corViewModel.CompOfRecordsValueObj.SexValue = corViewModel.SelectedSex == 0 ? -1 : corViewModel.SelectedSex;
                corViewModel.CompOfRecordsValueObj.MinAgeValue =
                    corViewModel.MinAge == "" ? -1 : Convert.ToInt32(corViewModel.MinAge);
                corViewModel.CompOfRecordsValueObj.MaxAgeValue =
                    corViewModel.MaxAge == "" ? -1 : Convert.ToInt32(corViewModel.MaxAge);

                IntiQueryTime(corViewModel);

                if (corViewModel.CompOfRecordsValueObj.StartDayValue == -1 || (corViewModel.CompOfRecordsValueObj.EndDayValue - corViewModel.CompOfRecordsValueObj.StartDayValue > 60 * 60 * 24 * 7))
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "时间间隔请小于 7 天！");
                }

                getCompOfRecordsDelegate.Invoke(1, corViewModel);
            }
            catch (Exception ex)
            {
                Logger<CompOfRecordsViewModel>.Log.Error("btnCompOfRecordsQuery_Click", ex);
            }
        }

        /// <summary>
        /// 初始化查询时间
        /// </summary>
        /// <param name="corViewModel"></param>
        private void IntiQueryTime(CompOfRecordsViewModel corViewModel)
        {
            try
            {
                long startDate = -1;
                if (!string.IsNullOrEmpty(corViewModel.StartDay))
                {
                    startDate =
                        CodeStacksDataHandler.DateTimeData.ConvertToLongBySubstractDelegate.Invoke(Convert.ToDateTime(corViewModel.StartDay), new DateTime(1970, 1, 1));

                    if (corViewModel.SelectedStartHour != -1)
                    {
                        startDate =
                            startDate + int.Parse(corViewModel.SelectedStartHour.ToString()) * 60 * 60 + SelectedStartMinutes * 60;
                    }
                }
                corViewModel.CompOfRecordsValueObj.StartDayValue = startDate;
                //结束时间
                long endDate = -1;
                if (!string.IsNullOrEmpty(corViewModel.EndDay))
                {
                    endDate =
                        CodeStacksDataHandler.DateTimeData.ConvertToLongBySubstractDelegate.Invoke(Convert.ToDateTime(corViewModel.EndDay), new DateTime(1970, 1, 1));

                    if (corViewModel.SelectedEndHour != -1)
                    {
                        endDate =
                            endDate + int.Parse(corViewModel.SelectedEndHour.ToString()) * 60 * 60 + SelectedEndMinutes * 60 + 3660;
                    }
                }
                else
                    endDate =
                        CodeStacksDataHandler.DateTimeData.ConvertToLongBySubstractDelegate.Invoke(DateTime.Now, new DateTime(1970, 1, 1));

                corViewModel.CompOfRecordsValueObj.EndDayValue = endDate;
            }
            catch (Exception ex)
            {
                Logger<CompOfRecordsViewModel>.Log.Error("IntiQueryTime", ex);
            }
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="cORViewModel"></param>
        private async void GetAllInfo(int pageIndex, CompOfRecordsViewModel cORViewModel)
        {
            cORViewModel.LoadingVisiblity = Visibility.Visible;

            await Task.Run(() =>
            {
                try
                {
                    int _pageSize = cORViewModel.CompOfRecordsValueObj.PageSize;

                    List<int> dailyPageSum = new List<int>();
                    int pageSum = 0, index = 0;
                    List<MyCapFaceLogWithImg> listMyCapFaceLogWithImg = new List<MyCapFaceLogWithImg>();
                    List<SCountInfo> resultCount = new List<SCountInfo>();

                    if (GlobalCache.AppType == 0)//筛选端
                    {
                        if (IsSearchPublished == true)
                            resultCount =
                                thirft.QueryCmpRecordTotalCountHSX(cORViewModel.CompOfRecordsValueObj, GlobalCache.AppRegion) ?? new List<SCountInfo>();
                        else
                            resultCount =
                                    thirft.QueryCmpRecordTotalCountH(cORViewModel.CompOfRecordsValueObj) ?? new List<SCountInfo>();
                    }
                    else//接收端
                    {
                        resultCount =
                                thirft.QueryCmpRecordTotalCountHSX(cORViewModel.CompOfRecordsValueObj, GlobalCache.AppRegion) ?? new List<SCountInfo>();
                    }

                    if (resultCount.Count() == 0) return;

                    //按日拆分后的数据
                    resultCount[0].Dayarr.Reverse();
                    foreach (SCountInfoOneDay item in resultCount[0].Dayarr)
                    {
                        pageSum += (item.Count / _pageSize + (item.Count % _pageSize == 0 ? 0 : 1));
                        dailyPageSum.Add(pageSum);
                    }

                    cORViewModel.CurrPage = pageIndex;//currentPageIndex
                    cORViewModel.MaxPage = pageSum;
                    cORViewModel.MaxDataCount = resultCount[0].Count;

                    //根据页数判断是属于哪一天
                    for (int i = 0; i < dailyPageSum.Count; i++)
                    {
                        if (pageIndex <= dailyPageSum[i])
                        {
                            index = i;
                            //修改当前的时间 和 最大的结果数量
                            DateTime dt1 = Convert.ToDateTime(resultCount[0].Dayarr[i].Daystr.Insert(6, "/").Insert(4, "/"));
                            long _date = CodeStacksDataHandler.DateTimeData.ConvertToLongBySubstractDelegate.Invoke(dt1, new DateTime(1970, 1, 1));

                            if (_date > cORViewModel.CompOfRecordsValueObj.StartDayValue)
                            {
                                cORViewModel.CompOfRecordsValueObj.StartDayValue = _date;
                            }
                            if (pageIndex != cORViewModel.MaxPage)
                            {
                                long endTime = _date + 24 * 60 * 60;
                                var todayEndtime = CodeStacksDataHandler.DateTimeData.ConvertToLongBySubstractDelegate.Invoke(dt1.AddDays(1), new DateTime(1970, 1, 1));

                                if (endTime > todayEndtime)
                                {
                                    endTime = todayEndtime;
                                }
                                cORViewModel.CompOfRecordsValueObj.EndDayValue = endTime;
                            }
                            cORViewModel.CompOfRecordsValueObj.MaxDataCount = resultCount[0].Dayarr[i].Count;
                            break;
                        }
                    }

                    if (pageIndex > 1)
                    {
                        int pageTem = 0;
                        if (index == 0)
                        {
                            pageTem = 0;
                        }
                        else
                        {
                            pageTem = dailyPageSum[index - 1];
                        }
                        cORViewModel.CompOfRecordsValueObj.PageStartRow = cORViewModel.CompOfRecordsValueObj.MaxDataCount - ((pageIndex - pageTem) * _pageSize);
                        if (cORViewModel.CompOfRecordsValueObj.PageStartRow < 0)
                        {
                            cORViewModel.CompOfRecordsValueObj.PageStartRow = 0;
                        }
                    }
                    else
                    {
                        cORViewModel.CompOfRecordsValueObj.PageStartRow = cORViewModel.CompOfRecordsValueObj.MaxDataCount - _pageSize;
                        if (cORViewModel.CompOfRecordsValueObj.PageStartRow < 0)
                        {
                            cORViewModel.CompOfRecordsValueObj.PageStartRow = 0;
                        }
                    }

                    int countTem = 0;
                    resultCount[0].Dayarr.Reverse();
                    for (int n = 0; n <= index; n++)
                    {
                        var dayary = resultCount[0].Dayarr[resultCount[0].Dayarr.Count - 1 - n].Count;
                        countTem += dayary;
                    }

                    cORViewModel.CompOfRecordsValueObj.MaxDataCount = countTem;
                    if (GlobalCache.AppType == 0)
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                            {
                                if (IsSearchPublished == true)
                                    cORViewModel.CompareResultItems = thirft.QueryCmpLogSX(cORViewModel.CompOfRecordsValueObj, GlobalCache.AppRegion);
                                else
                                    cORViewModel.CompareResultItems = thirft.QueryCmpLog(cORViewModel.CompOfRecordsValueObj);
                            });
                    }
                    else
                    {
                        DispatcherHelper.CheckBeginInvokeOnUI(() =>
                        {
                            cORViewModel.CompareResultItems = thirft.QueryCmpLogSX(cORViewModel.CompOfRecordsValueObj, GlobalCache.AppRegion);
                        });
                    }
                }
                catch (Exception ex)
                {
                    Logger<CompOfRecordsViewModel>.Log.Error("GetAllInfo(int pageIndex, CompOfRecordsViewModel cORViewModel)", ex);
                }
                finally
                {
                    cORViewModel.LoadingVisiblity = Visibility.Collapsed;
                }
            });
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void PublishCompareResultCmdFunc(object obj)
        {
            if (obj is MyCmpFaceLogWidthImgModel)
            {
                MyCmpFaceLogWidthImgModel publishObj = obj as MyCmpFaceLogWidthImgModel;
                int res = WarningMessageCmd.SendOneResultInfo(publishObj);
                if (res == 0)
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 1, "推送成功");
                }
            }
        }




    }
}
