﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace com.dcs.entity
{
    public class CustomItemValue:Entity
    {
        /// <summary>
        /// 自定义项Id
        /// </summary>
        [Required]
        public Guid CustomItemId { get; set; }

        [Required]
        [MaxLength(50)]
        public string ItemName { get; set; }
        /// <summary>
        /// 自定义项 值
        /// </summary>
        [MaxLength(255)]
        public string ItemValue { get; set; } 

        /// <summary>
        /// 绑定的数据Id
        /// </summary>
        [Required]
        public Guid InforId { get; set; }
    }
}
