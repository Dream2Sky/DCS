using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace com.dcs.common
{
    public class ConfigManager
    {
        private static string config = string.Empty;
        private static string accountPrefix = string.Empty;
        private static string defaultPassword = string.Empty;
        private static int indexseed = 0;
        public static void LoadXmlConfig(string configfile)
        {
            config = configfile;
            InitAppConfig();
        }

        /// <summary>
        /// 初始化 配置  账号生成的前缀 和 初始密码
        /// </summary>
        public static void InitAppConfig()
        {
            if (config == string.Empty)
            {
                throw new Exception("can not find the init config file");
            }
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(config);
                XmlNodeList nodeList = doc.GetElementsByTagName("add");

                foreach (var item in nodeList)
                {
                    XmlElement xe = (XmlElement)item;
                    var key = xe.GetAttribute("key");
                    var value = xe.GetAttribute("value");

                    if (key == "prefix")
                    {
                        accountPrefix = value;
                    }
                    else if (key == "defaultPassword")
                    {
                        defaultPassword = value;
                    }
                    else if (key == "index")
                    {
                        indexseed = int.Parse(value);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
       
        /// <summary>
        /// 获取账号生成的前缀
        /// </summary>
        /// <returns></returns>
        public static string GetAccountPrefix()
        {
            return accountPrefix;
        }

        /// <summary>
        /// 获取初始密码
        /// </summary>
        /// <returns></returns>
        public static string GetDefaultPassword()
        {
            return defaultPassword;
        }

        /// <summary>
        /// 获得账号生成的流水工号
        /// 并把最新的工号更新到配置文件中
        /// </summary>
        /// <returns></returns>
        public static string GetIndexSeed()
        {
            if (config == string.Empty)
            {
                throw new Exception("can not find the init config file,failed to get the index seed");
            }

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(config);
                var nodeList = doc.GetElementsByTagName("add");

                foreach (var item in nodeList)
                {
                    var key = ((XmlElement)item).GetAttribute("key");
                    var value = ((XmlElement)item).GetAttribute("value");
                    if (key == "index")
                    {
                        ((XmlElement)item).SetAttribute("value", (++indexseed).ToString());
                        break;
                    }
                }

                // 要記得保存。。。
                doc.Save(config);

                return indexseed.ToString("0000");
            }
            catch (Exception)
            { 
                throw;
            }
        }
    }
}
