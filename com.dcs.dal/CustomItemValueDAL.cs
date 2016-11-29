using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.dcs.entity;
using com.dcs.idal;

namespace com.dcs.dal
{
    public class CustomItemValueDAL : DataBaseDAL<CustomItemValue>, ICustomItemValueDAL
    {
        public CustomItemValue SelectByCustomItemIdAndInforId(Guid customItemId, Guid InforId)
        {
            try
            {
                return db.Set<CustomItemValue>().Where(n => n.CustomItemId == customItemId && n.InforId == InforId && n.IsDeleted == false).SingleOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CustomItemValue> SelectByItemNameAndInforId(string itemName, Guid inforId)
        {
            try
            {
                return db.Set<CustomItemValue>().Where(n => n.ItemName == itemName && n.InforId == inforId && n.IsDeleted == false).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CustomItemValue> SelectByKeyword(string keyword, Guid customItemId)
        {
            try
            {
                if (keyword != string.Empty)
                {
                    return db.Set<CustomItemValue>().Where(n => n.CustomItemId == customItemId && n.IsDeleted == false).Where(n => n.ItemValue.Contains(keyword)).ToList();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
