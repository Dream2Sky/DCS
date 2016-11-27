using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.dcs.entity;

namespace com.dcs.idal
{
    public interface ICustomItemValueDAL:IDataBaseDAL<CustomItemValue>
    {
        List<CustomItemValue> SelectByItemNameAndInforId(string itemName, Guid inforId);
    }
}
