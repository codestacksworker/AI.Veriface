using DATA.MODELS.GlobalModels;
using PeopleModel;
using Prism.Commands;
using Prism.Mvvm;
using SearchModel;
using SENSING.THRIFT.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using xiaowen.codestacks.data;
using xiaowen.codestacks.popwindow;
using Zhangxiaowen.i20170111.Sensing;
using Zhangxiaowen.i20170111.Sensing.Zhangxiaowen.i20170111.Sensing.Units;

namespace SC_MODULES.ViewModels
{
    public partial class MainControlViewModel : BindableBase
    {
        SearchSchemaModel _searchSchemaDatas;
        public SearchSchemaModel SearchSchemaDatas
        {
            get { return _searchSchemaDatas; }
            set { SetProperty(ref _searchSchemaDatas, value); }
        }

        int _dataCount;
        public int DataCount
        {
            get { return _dataCount; }
            set { SetProperty(ref _dataCount, value); }
        }

        static BitmapImage bimg;
        public static BitmapImage Bimg
        {
            get
            {
                return bimg;
            }

            set
            {
                bimg = value;
            }
        }


        public ICommand SearchCommand { get; set; }
        public ICommand ImportImageCommand { get; set; }
        public ICommand CmdCancelMarkUpKeyObject { get; set; }
        public ICommand CmdMarkUpKeyObject { get; set; }
        /// <summary>
        /// 选择分析库
        /// </summary>
        public ICommand SelectedAnalysis { get; set; }

        void initCmd()
        {
            SearchCommand = new DelegateCommand<object>(SearchCommandFunc);
            ImportImageCommand = new DelegateCommand<object>(ImportImageCommandFunc);
            CmdMarkUpKeyObject = new DelegateCommand<object>(CmdMarkUpKeyObjectFunc);
            CmdCancelMarkUpKeyObject = new DelegateCommand<object>(CmdCancelMarkUpKeyObjectFunc);
            SelectedAnalysis = new DelegateCommand(SelectedAnalysisFunc);
            SearchSchemaDatas = new SearchModel.SearchSchemaModel(true);
            SearchSchemaDatas.EndTimeIndex = 23;
            SearchSchemaDatas.EndMinute = 59;
        }

        private void CmdMarkUpKeyObjectFunc(object obj)
        {
            CameraSnapPerson csp = obj as CameraSnapPerson;
            ThriftServiceUtilities thrift = new ThriftServiceUtilities();
            if (thrift.MarkUpKeyObject(csp.SnapId, 1) == 0)
                CodeStacksWindow.MessageBox.Invoke(false, false, 1, "操作成功");
            else
                CodeStacksWindow.MessageBox.Invoke(false, false, 1, "操作失败");
        }

        private void CmdCancelMarkUpKeyObjectFunc(object obj)
        {
            CameraSnapPerson csp = obj as CameraSnapPerson;
            ThriftServiceUtilities thrift = new ThriftServiceUtilities();
            if (thrift.MarkUpKeyObject(csp.SnapId, -1) == 0)
                CodeStacksWindow.MessageBox.Invoke(false, false, 1, "操作成功");
            else
                CodeStacksWindow.MessageBox.Invoke(false, false, 1, "操作失败");
        }

        string prexPhoto = string.Empty;
        private void ImportImageCommandFunc(object obj)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "图片|*.jpg;*.png;*.bmp;*.jpeg";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;
                if (filename != prexPhoto)
                {
                    string toptipMsg = string.Empty;
                    if (ZhangXiaowen.GetFileSize(filename, SizeUnits.KByte, ref toptipMsg) < 500)
                    {
                        SearchSchemaDatas.ImportImageByteArray = CodeStacksDataHandler.ImageData.ConvertToBuffer1Delegate(filename);
                        SearchSchemaDatas.ImportImage = CodeStacksDataHandler.ImageData.ConvertToBitmapImageDelegate1(SearchSchemaDatas.ImportImageByteArray);
                        RaisePropertyChanged("SearchSchemaDatas");
                    }
                    else
                        CodeStacksWindow.MessageBox.Invoke(false, false, 2, "当前文件" + toptipMsg + "\n文件不能大于500KB");
                }
            }
        }

        private void SelectedAnalysisFunc()
        {
            if (GlobalCache.AppType == 0)
            {
                if (SearchSchemaDatas.TemplateStoreIndex == 0)
                {
                    IsVisibleMarkUp = Visibility.Visible;
                }
                else
                {
                    IsVisibleMarkUp = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void SearchCommandFunc(object obj)
        {
            SearchSchemaModel searchData = obj as SearchSchemaModel;
            ThriftServiceUtilities thrift = new ThriftServiceUtilities();

            long startDate =
                CodeStacksDataHandler.DateTimeData.ConvertToLongDelegate1(true, searchData.StartDateValue, searchData.StartTimeIndex, searchData.StartMinute);
            long endDate =
                CodeStacksDataHandler.DateTimeData.ConvertToLongDelegate1(false, searchData.EndDateValue, searchData.EndTimeIndex, searchData.EndMinute);

            if (searchData.TemplateStoreIndex == 0)
            {
                IsSnapStore = Visibility.Collapsed;

                Task task = QueryResultFromTemplateStore(thrift, startDate, endDate, searchData);
            }
            else
            {
                IsSnapStore = Visibility.Visible;
                Task task = QueryResultFromSanpStore(thrift, startDate, endDate);
            }
            SelectedAnalysisFunc();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thrift"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="searchData"></param>
        /// <returns></returns>
        async Task QueryResultFromTemplateStore(ThriftServiceUtilities thrift, long start, long end, SearchSchemaModel searchData)
        {
            await Task.Run(() =>
            {
                LoadingVisiblity = Visibility.Visible;

                IList<CameraSnapPerson> result = thrift.SC_GetAnalysisResultFromTemplateStore(searchData.ImportImageByteArray, searchData.ThresholdValue, searchData.AppearCountValue, "模板库");

                TemplatePersonItems = result;
                DataCount = TemplatePersonItems.Count;

                LoadingVisiblity = Visibility.Collapsed;
            }).ConfigureAwait(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thrift"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        async Task QueryResultFromSanpStore(ThriftServiceUtilities thrift, long start, long end)
        {
            await Task.Run(() =>
            {
                LoadingVisiblity = Visibility.Visible;
                IList<CameraSnapPerson> result = thrift.SC_GetAnalysisResult(SearchSchemaDatas.CaptureId, SearchSchemaDatas.ImportImageByteArray, start, end, SearchSchemaDatas.ThresholdValue, SearchSchemaDatas.AppearCountValue, "抓拍库");
                TemplatePersonItems = result;
                DataCount = TemplatePersonItems.Count;

                LoadingVisiblity = Visibility.Collapsed;
            }).ConfigureAwait(false);
        }
    }
}
