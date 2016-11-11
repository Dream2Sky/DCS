using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace com.dcs.entity
{
    public class CustomItem:Entity
    {

        /// <summary>
        /// 自定义项 名称
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string ItemName { get; set; }

        /// <summary>
        /// 添加此自定义项的用户  账号
        /// </summary>
        [Required]
        [MaxLength(12)]
        public string InsertMember { get; set; }
    }
}
