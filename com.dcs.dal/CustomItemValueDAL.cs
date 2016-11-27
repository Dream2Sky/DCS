﻿using System;
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
    }
}
