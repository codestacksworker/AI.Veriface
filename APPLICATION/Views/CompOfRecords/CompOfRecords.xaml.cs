using FaceSysByMvvm.ViewModels.CompOfRecords;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FaceSysByMvvm.Model;
using xiaowen.codestacks.popwindow;

namespace FaceSysByMvvm.Views.ChannelManager
{
    /// <summary>
    /// CompOfRecords.xaml 的交互逻辑
    /// </summary>
    public partial class CompOfRecords : UserControl
    {
        public CompOfRecordsViewModel cORViewModel;
        ThriftServiceNameSpace.ThriftService thirft = new ThriftServiceNameSpace.ThriftService();
        string exportCurrDay = "";

        public CompOfRecords()
        {
            InitializeComponent();
            cORViewModel = new CompOfRecordsViewModel();
            this.DataContext = cORViewModel;
            cORViewModel.CmpDataContext = cORViewModel;
        }

        /// <summary>
        /// 刷新通道下拉列表
        /// </summary>
        internal void RefreshChannelComboBox()
        {
            cORViewModel.RefreshChannelList();
        }

        ///// <summary>
        ///// 清除时间
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void ClearDatePickerTime(object sender, MouseButtonEventArgs e)
        //{
        //    DatePicker dp = sender as DatePicker;
        //    dp.Text = "";
        //}

        /// <summary>
        /// 导出excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ExportExcelFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string saveExcelPath = ConfigurationManager.AppSettings["Excel保存地址"];
                if (string.IsNullOrEmpty(saveExcelPath))
                {
                    saveExcelPath = System.Windows.Forms.Application.StartupPath + @"\比对记录导出";
                }
                else
                {
                    saveExcelPath += @"\比对记录导出";
                }
                await Task.Run(
                      () =>
                      {
                          XSSFWorkbook workbook = new XSSFWorkbook();
                          XSSFCellStyle style = (XSSFCellStyle)workbook.CreateCellStyle();
                          style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                          style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                          style.BorderBottom = BorderStyle.Thin;
                          style.BorderLeft = BorderStyle.Thin;
                          style.BorderRight = BorderStyle.Thin;
                          style.BorderTop = BorderStyle.Thin;
                          XSSFCellStyle style2 = (XSSFCellStyle)workbook.CreateCellStyle();
                          style2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                          style2.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                          style2.BorderBottom = BorderStyle.Thin;
                          style2.BorderLeft = BorderStyle.Thin;
                          style2.BorderRight = BorderStyle.Thin;
                          style2.BorderTop = BorderStyle.Thin;
                          XSSFFont font = (XSSFFont)workbook.CreateFont();
                          font.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
                          style2.SetFont(font);
                          XSSFCellStyle style0 = (XSSFCellStyle)workbook.CreateCellStyle();
                          style0.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                          style0.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                          style0.BorderBottom = BorderStyle.Thin;
                          style0.BorderLeft = BorderStyle.Thin;
                          style0.BorderRight = BorderStyle.Thin;
                          style0.BorderTop = BorderStyle.Thin;
                          IDataFormat dataformat = workbook.CreateDataFormat();
                          style0.DataFormat = dataformat.GetFormat("yyyy年MM月dd日 h:mm:ss 上午/下午");
                          XSSFSheet sheet = (XSSFSheet)workbook.CreateSheet("比对记录");
                          int row = 0;
                          List<MyCmpFaceLogWidthImgModel> allCmpInfo = new List<MyCmpFaceLogWidthImgModel>();
                          allCmpInfo = GetCmpFaceLogByPage(1);
                          foreach (var item in allCmpInfo)
                          {
                              if (row == 0)
                              {
                                  sheet.CreateRow(row).CreateCell(0).SetCellValue("序号");
                                  sheet.GetRow(row).CreateCell(1).SetCellValue("抓拍通道");
                                  sheet.GetRow(row).CreateCell(2).SetCellValue("抓拍时间");
                                  sheet.GetRow(row).CreateCell(3).SetCellValue("模版姓名");
                                  sheet.GetRow(row).CreateCell(4).SetCellValue("注册类型");
                                  sheet.GetRow(row).CreateCell(5).SetCellValue("相似度");
                                  sheet.GetRow(row).CreateCell(6).SetCellValue("抓拍照片");
                                  sheet.GetRow(row).CreateCell(7).SetCellValue("模版照片");
                                  sheet.GetRow(row).GetCell(0).CellStyle = style2;
                                  sheet.GetRow(row).GetCell(1).CellStyle = style2;
                                  sheet.GetRow(row).GetCell(2).CellStyle = style2;
                                  sheet.GetRow(row).GetCell(3).CellStyle = style2;
                                  sheet.GetRow(row).GetCell(4).CellStyle = style2;
                                  sheet.GetRow(row).GetCell(5).CellStyle = style2;
                                  sheet.GetRow(row).GetCell(6).CellStyle = style2;
                                  sheet.GetRow(row).GetCell(7).CellStyle = style2;
                                  sheet.SetColumnWidth(2, 30 * 256);
                                  sheet.SetColumnWidth(6, 30 * 256);
                                  sheet.SetColumnWidth(7, 30 * 256);
                                  row++;
                              }
                              sheet.CreateRow(row).CreateCell(0).SetCellValue(row);
                              sheet.GetRow(row).Height = 100 * 40;
                              sheet.GetRow(row).CreateCell(1).SetCellValue(item.channelName);
                              sheet.GetRow(row).CreateCell(2).SetCellValue(item.time);
                              sheet.GetRow(row).CreateCell(3).SetCellValue(item.name);
                              sheet.GetRow(row).CreateCell(4).SetCellValue(item.type);
                              sheet.GetRow(row).CreateCell(5).SetCellValue(item.score);
                              sheet.GetRow(row).CreateCell(6).SetCellValue("");
                              sheet.GetRow(row).CreateCell(7).SetCellValue("");
                              sheet.GetRow(row).GetCell(0).CellStyle = style;
                              sheet.GetRow(row).GetCell(1).CellStyle = style;
                              sheet.GetRow(row).GetCell(2).CellStyle = style0;
                              sheet.GetRow(row).GetCell(3).CellStyle = style;
                              sheet.GetRow(row).GetCell(4).CellStyle = style;
                              sheet.GetRow(row).GetCell(5).CellStyle = style;
                              sheet.GetRow(row).GetCell(6).CellStyle = style;
                              sheet.GetRow(row).GetCell(7).CellStyle = style;
                              if (true)
                              {
                                  XSSFDrawing patriarch = (XSSFDrawing)sheet.CreateDrawingPatriarch();
                                  List<byte[]> capImage = new List<byte[]>();
                                  capImage = thirft.QueryCmpLogImageH(item.ID, exportCurrDay);
                                  if (capImage.Count > 0)
                                  {
                                      byte[] bytes = capImage[0];
                                      int pictureIdx = workbook.AddPicture(bytes, PictureType.JPEG);
                                      XSSFClientAnchor anchor = new XSSFClientAnchor(0, 0, 0, 0, 6, row, 7, row + 1);
                                      XSSFPicture pict = (XSSFPicture)patriarch.CreatePicture(anchor, pictureIdx);
                                  }
                                  var faceObject = thirft.QueryFaceObj(item.tcUuid);
                                  if (faceObject.Count > 0 && faceObject[0].Tmplate.Count > 0)
                                  {
                                      byte[] bytes2 = faceObject[0].Tmplate[0].Img;
                                      int pictureIdx2 = workbook.AddPicture(bytes2, PictureType.JPEG);
                                      XSSFClientAnchor anchor2 = new XSSFClientAnchor(0, 0, 0, 0, 7, row, 8, row + 1);
                                      XSSFPicture pict2 = (XSSFPicture)patriarch.CreatePicture(anchor2, pictureIdx2);
                                  }
                              }
                              row++;
                              Thread.Sleep(1);
                          }
                          FileStream fs = new FileStream(saveExcelPath + ".xlsx", FileMode.Create, FileAccess.Write);
                          workbook.Write(fs);
                          workbook = null;
                      });
                CodeStacksWindow.MessageBox.Invoke(true, false, 2, "导出完成!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 根据页数获取抓拍记录
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private List<MyCmpFaceLogWidthImgModel> GetCmpFaceLogByPage(int page)
        {
            int curpage = 0;
            int index = 0;
            List<int> pageSplit = new List<int>();
            var queryCount = thirft.QueryCmpRecordTotalCountH(cORViewModel.CompOfRecordsValueObj);
            if (queryCount.Count <= 0)
            {
                return null;
            }
            cORViewModel.CompOfRecordsValueObj.MaxDataCount = queryCount[0].Count;
            for (int no = queryCount[0].Dayarr.Count - 1; no >= 0; no--)
            {
                var dayary = queryCount[0].Dayarr[no];
                curpage += dayary.Count % cORViewModel.CompOfRecordsValueObj.PageSize != 0 ? dayary.Count / cORViewModel.CompOfRecordsValueObj.PageSize + 1 : dayary.Count / cORViewModel.CompOfRecordsValueObj.PageSize;
                pageSplit.Add(curpage);
            }
            cORViewModel.MaxPage = curpage;
            foreach (var dayPage in pageSplit)
            {
                if (page <= dayPage)
                {
                    index = pageSplit.IndexOf(dayPage);
                    exportCurrDay = queryCount[0].Dayarr[queryCount[0].Dayarr.Count - 1 - index].Daystr;
                    DateTime dt1 = Convert.ToDateTime(queryCount[0].Dayarr[queryCount[0].Dayarr.Count - 1 - index].Daystr.Insert(6, "/").Insert(4, "/"));
                    TimeSpan dt1Span = new TimeSpan(dt1.Ticks);
                    DateTime dt2 = new DateTime(1970, 1, 1);
                    TimeSpan dt2Span = new TimeSpan(dt2.Ticks);
                    long longdtPkCompRecordStarTime = Convert.ToInt64(dt1Span.TotalSeconds - dt2Span.TotalSeconds);
                    if (longdtPkCompRecordStarTime > cORViewModel.CompOfRecordsValueObj.StartDayValue)
                    {
                        cORViewModel.CompOfRecordsValueObj.StartDayValue = longdtPkCompRecordStarTime;
                    }
                    if (page != cORViewModel.MaxPage)
                    {
                        long longdtPkCompRecordEndTime = Convert.ToInt64(dt1Span.TotalSeconds - dt2Span.TotalSeconds) + 24 * 60 * 60;
                        var todayEndtime = Convert.ToInt64(new TimeSpan(dt1.AddDays(1).Ticks).TotalSeconds - dt2Span.TotalSeconds);
                        if (longdtPkCompRecordEndTime > todayEndtime)
                        {
                            longdtPkCompRecordEndTime = todayEndtime;
                        }
                        cORViewModel.CompOfRecordsValueObj.EndDayValue = longdtPkCompRecordEndTime;
                    }
                    cORViewModel.CompOfRecordsValueObj.MaxDataCount = queryCount[0].Dayarr[queryCount[0].Dayarr.Count - 1 - index].Count;
                    break;
                }
            }
            if (page > 1)
            {
                int pageTem = 0;
                if (index == 0)
                {
                    pageTem = 0;
                }
                else
                {
                    pageTem = pageSplit[index - 1];
                }
                cORViewModel.CompOfRecordsValueObj.PageStartRow = cORViewModel.CompOfRecordsValueObj.MaxDataCount - ((page - pageTem) * cORViewModel.CompOfRecordsValueObj.PageSize);
                if (cORViewModel.CompOfRecordsValueObj.PageStartRow < 0)
                {
                    cORViewModel.CompOfRecordsValueObj.PageStartRow = 0;
                }
            }
            else
            {
                cORViewModel.CompOfRecordsValueObj.PageStartRow = cORViewModel.CompOfRecordsValueObj.MaxDataCount - (cORViewModel.CompOfRecordsValueObj.PageSize);
                if (cORViewModel.CompOfRecordsValueObj.PageStartRow < 0)
                {
                    cORViewModel.CompOfRecordsValueObj.PageStartRow = 0;
                }
            }
            return thirft.QueryCmpLogOld(cORViewModel.CompOfRecordsValueObj);
        }
    }
}