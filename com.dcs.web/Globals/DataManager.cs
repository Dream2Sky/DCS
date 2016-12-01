using com.dcs.common;
using com.dcs.entity;
using com.dcs.ibll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.dcs.web.Globals
{
    public class DataManager
    {
        private IInformationBLL _informationBLL;
        public DataManager(IInformationBLL informationBLL)
        {
            _informationBLL = informationBLL;
        }

        /// <summary>
        /// 导入数据到数据库
        /// </summary>
        /// <param name="informationList"></param>
        /// <param name="currentUser"></param>
        /// <param name="customItemList"></param>
        /// <param name="customItemValueList"></param>
        /// <returns></returns>
        public bool ExcelToDataBase(List<Information> informationList,Member currentUser, List<CustomItem> customItemList,List<CustomItemValue> customItemValueList)
        {
            using (var db = new DCSDBContext())
            {
                using (var trans = db.Database.BeginTransaction())
                {
                    try
                    {
                        List<Guid> guidList = new List<Guid>();
                        foreach (var item in informationList)
                        {
                            if (!_informationBLL.IsExsit(item.Phone, item.QQ, item.WebCat, currentUser.CompanyCode))
                            {
                                item.CompanyCode = currentUser.CompanyCode;
                                item.InsertMember = currentUser.Account;
                                item.InsertTime = item.UpdateTime = DateTime.Now;
                                item.UsageMember = currentUser.Account;
                                item.State = (int)InformatinState.Assigned;
                                item.IsDeleted = false;
                                item.DataCode = "DC" + currentUser.CompanyCode + TimeManager.GetTimeSpan() + RandomManager.GenerateRandom(5);

                                db.Set<Information>().Add(item);
                            }
                            else
                            {
                                guidList.Add(item.Id);
                            }
                        }

                        foreach (var item in customItemList)
                        {
                            if (db.Set<CustomItem>().Where(n => n.Id == item.Id).SingleOrDefault() == null)
                            {
                                db.Set<CustomItem>().Add(item);
                            }
                        }

                        foreach (var item in customItemValueList)
                        {
                            if (!guidList.Contains(item.InforId))
                            {
                                db.Set<CustomItemValue>().Add(item);
                            }
                        }

                        db.SaveChanges();
                        trans.Commit();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        LogHelper.writeLog_error(ex.Message);
                        LogHelper.writeLog_error(ex.StackTrace);

                        trans.Rollback();

                        return false;
                    }
                }
            }
        }


    }
}