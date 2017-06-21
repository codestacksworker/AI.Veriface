using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zhangxiaowen.i20170111.Sensing.Zhangxiaowen.i20170111.Sensing.Units;

namespace Zhangxiaowen.i20170111.Sensing
{
    public partial class ZhangXiaowen
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static double GetFileSize(string filePath, SizeUnits units, ref string msg)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            double fileSize = 0;
            switch (units)
            {
                case SizeUnits.bit:
                    break;
                case SizeUnits.Byte:
                    fileSize = fileInfo.Length;
                    msg = fileSize + "B";
                    break;
                case SizeUnits.KByte:
                    fileSize = fileInfo.Length / 1024;
                    msg = fileSize + "KB";
                    break;
                case SizeUnits.MByte:
                    fileSize = fileInfo.Length / 1024 / 1024;
                    msg = fileSize + "MB";
                    break;
                case SizeUnits.GByte:
                    fileSize = fileInfo.Length / 1024 / 1024 / 1024;
                    msg = fileSize + "GB";
                    break;
                case SizeUnits.TByte:
                    fileSize = fileInfo.Length / 1024 / 1024 / 1024 / 1024;
                    msg = fileSize + "TB";
                    break;
                default:
                    break;
            }
            return fileSize;
        }

    }
    namespace Zhangxiaowen.i20170111.Sensing.Units
    {
        public enum SizeUnits
        {
            bit = 1,
            Byte = 2,
            KByte = 3,
            MByte = 4,
            GByte = 5,
            TByte = 6
        }
    }
}
