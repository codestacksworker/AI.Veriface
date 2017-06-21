using SENSING.ClassPool;
using System.Windows.Media;
using sensing = xiaowen.codestacks.data.SenSingModels;

namespace FaceSysByMvvm.Model
{
    public class MyCmpFaceLogWidthImgModel : MyCmpFaceLogWidthImg
    {
        #region 20170322

        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int Score { get; set; }
        /// <summary>
        /// 出现次数
        /// </summary>
        public long AppearCount { get; set; }

        #endregion

        /// <summary>
        /// 抓拍人像
        /// </summary>
        public ImageSource SnapImage { get; set; }
        public byte[] SnapImageBuffer { get; set; }
        /// <summary>
        /// 数据库人像
        /// </summary>
        public ImageSource TemplatePhoto { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ImageSource Background { get; set; }
        ///// <summary>
        ///// 抓拍场景
        ///// </summary>
        //public ImageSource EnvironmentPhoto { get; set; }
        public CompOfRecordTemplate T1 { get; set; }
        public CompOfRecordTemplate T2 { get; set; }
        public CompOfRecordTemplate T3 { get; set; }
        public CompOfRecordTemplate T4 { get; set; }
        public CompOfRecordTemplate T5 { get; set; }


        public static sensing.Compare DataConvertToCmpare(MyCmpFaceLogWidthImgModel thisObj)
        {
            sensing.Compare compare = new sensing.Compare();
            compare.Row = thisObj.num;
            compare.Score = thisObj.Score == 0 ? thisObj.score : thisObj.Score;
            compare.Snap = new sensing.Snap();
            compare.Snap.Guid = thisObj.ID;
            compare.Snap.Photo = thisObj.SnapImage;
            //compare.Snap.EnvironmentPhoto = thisObj.EnvironmentPhoto;
            compare.Template = new sensing.Template();
            compare.Template.Guid = thisObj.tcUuid;
            compare.Template.TypeKey = thisObj.TypeKey;
            compare.Template.TypeValue = thisObj.type;
            compare.Template.PersonInfo = new sensing.Person();
            compare.Template.PersonInfo.Name = thisObj.name;
            compare.Template.PersonInfo.Photo = thisObj.TemplatePhoto;
            compare.Snap.DateTime = thisObj.date + " " + thisObj.time;
            compare.Camera = new sensing.Camera();
            compare.Camera.Guid = thisObj.channel;
            compare.Camera.Location = thisObj.channelName;

            return compare;
        }

        public static void DataConvertToCapture(MyCapFaceLogWithImg thisObj, sensing.Compare compare)
        {
            //compare.Row = thisObj.num;
            //compare.Snap = new sensing.Snap();
            //compare.Snap.Guid = thisObj.ID;
            //compare.Snap.Photo = thisObj.;
            //compare.Template = new sensing.Template();
            //compare.Template.TypeValue = thisObj.type;
            //compare.Template.PersonInfo = new sensing.Person();
            //compare.Template.PersonInfo.Name = thisObj.name;
            //compare.Template.PersonInfo.Photo = thisObj.TemplatePhoto;
            //compare.Snap.DateTime = thisObj.time;
            //compare.Camera = new sensing.Camera();
            //compare.Camera.Guid = thisObj.channel;
            //compare.Camera.Location = thisObj.channelName;
        }

    }
}
