using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.dcs.ibll;
using com.dcs.idal;
using com.dcs.dal;
using com.dcs.common;
using com.dcs.entity;

namespace com.dcs.bll
{
    public class InformationBLL : IInformationBLL
    {
        private IInformationDAL _informationDAL;
        public InformationBLL(IInformationDAL informationDAL)
        {
            _informationDAL = informationDAL;
        }

        public OperatorState AddInformation(Information information)
        {
            if (information == null)
            {
                return OperatorState.empty;
            }

            try
            {
                information.InsertTime = DateTime.Now;
                information.UpdateTime = DateTime.Now;
                information.IsDeleted = false;
                
                if (_informationDAL.Insert(information))
                {
                    return OperatorState.success;
                }
                else
                {
                    return OperatorState.error;
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                throw;
            }
        }

        public bool IsExsit(string phone, string qq, string webchat)
        {
            
        }
    }
}
