using BI_MODULES.PortableLibs;
using DATA.UTILITIES.Log4Net;
using LiveCharts;
using PeopleModel;
using Prism.Commands;
using Prism.Mvvm;
using SearchModel;
using SENSING.THRIFT.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using xiaowen.codestacks.data;

namespace SENSING.Plugin.Intelligent.ViewModels
{
    public partial class MainControlViewModel : BindableBase
    {
        public ICommand SearchCommand { get; private set; }
        public ICommand AddTeamplateCmd { get; set; }

        void initSearchCommand()
        {
            SearchCommand = new DelegateCommand<object>(SearchCommandFunc);
            AddTeamplateCmd = new DelegateCommand<object>(AddTeamplateCmdFunc);
            SearchSchemaDatas = new SearchSchemaModel();
            SearchSchemaDatas.StartDateValue = DateTime.Now.ToString("yyyy/MM/dd");
            SearchSchemaDatas.EndDateValue = DateTime.Now.ToString("yyyy/MM/dd");
            SearchSchemaDatas.IsTargetValue = "无目标分析";
            SearchSchemaDatas.StartTimeIndex = 0;
            SearchSchemaDatas.StartMinute = 0;
            SearchSchemaDatas.EndTimeIndex = 23;
            SearchSchemaDatas.EndMinute = 59;
            RaisePropertyChanged("SearchSchemaDatas");
        }


        private void AddTeamplateCmdFunc(object obj)
        {
            if (obj is CameraSnapPerson)
            {
                CameraSnapPerson csp = obj as CameraSnapPerson;
                MethodInfo mi = null;
                Window w = null;
                foreach (var item in Application.Current.Windows)
                {
                    TypeInfo ti = item.GetType() as TypeInfo;
                    if (ti.Name.Equals("HomeView"))
                    {
                        w = item as Window;
                        mi = ti.GetMethod("SetTemplatePopWindow");
                        break;
                    }
                }
                mi.Invoke(w, new object[] { csp.PhotoByteArray });
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        void SearchCommandFunc(object obj)
        {
            SearchSchemaModel searchData = obj as SearchSchemaModel;
            long startTime = CodeStacksDataHandler.DateTimeData.ConvertToLongDelegate1(true, searchData.StartDateValue, searchData.StartTimeIndex, searchData.StartMinute);
            long endTime = CodeStacksDataHandler.DateTimeData.ConvertToLongDelegate1(false, searchData.EndDateValue, searchData.EndTimeIndex, searchData.EndMinute);

            try
            {
                ThriftServiceUtilities thrift = new ThriftServiceUtilities();
                if (searchData.IsTargetValue == "有目标分析")
                {
                    GoTarget(thrift, startTime, endTime);
                }
                else
                {
                    GoNoTarget(thrift, startTime, endTime);
                }
            }
            catch (Exception)
            {
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="thrift"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        async void GoNoTarget(ThriftServiceUtilities thrift, long startTime, long endTime)
        {
            List<TargetedAnalysis> formapResult = new List<TargetedAnalysis>();
            ObservableCollection<TmpSnapInfo> forsuspectResult = new ObservableCollection<TmpSnapInfo>();
            SusipciousInfo = new CameraSnapPerson();

            await Task.Run(() =>
             {
                 LoadingVisiblity = Visibility.Visible;
                 RaisePropertyChanged("LoadingVisiblity");

                 formapResult = AsyncSelectNoTargetForMap(thrift, startTime, endTime).Result;
                 forsuspectResult = AsyncSelectNoTargetForSusipcious(thrift, startTime, endTime).Result;
             }).ConfigureAwait(false);

            try
            {
                CodeStacksDataHandler.UIThread.Invoke(() =>
                {
                    NoTargetControlContent.MainMap.Points =
                                     GMapForWpf.SetMapAnchor<TargetedAnalysis>(formapResult,
                                     new Uri("pack://application:,,,/Images/home-icon-bluecamera.png"));
                    NoTargetControlContent.MainMap.MapRefresh.Invoke(null, null);

                    if (forsuspectResult.Count > 0)
                    {
                        TopNumberItems = forsuspectResult;
                        var temp = tempSuspectObject;
                        //SusipciousInfo = temp;//给SusipciousInfo赋值，导致异常
                        if (temp.PhotoByteArray.Length == 0)
                        {
                            SusipciousInfo.Photo = CodeStacksDataHandler.ImageData.ConvertToImageSourceDelegate1("pack://application:,,,/Images/unkonw.png");
                            RaisePropertyChanged("SusipciousInfo");
                        }
                        else
                        {
                            SusipciousInfo = tempSuspectObject;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Logger<MainControlViewModel>.Log.Error("GoNoTarget", ex);
            }
            finally
            {
                AsyncAwaitTaskSleepForFiveSeconds();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thrift"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        async void GoTarget(ThriftServiceUtilities thrift, long startTime, long endTime)
        {
            try
            {
                CameraForLiveCharts = new ObservableCollection<string>();

                ChartValues<double> chartVals = new ChartValues<double>();
                if (ForTargetPage != null && ForTargetPage.PhotoByteArray.Length > 0)
                {
                    await Task.Run(() =>
                    {
                        SnapPersonItems = AsyncGetTargetAnalysisResult(thrift, string.Empty, ForTargetPage.PhotoByteArray, -1, startTime, endTime).Result;
                    }).ConfigureAwait(false);

                    Parallel.Invoke(
                        () =>
                        {
                            CodeStacksDataHandler.UIThread.Invoke(() =>
                            {
                                #region LiveCharts
                                for (int i = 0; i < 10; i++)
                                {
                                    int count = SnapPersonItems.Count;
                                    if (count > i)
                                    {
                                        chartVals.Insert(0, Convert.ToDouble(SnapPersonItems[i].SnapPeopleCount));
                                        CameraForLiveCharts.Insert(0, SnapPersonItems[i].Name);
                                    }
                                    else
                                    {
                                        chartVals.Insert(0, 0);
                                        CameraForLiveCharts.Insert(0, "区域" + i.ToString());
                                    }
                                }

                                initBarCharts(false, chartVals);
                                #endregion
                            });
                        },
                        () =>
                        {
                            CodeStacksDataHandler.UIThread.Invoke(() =>
                            {
                                TargetControlContent.MainMap.Points = GMapForWpf.SetMapAnchor<CameraSnapPerson>(SnapPersonItems, ForTargetPage.PhotoByteArray, ForTargetPage.Name);

                                TargetControlContent.MainMap.MapRefresh.Invoke(null, null);
                            });

                        });
                }
            }
            catch (Exception ex)
            {
                Logger<MainControlViewModel>.Log.Error("GoTarget", ex);
            }
            finally
            {
                AsyncAwaitTaskSleepForFiveSeconds();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        async void AsyncAwaitTaskSleepForFiveSeconds()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));
            LoadingVisiblity = Visibility.Collapsed;
        }

        async Task<ObservableCollection<xiaowen.codestacks.data.SenSingModels.Camera>> AsyncGetTargetAnalysisResult(ThriftServiceUtilities thrift, string capid, byte[] capimg, int threshold, long btime, long etime)
        {
            LoadingVisiblity = Visibility.Visible;
            return await Task.Run(() =>
            {
                return thrift.GetTargetAnalysisResult(string.Empty, ForTargetPage.PhotoByteArray, -1, btime, etime);
            }).ConfigureAwait(false);
        }


        /**
         * 解决方案：
         * 使用异步查询数据，等待数据返回后执行主线程
         * 
         * **/
        #region No Target

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thrift"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>

        async Task<List<TargetedAnalysis>> AsyncSelectNoTargetForMap(ThriftServiceUtilities thrift, long startTime, long endTime)
        {
            LoadingVisiblity = Visibility.Visible;

            return await Task<List<TargetedAnalysis>>.Run(() =>
             {
                 return thrift.GetNoTargetAnalysisResultForMap(startTime, endTime);
             }).ConfigureAwait(false);
        }


        private CameraSnapPerson tempSuspectObject = new CameraSnapPerson();
        async Task<ObservableCollection<TmpSnapInfo>> AsyncSelectNoTargetForSusipcious(ThriftServiceUtilities thrift, long startTime, long endTime)
        {
            return await Task<ObservableCollection<TmpSnapInfo>>.Run(() =>
            {
                CameraSnapPerson suspectObj = new CameraSnapPerson();
                ObservableCollection<TmpSnapInfo> result = thrift.GetNoTargetAnalysisResult(ref suspectObj, startTime, endTime);
                tempSuspectObject = suspectObj;
                return result;
            }).ConfigureAwait(false);
        }

        #endregion
    }
}
