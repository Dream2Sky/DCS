﻿using System;
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
                    HttpContext.Current.Session["UnderlingList"] = underlingList;
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
                foreach (var item in underlingList)
                {
                    if (item.Account == member.Account)
                    {
                        underlingList.Remove(item);
                    }
                }
                HttpContext.Current.Session["UnderlingList"] = underlingList;
            }
        }

        public void UpdateUnderlingList(Member member)
        {
            try
            {
                if (member == null)
                {
                    return;
                }
                List<Member> memberList = HttpContext.Current.Session["UnderlingList"] as List<Member>;
                foreach (var item in memberList)
                {
                    if (item.Account == member.Account)
                    {
                        memberList.Remove(item);
                        break;
                    }
                }
                memberList.Add(member);

                HttpContext.Current.Session["UnderlingList"] = memberList;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                throw;
            }
        }

        public void InitUnderlingList(List<Member> memberList)
        {
            try
            {
                HttpContext.Current.Session["UnderlingList"] = memberList;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);

                throw;
            }
        } 
    }
}