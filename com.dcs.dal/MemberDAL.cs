using com.dcs.entity;
using com.dcs.idal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.dcs.dal
{
    public class MemberDAL : DataBaseDAL<Member>, IMemberDAL
    {
        public Member SelectByAccount(string account)
        {
            try
            {
                return db.Set<Member>().Where(n => n.Account == account).SingleOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Member> SelectByParents(string parents)
        {
            try
            {
                return db.Set<Member>().Where(n => n.Parent == parents && n.IsDeleted == false).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Member> SelectByRoles(int role)
        {
            try
            {
                return db.Set<Member>().Where(n => n.Role == role && n.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
