using com.dcs.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace com.dcs.web.Globals
{
    public class DataCacheManager
    {
        public static List<InformationModel> GetDataCache(string cachekey)
        {
            return System.Web.HttpContext.Current.Session[cachekey] as List<InformationModel>;
        }

        public static void SetDataCache(string cachekey, List<InformationModel> objObject)
        {
            HttpContext.Current.Session[cachekey] = objObject;
        }
    }
}