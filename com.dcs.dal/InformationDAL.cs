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

        public IEnumerable<InformationModel> SelectByConditions(ConditionModal conditionModel, string keyword, string member)
        {
            try
            {
                var set = db.Set<Information>().Where(n => n.InsertMember == member || n.UsageMember == member)
                    .Where(n => n.Sex == conditionModel.Sex && n.HasHouse == conditionModel.HasHouse &&
                    n.HasCar == conditionModel.HasCar && n.Children == conditionModel.Children && n.IsMarry == n.IsMarry);

                if (conditionModel.Income != string.Empty)
                {
                    set = set.Where(n => n.Income == conditionModel.Income);
                }

                if (conditionModel.MinAge >= 0)
                {
                    set = set.Where(n => n.Age >= conditionModel.MinAge);
                }

                if (conditionModel.MaxAge <= 0)
                {
                    set = set.Where(n => n.Age <= conditionModel.MaxAge);
                }

                if (keyword != string.Empty)
                {

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
    }
}
