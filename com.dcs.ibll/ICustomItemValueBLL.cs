using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.dcs.idal;
using com.dcs.common;
using com.dcs.entity;

namespace com.dcs.ibll
{
    public interface ICustomItemValueBLL
    {
        OperatorState GetCustomItemValueByMember(string inserMember, Guid informationId, ref List<CustomItemValue> customItemValueList);
        CustomItemValue GetCustomItemValueByCustomItemIdAndInforId(Guid customItemId, Guid informationId);
    }
}
