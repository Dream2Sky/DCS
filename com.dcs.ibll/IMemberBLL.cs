using com.dcs.common;
using com.dcs.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.dcs.ibll
{
    public interface IMemberBLL
    {
        LoginState Login(string account, string password, ref Member member);
        IEnumerable<Member> GetUsersByRole(int role);
        OperatorState AddMember(string name, int role, string parent, ref Member member);
        IEnumerable<Member> GetUsersByParent(string parent);
        Member GetUserByAccount(string account);
    }
}
