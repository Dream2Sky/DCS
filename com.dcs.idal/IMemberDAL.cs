using com.dcs.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.dcs.idal
{
    public interface IMemberDAL: IDataBaseDAL<Member>
    {
        Member SelectByAccount(string account);
        IEnumerable<Member> SelectByRoles(int role);
        List<Member> SelectByParents(string parents);
    }
}
