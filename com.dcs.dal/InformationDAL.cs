using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
