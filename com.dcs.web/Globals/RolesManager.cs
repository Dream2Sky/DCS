using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.dcs.web.Models;
using System.Xml;
using com.dcs.common;

namespace com.dcs.web.Globals
{
    public class RolesManager
    {
        private static string config = string.Empty;
        private static List<RolesModel> rolesList = null;
        private static List<KeyValuePair<string, int>> rolesType = null;
        public RolesManager(string configfile)
        {
            config = configfile;
            rolesList = LoadRolesList();
            rolesType = InitRolesTypeList(rolesList);
        }

        public static void LoadXmlConfig(string configile)
        {
            config = configile;
            rolesList = LoadRolesList();
            rolesType = InitRolesTypeList(rolesList);
        }

        private static List<RolesModel> LoadRolesList()
        {
            List<RolesModel> rolesList = new List<RolesModel>();

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(config);
            }
            catch (Exception)
            {
                return null;
            }

            XmlNodeList nodeList = doc.GetElementsByTagName("role");

            foreach (var item in nodeList)
            {
                XmlElement xe = (XmlElement)item;

                RolesModel rm = new RolesModel();
                rm.Code = int.Parse(xe.GetAttribute("value"));
                rm.Name = ((XmlElement)xe.FirstChild).GetAttribute("value");
                rm.Url = ((XmlElement)xe.LastChild).GetAttribute("value");
                rm.Content = ((XmlElement)xe.ChildNodes.Item(1)).GetAttribute("value");

                rolesList.Add(rm);
            }

            return rolesList;
        }

        /// <summary>
        /// 实例化角色类型列表
        /// </summary>
        /// <param name="rmList"></param>
        /// <returns></returns>
        private static List<KeyValuePair<string, int>> InitRolesTypeList(List<RolesModel> rmList)
        {
            List<KeyValuePair<string, int>> rolesTypeList = new List<KeyValuePair<string, int>>();
            if (rmList == null)
            {
                return null;
            }

            foreach (var item in rmList)
            {
                KeyValuePair<string, int> kp = new KeyValuePair<string, int>(item.Name,item.Code);
                rolesTypeList.Add(kp);
            }

            return rolesTypeList;
        }

        /// <summary>
        /// 获取 角色名为 key 的角色对相应的 code
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static int GetRolesCode(string key)
        {
            try
            {
                return rolesType.Where(n => n.Key == key).SingleOrDefault().Value;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);
                throw;
            }
        }

        public static RolesModel GetRoleInfo(int role)
        {
            return rolesList == null ? null : rolesList.Where(n => n.Code == role).SingleOrDefault();
        }

        public static List<RolesModel> GetRolesList()
        {
            return rolesList ?? null;
        }
    }
}