using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace com.dcs.entity
{
    public class Member:Entity
    {
        [Required]
        [MaxLength(12)]
        public string Account { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required]
        public int Role { get; set; }

        /// <summary>
        /// 用户上级 表示用户间的从属关系
        /// SuperAdmin (超级管理员) -> Admin (管理员) -> Competent (主管) -> Collector (收集员)
        ///                                                              -> GeneralStaff (普通员工)
        ///                                                              
        /// 记录的是账号 不是 Id
        /// 
        /// </summary>
        [Required]
        [MaxLength(12)]
        public string Parent { get; set; }

        [Required]
        [MaxLength(10)]
        public string CompanyCode { get; set; }
    }
}
