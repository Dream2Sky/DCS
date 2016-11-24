using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.dcs.common
{
    /// <summary>
    /// 登陆状态
    /// </summary>
    public enum LoginState
    {
        /// <summary>
        /// 账号为空
        /// </summary>
        empty,
        /// <summary>
        /// 登陆成功
        /// </summary>
        success,
        /// <summary>
        /// 不存在的账号
        /// </summary>
        account_error,
        /// <summary>
        /// 密码错误
        /// </summary>
        password_error,
        /// <summary>
        /// 登陆失败
        /// </summary>
        failed
    }

    /// <summary>
    /// 这个枚举主要用在数据库操作上 比如增加 修改 删除等等操作
    /// </summary>
    public enum OperatorState
    {
        /// <summary>
        /// 空对象
        /// </summary>
        empty,
        /// <summary>
        /// 操作成功
        /// </summary>
        success,
        /// <summary>
        /// 操作失败
        /// </summary>
        error,
        /// <summary>
        /// 重复对象
        /// </summary>
        repeat
    }

    /// <summary>
    /// 数据状态  待审批 已分配 未分配
    /// </summary>
    public enum InformatinState {
        Approved,
        Assigned,
        UnAssigned
    }
}
