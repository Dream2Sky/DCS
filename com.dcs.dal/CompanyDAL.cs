using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.dcs.entity;
using com.dcs.idal;

namespace com.dcs.dal
{
    public class CompanyDAL : DataBaseDAL<Company>, ICompanyDAL
    {
        public Company SelectByCompanyCode(string code)
        {
            if (code == string.Empty)
            {
                return null;
            }

            try
            {
                return db.Set<Company>().Where(n => n.IsDeleted == false && n.CompanyCode == code).SingleOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
