using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.dcs.common;
using com.dcs.entity;
using com.dcs.idal;

namespace com.dcs.dal
{
    public class InformationDAL : DataBaseDAL<Information>, IInformationDAL
    {
        public bool IsExist(string phone, string qq, string webchat, string companycode)
        {
            try
            {
                var information = db.Informations
                    .Where(n => n.CompanyCode == companycode && n.IsDeleted == false)
                    .Where(n => n.Phone != string.Empty || n.QQ != string.Empty || n.WebCat != string.Empty)
                    .Where(n => n.Phone == phone && n.QQ == qq && n.WebCat == webchat).FirstOrDefault();
                if (information == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public InformationModel SelectByConditions(Guid id)
        {
            try
            {
                return db.Set<Information>().Where(n => n.Id == id).Select(n => new InformationModel
                {
                    dataCode = n.DataCode,
                    insertMember = n.InsertMember,
                    customerName = n.CustomerName,
                    sex = n.Sex,
                    phone = n.Phone,
                    qq = n.QQ,
                    webchat = n.WebCat,
                    email = n.Email,
                    hashouse = n.HasHouse,
                    hobby = n.Hobby,
                    state = n.State,
                    usageMember = n.UsageMember
                }).SingleOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<InformationModel> SelectByConditions(Member member)
        {
            try
            {
                return db.Set<Information>().Where(n => n.InsertMember == member.Account || n.UsageMember == member.Account)
                    .Where(n => n.IsDeleted == false)
                    .Select(n => new InformationModel
                    {
                        dataCode = n.DataCode,
                        insertMember = n.InsertMember,
                        customerName = n.CustomerName,
                        sex = n.Sex,
                        phone = n.Phone,
                        qq = n.QQ,
                        webchat = n.WebCat,
                        email = n.Email,
                        hashouse = n.HasHouse,
                        hobby = n.Hobby,
                        state = n.State,
                        usageMember = n.UsageMember
                    }).AsEnumerable();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<InformationModel> SelectByConditions(string member, InformatinState state)
        {
            try
            {
                return db.Set<Information>().Where(n => n.InsertMember == member || n.UsageMember == member)
                    .Where(n => n.IsDeleted == false && n.State == (int)state).Select(n =>
                     new InformationModel
                     {
                         dataCode = n.DataCode,
                         insertMember = n.InsertMember,
                         customerName = n.CustomerName,
                         sex = n.Sex,
                         phone = n.Phone,
                         qq = n.QQ,
                         webchat = n.WebCat,
                         email = n.Email,
                         hashouse = n.HasHouse,
                         hobby = n.Hobby,
                         state = n.State,
                         usageMember = n.UsageMember
                     }).AsEnumerable();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据 usagemember 查找关键字
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public IEnumerable<InformationModel> SelectByConditions(string keyword, string member)
        {
            try
            {
                if (keyword == string.Empty)
                {
                    return db.Set<Information>().Where(n => n.UsageMember == member && n.IsDeleted == false).Select(n => new InformationModel
                    {
                        dataCode = n.DataCode,
                        insertMember = n.InsertMember,
                        customerName = n.CustomerName,
                        sex = n.Sex,
                        phone = n.Phone,
                        qq = n.QQ,
                        webchat = n.WebCat,
                        email = n.Email,
                        hashouse = n.HasHouse,
                        hobby = n.Hobby,
                        state = n.State,
                        usageMember = n.UsageMember
                    });
                }
                else
                {
                    return db.Set<Information>()
                            .Where(n => n.CustomerName.Contains(keyword) || n.Email.Contains(keyword) || n.Hobby.Contains(keyword)
                            || n.InsertMember.Contains(keyword) || n.Address.Contains(keyword) || n.QQ.Contains(keyword)
                            || n.WebCat.Contains(keyword) || n.Industry.Contains(keyword) || n.InvestConc.Contains(keyword)
                            || n.InvestLife.Contains(keyword) || n.InvestProj.Contains(keyword) || n.Note1.Contains(keyword)
                            || n.Note2.Contains(keyword) || n.Note3.Contains(keyword) || n.Occupation.Contains(keyword) || n.Phone.Contains(keyword) 
                            || n.InsertMember.Contains(keyword) || n.UsageMember.Contains(keyword))
                            .Where(n => n.UsageMember == member && n.IsDeleted == false).Select(n => new InformationModel
                            {
                                dataCode = n.DataCode,
                                insertMember = n.InsertMember,
                                customerName = n.CustomerName,
                                sex = n.Sex,
                                phone = n.Phone,
                                qq = n.QQ,
                                webchat = n.WebCat,
                                email = n.Email,
                                hashouse = n.HasHouse,
                                hobby = n.Hobby,
                                state = n.State,
                                usageMember = n.UsageMember
                            });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 带条件的关键字搜索
        /// </summary>
        /// <param name="conditionModel"></param>
        /// <param name="keyword"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        public IEnumerable<InformationModel> SelectByConditions(ConditionModal conditionModel, string keyword, string member)
        {
            try
            {
                if (keyword == string.Empty)
                {
                    var set = db.Set<Information>().Where(n => n.UsageMember == member && n.IsDeleted == false);

                    set = Filter(set, conditionModel);

                    return set.Select(n => new InformationModel
                    {
                        dataCode = n.DataCode,
                        insertMember = n.InsertMember,
                        customerName = n.CustomerName,
                        sex = n.Sex,
                        phone = n.Phone,
                        qq = n.QQ,
                        webchat = n.WebCat,
                        email = n.Email,
                        hashouse = n.HasHouse,
                        hobby = n.Hobby,
                        state = n.State,
                        usageMember = n.UsageMember
                    });
                }
                else
                {
                    var set = db.Set<Information>()
                            .Where(n => n.CustomerName.Contains(keyword) || n.Email.Contains(keyword) || n.Hobby.Contains(keyword)
                            || n.InsertMember.Contains(keyword) || n.Address.Contains(keyword) || n.QQ.Contains(keyword)
                            || n.WebCat.Contains(keyword) || n.Industry.Contains(keyword) || n.InvestConc.Contains(keyword)
                            || n.InvestLife.Contains(keyword) || n.InvestProj.Contains(keyword) || n.Note1.Contains(keyword)
                            || n.Note2.Contains(keyword) || n.Note3.Contains(keyword) || n.Occupation.Contains(keyword) 
                            || n.Phone.Contains(keyword) || n.InsertMember.Contains(keyword) || n.UsageMember.Contains(keyword))
                            .Where(n => n.UsageMember == member && n.IsDeleted == false);

                    set = Filter(set, conditionModel);

                    return set.Select(n => new InformationModel
                    {
                        dataCode = n.DataCode,
                        insertMember = n.InsertMember,
                        customerName = n.CustomerName,
                        sex = n.Sex,
                        phone = n.Phone,
                        qq = n.QQ,
                        webchat = n.WebCat,
                        email = n.Email,
                        hashouse = n.HasHouse,
                        hobby = n.Hobby,
                        state = n.State,
                        usageMember = n.UsageMember
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Information SelectByDataCode(string dataCode)
        {
            try
            {
                return db.Informations.SingleOrDefault(n => n.DataCode == dataCode && n.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 筛选
        /// </summary>
        /// <param name="set"></param>
        /// <param name="conditionModel"></param>
        /// <returns></returns>
        private IQueryable<Information> Filter(IQueryable<Information> set, ConditionModal conditionModel)
        {
            // 筛选收入
            if (!string.IsNullOrEmpty(conditionModel.Income))
            {
                set = set.Where(n => n.Income.Contains(conditionModel.Income));
            }

            // 筛选子女
            if (conditionModel.Children > 0)
            {
                set = set.Where(n => n.Children == 1);
            }
            else if (conditionModel.Children == 0)
            {
                set = set.Where(n => n.Children == 0);
            }

            // 筛选车否
            if (conditionModel.HasCar > 0)
            {
                set = set.Where(n => n.HasCar == 1);
            }
            else if (conditionModel.HasCar == 0)
            {
                set = set.Where(n => n.HasCar == 0);
            }

            //筛选房否
            if (conditionModel.HasHouse > 0)
            {
                set = set.Where(n => n.HasHouse == 1);
            }
            else if (conditionModel.HasHouse == 0)
            {
                set = set.Where(n => n.HasHouse == 0);
            }

            //筛选性别
            if (conditionModel.Sex > 0)
            {
                set = set.Where(n => n.Sex == 1);
            }
            else if (conditionModel.Sex == 0)
            {
                set = set.Where(n => n.Sex == 0);
            }

            //筛选婚否
            if (conditionModel.IsMarry > 0)
            {
                set = set.Where(n => n.IsMarry == 1);
            }
            else if (conditionModel.IsMarry == 0)
            {
                set = set.Where(n => n.IsMarry == 0);
            }

            if (conditionModel.MinAge > 0 && conditionModel.MaxAge <= 0)
            {
                set = set.Where(n => n.Age >= conditionModel.MinAge);
            }
            else if (conditionModel.MinAge > 0 && conditionModel.MaxAge > 0)
            {
                set = set.Where(n => n.Age >= conditionModel.MinAge && n.Age <= conditionModel.MaxAge);
            }
            else if (conditionModel.MinAge <= 0 && conditionModel.MaxAge > 0)
            {
                set = set.Where(n => n.Age <= conditionModel.MaxAge);
            }

            var starttime = DateTime.Now;
            var endtime = DateTime.Now;
            bool startnull = string.IsNullOrEmpty(conditionModel.StartTime);
            bool endnull = string.IsNullOrEmpty(conditionModel.EndTime);

            if (startnull == true && endnull == false)
            {
                endtime = DateTime.Parse(conditionModel.EndTime);
                set = set.Where(n => n.InsertTime <= endtime);
            }
            else if (startnull == false && endnull == false)
            {
                starttime = DateTime.Parse(conditionModel.StartTime);
                endtime = DateTime.Parse(conditionModel.EndTime);

                set = set.Where(n => n.InsertTime <= endtime && n.InsertTime >= starttime);
            }
            else if (startnull == false && endnull == true)
            {
                starttime = DateTime.Parse(conditionModel.StartTime);
                set = set.Where(n => n.InsertTime >= starttime);
            }

            return set;
        }

    }
}
