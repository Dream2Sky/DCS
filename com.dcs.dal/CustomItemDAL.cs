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
        /// 根据名称查找自定义项
        /// </summary>
        /// <param name="customItemName"></param>
        /// <returns></returns>
        public CustomItem SelectByCustomItemName(string customItemName)
        {
            try
            {
                return db.Set<CustomItem>().Where(n => n.ItemName == customItemName && n.IsDeleted == false).SingleOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根據用戶查找自定義項
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public List<CustomItem> SelectByInsertMember(string account)
        {
            try
            {
                return db.Set<CustomItem>().Where(n => n.InsertMember == account && n.IsDeleted == false).ToList();
            }
            catch (Exception)
            { 
                throw;
            }
        }
    }
}
