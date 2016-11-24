using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.dcs.entity;
using com.dcs.ibll;
using com.dcs.idal;
using com.dcs.common;

namespace com.dcs.bll
{
    public class CompanyBLL : ICompanyBLL
    {
        private ICompanyDAL _companyDAL;
        public CompanyBLL(ICompanyDAL companyDAL)
        {
            _companyDAL = companyDAL;
        }

        public Company GetCompanyByCode(string code)
        {
            if (code == string.Empty)
            {
                return null;
            }

            try
            {
                return _companyDAL.SelectByCompanyCode(code);
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
