
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace SearchModel
{
    public class SearchSchemaModel
    {
        public string CaptureId { get; set; }
        public ImageSource ImportImage { get; set; }
        public byte[] ImportImageByteArray { get; set; }

        public Visibility Target { get; set; }
        public Visibility NoTarget { get; set; }

        public int IsTargetIndex { get; set; }
        public string IsTargetValue { get; set; }
        public string StartDateValue { get; set; }
        public string EndDateValue { get; set; }
        public string StartTimeValue { get; set; }
        public string EndTimeValue { get; set; }
        public int StartMinute { get; set; }
        public int EndMinute { get; set; }

        public int StartTimeIndex { get; set; }
        public int EndTimeIndex { get; set; }


        public string TimeStart { get; set; }
        public string TimeEnd { get; set; }


        public ObservableCollection<string> IsTargetItems { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<int> SearchResultAppearCountItems { get; set; }
        public int AppearCountValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<int> ThresholdItems { get; set; }
        public int ThresholdValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<string> TemplateStoreItems { get; set; }
        public string TemplateStoreValue { get; set; }
        public int TemplateStoreIndex { get; set; }

        public ObservableCollection<string> StartTimeItems { get; set; }
        public ObservableCollection<string> EndTimeItems { get; set; }
        public ObservableCollection<int> StartMinuteItems { get; set; }
        public ObservableCollection<int> EndMinuteItems { get; set; }

        public SearchSchemaModel()
        {
            initBasicDatas();
        }

        public SearchSchemaModel(bool andvance)
        {
            initBasicDatas();
            SearchResultAppearCountItems = new ObservableCollection<int>();
            ThresholdItems = new ObservableCollection<int>();
            TemplateStoreItems = new ObservableCollection<string>();

            for (int i = 1; i <= 100; i++)
            {
                ThresholdItems.Add(i);
                switch (i)
                {
                    case 10:
                        SearchResultAppearCountItems.Add(i);
                        break;
                    case 15:
                        SearchResultAppearCountItems.Add(i);
                        break;
                    case 20:
                        SearchResultAppearCountItems.Add(i);
                        break;
                    case 25:
                        SearchResultAppearCountItems.Add(i);
                        break;
                    case 30:
                        SearchResultAppearCountItems.Add(i);
                        break;
                    case 35:
                        SearchResultAppearCountItems.Add(i);
                        break;
                    case 40:
                        SearchResultAppearCountItems.Add(i);
                        break;
                    case 45:
                        SearchResultAppearCountItems.Add(i);
                        break;
                    case 50:
                        SearchResultAppearCountItems.Add(i);
                        break;
                    case 55:
                        SearchResultAppearCountItems.Add(i);
                        break;
                    case 60:
                        SearchResultAppearCountItems.Add(i);
                        break;
                    case 65:
                        SearchResultAppearCountItems.Add(i);
                        break;
                    case 70:
                        SearchResultAppearCountItems.Add(i);
                        break;
                    case 75:
                        SearchResultAppearCountItems.Add(i);
                        break;
                    case 80:
                        SearchResultAppearCountItems.Add(i);
                        break;
                    case 85:
                        SearchResultAppearCountItems.Add(i);
                        break;
                    case 90:
                        SearchResultAppearCountItems.Add(i);
                        break;
                    case 95:
                        SearchResultAppearCountItems.Add(i);
                        break;
                    case 100:
                        SearchResultAppearCountItems.Add(i);
                        break;
                }
            }
            //TemplateStoreItems.Add("普通");
            StartDateValue = DateTime.Now.ToString("yyyy/MM/dd");
            EndDateValue = DateTime.Now.ToString("yyyy/MM/dd");
            ThresholdValue = 50;

            TemplateStoreItems.Add("模板库");// 0
            TemplateStoreItems.Add("抓拍库");// 1
            //TemplateStoreValue = "模板库";
            TemplateStoreIndex = 0;
            AppearCountValue = 10;
        }

        void initBasicDatas()
        {
            StartTimeItems = new ObservableCollection<string>();
            EndTimeItems = new ObservableCollection<string>();
            StartMinuteItems = new ObservableCollection<int>();
            EndMinuteItems = new ObservableCollection<int>();
            IsTargetItems = new ObservableCollection<string>();
            for (int i = 0; i <= 60; i++)
            {
                if (i <= 24)
                {
                    StartTimeItems.Add(i + ":00");
                }
                StartMinuteItems.Add(i);
            }
            EndTimeItems = StartTimeItems;
            EndMinuteItems = StartMinuteItems;
            IsTargetItems.Add("无目标分析");
            IsTargetItems.Add("有目标分析");
        }

    }
}
