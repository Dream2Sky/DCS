﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.dcs.entity;

namespace com.dcs.ibll
{
    public interface ICompanyBLL
    {
        Company GetCompanyByCode(string code);
    }
}
