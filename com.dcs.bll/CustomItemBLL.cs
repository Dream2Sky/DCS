using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.dcs.dal;
using com.dcs.idal;
using com.dcs.ibll;
using com.dcs.entity;

namespace com.dcs.bll
{
    public class CustomItemBLL : ICustomItemBLL
    {
        private ICustomItemDAL _customItemDAL;
        public CustomItemBLL(ICustomItemDAL customItemDAL)
        {
            _customItemDAL = customItemDAL;
        }
        public IEnumerable<CustomItem> GetCustomItems(string account)
        {
            if (account == string.Empty)
            {
                return null;
            }
        }
    }
}
