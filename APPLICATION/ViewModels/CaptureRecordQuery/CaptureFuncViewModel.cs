using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThriftServiceNameSpace;
using SENSING.ClassPool;
using Prism.Mvvm;
using System.Windows;
using DATA.MODELS.GlobalModels;
using xiaowen.codestacks.popwindow;
using Zhangxiaowen.i20170111.Sensing;
using DATA.UTILITIES.Log4Net;
using System.Windows.Controls.Primitives;
using System.Reflection;
using System.Windows.Controls;
using SINGLEUSER.Models;
using Prism.Commands;
using System.Windows.Input;
using xiaowen.codestacks.data;
using GalaSoft.MvvmLight.Threading;
using xiaowen.codestacks.wpf.Utilities;

namespace FaceSysByMvvm.ViewModels.CaptureRecordQuery
{
    /// <summary>
    /// 抓拍记录
    /// </summary>
    public partial class CaptureRecordQueryViewModel : BindableBase
    {
        Action<int, CaptureRecordQueryViewModel> getCaptureRecordsDelegate;

        private void SearchCommandFunc(object obj)
        {
            CaptureRecordQueryViewModel crv = obj as CaptureRecordQueryViewModel;
            getCaptureRecordsDelegate = GetAllInfo;
            btnCaptureRecordQuery_Click(crv);
        }

        private async void FirstPageCommandFunc(object obj)
        {
            await Task.Run(() =>
            {
                CaptureRecordQueryViewModel capViewModel = obj as CaptureRecordQueryViewModel;
                IntiQueryTime(capViewModel);
                getCaptureRecordsDelegate.Invoke(1, capViewModel);
            });
        }

        private async void PrevPageCommandFunc(object obj)
        {
            await Task.Run(() =>
            {
                try
                {
                    CaptureRecordQueryViewModel capViewModel = obj as CaptureRecordQueryViewModel;
                    if (capViewModel.CurrPage > 1)
                    {
                        IntiQueryTime(capViewModel);
                        getCaptureRecordsDelegate.Invoke(capViewModel.CurrPage - 1, capViewModel);
                    }
                }
                catch (Exception)
                {
                }
            });
        }

        private async void NextPageCommandFunc(object obj)
        {
            await Task.Run(() =>
            {
                CaptureRecordQueryViewModel capViewModel = obj as CaptureRecordQueryViewModel;
                if (capViewModel.maxPage == capViewModel.currPage)
                {
                }
                else
                {
                    IntiQueryTime(capViewModel);
                    getCaptureRecordsDelegate.Invoke(capViewModel.CurrPage + 1, capViewModel);
                }
            });
        }

        private async void LastPageCommandFunc(object obj)
        {
            await Task.Run(() =>
            {
                CaptureRecordQueryViewModel capViewModel = obj as CaptureRecordQueryViewModel;
                IntiQueryTime(capViewModel);
                getCaptureRecordsDelegate.Invoke(capViewModel.MaxPage, capViewModel);
            });
        }

        private void JumpToPageIndexCommandFunc(object obj)
        {
            try
            {
                CaptureRecordQueryViewModel capViewdel = obj as CaptureRecordQueryViewModel;
                if (capViewdel.MaxPage >= capViewdel.InputPageIndex)
                {
                    getCaptureRecordsDelegate.Invoke(Convert.ToInt32(capViewdel.InputPageIndex), capViewdel);
                }
                else
                {
                    capViewdel.InputPageIndex = capViewdel.MaxPage;
                }
            }
            catch (Exception)
            {
                CodeStacksWindow.MessageBox.Invoke(false, false, 2, "请输入正确的跳转页码!");
            }
        }

        private void GotoMCCommandFunc(object obj)
        {
            MyCapFaceLogWithImg model = obj as MyCapFaceLogWithImg;
            GoToCaptureFunc(model, 4);
        }
        /// <summary>
        /// 打开窗体
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="index"></param>
        void GoToCaptureFunc(object obj, int index)
        {
            try
            {
                MyCapFaceLogWithImg myCap = obj as MyCapFaceLogWithImg;

                List<byte[]> listImageBytes = new List<byte[]>();
                listImageBytes = thirft.QueryCapLogImageH(myCap.ID, DateTime.Parse(myCap.time).ToString("yyyyMMdd"));
                object newObj = null;
                //得到图片
                if (listImageBytes[0].Length > 0)
                {
                    List<byte[]> senceImg = thirft.QuerySenceImg(myCap.ID, myCap.time.Split(' ')[0].Replace(@"/", "").Replace(@"/", ""));
                    newObj = listImageBytes[0];
                }

                ReflactionView.GoTo1(newObj, 4, "HomeView", "FuncationToggleButton_Checked");
            }
            catch (Exception ex)
            {
                Logger<CaptureRecordQueryViewModel>.Log.Error("GoToCommandFunc", ex);
            }

            #region Old
            //try
            //{
            //    if (obj != null)
            //    {
            //        MyCapFaceLogWithImg myCap = obj as MyCapFaceLogWithImg;
            //        string mainWindowName = ZhangXiaowen.MainWindow;

            //        var s = Application.Current.Windows.Cast<Window>().FirstOrDefault(x => x.Name == mainWindowName);

            //        foreach (Window item in Application.Current.Windows)
            //        {
            //            TypeInfo mainObject = item.GetType() as TypeInfo;
            //            if (mainWindowName.Equals(mainObject.ToString()))
            //            {
            //                Grid grid = item.Content as Grid;
            //                grid = grid.Children[1] as Grid;
            //                StackPanel stackPanel = grid.Children[0] as StackPanel;
            //                ToggleButton tbtn = stackPanel.Children[index] as ToggleButton;
            //                object mainDatacontext = item.DataContext;
            //                IEnumerable<MethodInfo> imethods = mainObject.DeclaredMethods;
            //                var md = imethods.FirstOrDefault(method => method.Name == ZhangXiaowen.ToggleBtnEventFunc) ?? null;
            //                if (md != null)
            //                {
            //                    List<byte[]> listImageBytes = new List<byte[]>();
            //                    listImageBytes = thirft.QueryCapLogImageH(myCap.ID, DateTime.Parse(myCap.time).ToString("yyyyMMdd"));
            //                    object newObj = null;
            //                    //得到图片
            //                    if (listImageBytes[0].Length > 0)
            //                    {
            //                        List<byte[]> senceImg = thirft.QuerySenceImg(myCap.ID, myCap.time.Split(' ')[0].Replace(@"/", "").Replace(@"/", ""));
            //                        newObj = listImageBytes[0];

            //                    }
            //                    if (newObj != null)
            //                    {
            //                        RoutedEventArgs e = new RoutedEventArgs(ToggleButton.ClickEvent);
            //                        e.Source = newObj;
            //                        md.Invoke(item, new object[] { tbtn, e });
            //                        break;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Logger<CaptureRecordQueryViewModel>.Log.Error("GoToCommandFunc", ex);
            //} 
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cORViewModel"></param>
        private void btnCaptureRecordQuery_Click(CaptureRecordQueryViewModel capViewModel)
        {
            try
            {
                string strChannelValue = capViewModel.SelectedChannel == 0 ? "" : capViewModel.ChannelId[capViewModel.SelectedChannel - 1];
                if (strChannelValue.Contains("\\") && !strChannelValue.Contains("\\\\"))
                {
                    strChannelValue = strChannelValue.Replace("\\", "\\\\");
                }

                capViewModel.CaptureRecordsValueObj.ChannelValue = strChannelValue;

                IntiQueryTime(capViewModel);

                if (capViewModel.CaptureRecordsValueObj.StartDayValue == -1 || (capViewModel.CaptureRecordsValueObj.EndDayValue - capViewModel.CaptureRecordsValueObj.StartDayValue > 60 * 60 * 24 * 7))
                {
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "时间间隔请小于 7 天！");
                }

                getCaptureRecordsDelegate.Invoke(1, capViewModel);
            }
            catch (Exception ex)
            {
                Logger<CaptureRecordQueryViewModel>.Log.Error("btnCaptureRecordQuery_Click", ex);
            }
        }

        /// <summary>
        /// 初始化查询时间
        /// </summary>
        /// <param name="corViewModel"></param>
        private void IntiQueryTime(CaptureRecordQueryViewModel capViewModel)
        {
            try
            {
                //开始时间
                long startTime = -1;
                if (!string.IsNullOrEmpty(capViewModel.StartDay))
                {
                    startTime =
                        CodeStacksDataHandler.DateTimeData.ConvertToLongBySubstractDelegate.Invoke(Convert.ToDateTime(capViewModel.StartDay), new DateTime(1970, 1, 1));

                    if (capViewModel.SelectedStartHour > -1)
                    {
                        startTime = startTime + int.Parse(capViewModel.SelectedStartHour.ToString()) * 60 * 60 + SelectedStartMinutes * 60;
                    }
                }
                capViewModel.CaptureRecordsValueObj.StartDayValue = startTime;
                //结束时间
                long endTime = -1;
                if (!string.IsNullOrEmpty(capViewModel.EndDay))
                {
                    endTime = CodeStacksDataHandler.DateTimeData.ConvertToLongBySubstractDelegate.Invoke(Convert.ToDateTime(capViewModel.EndDay), new DateTime(1970, 1, 1));
                    if (capViewModel.SelectedEndHour > -1)
                    {
                        endTime = endTime + int.Parse(capViewModel.SelectedEndHour.ToString()) * 60 * 60 + SelectedEndMinutes * 60 + 3660;
                    }
                }
                else
                {
                    endTime =
                        CodeStacksDataHandler.DateTimeData.ConvertToLongBySubstractDelegate.Invoke(DateTime.Now, new DateTime(1970, 1, 1));
                }
                capViewModel.CaptureRecordsValueObj.EndDayValue = endTime;
            }
            catch (Exception ex)
            {
                Logger<CaptureRecordQueryViewModel>.Log.Error("IntiQueryTime", ex);
            }
        }

        private async void GetAllInfo(int pageIndex, CaptureRecordQueryViewModel capViewModel)
        {
            capViewModel.LoadingVisiblity = Visibility.Visible;
            await Task.Run(() =>
            {
                try
                {
                    List<int> pageSplit = new List<int>();
                    int curpage = 0;
                    int index = 0;
                    int _pageSize = capViewModel.CaptureRecordsValueObj.PageRowValue;
                    List<MyCapFaceLogWithImg> listMyCapFaceLogWithImg = new List<MyCapFaceLogWithImg>();
                    List<SCountInfo> queryCount = new List<SCountInfo>();
                    if (GlobalCache.AppType != 1)
                    {
                        queryCount = thirft.QueryCapRecordTotalCountH(capViewModel.CaptureRecordsValueObj);
                    }
                    else
                    {
                        if (capViewModel.CaptureRecordsValueObj.ChannelValue != "")
                        {
                            List<string> channelTemp = new List<string>();
                            channelTemp.Add(capViewModel.CaptureRecordsValueObj.ChannelValue);
                            queryCount = thirft.QueryCapRecordTotalCountHSXC(capViewModel.CaptureRecordsValueObj, channelTemp);
                        }
                        else
                        {
                            queryCount = thirft.QueryCapRecordTotalCountHSXC(capViewModel.CaptureRecordsValueObj, capViewModel.ChannelId);
                        }
                    }

                    if (queryCount.Count <= 0)
                    {
                        return;
                    }
                    capViewModel.MaxCount = queryCount[0].Count;
                    for (int no = queryCount[0].Dayarr.Count - 1; no >= 0; no--)
                    {
                        var dayary = queryCount[0].Dayarr[no];
                        curpage += dayary.Count % _pageSize != 0 ?
                        dayary.Count / _pageSize + 1 :
                        dayary.Count / _pageSize;
                        pageSplit.Add(curpage);
                    }
                    capViewModel.MaxPage = curpage;
                    //根据页数判断是属于哪一天
                    foreach (var dayPage in pageSplit)
                    {
                        if (pageIndex <= dayPage)
                        {
                            index = pageSplit.IndexOf(dayPage);
                            //修改当前的时间 和 最大的结果数量
                            capViewModel.SelectCurrDay = queryCount[0].Dayarr[queryCount[0].Dayarr.Count - 1 - index].Daystr;
                            DateTime dt1 = Convert.ToDateTime(queryCount[0].Dayarr[queryCount[0].Dayarr.Count - 1 - index].Daystr.Insert(6, "/").Insert(4, "/"));
                            TimeSpan dt1Span = new TimeSpan(dt1.Ticks);
                            DateTime dt2 = new DateTime(1970, 1, 1);
                            TimeSpan dt2Span = new TimeSpan(dt2.Ticks);
                            long longdtPkCompRecordStarTime = Convert.ToInt64(dt1Span.TotalSeconds - dt2Span.TotalSeconds);
                            if (longdtPkCompRecordStarTime > capViewModel.CaptureRecordsValueObj.StartDayValue)
                            {
                                capViewModel.CaptureRecordsValueObj.StartDayValue = longdtPkCompRecordStarTime;
                            }
                            if (pageIndex != capViewModel.MaxPage)
                            {
                                long longdtPkCompRecordEndTime = Convert.ToInt64(dt1Span.TotalSeconds - dt2Span.TotalSeconds) + 24 * 60 * 60;
                                var todayEndtime = Convert.ToInt64(new TimeSpan(dt1.AddDays(1).Ticks).TotalSeconds - dt2Span.TotalSeconds);
                                if (longdtPkCompRecordEndTime > todayEndtime)
                                {
                                    longdtPkCompRecordEndTime = todayEndtime;
                                }
                                capViewModel.CaptureRecordsValueObj.EndDayValue = longdtPkCompRecordEndTime;
                            }
                            capViewModel.CaptureRecordsValueObj.MaxCount = queryCount[0].Dayarr[queryCount[0].Dayarr.Count - 1 - index].Count;
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
                            pageTem = pageSplit[index - 1];
                        }
                        capViewModel.CaptureRecordsValueObj.StartRowValue = capViewModel.CaptureRecordsValueObj.MaxCount - ((pageIndex - pageTem) * capViewModel.CaptureRecordsValueObj.PageRowValue);
                        if (capViewModel.CaptureRecordsValueObj.StartRowValue < 0)
                        {
                            capViewModel.CaptureRecordsValueObj.StartRowValue = 0;
                        }
                    }
                    else
                    {
                        capViewModel.CaptureRecordsValueObj.StartRowValue = capViewModel.CaptureRecordsValueObj.MaxCount - (capViewModel.CaptureRecordsValueObj.PageRowValue);
                        if (capViewModel.CaptureRecordsValueObj.StartRowValue < 0)
                        {
                            capViewModel.CaptureRecordsValueObj.StartRowValue = 0;
                        }
                    }
                    capViewModel.CurrPage = pageIndex;
                    int countTem = 0;
                    for (int n = 0; n <= index; n++)
                    {
                        var dayary = queryCount[0].Dayarr[queryCount[0].Dayarr.Count - 1 - n].Count;
                        countTem += dayary;
                    }
                    capViewModel.CaptureRecordsValueObj.MaxCount = countTem;

                    if (GlobalCache.AppType != 1)
                    {
                        capViewModel.CaptureResultItems = thirft.QueryCapLog(capViewModel.CaptureRecordsValueObj);
                    }
                    else
                    {
                        if (capViewModel.CaptureRecordsValueObj.ChannelValue != "")
                        {
                            List<string> channelTemp = new List<string>();
                            channelTemp.Add(capViewModel.CaptureRecordsValueObj.ChannelValue);
                            capViewModel.CaptureResultItems = thirft.QueryCapLogSXC(capViewModel.CaptureRecordsValueObj, channelTemp);
                        }
                        else
                        {
                            capViewModel.CaptureResultItems = thirft.QueryCapLogSXC(capViewModel.CaptureRecordsValueObj, capViewModel.ChannelId);
                        }
                    }
                    capViewModel.CaptureRecordsValueObj.MaxCount = countTem;

                    DispatcherHelper.CheckBeginInvokeOnUI(() =>
                    {
                        capViewModel.CaptureResultItems = thirft.QueryCapLog(capViewModel.CaptureRecordsValueObj);
                    });
                }
                catch (Exception ex)
                {
                    Logger<CaptureRecordQueryViewModel>.Log.Error("GetAllInfo(int pageIndex, CaptureRecordQueryViewModel capViewModel)", ex);
                }
                finally
                {
                    capViewModel.LoadingVisiblity = Visibility.Collapsed;
                }
            });
        }
    }
}
