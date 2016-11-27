using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.dcs.entity;
using com.dcs.web.Models;

namespace com.dcs.web.Globals
{
    public class ModelChangeManager
    {
        public static List<MemberModel> ChangeTOMemberModel(IEnumerable<Member> memberList)
        {
            List<MemberModel> mmList = new List<Models.MemberModel>();

            foreach (var item in memberList)
            {
                MemberModel mm = new Models.MemberModel();
                mm.Account = item.Account;
                mm.Name = item.Name;
                mm.ParentName = item.Parent;
                mm.RoleCode = item.Role;
                mm.Apcount = item.Apcount;
                mm.Ascount = item.Ascount;
                mm.Cocount = item.Cocount;

                mmList.Add(mm);
            }

            return mmList;
        }

        public static MemberModel ChangeTOMemberModel(Member member)
        {
            if (member == null)
            {
                return null;
            }
            MemberModel mm = new MemberModel();
            mm.Account = member.Account;
            mm.Name = member.Name;
            mm.ParentName = member.Parent;
            mm.RoleCode = member.Role;

            return mm;
        }

        public static CustomItemModel ChangeTOCustomItemModel(CustomItem ci)
        {
            if (ci == null)
            {
                return null;
            }

            CustomItemModel cm = new Models.CustomItemModel();
            cm.Name = ci.ItemName;
            cm.Content = ci.ItemContent;
            return cm;
        }

        public static List<CustomItemModel> ChangeTOCustomItemModel(List<CustomItem> customItemList)
        {
            if (customItemList == null)
            {
                return null;
            }
            List<CustomItemModel> cmList = new List<CustomItemModel>();

            foreach (var item in customItemList)
            {
                CustomItemModel cm = new CustomItemModel();
                cm.Content = item.ItemContent;
                cm.Name = item.ItemName;

                cmList.Add(cm);    
            }

            return cmList;
        }

        public static CustomItemModel ChangeTOCustomItemModel(CustomItemValue civ)
        {
            try
            {
                if (civ == null)
                {
                    return null;
                }

                CustomItemModel cm = new CustomItemModel();
                cm.Name = civ.ItemName;
                cm.Value = civ.ItemValue;

                return cm;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<CustomItemModel> ChangeTOCustomItemModel(List<CustomItemValue> customItemValueList)
        {
            try
            {
                if (customItemValueList == null)
                {
                    return null;
                }

                List<CustomItemModel> cmList = new List<Models.CustomItemModel>();
                foreach (var item in customItemValueList)
                {
                    CustomItemModel cm = new Models.CustomItemModel();
                    cm.Name = item.ItemName;
                    cm.Value = item.ItemValue;

                    cmList.Add(cm);
                }

                return cmList;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}