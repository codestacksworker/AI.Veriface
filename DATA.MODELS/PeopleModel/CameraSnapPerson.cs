using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace PeopleModel
{
    public partial class CameraSnapPerson
    {
        public int RowNumber { get; set; }
        /// <summary>
        /// 抓拍ID
        /// </summary>
        public string SnapId { get; set; }
        public string Birthday { get; set; }
        
        public string HomeAddress { get; set; }

        public string IDCard { get; set; }

        public string NameTitle { get; set; }
        public string Name { get; set; }

        public string Source { get; set; }

        public int SourceStoreKey { get; set; }

        public string SourceStoreValue { get; set; }

        public ObservableCollection<string> MyProperty { get; set; }

        public Camera CameraInfo { get; set; }

        public string SnapDate { get; set; }
        public string SnapTime { get; set; }

        public string Datetime { get; set; }
        public int Score { get; set; }
        public int DataIndex { get; set; }

        public ImageSource Photo { get; set; }
        public byte[] PhotoByteArray { get; set; }
        
        public TmpSnapInfo TmpSnapTopInfo { get; set; }
        /// <summary>
        /// 20170522 查询数据中的照片数组
        /// </summary>
        public byte[] SourcePhotoByteArray { get; set; }
        /// <summary>
        /// 主照片ID
        /// </summary>
        public int Main_ftID { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
        public int SST { get; set; }
        public int Exten { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        public long Tm { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }

    }
}
