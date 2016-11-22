using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.dcs.entity;
using com.dcs.common;

namespace com.dcs.ibll
{
    public interface ICustomItemBLL
    {
        OperatorState GetCustomItems(string account, ref List<CustomItem> customItemList);
        OperatorState AddCustomItems(string account, string customItemContent, ref CustomItem ci);
        bool IsExist(string account, string customItemContent);
    }
}
