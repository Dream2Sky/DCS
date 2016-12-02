using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using com.dcs.common;
using com.dcs.entity;

namespace com.dcs.web.Globals
{
    public class ExcelManager
    {
        private int baseColCount = 22;
        private IWorkbook _workbook { get; set; }
        private string _filename { get; set; }
        private ISheet _currentSheet { get; set; }

        public void Open(string filename)
        {
            try
            {
                _filename = AppDomain.CurrentDomain.BaseDirectory + "/Files/" + filename;
                FileStream fs = new FileStream(_filename, FileMode.Open, FileAccess.Read);
                if (_filename.IndexOf(".xlsx") > 0)
                {
                    _workbook = new XSSFWorkbook(fs);
                }
                else if (_filename.IndexOf(".xls") > 0)
                {
                    _workbook = new HSSFWorkbook(fs);
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);
                throw;
            }
        }

        public void Close()
        {
            try
            {
                _workbook.Close();

                FileInfo fi = new FileInfo(_filename);
                fi.Delete();
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                throw;
            }
        }

        public void GetDataFromExcel(string account, ref List<Information> informationList,
            ref List<CustomItem> customItemList, ref List<CustomItemValue> customItemValueList)
        {
            informationList = GetInformation();
            GetCustomItemAndValue(account, ref informationList, ref customItemList, ref customItemValueList);
        }

        /// <summary>
        /// 获取excel表格的 information 
        /// </summary>
        /// <returns></returns>
        private List<Information> GetInformation()
        {
            try
            {
                List<Information> informationList = new List<Information>();
                for (int i = 0; i < _workbook.NumberOfSheets; i++)
                {
                    _currentSheet = _workbook.GetSheetAt(i);

                    IRow firstRow = _currentSheet.GetRow(0);
                    if (firstRow == null)
                    {
                        continue;
                    }

                    int columnCount = firstRow.LastCellNum;

                    for (int k = 1; k < _currentSheet.LastRowNum + 1; k++)
                    {
                        Information infor = new Information();
                        IRow _row = _currentSheet.GetRow(k);

                        infor.CustomerName = _row.GetCell(0) == null ? null : _row.GetCell(0).StringCellValue;
                        infor.Sex = _row.GetCell(1) == null ? -1 : _row.GetCell(1).StringCellValue.Contains("男") ? 1 : 0;
                        try
                        {
                            infor.Age = _row.GetCell(2) == null ? -1 : (int)_row.GetCell(2).NumericCellValue;
                        }
                        catch (Exception)
                        {
                            infor.Age = _row.GetCell(2) == null ? -1 : int.Parse(_row.GetCell(2).StringCellValue);
                        }

                        infor.IsMarry = _row.GetCell(3) == null ? -1 : _row.GetCell(3).StringCellValue.Contains("已婚") ? 1 : 0;
                        infor.Children = _row.GetCell(4) == null ? -1 : _row.GetCell(4).StringCellValue.Contains("有") ? 1 : 0;
                        try
                        {
                            infor.Phone = _row.GetCell(5) == null ? null : _row.GetCell(5).NumericCellValue.ToString();
                        }
                        catch (Exception)
                        {
                            infor.Phone = _row.GetCell(5) == null ? null : _row.GetCell(5).StringCellValue;
                        }

                        try
                        {
                            infor.QQ = _row.GetCell(6) == null ? null : _row.GetCell(6).NumericCellValue.ToString();
                        }
                        catch (Exception)
                        {
                            infor.QQ = _row.GetCell(6) == null ? null : _row.GetCell(6).StringCellValue;
                        }

                        infor.WebCat = _row.GetCell(7) == null ? null : _row.GetCell(7).StringCellValue;
                        infor.Email = _row.GetCell(8) == null ? null : _row.GetCell(8).StringCellValue;
                        infor.Address = _row.GetCell(9) == null ? null : _row.GetCell(9).StringCellValue;
                        infor.Industry = _row.GetCell(10) == null ? null : _row.GetCell(10).StringCellValue;
                        infor.Occupation = _row.GetCell(11) == null ? null : _row.GetCell(11).StringCellValue;
                        try
                        {
                            infor.Income = _row.GetCell(12) == null ? null : _row.GetCell(12).NumericCellValue.ToString();
                        }
                        catch (Exception)
                        {
                            infor.Income = _row.GetCell(12) == null ? null : _row.GetCell(12).StringCellValue;
                        }
                        infor.Hobby = _row.GetCell(13) == null ? null : _row.GetCell(13).StringCellValue;
                        infor.HasCar = _row.GetCell(14) == null ? -1 : _row.GetCell(14).StringCellValue.Contains("有") ? 1 : 0;
                        infor.HasHouse = _row.GetCell(15) == null ? -1 : _row.GetCell(15).StringCellValue.Contains("有") ? 1 : 0;
                        infor.InvestProj = _row.GetCell(16) == null ? null : _row.GetCell(16).StringCellValue;
                        infor.InvestConc = _row.GetCell(17) == null ? null : _row.GetCell(17).StringCellValue;
                        infor.InvestLife = _row.GetCell(18) == null ? null : _row.GetCell(18).StringCellValue;
                        infor.Note1 = _row.GetCell(19) == null ? null : _row.GetCell(19).StringCellValue;
                        infor.Note2 = _row.GetCell(20) == null ? null : _row.GetCell(20).StringCellValue;
                        infor.Note3 = _row.GetCell(21) == null ? null : _row.GetCell(21).StringCellValue;

                        informationList.Add(infor);
                    }
                }

                return informationList;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// 获取excel表格的  自定义项 和 对应的值
        /// </summary>
        /// <param name="informationList"></param>
        /// <param name="customItemList"></param>
        /// <param name="customItemValueList"></param>
        private void GetCustomItemAndValue(string account, ref List<Information> informationList,
            ref List<CustomItem> customItemList, ref List<CustomItemValue> customItemValueList)
        {
            try
            {
                if (informationList == null || informationList.Count <= 0)
                {
                    return;
                }
                int lastSheetRowCount = 0;
                for (int i = 0; i < _workbook.NumberOfSheets; i++)
                {
                    _currentSheet = _workbook.GetSheetAt(i);

                    if (i > 0)
                    {
                        lastSheetRowCount += _workbook.GetSheetAt(i - 1).LastRowNum - 1;
                    }

                    IRow _row = _currentSheet.GetRow(0);
                    int colCount = _row.LastCellNum;
                    for (int k = baseColCount; k < colCount; k++)
                    {
                        var tmp = _row.GetCell(k) == null ? null : _row.GetCell(k).StringCellValue;
                        if (customItemList.SingleOrDefault(n => n.ItemContent == tmp) == null)
                        {
                            CustomItem ci = new CustomItem();
                            ci.ItemContent = tmp;
                            ci.ItemName = account + "C" + TimeManager.GetTimeSpan();
                            ci.InsertMember = account;
                            ci.InsertTime = DateTime.Now;
                            ci.IsDeleted = false;
                            ci.UpdateTime = DateTime.Now;

                            customItemList.Add(ci);
                        }

                        for (int j = 1; j < _currentSheet.LastRowNum + 1; j++)
                        {
                            var tmpvalue = _row.GetCell(k) == null ? null : _row.GetCell(k).StringCellValue;
                            var ci = customItemList.SingleOrDefault(n => n.ItemContent == tmpvalue);
                            if (ci == null)
                            {
                                break;
                            }
                            CustomItemValue cv = new CustomItemValue();
                            cv.CustomItemId = ci.Id;
                            cv.InsertTime = cv.UpdateTime = DateTime.Now;
                            cv.IsDeleted = false;
                            cv.ItemName = ci.ItemName;
                            cv.InforId = informationList[lastSheetRowCount + j - 1].Id;
                            try
                            {
                                cv.ItemValue = _currentSheet.GetRow(j).GetCell(k) == null ? null : _currentSheet.GetRow(j).GetCell(k).NumericCellValue.ToString();
                            }
                            catch (Exception)
                            {
                                cv.ItemValue = _currentSheet.GetRow(j).GetCell(k) == null ? null : _currentSheet.GetRow(j).GetCell(k).StringCellValue;
                            }

                            customItemValueList.Add(cv);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);
                throw;
            }
        }

        public MemoryStream DataTOExcel(List<Information> informationList,
            List<CustomItem> customItemList, List<CustomItemValue> customItemValueList)
        {
            try
            {
                _workbook = new HSSFWorkbook();
                _currentSheet = _workbook.CreateSheet("Sheet1");
                IRow firstRow = _currentSheet.CreateRow(0);

                firstRow.CreateCell(0).SetCellValue("录入时间");
                firstRow.CreateCell(1).SetCellValue("收集员");
                firstRow.CreateCell(2).SetCellValue("当前使用人");
                firstRow.CreateCell(3).SetCellValue("客户姓名");
                firstRow.CreateCell(4).SetCellValue("性别");
                firstRow.CreateCell(5).SetCellValue("年龄");
                firstRow.CreateCell(6).SetCellValue("婚否");
                firstRow.CreateCell(7).SetCellValue("子女");
                firstRow.CreateCell(8).SetCellValue("电话");
                firstRow.CreateCell(9).SetCellValue("QQ");
                firstRow.CreateCell(10).SetCellValue("微信");
                firstRow.CreateCell(11).SetCellValue("邮箱");
                firstRow.CreateCell(12).SetCellValue("通信地址");
                firstRow.CreateCell(13).SetCellValue("所属行业");
                firstRow.CreateCell(14).SetCellValue("职业");
                firstRow.CreateCell(15).SetCellValue("年收入");
                firstRow.CreateCell(16).SetCellValue("爱好");
                firstRow.CreateCell(17).SetCellValue("是否有车");
                firstRow.CreateCell(18).SetCellValue("是否有房");
                firstRow.CreateCell(19).SetCellValue("投资项目");
                firstRow.CreateCell(20).SetCellValue("投资项目");
                firstRow.CreateCell(21).SetCellValue("投资项目");
                firstRow.CreateCell(22).SetCellValue("备注1");
                firstRow.CreateCell(23).SetCellValue("备注2");
                firstRow.CreateCell(24).SetCellValue("备注3");
                for (int i = 0; i < customItemList.Count; i++)
                {
                    firstRow.CreateCell(25 + i).SetCellValue(customItemList[i].ItemContent);
                }

                int count = 1;
                foreach (var item in informationList)
                {
                    IRow row = _currentSheet.CreateRow(count);
                    row.CreateCell(0).SetCellValue(item.InsertTime.ToString());
                    row.CreateCell(1).SetCellValue(item.InsertMember);
                    row.CreateCell(2).SetCellValue(item.UsageMember);
                    row.CreateCell(3).SetCellValue(item.CustomerName);
                    row.CreateCell(4).SetCellValue(item.Sex);
                    row.CreateCell(5).SetCellValue(item.Age);
                    row.CreateCell(6).SetCellValue(item.IsMarry);
                    row.CreateCell(7).SetCellValue(item.Children);
                    row.CreateCell(8).SetCellValue(item.Phone);
                    row.CreateCell(9).SetCellValue(item.QQ);
                    row.CreateCell(10).SetCellValue(item.WebCat);
                    row.CreateCell(11).SetCellValue(item.Email);
                    row.CreateCell(12).SetCellValue(item.Address);
                    row.CreateCell(13).SetCellValue(item.Industry);
                    row.CreateCell(14).SetCellValue(item.Occupation);
                    row.CreateCell(15).SetCellValue(item.Income);
                    row.CreateCell(16).SetCellValue(item.Hobby);
                    row.CreateCell(17).SetCellValue(item.HasCar);
                    row.CreateCell(18).SetCellValue(item.HasHouse);
                    row.CreateCell(19).SetCellValue(item.InvestProj);
                    row.CreateCell(20).SetCellValue(item.InvestConc);
                    row.CreateCell(21).SetCellValue(item.InvestLife);
                    row.CreateCell(22).SetCellValue(item.Hobby);
                    row.CreateCell(23).SetCellValue(item.HasCar);
                    row.CreateCell(24).SetCellValue(item.HasHouse);

                    int incount = 0;
                    foreach (var ci in customItemList)
                    {
                        var tmp = customItemValueList.Where(n => n.InforId == item.Id && n.CustomItemId == ci.Id).SingleOrDefault();

                        row.CreateCell(25 + incount).SetCellValue(tmp == null ? "" : tmp.ItemValue);
                        incount++;
                    }

                    count++;
                }

                MemoryStream ms = new MemoryStream();

                try
                {
                    // 把 workbook 写到内存流里面
                    _workbook.Write(ms);

                    // 设置内存流的读取偏移量
                    ms.Seek(0, SeekOrigin.Begin);
                }
                catch (Exception ex)
                {
                    LogHelper.writeLog_error(ex.Message);
                    LogHelper.writeLog_error(ex.StackTrace);
                    throw;
                }

                return ms;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);
                throw;
            }

            
        }
    }
}