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

        /// <summary>
        /// 累计搜集数据条数
        /// </summary>
        public int Cocount { get; set; }
        /// <summary>
        /// 当前待审批数据条数
        /// </summary>
        public int Apcount { get; set; }
        /// <summary>
        /// 当前已分配数据条数
        /// </summary>
        public int Ascount { get; set; }
    }
}