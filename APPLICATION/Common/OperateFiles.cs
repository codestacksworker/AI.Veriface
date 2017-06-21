using DATA.UTILITIES.Log4Net;
using SENSING.ClassPool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xiaowen.codestacks.popwindow;

namespace SENSING.APPLICATION.Common
{
    /// <summary>
    /// 公共类 操作文件夹和文件类
    /// </summary>
    public class OperateFiles
    {
        /// <summary>
        /// 获取文件夹的所有图片文件
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="bIsContainsSubFold"></param>
        /// <returns></returns>
        public static FileInfo[] GetFilesPic(string folder, bool bIsContainsSubFold)//传入参数是文件夹路径
        {
            try
            {
                DirectoryInfo directory = new DirectoryInfo(folder);
                if (directory.Exists)
                {
                    //文件夹及子文件夹下的所有文件的全路径
                    FileInfo[] files = null;
                    if (bIsContainsSubFold)
                    {
                        files = directory.GetFiles("*.jpg", SearchOption.AllDirectories);
                    }
                    else
                    {
                        files = directory.GetFiles("*.jpg");
                    }
                    return files;
                }
                CodeStacksWindow.MessageBox.Invoke(true, false, 2, "文件不存在");
                return null;
            }
            catch (Exception ex)
            {
                Logger<OperateFiles>.Log.Error("GetFilesPic", ex);
                return null;
            }
        }

        /// <summary>
        /// 读取图片文件，返回总信息条数
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public static void GetNumReadPic(FileInfo[] files, ref int nNunPic)
        {
            try
            {
                for (int i = 0; i < files.Length; i++)
                {
                    string strPathName = (files[i].DirectoryName + "\\" + files[i].Name);
                    if (File.Exists(strPathName))
                    {
                        nNunPic++;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger<OperaExcel>.Log.Error("OpenCSVReadNum", ex);
                return;
            }
        }
    }
}
