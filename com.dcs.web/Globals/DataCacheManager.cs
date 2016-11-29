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
            System.Web.Caching.Cache objcache = HttpRuntime.Cache;
            return objcache[cachekey] as List<InformationModel>;
        }

        public static void SetDataCache(string cachekey, List<InformationModel> objObject)
        {
            if (objObject == null)
            {
                return;
            }
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cachekey, objObject);
        }
    }
}