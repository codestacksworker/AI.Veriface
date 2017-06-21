using ThriftServiceNameSpace;
using Prism.Mvvm;
using System.Collections.Generic;

namespace FaceSysByMvvm.ViewModels.TemplateManager
{
    public class TempleteInfoPopViewModel : BindableBase
    {
        #region TemplateGallery



        #endregion

        #region 属性的声明
        //窗体标题
        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                SetProperty(ref title, value);
            }
        }

        //模版姓名     
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                SetProperty(ref name, value);
            }
        }
        //模版ID     
        private string id;
        public string Id
        {
            get { return id; }
            set
            {
                SetProperty(ref id, value);
            }
        }
        //模版年龄
        private string age;
        public string Age
        {
            get { return age; }
            set
            {
                SetProperty(ref age, value);
            }
        }
        //模版导入时间    
        private string importTime;
        public string ImportTime
        {
            get { return importTime; }
            set
            {
                SetProperty(ref importTime, value);
            }
        }
        //模版备注 
        private string remark;
        public string Remark
        {
            get { return remark; }
            set
            {
                SetProperty(ref remark, value);
            }
        }

        //模版类型      
        private int selectedType;
        public int SelectedType
        {
            get { return selectedType; }
            set
            {
                SetProperty(ref selectedType, value);
            }
        }
        private List<string> type;
        public List<string> Type
        {
            get { return type; }
            set
            {
                SetProperty(ref type, value);
            }
        }

        //模版性别
        private int selectedSex;
        public int SelectedSex
        {
            get { return selectedSex; }
            set
            {
                SetProperty(ref selectedSex, value);
            }
        }

        private List<string> sex;
        public List<string> Sex
        {
            get { return sex; }
            set
            {
                SetProperty(ref sex, value);
            }
        }

        #endregion

        public FaceObj _FaceObj;
        #region 初始化
        public TempleteInfoPopViewModel()
        {
            ThriftServiceNameSpace.ThriftService thirft = new ThriftServiceNameSpace.ThriftService();
            _FaceObj = new FaceObj();
            //初始化模版类型
            Type = thirft.QueryDefFaceObjType();
            SelectedType = 0;
            //初始化模版性别            
            Sex = new List<string>() { "未知", "男", "女" };
        }
        #endregion
    }
}
