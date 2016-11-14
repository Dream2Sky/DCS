using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.dcs.web.Models;
using System.Xml;

namespace com.dcs.web.Globals
{
    public class RolesManager
    {
        private static string config = string.Empty;
        private static List<RolesModel> rolesList = null;
        public RolesManager(string configfile)
        {
            config = configfile;
            rolesList = LoadRolesList();
        }

        public static void LoadXmlConfig(string configile)
        {
            config = configile;

            rolesList = LoadRolesList();
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