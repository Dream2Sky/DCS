using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            //var currentTime = DateTime.Now;
            //var mcode = GetHardDiskID();
            //var millsce = DateTime.Now.Millisecond;


            //Console.WriteLine(currentTime);
            //Console.WriteLine(mcode);

            Console.WriteLine((1111).ToString("0000"));
            Console.Read();
        }

        //取第一块硬盘编号
        public static string GetHardDiskID()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                String strHardDiskID = null;
                foreach (ManagementObject mo in searcher.Get())
                {
                    strHardDiskID = mo["SerialNumber"].ToString().Trim();
                    break;
                }
                return strHardDiskID;
            }
            catch
            {
                return "";
            }
        }//end
    }
}
