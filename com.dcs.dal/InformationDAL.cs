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
                    .Where(n => n.Phone == phone || n.QQ == qq || n.WebCat == webchat).SingleOrDefault();
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
                        state = n.State
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
                         state = n.State
                     }).AsEnumerable();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
