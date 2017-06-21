using SENSING.ClassPool;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Windows.Input;
using xiaowen.codestacks.popwindow;
using SENSING.THRIFT.Services;
using xiaowen.codestacks.wpf.Utilities;

namespace FaceSysByMvvm.ViewModels.TemplateManager //SENSING_SINGLEUSER.ViewModels.TemplateManager
{
    public partial class TemplateManagerViewModel : BindableBase
    {
        public ICommand GotoTraceAnalysisCommand { get; private set; }
        public ICommand CmdMarkUpKeyObject { get; private set; }
        public ICommand CmdCancelMarkUpKeyObject { get; private set; }

        void initCmd()
        {
            GotoTraceAnalysisCommand = new DelegateCommand<object>(GotoTraceAnalysisFunc);
            CmdMarkUpKeyObject = new DelegateCommand<object>(CmdMarkUpKeyObjectFunc);
            CmdCancelMarkUpKeyObject = new DelegateCommand<object>(CmdCancelMarkUpKeyObjectFunc);
        }

        private void CmdCancelMarkUpKeyObjectFunc(object obj)
        {
            MyFaceObj myFaceObj = obj as MyFaceObj;
            string guid = myFaceObj.faceObj.TcUuid;

            ThriftServiceUtilities thrift = new ThriftServiceUtilities();
            if (thrift.MarkUpKeyObject(guid, -1) == 0)
                CodeStacksWindow.MessageBox.Invoke(false, false, 1, "操作成功");
            else
                CodeStacksWindow.MessageBox.Invoke(false, false, 1, "操作失败");
            //取消标记重点
        }

        private void CmdMarkUpKeyObjectFunc(object obj)
        {
            MyFaceObj myFaceObj = obj as MyFaceObj;
            string guid = myFaceObj.faceObj.TcUuid;

            ThriftServiceUtilities thrift = new ThriftServiceUtilities();
            if (thrift.MarkUpKeyObject(guid, 1) == 0)
                CodeStacksWindow.MessageBox.Invoke(false, false, 1, "操作成功");
            else
                CodeStacksWindow.MessageBox.Invoke(false, false, 1, "操作失败");
            //标记重点
        }

        private void GotoTraceAnalysisFunc(object obj)
        {
            MyFaceObj myFaceObj = obj as MyFaceObj;
            try
            {
                if (obj != null && MyFaceObj.imgStream.Length > 0)
                {
                    ReflactionView.GoTo1(myFaceObj, 5, "HomeView", "FuncationToggleButton_Checked");
                }
                else
                {
                    CodeStacksWindow.MessageBox.Invoke(true, false, 2, "请选中要操作的一条数据");
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
