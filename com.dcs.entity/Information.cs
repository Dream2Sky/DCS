using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using MySql.Data.Types;

namespace com.dcs.entity
{
    public class Information : Entity
    {
        /// <summary>
        /// 数据来源 表示由哪个用户录入
        /// 记录用户的账号 Account
        /// </summary>
        [Required]
        public string InsertMember { get; set; }

        /// <summary>
        /// 冗余字段 标记数据是哪个公司的
        /// 便于搜索
        /// </summary>
        [Required]
        public string CompanyCode { get; set; }
        /// <summary>
        /// 客户名
        /// </summary>
        [MaxLength(20)]
        public string CustomerName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Sex { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [MaxLength(3)]
        public string Age { get; set; }

        /// <summary>
        /// 婚否
        /// </summary>
        public int IsMarry { get; set; }

        /// <summary>
        /// 子女
        /// </summary>
        public int Children { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [MaxLength(20)]
        public string Phone { get; set; }

        /// <summary>
        /// QQ
        /// </summary>
        [MaxLength(20)]
        public string QQ { get; set; }

        /// <summary>
        /// 微信号
        /// </summary>
        [MaxLength(30)]
        public string WebCat { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(50)]
        public string Email { get; set; }

        /// <summary>
        /// 通信地址
        /// </summary>
        [MaxLength(255)]
        public string Address { get; set; }
        /// <summary>
        /// 所属行业
        /// </summary>
        [MaxLength(30)]
        public string Industry { get; set; }
        /// <summary>
        /// 职业
        /// </summary>
        [MaxLength(30)]
        public string Occupation { get; set; }
        /// <summary>
        /// 年收入
        /// </summary>
        [MaxLength(20)]
        public string Income { get; set; }
        /// <summary>
        /// 爱好
        /// </summary>
        [MaxLength(30)]
        public string Hobby { get; set; }
        /// <summary>
        /// 是否有车
        /// </summary>
        public int HasCar { get; set; }
        /// <summary>
        /// 是否有房
        /// </summary>
        public int HasHouse { get; set; }

        /// <summary>
        /// 投资项目
        /// </summary>
        [MaxLength(50)]
        public string InvestProj { get; set; }

        /// <summary>
        /// 投资理念
        /// </summary>
        [MaxLength(50)]
        public string InvestConc { get; set; }

        /// <summary>
        /// 投资年限
        /// </summary>
        [MaxLength(30)]
        public string InvestLife { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(255)]
        public string Note1 { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(255)]
        public string Note2 { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(255)]
        public string Note3 { get; set; }

        /// <summary>
        /// 数据状态  待审批 已分配 未分配
        /// </summary>
        [Required]
        public int State { get; set; }

        /// <summary>
        /// 当前可使用用户
        /// </summary>
        public string UsageMember { get; set; }
    }
}
