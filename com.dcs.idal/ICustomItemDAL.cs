﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.dcs.entity;

namespace com.dcs.idal
{
    public interface ICustomItemDAL:IDataBaseDAL<CustomItem>
    {
        List<CustomItem> SelectByInsertMember(string account);
        CustomItem SelectByCustomItemName(string customItemName);
    }
}
