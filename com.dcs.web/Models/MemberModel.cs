using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.dcs.web.Models
{
    public class MemberModel
    {
        public string Account { get; set; } // 账号
        public string Name { get; set; }  // 姓名
        public int RoleCode { get; set; } // 角色
        public string ParentName { get; set; } // 上级账号  Account
    }
}