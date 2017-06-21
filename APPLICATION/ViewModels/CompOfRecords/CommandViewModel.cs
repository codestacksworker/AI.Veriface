using FaceSysByMvvm.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows.Input;
using xiaowen.codestacks.popwindow;
using xiaowen.codestacks.wpf.Utilities;

namespace FaceSysByMvvm.ViewModels.CompOfRecords
{
    public partial class CompOfRecordsViewModel : BindableBase
    {
        public ICommand SearchCommand { get; private set; }
        public ICommand FirstPageCommand { get; private set; }
        public ICommand PrevPageCommand { get; private set; }
        public ICommand NextPageCommand { get; private set; }
        public ICommand LastPageCommand { get; private set; }
        public ICommand JumpToPageIndexCommand { get; private set; }
        public ICommand GotoBICommand { get; private set; }
        public ICommand GotoTRCommand { get; private set; }
        public ICommand PublishCompareResultCmd { get; set; }

        void InitCmd()
        {
            SearchCommand = new DelegateCommand<Object>(SearchCommandFunc);
            FirstPageCommand = new DelegateCommand<object>(FirstPageCommandFunc);
            PrevPageCommand = new DelegateCommand<object>(PrevPageCommandFunc);
            NextPageCommand = new DelegateCommand<object>(NextPageCommandFunc);
            LastPageCommand = new DelegateCommand<object>(LastPageCommandFunc);
            JumpToPageIndexCommand = new DelegateCommand<object>(JumpToPageIndexCommandFunc);

            GotoBICommand = new DelegateCommand<object>((obj) =>
            {
                MyCmpFaceLogWidthImgModel model = obj as MyCmpFaceLogWidthImgModel;
                if (string.IsNullOrEmpty(model.T1.UserName) && string.IsNullOrEmpty(model.T1.TemplateType))
                    CodeStacksWindow.MessageBox.Invoke(false, false, 2, "因智能分析取自模板照片\n请添加模板照片后重新进入智能分析功能");
                else
                    ReflactionView.GoTo1(model, 6, "HomeView", "FuncationToggleButton_Checked");
            });

            GotoTRCommand = new DelegateCommand<object>((obj) =>
            {
                MyCmpFaceLogWidthImgModel model = obj as MyCmpFaceLogWidthImgModel;
                ReflactionView.GoTo1(model, 5, "HomeView", "FuncationToggleButton_Checked");
            });
            PublishCompareResultCmd = new DelegateCommand<object>(PublishCompareResultCmdFunc);
            getCompOfRecordsDelegate = GetAllInfo;
        }


    }
}
