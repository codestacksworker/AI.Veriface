using System;
using System.Windows.Media;

namespace SENSING.ClassPool
{
    #region 与接口中的表对应
    #region 识别结果信息显示
    public class PublishResult//来自于比对记录查询和模板
    {
        /// <summary>
        /// sanp id -guid
        /// </summary>
        public string ID { get; set; } 		// 标识ID，抓拍id，提醒邓工
        /// <summary>
        /// 
        /// </summary>
        public string RegID { get; set; } 		// 标识ID，抓拍id，提醒邓工
        /// <summary>
        /// 
        /// </summary>
        public ImageSource SanpImage { get; set; }		// 抓拍照片
        /// <summary>
        /// 
        /// </summary>
        public byte[] SnapImageBuffer { get; set; }
        /// <summary>
        /// main template image
        /// </summary>
        public ImageSource TemplateImage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public byte[] TemplateImageBuffer { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RegInfo { get; set; }    //注册照片信息集合 
        /// <summary>
        /// 
        /// </summary>
        public string ChannelName { get; set; }
        /// <summary>
        /// template type
        /// </summary>
        public string TemplateType { get; set; }
        /// <summary>
        /// old result obj
        /// </summary>
        public RealtimeCmpInfo Info { get; set; } //相关信息

        /// <summary>
        /// new result obj
        /// compare result
        /// </summary>
        public RealtimeCmpInfoLBS NewCmpResult { get; set; }
    }
    #endregion


    #region 通道
    public class MyChannelCfg
    {
        public int ChannelType { get; set; }
        public string TcChaneelID { get; set; }
        public string TcUID { get; set; }
        public string TcPSW { get; set; }
        public string Name { get; set; }
        public string TcDescription { get; set; }
        public CaptureCfg CaptureCfg { get; set; }
        public CatchFaceCfg CatchFaceCfg { get; set; }
        public string Addr { get; set; }
        public int Port { get; set; }
        public ImageSource ImageSource { get; set; }

        #region MyRegion

        public string Channel_address { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        #endregion

        public ChannelCfg MyChannelCfgToChannelCfg(MyChannelCfg _MyChannelCfg)
        {
            ChannelCfg _ChannelCfg = new ChannelCfg();
            _ChannelCfg.TcChaneelID = _MyChannelCfg.TcChaneelID;
            _ChannelCfg.TcUID = _MyChannelCfg.TcUID;
            _ChannelCfg.TcPSW = _MyChannelCfg.TcPSW;
            _ChannelCfg.Name = _MyChannelCfg.Name;
            _ChannelCfg.TcDescription = _MyChannelCfg.TcDescription;
            _ChannelCfg.CaptureCfg = _MyChannelCfg.CaptureCfg;
            _ChannelCfg.CatchFaceCfg = _MyChannelCfg.CatchFaceCfg;
            _ChannelCfg.Addr = _MyChannelCfg.Addr;
            _ChannelCfg.Port = _MyChannelCfg.Port;
            return _ChannelCfg;
        }

        public MyChannelCfg ChannelCfgToMyChannelCfg(ChannelCfg _ChannelCfg)
        {
            MyChannelCfg _MyChannelCfg = new MyChannelCfg();
            _MyChannelCfg.TcChaneelID = _ChannelCfg.TcChaneelID;
            _MyChannelCfg.TcUID = _ChannelCfg.TcUID;
            _MyChannelCfg.TcPSW = _ChannelCfg.TcPSW;
            _MyChannelCfg.Name = _ChannelCfg.Name;
            _MyChannelCfg.TcDescription = _ChannelCfg.TcDescription;
            _MyChannelCfg.CaptureCfg = _ChannelCfg.CaptureCfg;
            _MyChannelCfg.CatchFaceCfg = _ChannelCfg.CatchFaceCfg;
            _MyChannelCfg.Addr = _ChannelCfg.Addr;
            _MyChannelCfg.Port = _ChannelCfg.Port;
            return _MyChannelCfg;
        }

        public ChannelCfgLBS MyChannelCfgToChannelCfgLBS(MyChannelCfg _MyChannelCfg)
        {
            ChannelCfgLBS _ChannelCfg = new ChannelCfgLBS();
            _ChannelCfg.TcChannelID = _MyChannelCfg.TcChaneelID;
            _ChannelCfg.TcUID = _MyChannelCfg.TcUID;
            _ChannelCfg.TcPSW = _MyChannelCfg.TcPSW;
            _ChannelCfg.Name = _MyChannelCfg.Name;
            _ChannelCfg.TcDescription = _MyChannelCfg.TcDescription;
            _ChannelCfg.CaptureCfg = _MyChannelCfg.CaptureCfg;
            _ChannelCfg.CatchFaceCfg = _MyChannelCfg.CatchFaceCfg;
            _ChannelCfg.Addr = _MyChannelCfg.Addr;
            _ChannelCfg.Channel_address = _MyChannelCfg.Channel_address;
            _ChannelCfg.Port = _MyChannelCfg.Port;
            _ChannelCfg.Latitude = _MyChannelCfg.Latitude;
            _ChannelCfg.Longitude = _MyChannelCfg.Longitude;
            return _ChannelCfg;
        }
        public MyChannelCfg ChannelCfgToMyChannelCfgLBS(ChannelCfgLBS _ChannelCfg)
        {
            MyChannelCfg _MyChannelCfg = new MyChannelCfg();
            #region 20170531
            //if (_ChannelCfg.TcChannelID.Contains("\\"))
            //{
            //    _ChannelCfg.TcChannelID = _ChannelCfg.TcChannelID.Replace("\\","\\\\");
            //}
            #endregion
            _MyChannelCfg.TcChaneelID = _ChannelCfg.TcChannelID;
            _MyChannelCfg.ChannelType = _ChannelCfg.Channel_type;
            _MyChannelCfg.TcUID = _ChannelCfg.TcUID;
            _MyChannelCfg.TcPSW = _ChannelCfg.TcPSW;
            _MyChannelCfg.Name = _ChannelCfg.Name;
            _MyChannelCfg.TcDescription = _ChannelCfg.TcDescription;
            _MyChannelCfg.CaptureCfg = _ChannelCfg.CaptureCfg;
            _MyChannelCfg.CatchFaceCfg = _ChannelCfg.CatchFaceCfg;
            _MyChannelCfg.Addr = _ChannelCfg.Addr;
            _MyChannelCfg.Channel_address = _ChannelCfg.Channel_address;
            _MyChannelCfg.Port = _ChannelCfg.Port;
            _MyChannelCfg.Latitude = _ChannelCfg.Latitude;
            _MyChannelCfg.Longitude = _ChannelCfg.Longitude;
            return _MyChannelCfg;
        }

    }
    #endregion
    #region 识别结果弹出窗口
    public class MyIdentifyResultsWinShow
    {
        public string ID { get; set; }//模板ID

        public ImageSource img { get; set; }//模板照片

        public string name { get; set; }//模板名称

        public int score { get; set; }//相似度
    }
    #endregion
    #region 比对记录查询
    public class MyCmpFaceLogWidthImg
    {
        public int num { get; set; }
        public string ID { get; set; } 		// 标识ID，抓拍id，提醒邓工
        public string name { get; set; }	 	// 姓名
        public string channel { get; set; }	// 抓拍通道
        public string channelName { get; set; }	// 抓拍通道名称
        public string date { get; set; }
        public string time { get; set; }	 	// 抓拍时间
        public int TypeKey { get; set; }
        public string type { get; set; }		// 注册类型
        public int score { get; set; }	 	// 相似度
        public ImageSource img { get; set; }		// 照片
        public string tcUuid { get; set; }   // uuid，模板对象ID
        public bool IsChecked { get; set; } //是否选中推送
    }
    #endregion
    #region 抓拍记录查询
    public class MyCapFaceLogWithImg
    {
        public int Id { get; set; }
        public string ID { get; set; }		// 抓拍id
        public string ChannelID { get; set; } // 通道id 
        public string ChannelName { get; set; } // 通道名称
        public string time { get; set; }		// 抓拍时间
        public string timeIn { get; set; }	// 对象进入镜头时间
        public string timeOut { get; set; }	// 对象离开镜头时间
        public int quality { get; set; }		// 图像质量
        public int age { get; set; }			// 年龄
        public int gender { get; set; }		// 性别
        public ImageSource img { get; set; }

        #region 20170322
        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public int Score { get; set; }
        #endregion
    }
    #endregion
    #region 模板管理显示
    public class MyFaceObj
    {
        public FaceObj faceObj { get; set; }
        public int ID { get; set; }
        public string fa_ob_tcUuid { get; set; }	// 人脸uuid
        public string tcName { get; set; }		// 姓名
        public int nMain_ftID { get; set; }		// 人脸首选模板标识序号
        public string nType { get; set; }			// 类型
        public int nSST { get; set; }			// 敏感等级
        public int nExten { get; set; }			// 额外信息，报警信息是否输出等
        public string nSex { get; set; }			// 性别（0，未知；1，男；2，女）
        public int nAge { get; set; }			// 年龄
        public string fa_ob_dTm { get; set; }			// 人脸对象添加时间
        public string fa_ob_tcRemarks { get; set; }  	// 备注
        // 照片模板
        public string temp_tcUuid { get; set; }		// 模板uuid
        public string tcObjid { get; set; }		// 所属FaceObj的uuid
        public string tcKey { get; set; }		// 模板标识键（抓拍工作产生的人脸模板时，设置对应的设备通道标识 ID）
        public int nIndex { get; set; }			// 模板序号
        public DateTime temp_dTm { get; set; }			// 模板添加时间
        public string temp_tcRemarks { get; set; }	// 模板备注
        public ImageSource img { get; set; }			// 图像
        public byte[] imgStream { get; set; } //
    }
    #endregion
    public class RegisterPhoto
    {
        public string key { get; set; }
        public string value { get; set; }
    }
    #endregion
}
