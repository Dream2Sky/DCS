using com.dcs.common;
using com.dcs.dal;
using com.dcs.entity;
using com.dcs.ibll;
using com.dcs.idal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.dcs.bll
{
    public class CustomItemBLL : ICustomItemBLL
    {
        private ICustomItemDAL _customItemDAL;
        private IMemberDAL _memberDAL;
        public CustomItemBLL(ICustomItemDAL customItemDAL, IMemberDAL memberDAL)
        {
            _customItemDAL = customItemDAL;
            _memberDAL = memberDAL;
        }

        /// <summary>
        /// 添加一个自定义项
        /// </summary>
        /// <param name="account">谁添加的</param>
        /// <param name="customItemContent">自定义项的内容 即用户可视的项目名</param>
        /// <returns></returns>
        public OperatorState AddCustomItems(string account, string customItemContent, ref CustomItem customItem)
        {
            if (account == string.Empty)
            {
                return OperatorState.empty;
            }

            if (IsExist(account,customItemContent))
            {
                return OperatorState.repeat;
            }

            CustomItem ci = new CustomItem();
            ci.ItemName = account + "C" + TimeManager.GetTimeSpan();
            ci.ItemContent = customItemContent;
            ci.InsertMember = account;
            ci.InsertTime = DateTime.Now;
            ci.IsDeleted = false;
            ci.UpdateTime = DateTime.Now;

            if (_customItemDAL.Insert(ci))
            {
                customItem = ci;
                return OperatorState.success;
            }
            else
            {
                return OperatorState.error;
            }
        }

        public OperatorState DeleteCustomItems(string customItemName)
        {
            if (customItemName == string.Empty)
            {
                return OperatorState.empty;
            }

            try
            {
                var customItem = _customItemDAL.SelectByCustomItemName(customItemName);
                if (customItem == null)
                {
                    return OperatorState.empty;
                }

                customItem.IsDeleted = true;
                customItem.UpdateTime = DateTime.Now;

                if (_customItemDAL.Update(customItem))
                {
                    return OperatorState.success;
                }
                else
                {
                    return OperatorState.error;
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                return OperatorState.error;
            }
        }

        public OperatorState GetCustomItems(string customItemName, ref CustomItem customItem)
        {
            if (customItemName == string.Empty)
            {
                return OperatorState.empty;
            }

            try
            {
                customItem = _customItemDAL.SelectByCustomItemName(customItemName);
                return OperatorState.success;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                return OperatorState.error;
            }
        }

        public OperatorState GetCustomItems(string account, ref List<CustomItem> customItemList)
        {
            if (account == string.Empty)
            {
                return OperatorState.empty;
            }

            try
            {
                customItemList = _customItemDAL.SelectByInsertMember(account).ToList();
                return OperatorState.success;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                return OperatorState.error;
            }
        }

        /// <summary>
        /// 判断某个用户是否存在某个自定义项
        /// </summary>
        /// <param name="account"></param>
        /// <param name="customItemContent"></param>
        /// <returns></returns>
        public bool IsExist(string account, string customItemContent)
        {
            try
            {
                var customItem = _customItemDAL.SelectByInsertMember(customItemContent).Where(n => n.InsertMember == account && n.IsDeleted == false).SingleOrDefault();
                if (customItem == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                throw;
            }
        }
        
        /// <summary>
        /// 更新自定义项 
        /// </summary>
        /// <param name="customItemName"></param>
        /// <param name="customItemContent"></param>
        /// <returns></returns>
        public OperatorState UpdateCustomItems(string customItemName, string customItemContent)
        {
            if (customItemName == string.Empty || customItemContent == string.Empty)
            {
                return OperatorState.empty;
            }

            try
            {
                var customItem = _customItemDAL.SelectByCustomItemName(customItemName);
                if (customItem == null)
                {
                    return OperatorState.empty;
                }

                customItem.ItemContent = customItemContent;
                customItem.UpdateTime = DateTime.Now;

                if (_customItemDAL.Update(customItem))
                {
                    return OperatorState.success;
                }
                else
                {
                    return OperatorState.error;
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                return OperatorState.error;
            }
        }
    }
}
