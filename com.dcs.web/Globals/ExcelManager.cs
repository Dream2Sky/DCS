using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace com.dcs.web.Globals
{
    public class ExcelManager
    {
        private IWorkbook _workbook { get; set; }
        private string _filename { get; set; }
        private ISheet _currentSheet { get; set; }
        public ExcelManager(string filename)
        {
            _filename = AppDomain.CurrentDomain.BaseDirectory + "/Files/" + filename;
        }

        public IWorkbook Open(string filename)
        {
            
        }
    }
}