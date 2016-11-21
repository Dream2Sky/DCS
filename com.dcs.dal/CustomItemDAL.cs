using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.dcs.entity;
using com.dcs.idal;

namespace com.dcs.dal
{
    public class CustomItemDAL : DataBaseDAL<CustomItem>, ICustomItemDAL
    {
        /// <summary>
        /// 根據用戶查找自定義項
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public IEnumerable<CustomItem> SelectByInsertMember(string account)
        {
            try
            {
                return db.Set<CustomItem>().Where(n => n.InsertMember == account && n.IsDeleted == false);
            }
            catch (Exception)
            { 
                throw;
            }
        }
    }
}
