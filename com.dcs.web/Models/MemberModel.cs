using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.dcs.web.Models
{
    public class MemberModel
    {
        public string Name { get; set; }
        public int RoleCode { get; set; }
        public string ParentName { get; set; }
    }
}