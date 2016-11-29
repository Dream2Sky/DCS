using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.dcs.ibll;
using com.dcs.idal;
using com.dcs.dal;
using com.dcs.common;
using com.dcs.entity;

namespace com.dcs.bll
{
    public class InformationBLL : IInformationBLL
    {
        private IInformationDAL _informationDAL;
        private ICompanyDAL _companyDAL;
        private IMemberBLL _memberBLL;
        private ICustomItemValueDAL _customItemValueDAL;
        public InformationBLL(IInformationDAL informationDAL, ICompanyDAL companyDAL, ICustomItemValueDAL customItemValueDAL)
        {
            _informationDAL = informationDAL;
            _companyDAL = companyDAL;
            _customItemValueDAL = customItemValueDAL;
        }

        public OperatorState AddInformation(Information information, string user, string companycode)
        {
            if (information == null)
            {
                return OperatorState.empty;
            }

            try
            {
                // 构造datacode
                information.DataCode = new StringBuilder().
                    Append("DC").Append(companycode).
                    Append(TimeManager.GetTimeSpan()).
                    Append(RandomManager.GenerateRandom(5)).ToString();
                information.InsertTime = DateTime.Now;
                information.UpdateTime = DateTime.Now;
                information.IsDeleted = false;
                information.State = (int)InformatinState.UnAssigned; // 初始时为  未分配 状态

                information.UsageMember = information.InsertMember = user; // 初始化可使用用户  和 插入用户
                information.CompanyCode = companycode; // 次条数据的公司代码

                if (_informationDAL.Insert(information))
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

                throw;
            }
        }

        public bool IsExsit(string phone, string qq, string webchat, string companycode)
        {
            try
            {
                // 当 三个 条件都为空时 不做重复性判断
                // 直接返回false  表示继续
                var p = phone ?? "";
                var q = qq ?? "";
                var w = webchat ?? "";
                if (p == string.Empty && q == string.Empty && w == string.Empty)
                {
                    return false;
                }
                return _informationDAL.IsExist(phone, qq, webchat, companycode);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                throw;
            }
        }

        public IEnumerable<Information> GetInformationByCompany(string company)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取指定用户  指定状态的 数据
        /// </summary>
        /// <param name="member"></param>
        /// <param name="state"></param>
        /// <param name="modelList"></param>
        /// <returns></returns>
        public OperatorState GetInformation(string member, InformatinState state, ref List<InformationModel> modelList)
        {
            try
            {
                var data = _informationDAL.SelectByConditions(member, state);
                if (data == null)
                {
                    return OperatorState.empty;
                }
                else
                {
                    modelList = data.ToList();
                    return OperatorState.success;
                }
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                return OperatorState.error;
            }
        }

        /// <summary>
        /// 获取指定用户所有的所有的数据
        /// </summary>
        /// <param name="member"></param>
        /// <param name="modelList"></param>
        /// <returns></returns>
        public OperatorState GetInformation(Member member, ref List<InformationModel> modelList)
        {
            try
            {
                var data = _informationDAL.SelectByConditions(member);

                modelList = data.ToList();
                return OperatorState.success;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                return OperatorState.error;
            }
        }

        public Information GetInformation(string dataCode)
        {
            if (dataCode == string.Empty)
            {
                return null;
            }

            try
            {
                return _informationDAL.SelectByDataCode(dataCode);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);
                throw;
            }
        }

        public OperatorState GetInformation(string keyword, string member, ConditionModal conditionModal, List<CustomItem> customItemList, ref List<InformationModel> modelList)
        {
            if (member == string.Empty)
            {
                return OperatorState.empty;
            }

            try
            {
                if (conditionModal == null)
                {
                    modelList.AddRange(_informationDAL.SelectByConditions(keyword, member));
                }
                else
                {
                    modelList.AddRange(_informationDAL.SelectByConditions(conditionModal, keyword, member));
                }

                foreach (var item in customItemList)
                {
                    var temp = _customItemValueDAL.SelectByKeyword(keyword, item.Id);
                    if (temp != null)
                    {
                        foreach (var subitem in temp)
                        {
                            modelList.Add(_informationDAL.SelectByConditions(subitem.InforId));
                        }
                    }
                }

                return OperatorState.success;
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
