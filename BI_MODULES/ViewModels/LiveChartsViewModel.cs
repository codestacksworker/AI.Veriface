using LiveCharts;
using LiveCharts.Wpf;
using Prism.Mvvm;
using System.Collections;
using System.Collections.ObjectModel;
using xiaowen.codestacks.data;

namespace SENSING.Plugin.Intelligent.ViewModels
{
    public partial class MainControlViewModel : BindableBase
    {
        SeriesCollection _xwRowSeries = new SeriesCollection();
        public SeriesCollection XwRowSeries
        {
            get { return _xwRowSeries; }
            set { SetProperty(ref _xwRowSeries, value); }
        }

        ChartValues<double> _chartValues;
        public ChartValues<double> ChartValues
        {
            get { return _chartValues; }
            set { SetProperty(ref _chartValues, value); }
        }

        public ObservableCollection<string> _cameraForLiveCharts;
        public ObservableCollection<string> CameraForLiveCharts
        {
            get { return _cameraForLiveCharts; }
            set { SetProperty(ref _cameraForLiveCharts, value); }
        }

        /// <summary>
        /// 区域10,区域9,区域8,区域7,区域6,区域5,区域4,区域3,区域2,区域1
        /// </summary>
        /// <param name="isInit"></param>
        /// <param name="chartVals"></param>
        void initBarCharts(bool isInit, ChartValues<double> chartVals)
        {
            CodeStacksDataHandler.UIThread.Invoke(() =>
            {
                if (isInit)
                    XwRowSeries = new SeriesCollection { new RowSeries {
                    Values = new ChartValues<double> { 0,0,0,0,0,0,0,0,0,0 },
                    Name="出现次数"
                } };
                else
                    XwRowSeries = new SeriesCollection{
                new RowSeries {
                    Values = chartVals,
                    Name="出现次数"
                }};
            });
        }

        ChartValues<double> XwGetRegionFrequency(ChartValues<double> chartVals)
        {
            IEnumerable frequency = new ChartValues<double>();
            frequency = chartVals;
            return frequency as ChartValues<double>;
        }


    }
}
