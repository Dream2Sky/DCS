using com.dcs.ibll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.dcs.common;
using com.dcs.entity;
using com.dcs.idal;

namespace com.dcs.bll
{
    public class MemberBLL : IMemberBLL
    {
        private IMemberDAL _memberDAL;
        public MemberBLL(IMemberDAL memberDAL)
        {
            _memberDAL = memberDAL;
        }

        public OperatorState AddMember(string name, int role, string parent, string company, ref Member member)
        {
            LogHelper.writeLog_info("start adding a member");
            var m = _memberDAL.SelectByAccount(name);
            if (m != null)
            {
                return OperatorState.repeat;
            }

            Member me = new Member();
            me.Account = ConfigManager.GetAccountPrefix() + ConfigManager.GetIndexSeed() ;
            me.Name = name;
            me.IsDeleted = false;
            me.Password = EncryptManager.SHA1(ConfigManager.GetDefaultPassword());
            me.CompanyCode = company;
            me.Role = role;
            me.Parent = parent;
            

            if (_memberDAL.Insert(me))
            {
                member = me;
                return OperatorState.success;
            }
            else
            {
                return OperatorState.error;
            }
        }

        public OperatorState DeleteMember(Member member)
        {
            if (member == null)
            {
                return OperatorState.error;
            }

            try
            {
                member.IsDeleted = true;
                if (_memberDAL.Update(member))
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

        public Member GetUserByAccount(string account)
        {
            if (account == null)
            {
                return null;
            }
            try
            {
                return _memberDAL.SelectByAccount(account);
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);
                throw;
            }
        }

        public IEnumerable<Member> GetUsersByParent(string parent)
        {
            return _memberDAL.SelectByParents(parent);
        }

        public IEnumerable<Member> GetUsersByRole(int role)
        {
            return _memberDAL.SelectByRoles(role);
        }

        public LoginState Login(string account, string password, ref Member member)
        {
            if (string.IsNullOrEmpty(account))
            {
                return LoginState.empty;
            }

            try
            {
                var m = _memberDAL.SelectByAccount(account);

                if (m == null)
                {
                    return LoginState.account_error;
                }

                if (m.Password != EncryptManager.SHA1(password))
                {
                    return LoginState.password_error;
                }

                member = m;
                return LoginState.success;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                member = null;
                return LoginState.failed;
            }
        }

        public OperatorState UpdateMember(string account, string name, int role, string parent)
        {
            try
            {
                var member = _memberDAL.SelectByAccount(account);
                member.Name = name;
                member.Role = role;
                member.Parent = parent;

                member.UpdateTime = DateTime.Now;

                if (_memberDAL.Update(member))
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
