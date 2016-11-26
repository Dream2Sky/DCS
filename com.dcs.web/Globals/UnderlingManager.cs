using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.dcs.ibll;
using com.dcs.entity;
using com.dcs.common;

namespace com.dcs.web.Globals
{
    public class UnderlingManager
    {
        private IMemberBLL _memberBLL;
        public UnderlingManager(IMemberBLL memberBLL)
        {
            _memberBLL = memberBLL;
        }

        public List<Member> GetUnderlingList()
        {
            return HttpContext.Current.Session["UnderlingList"] as List<Member>;
        }

        public void AddUnderlingList(Member member)
        {
            if (member == null)
            {
                return;
            }

            if (HttpContext.Current.Session["UnderlingList"] == null)
            {
                HttpContext.Current.Session["UnderlingList"] = new List<Member>() {
                    member
                };
            }
            else
            {
                if (!IsExist(member))
                {
                    List<Member> underlingList = HttpContext.Current.Session["UnderlingList"] as List<Member>;
                    underlingList.Add(member);
                }
            }
        }

        public bool IsExist(Member member)
        {
            List<Member> underlingList = HttpContext.Current.Session["UnderlingList"] as List<Member>;
            if (underlingList.Where(n=>n.Account==member.Account).SingleOrDefault() != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SubUnderlingList(Member member)
        {
            if (member == null || HttpContext.Current.Session["UnderlingList"] as List<Member> == null)
            {
                return;
            }

            if (IsExist(member))
            {
                List<Member> underlingList = HttpContext.Current.Session["UnderlingList"] as List<Member>;
                underlingList.Remove(member);
            }
        }

        public bool UpdateUnderlingList(Member member)
        {
            try
            {
                HttpContext.Current.Session["UnderlingList"] = _memberBLL.GetUnderling(member).ToList();

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                return false;
            }
        } 
    }
}