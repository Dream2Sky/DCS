using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.dcs.common;
using com.dcs.entity;
using com.dcs.ibll;
using com.dcs.idal;

namespace com.dcs.bll
{
    public class CustomItemValueBLL : ICustomItemValueBLL
    {
        private ICustomItemValueDAL _customItemValueDAL;
        private ICustomItemDAL _customItemDAL;
        public CustomItemValueBLL(ICustomItemValueDAL customItemValueDAL, ICustomItemDAL customItemDAL)
        {
            _customItemValueDAL = customItemValueDAL;
            _customItemDAL = customItemDAL;
        }
        public OperatorState GetCustomItemValueByMember(string inserMember, Guid informationId, ref List<CustomItemValue> customItemValueList)
        {
            if (inserMember == string.Empty)
            {
                return OperatorState.empty;
            }

            try
            {
                List<CustomItemValue> civList = new List<CustomItemValue>();
                var ItemList = _customItemDAL.SelectByInsertMember(inserMember);
                foreach (var item in ItemList)
                {
                    civList.AddRange(_customItemValueDAL.SelectByItemNameAndInforId(item.ItemName, informationId));
                }

                customItemValueList = civList;

                return OperatorState.success;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                return OperatorState.error;
            }
        }
    }
}
