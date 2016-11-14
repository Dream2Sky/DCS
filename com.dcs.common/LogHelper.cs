using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.dcs.common
{
    public class LogHelper
    {
        public static void writeLog_error(string txt)
        {
            ILog log = LogManager.GetLogger("log4netlogger");
            log.Error(txt);
        }

        public static void writeLog_info(string txt)
        {
            ILog log = LogManager.GetLogger("log4netlogger");
            log.Info(txt);
        }
    }
}
