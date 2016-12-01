using com.dcs.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace com.dcs.web.Globals
{
    public class FileManager
    {
        public static List<string> SaveFile(HttpPostedFileBase[] fileCollection, string filePath)
        {
            List<string> fileList = new List<string>();
            #region 保存文件
            try
            {
                foreach (HttpPostedFileBase file in fileCollection)
                {
                    //fileList.Add(file.FileName);

                    var filename = System.IO.Path.GetFileName(file.FileName);
                    var temp = filename.Split('.');
                    filename = TimeManager.GetTimeSpan() + "." + temp[temp.Length - 1];

                    string path = System.IO.Path.Combine(filePath, filename);

                    fileList.Add(filename);
                    file.SaveAs(path);
                }

                return fileList;
            }
            catch (Exception ex)
            {
                LogHelper.writeLog_error(ex.Message);
                LogHelper.writeLog_error(ex.StackTrace);
                return null;
            }
            #endregion
        }
    }
}